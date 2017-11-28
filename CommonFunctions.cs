using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace ShareTrading

{
    class CommonFunctions
    {
        // Check for the Qty of Stock that is applicable to this stock and date
        public static DBAccess.DivPaid CheckForDividends(String ASXCode, DateTime dt, bool runningSimulation)
        {
            //DBAccess DB = new DBAccess();
            DBAccess.DividendHistory dividendHistory = null;
      List<DBAccess.DividendHistory> list = new List<DBAccess.DividendHistory>();
            if (DBAccess.GetDividends(ASXCode, dt, out list, DBAccess.dirn.equals))
                dividendHistory = list[0];
            if (dividendHistory == null)
                return null;
            int TtlASXCodeSOH = DBAccess.GetASXCodeSOH(ASXCode, dt, runningSimulation);
            if (TtlASXCodeSOH == 0)
                return null;
            DBAccess.DivPaid divPaid = new DBAccess.DivPaid();
            divPaid.ASXCode = ASXCode;
            divPaid.DatePaid = dividendHistory.DatePayable;
            //divPaid.DividendPerShare = dividendHistory.Amount;
            //divPaid.QtyShares = TtlASXCodeSOH;
            //divPaid.TtlDividend = TtlASXCodeSOH * dividendHistory.Amount;
            //DBAccess.Insert(divPaid);
            return divPaid;
        }

        public static int GetBuyQty(DBAccess.BankBal bankBal, Decimal BuyPrice, Decimal MarginLendingBarrier)
        {
            Decimal TtlDlrs = (decimal)(10000 + (bankBal.AcctBal - MarginLendingBarrier * bankBal.MarginLoan + bankBal.TtlDlrSOH - 300000) / 20);
            if (TtlDlrs > 20000)
                TtlDlrs = 20000;
            if (TtlDlrs < 5000)
                TtlDlrs = 5000;
            int BuyQty = (int)Math.Round(TtlDlrs / BuyPrice);
            return BuyQty;
        }

        public static void ImportTransactions(bool runningSimulation)
        {
            string[] fileEntries = Directory.GetFiles(@"C:\Users\Ray\Downloads\");
            string line;    
            foreach (string fileName in fileEntries)
            {
                if (fileName.Contains("Order History"))
                {
                    // we need to apply them in reverse order so do them one at at time as long as the next one is already there
                    List<String> linesToProcess = new List<String>();
                    using (StreamReader reader = new StreamReader(@fileName))
                    {
                        // Read first line - just headings
                        line = reader.ReadLine();
                        while ((line = reader.ReadLine()) != null)
                            linesToProcess.Insert(0, line);

                        foreach (String newline in linesToProcess)
                        {
                            //Define pattern
                            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                            //Separating columns to array
                            String[] Y = CSVParser.Split(newline);
                            DBAccess.TransRecords Trn = new DBAccess.TransRecords();
                            if ((Trn = DBAccess.FindNABTransaction((Y[8]).Replace(" ",""))) != null)
                                continue;
                            String ASXCode = ((String)Y[2]).Substring(1,3);
                            int Qty = 0;
                            int.TryParse(Y[5], out Qty);
                            Decimal UnitPrice = 0;
                            Decimal.TryParse(Y[4], out UnitPrice);
                            int startDate = Y[0].IndexOf('"');
                            int EndDate = Y[0].LastIndexOf('"');
                            DateTime dt = Convert.ToDateTime(Y[0].Substring(startDate+1, EndDate - startDate -1));
                            if (dt <= new DateTime(2016, 12, 23))
                                continue;
                            if (Y[6].Contains("CANCEL"))
                                continue;
                            if (Y[1].Contains("Buy"))
                                Form1.BuyTransaction(ASXCode,Qty , UnitPrice, dt, Y[8], runningSimulation);
                            else
                                Form1.SellTransaction(ASXCode,Qty,UnitPrice, dt, null,Y[8], runningSimulation);

                            /* Do something with X */
                        }
                    }
                    File.Delete(@fileName);
                }
            }
        }

        public static void ImportPrices()
        {
            DBAccess DB = new DBAccess();
            // .
            string[] fileEntries = Directory.GetFiles(@"C:\Users\Ray\Downloads\");
            foreach (string fileName in fileEntries)
            { 
                if (fileName.Contains("Watchlist"))
                {
                    using (StreamReader reader = new StreamReader(@fileName))
                    {
                        string line;
                        // Read first line - just headings
                        line = reader.ReadLine();
                        while ((line = reader.ReadLine()) != null)
                        {
                            //Define pattern
                            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                                               
                            //Separating columns to array
                            string[] X = CSVParser.Split(line);
                            DBAccess.ASXPriceDate ASXPriceDate = new DBAccess.ASXPriceDate();
                            ASXPriceDate.PriceDate = DateTime.Now;
                            ASXPriceDate.ASXCode = X[0];
              Decimal prcClose = ASXPriceDate.PrcClose;
              decimal prcOpen = ASXPriceDate.PrcOpen;
              decimal prcHigh = ASXPriceDate.PrcHigh;
              decimal prcLow = ASXPriceDate.PrcLow;
              int vol = ASXPriceDate.Volume;
                            Decimal.TryParse(X[5],out prcClose);
                            Decimal.TryParse(X[9], out prcOpen);
                            Decimal.TryParse(X[11], out prcHigh);
                            Decimal.TryParse(X[10], out prcLow);
                            int.TryParse(X[12], out vol);
                            DBAccess.ASXprcInsert(ASXPriceDate);
                            /* Do something with X */

                        }
                    }
                    File.Delete(@fileName);
                }
            }
        }

  
  }
}
