
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;


namespace ShareTrading
{
    public class DBAccess
    {
        public OleDbConnection connection = new OleDbConnection();
        public Boolean SimulationRunning = false;


        public DBAccess()
        {
            connection.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
        }

        public class DividendHistory
        {
            public int ID;
            public String ASXCode;
            public DateTime ExDividend;
            public DateTime BooksClose;
            public DateTime DatePayable;
            public Decimal Amount;
            public Decimal Franking;
            public Decimal FrankingCredit;
            public Decimal GrossDividend;
        }

        static OleDbConnection connectionDividend = new OleDbConnection();
        static OleDbDataReader DividendReader;

        public void DividendHistoryInsert(DividendHistory Dividend)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionDividend;
            connectionDividend.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionDividend.Open();
            command.CommandText = "insert into  DividendHistory  (ASXCode, ExDividend, BooksClose, DatePayable, Amount, Franking, FrankingCredit, GrossDividend) values ('" +
                    Dividend.ASXCode +
                    "',#" + Dividend.ExDividend.ToString("yyyy - MM - dd") +
                    "#,#" + Dividend.BooksClose.ToString("yyyy - MM - dd") +
                    "#,#" + Dividend.DatePayable.ToString("yyyy - MM - dd") +
                    "#," + Dividend.Amount +
                    "," + Dividend.Franking +
                    "," + Dividend.FrankingCredit +
                    "," + Dividend.GrossDividend +
                    ")";
            command.ExecuteNonQuery();
            connectionDividend.Close();
        }

        public Boolean GetMostRecentDividend(String ASXCode, DateTime dt)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionDividend;
            if (connectionDividend.State == System.Data.ConnectionState.Open)
                connectionDividend.Close();
            connectionDividend.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionDividend.Open();
            command.CommandText = "Select * from DividendHistory where ASXCode = '" + ASXCode +
                                    "' and ExDividend < #" + dt.ToString("yyyy - MM - dd") + "# order by ExDividend desc";
            DividendReader = command.ExecuteReader();
            return true;
        }


        public Boolean GetDividendHistory(String ASXCode, DateTime dt)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionDividend;
            if (connectionDividend.State == System.Data.ConnectionState.Open)
                connectionDividend.Close();
            connectionDividend.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionDividend.Open();
            command.CommandText = "Select * from DividendHistory where ASXCode = '" + ASXCode +
                                    "' and ExDividend = #" + dt.ToString("yyyy - MM - dd") + "#";
            DividendReader = command.ExecuteReader();
            return true;
        }

        public Boolean GetNextDividend(String ASXCode, DateTime dt)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionDividend;
            if (connectionDividend.State == System.Data.ConnectionState.Open)
                connectionDividend.Close();
            connectionDividend.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionDividend.Open();
            command.CommandText = "Select * from DividendHistory where ASXCode = '" + ASXCode +
                                    "' and ExDividend >= #" + dt.ToString("yyyy - MM - dd") + "# order by ExDividend asc";
            DividendReader = command.ExecuteReader();
            return true;
        }


        public DividendHistory GetNextDividendHistory()
        {
            DividendHistory DividendHistory = new DividendHistory();
            if (DividendReader.Read())
            {
                DividendHistory.ID = DividendReader.GetInt32(0);
                DividendHistory.ASXCode = DividendReader.GetString(1);
                DividendHistory.ExDividend = DividendReader.GetDateTime(2);
                DividendHistory.BooksClose = DividendReader.GetDateTime(3);
                DividendHistory.DatePayable = DividendReader.GetDateTime(4);
                DividendHistory.Amount = DividendReader.GetDecimal(5);
                DividendHistory.Franking = DividendReader.GetDecimal(6);
                DividendHistory.FrankingCredit = DividendReader.GetDecimal(7);
                DividendHistory.GrossDividend = DividendReader.GetDecimal(8);
                return DividendHistory;
            }
            return null;
        }


        // *************************************************************************************************

        public class SimulationPerformance
        {
            public int ID;
            public bool MaxBuys;
            public bool MaxSells;
            public bool ReBuysEnabed;
            public int MaxRebuyCount;  // The maximum number of parcels of any stock - Not implemented yet
            public bool ChaseDividends;  // Buy close to dividends to look for dividend or short gains
            public Decimal MarginLoanRebuyLimit;  //After we reach this limit (eg say.1) no more buys are allowed
            public Decimal TargetBuyReturn;  // THis is used as the "log" target for Buys
            public Decimal TargetSellReturn;  // THis is used as the "log" target for Sells
            public bool BuyOnDaysMin; //  Only buy on 5,0 .. days min if allowed
            public int MinPriceDays;
            public Decimal BuyPriceTargetPct;
            public Decimal SellPriceTargetPct;
            public Decimal RebuyPct;
            public Decimal MarginLendingBarrier;
            public DateTime StartDate;
            public DateTime EndDate;
            public Decimal NetProfit;
            public Decimal MaxMarginLoan;
            public Decimal MaxMarginLoanPctOfSOH;
            public Decimal ROI;
        }

        static OleDbConnection connectionPerformance = new OleDbConnection();

        public void SimulationPerformanceInsert(SimulationPerformance Performance)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionPerformance;
            connectionPerformance.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionPerformance.Open();
            command.CommandText = "insert into  SimulationPerformance  (MaxRebuyCount, MaxBuying, MaxSelling, ChaseDividends, MarginLoanRebuyLimit, TargetBuyReturn, TargetSellReturn, BuyOnDaysMin, MinPriceDays, BuyPriceTargetPct, SellPriceTargetPct, RebuyPct, MarginLendingBarrier, StartDateSimulation, EndDateSimulation, NetProfit, MaxMarginLoan, MaxMarginLoanPctOfSOH, ROI) values (" +
            Performance.MaxRebuyCount +
                    "," + Performance.MaxBuys +
                    "," + Performance.MaxSells +
                    "," + Performance.ChaseDividends +
                    "," + Performance.MarginLoanRebuyLimit +
                    "," + Performance.TargetBuyReturn +
                    "," + Performance.TargetSellReturn +
                    "," + Performance.BuyOnDaysMin +
                    "," + Performance.MinPriceDays +
                    "," + Performance.BuyPriceTargetPct +
                    "," + Performance.SellPriceTargetPct +
                    "," + Performance.RebuyPct +
                    "," + Performance.MarginLendingBarrier +
                    ",#" + Performance.StartDate.ToString("yyyy - MM - dd") +
                    "#,#" + Performance.EndDate.ToString("yyyy - MM - dd") +
                    "#," + Performance.NetProfit +
                    "," + Performance.MaxMarginLoan +
                    "," + Performance.MaxMarginLoanPctOfSOH +
                    "," + Performance.ROI +
                    ")";
            command.ExecuteNonQuery();
            connectionPerformance.Close();

        }




        // *************************************************************************************************

        public class BankBal
        {
            public int ID;
            public DateTime BalDate;
            public Decimal AcctBal;
            public Decimal MarginLoan;
            public Decimal TtlTradeProfit;
            public Decimal DayTradeProfit;
            public Decimal TtlDividendEarned;
            public Decimal DayDividend;
            public Decimal TtlDlrDaysInvested;
            public Decimal DlrDaysInvested;
            public Decimal TtlDlrSOH;
            public Decimal ROI;
        }

        static OleDbDataReader BankBalReader;
        static OleDbConnection connectionBankBal = new OleDbConnection();
        public Boolean GetBankBal(DateTime dt)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionBankBal;
            if (connectionBankBal.State == System.Data.ConnectionState.Open)
                connectionBankBal.Close();
            connectionBankBal.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionBankBal.Open();
            if (dt == new DateTime(1900, 1, 1))
            {
                if (SimulationRunning)
                    command.CommandText = "Select * from SimulationBankBal order by BalDate Desc";
                else
                    command.CommandText = "Select * from BankBal order by BalDate Desc";
            }
            else
            {
                if (SimulationRunning)
                    command.CommandText = "Select SimulationBankBal.* from SimulationBankBal where [SimulationBankBal.BalDate]  = #" + dt.ToString("yyyy - MM - dd") + "#";
                else
                    command.CommandText = "Select BankBal.* from BankBal where [BankBal.BalDate]  = #" + dt.ToString("yyyy - MM - dd") + "#";
            }
            BankBalReader = command.ExecuteReader();
            return true;
        }

        public BankBal GetNextBankBal()
        {
            BankBal bankBal = new BankBal();
            if (BankBalReader.Read())
            {
                bankBal.ID = BankBalReader.GetInt32(0);
                bankBal.BalDate = BankBalReader.GetDateTime(1);
                bankBal.AcctBal = BankBalReader.GetDecimal(2);
                bankBal.MarginLoan = BankBalReader.GetDecimal(3);
                bankBal.TtlTradeProfit = BankBalReader.GetDecimal(4);
                bankBal.DayTradeProfit = BankBalReader.GetDecimal(5);
                bankBal.TtlDividendEarned = BankBalReader.GetDecimal(6);
                bankBal.DayDividend = BankBalReader.GetDecimal(7);
                bankBal.TtlDlrDaysInvested = BankBalReader.GetDecimal(10);
                bankBal.DlrDaysInvested = BankBalReader.GetDecimal(9);
                bankBal.TtlDlrSOH = BankBalReader.GetDecimal(8);
                bankBal.ROI = BankBalReader.GetDecimal(11);

                return bankBal;
            }
            return null;
        }

        static OleDbConnection connectionBankBal2 = new OleDbConnection();

        public void BankBalInsert(BankBal BankBal)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionBankBal2;
            if (connectionBankBal2.State == System.Data.ConnectionState.Open)
                connectionBankBal2.Close();
            connectionBankBal2.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionBankBal2.Open();
            String BankTables;
            if (SimulationRunning)
                BankTables = "SimulationBankBal";
            else
                BankTables = "BankBal";

            command.CommandText = "insert into " + BankTables + " (BalDate, AcctBal, MarginLoan, TtlTradeProfit, DayTradeProfit, TtlDividendEarned, DayDividend, TtlDlrDaysInvested, DlrDaysInvested, TtlDlrSOH, ROI) values (" +
                    "#" + BankBal.BalDate.ToString("yyyy - MM - dd") +
                    "#," + BankBal.AcctBal +
                    "," + BankBal.MarginLoan +
                    "," + BankBal.TtlTradeProfit +
                    "," + BankBal.DayTradeProfit +
                    "," + BankBal.TtlDividendEarned +
                    "," + BankBal.DayDividend +
                    "," + BankBal.TtlDlrDaysInvested +
                    "," + BankBal.DlrDaysInvested +
                    "," + BankBal.TtlDlrSOH +
                    "," + BankBal.ROI +
                    ")";
            command.ExecuteNonQuery();
            connectionBankBal2.Close();

        }

        static OleDbDataReader SOHReader;
        static OleDbConnection connectionSOH = new OleDbConnection();

        // Put the current value of SOHinto the Banking Record
        public Decimal UpdateCurrentSOH(BankBal bankBal)
        {
            DateTime lastDate = bankBal.BalDate;
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionSOH;
            if (connectionSOH.State == System.Data.ConnectionState.Open)
                connectionSOH.Close();
            connectionSOH.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionSOH.Open();
            String tranTable;
            if (SimulationRunning)
                tranTable = "SimulationTransRecords";
            else
                tranTable = "TransRecord";

            command.CommandText = "select sum([ASXPriceDate.PrcClose] * [" + tranTable + ".SOH])  " +
                                  "from [" + tranTable + "], [ASXPriceDate]  where [" + tranTable + ".SOH] > 0 and [ASXPriceDate.PriceDate] = #" + lastDate.ToString("yyyy - MM - dd") + "# and [ASXPriceDate.ASXCode]=[" + tranTable + ".ASXCode]";
            SOHReader = command.ExecuteReader();
            SOHReader.Read();
            try
            {
                Decimal SOHValue = SOHReader.GetDecimal(0);
                bankBal.TtlDlrSOH = SOHValue;
            }
            catch (Exception ex)
            {
                bankBal.TtlDlrSOH = 0;
            }
            BankBalUpdate(bankBal);
            return bankBal.TtlDlrSOH;
        }

        public void BankBalUpdate(BankBal BankBal)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionBankBal2;
            if (connectionBankBal2.State == System.Data.ConnectionState.Open)
                connectionBankBal2.Close();
            connectionBankBal2.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionBankBal2.Open();
            String BankTables;
            if (SimulationRunning)
                BankTables = "SimulationBankBal";
            else
                BankTables = "BankBal";

            command.CommandText = "update " + BankTables + " set " +
                                  "BalDate = #" + BankBal.BalDate.ToString("yyyy - MM - dd") + "#," +
                                  "AcctBal = " + BankBal.AcctBal + "," +
                                  "MarginLoan = " + BankBal.MarginLoan + "," +
                                  "TtlTradeProfit = " + BankBal.TtlTradeProfit + "," +
                                  "DayTradeProfit = " + BankBal.DayTradeProfit + "," +
                                  "TtlDividendEarned = " + BankBal.TtlDividendEarned + "," +
                                  "DayDividend = " + BankBal.DayDividend + "," +
                                  "TtlDlrDaysInvested = " + BankBal.TtlDlrDaysInvested + "," +
                                  "DlrDaysInvested = " + BankBal.DlrDaysInvested + "," +
                                  "TtlDlrSOH = " + BankBal.TtlDlrSOH + "," +
                                  "ROI = " + BankBal.ROI + " where ID = " + BankBal.ID;
            command.ExecuteNonQuery();
            connectionBankBal2.Close();
        }


        // *************************************************************************************************
        public class DivPaid
        {
            public int ID;
            public String ASXCode;
            public DateTime DatePaid;
            public Decimal DividendPerShare;
            public int QtyShares;
            public Decimal TtlDividend;
        }

        static OleDbDataReader DivPaidReader;
        static OleDbConnection connectionDicPaidRecs = new OleDbConnection();


        public Boolean GetDivPaidRecords(String ASXCode, DateTime dt)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionDicPaidRecs;
            if (connectionDicPaidRecs.State == System.Data.ConnectionState.Open)
                connectionDicPaidRecs.Close();
            connectionDicPaidRecs.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionDicPaidRecs.Open();

            if (dt == new DateTime(1900, 1, 1))  // use as null equivalent
            {
                if (ASXCode != null)
                {
                    if (SimulationRunning)
                    {
                        command.CommandText = "Select * from SimulationDivPaid where ASXCode = '" + ASXCode + "' order by DatePaid Desc";
                    }
                    else
                    {
                        command.CommandText = "Select * from DivPaid where ASXCode = '" + ASXCode + "' order by DatePaid Desc";
                    }
                }
                else
                {
                    if (SimulationRunning)
                    {
                        command.CommandText = "Select * from SimulationDivPaid order by DatePaid Desc";
                    }
                    else
                    {
                        command.CommandText = "Select * from DivPaid order by DatePaid Desc";
                    }
                }
            }
            else
            {
                if (ASXCode != null)
                    if (SimulationRunning)
                        command.CommandText = "Select * from SimulationDivPaid where ASXCode = " + ASXCode + " and DatePaid  = #" + dt.ToString("yyyy - MM - dd") + "#";
                    else
                        command.CommandText = "Select * from DivPaid where ASXCode = " + ASXCode + " and DatePaid  = #" + dt.ToString("yyyy - MM - dd") + "#";
                else
                    if (SimulationRunning)
                    command.CommandText = "Select * from SimulationDivPaid order by DatePaid Desc";
                else
                    command.CommandText = "Select * from DivPaid order by DatePaid Desc";
            }
            DivPaidReader = command.ExecuteReader();
            return true;
        }


        public DivPaid GetNextDivPaidRecords()
        {
            DivPaid DivPaid = new DivPaid();
            if (DivPaidReader.Read())
            {
                DivPaid.ID = DivPaidReader.GetInt32(0);
                DivPaid.ASXCode = DivPaidReader.GetString(1);
                DivPaid.DatePaid = DivPaidReader.GetDateTime(2);
                DivPaid.DividendPerShare = DivPaidReader.GetDecimal(3);
                DivPaid.QtyShares = DivPaidReader.GetInt32(4);
                DivPaid.TtlDividend = DivPaidReader.GetDecimal(5);
                return DivPaid;
            }
            connectionDicPaidRecs.Close();
            return null;
        }

        public void DivPaidInsert(DivPaid myDivPaidRecord)
        {
            OleDbConnection connection_tmp = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connection_tmp.Open();
            command.Connection = connection_tmp;
            String tranTable = "DivPaid";
            if (SimulationRunning)
                tranTable = "SimulationDivPaid";
            else
                tranTable = "DivPaid";
            command.CommandText = "insert into " + tranTable + " (ASXCode, DatePaid, DivPerShare, QtyShares, TtlDividend) " +
                                  " values ('" + myDivPaidRecord.ASXCode +
                                  "',#" + myDivPaidRecord.DatePaid.ToString("yyyy-MM-dd") +
                                  "#, " + myDivPaidRecord.DividendPerShare +
                                  "," + myDivPaidRecord.QtyShares +
                                  "," + myDivPaidRecord.TtlDividend +
                                  ")";
            command.ExecuteNonQuery();
            connection_tmp.Close();
        }

        public void DivPaidUpdate(DivPaid myDivPaidRecord)
        {
            OleDbConnection connection_tmp = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connection_tmp.Open();
            command.Connection = connection_tmp;
            String tranTable = "DivPaid";
            if (SimulationRunning)
                tranTable = "SimulationDivPaid";
            else
                tranTable = "DivPaid";
            command.CommandText = "update " + tranTable + " " +
                                    "set ASXCode = '" + myDivPaidRecord.ASXCode +
                                    "' ,DatePaid = #" + myDivPaidRecord.DatePaid.ToString("yyyy-MM-dd") +
                                    "# ,DividendPerShare = '" + myDivPaidRecord.DividendPerShare +
                                    "' ,QtyShares = " + myDivPaidRecord.QtyShares +
                                    ",TtlDividend = " + myDivPaidRecord.TtlDividend +
                                    " where ID = " + myDivPaidRecord.ID;

            command.ExecuteNonQuery();
            connection_tmp.Close();
        }



        // *************************************************************************************************
        public class TransRecords
        {
            public int ID;
            public String ASXCode;
            public DateTime TranDate;
            public String BuySell;
            public int TransQty;
            public Decimal UnitPrice;
            public Decimal BrokerageInc;
            public Decimal GST;
            public int SOH;
            public Decimal TradeProfit;
            public int RelatedTransactionID;
            public int DaysHeld;
            public String TransType;
            public String NABOrderNmbr;
        }

        static OleDbDataReader TransRecordsReader;
        static OleDbConnection connectionTransRecs = new OleDbConnection();
        public Boolean GetTransRecords(String ASXCode, DateTime dt)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionTransRecs;
            if (connectionTransRecs.State == System.Data.ConnectionState.Open)
                connectionTransRecs.Close();
            connectionTransRecs.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionTransRecs.Open();

            if (dt == new DateTime(1900, 1, 1))  // use as null equivalent
            {
                if (ASXCode != null)
                {
                    if (SimulationRunning)
                    {
                        command.CommandText = "Select * from SimulationTransRecords where SOH > 0 and ASXCode = '" + ASXCode + "' order by TranDate Desc";
                    }
                    else
                    {
                        command.CommandText = "Select * from TransRecord where SOH > 0 and ASXCode = '" + ASXCode + "' order by TranDate Desc";
                    }
                }
                else
                {
                    if (SimulationRunning)
                    {
                        command.CommandText = "Select * from SimulationTransRecords where SOH > 0  order by TranDate Desc";
                    }
                    else
                    {
                        command.CommandText = "Select * from TransRecord where SOH > 0 order by TranDate Desc";
                    }
                }
            }
            else
            {
                if (ASXCode != null)
                {
                    if (SimulationRunning)
                        command.CommandText = "Select * from SimulationTransRecords where ASXCode = '" + ASXCode + "' and TranDate  = #" + dt.ToString("yyyy - MM - dd") + "#";
                    else
                        command.CommandText = "Select * from TransRecord where ASXCode = '" + ASXCode + "' and TranDate  = #" + dt.ToString("yyyy - MM - dd") + "#";
                }
                else
                {
                    if (SimulationRunning)
                        command.CommandText = "Select * from SimulationTransRecords order by TranDte Desc";
                    else
                        command.CommandText = "Select * from TransRecord order by TranDte Desc";
                }
            }
            TransRecordsReader = command.ExecuteReader();
            return true;
        }


        public static TransRecords FindNABTransaction(String NABOrderNmbr)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionTransRecs;
            if (connectionTransRecs.State == System.Data.ConnectionState.Open)
                connectionTransRecs.Close();
            connectionTransRecs.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionTransRecs.Open();
            command.CommandText = "Select * from TransRecord where NABOrderNmbr = '" + NABOrderNmbr + "' order by TranDate Desc";
            TransRecordsReader = command.ExecuteReader();
            TransRecords TransRecord = new TransRecords();
            if (TransRecordsReader.Read())
            {
                TransRecord.ID = TransRecordsReader.GetInt32(0);
                TransRecord.ASXCode = TransRecordsReader.GetString(1);
                TransRecord.TranDate = TransRecordsReader.GetDateTime(2);
                TransRecord.BuySell = TransRecordsReader.GetString(3);
                TransRecord.TransQty = TransRecordsReader.GetInt32(4);
                TransRecord.UnitPrice = TransRecordsReader.GetDecimal(5);
                TransRecord.BrokerageInc = TransRecordsReader.GetDecimal(6);
                TransRecord.GST = TransRecordsReader.GetDecimal(7);
                TransRecord.SOH = TransRecordsReader.GetInt32(8);
                TransRecord.TradeProfit = TransRecordsReader.GetDecimal(9);
                TransRecord.RelatedTransactionID = TransRecordsReader.GetInt32(10);
                TransRecord.DaysHeld = TransRecordsReader.GetInt32(11);
                TransRecord.NABOrderNmbr = TransRecordsReader.GetString(12);
                TransRecord.TransType = TransRecordsReader.GetString(13);
                return TransRecord;
            }
            return null;
        }

        public TransRecords GetNextTransRecords()
        {
            TransRecords TransRecord = new TransRecords();
            if (TransRecordsReader.Read())
            {
                TransRecord.ID = TransRecordsReader.GetInt32(0);
                TransRecord.ASXCode = TransRecordsReader.GetString(1);
                TransRecord.TranDate = TransRecordsReader.GetDateTime(2);
                TransRecord.BuySell = TransRecordsReader.GetString(3);
                TransRecord.TransQty = TransRecordsReader.GetInt32(4);
                TransRecord.UnitPrice = TransRecordsReader.GetDecimal(5);
                TransRecord.BrokerageInc = TransRecordsReader.GetDecimal(6);
                TransRecord.GST = TransRecordsReader.GetDecimal(7);
                TransRecord.SOH = TransRecordsReader.GetInt32(8);
                TransRecord.TradeProfit = TransRecordsReader.GetDecimal(9);
                TransRecord.RelatedTransactionID = TransRecordsReader.GetInt32(10);
                TransRecord.DaysHeld = TransRecordsReader.GetInt32(11);
                TransRecord.NABOrderNmbr = TransRecordsReader.GetString(12);
                TransRecord.TransType = TransRecordsReader.GetString(13);
                return TransRecord;
            }
            return null;
        }


        static OleDbDataReader LastSellReader;
        static OleDbConnection connectionTransRecs2 = new OleDbConnection();

        //Get the last Sell record 
        public Boolean SetupLastSellRecords(String ASXCode)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionTransRecs2;
            if (connectionTransRecs2.State == System.Data.ConnectionState.Open)
                connectionTransRecs2.Close();
            connectionTransRecs2.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionTransRecs2.Open();

            if (ASXCode != null)
            {
                if (SimulationRunning)
                {
                    command.CommandText = "Select * from SimulationTransRecords where ASXCode = '" + ASXCode + "' order by TranDate Desc";
                }
                else
                {
                    command.CommandText = "Select * from TransRecord where ASXCode = '" + ASXCode + "' order by TranDate Desc";
                }
            }
            LastSellReader = command.ExecuteReader();
            return true;
        }

        public TransRecords GetLastTransRecords()
        {
            TransRecords TransRecord = new TransRecords();
            if (LastSellReader.Read())
            {
                TransRecord.ID = LastSellReader.GetInt32(0);
                TransRecord.ASXCode = LastSellReader.GetString(1);
                TransRecord.TranDate = LastSellReader.GetDateTime(2);
                TransRecord.BuySell = LastSellReader.GetString(3);
                TransRecord.TransQty = LastSellReader.GetInt32(4);
                TransRecord.UnitPrice = LastSellReader.GetDecimal(5);
                TransRecord.BrokerageInc = LastSellReader.GetDecimal(6);
                TransRecord.GST = LastSellReader.GetDecimal(7);
                TransRecord.SOH = LastSellReader.GetInt32(8);
                TransRecord.TradeProfit = LastSellReader.GetDecimal(9);
                TransRecord.RelatedTransactionID = LastSellReader.GetInt32(10);
                TransRecord.DaysHeld = LastSellReader.GetInt32(11);
                return TransRecord;
            }
            return null;
        }



        public int GetASXCodeSOH(String ASXCode, DateTime dt)
        {
            OleDbDataReader TransSOHReader;
            OleDbConnection connectionSOHRecs = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionSOHRecs;
            if (connectionSOHRecs.State == System.Data.ConnectionState.Open)
                connectionSOHRecs.Close();
            connectionSOHRecs.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionSOHRecs.Open();
            String tranTable = "TransRecord";
            if (SimulationRunning)
                tranTable = "SimulationTransRecords";
            command.CommandText = "Select * from [" + tranTable + "] where [" + tranTable + ".SOH] > 0 and  [" + tranTable + ".ASXCode] = '" + ASXCode + "' and " +
                                  " [" + tranTable + ".TranDate] < #" + dt.ToString("yyyy-MM-dd") + "#";
            TransSOHReader = command.ExecuteReader();
            Int32 SOHValue = 0;
            while (TransSOHReader.Read())
                SOHValue = SOHValue + TransSOHReader.GetInt32(8);
            return SOHValue;
        }


        public void TransInsert(TransRecords myTransRecord)
        {
            OleDbConnection connection_tmp = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            command.Connection = connection_tmp;
            if (connection_tmp.State == System.Data.ConnectionState.Open)
                connection_tmp.Close();
            connection_tmp.Open();
            command.Connection = connection_tmp;
            String tranTable = "TransRecord";
            if (SimulationRunning)
                tranTable = "SimulationTransRecords";
            else
                tranTable = "TransRecord";
            command.CommandText = "insert into " + tranTable + " (ASXCode, TranDate, BuySell, UnitPrice, TransQty, BrokerageInc, GST, SOH, TradeProfit, RelatedTransactionID, DaysHeld, NABOrderNmbr, TransType) " +
                                  " values ('" + myTransRecord.ASXCode +
                                  "',#" + myTransRecord.TranDate.ToString("yyyy-MM-dd") +
                                  "#, '" + myTransRecord.BuySell +
                                  "'," + myTransRecord.UnitPrice +
                                  "," + myTransRecord.TransQty +
                                  "," + myTransRecord.BrokerageInc +
                                  "," + myTransRecord.GST +
                                  "," + myTransRecord.SOH +
                                  "," + myTransRecord.TradeProfit +
                                  "," + myTransRecord.RelatedTransactionID +
                                  "," + myTransRecord.DaysHeld +
                                  ",'" + myTransRecord.NABOrderNmbr +
                                  "','" + myTransRecord.TransType +
                                 "')";
            command.ExecuteNonQuery();
            connection_tmp.Close();
        }

        public void TransUpdate(TransRecords myTransRecord)
        {
            OleDbConnection connection_tmp = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connection_tmp.Open();
            command.Connection = connection_tmp;
            String tranTable = "TransRecord";
            if (SimulationRunning)
                tranTable = "SimulationTransRecords";
            else
                tranTable = "TransRecord";
            command.CommandText = "update " + tranTable + " " +
                                    "set ASXCode = '" + myTransRecord.ASXCode +
                                    "' ,TranDate = #" + myTransRecord.TranDate.ToString("yyyy-MM-dd") +
                                    "# ,BuySell = '" + myTransRecord.BuySell +
                                    "' ,TransQty = " + myTransRecord.TransQty +
                                    ",UnitPrice = " + myTransRecord.UnitPrice +
                                    ",BrokerageInc = " + myTransRecord.BrokerageInc +
                                    ",GST = " + myTransRecord.GST +
                                    ",SOH = " + myTransRecord.SOH +
                                    ",TradeProfit = " + myTransRecord.TradeProfit +
                                    ",RelatedTransactionID = " + myTransRecord.RelatedTransactionID +
                                    ",DaysHeld = " + myTransRecord.DaysHeld + " where ID = " + myTransRecord.ID;
            command.ExecuteNonQuery();
            connection_tmp.Close();
        }
        // ***************************************************************
        public class TodaysTrades
        {
            public int ID;
            public String ASXCode;
            public String BuySell;
            public int TransQty;
            public Decimal UnitPrice;
            public String TransType;
            public Decimal ROI;
            public Decimal CurrPrc;
            public Decimal TargetProfit;
            public Decimal CurrProfit;
            public Decimal PricePaid;
            public Decimal DaysHeld;
            public int RqdMove;
        }

        public static void PrepareForSuggestions()
        {
            OleDbConnection connectionTrades = new OleDbConnection();
            connectionTrades.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionTrades.Open();
            OleDbCommand DeleteCommand = new OleDbCommand();
            DeleteCommand.Connection = connectionTrades;
            DeleteCommand.CommandText = "Delete from TodaysTrades where 1 = 1";
            DeleteCommand.ExecuteNonQuery();
            connectionTrades.Close();
        }

        public static void TodaysTradesInsert(TodaysTrades TodaysTrades)
        {
            OleDbConnection connectionTrades = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connectionTrades.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            command.Connection = connectionTrades;
            if (connectionTrades.State == System.Data.ConnectionState.Open)
                connectionTrades.Close();
            connectionTrades.Open();
            command.Connection = connectionTrades;
            command.CommandText = "insert into TodaysTrades (ASXCode,  BuySell, UnitPrice, TransQty, TransType, ROI, CurrPrc, TargetProfit, CurrProfit,PricePaid, DaysHeld, RqdMove ) " +
                                  " values ('" + TodaysTrades.ASXCode +
                                  "', '" + TodaysTrades.BuySell +
                                  "'," + TodaysTrades.UnitPrice +
                                  "," + TodaysTrades.TransQty +
                                  ",'" + TodaysTrades.TransType +
                                  "'," + TodaysTrades.ROI +
                                  "," + TodaysTrades.CurrPrc +
                                  "," + TodaysTrades.TargetProfit +
                                  "," + TodaysTrades.CurrProfit +
                                  "," + TodaysTrades.PricePaid +
                                  "," + TodaysTrades.DaysHeld +
                                  "," + TodaysTrades.RqdMove +
                                  ")";
            command.ExecuteNonQuery();
            connectionTrades.Close();
        }


        // *************************************************************************************************
        public void PrepareForSimulation()
        {
            OleDbConnection connection_tmp = new OleDbConnection();
            connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connection_tmp.Open();
            OleDbCommand DeleteCommand = new OleDbCommand();
            DeleteCommand.Connection = connection;
            DeleteCommand.CommandText = "Delete from SimulationTransRecords where 1 = 1";
            DeleteCommand.ExecuteNonQuery();
            DeleteCommand.CommandText = "Delete from SimulationBankBal where 1 = 1";
            DeleteCommand.ExecuteNonQuery();
            DeleteCommand.CommandText = "Delete from SimulationDivPaid where 1 = 1";
            DeleteCommand.ExecuteNonQuery();
            SimulationRunning = true;
            connection_tmp.Close();
        }

        public class ASXPriceDate
        {
            public int ID;
            public String ASXCode;
            public DateTime PriceDate;
            public Decimal PrcOpen;
            public Decimal PrcHigh;
            public Decimal PrcLow;
            public Decimal PrcClose;
            public int Volume;
            public Decimal AdjClose;
            public Decimal Day5Min;
            public Decimal Day5Max;
            public Decimal Day5Pct;
            public Decimal Day30Min;
            public Decimal Day30Max;
            public Decimal Day30Pct;
            public Decimal Day60Min;
            public Decimal Day60Max;
            public Decimal Day60Pct;
            public Decimal Day90Min;
            public Decimal Day90Max;
            public Decimal Day90Pct;
        }

        static OleDbDataReader PriceReader;
        public Boolean GetAllPrices(String ASXCode, DateTime thsdte)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;

            if (ASXCode == null)
                command.CommandText = "Select * from ASXPriceDate where PriceDate >= #" + thsdte.ToString("yyyy-MM-dd") + "# order by PriceDate, ASXCode";
            else
                command.CommandText = "Select * from ASXPriceDate where ASXCode = '" + ASXCode + "' and PriceDate >= #" + thsdte.ToString("yyyy-MM-dd") + "# order by ASXPriceDate.PriceDate, ASXPriceDate.ASXCode";
            try
            {
                PriceReader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.Write("Exception " + ex.ToString());
                return false;
            }
            return true;
        }


        public ASXPriceDate GetNextPriceDate()
        {
            ASXPriceDate PrcDte = new ASXPriceDate();
            if (PriceReader.Read())
            {
                PrcDte.ID = PriceReader.GetInt32(0);
                PrcDte.PriceDate = PriceReader.GetDateTime(1);
                PrcDte.ASXCode = PriceReader.GetString(2);
                PrcDte.PrcOpen = PriceReader.GetDecimal(3);
                PrcDte.PrcHigh = PriceReader.GetDecimal(4);
                PrcDte.PrcLow = PriceReader.GetDecimal(5);
                PrcDte.PrcClose = PriceReader.GetDecimal(6);
                PrcDte.Volume = PriceReader.GetInt32(7);
                PrcDte.AdjClose = PriceReader.GetDecimal(8);
                PrcDte.Day5Min = PriceReader.GetDecimal(9);
                PrcDte.Day5Max = PriceReader.GetDecimal(10);
                PrcDte.Day5Pct = PriceReader.GetDecimal(11);
                PrcDte.Day30Min = PriceReader.GetDecimal(12);
                PrcDte.Day30Max = PriceReader.GetDecimal(13);
                PrcDte.Day30Pct = PriceReader.GetDecimal(14);
                PrcDte.Day60Min = PriceReader.GetDecimal(15);
                PrcDte.Day60Max = PriceReader.GetDecimal(16);
                PrcDte.Day60Pct = PriceReader.GetDecimal(17);
                PrcDte.Day90Min = PriceReader.GetDecimal(18);
                PrcDte.Day90Max = PriceReader.GetDecimal(19);
                PrcDte.Day90Pct = PriceReader.GetDecimal(20);
            }
            return PrcDte;
        }

        // Just get the company codes
        OleDbDataReader CompaniesReader;
        public Boolean GetAllCodes()
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            connection.Open();

            command.CommandText = "Select ASXCode from CompanyDetails order by ASXCode";
            CompaniesReader = command.ExecuteReader();
            return true;
        }


        public String GetNextCode()
        {
            String ASXCode = null;

            if (CompaniesReader.Read())
            {
                ASXCode = CompaniesReader.GetString(0);
            }
            return ASXCode;
        }

        public void ASXprcInsert(ASXPriceDate myASXPrcRecord)
        {
            OleDbConnection connectionPrice = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connectionPrice.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionPrice.Open();
            command.Connection = connectionPrice;
            command.CommandText = "delete from ASXPriceDate where PriceDate = #" + myASXPrcRecord.PriceDate.ToString("yyyy-MM-dd") + "# and ASXCode = '" + myASXPrcRecord.ASXCode + "'";
            command.ExecuteNonQuery();
            command.CommandText = "insert into ASXPriceDate (PriceDate, ASXCode, PrcOpen, PrcHigh, PrcLow, PrcClose, Volume, AdjClose, Day5Min, Day5Max, Day5Pct, Day30Min, Day30Max, Day30Pct, Day60Min, Day60Max, Day60Pct, Day90Min, Day90Max, Day90Pct)" +
                                  " values (#" +
            myASXPrcRecord.PriceDate.ToString("yyyy-MM-dd") + "#,'" +
            myASXPrcRecord.ASXCode + "'," +
            myASXPrcRecord.PrcOpen + "," +
            myASXPrcRecord.PrcHigh + "," +
            myASXPrcRecord.PrcLow + "," +
            myASXPrcRecord.PrcClose + "," +
            myASXPrcRecord.Volume + "," +
            myASXPrcRecord.AdjClose + "," +
            myASXPrcRecord.Day5Min + "," +
            myASXPrcRecord.Day5Max + "," +
            myASXPrcRecord.Day5Pct + "," +
            myASXPrcRecord.Day30Min + "," +
            myASXPrcRecord.Day30Max + "," +
            myASXPrcRecord.Day30Pct + "," +
            myASXPrcRecord.Day60Min + "," +
            myASXPrcRecord.Day60Max + "," +
            myASXPrcRecord.Day60Pct + "," +
            myASXPrcRecord.Day90Min + "," +
            myASXPrcRecord.Day90Max + "," +
            myASXPrcRecord.Day90Pct +
            ")";
            command.ExecuteNonQuery();
            connectionPrice.Close();
        }

        public void ASXprcUpdate(ASXPriceDate myASXPrcRecord)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "update ASXPriceDate set" +
            " Day5Min = " + myASXPrcRecord.Day5Min +
            ", Day5Max = " + myASXPrcRecord.Day5Max +
            ", Day5Pct = " + myASXPrcRecord.Day5Pct +
            ", Day30Min = " + myASXPrcRecord.Day30Min +
            ", Day30Max = " + myASXPrcRecord.Day30Max +
            ", Day30Pct = " + myASXPrcRecord.Day30Pct +
            ", Day60Min = " + myASXPrcRecord.Day60Min +
            ", Day60Max = " + myASXPrcRecord.Day60Max +
            ", Day60Pct = " + myASXPrcRecord.Day60Pct +
            ", Day90Min = " + myASXPrcRecord.Day90Min +
            ", Day90Max = " + myASXPrcRecord.Day90Max +
            ", Day90Pct = " + myASXPrcRecord.Day90Pct +
            " where ID = " + myASXPrcRecord.ID;
            command.ExecuteNonQuery();
        }


        public decimal GetMinPrice(int Period, String ASXCode, DateTime thisDate)
        {
            Decimal MinValue = (Decimal)0.0;
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            DateTime previousDate = thisDate.AddDays(-(Double)Period);
            command.CommandText = "Select min(PrcLow) as thismin from ASXPriceDate where ASXCode = '" + ASXCode + "' and PriceDate >= #" + previousDate.ToString("yyyy-MM-dd") + "# and PriceDate < #" + thisDate.ToString("yyyy-MM-dd") + "#";
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (reader["thismin"] != System.DBNull.Value)
                    MinValue = (Decimal)reader["thismin"];
            }
            reader.Close();
            return MinValue;
        }

        public decimal GetMaxPrice(int Period, String ASXCode, DateTime thisDate)
        {
            Decimal MaxValue = (Decimal)0.0;
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            DateTime previousDate = thisDate.AddDays(-(Double)Period);
            command.CommandText = "Select max(PrcHigh) as thismax from ASXPriceDate where ASXCode = '" + ASXCode + "' and PriceDate >= #" + previousDate.ToString("yyyy-MM-dd") + "# and PriceDate < #" + thisDate.ToString("yyyy-MM-dd") + "#";
            OleDbDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                if (reader["thismax"] != System.DBNull.Value)
                    MaxValue = (Decimal)reader["thismax"];
            }
            reader.Close();
            return MaxValue;
        }



        public class CompanyDetails
        {
            public int ID;
            public String ASXCode;
            public String CompanyName;
            public DateTime FrstDividendDate;
            public Decimal FrstDividendPerShare;
            public DateTime SecDividendDate;
            public Decimal SecDividendPerShare;
            public Decimal EarnperShare;
        }


        //****************************************************************************
        public class TransImport
        {
            public int ID;
            public String ASXCode;
            public DateTime TranDate;
            public String BuySell;
            public int TransQty;
            public Decimal UnitPrice;
            public String NABOrderNmbr;
        }

        static OleDbDataReader TransImportReader;
        static OleDbConnection connectionTransImpRecs = new OleDbConnection();
        public Boolean GetTransImpRecords()
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connectionTransImpRecs;
            if (connectionTransImpRecs.State == System.Data.ConnectionState.Open)
                connectionTransImpRecs.Close();
            connectionTransImpRecs.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connectionTransImpRecs.Open();
            command.CommandText = "Select * from TransImport order by TranDate asc";
            try
            {
                TransImportReader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.Write("Exception " + ex.ToString());
                return false;
            }
            return true;
        }


        public TransImport GetNextTransImpRecords()
        {
            TransImport TransRecord = new TransImport();
            if (TransImportReader.Read())
            {
                TransRecord.ID = TransImportReader.GetInt32(0);
                TransRecord.TranDate = TransImportReader.GetDateTime(1);
                TransRecord.BuySell = TransImportReader.GetString(2);
                TransRecord.ASXCode = TransImportReader.GetString(3);
                TransRecord.UnitPrice = TransImportReader.GetDecimal(4);
                TransRecord.TransQty = TransImportReader.GetInt32(5);
                TransRecord.NABOrderNmbr = TransImportReader.GetString(6);
                return TransRecord;
            }
            return null;
        }

        public void TransImpDelete(TransImport myTransImpRecord)
        {
            OleDbConnection connection_tmp = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            connection_tmp.Open();
            command.Connection = connection_tmp;
            command.CommandText = "delete from TransImport " +
                                   " where ID = " + myTransImpRecord.ID;
            command.ExecuteNonQuery();
            connection_tmp.Close();
        }
    }
}
// *************************************************************************************************



