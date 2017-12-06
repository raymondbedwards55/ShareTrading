using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devart.Data.PostgreSql;

using System.Windows.Forms;
//using System.Data.OleDb;
using Devart.Data.PostgreSql;

//using System.Web.UI.DataVisualization.Charting;


// ***************************
//  Let's document ray 9      
// ***************************

namespace ShareTrading
{
    public partial class Form1 : Form
    {

        public DBAccess DB;
        private Decimal MarginLending = (Decimal)0.065;
        private Decimal MinMargin = (Decimal)0.05;
        private Decimal BuyResistance = (Decimal)0.0;
        private int PeriodForLow = 60;
        private int PeriodForHigh = 60;
        private bool MaxBuys = true;
        private bool MaxSells = true;
        private int MaxRebuyCount = 3;  // The maximum number of parcels of any stock
        private bool ChaseDividends = true;  // Buy close to dividends to look for dividend or short gains
        private Decimal MarginLoanRebuyLimit = (Decimal).1;  //After we reach this limit (eg say.1) no more buys are allowed
        private Decimal TargetBuyReturn = (Decimal).005;  // THis is used as the log target for Buys
        private Decimal TargetSellReturn = (Decimal).02;  // THis is used as the log target for Sells
        private bool BuyOnDaysMin = true; //  Only buy on 5,0 .. days min if allowed


        public Decimal MaxMarginLoan = 0;
        public Decimal CorrespondingSOH = 0;

        public DateTime StartDate = new DateTime(2016, 1, 1);
        public DateTime EndDate = new DateTime(2016, 12, 15);
        public Decimal StartBal = 300000;
        public Decimal AddSellMrgn = (Decimal)1.015;
        public Decimal AddBuyMrgn = (Decimal).985;
        public Decimal RebuyMargin = (Decimal)0.80;
        public Decimal MarginLendingBarrier = (Decimal)3;


        public Form1()
        {
            InitializeComponent();
      //DB = new DBAccess();
      if (!DBAccess.CheckConnection(/* "localhost", 5432 */))
      {
        MessageBox.Show("Unable to connect to database", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Close();
      }

     }

    private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PlotChart()
        {

        }

    private void FillGrid()
    {
      /*
                  public int ID;
                  public String ASXCode;
                  public String BuySell;
                  public int TransQty;
                  public Decimal UnitPrice;
                  public String TransType;
                  public Decimal ROI;
                  public Decimal CurrPrc;
                  public Decimal Profit;
                  public Decimal CurrProfit;
      */
      //OleDbConnection connectionPrice = new OleDbConnection();
      //OleDbCommand command = new OleDbCommand();
      //connectionPrice.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
      //string query = "SELECT ASXCode, TransQty, CurrPrc, UnitPrice, ROI, CurrProfit, TargetProfit From TodaysTrades where BuySell = 'Sell'";
      //using (OleDbConnection conn = new OleDbConnection(connectionPrice.ConnectionString))
      //{
      //    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
      //    {
      //        DataSet ds = new DataSet();
      //        adapter.Fill(ds);
      //        DgvSuggestedSells.DataSource = ds.Tables[0];
      //    }
      //}
      //query = "SELECT * From TodaysTrades where BuySell = 'Buy'";
      //using (OleDbConnection conn = new OleDbConnection(connectionPrice.ConnectionString))
      //{
      //    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
      //    {
      //        DataSet ds = new DataSet();
      //        adapter.Fill(ds);
      //        DgvSuggestedBuys.DataSource = ds.Tables[0];
      //    }
      //}

      using (PgSqlConnection conn = new PgSqlConnection(DBAccess.DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          string whereString = string.Empty;
          command.CommandText = string.Format("SELECT ASXCode, TransQty, CurrPrc, UnitPrice, ROI, CurrProfit, TargetProfit From TodaysTrades where BuySell = 'Sell' ");
          command.Prepare();
          try
          {
            DataSet ds = new DataSet();
            PgSqlDataAdapter da = new PgSqlDataAdapter(command);
            da.Fill(ds);
            DgvSuggestedSells.DataSource = ds.Tables[0];
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return;
          }
          command.CommandText = string.Format("SELECT ASXCode, TransQty, CurrPrc, UnitPrice, ROI, CurrProfit, TargetProfit From TodaysTrades where BuySell = 'Buy' ");
          try
          {
            DataSet ds = new DataSet();
            PgSqlDataAdapter da = new PgSqlDataAdapter(command);
            da.Fill(ds);
            DgvSuggestedBuys.DataSource = ds.Tables[0];

          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return;
          }

        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DBAccess.DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
    }

        // NAB Brokerage rules
        private static decimal CalcBrokerage(decimal transVal)
        {
            if (transVal <= (decimal)5000.0)
                return (decimal)14.95;
            else if (transVal <= (decimal)20000.0)
                return (decimal)19.95;
            else
                return (decimal)(transVal * (decimal).0011);
        }


        private void TrialSimulation(object sender, EventArgs e)
        {
            //AddSellMrgn = (Decimal)1.015;
            // AddBuyMrgn = (Decimal).985;
            // RebuyMargin = (Decimal)0.7;
            MarginLendingBarrier = (Decimal)3;
            for (AddSellMrgn = (Decimal)1.005; AddSellMrgn < (Decimal)1.010; AddSellMrgn += (Decimal).005)
            {
                for (AddBuyMrgn = (Decimal)0.990; AddBuyMrgn < (Decimal)1.0; AddBuyMrgn += (Decimal).005)
                {
                    for (RebuyMargin = (Decimal)0.96; RebuyMargin <= (Decimal).99; RebuyMargin += (Decimal).01)
                    {
                        for (TargetSellReturn = (Decimal)0.008; TargetSellReturn <= (Decimal).03; TargetSellReturn += (Decimal).002)
                        {
                            for (TargetBuyReturn = (Decimal)0.002; TargetBuyReturn <= (Decimal).03; TargetBuyReturn += (Decimal).002)
                            {
                                RangeTest(true);
                            }
                        }
                    }
                }
            }
        }

        private void RangeTest(bool runningSimulation)
        {
            DBAccess.TransRecords TransRecords;
      List<DBAccess.TransRecords> transList = null;
            //DB = new DBAccess();
            //DBAccess.connection.Open();
            DBAccess.PrepareForSimulation();
      List<DBAccess.ASXPriceDate> allPrices = new List<DBAccess.ASXPriceDate>();
            DBAccess.GetAllPrices(null, StartDate, out allPrices, DBAccess.ASXPriceDateFieldList);
            DateTime lastDate = StartDate;
            //  Set up the starting Account Bal
            DBAccess.BankBal bankBal = new DBAccess.BankBal();
            bankBal.BalDate = lastDate;
            bankBal.AcctBal = StartBal;
            DBAccess.BankBalInsert(bankBal, runningSimulation);
            Decimal DayDivTotal = (Decimal)0.0;

            foreach(DBAccess.ASXPriceDate rec in allPrices)
            {
                DBAccess.DividendHistory dividendHistory = null;
        List<DBAccess.DividendHistory> list = new List<DBAccess.DividendHistory>();
        if (DBAccess.GetDividends(rec.ASXCode, lastDate, out list, DBAccess.dirn.lessThan))
          dividendHistory = list[0]; ;
                if (dividendHistory == null)
                    continue;
                //                if (dividendHistory.GrossDividend > ASXPriceDate.PrcClose * (Decimal).02)
                //                    continue;

                //                if (ASXPriceDate.ASXCode != "LL")
                //                   continue;
                int ID = rec.ID;
                DBAccess.DivPaid dp = CommonFunctions.CheckForDividends(rec.ASXCode, rec.PriceDate, runningSimulation);
                //if (dp != null)
                //    DayDivTotal = dp.TtlDividend + DayDivTotal;
                if (rec.PriceDate > lastDate)
                {
                    if (MaxMarginLoan < bankBal.MarginLoan)
                    {
                        MaxMarginLoan = bankBal.MarginLoan;
                        CorrespondingSOH = bankBal.TtlDlrSOH;
                    }

                    if (rec.PriceDate == EndDate)
                    {
                        DBAccess.SimulationPerformance Performance = new DBAccess.SimulationPerformance();
                        Performance.EndDate = EndDate;
                        Performance.StartDate = StartDate;
                        Performance.BuyPriceTargetPct = AddBuyMrgn;
                        Performance.SellPriceTargetPct = AddSellMrgn;
                        Performance.MarginLendingBarrier = MarginLendingBarrier;
                        Performance.MaxMarginLoan = MaxMarginLoan;
                        if (CorrespondingSOH > 0)
                            Performance.MaxMarginLoanPctOfSOH = MaxMarginLoan / CorrespondingSOH;
                        Performance.MinPriceDays = 5;
                        Performance.NetProfit = bankBal.TtlDlrSOH + bankBal.AcctBal - StartBal - bankBal.MarginLoan + bankBal.TtlDividendEarned;
                        Performance.RebuyPct = RebuyMargin;
                        Performance.MaxRebuyCount = MaxRebuyCount;
                        Performance.ChaseDividends = ChaseDividends;
                        Performance.MarginLoanRebuyLimit = MarginLoanRebuyLimit;
                        Performance.TargetBuyReturn = TargetBuyReturn;
                        Performance.TargetSellReturn = TargetSellReturn;
                        Performance.BuyOnDaysMin = BuyOnDaysMin;
                        Performance.MaxSells = MaxSells;
                        Performance.MaxBuys = MaxBuys;
                        DBAccess.SimulationPerformanceInsert(Performance);
                        MaxMarginLoan = 0;
                        CorrespondingSOH = 0;
                        return;
                    }
          List<DBAccess.BankBal> balList = new List<DBAccess.BankBal>();
                    if (DBAccess.GetAllBankBalRecords(lastDate, out balList, string.Empty, runningSimulation, DBAccess.dirn.equals  ))
                        bankBal = balList[0];

                    bankBal.TtlDlrSOH = DBAccess.UpdateCurrentSOH(bankBal, runningSimulation);
                    bankBal.TtlTradeProfit = bankBal.TtlTradeProfit + bankBal.DayTradeProfit;
                    //                    bankBal.MarginLoan = MarginLoanBal;
                    //                    bankBal.AcctBal = AcctBal;
                    bankBal.BalDate = lastDate;
                    bankBal.DlrDaysInvested = StartBal - bankBal.AcctBal + bankBal.MarginLoan;
                    bankBal.TtlDlrDaysInvested = bankBal.TtlDlrDaysInvested + bankBal.DlrDaysInvested;
                    bankBal.DayDividend = DayDivTotal;
                    bankBal.TtlDividendEarned = bankBal.TtlDividendEarned + DayDivTotal;
                    DBAccess.BankBalUpdate(bankBal);

                    // Now move forward & Reset the Day stuff
                    lastDate = rec.PriceDate;
                    DayDivTotal = (Decimal)0.0;
                    bankBal.BalDate = lastDate;
                    bankBal.DayTradeProfit = 0;
                    bankBal.DlrDaysInvested = 0;
                    bankBal.TtlDlrDaysInvested = 0;
                    bankBal.DayDividend = 0;
                    DBAccess.BankBalInsert(bankBal, runningSimulation);
                }
                bool didSell = false;
                if (!DBAccess.GetAllTransRecords(rec.ASXCode, DateTime.MinValue, out transList, DBAccess.TransRecordsFieldList, string.Empty, runningSimulation))
                    continue;
                else
                {
          
                    // Sellls  ------------------------------------------------------------
                    foreach (DBAccess.TransRecords transRec in transList)  // get transaction where we bought these
                    {
                        Decimal SellPrice = 0;
                        DateTime TransDate = transRec.TranDate;
                        // Difference in days, hours, and minutes.
                        TimeSpan ts = lastDate - TransDate;
                        // Difference in days.
                        Double DaysHeld = (Double)ts.Days;

                        Decimal TargetPrice = 0;
                        TargetPrice = transRec.UnitPrice * (Decimal)(1.005 + ((Double)TargetSellReturn * Math.Sqrt(DaysHeld)));
                        if (rec.PrcOpen >= TargetPrice)
                        {
                            SellPrice = rec.PrcOpen;
                            if (transRec.SOH <= 0)
                                continue;
                            DBAccess.TransRecords SellTrn = SellTransaction(rec.ASXCode, transRec.TransQty, SellPrice, lastDate, transRec, "SellOnOpen4Return", runningSimulation);
                            didSell = true;
                            Decimal NewBuyTarget = rec.PrcOpen * (Decimal)(0.995 - ((Double)TargetBuyReturn * Math.Sqrt(1)));
                            if (rec.PrcLow < NewBuyTarget && MaxBuys)  //  If MayBBuys && Can only buy during the remainder of the day
                            {
                                int BuyQty = CommonFunctions.GetBuyQty(bankBal, NewBuyTarget, MarginLendingBarrier);
                                BuyTransaction(rec.ASXCode, BuyQty, NewBuyTarget, lastDate, "SDBuyAfterSell", runningSimulation);
                            }
                            break;
                        }
                        if (rec.PrcHigh >= TargetPrice)
                        {
                            if (transRec.SOH <= 0)
                                continue;
                            DBAccess.TransRecords SellTrn = SellTransaction(rec.ASXCode, transRec.TransQty, TargetPrice, lastDate, transRec, "SellOnDayHigh", runningSimulation);
                            didSell = true;
                        }
                    }
                }
                if (didSell)
                    continue;

                // Buys ------------------------------------

                //  Buy on margin below last sell - 
                if (DBAccess.SetupLastSellRecords(rec.ASXCode, runningSimulation, out transList))
                {
                    if (transList.Count > 0)
                    {
            TransRecords = transList[0];
                        if (TransRecords.BuySell == "Sell")
                        {
                            DateTime TransDate = TransRecords.TranDate;
                            // Difference in days, hours, and minutes.
                            TimeSpan ts = lastDate - TransDate;
                            // Difference in days.
                            Double DaysHeld = (Double)ts.Days;
                            Decimal BuyPrice = 0;
                            Decimal TargetPrice = 0;
                            int BuyQty = 0;
                            TargetPrice = TransRecords.UnitPrice * (Decimal)(0.995 - ((Double)TargetBuyReturn * Math.Sqrt(DaysHeld)));
                            if (rec.PrcLow <= TargetPrice)
                            {
                                if (rec.PrcOpen <= TargetPrice)
                                {
                                    BuyPrice = rec.PrcOpen;
                                    BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                    TransRecords = BuyTransaction(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnOpenBelowSell", runningSimulation);

                                    TargetPrice = TransRecords.UnitPrice * (Decimal)(1.005 + ((Double)TargetSellReturn * Math.Sqrt(1)));
                                    if (rec.PrcHigh >= TargetPrice && MaxBuys)
                                    {
                                        if (TransRecords.SOH <= 0)
                                            continue;
                                        TransRecords = SellTransaction(rec.ASXCode, BuyQty, TargetPrice, lastDate, TransRecords, "SellSameDayOnBuy", runningSimulation);
                                        continue;
                                    }
                                }
                                else
                                    BuyPrice = TargetPrice;
                                //Transaction Size
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuyTransaction(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnLow4Day", runningSimulation);
                                continue;
                            }
                        }
                    }
                }

                // We don't have any so lets buy on a 5 day low or if very close to a Dividend
                if (DBAccess.GetAllTransRecords(rec.ASXCode, DateTime.MinValue, out transList, DBAccess.TransRecordsFieldList, string.Empty, runningSimulation))
                {
                    Decimal BuyPrice = 0;
                    if (transList.Count <= 0)
                    {
            DBAccess.TransRecords transRec = transList[0];
                        //  Buy within 10 days of Dividend - 
                        DBAccess.DividendHistory DivHis = new DBAccess.DividendHistory();
            if (DBAccess.GetDividends(rec.ASXCode, rec.PriceDate, out list, DBAccess.dirn.greaterThanEquals))
            {
              DivHis = list[0];
              if (DivHis  != null && ChaseDividends)  // Only do this is chasing Dividends
                            {
                                if (DateTime.Compare(DivHis.ExDividend, rec.PriceDate.AddDays(10)) < 0)
                                {
                                    //Transaction Size
                                    BuyPrice = rec.PrcOpen;
                                    int BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                    DBAccess.TransRecords BuyTrn = BuyTransaction(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyNearDividend", runningSimulation);
                                    continue;
                                }
                            }
                        }

                        if (rec.PrcLow <= rec.Day5Min * AddBuyMrgn &&
                            rec.Day5Min > rec.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            int BuyQty = 0;
                            if (rec.PrcOpen <= rec.Day5Min * AddBuyMrgn)
                            {
                                BuyPrice = rec.PrcOpen;
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                DBAccess.TransRecords BuyTrn = BuyTransaction(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnOpenDayMin", runningSimulation);
                                Decimal TargetPrice = BuyPrice * (Decimal)(1.005 + (.01 * Math.Sqrt(1)));
                                if (rec.PrcHigh > TargetPrice && MaxSells)
                                    SellTransaction(rec.ASXCode, BuyQty, TargetPrice, lastDate, BuyTrn, "SellAfterBuyOnMin", runningSimulation);
                            }
                            else if (rec.PrcLow <= rec.Day5Min * AddBuyMrgn && bankBal.TtlDlrSOH > 0)
                            {
                                if (bankBal.MarginLoan / bankBal.TtlDlrSOH > (Decimal)MarginLoanRebuyLimit)
                                    continue;
                                BuyPrice = rec.Day5Min * AddBuyMrgn;
                                //Transaction Size
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuyTransaction(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnDayMin", runningSimulation);
                            }
                        }
                    }
                    else  // already have some - doing rebuy
                    {
                        if (rec.PrcLow <= rec.Day5Min * AddBuyMrgn &&
                            rec.Day5Min > rec.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            if (rec.PrcOpen <= rec.Day5Min * AddBuyMrgn)
                                BuyPrice = rec.PrcOpen;
                            else
                                BuyPrice = rec.Day5Min * AddBuyMrgn;
                            if (BuyPrice < (Decimal)RebuyMargin * transList[0].UnitPrice && bankBal.TtlDlrSOH > 0)
                            {
                                if (bankBal.MarginLoan / bankBal.TtlDlrSOH > (Decimal)MarginLoanRebuyLimit)
                                    continue;
                                int BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuyTransaction(rec.ASXCode, BuyQty, BuyPrice, lastDate, "Rebuy", runningSimulation);
                            }
                        }
                    }
                }


                /*               
                if ((TransRecords = DB.GetNextTransRecords()) != null)
                {
                    Decimal SellPrice = 0;
                    if (ASXPriceDate.High >= ASXPriceDate.Day5Max * AddSellMrgn &&
                        ASXPriceDate.Day5Max >= ASXPriceDate.Day90Max )  // 'x' day max
                    {
                        if (ASXPriceDate.PrcOpen >= ASXPriceDate.Day5Max * AddSellMrgn)
                            SellPrice = ASXPriceDate.PrcOpen;
                        else
                            SellPrice = ASXPriceDate.Day5Max * AddSellMrgn;
                        if (SellPrice < TransRecords.UnitPrice * (Decimal)1.05 || TransRecords.SOH <= 0)
                            continue;
                        SellTransaction(ASXPriceDate.ASXCode, TransRecords.TransQty , SellPrice, lastDate, TransRecords);
                    }
                }
                if (DB.GetTransRecords(ASXPriceDate.ASXCode, new DateTime(1900, 1, 1)))
                {
                    Decimal BuyPrice = 0;
                    if ((TransRecords = DB.GetNextTransRecords()) == null)
                    {
                        if (ASXPriceDate.Low <= ASXPriceDate.Day5Min * AddBuyMrgn &&
                            ASXPriceDate.Day5Min > ASXPriceDate.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            if (ASXPriceDate.PrcOpen <= ASXPriceDate.Day5Min * AddBuyMrgn)
                                BuyPrice = ASXPriceDate.PrcOpen;
                            else
                                BuyPrice = ASXPriceDate.Day5Min * AddBuyMrgn;

                            //Transaction Size
                            Decimal TtlDlrs = (decimal)(10000 + (bankBal.AcctBal - MarginLendingBarrier * bankBal.MarginLoan + bankBal.TtlDlrSOH - 300000) / 20);
                            if (TtlDlrs > 20000)
                                TtlDlrs = 20000;
                            if (TtlDlrs < 5000)
                                TtlDlrs = 5000;
                            int BuyQty = (int)Math.Round(TtlDlrs/ BuyPrice);
                            BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate);
                        }
                    }
                    else  // already have some - doing rebuy
                    {
                        if (ASXPriceDate.Low <= ASXPriceDate.Day5Min * AddBuyMrgn &&
                            ASXPriceDate.Day5Min > ASXPriceDate.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            if (ASXPriceDate.PrcOpen == ASXPriceDate.Day5Min * AddBuyMrgn)
                                BuyPrice = ASXPriceDate.PrcOpen;
                            else
                                BuyPrice = ASXPriceDate.Day5Min * AddBuyMrgn;
                            if (BuyPrice < (Decimal)RebuyMargin * TransRecords.UnitPrice)
                            {
                                Decimal TtlDlrs = (decimal)(10000 + (bankBal.AcctBal - MarginLendingBarrier * bankBal.MarginLoan + bankBal.TtlDlrSOH - 300000) / 20);
                                if (TtlDlrs > 20000)
                                    TtlDlrs = 20000;
                                if (TtlDlrs < 5000)
                                    TtlDlrs = 5000;
                                int BuyQty = (int)Math.Round(TtlDlrs/ BuyPrice);
                                BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate);
                            }
                        }
                    } 
                }
            */
            }
        }
        /*       
                private int GetBuyQty(DBAccess.BankBal bankBal, Decimal BuyPrice)
                {
                    Decimal TtlDlrs = (decimal)(10000 + (bankBal.AcctBal - MarginLendingBarrier * bankBal.MarginLoan + bankBal.TtlDlrSOH - 300000) / 20);
                    if (TtlDlrs > 20000)
                        TtlDlrs = 20000;
                    if (TtlDlrs < 5000)
                        TtlDlrs = 5000;
                    int BuyQty = (int)Math.Round(TtlDlrs / BuyPrice);
                    return BuyQty;
                }
        */
        private void SetMinMaxs()
        {
            SetMinMaxs(new DateTime(2016, 12, 13));
        }

        private void SetMinMaxs(DateTime dt)
        {
            DateTime lastDateTime = dt;
      DBAccess.ASXPriceDate ASXPriceDate = new DBAccess.ASXPriceDate();
      //DB.connection.Open();
      //PgSqlDataReader priceReader;
      List<DBAccess.ASXPriceDate> list = new List<DBAccess.ASXPriceDate>();
      DBAccess.GetAllPrices(null, lastDateTime, out list, DBAccess.ASXPriceDateFieldList);
      foreach (DBAccess.ASXPriceDate rec in list)
      { 
            //while ((ASXPriceDate = DB.GetNextPriceDate(priceReader)) != null)
            //{
                DateTime PriceDate = ASXPriceDate.PriceDate;
                if (lastDateTime != PriceDate)
                {
                    lastDateTime = PriceDate;
                }
                string ASXCode = ASXPriceDate.ASXCode;
                if (ASXCode == null)
                    break;
                ASXPriceDate.Day5Max = DBAccess.GetMaxPrice(7, ASXCode, PriceDate);
                ASXPriceDate.Day30Max = DBAccess.GetMaxPrice(30, ASXCode, PriceDate);
                ASXPriceDate.Day60Max = DBAccess.GetMaxPrice(60, ASXCode, PriceDate);
                ASXPriceDate.Day90Max = DBAccess.GetMaxPrice(90, ASXCode, PriceDate);
                ASXPriceDate.Day5Min = DBAccess.GetMinPrice(7, ASXCode, PriceDate);
                ASXPriceDate.Day30Min = DBAccess.GetMinPrice(30, ASXCode, PriceDate);
                ASXPriceDate.Day60Min = DBAccess.GetMinPrice(90, ASXCode, PriceDate);
                ASXPriceDate.Day90Min = DBAccess.GetMinPrice(90, ASXCode, PriceDate);
                if (ASXPriceDate.Day5Min > 0)
                    ASXPriceDate.Day5Pct = (100 * (ASXPriceDate.Day5Max - ASXPriceDate.Day5Min) / ASXPriceDate.Day5Min);
                if (ASXPriceDate.Day30Min > 0)
                    ASXPriceDate.Day30Pct = (100 * (ASXPriceDate.Day30Max - ASXPriceDate.Day30Min) / ASXPriceDate.Day30Min);
                if (ASXPriceDate.Day60Min > 0)
                    ASXPriceDate.Day60Pct = (100 * (ASXPriceDate.Day60Max - ASXPriceDate.Day60Min) / ASXPriceDate.Day60Min);
                if (ASXPriceDate.Day90Min > 0)
                    ASXPriceDate.Day90Pct = (100 * (ASXPriceDate.Day90Max - ASXPriceDate.Day90Min) / ASXPriceDate.Day90Min);
                DBAccess.ASXprcUpdate(ASXPriceDate);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetMinMaxs();
        }

        private void setMinMaxS(object sender, EventArgs e)
        {
            //SetLimitTriggers(new DateTime(2015, 8, 18));
        }

        public static DBAccess.TransRecords BuyTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, String TransType, bool runningSimulation)
        {
            return BuyTransaction(ASXCode, Qty, Price, dt, TransType, null, runningSimulation);
        }

        public static DBAccess.TransRecords BuyTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, String TransType, String NABOrderNmbr, bool simulationRunning)
        {
            //DBAccess DB = new DBAccess();
            decimal Brokerage = CalcBrokerage(Qty * Price);  // Brokerage includes GST
            decimal GST = (Brokerage) / 11;
            DBAccess.TransRecords myTransRecord = new DBAccess.TransRecords();
            myTransRecord.ASXCode = ASXCode;
            myTransRecord.TransQty = Qty;
            myTransRecord.SOH = Qty;
            myTransRecord.UnitPrice = Price;
            myTransRecord.TradeProfit = -Brokerage;
            myTransRecord.TranDate = dt;
            myTransRecord.BuySell = "Buy";
            myTransRecord.BrokerageInc = Brokerage;
            myTransRecord.GST = GST;
            myTransRecord.TransType = TransType;
            myTransRecord.NABOrderNmbr = NABOrderNmbr;
            DBAccess.TransInsert(myTransRecord);
            UpdateBankBal(-(Price * Qty + myTransRecord.BrokerageInc), myTransRecord.TradeProfit, dt, simulationRunning);
      List<DBAccess.TransRecords> transList = new List<DBAccess.TransRecords>();
            DBAccess.GetAllTransRecords(ASXCode, dt, out transList, DBAccess.TransRecordsFieldList, string.Empty, simulationRunning);
      if (transList.Count > 0)
            myTransRecord = transList[0];
            return myTransRecord;
        }

        public static DBAccess.TransRecords SellTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, DBAccess.TransRecords BoughtRecord, String TransType, bool runningSimulation)
        {
            return SellTransaction(ASXCode, Qty, Price, dt, BoughtRecord, TransType, "", runningSimulation);
        }

        public static DBAccess.TransRecords SellTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, DBAccess.TransRecords BoughtRecord, String TransType, String NABOrderNmbr, bool simulationRunning)
        {
      List<DBAccess.TransRecords> transList = new List<DBAccess.TransRecords>();
            //DBAccess DB = new DBAccess();
            if (BoughtRecord == null)
            {
                DBAccess.GetAllTransRecords(ASXCode, DateTime.MinValue, out transList, DBAccess.TransRecordsFieldList, string.Empty, simulationRunning);
                BoughtRecord = transList[0];
            }

            Decimal PurchaseCost = 0;
            int QtyNeeded = Qty;
            while (QtyNeeded > 0)
            {
        int counter = 1;
                if (QtyNeeded > BoughtRecord.SOH)
                {
                    QtyNeeded = QtyNeeded - BoughtRecord.SOH;
                    PurchaseCost = PurchaseCost + BoughtRecord.SOH * BoughtRecord.UnitPrice;
                    BoughtRecord.SOH = 0;
                    DBAccess.TransUpdate(BoughtRecord);
                    BoughtRecord = transList[counter++];
                }
                else
                {
                    BoughtRecord.SOH = BoughtRecord.SOH - QtyNeeded;
                    PurchaseCost = PurchaseCost + QtyNeeded * BoughtRecord.UnitPrice;
                    DBAccess.TransUpdate(BoughtRecord);
                    QtyNeeded = 0;
                }
            }
            DateTime TransDate = BoughtRecord.TranDate;
            // Difference in days, hours, and minutes.
            TimeSpan ts = dt - TransDate;
            // Difference in days.
            int DaysHeld = ts.Days;
            //            if (dt == new DateTime(2002, 1, 10))
            //                dt = new DateTime(2002, 1, 10);
            DBAccess.TransRecords myTransRecord = new DBAccess.TransRecords();
            myTransRecord.ASXCode = ASXCode;
            myTransRecord.TranDate = dt;
            myTransRecord.BuySell = "Sell";
            myTransRecord.UnitPrice = Price;
            myTransRecord.TransQty = Qty;
            myTransRecord.BrokerageInc = CalcBrokerage(Qty * Price);  // Brokerage includes GST
            myTransRecord.GST = myTransRecord.BrokerageInc / 11;
            myTransRecord.SOH = 0;
            myTransRecord.TradeProfit = Price * Qty - PurchaseCost - myTransRecord.BrokerageInc;
            myTransRecord.DaysHeld = DaysHeld;
            myTransRecord.TransType = TransType;
            myTransRecord.RelatedTransactionID = BoughtRecord.ID;
            myTransRecord.NABOrderNmbr = NABOrderNmbr;
            DBAccess.TransInsert(myTransRecord);
            decimal CashSurplace = Price * Qty - myTransRecord.BrokerageInc;
            UpdateBankBal(Price * Qty - myTransRecord.BrokerageInc, myTransRecord.TradeProfit, dt, simulationRunning);
            return myTransRecord;
        }



        public static void UpdateBankBal(Decimal NewAmount, Decimal TradeProfit, DateTime trnDate, bool runningSimulation)
        {
            DBAccess DB = new DBAccess();
            DBAccess.BankBal myBankBal = null;
      List<DBAccess.BankBal> balList = new List<DBAccess.BankBal>();
            if (DBAccess.GetAllBankBalRecords(trnDate, out balList, string.Empty, runningSimulation, DBAccess.dirn.equals))
                myBankBal = balList[0];
            if (myBankBal == null)
            {
                if (DBAccess.GetAllBankBalRecords(DateTime.MinValue, out balList, string.Empty, runningSimulation, DBAccess.dirn.equals))
          myBankBal = balList[0];
                else
                    myBankBal = new DBAccess.BankBal();
            }
            if (!myBankBal.BalDate.ToShortDateString().Equals(trnDate.ToShortDateString()))
            {
                myBankBal.BalDate = trnDate.Date;
                myBankBal.ID = 0;
            }
            else
                myBankBal.BalDate = myBankBal.BalDate.Date;
            myBankBal.DayTradeProfit = myBankBal.DayTradeProfit + TradeProfit;
            //            myBankBal.DlrDaysInvested = myBankBal.DlrDaysInvested + BoughtRecord.TransQty * BoughtRecord.UnitPrice;
            myBankBal.AcctBal = myBankBal.AcctBal + NewAmount;
            if (myBankBal.AcctBal < 0)
            {
                myBankBal.MarginLoan = myBankBal.MarginLoan - myBankBal.AcctBal;
                myBankBal.AcctBal = 0;
            }
            if (myBankBal.MarginLoan > 0)
            {
                if (myBankBal.MarginLoan >= myBankBal.AcctBal)
                {
                    myBankBal.MarginLoan = myBankBal.MarginLoan - myBankBal.AcctBal;
                    myBankBal.AcctBal = 0;
                }
                else
                {
                    myBankBal.AcctBal = myBankBal.AcctBal - myBankBal.MarginLoan;
                    myBankBal.MarginLoan = 0;
                }
            }
            myBankBal.ROI = 0;
            //myBankBal.TtlDlrSOH = 0;   // Update after the DateChanges
            //            myBankBal.TtlDlrDaysInvested = myBankBal.TtlDlrDaysInvested + myBankBal.DlrDaysInvested;
            //            myBankBal.TtlTradeProfit = myBankBal.TtlTradeProfit + myBankBal.DayTradeProfit;
            myBankBal.TtlDlrSOH = DBAccess.UpdateCurrentSOH(myBankBal, runningSimulation);
            //            TtlDlrsSOH = myBankBal.TtlDlrSOH;
            //            MarginLoanBal = myBankBal.MarginLoan;
            //            AcctBal = myBankBal.AcctBal;
            myBankBal.DlrDaysInvested = 300000 - myBankBal.AcctBal + myBankBal.MarginLoan;
            //            myBankBal.DlrDaysInvested = DlrDaysInvestedToday;
            if (myBankBal.ID != 0)
                DBAccess.BankBalUpdate(myBankBal);
            else
                DBAccess.BankBalInsert(myBankBal, runningSimulation);
        }


        // Check for the Qty of Stock that is applicable to this stock and date
        /*        private DBAccess.DivPaid CheckForDividends(String ASXCode, DateTime dt)
                {
                    DBAccess.DividendHistory dividendHistory = null;
                    if (DB.GetDividendHistory(ASXCode, dt))
                        dividendHistory = DB.GetNextDividendHistory();
                    if (dividendHistory == null)
                        return null;
                    int TtlASXCodeSOH = DB.GetASXCodeSOH(ASXCode, dt);
                    if (TtlASXCodeSOH == 0)
                        return null;
                    DBAccess.DivPaid divPaid = new DBAccess.DivPaid();
                    divPaid.ASXCode = ASXCode;
                    divPaid.DatePaid = dividendHistory.DatePayable;
                    divPaid.DividendPerShare = dividendHistory.Amount;
                    divPaid.QtyShares = TtlASXCodeSOH;
                    divPaid.TtlDividend = TtlASXCodeSOH * dividendHistory.Amount;
                    DB.DivPaidInsert(divPaid);
                    return divPaid;
                }
        */
        private void testGetMn_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DividendImport divImpt = new DividendImport();
            divImpt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ImportDailyPrices impPrices = new ImportDailyPrices();
            impPrices.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ImportRecentPrices impRecentPrices = new ImportRecentPrices();
            impRecentPrices.Show();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void DisplayGrid_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
      bool runningSimulation = false;
      List<DBAccess.TransImport> transImportList = new List<DBAccess.TransImport>();
            //DBAccess.TransImport newImport = new DBAccess.TransImport();
            DBAccess.TransRecords TransRecords = new DBAccess.TransRecords();
            if (!DBAccess.GetAllTransImpRecords(out transImportList))
                return;
            DBAccess.BankBal bankBal = new DBAccess.BankBal();
            bankBal.BalDate = new DateTime(2008, 9, 23);
            bankBal.AcctBal = (Decimal)410022.28;
            DBAccess.BankBalInsert(bankBal, runningSimulation);
            foreach (DBAccess.TransImport newImport in transImportList)
            {
                
              
                if (newImport.BuySell == "Buy")
                {
                    BuyTransaction(newImport.ASXCode, newImport.TransQty, newImport.UnitPrice, newImport.TranDate, "Initial", newImport.NABOrderNmbr, runningSimulation);
                }
                else
                {
          List<DBAccess.TransRecords> list = null;
          if (!DBAccess.GetAllTransRecords(newImport.ASXCode, DateTime.MinValue, out list, DBAccess.TransRecordsFieldList, string.Empty, false ))
                    {
                        const string message = "I don't have any to sell";
                        const string caption = "Over Sold";
                        var result = MessageBox.Show(message, caption,
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Error);

                        return;
                    }
                    else
                    {
                        // Sellls  ------------------------------------------------------------
                        if (newImport.ASXCode == "QBE")
                            ;
                        if (transImportList.Count > 0)  // get transaction where we bought these
                        {
                            SellTransaction(newImport.ASXCode, newImport.TransQty, newImport.UnitPrice, newImport.TranDate, TransRecords, "Initial", runningSimulation);
                        }
                    }
                }
            }
        }

        private void BtnGenerateSuggestions_Click(object sender, EventArgs e)
        {

            FindSuggestions findSuggestions = new FindSuggestions();
            DBAccess.PrepareForSuggestions();
            findSuggestions.CheckAllCompanies(true);
            FillGrid();

        }

        private void BtnImportTransactions_Click(object sender, EventArgs e)
        {
            CommonFunctions.ImportPrices();
        }

        private void DgvSuggestedSells_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnSuggestedSells_Click(object sender, EventArgs e)
        {

        }

    private string getfilename(string fileType)
    {
      CSVopenFileDialog.Title = "Load CSV File";
      CSVopenFileDialog.Filter = string.Format("{0} (*.csv)|*.csv|Any File (*.*)|*.*", fileType);
      CSVopenFileDialog.InitialDirectory = @"c:\Users\Ray\Downloads";
      CSVopenFileDialog.FileName = String.Empty;
      if (CSVopenFileDialog.ShowDialog() == DialogResult.OK)
        return  CSVopenFileDialog.FileName ;
      return string.Empty;
    }
    private void importNABTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string ASXCodeinError = string.Empty;
      string filename = getfilename("Confirmation Notes");
      if (string.IsNullOrEmpty(filename))
      {
        MessageBox.Show("Unable to open file selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {

        using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
        {
          string inputLine;
          //CommonFunctions.ImportTransactions(false);
          inputLine = sr.ReadLine();
          if (string.IsNullOrEmpty(inputLine))
            return;
          // header line
          string[] hdr = inputLine.Split(',');
          while ((inputLine = sr.ReadLine()) != null)
          {
            if (String.IsNullOrEmpty(inputLine) || inputLine.Length < 2)
              continue;
            string[] flds = inputLine.Replace('"', '#').Replace("#","").Split(',');
            DBAccess.TransRecords tx = new DBAccess.TransRecords();
            tx.TranDate = DateTime.Parse(flds[0]);
            tx.NABOrderNmbr = flds[1].Trim();       
            tx.ASXCode = flds[2].Substring(0, flds[2].IndexOf('.')).Trim();
            tx.TransQty = (int) decimal.Parse(flds[3]);
            tx.TransType = flds[4].Trim();
            tx.BuySell = flds[4].Trim();
            tx.UnitPrice = decimal.Parse(flds[5]);
            tx.BrokerageInc = decimal.Parse(flds[6]);
            tx.SellConfirmation = string.Empty;
            //  Is this record a duplicate? use the confirmation number
            List<DBAccess.TransRecords> list;
            List<PgSqlParameter> paramList = new List<PgSqlParameter>();
            paramList.Add(new PgSqlParameter("@P1", tx.NABOrderNmbr));
            if (DBAccess.GetTransRecords(paramList, out list, DBAccess.TransRecordsFieldList, " AND trn_nabordernumber = @P1 ", string.Empty, false))
              continue;

            tx.SOH = tx.TransQty;
            if (string.IsNullOrEmpty(tx.SellConfirmation))
              tx.SellConfirmation = string.Empty;
            DBAccess.DBInsert(tx, "transrecord", typeof(DBAccess.TransRecords));
          }
          // Get all Sells with SOH > 0 (ie. not matched with a Buy)
          List<DBAccess.TransRecords> sellList;
          if (!DBAccess.GetTransRecords(null, out sellList, DBAccess.TransRecordsFieldList, " AND trn_buysell = 'Sell' AND trn_soh > 0 ", " ORDER BY trn_transdate", false))
            return;

          foreach (DBAccess.TransRecords tx in sellList)
          {
            ASXCodeinError = tx.ASXCode;
            Console.WriteLine(tx.ASXCode + ";" + tx.TranDate.ToShortDateString());
            if (ASXCodeinError.Contains("MQG"))
            { }
            decimal profit = 0M;
            decimal daysHeld = 0;
            // Get Buys for this ASX Code where SOH > 0 in descending order and Buy is earlier than the Sell
            List<DBAccess.TransRecords> buyList;
            int remainingSOH = tx.TransQty;

            // Change this to get for all the 'suggested sells' and use those in descending order
            if (DBAccess.GetAllTransRecords(tx.ASXCode, tx.TranDate, out buyList, DBAccess.TransRecordsFieldList, " AND trn_SOH > 0 AND trn_buysell = 'Buy'", false, DBAccess.dirn.lessThanEquals))
            {
              DBAccess.TransRecords buyRec = buyList.Find(delegate (DBAccess.TransRecords r1) { return !string.IsNullOrEmpty(r1.SellConfirmation) && r1.SellConfirmation.Trim() == tx.NABOrderNmbr.Trim(); });
              if (buyRec != null)
              {
                DBAccess.TransRecords sell = tx;
                if (matchedOnConNr(ref sell, buyRec, out remainingSOH))
                  if (remainingSOH <= 0)
                    continue;
              }
            }


            if (DBAccess.GetAllTransRecords(tx.ASXCode, tx.TranDate, out buyList, DBAccess.TransRecordsFieldList, " AND trn_SOH > 0 AND trn_buysell = 'Buy'", false, DBAccess.dirn.lessThanEquals))
            {
              buyList = buyList.OrderByDescending(x => x.TranDate).ToList();
              // Buy records found
              foreach (DBAccess.TransRecords buy in buyList)
              {
                if (remainingSOH <= 0)
                  break;
                if (buy.SOH <= 0)
                  continue;
                // Add Related Buy/Sell transaction
                if (remainingSOH < buy.SOH)
                {
                  addRelatedBuySell(buy, tx, remainingSOH, out profit, out daysHeld);
                  // This is the last buy we have to match with -  Update the Sell to the remaing SOH and put the Buy Id into the RelatedTransId
                  tx.TradeProfit += profit;
                  tx.DaysHeld += daysHeld;
                  tx.SOH = 0;
                  DBAccess.DBUpdate(tx, "transrecord", typeof(DBAccess.TransRecords));

                  buy.SOH -= remainingSOH;
                  DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
                  remainingSOH = 0;

                }
                else if (buy.SOH == remainingSOH)
                {
                  addRelatedBuySell(buy, tx, remainingSOH, out profit, out daysHeld);
                  remainingSOH -= buy.SOH;
                  buy.SOH = 0;
                  DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
                  tx.RelatedTransactionID = buy.ID;
                  tx.TradeProfit += profit;
                  tx.DaysHeld += daysHeld;
                  tx.SOH = 0;
                  DBAccess.DBUpdate(tx, "transrecord", typeof(DBAccess.TransRecords));
                }
                else  // remainingSOH > buy.SOH
                {
                  addRelatedBuySell(buy, tx, buy.SOH, out profit, out daysHeld);

                  remainingSOH -= buy.SOH;
                  tx.SOH -= buy.SOH;
                  tx.DaysHeld += daysHeld;
                  tx.TradeProfit += profit;
                  DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
                  // The sell sold more than this buy so create a relatedSell record
                  buy.SOH = 0;
                  DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
                }
              }
            }
            
            if (remainingSOH > 0)
            {
              //  WHAT DO WE DO NOW??
              throw new DBAccess.DatabaseException(string.Format("Stock has been oversold", ASXCodeinError));

            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format("Unable to open selected file {0} ", ex.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
    }

    private bool matchedOnConNr(ref DBAccess.TransRecords sell, DBAccess.TransRecords buy, out int remainingSOH)
    {
      remainingSOH = sell.TransQty;
      decimal profit = 0M;
      decimal daysHeld = 0M;
      if (remainingSOH < buy.SOH)
      {
        addRelatedBuySell(buy, sell, remainingSOH, out profit, out daysHeld);
        // This is the last buy we have to match with -  Update the Sell to the remaing SOH and put the Buy Id into the RelatedTransId
        sell.TradeProfit += profit;
        sell.DaysHeld += daysHeld;
        sell.SOH = 0;
        DBAccess.DBUpdate(sell, "transrecord", typeof(DBAccess.TransRecords));

        buy.SOH -= remainingSOH;
        DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
        remainingSOH = 0;

      }
      else if (buy.SOH == remainingSOH)
      {
        addRelatedBuySell(buy, sell, remainingSOH, out profit, out daysHeld);
        remainingSOH -= buy.SOH;
        buy.SOH = 0;
        DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
        sell.RelatedTransactionID = buy.ID;
        sell.TradeProfit += profit;
        sell.DaysHeld += daysHeld;
        sell.SOH = 0;
        DBAccess.DBUpdate(sell, "transrecord", typeof(DBAccess.TransRecords));
      }
      else  // remainingSOH > buy.SOH
      {
        addRelatedBuySell(buy, sell, buy.SOH, out profit, out daysHeld);

        remainingSOH -= buy.SOH;
        sell.SOH -= buy.SOH;
        sell.DaysHeld += daysHeld;
        sell.TradeProfit += profit;
        DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
        // The sell sold more than this buy so create a relatedSell record
        buy.SOH = 0;
        DBAccess.DBUpdate(buy, "transrecord", typeof(DBAccess.TransRecords));
      }
      return remainingSOH <= 0;
    }

    private void x(DBAccess.TransRecords sell, DBAccess.TransRecords buy)
    {

    }
    private static void addRelatedBuySell(DBAccess.TransRecords buy, DBAccess.TransRecords sell, int qty, out decimal profit, out decimal daysHeld)
    {
      DBAccess.RelatedBuySellTrans rbs = new DBAccess.RelatedBuySellTrans();
      rbs.BuyId = buy.ID;
      rbs.SellId = sell.ID;

      rbs.TransQty = qty;
      rbs.TradeProfit = Decimal.Round(rbs.TransQty * (sell.UnitPrice - buy.UnitPrice), 2);
      rbs.DaysHeld = Decimal.Round((Decimal) (sell.TranDate.Date - buy.TranDate.Date).TotalDays * qty / sell.TransQty, 2);
      profit = rbs.TradeProfit;
      daysHeld = rbs.DaysHeld;
      rbs.SaleBrokerage = Decimal.Round((sell.BrokerageInc * qty / sell.TransQty * 10 / 11) + (buy.BrokerageInc * qty / buy.TransQty * 10 / 11), 2);
      DBAccess.DBInsert(rbs, "relatedbuyselltrans", typeof(DBAccess.RelatedBuySellTrans));

    }

    private void importNABPricesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      //CommonFunctions.ImportPrices();
      string filename = string.Format("c://Users//{0}////Downloads//WatchlistData.csv", Environment.UserName); // getfilename("WatchlistData");
      
      if (string.IsNullOrEmpty(filename))
      {
        MessageBox.Show("Unable to open file selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {

        List<string> exceptionCodes = new List<string>();
        using (System.IO.StreamReader sr = new System.IO.StreamReader(System.IO.File.Open(filename, System.IO.FileMode.Open)))
        {
          string inputLine = sr.ReadLine();
          if (string.IsNullOrEmpty(inputLine))
            return;
          // header line
          string[] hdr = inputLine.Split(',');
          while ((inputLine = sr.ReadLine()) != null)
          {
            if (String.IsNullOrEmpty(inputLine) || inputLine.Length < 2)
              continue;
            string[] flds = inputLine.Split(',');
            if (flds.Length > 13)     // in case there are commas within any of the field values
            {
              exceptionCodes.Add(flds[0]);
              continue;
            }
            DBAccess.ASXPriceDate tx = DBAccess.GetSpecificASXPriceRecord(flds[0], DateTime.Today);
            if (tx == null)
              tx = new DBAccess.ASXPriceDate();
            tx.PriceDate = DateTime.Today;
            tx.ASXCode = flds[0];
            Console.WriteLine(">>" + tx.ASXCode + "<<");
            if (tx.ASXCode == "RGC")
            { }
            tx.PrcClose = decimal.Parse(flds[5]);
            tx.PrcOpen = decimal.Parse(flds[9]);
            tx.PrcLow = flds[10] == "--" ? 0M : decimal.Parse(flds[10]);
            tx.PrcHigh = flds[11] == "--" ? 0M : decimal.Parse(flds[11]);
            tx.Volume = int.Parse(flds[12]);
            if (tx.ID == 0)
              DBAccess.DBInsert(tx, "asxpricedate", typeof(DBAccess.ASXPriceDate));
            else
              DBAccess.DBUpdate(tx, "asxpricedate", typeof(DBAccess.ASXPriceDate));
          }

        }
        if (exceptionCodes.Count > 0)
        {
          string eList = string.Join("\r\n", exceptionCodes.ToArray());
          MessageBox.Show(string.Format("Unable to import data for the following ASX Codes - \r\n {0}", eList), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        if (System.IO.File.Exists(filename))
          System.IO.File.Delete(filename);
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format("Unable to read file selected - {0}", ex.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
    }
        private void timer1_Tick(object sender, EventArgs e)
        {
      //DBAccess DB = new DBAccess();
      //DBAccess.BankBal bal;
      //DB.GetBankBal(new DateTime(1900, 1, 1));
      //bal = DB.GetNextBankBal();
      //TbSOH.Text = bal.TtlDlrSOH.ToString();
      //TbProfit.Text = (bal.TtlTradeProfit + bal.TtlDividendEarned).ToString();
    }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //OleDbDataReader PriceHistReader;
            //string myConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";

            //// Define the database query    
            //string mySelectQuery = "SELECT PriceDate, PrcClose FROM ASXPriceDate where ASXCode = 'AMP' order by PriceDate";


            //// Create a database connection object using the connection string    
            //OleDbConnection myConnection = new OleDbConnection(myConnectionString);

            //// Create a database command on the connection using query    
            //OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConnection);
            //myConnection.Open();
            //PriceHistReader = myCommand.ExecuteReader();
            DateTime MinDate = new DateTime();
            DateTime MaxDate = new DateTime();
            Decimal MinPrice = 0;
            Decimal MaxPrice = 0;
            int cnt = 0;

      String ASXCode = string.Empty;
      using (PgSqlConnection conn = new PgSqlConnection(DBAccess.DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          command.Parameters.Add("@P1", ASXCode);
          command.CommandText = string.Format("SELECT apd_pricedate, apd_prcclose FROM asxpricedate WHERE apd_datedeleted = @P0 AND apd_ASXCode = @P1");
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
              chart1.Series[0].Points.AddXY(reader.GetDateTime(0), reader.GetDecimal(1));
              if (cnt == 0)
              {
                MinPrice = reader.GetDecimal(1);
                MaxPrice = reader.GetDecimal(1);
                MinDate = reader.GetDateTime(0);
                MaxDate = reader.GetDateTime(0);
              }
              else
                MaxDate = reader.GetDateTime(0);
              if (reader.GetDecimal(1) > MaxPrice)
                MaxPrice = reader.GetDecimal(1);
            }

          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DBAccess.DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }

            //            chart1.Series[0].Points.;
            //            chart1.Show();
            // set chart data source - the data source must implement IEnumerable
            //chart1.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        private void importRecentPricesToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

    private void GLCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      EditGeneralLedger frm = new EditGeneralLedger();
      frm.ShowDialog();
    }

    private void dividendsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

 
    private void toMYOBToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ExportBuysSellsToMYOB frm = new ExportBuysSellsToMYOB();
      frm.ShowDialog();

    }

    private void suggestionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FrmSuggestions frm = new FrmSuggestions();
      frm.ShowDialog();
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      // Get list of ASX Companies required
      List<DBAccess.ASXPriceDate> coList = new List<DBAccess.ASXPriceDate>();
      if (!DBAccess.GetAllPrices(new List<PgSqlParameter>(),  out coList, " DISTINCT apd_asxcode  ", string.Empty, string.Empty))
        return;
      // foreach company, get dividend history
      int counter = 0;
      foreach (DBAccess.ASXPriceDate rec in coList)
      {
        Console.WriteLine(">" + rec.ASXCode + "<");
        ImportDividendHistory.ImportDividends(rec.ASXCode);
        if (counter > 100)
          counter = 0;
        backgroundWorker1.ReportProgress(counter++);
      }


    }

    private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      // Change the value of the ProgressBar to the BackgroundWorker progress.
      progressBar.Value = e.ProgressPercentage;
      // Set the text.
      this.Text = e.ProgressPercentage.ToString();
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      MessageBox.Show("Dividend History Import Completed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
      progressBar.Visible = false;

    }

    private void toolStripMenuItemImportDivHistory_Click(object sender, EventArgs e)
    {
      if (backgroundWorker1.IsBusy)
        return;
      progressBar.Visible = true;
      backgroundWorker1.RunWorkerAsync();
    }

    private void enterConfirmationNrToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FrmEnterSellConfrmationNr frm = new FrmEnterSellConfrmationNr();
      frm.ShowDialog();
    }

    private void gatherStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FrmGatherStats frm = new FrmGatherStats();
      frm.ShowDialog();

    }

    // ******************************  Import Price History *************************************


    private void importPriceHistoryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (backgroundWorkerPrcHst.IsBusy)
        return;
      importPriceHistoryToolStripMenuItem.Enabled = false;
      
      string[] files = Directory.GetFiles(getPriceHistoryFolder());
      if (files.Length <= 0)
      {
        MessageBox.Show("No files found in selected directory");
        return;
      }
      progressBar.Visible = true;
      statusLabel.Visible = true;
      WorkState ws = new WorkState();
      ws.filenames = files;
      startProgressBarMarquee();
      statusLabel.Text = "Loading Files ...";
      backgroundWorkerPrcHst.RunWorkerAsync(ws);

    }

    internal class WorkState
    {
      public string status { get; set; }
      public string[] filenames { get; set; }
    }

    private void backgroundWorkerPrcHst_DoWork(object sender, DoWorkEventArgs e)
    {
      WorkState ws = e.Argument as WorkState;
      string[] fileNames = ws.filenames;
      
      // foreach file, import data
      foreach (string filename in fileNames)
      {
        Console.WriteLine(">" + filename + "<");
        ImportPriceHistory.ImportPriceFile(filename);
      }
      e.Result = true;
    }

    private void backgroundWorkerPrcHst_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      WorkState ws = e.UserState as WorkState;
      statusLabel.Text = ws.status;

    }

    private void backgroundWorkerPrcHst_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      endProgressBarMarquee();
      if ((bool)e.Result)
        statusLabel.Text = "Complete";
      else
        statusLabel.Text = "Error loading files";
      importPriceHistoryToolStripMenuItem.Enabled = true;

    }

    private void startProgressBarMarquee()
    {
      progressBar.Style = ProgressBarStyle.Marquee;
      progressBar.Minimum = 0;
      progressBar.Value = 0;
      progressBar.Visible = true;
      progressBar.Minimum = 1;
      progressBar.Maximum = 100;
      progressBar.Step = 1;
    }

    private void endProgressBarMarquee()
    {
      progressBar.Style = ProgressBarStyle.Blocks;
      progressBar.MarqueeAnimationSpeed = 0;
      progressBar.Value = progressBar.Maximum;
    }
    private string getPriceHistoryFolder()
    {
      CSVopenFileDialog.Title = "Load Price History Files in Folder";
      CSVopenFileDialog.Filter = string.Format("{0} Any File (*.*)|*.*", string.Empty);
      CSVopenFileDialog.InitialDirectory = string.Format(@"c://Users//{0}////Downloads", Environment.UserName);
      CSVopenFileDialog.FileName = String.Empty;
      if (CSVopenFileDialog.ShowDialog() == DialogResult.OK)
        return Path.GetDirectoryName(CSVopenFileDialog.FileName);
      return string.Empty;
    }

    //  *****************************  Company Details  ******************************************

    private void editCompanyDetailsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FrmEditCompanyDetails frm = new FrmEditCompanyDetails();
      frm.ShowDialog();

    }

    //  **********************************  Update Brokerage  ***************************************
    private void btnUpdateBrokerage_Click(object sender, EventArgs e)
    {
      List<DBAccess.RelatedBuySellTrans> rbsList = new List<DBAccess.RelatedBuySellTrans>();
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      if (!DBAccess.GetAllRelated(paramList, out rbsList, DBAccess.RelatedTransFieldList, string.Empty, string.Empty))
        return;
      foreach (DBAccess.RelatedBuySellTrans rec in rbsList)
      {
        // Get the buy 
        List<PgSqlParameter> buyParams = new List<PgSqlParameter>();
        buyParams.Add(new PgSqlParameter("@P2", rec.BuyId));
        List<DBAccess.TransRecords> buyList = new List<DBAccess.TransRecords>();
        if (!DBAccess.GetTransRecords(buyParams, out buyList, DBAccess.TransRecordsFieldList, " AND trn_ID = @P2 ",  string.Empty, false))
          continue;
        DBAccess.TransRecords buy = buyList[0];

        // & sell record 
        List<PgSqlParameter> sellParams = new List<PgSqlParameter>();
        sellParams.Add(new PgSqlParameter("@P3", rec.SellId));
        List<DBAccess.TransRecords> sellList = new List<DBAccess.TransRecords>();
        if (!DBAccess.GetTransRecords(sellParams, out sellList, DBAccess.TransRecordsFieldList, " AND trn_ID = @P3 ", string.Empty, false))
          continue;
        DBAccess.TransRecords sell = sellList[0];
        // and then update the brokerage
        rec.SaleBrokerage = Decimal.Round((sell.BrokerageInc * rec.TransQty / sell.TransQty * 10 / 11) + (buy.BrokerageInc * rec.TransQty / buy.TransQty * 10 / 11), 2);
        DBAccess.DBUpdate(rec, "relatedbuyselltrans", typeof(DBAccess.RelatedBuySellTrans));

      }
    }
  }
}

