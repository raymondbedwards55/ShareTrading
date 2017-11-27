using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devart.Data.PostgreSql;

namespace ShareTrading
{
  public partial class ExportBuysSellsToMYOB : Form
  {
    public ExportBuysSellsToMYOB()
    {
      InitializeComponent();
    }
    private void ExportBuysSellsToMYOB_Load(object sender, EventArgs e)
    {
      // get the start of this quarter & then start & end of last quarter for defaults
      int startMonth = (DateTime.Now.Month - 1) / 3;
      startMonth *= 3;
      startMonth++;
      DateTime thisQtrStart = new DateTime(DateTime.Now.Year, startMonth, 1);
      DateTime lastQtrStart = thisQtrStart.AddMonths(-3);
      DateTime lastQtrEnd = thisQtrStart.AddDays(-1);
      dtpFrom.Value = lastQtrStart;
      dtpTo.Value = lastQtrEnd;
    }
    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
      if (DateTime.Compare(dtpFrom.Value, dtpTo.Value) > 0)
        MessageBox.Show("Please ensure that Date From is earlier than Date To", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
      if (DateTime.Compare(dtpFrom.Value, dtpTo.Value) > 0)
        MessageBox.Show("Please ensure that Date From is earlier than Date To", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    private void rbBuySell_CheckedChanged(object sender, EventArgs e)
    {

    }


    private void toolStripButton_Close_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void toolStripButton_Export_Click(object sender, EventArgs e)
    {
      if (DateTime.Compare(dtpFrom.Value, dtpTo.Value) > 0)
      {
        MessageBox.Show("Please ensure that Date From is earlier than Date To", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      if (rbDividend.Checked)
        exportDividends();
      else
        exportFile(rbBuy.Checked);
      MessageBox.Show("File export completed");
    }

    private void exportDividends()
    {
      string lit = "Dividends";
      string filename = getfilename("MYOB Export", string.Format("MYOBExport{2}_{0}_{1}", dtpFrom.Value.ToString("yyMMdd"), dtpTo.Value.ToString("yyMMdd"), lit), lit);
      if (string.IsNullOrEmpty(filename))
      {
        MessageBox.Show("Unable to save to file selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {
        using (System.IO.StreamWriter sr = new System.IO.StreamWriter(filename))
        {
          string outputLine;
          // write header line
          outputLine = string.Join(",", System.Enum.GetNames(typeof(receiveMoney)));
          sr.WriteLine(outputLine);
          int counter = 0;

          // get all dividends with ex dividend date in date range selected
          List<DBAccess.DividendHistory> divHistoryList = new List<DBAccess.DividendHistory>();
          List<PgSqlParameter> divHistoryParams = new List<PgSqlParameter>();
          divHistoryParams.Add(new PgSqlParameter("@P1", dtpFrom.Value));
          divHistoryParams.Add(new PgSqlParameter("@P2", dtpTo.Value));
          if (!DBAccess.GetDividends(divHistoryParams, out divHistoryList, " AND dvh_exdivdate BETWEEN @P1 AND @P2 ", " ORDER BY dvh_asxcode "))
          {
            MessageBox.Show("Unable to find any dividends in Date Range selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
          }
          // foreach dividend, are there any Buys for this company before / equal the ex dividend date
          foreach (DBAccess.DividendHistory divHistoryRec in divHistoryList)
          {
            if (divHistoryRec.ASXCode != "WPL")
            {
              continue;
            }
            int SOHonDividendDate = 0;
            // foreach Buy before the ex-Dividend date, add SOH to totalremainingStock (may be zero)
            List<DBAccess.TransRecords> buyList = null;
            List<PgSqlParameter> paramList = new List<PgSqlParameter>();
            paramList.Add(new PgSqlParameter("@P1", "Buy"));
            paramList.Add(new PgSqlParameter("@P2", divHistoryRec.ExDividend));
            paramList.Add(new PgSqlParameter("@P3", divHistoryRec.ASXCode));
            if (DBAccess.GetTransRecords(paramList, out buyList, DBAccess.TransRecordsFieldList, " AND trn_buysell = @P1 AND trn_transdate < @P2 AND trn_asxcode = @P3  "  /* where */, " ORDER BY trn_transdate, trn_asxcode " /* order by */, false /* simulation */))
            {
              foreach (DBAccess.TransRecords rec in buyList)
              {
                SOHonDividendDate += rec.SOH;
                //  get related Sell records and if Sell if after the ex Dividend Date, then add SOH to totalRemainingStock
                List<DBAccess.RelatedBuySellTrans> relatedList = new List<DBAccess.RelatedBuySellTrans>();
                if (DBAccess.GetAllRelated(rec.ID, 0, out relatedList))
                {
                  foreach (DBAccess.RelatedBuySellTrans relRec in relatedList)
                  {
                    // get Sell record
                    List<PgSqlParameter> sellParams = new List<PgSqlParameter>();
                    sellParams.Add(new PgSqlParameter("@P1", relRec.SellId));
                    sellParams.Add(new PgSqlParameter("@P2", divHistoryRec.BooksClose));
                    List<DBAccess.TransRecords> sellList = new List<DBAccess.TransRecords>();
                    if (DBAccess.GetTransRecords(sellParams, out sellList, DBAccess.TransRecordsFieldList, " AND trn_id = @P1 AND trn_transdate > @P2 ", string.Empty, false))
                    {
                      foreach (DBAccess.TransRecords sellrec in sellList)
                        SOHonDividendDate += relRec.TransQty;
                    }
                  }
                }
                }
              }
              if (SOHonDividendDate == 0)
                continue;
              string transId = string.Format("{0}{1}", DateTime.Today.ToString("yyMMdd"), counter.ToString().PadLeft(2, '0'));
              var values = System.Enum.GetValues(typeof(dividendLineTypes));
              foreach (int v in values)
              {

                outputLine = formatDividendReceiveMoney(divHistoryRec, v, ((int[])values)[0], transId, SOHonDividendDate);
                sr.WriteLine(outputLine);
              }
              sr.WriteLine(String.Empty);
              counter += 1;
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void exportFile(bool buy)
    {
      string lit = buy ? "Buy" : "Sell";
      string filename = getfilename("MYOB Export", string.Format("MYOBExport{2}_{0}_{1}", dtpFrom.Value.ToString("yyMMdd"), dtpTo.Value.ToString("yyMMdd"), lit), lit);
      if (string.IsNullOrEmpty(filename))
      {
        MessageBox.Show("Unable to save to file selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {
        using (System.IO.StreamWriter sr = new System.IO.StreamWriter(filename))
        {
          string outputLine;
          // get all buy transactions in date range selected
          List<DBAccess.TransRecords> transList = null;
          List<PgSqlParameter> paramList = new List<PgSqlParameter>();
          paramList.Add(new PgSqlParameter("@P1", lit));
          paramList.Add(new PgSqlParameter("@P2", dtpFrom.Value));
          paramList.Add(new PgSqlParameter("@P3", dtpTo.Value));
          if (DBAccess.GetTransRecords(paramList, out transList, DBAccess.TransRecordsFieldList, " AND trn_buysell = @P1 AND trn_transdate BETWEEN @P2 AND @P3 "  /* where */, " ORDER BY trn_transdate, trn_asxcode " /* order by */, false /* simulation */))
          {
            // write header line
            outputLine = string.Join(",", buy ? System.Enum.GetNames(typeof(spendMoney)) : System.Enum.GetNames(typeof(receiveMoney)));
            sr.WriteLine(outputLine);
            int counter = 0;
            foreach (DBAccess.TransRecords rec in transList)
            {
              counter += 1;
              string transId = string.Format("{0}{1}", DateTime.Today.ToString("yyMMdd"), counter.ToString().PadLeft(2, '0'));
              var values = System.Enum.GetValues(buy ? typeof(buySharesLineTypes) : typeof(sellSharesLineTypes));
              foreach (int v in values)
              {

                outputLine = buy ? formatSpend(rec, v, ((int[])values)[0]) : formatReceive(rec, v, ((int[])values)[0], transId);
                sr.WriteLine(outputLine);
              }
              sr.WriteLine(String.Empty);
            }
          }
          else
          {
            MessageBox.Show("No records found", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
    private string formatSpend(DBAccess.TransRecords rec, int v, int acct)
    {
      string line = string.Empty;
      string chqAct = DBAccess.getGLCode(acct);
        switch (v)
        {
    //            return @"ChequeAccount,ChequeNmbr,Date,Inclusive,LastName,,,,,,Memo,AllocAccount,ExTaxAmt,IncTaxAmt,,TaxCode,0,TaxAmt,0,N,,,,AllocMemo,,,";

          case 1:     // Cheque Account Line
            string sp = spendMoneyDefaults;
            sp = sp.Replace("ChequeAccount",chqAct);
            sp = sp.Replace("ChequeNmbr",string.Empty);
            sp = sp.Replace("Date", rec.TranDate.ToString("dd-MM-yyyy"));
            sp = sp.Replace("LastName", "SHARES");
            sp = sp.Replace("AllocAccount", chqAct);
            decimal incTaxAmt = (Decimal.Round(rec.UnitPrice * rec.TransQty, 2) + rec.BrokerageInc);
            sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
            sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
            sp = sp.Replace("TaxAmt", "0.00");
            sp = sp.Replace("AllocMemo", string.Empty);
            sp = sp.Replace("Memo", string.Format("BUY {0} {1}", rec.TransQty.ToString("#"), rec.ASXCode));
            sp = sp.Replace("TaxCode", string.Empty);
          line = sp;
            break;
          case 2:   // Asset
          sp = spendMoneyDefaults;
          sp = sp.Replace("ChequeAccount", chqAct);
          sp = sp.Replace("ChequeNmbr", string.Empty);
          sp = sp.Replace("Date", rec.TranDate.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", DBAccess.getGLCode(v));
          incTaxAmt = Decimal.Round(rec.UnitPrice * rec.TransQty, 2) - rec.TradeProfit;
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("TaxAmt", "0.00");
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("BUY {0} {1}", rec.TransQty.ToString("#"), rec.ASXCode));
          sp = sp.Replace("TaxCode", DBAccess.getTaxCode(v));
          line = sp;
          break;
          case 4:
          sp = spendMoneyDefaults;
          sp = sp.Replace("ChequeAccount", chqAct);
          sp = sp.Replace("ChequeNmbr", string.Empty);
          sp = sp.Replace("Date", rec.TranDate.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", DBAccess.getGLCode(v));
          incTaxAmt = rec.BrokerageInc;
          decimal taxAmt = Decimal.Round(incTaxAmt / 11, 2);
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", (incTaxAmt - taxAmt).ToString("#.##"));
          sp = sp.Replace("TaxAmt", taxAmt.ToString("#.##"));
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("BUY {0} {1}", rec.TransQty.ToString("#"), rec.ASXCode));
          sp = sp.Replace("TaxCode", DBAccess.getTaxCode(v));
          line = sp;
          break;
          default:
            break;
        }
      
      
      return line;
    }



    private string formatReceive(DBAccess.TransRecords rec, int v, int acct, string id)
    {
      string line = string.Empty;
      string chqAct = DBAccess.getGLCode(acct);
      switch (v)
      {
        //                    return @"DepositAccount,IDNmbr,Date,LastName,,Memo,Y,AllocAccount,ExTaxAmt,IncTaxAmt,,TaxCode,0,TaxAmt,0,,,,,,,,,,,,,,AllocMemo,,,";


        case 1:     // Cheque Account Line
          string sp = receiveMoneyDefaults;
          sp = sp.Replace("DepositAccount", chqAct);
          sp = sp.Replace("IDNmbr", id);
          sp = sp.Replace("Date", rec.TranDate.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", chqAct);
          decimal incTaxAmt = (Decimal.Round(rec.UnitPrice * rec.TransQty, 2) - rec.BrokerageInc);
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("TaxAmt", "0.00");
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("SELL {0} {1}", rec.TransQty.ToString("#"), rec.ASXCode));
          sp = sp.Replace("TaxCode", string.Empty);
          line = sp;
          break;
        case 2:   // Asset
          sp = receiveMoneyDefaults;
          sp = sp.Replace("DepositAccount", chqAct);
          sp = sp.Replace("IDNmbr", id);
          sp = sp.Replace("Date", rec.TranDate.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", DBAccess.getGLCode(v));
          incTaxAmt = Decimal.Round((rec.UnitPrice * rec.TransQty) - rec.TradeProfit, 2 );
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("TaxAmt", "0.00");
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("SELL {0} {1}", rec.TransQty.ToString("#"), rec.ASXCode));
          sp = sp.Replace("TaxCode", DBAccess.getTaxCode(v));
          line = sp;
          break;
        case 3:
          sp = receiveMoneyDefaults;
          sp = sp.Replace("DepositAccount", chqAct);
          sp = sp.Replace("IDNmbr", id);
          sp = sp.Replace("Date", rec.TranDate.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", DBAccess.getGLCode(v));
          incTaxAmt = rec.TradeProfit;
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("TaxAmt", "0.00");
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("SELL {0} {1}", rec.TransQty.ToString("#"), rec.ASXCode));
          sp = sp.Replace("TaxCode", DBAccess.getTaxCode(v));
          line = sp;
          break;
        case 4:
          sp = receiveMoneyDefaults;
          sp = sp.Replace("DepositAccount", chqAct);
          sp = sp.Replace("IDNmbr", id);
          sp = sp.Replace("Date", rec.TranDate.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", DBAccess.getGLCode(v));
          incTaxAmt = -rec.BrokerageInc;
          decimal taxAmt = Decimal.Round(incTaxAmt / 11, 2);
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", (incTaxAmt - taxAmt).ToString("#.##"));
          sp = sp.Replace("TaxAmt", taxAmt.ToString("#.##"));
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("SELL {0} {1}", rec.TransQty.ToString("#"), rec.ASXCode));
          sp = sp.Replace("TaxCode", DBAccess.getTaxCode(v));
          line = sp;
          break;
        default:
          break;
      }


      return line;
    }

    private string formatDividendReceiveMoney(DBAccess.DividendHistory rec, int v, int acct, string id, decimal ttlSOH)
    {
      string line = string.Empty;
      string chqAct = DBAccess.getGLCode(acct);
      switch (v)
      {
        //                    return @"DepositAccount,IDNmbr,Date,LastName,,Memo,Y,AllocAccount,ExTaxAmt,IncTaxAmt,,TaxCode,0,TaxAmt,0,,,,,,,,,,,,,,AllocMemo,,,";


        case 1:     // Cheque Account Line - Nett Dividend
          string sp = receiveMoneyDefaults;
          sp = sp.Replace("DepositAccount", chqAct);
          sp = sp.Replace("IDNmbr", id);
          sp = sp.Replace("Date", rec.DatePayable.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", chqAct);
          decimal incTaxAmt = (Decimal.Round(rec.Amount * ttlSOH, 2));
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("TaxAmt", "0.00");
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("DIVDEND {1} {0} Shares @ {2}", ttlSOH.ToString("#"), rec.ASXCode, rec.Amount));
          sp = sp.Replace("TaxCode", string.Empty);
          line = sp;
          break;
        case 5:   // Gross Dividend
          sp = receiveMoneyDefaults;
          sp = sp.Replace("DepositAccount", chqAct);
          sp = sp.Replace("IDNmbr", id);
          sp = sp.Replace("Date", rec.DatePayable.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", DBAccess.getGLCode(v));
          incTaxAmt = Decimal.Round((rec.GrossDividend * ttlSOH), 2);
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("TaxAmt", "0.00");
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("DIVDEND {1} {0} Shares @ {2}", ttlSOH.ToString("#"), rec.ASXCode, rec.Amount));
          sp = sp.Replace("TaxCode", DBAccess.getTaxCode(v));
          line = sp;
          break;
        case 6: // Franking Credit
          sp = receiveMoneyDefaults;
          sp = sp.Replace("DepositAccount", chqAct);
          sp = sp.Replace("IDNmbr", id);
          sp = sp.Replace("Date", rec.DatePayable.ToString("dd-MM-yyyy"));
          sp = sp.Replace("LastName", "SHARES");
          sp = sp.Replace("AllocAccount", DBAccess.getGLCode(v));
          incTaxAmt = Decimal.Round(rec.FrankingCredit * ttlSOH, 2);
          sp = sp.Replace("IncTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("ExTaxAmt", incTaxAmt.ToString("#.##"));
          sp = sp.Replace("TaxAmt", "0.00");
          sp = sp.Replace("AllocMemo", string.Empty);
          sp = sp.Replace("Memo", string.Format("DIVDEND {1} {0} Shares @ {2}", ttlSOH.ToString("#"), rec.ASXCode, rec.Amount));
          sp = sp.Replace("TaxCode", DBAccess.getTaxCode(v));
          line = sp;
          break;
        default:
          break;
      }


      return line;
    }
    private string getfilename(string fileType, string dtRng, string lit)
    {
      saveFileDialog1.Title = "Export to TXT File";
      saveFileDialog1.Filter = string.Format("{0} (*.txt)|*.txt|Any File (*.*)|*.*", fileType);
      saveFileDialog1.InitialDirectory = @"c:\Users\Ray\Downloads";
      saveFileDialog1.FileName = string.Format("{1}Export_{0}", dtRng, lit);
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        return saveFileDialog1.FileName;
      return string.Empty;
    }

    public enum sellSharesLineTypes
    {
      BankAccountLine = 1,
      ShareAssetLine = 2,
      TradingProfitLine = 3,
      BrokerageLine = 4
    }

    public enum buySharesLineTypes
    {
      BankAccountLine = 1,
      ShareAssetLine = 2,
      BrokerageLine = 4
    }

    public enum dividendLineTypes
    {
      BankAccountLine = 1,
      GrossDividendLine = 5,
      FrankingCreditLine = 6
    }
    private string spendMoneyDefaults
    {
      get
      {
        return @"ChequeAccount,ChequeNmbr,Date,Y,LastName,,,,,,Memo,AllocAccount,ExTaxAmt,IncTaxAmt,,TaxCode,0,TaxAmt,0,N,,,,AllocMemo,,,";
      }
    }
    private enum spendMoney
    {
      [Description("Cheque Account")]
      ChqAct = 1,
      [Description("Cheque#")]
      ChqNmbr = 2,
      [Description("Date")]
      Date = 3,
      [Description("Inclusive")]
      Inclusive = 4,
      [Description("Co/LastName")]
      LastName = 5,
      [Description("First Name")]
      FirstName = 6,
      [Description("AddrLine1")]
      Addr1 = 7,
      [Description("AddrLine2")]
      Addr2 = 8,
      [Description("AddrLine3")]
      Addr3 = 9,
      [Description("AddrLine4")]
      Addr4 = 10,
      [Description("Memo")]
      Memo = 11,
      [Description("Allocation Account#")]
      AllocActNmbr = 12,
      [Description("Ex-Tax Amount")]
      ExTaxAmt = 13,
      [Description("Inc-Tax Amount")]
      IncTaxAmt = 14,
      [Description("Job #")]
      JobNmbr = 15,
      [Description("Tax Code")]
      TaxCode = 16,
      [Description("Non GST/LCT Amount")]
      NonGSTLCTAmt = 17,
      [Description("Tax Amount")]
      TaxAmt = 18,
      [Description("Import Duty Amount")]
      ImportDutyAmt = 19,
      [Description("Printed")]
      Printed = 20,
      [Description("Currency Code")]
      CurrencyCode = 21,
      [Description("Exchange Rate")]
      ExchangeRate = 22,
      [Description("StatementText")]
      StatementText = 23,
      [Description("Allocation Memo")]
      AllocationMemo = 24,
      [Description("Category")]
      Category = 25,
      [Description("Card Id")]
      CardId = 26,
      [Description("Record Id")]
      RecordId = 27,

    }

    private string receiveMoneyDefaults
    {
      get
      {
        return @"DepositAccount,IDNmbr,Date,LastName,,Memo,Y,AllocAccount,ExTaxAmt,IncTaxAmt,,TaxCode,0,TaxAmt,0,,,,,,,,,,,,,AllocMemo,,,";
      }
    }

    private enum receiveMoney
    {
      [Description("Deposit Account")]
      ChqAct = 1,
      [Description("ID #")]
      IDNmbr = 2,
      [Description("Date")]
      Date = 3,
      [Description("Co/LastName")]
      LastName = 4,
      [Description("First Name")]
      FirstName = 5,
      [Description("Memo")]
      Memo = 6,
      [Description("Inclusive")]
      Inclusive = 7,
      [Description("Allocation Account#")]
      AllocActNmbr = 8,
      [Description("Ex-Tax Amount")]
      ExTaxAmt = 9,
      [Description("Inc-Tax Amount")]
      IncTaxAmt = 10,
      [Description("Job #")]
      JobNmbr = 11,
      [Description("Tax Code")]
      TaxCode = 12,
      [Description("Non GST/LCT Amount")]
      NonGSTLCTAmt = 13,
      [Description("Tax Amount")]
      TaxAmt = 14,
      [Description("Import Duty Amount")]
      ImportDutyAmt = 15,
      [Description("Currency Code")]
      CurrencyCode = 16,
      [Description("Exchange Rate")]
      ExchangeRate = 17,
      [Description("Payment Method")]
      PaymentMethod = 18,
      [Description("Drawer/Account Name")]
      AccountName = 19,
      [Description("BSB")]
      BSB = 20,
      [Description("Account Number")]
      AcctNmbr = 21,
      [Description("Cheque #")]
      ChqNmbr = 22,
      [Description("Card Number")]
      CardNmbr = 23,
      [Description("Name on Card")]
      CardName = 24,
      [Description("Expiry Date")]
      ExpiryDate = 25,
      [Description("Authorisation Code")]
      AuthorizationCode = 26,
      [Description("Notes")]
      Notes = 27,
      [Description("Allocation Memo")]
      AllocationMemo = 28,
      [Description("Category")]
      Category = 29,
      [Description("Card Id")]
      CardId = 30,
      [Description("Record Id")]
      RecordId = 31,

    }


  }
}
