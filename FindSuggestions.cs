using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using System.Data.OleDb;
using Devart.Data.PostgreSql;




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


    public void CheckAllCompanies(bool runningSimulation)
    {
      DBAccess.TransRecords TransRecords;
      List<DBAccess.TransRecords> transList = null;

      List<DBAccess.ASXPriceDate> allPrices = new List<DBAccess.ASXPriceDate>();
      DBAccess.GetAllPrices(null, DateTime.Today, out allPrices, DBAccess.ASXPriceDateFieldList);
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
         if (!DBAccess.GetAllTransRecords(rec.ASXCode, DateTime.MinValue, out transList, DBAccess.TransRecordsFieldList, " AND SOH > 0 ", runningSimulation))
           continue;
        if (transList.Count <= 0)
          continue;
        DBAccess.TransRecords transRec = transList[0];
          // Sellls  ------------------------------------------------------------
         Decimal SellPrice = 0;
         DateTime TransDate = transRec.TranDate;
         // Difference in days, hours, and minutes.
         TimeSpan ts = lastDate - TransDate;
         // Difference in days.
         Double DaysHeld = (Double)ts.Days;

         Decimal TargetPrice = 0;
         TargetPrice = transRec.UnitPrice * (Decimal)(1.005 + ((Double)TargetSellReturn * Math.Sqrt(DaysHeld)));
         DBAccess.TransRecords SellTrn = SellSuggestion(rec.ASXCode, transRec.TransQty, TargetPrice, lastDate, transRec, "SellOnOpen4Return", rec);
         
         if (runningSimulation)
           continue;

                // Buys ------------------------------------

                //  Buy on margin below last sell - 
                if (DBAccess.SetupLastSellRecords(rec.ASXCode, runningSimulation, out transList))
                {
                    if (transList.Count > 0)
                    {
                        if (transList[0].BuySell == "Sell")
                        {
                            TransDate = transList[0].TranDate;
                            // Difference in days, hours, and minutes.
                            ts = lastDate - TransDate;
                            // Difference in days.
                            DaysHeld = (Double)ts.Days;
                            Decimal BuyPrice = 0;
                            TargetPrice = 0;
                            int BuyQty = 0;
                            TargetPrice = transList[0].UnitPrice * (Decimal)(1.0 - ((Double)TargetBuyReturn * Math.Sqrt(DaysHeld)) + (.15 * (Double)DaysHeld / 365.0));

                            BuyQty = CommonFunctions.GetBuyQty(bankBal, TargetPrice, MarginLendingBarrier);
                            BuySuggestion(rec.ASXCode, BuyQty, TargetPrice, lastDate, "BuyOnOpenBelowSell");
                        }
                    }
          continue;
         }

                // We don't have any so lets buy on a 5 day low or if very close to a Dividend
        if (DBAccess.GetAllTransRecords(rec.ASXCode, DateTime.MinValue, out transList, DBAccess.TransRecordsFieldList, " AND SOH > 0 ", runningSimulation))
        {
                    Decimal BuyPrice = 0;
                    if ((transList.Count > 0))
                    {
            transRec = transList[0];
                        //  Buy within 10 days of Dividend - 
                        DBAccess.DividendHistory DivHis = new DBAccess.DividendHistory();
            List<DBAccess.DividendHistory> list = new List<DBAccess.DividendHistory>();
                        if (DBAccess.GetDividends(rec.ASXCode, rec.PriceDate, out list, DBAccess.dirn.greaterThanEquals))
                        {
              DivHis = list[0];
                            if (DivHis != null && ChaseDividends)  // Only do this is chasing Dividends
                            {
                                if (DateTime.Compare(DivHis.ExDividend, rec.PriceDate.AddDays(10)) < 0)
                                {
                                    //Transaction Size
                                    BuyPrice = rec.PrcOpen;
                                    int BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                    BuySuggestion(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyNearDividend");
                                    continue;
                                }
                            }
                        }

                        if (rec.PrcLow <= rec.Day5Min * AddBuyMrgn &&
                            rec.Day5Min > rec.Day90Min)  // This is an attempt to make sure the price is not just diving
                        {
                            if (bankBal.MarginLoan / bankBal.TtlDlrSOH > (Decimal)MarginLoanRebuyLimit)
                                continue;
                            int BuyQty = 0;
                            if (rec.PrcOpen <= rec.Day5Min * AddBuyMrgn)
                            {
                                BuyPrice = rec.PrcOpen;
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuySuggestion(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnOpenDayMin");
                            }
                            else if (rec.PrcLow <= rec.Day5Min * AddBuyMrgn && bankBal.TtlDlrSOH > 0)
                            {
                                BuyPrice = rec.Day5Min * AddBuyMrgn;
                                //Transaction Size
                                BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuySuggestion(rec.ASXCode, BuyQty, BuyPrice, lastDate, "BuyOnDayMin");
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
                            if (BuyPrice < (Decimal)RebuyMargin * transRec.UnitPrice && bankBal.TtlDlrSOH > 0)
                            {
                                if (bankBal.MarginLoan / bankBal.TtlDlrSOH > (Decimal)MarginLoanRebuyLimit)
                                    continue;
                                int BuyQty = CommonFunctions.GetBuyQty(bankBal, BuyPrice, MarginLendingBarrier);
                                BuySuggestion(rec.ASXCode, BuyQty, BuyPrice, lastDate, "Rebuy");
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
