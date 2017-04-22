using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;


namespace ShareTrading
{
    class FindSuggestions
    {
        public DBAccess DB;
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

        public DateTime StartDate = DateTime.Today;
//        public DateTime EndDate = new DateTime(2016, 12, 15);
        public Decimal StartBal = 300000;
        public Decimal AddSellMrgn = (Decimal)1.015;
        public Decimal AddBuyMrgn = (Decimal).985;
        public Decimal RebuyMargin = (Decimal)0.80;
        public Decimal MarginLendingBarrier = (Decimal)3;


        public void CheckAllCompanies()
        {
            DBAccess.ASXPriceDate ASXPriceDate;
            DBAccess.TransRecords TransRecords;
            DB = new DBAccess();
            DB.connection.Open();
            DB.GetAllPrices(null, DateTime.Today);
            DateTime lastDate = StartDate;
            //  Set up the starting Account Bal
            DBAccess.BankBal bankBal = new DBAccess.BankBal();
            bankBal.BalDate = lastDate;
            bankBal.AcctBal = StartBal;
            DB.BankBalInsert(bankBal);
            Decimal DayDivTotal = (Decimal)0.0;

            while ((ASXPriceDate = DB.GetNextPriceDate()) != null)
            {
                if (ASXPriceDate.ASXCode == null)
                    return;
                DBAccess.DividendHistory dividendHistory = null;
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
                        DBAccess.TransRecords SellTrn = SellSuggestion(ASXPriceDate.ASXCode, TransRecords.TransQty, TargetPrice, lastDate, TransRecords, "SellOnOpen4Return", ASXPriceDate);
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
                            TargetPrice = TransRecords.UnitPrice * (Decimal)(1.0 - ((Double)TargetBuyReturn * Math.Sqrt(DaysHeld)) + (.15 * (Double)DaysHeld / 365.0));

                            BuyQty = CommonFunctions.GetBuyQty(bankBal, TargetPrice, MarginLendingBarrier);
                            BuySuggestion(ASXPriceDate.ASXCode, BuyQty, TargetPrice, lastDate, "BuyOnOpenBelowSell");
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
                                    BuySuggestion(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyNearDividend");
                                    continue;
                                }
                            }
                        }

                        if (ASXPriceDate.PrcLow <= ASXPriceDate.Day5Min * AddBuyMrgn &&
                            ASXPriceDate.Day5Min > ASXPriceDate.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            if (bankBal.MarginLoan / bankBal.TtlDlrSOH > (Decimal)MarginLoanRebuyLimit)
                                continue;
                            int BuyQty = 0;
                            if (ASXPriceDate.PrcOpen <= ASXPriceDate.Day5Min * AddBuyMrgn)
                            {
                                BuyPrice = ASXPriceDate.PrcOpen;
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuySuggestion(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnOpenDayMin");
                            }
                            else if (ASXPriceDate.PrcLow <= ASXPriceDate.Day5Min * AddBuyMrgn && bankBal.TtlDlrSOH > 0)
                            {
                                BuyPrice = ASXPriceDate.Day5Min * AddBuyMrgn;
                                //Transaction Size
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuySuggestion(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnDayMin");
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
                                BuySuggestion(ASXPriceDate.ASXCode, BuyQty, BuyPrice, lastDate, "Rebuy");
                            }
                        }
                    }
                }
            }
        }

        public DBAccess.TransRecords SellSuggestion(String ASXCode, int Qty, Decimal Price, DateTime TransDate, DBAccess.TransRecords BoughtRecord, String TransType, DBAccess.ASXPriceDate ASXPriceDate)
        {
            DBAccess.TodaysTrades TodaysTrades = new DBAccess.TodaysTrades();
            TimeSpan ts = DateTime.Today - BoughtRecord.TranDate;
            // Difference in days.
            Double DaysHeld = (Double)1.0 + ts.Days;

            TodaysTrades.ASXCode = ASXCode;
            TodaysTrades.BuySell = "Sell";
            TodaysTrades.TransQty = Qty;
            TodaysTrades.UnitPrice = Price;
            TodaysTrades.TransType = TransType;
            TodaysTrades.ROI = (((Decimal)100.0 * (Price - BoughtRecord.UnitPrice) / BoughtRecord.UnitPrice)) * 365/(Decimal)DaysHeld;
            TodaysTrades.CurrPrc = ASXPriceDate.PrcClose;
            TodaysTrades.TargetProfit = (Price - BoughtRecord.UnitPrice) * Qty;
            TodaysTrades.CurrProfit = (ASXPriceDate.PrcClose - BoughtRecord.UnitPrice) * Qty;
            TodaysTrades.PricePaid = BoughtRecord.UnitPrice;
            TodaysTrades.DaysHeld = (int)DaysHeld;
            DBAccess.TodaysTradesInsert(TodaysTrades);
            return BoughtRecord;
        }


        public void BuySuggestion(String ASXCode, int Qty, Decimal Price, DateTime TransDate, String TransType)
        {
            DBAccess.TodaysTrades TodaysTrades = new DBAccess.TodaysTrades();
            TodaysTrades.ASXCode = ASXCode;
            TodaysTrades.BuySell = "Buy";
            TodaysTrades.TransQty = Qty;
            TodaysTrades.UnitPrice = Price;
            TodaysTrades.TransType = TransType;
            DBAccess.TodaysTradesInsert(TodaysTrades);
        }
    }
}
