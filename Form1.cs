using System;
using System.Collections.Generic;
using System.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
//using System.Web.UI.DataVisualization.Charting;


    // ***************************
    //  Let's document ray 5
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
            DB = new DBAccess();
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
            OleDbConnection connectionPrice = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connectionPrice.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            string query = "SELECT ASXCode, TransQty, CurrPrc, UnitPrice, ROI, CurrProfit, TargetProfit From TodaysTrades where BuySell = 'Sell'";
            using (OleDbConnection conn = new OleDbConnection(connectionPrice.ConnectionString))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DgvSuggestedSells.DataSource = ds.Tables[0];
                }
            }
            query = "SELECT * From TodaysTrades where BuySell = 'Buy'";
            using (OleDbConnection conn = new OleDbConnection(connectionPrice.ConnectionString))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DgvSuggestedBuys.DataSource = ds.Tables[0];
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
                                RangeTest();
                            }
                        }
                    }
                }
            }
        }

        private void RangeTest()
        {
            DBAccess.ASXPriceDate ASXPriceDate;
            DBAccess.TransRecords TransRecords;
            DB = new DBAccess();
            DB.connection.Open();
            DB.PrepareForSimulation();
            DB.GetAllPrices(null, StartDate);
            DateTime lastDate = StartDate;
            //  Set up the starting Account Bal
            DBAccess.BankBal bankBal = new DBAccess.BankBal();
            bankBal.BalDate = lastDate;
            bankBal.AcctBal = StartBal;
            DB.BankBalInsert(bankBal);
            Decimal DayDivTotal = (Decimal)0.0;

            while ((ASXPriceDate = DB.GetNextPriceDate()) != null)
            {
                DBAccess.DividendHistory dividendHistory = null;
                if (DB.GetMostRecentDividend(ASXPriceDate.ASXCode, lastDate))
                    dividendHistory = DB.GetNextDividendHistory();
                if (dividendHistory == null)
                    continue;
                //                if (dividendHistory.GrossDividend > ASXPriceDate.PrcClose * (Decimal).02)
                //                    continue;

                //                if (ASXPriceDate.ASXCode != "LL")
                //                   continue;
                int ID = ASXPriceDate.ID;
                DBAccess.DivPaid dp = CommonFunctions.CheckForDividends(ASXPriceDate.ASXCode, ASXPriceDate.PriceDate);
                if (dp != null)
                    DayDivTotal = dp.TtlDividend + DayDivTotal;
                if (ASXPriceDate.PriceDate > lastDate)
                {
                    if (MaxMarginLoan < bankBal.MarginLoan)
                    {
                        MaxMarginLoan = bankBal.MarginLoan;
                        CorrespondingSOH = bankBal.TtlDlrSOH;
                    }

                    if (ASXPriceDate.PriceDate == EndDate)
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
                        DB.SimulationPerformanceInsert(Performance);
                        MaxMarginLoan = 0;
                        CorrespondingSOH = 0;
                        return;
                    }
                    if (DB.GetBankBal(lastDate))
                        bankBal = DB.GetNextBankBal();

                    bankBal.TtlDlrSOH = DB.UpdateCurrentSOH(bankBal);
                    bankBal.TtlTradeProfit = bankBal.TtlTradeProfit + bankBal.DayTradeProfit;
                    //                    bankBal.MarginLoan = MarginLoanBal;
                    //                    bankBal.AcctBal = AcctBal;
                    bankBal.BalDate = lastDate;
                    bankBal.DlrDaysInvested = StartBal - bankBal.AcctBal + bankBal.MarginLoan;
                    bankBal.TtlDlrDaysInvested = bankBal.TtlDlrDaysInvested + bankBal.DlrDaysInvested;
                    bankBal.DayDividend = DayDivTotal;
                    bankBal.TtlDividendEarned = bankBal.TtlDividendEarned + DayDivTotal;
                    DB.BankBalUpdate(bankBal);

                    // Now move forward & Reset the Day stuff
                    lastDate = ASXPriceDate.PriceDate;
                    DayDivTotal = (Decimal)0.0;
                    bankBal.BalDate = lastDate;
                    bankBal.DayTradeProfit = 0;
                    bankBal.DlrDaysInvested = 0;
                    bankBal.TtlDlrDaysInvested = 0;
                    bankBal.DayDividend = 0;
                    DB.BankBalInsert(bankBal);
                }
                bool didSell = false;
                if (!DB.GetTransRecords(ASXPriceDate.ASXCode, new DateTime(1900, 1, 1)))
                    continue;
                else
                {
                    // Sellls  ------------------------------------------------------------
                    while ((TransRecords = DB.GetNextTransRecords()) != null)  // get transaction where we bought these
                    {
                        Decimal SellPrice = 0;
                        DateTime TransDate = TransRecords.TranDate;
                        // Difference in days, hours, and minutes.
                        TimeSpan ts = lastDate - TransDate;
                        // Difference in days.
                        Double DaysHeld = (Double)ts.Days;

                        Decimal TargetPrice = 0;
                        TargetPrice = TransRecords.UnitPrice * (Decimal)(1.005 + ((Double)TargetSellReturn * Math.Sqrt(DaysHeld)));
                        if (ASXPriceDate.PrcOpen >= TargetPrice)
                        {
                            SellPrice = ASXPriceDate.PrcOpen;
                            if (TransRecords.SOH <= 0)
                                continue;
                            DBAccess.TransRecords SellTrn = SellTransaction(ASXPriceDate.ASXCode, TransRecords.TransQty, SellPrice, lastDate, TransRecords, "SellOnOpen4Return");
                            didSell = true;
                            Decimal NewBuyTarget = ASXPriceDate.PrcOpen * (Decimal)(0.995 - ((Double)TargetBuyReturn * Math.Sqrt(1)));
                            if (ASXPriceDate.PrcLow < NewBuyTarget && MaxBuys)  //  If MayBBuys && Can only buy during the remainder of the day
                            {
                                int BuyQty = CommonFunctions.GetBuyQty(bankBal, NewBuyTarget, MarginLendingBarrier);
                                BuyTransaction(ASXPriceDate.ASXCode, BuyQty, NewBuyTarget, lastDate, "SDBuyAfterSell");
                            }
                            break;
                        }
                        if (ASXPriceDate.PrcHigh >= TargetPrice)
                        {
                            if (TransRecords.SOH <= 0)
                                continue;
                            DBAccess.TransRecords SellTrn = SellTransaction(ASXPriceDate.ASXCode, TransRecords.TransQty, TargetPrice, lastDate, TransRecords, "SellOnDayHigh");
                            didSell = true;
                        }
                    }
                }
                if (didSell)
                    continue;

                // Buys ------------------------------------

                //  Buy on margin below last sell - 
                if (DB.SetupLastSellRecords(ASXPriceDate.ASXCode))
                {
                    if ((TransRecords = DB.GetLastTransRecords()) != null)
                    {
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
                            if (ASXPriceDate.PrcLow <= TargetPrice)
                            {
                                if (ASXPriceDate.PrcOpen <= TargetPrice)
                                {
                                    BuyPrice = ASXPriceDate.PrcOpen;
                                    BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                    TransRecords = BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnOpenBelowSell");

                                    TargetPrice = TransRecords.UnitPrice * (Decimal)(1.005 + ((Double)TargetSellReturn * Math.Sqrt(1)));
                                    if (ASXPriceDate.PrcHigh >= TargetPrice && MaxBuys)
                                    {
                                        if (TransRecords.SOH <= 0)
                                            continue;
                                        TransRecords = SellTransaction(ASXPriceDate.ASXCode, BuyQty, TargetPrice, lastDate, TransRecords, "SellSameDayOnBuy");
                                        continue;
                                    }
                                }
                                else
                                    BuyPrice = TargetPrice;
                                //Transaction Size
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnLow4Day");
                                continue;
                            }
                        }
                    }
                }

                // We don't have any so lets buy on a 5 day low or if very close to a Dividend
                if (DB.GetTransRecords(ASXPriceDate.ASXCode, new DateTime(1900, 1, 1)))
                {
                    Decimal BuyPrice = 0;
                    if ((TransRecords = DB.GetNextTransRecords()) == null)
                    {
                        //  Buy within 10 days of Dividend - 
                        DBAccess.DividendHistory DivHis = new DBAccess.DividendHistory();
                        if (DB.GetNextDividend(ASXPriceDate.ASXCode, ASXPriceDate.PriceDate))
                        {
                            if ((DivHis = DB.GetNextDividendHistory()) != null && ChaseDividends)  // Only do this is chasing Dividends
                            {
                                if (DateTime.Compare(DivHis.ExDividend, ASXPriceDate.PriceDate.AddDays(10)) < 0)
                                {
                                    //Transaction Size
                                    BuyPrice = ASXPriceDate.PrcOpen;
                                    int BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                    DBAccess.TransRecords BuyTrn = BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyNearDividend");
                                    continue;
                                }
                            }
                        }

                        if (ASXPriceDate.PrcLow <= ASXPriceDate.Day5Min * AddBuyMrgn &&
                            ASXPriceDate.Day5Min > ASXPriceDate.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            int BuyQty = 0;
                            if (ASXPriceDate.PrcOpen <= ASXPriceDate.Day5Min * AddBuyMrgn)
                            {
                                BuyPrice = ASXPriceDate.PrcOpen;
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                DBAccess.TransRecords BuyTrn = BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnOpenDayMin");
                                Decimal TargetPrice = BuyPrice * (Decimal)(1.005 + (.01 * Math.Sqrt(1)));
                                if (ASXPriceDate.PrcHigh > TargetPrice && MaxSells)
                                    SellTransaction(ASXPriceDate.ASXCode, BuyQty, TargetPrice, lastDate, BuyTrn, "SellAfterBuyOnMin");
                            }
                            else if (ASXPriceDate.PrcLow <= ASXPriceDate.Day5Min * AddBuyMrgn && bankBal.TtlDlrSOH > 0)
                            {
                                if (bankBal.MarginLoan / bankBal.TtlDlrSOH > (Decimal)MarginLoanRebuyLimit)
                                    continue;
                                BuyPrice = ASXPriceDate.Day5Min * AddBuyMrgn;
                                //Transaction Size
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnDayMin");
                            }
                        }
                    }
                    else  // already have some - doing rebuy
                    {
                        if (ASXPriceDate.PrcLow <= ASXPriceDate.Day5Min * AddBuyMrgn &&
                            ASXPriceDate.Day5Min > ASXPriceDate.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            if (ASXPriceDate.PrcOpen <= ASXPriceDate.Day5Min * AddBuyMrgn)
                                BuyPrice = ASXPriceDate.PrcOpen;
                            else
                                BuyPrice = ASXPriceDate.Day5Min * AddBuyMrgn;
                            if (BuyPrice < (Decimal)RebuyMargin * TransRecords.UnitPrice && bankBal.TtlDlrSOH > 0)
                            {
                                if (bankBal.MarginLoan / bankBal.TtlDlrSOH > (Decimal)MarginLoanRebuyLimit)
                                    continue;
                                int BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuyTransaction(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "Rebuy");
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
            DB.connection.Close();
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
            DB.connection.Open();
            DB.GetAllPrices(null, lastDateTime);
            while ((ASXPriceDate = DB.GetNextPriceDate()) != null)
            {
                DateTime PriceDate = ASXPriceDate.PriceDate;
                if (lastDateTime != PriceDate)
                {
                    lastDateTime = PriceDate;
                }
                string ASXCode = ASXPriceDate.ASXCode;
                if (ASXCode == null)
                    break;
                ASXPriceDate.Day5Max = DB.GetMaxPrice(7, ASXCode, PriceDate);
                ASXPriceDate.Day30Max = DB.GetMaxPrice(30, ASXCode, PriceDate);
                ASXPriceDate.Day60Max = DB.GetMaxPrice(60, ASXCode, PriceDate);
                ASXPriceDate.Day90Max = DB.GetMaxPrice(90, ASXCode, PriceDate);
                ASXPriceDate.Day5Min = DB.GetMinPrice(7, ASXCode, PriceDate);
                ASXPriceDate.Day30Min = DB.GetMinPrice(30, ASXCode, PriceDate);
                ASXPriceDate.Day60Min = DB.GetMinPrice(90, ASXCode, PriceDate);
                ASXPriceDate.Day90Min = DB.GetMinPrice(90, ASXCode, PriceDate);
                if (ASXPriceDate.Day5Min > 0)
                    ASXPriceDate.Day5Pct = (100 * (ASXPriceDate.Day5Max - ASXPriceDate.Day5Min) / ASXPriceDate.Day5Min);
                if (ASXPriceDate.Day30Min > 0)
                    ASXPriceDate.Day30Pct = (100 * (ASXPriceDate.Day30Max - ASXPriceDate.Day30Min) / ASXPriceDate.Day30Min);
                if (ASXPriceDate.Day60Min > 0)
                    ASXPriceDate.Day60Pct = (100 * (ASXPriceDate.Day60Max - ASXPriceDate.Day60Min) / ASXPriceDate.Day60Min);
                if (ASXPriceDate.Day90Min > 0)
                    ASXPriceDate.Day90Pct = (100 * (ASXPriceDate.Day90Max - ASXPriceDate.Day90Min) / ASXPriceDate.Day90Min);
                DB.ASXprcUpdate(ASXPriceDate);
            }
            DB.connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetMinMaxs();
        }

        private void setMinMaxS(object sender, EventArgs e)
        {
            //SetLimitTriggers(new DateTime(2015, 8, 18));
        }

        public static DBAccess.TransRecords BuyTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, String TransType)
        {
            return BuyTransaction(ASXCode, Qty, Price, dt, TransType, "");
        }

        public static DBAccess.TransRecords BuyTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, String TransType, String NABOrderNmbr)
        {
            DBAccess DB = new DBAccess();
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
            DB.TransInsert(myTransRecord);
            UpdateBankBal(-(Price * Qty + myTransRecord.BrokerageInc), myTransRecord.TradeProfit, dt);
            DB.GetTransRecords(ASXCode, dt);
            myTransRecord = DB.GetNextTransRecords();
            return myTransRecord;
        }

        public static DBAccess.TransRecords SellTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, DBAccess.TransRecords BoughtRecord, String TransType)
        {
            return SellTransaction(ASXCode, Qty, Price, dt, BoughtRecord, "", TransType);
        }

        public static DBAccess.TransRecords SellTransaction(String ASXCode, int Qty, Decimal Price, DateTime dt, DBAccess.TransRecords BoughtRecord, String TransType, String NABOrderNmbr)
        {
            DBAccess DB = new DBAccess();
            if (BoughtRecord == null)
            {
                DB.GetTransRecords(ASXCode, new DateTime(1900, 1, 1));
                BoughtRecord = DB.GetNextTransRecords();
            }

            Decimal PurchaseCost = 0;
            int QtyNeeded = Qty;
            while (QtyNeeded > 0)
            {
                if (QtyNeeded > BoughtRecord.SOH)
                {
                    QtyNeeded = QtyNeeded - BoughtRecord.SOH;
                    PurchaseCost = PurchaseCost + BoughtRecord.SOH * BoughtRecord.UnitPrice;
                    BoughtRecord.SOH = 0;
                    DB.TransUpdate(BoughtRecord);
                    BoughtRecord = DB.GetNextTransRecords();
                }
                else
                {
                    BoughtRecord.SOH = BoughtRecord.SOH - QtyNeeded;
                    PurchaseCost = PurchaseCost + QtyNeeded * BoughtRecord.UnitPrice;
                    DB.TransUpdate(BoughtRecord);
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
            DB.TransInsert(myTransRecord);
            decimal CashSurplace = Price * Qty - myTransRecord.BrokerageInc;
            UpdateBankBal(Price * Qty - myTransRecord.BrokerageInc, myTransRecord.TradeProfit, dt);
            return myTransRecord;
        }



        public static void UpdateBankBal(Decimal NewAmount, Decimal TradeProfit, DateTime trnDate)
        {
            DBAccess DB = new DBAccess();
            DBAccess.BankBal myBankBal = null;
            if (DB.GetBankBal(trnDate))
                myBankBal = DB.GetNextBankBal();
            if (myBankBal == null)
            {
                if (DB.GetBankBal(new DateTime(1900, 1, 1)))
                    myBankBal = DB.GetNextBankBal();
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
            myBankBal.TtlDlrSOH = DB.UpdateCurrentSOH(myBankBal);
            //            TtlDlrsSOH = myBankBal.TtlDlrSOH;
            //            MarginLoanBal = myBankBal.MarginLoan;
            //            AcctBal = myBankBal.AcctBal;
            myBankBal.DlrDaysInvested = 300000 - myBankBal.AcctBal + myBankBal.MarginLoan;
            //            myBankBal.DlrDaysInvested = DlrDaysInvestedToday;
            if (myBankBal.ID != 0)
                DB.BankBalUpdate(myBankBal);
            else
                DB.BankBalInsert(myBankBal);
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
            DBAccess.TransImport newImport = new DBAccess.TransImport();
            DBAccess.TransRecords TransRecords = new DBAccess.TransRecords();
            if (!DB.GetTransImpRecords())
                return;
            DBAccess.BankBal bankBal = new DBAccess.BankBal();
            bankBal.BalDate = new DateTime(2008, 9, 23);
            bankBal.AcctBal = (Decimal)410022.28;
            DB.BankBalInsert(bankBal);
            while (true)
            {
                newImport = DB.GetNextTransImpRecords();
                if (newImport == null)
                    break;
                if (newImport.BuySell == "Buy")
                {
                    BuyTransaction(newImport.ASXCode, newImport.TransQty, newImport.UnitPrice, newImport.TranDate, "Initial", newImport.NABOrderNmbr);
                }
                else
                {
                    if (!DB.GetTransRecords(newImport.ASXCode, new DateTime(1900, 1, 1)))
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
                        if ((TransRecords = DB.GetNextTransRecords()) != null)  // get transaction where we bought these
                        {
                            SellTransaction(newImport.ASXCode, newImport.TransQty, newImport.UnitPrice, newImport.TranDate, TransRecords, "Initial");
                        }
                    }
                }
            }
        }

        private void BtnGenerateSuggestions_Click(object sender, EventArgs e)
        {

            FindSuggestions findSuggestions = new FindSuggestions();
            DBAccess.PrepareForSuggestions();
            findSuggestions.CheckAllCompanies();
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

        private void importNABTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonFunctions.ImportTransactions();
        }

        private void importNABPricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonFunctions.ImportPrices();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DBAccess DB = new DBAccess();
            DBAccess.BankBal bal;
            DB.GetBankBal(new DateTime(1900, 1, 1));
            bal = DB.GetNextBankBal();
            TbSOH.Text = bal.TtlDlrSOH.ToString();
            TbProfit.Text = (bal.TtlTradeProfit + bal.TtlDividendEarned).ToString();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            OleDbDataReader PriceHistReader;
            string myConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";

            // Define the database query    
            string mySelectQuery = "SELECT PriceDate, PrcClose FROM ASXPriceDate where ASXCode = 'AMP' order by PriceDate";


            // Create a database connection object using the connection string    
            OleDbConnection myConnection = new OleDbConnection(myConnectionString);

            // Create a database command on the connection using query    
            OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConnection);
            myConnection.Open();
            PriceHistReader = myCommand.ExecuteReader();
            DateTime MinDate = new DateTime();
            DateTime MaxDate = new DateTime();
            Decimal MinPrice = 0;
            Decimal MaxPrice = 0;
            int cnt = 0;

            while (PriceHistReader.Read())
            {
                chart1.Series[0].Points.AddXY(PriceHistReader.GetDateTime(0), PriceHistReader.GetDecimal(1));
                if (cnt == 0)
                {
                    MinPrice = PriceHistReader.GetDecimal(1);
                    MaxPrice = PriceHistReader.GetDecimal(1);
                    MinDate = PriceHistReader.GetDateTime(0);
                    MaxDate = PriceHistReader.GetDateTime(0);
                }
                else
                    MaxDate = PriceHistReader.GetDateTime(0);
                if (PriceHistReader.GetDecimal(1) > MaxPrice)
                    MaxPrice = PriceHistReader.GetDecimal(1);
            }

            //            chart1.Series[0].Points.;
            //            chart1.Show();
            // set chart data source - the data source must implement IEnumerable
            //chart1.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        private void importRecentPricesToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}

