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
  public partial class FrmSuggestions : Form
  {
    public FrmSuggestions()
    {
      InitializeComponent();
    }

    private void FrmSuggestions_Load(object sender, EventArgs e)
    {
      populateSellGrid();
      populateBuyGrid();
      populateReBuyGrid();
      
    }

    private Boolean dividendPending(string ASXCode, int days1, int days2, out DBAccess.DividendHistory divHist)
    {
      divHist = new DBAccess.DividendHistory();
      List<DBAccess.DividendHistory> divList = new List<DBAccess.DividendHistory>();
      string extraWhere = string.Empty;
      string orderBy = string.Empty;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", ASXCode));
      extraWhere += " AND dvh_asxcode = @P1 ";
      paramList.Add(new PgSqlParameter("@P2", DateTime.Today.AddDays(days1)));
      paramList.Add(new PgSqlParameter("@P3", DateTime.Today.AddDays(days2)));
      extraWhere += " AND dvh_exdivdate BETWEEN @P2 AND @P3 ";
      orderBy += " ORDER BY dvh_exdivdate DESC ";
      if (DBAccess.GetDividends(paramList, out divList, extraWhere, orderBy))
      {
        divHist = divList[0];
        return true;
      }
      return false;

    }
    private bool calculateDividendTotal(string ASXCode, DateTime buyDate, out decimal totalDividends, out decimal totalFrCredits)
    {
      totalDividends = 0M;
      totalFrCredits = 0M;
      List<DBAccess.DividendHistory> divList = new List<DBAccess.DividendHistory>();
      string extraWhere = string.Empty;
      string orderBy = string.Empty;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", ASXCode));
      extraWhere += " AND dvh_asxcode = @P1 ";
      paramList.Add(new PgSqlParameter("@P2", buyDate.Date));
      paramList.Add(new PgSqlParameter("@P3", DateTime.Today));
      extraWhere += " AND dvh_exdivdate BETWEEN @P2 AND @P3 ";
      orderBy += " ORDER BY dvh_exdivdate DESC ";
      if (DBAccess.GetDividends(paramList, out divList, extraWhere, orderBy))
      {
        totalDividends = divList.Select(x => x.GrossDividend).Sum();
        totalFrCredits = divList.Select(x => x.FrankingCredit).Sum();
      }

        return true;
    }
    private void populateSellGrid()
    {
      List<SellSuggestions> suggestList = new List<SellSuggestions>();
      // Get all the shares we have stock of
      List<DBAccess.TransRecords> transList = null;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();

      if (!DBAccess.GetTransRecords(null, out transList, DBAccess.TransRecordsFieldList, " AND trn_soh > 0 ", " ORDER BY trn_ASXCode, trn_transdate DESC", false))
      {
        MessageBox.Show("No transactions found", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      var uCode = transList.Select(x => x.ASXCode).Distinct().ToList();
      foreach (string code in uCode)
      {
        if (code == "TLS")
        { }
        List<DBAccess.TransRecords> codeList = transList.FindAll(delegate (DBAccess.TransRecords r1) { return r1.ASXCode == code; });
        // For each ASXCode, get todays price and work out the percentages
        List<DBAccess.ASXPriceDate> todaysPrice = null;
        DateTime dt = DateTime.MinValue;
        Decimal prc = 0M;
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", code));
        if (DBAccess.GetPriceRecords(paramList, out todaysPrice, DBAccess.ASXPriceDateFieldList, " AND apd_asxcode = @P1 ", "ORDER BY apd_pricedate DESC ", false))
        {
          // no prices for this ASX Code so we'll pretend
          
        }
        if (todaysPrice != null && todaysPrice.Count > 0)
        {
          prc = todaysPrice[0].PrcClose;
          dt = todaysPrice[0].PriceDate;
        }
        // create records to display
        foreach (DBAccess.TransRecords transRec in codeList)
        {
          int daysHeld = (DateTime.Today - transRec.TranDate.Date).Days;
          if (daysHeld < 3)
            daysHeld = 3;         // takes 3 days for the transaction to be finalised
          SellSuggestions sug = new SellSuggestions();
          sug.ASXCode = transRec.ASXCode;
          sug.SOH = transRec.SOH;
          sug.UnitBuyPrice = transRec.UnitPrice;
          sug.TodaysUnitPrice = prc;
          sug.PctGain = 0M;
          sug.PctYear = 0M;
          sug.DaysHeld = daysHeld;
          sug.BuyDate = transRec.TranDate;
          sug.HighlightDiv = false;
          if (prc != 0 && sug.UnitBuyPrice != 0)
          {
            sug.PctGain = Decimal.Round((sug.TodaysUnitPrice - transRec.UnitPrice) * 100 / transRec.UnitPrice, 2);
            sug.PctYear = Decimal.Round((sug.TodaysUnitPrice - transRec.UnitPrice) / transRec.UnitPrice * 365 * 100 / daysHeld, 2);
          }

          //  Is there a dividend pending - already announced (ie. with ex dividend date +/- 10 days from today)
          DBAccess.DividendHistory divHist = new DBAccess.DividendHistory();
          if (dividendPending(sug.ASXCode, -30, 30, out divHist))
          {
            sug.LastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.TodaysUnitPrice * 100, 2);
            sug.LastDivDate = divHist.ExDividend;
            sug.DividendForecast = false;
            sug.HighlightDiv = true;
          }
          else
          {
            // or issued +/- 12 months ago
            if (dividendPending(sug.ASXCode, -374, -354, out divHist))
            {
              sug.LastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.TodaysUnitPrice * 100,2);
              sug.LastDivDate = divHist.ExDividend.AddYears(1);
              sug.DividendForecast = true;
              sug.HighlightDiv = true;
            }
            else
            {
              if (dividendPending(sug.ASXCode, -10000, 0, out divHist))
              {
                sug.LastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.TodaysUnitPrice * 100, 2);
                sug.LastDivDate = divHist.ExDividend; //.AddYears(1);
                sug.DividendForecast = false;
                sug.HighlightDiv = false;

              }
            }
          }
          // Get the sum of Dividends & sum of Franking Credits between buy date & now
          decimal totalDividends = 0M;
          decimal totalFrCredits = 0M;
          if (calculateDividendTotal(sug.ASXCode, sug.BuyDate, out totalDividends, out totalFrCredits))
          {
            sug.PctROI = Decimal.Round(((sug.TodaysUnitPrice - transRec.UnitPrice) + totalDividends) / transRec.UnitPrice * 100, 2);
            sug.PctYearROI = Decimal.Round(((sug.TodaysUnitPrice - transRec.UnitPrice) + totalDividends) / transRec.UnitPrice * 100 * 365 / daysHeld, 2);
          }
          suggestList.Add(sug);
        }

      }
      suggestList = suggestList.OrderByDescending(x => x.PctGain).ToList();
      // 
      dgvToSell.DataSource = null;
      ToSellBindingSource.DataSource = suggestList;
      dgvToSell.DataSource = ToSellBindingSource;
      dgvToSell.Refresh();

    }

    public class SellSuggestions
    {
      public string ASXCode { get; set; }
      public decimal LastDividendAmount { get; set; }
      public DateTime LastDivDate { get; set; }
      public Boolean DividendForecast { get; set; }
      public int SOH { get; set; }
      public decimal UnitBuyPrice { get; set; }
      public DateTime BuyDate { get; set; }
      public decimal DaysHeld { get; set; }
      public decimal TodaysUnitPrice { get; set; }
      public decimal PctGain { get; set; }
      public decimal PctYear { get; set; }
      public decimal PctROI { get; set; }
      public decimal PctYearROI{ get; set; }
      public bool HighlightDiv { get; set; }
    }

    private void toolStripButtonClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void toolStripButtonUpdate_Click(object sender, EventArgs e)
    {
      populateSellGrid();
      populateBuyGrid();
      populateReBuyGrid();
    }

    private int _previousIndex;
    private bool _sortDirection;

    private void dgvToSell_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == _previousIndex)
        _sortDirection ^= true; // toggle direction

      dgvToSell.DataSource = SortData(
          (List<SellSuggestions>)ToSellBindingSource.DataSource, dgvToSell.Columns[e.ColumnIndex].Name, _sortDirection);

      _previousIndex = e.ColumnIndex;
    }

    public List<SellSuggestions> SortData(List<SellSuggestions> list, string column, bool ascending)
    {
      try
      {
        return ascending ?
           /* RefreshId( */ list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /* ) */ :
           /* RefreshId( */ list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /*) */;
      }
      catch
      { }
      return list;
    }

    private void dgvToSell_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridView dgv = sender as DataGridView;
      if (dgv.Columns[e.ColumnIndex].ValueType == typeof(decimal))
        if ((decimal)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value < 0M)
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
      if ((dgv.Columns[e.ColumnIndex].Name == "LastDividendAmount" || dgv.Columns[e.ColumnIndex].Name == "LastDivDate") && (bool)dgv.Rows[e.RowIndex].Cells["HighlightDiv"].Value)
      {
        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
      }

    }

    private void populateBuyGrid()
    {
      List<BuySuggestions> suggestList = new List<BuySuggestions>();
      // Get all the shares we have stock of
      List<string> uCode = new List<string>();
      List<DBAccess.TransRecords> transList = null;
      List<DBAccess.CompanyDetails> allCompaniesList = null;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      if (chbAll.Checked)
      {
        if (!DBAccess.GetCompanyDetails(null, out allCompaniesList, false, false))
        { 
          MessageBox.Show("No companies found", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }
        uCode = allCompaniesList.Select(x => x.ASXCode).Distinct().ToList();

      }
      if (!DBAccess.GetTransRecords(null, out transList, DBAccess.TransRecordsFieldList, " AND trn_soh  = 0 ", " ORDER BY trn_ASXCode, trn_transdate DESC", false))
      {
        MessageBox.Show("No transactions found", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      if (allCompaniesList == null)
        uCode = transList.Select(x => x.ASXCode).Distinct().ToList();
      foreach (string code in uCode)
      {
        // For each ASXCode, get todays price and work out the percentages
        List<DBAccess.ASXPriceDate> todaysPrice = null;
        DateTime dt = DateTime.MinValue;
        Decimal prc = 0M;
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", code));
        if (DBAccess.GetPriceRecords(paramList, out todaysPrice, DBAccess.ASXPriceDateFieldList, " AND apd_asxcode = @P1 ", "ORDER BY apd_pricedate DESC ", false))
        {
          // no prices for this ASX Code so we'll pretend

        }
        if (todaysPrice != null && todaysPrice.Count > 0)
        {
          prc = todaysPrice[0].PrcClose;
          dt = todaysPrice[0].PriceDate;
        }

        List<DBAccess.TransRecords> codeList = transList.FindAll(delegate (DBAccess.TransRecords r1) { return r1.ASXCode == code; });
        BuySuggestions sug = new BuySuggestions();

        if (codeList == null || codeList.Count <= 0)
        {
          sug.BuyASXCode = code;
          sug.BuyTodaysUnitPrice = prc;
        }
        else
        {

          codeList = codeList.OrderByDescending(x => x.TranDate).ToList();
          // Check if stock has been bought again since this sell
          paramList = new List<PgSqlParameter>();
          paramList.Add(new PgSqlParameter("@P1", codeList[0].ASXCode));
          paramList.Add(new PgSqlParameter("@P2", codeList[0].TranDate));
          List<DBAccess.TransRecords> buyList = new List<DBAccess.TransRecords>();
          if (DBAccess.GetTransRecords(paramList, out buyList, DBAccess.TransRecordsFieldList, " AND trn_asxcode = @P1 AND trn_transdate >= @P2 AND trn_soh > 0 AND trn_buysell = 'Buy' ", string.Empty, false))
            continue;
          // create records to display
          DBAccess.TransRecords transRec = codeList[0];
          decimal daysHeld = (DateTime.Today - transRec.TranDate.Date).Days;
          if (daysHeld < 3)
            daysHeld = 3;         // takes 3 days for the transaction to be finalised
          sug.BuyASXCode = transRec.ASXCode;
          sug.BuySOH = transRec.TransQty;
          sug.UnitSellPrice = transRec.UnitPrice;
          sug.BuyTodaysUnitPrice = prc;
          sug.BuyPctGain = 0M;
          sug.BuyPctYear = 0M;
          sug.BuyDaysHeld = daysHeld;
          sug.SellDate = transRec.TranDate;
          if (prc != 0 && sug.UnitSellPrice != 0)
          {
            sug.BuyPctGain = Decimal.Round((sug.BuyTodaysUnitPrice - sug.UnitSellPrice) * 100 / sug.UnitSellPrice, 2);
            sug.BuyPctYear = Decimal.Round((sug.BuyTodaysUnitPrice - sug.UnitSellPrice) / sug.UnitSellPrice * 365 * 100 / daysHeld, 2);
          }
        }

        //  Is there a dividend pending - already announced (ie. with ex dividend date +/- 10 days from today)
        DBAccess.DividendHistory divHist = new DBAccess.DividendHistory();
        sug.BuyHighlightDiv = false;
        if (dividendPending(sug.BuyASXCode, -30, 30, out divHist))
        {
          sug.BuyLastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.BuyTodaysUnitPrice * 100, 2);
          sug.BuyLastDivDate = divHist.ExDividend;
          sug.BuyHighlightDiv = true;
        }
        else
        {
          // or issued +/- 12 months ago
          if (dividendPending(sug.BuyASXCode, -374, -354, out divHist))
          {
            sug.BuyLastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.BuyTodaysUnitPrice * 100, 2);
            sug.BuyLastDivDate = divHist.ExDividend;
            sug.BuyHighlightDiv = true;
          }
          else
          {
            if (dividendPending(sug.BuyASXCode, -10000, 0, out divHist))
            {
              sug.BuyLastDividendAmount = sug.BuyTodaysUnitPrice == 0 ? 0 : Decimal.Round(divHist.GrossDividend / sug.BuyTodaysUnitPrice * 100, 2);
              sug.BuyLastDivDate = divHist.ExDividend;
              sug.BuyHighlightDiv = false;
            }
          }
        }

        suggestList.Add(sug);

      }
      suggestList = suggestList.OrderBy(x => x.BuyPctGain).ToList();
      // 
      dgvToBuy.DataSource = null;
      ToBuyBindingSource.DataSource = suggestList;
      dgvToBuy.DataSource = ToBuyBindingSource;
      dgvToBuy.Refresh();

    }

    public class BuySuggestions
    {
      public string BuyASXCode { get; set; }
      public decimal BuyLastDividendAmount { get; set; }
      public DateTime BuyLastDivDate { get; set; }
      public int BuySOH { get; set; }
      public DateTime SellDate { get; set; }
      public decimal BuyDaysHeld { get; set; }
      public decimal UnitSellPrice { get; set; }
      public decimal BuyTodaysUnitPrice { get; set; }
      public decimal BuyPctGain { get; set; }
      public decimal BuyPctYear { get; set; }
      public bool BuyHighlightDiv { get; set; }
    }

    private void dgvToBuy_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      try
      {
        DataGridView dgv = sender as DataGridView;
        if (dgv.Columns[e.ColumnIndex].ValueType == typeof(decimal))
          if ((decimal)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value < 0M)
            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
        if ((dgv.Columns[e.ColumnIndex].Name == "BuyLastDividendAmount" || dgv.Columns[e.ColumnIndex].Name == "BuyLastDivDate") && (bool)dgv.Rows[e.RowIndex].Cells["BuyHighlightDiv"].Value)
        {
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
        }

      }
      catch
      { }
    }

    private void dgvToBuy_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == _previousIndex)
        _sortDirection ^= true; // toggle direction

      dgvToBuy.DataSource = SortToBuyData(
          (List<BuySuggestions>)ToBuyBindingSource.DataSource, dgvToBuy.Columns[e.ColumnIndex].Name, _sortDirection);

      _previousIndex = e.ColumnIndex;
    }

    public List<BuySuggestions> SortToBuyData(List<BuySuggestions> list, string column, bool ascending)
    {
      try
      {
        return ascending ?
           /* RefreshId( */ list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /* ) */ :
           /* RefreshId( */ list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /*) */;
      }
      catch
      { }
      return list;
    }

    // **************  Rebuys  ***********************************************************
    public class ReBuySuggestions
    {
      public string ReBuyASXCode { get; set; }
      public decimal ReBuyLastDividendAmount { get; set; }
      public DateTime ReBuyLastDivDate { get; set; }
      public int LastPurchaseQty { get; set; }
      public DateTime LastBuyDate { get; set; }
      public decimal DaysSinceLastBuy { get; set; }
      public decimal LastBuyPrice { get; set; }
      public decimal ReBuyTodaysUnitPrice { get; set; }
      public decimal ReBuyPctGain { get; set; }
      public decimal ReBuyPctYear { get; set; }
      public bool ReBuyHighlightDiv { get; set; }

    }
    private void populateReBuyGrid()
    {
      List<ReBuySuggestions> suggestList = new List<ReBuySuggestions>();
      // Get all the shares we have stock of
      List<DBAccess.TransRecords> transList = null;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();

      if (!DBAccess.GetTransRecords(null, out transList, DBAccess.TransRecordsFieldList, " AND trn_soh  > 0 ", " ORDER BY trn_ASXCode, trn_transdate DESC", false))
      {
        MessageBox.Show("No transactions found", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      var uCode = transList.Select(x => x.ASXCode).Distinct().ToList();
      foreach (string code in uCode)
      {
        List<DBAccess.TransRecords> codeList = transList.FindAll(delegate (DBAccess.TransRecords r1) { return r1.ASXCode == code; });
        codeList = codeList.OrderByDescending(x => x.TranDate).ToList();
        // Check if stock has been bought again since this sell
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", codeList[0].ASXCode));
        paramList.Add(new PgSqlParameter("@P2", codeList[0].TranDate));
        List<DBAccess.TransRecords> buyList = new List<DBAccess.TransRecords>();
        if (DBAccess.GetTransRecords(paramList, out buyList, DBAccess.TransRecordsFieldList, " AND trn_asxcode = @P1 AND trn_transdate >= @P2  AND trn_buysell = 'Sell' ", string.Empty, false))
          continue;
        //For each ASXCode, get latest buy but make sure there is not a sell after this buy and work out the percentages

        List<DBAccess.ASXPriceDate> todaysPrice = null;
        DateTime dt = DateTime.MinValue;
        Decimal prc = 0M;
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", code));
        if (DBAccess.GetPriceRecords(paramList, out todaysPrice, DBAccess.ASXPriceDateFieldList, " AND apd_asxcode = @P1 ", "ORDER BY apd_pricedate DESC ", false))
        {
          // no prices for this ASX Code so we'll pretend

        }
        if (todaysPrice != null && todaysPrice.Count > 0)
        {
          prc = todaysPrice[0].PrcClose;
          dt = todaysPrice[0].PriceDate;
        }
        // create records to display
        DBAccess.TransRecords transRec = codeList[0];
        decimal daysHeld = (DateTime.Today - transRec.TranDate.Date).Days;
        if (daysHeld < 3)
          daysHeld = 3;         // takes 3 days for the transaction to be finalised
        ReBuySuggestions sug = new ReBuySuggestions();
        sug.ReBuyASXCode = transRec.ASXCode;
        sug.LastPurchaseQty = transRec.TransQty;
        sug.LastBuyPrice = transRec.UnitPrice;
        sug.ReBuyTodaysUnitPrice = prc;
        sug.ReBuyPctGain = 0M;
        sug.ReBuyPctYear = 0M;
        sug.DaysSinceLastBuy = daysHeld;
        sug.LastBuyDate = transRec.TranDate;
        if (prc != 0 && sug.LastBuyPrice != 0)
        {
          sug.ReBuyPctGain = Decimal.Round((sug.ReBuyTodaysUnitPrice - sug.LastBuyPrice) * 100 / sug.LastBuyPrice, 2);
          sug.ReBuyPctYear = Decimal.Round((sug.ReBuyTodaysUnitPrice - sug.LastBuyPrice) / sug.LastBuyPrice * 365 * 100 / daysHeld, 2);
        }

        //  Is there a dividend pending - already announced (ie. with ex dividend date +/- 10 days from today)
        DBAccess.DividendHistory divHist = new DBAccess.DividendHistory();
        if (sug.ReBuyASXCode == "WES")
        { }

        if (dividendPending(sug.ReBuyASXCode, -30, 30, out divHist))
        {
          sug.ReBuyLastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.ReBuyTodaysUnitPrice * 100, 2);
          sug.ReBuyLastDivDate = divHist.ExDividend;
          sug.ReBuyHighlightDiv = true;
        }
        else
        {
          // or issued +/- 12 months ago
          if (dividendPending(sug.ReBuyASXCode, -374, -354, out divHist))
          {
            sug.ReBuyLastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.ReBuyTodaysUnitPrice * 100, 2);
            sug.ReBuyLastDivDate = divHist.ExDividend;
            sug.ReBuyHighlightDiv = true;
          }
          else
          {
            if (dividendPending(sug.ReBuyASXCode, -10000, 0, out divHist))
            {
              sug.ReBuyLastDividendAmount = Decimal.Round(divHist.GrossDividend / sug.ReBuyTodaysUnitPrice * 100, 2);
              sug.ReBuyLastDivDate = divHist.ExDividend;
              sug.ReBuyHighlightDiv = false;
            }
          }

        }

        suggestList.Add(sug);

      }
      suggestList = suggestList.OrderBy(x => x.ReBuyPctGain).ToList();
      // 
      dgvToReBuy.DataSource = null;
      ToReBuyBindingSource.DataSource = suggestList;
      dgvToReBuy.DataSource = ToReBuyBindingSource;
      dgvToReBuy.Refresh();

    }



    private void dgvToReBuy_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      try
      {
        DataGridView dgv = sender as DataGridView;
        if (dgv.Columns[e.ColumnIndex].ValueType == typeof(decimal))
          if ((decimal)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value < 0M)
            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
        if ((dgv.Columns[e.ColumnIndex].Name == "BuyLastDividendAmount" || dgv.Columns[e.ColumnIndex].Name == "BuyLastDivDate") && (bool)dgv.Rows[e.RowIndex].Cells["BuyHighlightDiv"].Value)
        {
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
        }

      }
      catch
      { }
    }

    private void dgvToReBuy_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == _previousIndex)
        _sortDirection ^= true; // toggle direction

      dgvToReBuy.DataSource = SortToReBuyData(
          (List<ReBuySuggestions>)ToReBuyBindingSource.DataSource, dgvToReBuy.Columns[e.ColumnIndex].Name, _sortDirection);

      _previousIndex = e.ColumnIndex;
    }

    public List<ReBuySuggestions> SortToReBuyData(List<ReBuySuggestions> list, string column, bool ascending)
    {
      try
      {
        return ascending ?
           /* RefreshId( */ list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /* ) */ :
           /* RefreshId( */ list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /*) */;
      }
      catch
      { }
      return list;
    }

    private void chbAll_CheckedChanged(object sender, EventArgs e)
    {
      populateBuyGrid();
    }

    //  ****  Transactions Tab
    public class Transactions
    {
      public string TransASXCode { get; set; }
      public DateTime TransDate { get; set; }
      public string TransBuySell { get; set; }
      public string TransNABOrderNmbr { get; set; }
      public int TransQty { get; set; }
      public decimal TransUnitPrice { get; set; }
      public decimal TransProfit { get; set; }
      public decimal TransSOH { get; set; }
      public decimal TransCompanyTotalSOH { get; set; }

    }
    private void populateTransactions()
    {
      List<Transactions> displayList = new List<Transactions>();
      // Get all the shares we have stock of
      List<DBAccess.TransRecords> transList = null;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();

      if (!DBAccess.GetTransRecords(null, out transList, DBAccess.TransRecordsFieldList, string.Empty, " ORDER BY trn_ASXCode, trn_transdate DESC", false))
      {
        MessageBox.Show("No transactions found", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      foreach (DBAccess.TransRecords rec in transList)
      {
        Decimal totalSOH = transList.FindAll(delegate (DBAccess.TransRecords r1) { return r1.ASXCode == rec.ASXCode; }).Sum(x => x.SOH);
        // Display transaction
        Transactions outRec = new FrmSuggestions.Transactions();
        outRec.TransASXCode = rec.ASXCode;
        outRec.TransDate = rec.TranDate;
        outRec.TransBuySell = rec.BuySell;
        outRec.TransNABOrderNmbr = rec.NABOrderNmbr;
        outRec.TransQty = rec.TransQty;
        outRec.TransUnitPrice = rec.UnitPrice;
        outRec.TransProfit = rec.TradeProfit;
        outRec.TransSOH = rec.SOH;
        outRec.TransCompanyTotalSOH = totalSOH;
        displayList.Add(outRec);

      }
      // 
      dgvTransactions.DataSource = null;
      TransactionsBindingSource.DataSource = displayList;
      dgvTransactions.DataSource = TransactionsBindingSource;
      dgvTransactions.Refresh();

    }



    private void dgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      try
      {
        DataGridView dgv = sender as DataGridView;
        if (dgv.Columns[e.ColumnIndex].ValueType == typeof(decimal))
          if ((decimal)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value < 0M)
            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
        if ((dgv.Columns[e.ColumnIndex].Name == "BuyLastDividendAmount" || dgv.Columns[e.ColumnIndex].Name == "BuyLastDivDate") && (bool)dgv.Rows[e.RowIndex].Cells["BuyHighlightDiv"].Value)
        {
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
        }

      }
      catch
      { }
    }

    private void dgvTransactions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == _previousIndex)
        _sortDirection ^= true; // toggle direction

      dgvTransactions.DataSource = SortTransactionsData(
          (List<Transactions>)TransactionsBindingSource.DataSource, dgvTransactions.Columns[e.ColumnIndex].Name, _sortDirection);

      _previousIndex = e.ColumnIndex;
    }

    public List<Transactions> SortTransactionsData(List<Transactions> list, string column, bool ascending)
    {
      try
      {
        return ascending ?
           /* RefreshId( */ list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /* ) */ :
           /* RefreshId( */ list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /*) */;
      }
      catch
      { }
      return list;
    }

    private void tabSells_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (tabSells.SelectedTab == tabSells.TabPages[3])
      {
        populateTransactions();
      }
    }
  }
}
