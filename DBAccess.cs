
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Devart.Data.PostgreSql;
using System.Reflection;  // reflection namespace
using System.Windows.Forms;
using System.ComponentModel;





namespace ShareTrading
{
  
    public class DBAccess
    {
    const string HOST = "localhost";
    const int PORT = 5432;
      //static  public OleDbConnection connection = new OleDbConnection();
      //static  public Boolean SimulationRunning = false;


        public DBAccess()
        {
            //connection.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            
        }

    public static  string DBConnectString()
    {
      PgSqlConnectionStringBuilder fCsb = new PgSqlConnectionStringBuilder();
      fCsb.Database = "sharetrading";
      fCsb.Host = HOST;
      fCsb.Port = PORT;
      fCsb.UserId = "postgres";
      fCsb.Password = "2493X8QGvR";    // TODO; This needs to be obfuscated
      fCsb.ConnectionTimeout = 5000;    // Don't want to wait too long
      return fCsb.ConnectionString;
    }

    public static void DBConnectClose(PgSqlConnection conn)
    {
      try
      {
        conn.Close();
      }
      catch
      { }
    }
    public static bool CheckConnection(/* String host, int portNo */)
    {
      PgSqlConnectionStringBuilder fCsb = new PgSqlConnectionStringBuilder();
      fCsb.Database = "sharetrading";
      fCsb.Host = HOST; // host;
      fCsb.Port = PORT; //  portNo;
      fCsb.UserId = "postgres";
      fCsb.Password = "2493X8QGvR";    // TODO; This needs to be obfuscated
      fCsb.ConnectionTimeout = 5000;    // Don't want to wait too long
      using (PgSqlConnection conn = new PgSqlConnection(fCsb.ConnectionString))
      {
        try
        {
          conn.Open();
          conn.Close();
          return true;
        }
        catch (PgSqlException)
        {
        }
      }
      return false;
    }

    public static List<string> GetColumnNames(string tableName)
    {
      List<string> list = new List<string>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.CommandText = string.Format("SELECT column_name from information_schema.columns WHERE table_name = '{0}' ", tableName);
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
              list.Add(reader.GetString(0));

          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();

        }
      }
      return list;

    }

    public static bool GetFieldsAndColumns(Type t, string tableName, out string fieldNames, out string classMembers, out int nrFields)
    {
      List<string> membersList = GetClassMembers(t);
      List<string> fieldList = GetColumnNames(tableName);
      if (membersList.Count != fieldList.Count)
      {
        throw new ArgumentException(string.Format("{0} - number of class members does not match number db fields"), tableName);
      }
      classMembers = string.Join(",", membersList.ToArray());
      fieldNames = string.Join(",", fieldList.ToArray());
      nrFields = membersList.Count(); 
      return true;
    }


    public static bool DBInsert(Object myRecord, string tableName, Type classType)
    {
      string fieldNames = string.Empty;
      string classMembers = string.Empty;
      List<memberAttributes> memberList = GetClassValues(myRecord);


      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {

        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.CommandText = buildInsertList(tableName, classType, memberList);
          for (int i = 1; i < memberList.Count + 1; i++)                                         // Field zero is ID which is system generated
            command.Parameters.Add(memberList[i - 1].paramName, memberList[i-1].memberName.Contains("DateCreated") || memberList[i - 1].memberName.Contains("DateModified") ? DateTime.Today : memberList[i - 1].memberValue);

          command.Prepare();
          try
          {
            return command.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
    }

    public static bool DBUpdate(Object myRecord, string tableName, Type classType)
    {
      string fieldNames = string.Empty;
      string classMembers = string.Empty;
      List<memberAttributes> memberList = GetClassValues(myRecord);

      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {

        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.CommandText = buildUpdateList(tableName, classType, memberList);
          for (int i = 1; i < memberList.Count + 1; i++)                                         // Field zero is ID which is system generated
            command.Parameters.Add(memberList[i - 1].paramName, memberList[i - 1].memberName.Contains("DateModified") ? DateTime.Today : memberList[i - 1].memberValue);

          command.Prepare();
          try
          {
            return command.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
    }

    public static bool DBDelete(Object myRecord, string tableName, Type classType)
    {
      List<string> fieldNames =  GetColumnNames(tableName);
      List<memberAttributes> memberList = GetClassValues(myRecord);

      PropertyInfo pi = classType.GetProperty("DateDeleted", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      pi.SetValue(myRecord, DateTime.Now);

      pi = classType.GetProperty("ID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {

        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.CommandText = string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", tableName, fieldNames.Select(x => x).Contains("DateDeleted"), DateTime.Now, fieldNames[0], pi.GetValue(myRecord));
          for (int i = 1; i < memberList.Count + 1; i++)                                         // Field zero is ID which is system generated
            command.Parameters.Add(memberList[i - 1].paramName, memberList[i - 1].memberValue);

          command.Prepare();
          try
          {
            return command.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }

    }

    public static bool DeleteAll(string tableName)
    {
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.CommandText = string.Format("DELETE FROM {0} WHERE 1 = 1 ", tableName);
          try
          {
            int nrRecs = command.ExecuteNonQuery();
            return true;
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          Console.Write("Exception " + pex.ToString());
          return false;
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
    }

    public class memberAttributes
    {
      public string memberName { get; set; }
      public object memberValue { get; set; }
      public Type memberType { get; set; }
      public string paramName { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }

    }
    public static List<string> GetClassMembers(Type t)
  {
      return  t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Select(y => y.Name.Substring(1, y.Name.IndexOf(">") - 1)).ToList();
  }

    public static List<memberAttributes> GetClassValues(Object c)
    {
      List<string> propNames = GetClassMembers(c.GetType());

      List<memberAttributes> fieldValues = new List<memberAttributes>();
      for (int i = 0; i < propNames.Count; i++ )
      {
        PropertyInfo pi = c.GetType().GetProperty(propNames[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        memberAttributes ma = new memberAttributes();
        ma.memberType = pi.PropertyType;
        ma.memberName = pi.Name;
        ma.paramName = string.Format("@P{0}", i.ToString());
        
        if (pi.PropertyType == typeof(bool))
          ma.memberValue = (bool) pi.GetValue(c) ? "Y" : "N";
        else 
        if (pi.PropertyType == typeof(GLCodes))
          ma.memberValue = (int)pi.GetValue(c);
        else
          ma.memberValue = pi.GetValue(c);

        fieldValues.Add(ma);
      }
      return fieldValues;
    }

    public static string buildInsertList(string tableName, Object className,  List<memberAttributes> membersList)
    {
      string insertSQL = string.Empty;
      List<string> fieldList = GetColumnNames(tableName);

      if (membersList.Count != fieldList.Count)
      {
        throw new ArgumentException(string.Format("{0} - number of class members does not match number db fields"), tableName);
      }
      // INSERT INTO tablename (<fieldNames>) VALUES (@P1, ... , @Pn)
      fieldList.RemoveAt(0);
      membersList.RemoveAt(0);
      return  string.Format(" INSERT INTO {0} ({1}) VALUES ({2}) ", tableName, string.Join(",",  fieldList), string.Join(",", membersList.Select(x => x.paramName)));
    }
    public static string buildUpdateList(string tableName, Object className, List<memberAttributes> membersList) // List<string> fieldList, List<string> membersList)
    {
      string insertSQL = string.Empty;
      List<string> fieldList = GetColumnNames(tableName);

      if (membersList.Count != fieldList.Count)
      {
        throw new ArgumentException(string.Format("{0} - number of class members does not match number db fields"), tableName);
      }
      // UPDATE tablename SET <f1> = @P1, ... , <fn> = @Pn WHERE <f0> = @P0 
      string idField = fieldList[0];
      memberAttributes idMember = membersList[0];
      string updText = string.Empty;
      for (int i = 1; i < fieldList.Count; i++)
        updText += string.Format(", {0} = {1} ", fieldList[i], membersList[i].paramName);
      updText = updText.Substring(1);
      return string.Format(" UPDATE {0} SET {1}  WHERE {2} = {3}", tableName, updText, fieldList[0], membersList[0].paramName );
    }

    public class DatabaseIOException : Exception
    {
      public DatabaseIOException(string message, Exception innerException)
        : base(message, innerException)
      {
        {
        }
      }
    }
    public class DatabaseException : Exception
    {
      public DatabaseException(string message) : base(message)
      {
      }
    }

    // **********************************
    public class DividendHistory
        {
            public int ID { get; set; }
      public String ASXCode { get; set; }
      public DateTime ExDividend { get; set; }
      public DateTime BooksClose { get; set; }
      public DateTime DatePayable { get; set; }
      public Decimal Amount { get; set; }
      public Decimal Franking { get; set; }
      public Decimal FrankingCredit { get; set; }
      public Decimal GrossDividend { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }

    }
    public static string DividendHistoryFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("dividendhistory").ToArray()).Replace("\r\n", "");
      }
    }
    public static  void DividendHistoryInsert(DividendHistory myDividend)
    {
      DBInsert(myDividend, "dividendhistory", typeof(DividendHistory));
    }

        //public static Boolean GetMostRecentDividend(String ASXCode, DateTime dt)
        //{
        //    OleDbCommand command = new OleDbCommand();
        //    command.Connection = connectionDividend;
        //    if (connectionDividend.State == System.Data.ConnectionState.Open)
        //        connectionDividend.Close();
        //    connectionDividend.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
        //    connectionDividend.Open();
        //    command.CommandText = "Select * from DividendHistory where ASXCode = '" + ASXCode +
        //                            "' and ExDividend < #" + dt.ToString("yyyy - MM - dd") + "# order by ExDividend desc";
        //    DividendReader = command.ExecuteReader();
        //    return true;
        //}


        //public static Boolean GetDividendHistory(String ASXCode, DateTime dt)
        //{
        //    OleDbCommand command = new OleDbCommand();
        //    command.Connection = connectionDividend;
        //    if (connectionDividend.State == System.Data.ConnectionState.Open)
        //        connectionDividend.Close();
        //    connectionDividend.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
        //    connectionDividend.Open();
        //    command.CommandText = "Select * from DividendHistory where ASXCode = '" + ASXCode +
        //                            "' and ExDividend = #" + dt.ToString("yyyy - MM - dd") + "#";
        //    DividendReader = command.ExecuteReader();
        //    return true;
        //}

        //public static Boolean GetNextDividend(String ASXCode, DateTime dt)
        //{
        //    OleDbCommand command = new OleDbCommand();
        //    command.Connection = connectionDividend;
        //    if (connectionDividend.State == System.Data.ConnectionState.Open)
        //        connectionDividend.Close();
        //    connectionDividend.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
        //    connectionDividend.Open();
        //    command.CommandText = "Select * from DividendHistory where ASXCode = '" + ASXCode +
        //                            "' and ExDividend >= #" + dt.ToString("yyyy - MM - dd") + "# order by ExDividend asc";
        //    DividendReader = command.ExecuteReader();
        //    return true;
        //}
    public enum dirn
    {
      lessThan = 1,
      lessThanEquals = 2,
      equals = 3,
      greaterThanEquals = 4,
      greaterThan = 5,
      notEqual = 6
    }

    public static string getMathOp(dirn e)
    {
      switch ((int) e)
      {
        case 1:
          return " < ";
        case 2:
          return " <= ";
        case 3:
          return " = ";
        case 4:
          return " >= ";
        case 5:
          return " > ";
        case 6:
          return " <> ";
        default:
          return string.Empty;
      }
    }
    public static Boolean GetDividends(String ASXCode, DateTime thsdte, out List<DividendHistory> list,  dirn op)
    {
      list = new List<DividendHistory>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          string whereString = string.Empty;
          if (DateTime.Compare(DateTime.MinValue, thsdte) != 0)
          {
            command.Parameters.Add("@P2", thsdte);
            whereString += string.Format("AND dvh_exdivdate {0} @P2", getMathOp(op));
          }
          if (ASXCode != null)
          {
            command.Parameters.Add("@P3", ASXCode);
            whereString += string.Format(" AND dvh_ASXCode = @P3 ");
          }
          command.CommandText = string.Format("SELECT {0} FROM {2} WHERE dvh_datedeleted = @P1  {1} ORDER BY dvh_exdivdate , dvh_ASXCode ", DividendHistoryFieldList, whereString, "dividendhistory");
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetDividendHistory(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      return true;
    }
    public static List<DividendHistory> GetDividendHistory(PgSqlDataReader reader)
    {
      List<DividendHistory> list = new List<DividendHistory>();
      while (reader.Read())
      {
        DividendHistory dividendRecord = new DividendHistory();
        dividendRecord.ID = reader.GetInt32(0);
        dividendRecord.ASXCode = reader.GetString(1);
        dividendRecord.ExDividend = reader.GetDateTime(2);
        dividendRecord.BooksClose = reader.GetDateTime(3);
        dividendRecord.DatePayable = reader.GetDateTime(4);
        dividendRecord.Amount = reader.GetDecimal(5);
        dividendRecord.Franking = reader.GetDecimal(6);
        dividendRecord.FrankingCredit = reader.GetDecimal(7);
        dividendRecord.GrossDividend = reader.GetDecimal(8);
        dividendRecord.DateCreated = reader.GetDateTime(9);
        dividendRecord.DateModified = reader.GetDateTime(10);
        dividendRecord.DateDeleted = reader.GetDateTime(11);
        list.Add(dividendRecord);
      }
      return list;
    }

        // *************************************************************************************************

        public class SimulationPerformance
        {
            public int ID { get; set; }
      public bool MaxBuys { get; set; }
      public bool MaxSells { get; set; }
      public bool ReBuysEnabed { get; set; }
            public int MaxRebuyCount { get; set; }  // The maximum number of parcels of any stock - Not implemented yet
      public bool ChaseDividends { get; set; }  // Buy close to dividends to look for dividend or short gains
      public Decimal MarginLoanRebuyLimit { get; set; }  //After we reach this limit (eg say.1) no more buys are allowed
      public Decimal TargetBuyReturn { get; set; } // THis is used as the "log" target for Buys
      public Decimal TargetSellReturn { get; set; }  // THis is used as the "log" target for Sells
            public bool BuyOnDaysMin { get; set; } //  Only buy on 5,0 .. days min if allowed
      public int MinPriceDays { get; set; }
      public Decimal BuyPriceTargetPct { get; set; }
      public Decimal SellPriceTargetPct { get; set; }
      public Decimal RebuyPct { get; set; }
      public Decimal MarginLendingBarrier { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      public Decimal NetProfit { get; set; }
      public Decimal MaxMarginLoan { get; set; }
      public Decimal MaxMarginLoanPctOfSOH { get; set; }
      public Decimal ROI { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }

    }

    static OleDbConnection connectionPerformance = new OleDbConnection();

        public static void SimulationPerformanceInsert(SimulationPerformance Performance)
        {
      DBInsert(Performance, "simulationperformance", typeof(SimulationPerformance));

        }




        // *************************************************************************************************

        public class BankBal
        {
            public int ID;
            public DateTime BalDate { get; set; }
      public Decimal AcctBal { get; set; }
      public Decimal MarginLoan { get; set; }
      public Decimal TtlTradeProfit { get; set; }
      public Decimal DayTradeProfit { get; set; }
      public Decimal TtlDividendEarned { get; set; }
      public Decimal DayDividend { get; set; }
      public Decimal TtlDlrDaysInvested { get; set; }
      public Decimal DlrDaysInvested { get; set; }
      public Decimal TtlDlrSOH { get; set; }
      public Decimal ROI { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }

    public static string BankBalFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("bankbal").ToArray()).Replace("\r\n", "");
      }
    }
    public static string SimulationBankBalFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("simulationbankbal").ToArray()).Replace("\r\n", "");
      }
    }
    //    static OleDbDataReader BankBalReader;
    //static OleDbConnection connectionBankBal = new OleDbConnection();
    //static PgSqlDataReader BankBalReader;
    //    static PgSqlConnection connectionBankBal = DBConnectOpen();
    //public static  Boolean GetBankBal(DateTime dt)
    //    {
    //  PgSqlConnection conn = new PgSqlConnection(DBConnectString());
    //  conn.Open();
    //  PgSqlCommand command = new PgSqlCommand();
    //  command.Connection = conn;
    //  if (dt == new DateTime(1900, 1, 1))
      
    //        {
    //            if (SimulationRunning)
    //                command.CommandText = "Select * from SimulationBankBal order by bbl_BalDate Desc";
    //            else
    //                command.CommandText = "Select * from BankBal order by bbl_BalDate Desc";
    //        }
    //        else
    //        {
    //            if (SimulationRunning)
    //                command.CommandText = "Select SimulationBankBal.* from SimulationBankBal where [SimulationBankBal.bbl_BalDate]  = " + dt.ToString("yyyy - MM - dd");
    //            else
    //                command.CommandText = "Select BankBal.* from BankBal where [BankBal.bbl_BalDate]  = " + dt.ToString("yyyy - MM - dd");
    //        }
    //        BankBalReader = command.ExecuteReader();
    //        return true;
    //    }
    public static Boolean GetAllBankBalRecords(DateTime thsdte, out List<BankBal> list, string extraWhere, bool simulationRunning, dirn op /* = */)
    {
      list = new List<BankBal>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          string whereString = string.Empty;
          if (DateTime.Compare(DateTime.MinValue, thsdte) != 0)
          {
            command.Parameters.Add("@P2", thsdte);
            whereString += string.Format("AND bbl_baldate {0} @P2", getMathOp(op));
          }
          command.CommandText = string.Format("SELECT {0} FROM {2} WHERE bbl_datedeleted = @P1  {1} {3} ORDER BY bbl_balDate ", 
            simulationRunning ? SimulationBankBalFieldList : BankBalFieldList, whereString, simulationRunning ? "simulationbankbal" : "bankbal", extraWhere);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetBankBalRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      return true;
    }

    public static List<BankBal> GetBankBalRecords(PgSqlDataReader reader)
        {
      List<BankBal> list = new List<BankBal>();
            if (reader.Read())
            {
            BankBal bankBal = new BankBal();
                bankBal.ID = reader.GetInt32(0);
                bankBal.BalDate = reader.GetDateTime(1);
                bankBal.AcctBal = reader.GetDecimal(2);
                bankBal.MarginLoan = reader.GetDecimal(3);
                bankBal.TtlTradeProfit = reader.GetDecimal(4);
                bankBal.DayTradeProfit = reader.GetDecimal(5);
                bankBal.TtlDividendEarned = reader.GetDecimal(6);
                bankBal.DayDividend = reader.GetDecimal(7);
                bankBal.TtlDlrDaysInvested = reader.GetDecimal(10);
                bankBal.DlrDaysInvested = reader.GetDecimal(9);
                bankBal.TtlDlrSOH = reader.GetDecimal(8);
                bankBal.ROI = reader.GetDecimal(11);

                list.Add(bankBal);
            }
            return list;
        }

        static OleDbConnection connectionBankBal2 = new OleDbConnection();

        public static void BankBalInsert(BankBal BankBal, bool runningSimulation)
        {
      //OleDbCommand command = new OleDbCommand();
      //command.Connection = connectionBankBal2;
      //if (connectionBankBal2.State == System.Data.ConnectionState.Open)
      //    connectionBankBal2.Close();
      //connectionBankBal2.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
      //connectionBankBal2.Open();
      //String BankTables;
      if (runningSimulation)
        DBInsert(BankBal, "simulationbankbal", typeof(BankBal));    // BankTables = "SimulationBankBal";
            else
        DBInsert(BankBal, "bankbal", typeof(BankBal));    //   BankTables = "BankBal";

            //command.CommandText = "insert into " + BankTables + " (BalDate, AcctBal, MarginLoan, TtlTradeProfit, DayTradeProfit, TtlDividendEarned, DayDividend, TtlDlrDaysInvested, DlrDaysInvested, TtlDlrSOH, ROI) values (" +
            //        "#" + BankBal.BalDate.ToString("yyyy - MM - dd") +
            //        "#," + BankBal.AcctBal +
            //        "," + BankBal.MarginLoan +
            //        "," + BankBal.TtlTradeProfit +
            //        "," + BankBal.DayTradeProfit +
            //        "," + BankBal.TtlDividendEarned +
            //        "," + BankBal.DayDividend +
            //        "," + BankBal.TtlDlrDaysInvested +
            //        "," + BankBal.DlrDaysInvested +
            //        "," + BankBal.TtlDlrSOH +
            //        "," + BankBal.ROI +
            //        ")";
            //command.ExecuteNonQuery();
            //connectionBankBal2.Close();

        }

        //static OleDbDataReader SOHReader;
        //static OleDbConnection connectionSOH = new OleDbConnection();

        // Put the current value of SOHinto the Banking Record
        public static  Decimal UpdateCurrentSOH(BankBal bankBal, bool runningSimulation)
        {
            DateTime lastDate = bankBal.BalDate;

      //OleDbCommand command = new OleDbCommand();
      //command.Connection = connectionSOH;
      //if (connectionSOH.State == System.Data.ConnectionState.Open)
      //    connectionSOH.Close();
      //connectionSOH.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
      //connectionSOH.Open();
      //String tranTable;
      //if (SimulationRunning)
      //    tranTable = "SimulationTransRecords";
      //else
      //    tranTable = "TransRecord";

      //command.CommandText = "select sum([ASXPriceDate.PrcClose] * [" + tranTable + ".SOH])  " +
      //                      "from [" + tranTable + "], [ASXPriceDate]  where [" + tranTable + ".SOH] > 0 and [ASXPriceDate.PriceDate] = #" + 
      //                      lastDate.ToString("yyyy - MM - dd") + "# and [ASXPriceDate.ASXCode]=[" + tranTable + ".ASXCode]";
      //SOHReader = command.ExecuteReader();
      //SOHReader.Read();
      //try
      //{
      //    Decimal SOHValue = SOHReader.GetDecimal(0);
      //    bankBal.TtlDlrSOH = SOHValue;
      //}
      //catch (Exception ex)
      //{
      //    bankBal.TtlDlrSOH = 0;
      //}
      //BankBalUpdate(bankBal);
      //return bankBal.TtlDlrSOH;
      Decimal ttlDlrSOH = 0M;
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          command.Parameters.Add("@P1", lastDate);
          command.CommandText = string.Format("SELECT SUM(apd_prcclose * trn_soh)  FROM {0}, asxpricedate  WHERE trn_datedeleted = @P0 AND apd_datedeleted = @P0 AND trn_soh > 0 AND apd_pricedate = @P1 and apd_asxcode = trn_asxcode", runningSimulation ? "simulationtransrecords" : "transrecord");
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            reader.Read();
            ttlDlrSOH = reader.GetDecimal(0);

          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return 0M;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      bankBal.TtlDlrSOH = ttlDlrSOH;
      DBUpdate(bankBal, "bankbal", typeof(BankBal));
      return ttlDlrSOH;
    }

        public static void BankBalUpdate(BankBal BankBal)
        {
      DBUpdate(BankBal, "bankbal", typeof(BankBal));
            //OleDbCommand command = new OleDbCommand();
            //command.Connection = connectionBankBal2;
            //if (connectionBankBal2.State == System.Data.ConnectionState.Open)
            //    connectionBankBal2.Close();
            //connectionBankBal2.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            //connectionBankBal2.Open();
            //String BankTables;
            //if (SimulationRunning)
            //    BankTables = "SimulationBankBal";
            //else
            //    BankTables = "BankBal";

            //command.CommandText = "update " + BankTables + " set " +
            //                      "BalDate = #" + BankBal.BalDate.ToString("yyyy - MM - dd") + "#," +
            //                      "AcctBal = " + BankBal.AcctBal + "," +
            //                      "MarginLoan = " + BankBal.MarginLoan + "," +
            //                      "TtlTradeProfit = " + BankBal.TtlTradeProfit + "," +
            //                      "DayTradeProfit = " + BankBal.DayTradeProfit + "," +
            //                      "TtlDividendEarned = " + BankBal.TtlDividendEarned + "," +
            //                      "DayDividend = " + BankBal.DayDividend + "," +
            //                      "TtlDlrDaysInvested = " + BankBal.TtlDlrDaysInvested + "," +
            //                      "DlrDaysInvested = " + BankBal.DlrDaysInvested + "," +
            //                      "TtlDlrSOH = " + BankBal.TtlDlrSOH + "," +
            //                      "ROI = " + BankBal.ROI + " where ID = " + BankBal.ID;
            //command.ExecuteNonQuery();
            //connectionBankBal2.Close();
        }


        // *************************************************************************************************
        public class DivPaid
        {
            public int ID { get; set; }
            public String ASXCode { get; set; }
      public DateTime DatePaid { get; set; }
      public Decimal DividendPerShare { get; set; }
      public int QtyShares { get; set; }
      public Decimal TtlDividend { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }

    public static string DividendPaidFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("dividendpaid").ToArray()).Replace("\r\n", "");
      }
    }
    public static string SimulationDividendPaidFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("simulationdividendpaid").ToArray()).Replace("\r\n", "");
      }
    }
    //static OleDbDataReader DivPaidReader;
    //static OleDbConnection connectionDicPaidRecs = new OleDbConnection();


    //public static Boolean GetDivPaidRecords(String ASXCode, DateTime dt)
    //    {
    //        OleDbCommand command = new OleDbCommand();
    //        command.Connection = connectionDicPaidRecs;
    //        if (connectionDicPaidRecs.State == System.Data.ConnectionState.Open)
    //            connectionDicPaidRecs.Close();
    //        connectionDicPaidRecs.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
    //        connectionDicPaidRecs.Open();

    //        if (dt == new DateTime(1900, 1, 1))  // use as null equivalent
    //        {
    //            if (ASXCode != null)
    //            {
    //                if (SimulationRunning)
    //                {
    //                    command.CommandText = "Select * from SimulationDivPaid where ASXCode = '" + ASXCode + "' order by DatePaid Desc";
    //                }
    //                else
    //                {
    //                    command.CommandText = "Select * from DivPaid where ASXCode = '" + ASXCode + "' order by DatePaid Desc";
    //                }
    //            }
    //            else
    //            {
    //                if (SimulationRunning)
    //                {
    //                    command.CommandText = "Select * from SimulationDivPaid order by DatePaid Desc";
    //                }
    //                else
    //                {
    //                    command.CommandText = "Select * from DivPaid order by DatePaid Desc";
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (ASXCode != null)
    //                if (SimulationRunning)
    //                    command.CommandText = "Select * from SimulationDivPaid where ASXCode = " + ASXCode + " and DatePaid  = #" + dt.ToString("yyyy - MM - dd") + "#";
    //                else
    //                    command.CommandText = "Select * from DivPaid where ASXCode = " + ASXCode + " and DatePaid  = #" + dt.ToString("yyyy - MM - dd") + "#";
    //            else
    //                if (SimulationRunning)
    //                command.CommandText = "Select * from SimulationDivPaid order by DatePaid Desc";
    //            else
    //                command.CommandText = "Select * from DivPaid order by DatePaid Desc";
    //        }
    //        DivPaidReader = command.ExecuteReader();
    //        return true;

    //    }
    public static Boolean GetDividendPaidRecords(string ASXCode,DateTime datePaid, out List<DivPaid> list, bool runningSimulation)
    {
      list = new List<DivPaid>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          command.Parameters.Add("@P1", datePaid);
          string ASXCodeString = string.Empty;
          if (ASXCode != null)
          {
            command.Parameters.Add("@P2", ASXCode);
            ASXCodeString = string.Format(" AND cod_ASXCode = @P2 ", ASXCode);
          }
          command.CommandText = string.Format("SELECT {0} FROM {2} WHERE dvp_datedeleted = @P0 AND dvp_datepaid = @P1 {1} ORDER BY dvp_ASXCode ", DividendPaidFieldList.Replace("\r\n", ""), ASXCodeString, runningSimulation ? "dividendpaid" : "simulationdividendpaid");
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetDivPaidRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      return true;
    }

    public static List<DivPaid> GetDivPaidRecords(PgSqlDataReader reader)
        {
      List<DivPaid> list = new List<DivPaid>();
            DivPaid DivPaid = new DivPaid();
            if (reader.Read())
            {
                DivPaid.ID = reader.GetInt32(0);
                DivPaid.ASXCode = reader.GetString(1);
                DivPaid.DatePaid = reader.GetDateTime(2);
                DivPaid.DividendPerShare = reader.GetDecimal(3);
                DivPaid.QtyShares = reader.GetInt32(4);
                DivPaid.TtlDividend = reader.GetDecimal(5);
        DivPaid.DateCreated = reader.GetDateTime(6);
        DivPaid.DateModified = reader.GetDateTime(7);
        DivPaid.DateDeleted = reader.GetDateTime(8);
                list.Add(DivPaid);
            }
            return list;
        }

        public static void DivPaidInsert(DivPaid myDivPaidRecord)
        {
      DBInsert(myDivPaidRecord, "dividendpaid", typeof(DivPaid));
            //OleDbConnection connection_tmp = new OleDbConnection();
            //OleDbCommand command = new OleDbCommand();
            //connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            //connection_tmp.Open();
            //command.Connection = connection_tmp;
            //String tranTable = "DivPaid";
            //if (SimulationRunning)
            //    tranTable = "SimulationDivPaid";
            //else
            //    tranTable = "DivPaid";
            //command.CommandText = "insert into " + tranTable + " (ASXCode, DatePaid, DivPerShare, QtyShares, TtlDividend) " +
            //                      " values ('" + myDivPaidRecord.ASXCode +
            //                      "',#" + myDivPaidRecord.DatePaid.ToString("yyyy-MM-dd") +
            //                      "#, " + myDivPaidRecord.DividendPerShare +
            //                      "," + myDivPaidRecord.QtyShares +
            //                      "," + myDivPaidRecord.TtlDividend +
            //                      ")";
            //command.ExecuteNonQuery();
            //connection_tmp.Close();
        }

        public static void DivPaidUpdate(DivPaid myDivPaidRecord)
        {
      DBUpdate(myDivPaidRecord, "dividendpaid", typeof(DivPaid));
            //OleDbConnection connection_tmp = new OleDbConnection();
            //OleDbCommand command = new OleDbCommand();
            //connection_tmp.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            //connection_tmp.Open();
            //command.Connection = connection_tmp;
            //String tranTable = "DivPaid";
            //if (SimulationRunning)
            //    tranTable = "SimulationDivPaid";
            //else
            //    tranTable = "DivPaid";
            //command.CommandText = "update " + tranTable + " " +
            //                        "set ASXCode = '" + myDivPaidRecord.ASXCode +
            //                        "' ,DatePaid = #" + myDivPaidRecord.DatePaid.ToString("yyyy-MM-dd") +
            //                        "# ,DividendPerShare = '" + myDivPaidRecord.DividendPerShare +
            //                        "' ,QtyShares = " + myDivPaidRecord.QtyShares +
            //                        ",TtlDividend = " + myDivPaidRecord.TtlDividend +
            //                        " where ID = " + myDivPaidRecord.ID;

            //command.ExecuteNonQuery();
            //connection_tmp.Close();
        }



        // *************************************************************************************************
    public class TransRecords
    {
      public int ID { get; set; }
      public String ASXCode { get; set; }
      public DateTime TranDate { get; set; }
      public String BuySell { get; set; }
      public int TransQty { get; set; }
      public Decimal UnitPrice { get; set; }
      public Decimal BrokerageInc { get; set; }
      public Decimal GST { get; set; }
      public int SOH { get; set; }
      public Decimal TradeProfit { get; set; }
      public int RelatedTransactionID { get; set; }
      public decimal DaysHeld { get; set; }
      public String NABOrderNmbr { get; set; }
      public String TransType { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
    public static string TransRecordsFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("transrecord").ToArray()).Replace("\r\n", "");
      }
    }
    public static string SimulationTransRecordsFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("simulationtransrecords").ToArray()).Replace("\r\n", "");
      }
    }

    public static Boolean GetAllTransRecords(string ASXCode, DateTime thsdte, out List<TransRecords> list, string reqdFields, string extraWhere, bool simulationRunning)
    {
      return GetAllTransRecords(ASXCode, thsdte, out list, reqdFields, extraWhere, simulationRunning, dirn.greaterThanEquals);
    }
    public static Boolean GetAllTransRecords(String ASXCode, DateTime thsdte, out List<TransRecords> list, string reqdFields, string extraWhere, bool simulationRunning, dirn op)
    {

      string whereString = string.Empty;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      if (DateTime.Compare(DateTime.MinValue, thsdte) != 0)
      {
        paramList.Add(new PgSqlParameter("@P2", thsdte));
        whereString += string.Format(" AND trn_transdate {0} @P2 ", getMathOp(op));
      }

      if (ASXCode != null)
      {
        paramList.Add(new PgSqlParameter("@P3", ASXCode));
        whereString += string.Format(" AND trn_ASXCode = @P3 ");
      }
      whereString += extraWhere;
      return GetTransRecords(paramList, out list, reqdFields, whereString, " ORDER BY trn_transdate, trn_ASXCode", simulationRunning);
    }



    public static bool GetTransRecords(List<PgSqlParameter> paramList, out List<TransRecords> list, string reqdFields, string extraWhere, string orderBy, bool simulationRunning)
    {
      list = new List<TransRecords>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          if (paramList != null)
            command.Parameters.AddRange(paramList.ToArray());

          command.CommandText = string.Format("SELECT {1} FROM {0} WHERE trn_datedeleted = @P0 {2} {3} ", simulationRunning ? "simulationtransrecords" : "transrecord", reqdFields, extraWhere, orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetAllTransRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count <= 0)
        return false;

      return true;
    }
    public static List<TransRecords> GetAllTransRecords(PgSqlDataReader reader)
    {
      List<TransRecords> list = new List<TransRecords>();
      while (reader.Read())
      {
        TransRecords TransRecord = new TransRecords();
        TransRecord.ID = reader.GetInt32(0);
        TransRecord.ASXCode = reader.GetString(1);
        TransRecord.TranDate = reader.GetDateTime(2);
        TransRecord.BuySell = reader.GetString(3);
        TransRecord.TransQty = reader.GetInt32(4);
        TransRecord.UnitPrice = reader.GetDecimal(5);
        TransRecord.BrokerageInc = reader.GetDecimal(6);
        TransRecord.GST = reader.GetDecimal(7);
        TransRecord.SOH = reader.GetInt32(8);
        TransRecord.TradeProfit = reader.GetDecimal(9);
        TransRecord.RelatedTransactionID = reader.GetInt32(10);
        TransRecord.DaysHeld = reader.GetInt32(11);
        TransRecord.NABOrderNmbr = reader.GetString(12);
        TransRecord.TransType = reader.GetString(13);
        TransRecord.DateCreated = reader.GetDateTime(14);
        TransRecord.DateModified = reader.GetDateTime(15);
        TransRecord.DateDeleted = reader.GetDateTime(16);
        list.Add(TransRecord);
      }
      return list;
    }

    public static TransRecords FindNABTransaction(String NABOrderNmbr)
    {
      List<TransRecords> list = new List<TransRecords>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          string whereString = string.Empty;
          if (string.IsNullOrEmpty(NABOrderNmbr))
          {
            command.Parameters.Add("@P2", NABOrderNmbr);
            whereString += string.Format(" AND trn_nabordernumber = @P2 ");
          }
          command.CommandText = string.Format("SELECT {0} FROM {2} WHERE trn_datedeleted = @P1  {1} ORDER BY trn_transdate DESC ", TransRecordsFieldList, whereString );
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetAllTransRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return null;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count > 0)
        return list[0];
      else
        return null;

    }

    //Get the last Sell record 
    public static Boolean SetupLastSellRecords(String ASXCode, bool simulationRunning, out List<TransRecords> list)
    {
            //if (ASXCode != null)
            //{
            //    if (simulationRunning)
            //    {
            //        command.CommandText = "Select * from SimulationTransRecords where ASXCode = '" + ASXCode + "' order by TranDate Desc";
            //    }
            //    else
            //    {
            //        command.CommandText = "Select * from TransRecord where ASXCode = '" + ASXCode + "' order by TranDate Desc";
            //    }
            //}
            //LastSellReader = command.ExecuteReader();
            //return true;
      list = new List<TransRecords>();
      if (string.IsNullOrEmpty(ASXCode))
        return false;
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          string whereString = string.Empty;
          command.Parameters.Add("@P2", ASXCode);
          whereString += string.Format(" AND trn_ASXCode = @P2 ");
          command.CommandText = string.Format("SELECT {0} FROM {2} WHERE trn_datedeleted = @P1  {1} ORDER BY trn_transdate DESC ", TransRecordsFieldList, whereString, simulationRunning ? "simulationtransrecords" : "transrecord");
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetAllTransRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count > 0)
        return true;
      else
        return false;

    }

        public static int GetASXCodeSOH(String ASXCode, DateTime thsdte, bool simulationRunning)
        {
            //String tranTable = "TransRecord";
            //if (SimulationRunning)
            //    tranTable = "SimulationTransRecords";
            //command.CommandText = "Select * from [" + tranTable + "] where [" + tranTable + ".SOH] > 0 and  [" + tranTable + ".ASXCode] = '" + ASXCode + "' and " +
            //                      " [" + tranTable + ".TranDate] < #" + dt.ToString("yyyy-MM-dd") + "#";
            //return SOHValue;
      List<TransRecords> list = new List<TransRecords>();
      list = new List<TransRecords>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          command.Parameters.Add("@P2", thsdte);
          string ASXCodeString = string.Empty;
          if (ASXCode != null)
          {
            command.Parameters.Add("@P3", ASXCode);
            ASXCodeString = string.Format(" AND apd_ASXCode = @P3 ", ASXCode);
          }

          command.CommandText = string.Format("SELECT trn_SOH FROM {0} WHERE trn_datedeleted = @P1 AND trn_transdate < @P2 AND trn_ASXCode = @P3 AND trn_SOH > 0 ORDER BY trn_transdate DESC, apd_ASXCode ", simulationRunning ? "simulationtransrecords" : "transrecord");
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetAllTransRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return 0;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count <= 0)
        return 0;

      return list[0].SOH;
    }


    public static bool TransInsert(TransRecords myTransRecord)
    {
      return DBInsert(myTransRecord, "trantable", typeof(TransRecords));
    }

    public static void TransUpdate(TransRecords myTransRecord)
    {
      DBUpdate(myTransRecord, "trantable", typeof(TransRecords));
    }
        // ***************************************************************
        public class TodaysTrades
        {
            public int ID { get; set; }
            public String ASXCode { get; set; }
      public String BuySell { get; set; }
      public int TransQty { get; set; }
      public Decimal UnitPrice { get; set; }
      public String TransType { get; set; }
      public Decimal ROI { get; set; }
      public Decimal CurrPrc { get; set; }
      public Decimal TargetProfit { get; set; }
      public Decimal CurrProfit { get; set; }
      public Decimal PricePaid { get; set; }
      public Decimal DaysHeld { get; set; }
      public int RqdMove { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }

    public static bool PrepareForSuggestions()
    {
      return DeleteAll("todaystrades");
    }

    public static bool TodaysTradesInsert(TodaysTrades myTodaysTrades)
    {
      return DBInsert(myTodaysTrades, "todaystrades", typeof(TodaysTrades));
    }


    // *************************************************************************************************
    public static void PrepareForSimulation()
    {
      DeleteAll("SimulationTransRecords");
      DeleteAll("SimulationBankBal");
      DeleteAll("SimulationDivPaid");
    }

    //  *************************************************************************************************

    public class RelatedBuySellTrans
    {
      public int ID { get; set; }
      public int BuyId { get; set; }
      public int SellId { get; set; }
      public int TransQty { get; set; }
      public decimal TradeProfit { get; set; }
      public decimal DaysHeld { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
    public static string RelatedTransFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("relatedselltrans").ToArray()).Replace("\r\n", "");
      }
    }

    public static Boolean GetAllRelated(int buyId, int sellId, out List<RelatedBuySellTrans> list)
    {
      list = new List<RelatedBuySellTrans>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          string where = string.Empty;
          if (buyId != 0)
          {
            command.Parameters.Add("@P2", buyId);
            where += " AND rst_buyid = @P2 ";
          }
          if (sellId != 0)
          {
            command.Parameters.Add("@P3", sellId);
            where += " AND rst_sellid = @P3 ";
          }
          command.CommandText = string.Format("SELECT {0} FROM relatedselltrans WHERE rst_datedeleted = @P1  {1}  ", RelatedTransFieldList, where);
          command.Prepare();
          try
          {
            PgSqlDataReader priceReader = command.ExecuteReader();
            list = GetAllRelatedTrans(priceReader);
            if (list == null || list.Count <= 0)
              return false;
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      return true;
    }

    public static List<RelatedBuySellTrans> GetAllRelatedTrans(PgSqlDataReader priceReader)
    {
      List<RelatedBuySellTrans> inputList = new List<RelatedBuySellTrans>();
      while (priceReader.Read())
      {
        RelatedBuySellTrans rec = new RelatedBuySellTrans();
        rec.ID = priceReader.GetInt32(0);
        rec.BuyId = priceReader.GetInt32(1);
        rec.SellId = priceReader.GetInt32(2); ;
        rec.DateCreated = priceReader.GetDateTime(21);
        rec.DateModified = priceReader.GetDateTime(22);
        rec.DateDeleted = priceReader.GetDateTime(23);

        inputList.Add(rec);
      }

      return inputList;
    }

    //  **************************************************************************************************
    public class ASXPriceDate
    {
      public int ID { get; set; }
      public DateTime PriceDate { get; set; }
      public String ASXCode { get; set; }
      public Decimal PrcOpen { get; set; }
      public Decimal PrcHigh { get; set; }
      public Decimal PrcLow { get; set; }
      public Decimal PrcClose { get; set; }
      public int Volume { get; set; }
      public Decimal AdjClose { get; set; }
      public Decimal Day5Min { get; set; }
      public Decimal Day5Max { get; set; }
      public Decimal Day5Pct { get; set; }
      public Decimal Day30Min { get; set; }
      public Decimal Day30Max { get; set; }
      public Decimal Day30Pct { get; set; }
      public Decimal Day60Min { get; set; }
      public Decimal Day60Max { get; set; }
      public Decimal Day60Pct { get; set; }
      public Decimal Day90Min { get; set; }
      public Decimal Day90Max { get; set; }
      public Decimal Day90Pct { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
   

    public static string ASXPriceDateFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("asxpricedate").ToArray()).Replace("\r\n", "");
      }
    }
    public static Boolean GetAllPrices(String ASXCode, DateTime thsdte, out List<ASXPriceDate> list, string reqdFields)
    {
      list = new List<ASXPriceDate>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          command.Parameters.Add("@P2", thsdte);
          string ASXCodeString = string.Empty;
          if (ASXCode != null)
          {
            command.Parameters.Add("@P3", ASXCode);
            ASXCodeString = string.Format(" AND apd_ASXCode = @P3 ", ASXCode);
          }
          command.CommandText = string.Format("SELECT {0} FROM asxpricedate WHERE apd_datedeleted = @P1 AND apd_PriceDate >= @P2 {1} ORDER BY apd_PriceDate, apd_ASXCode ", reqdFields, ASXCodeString);
          command.Prepare();
          try
          {
            PgSqlDataReader priceReader = command.ExecuteReader();
            list = GetNextPriceDate(priceReader);
            if (list == null || list.Count <= 0)
              return false;
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      return true;
    }

    public static List<ASXPriceDate> GetNextPriceDate(PgSqlDataReader priceReader)
    {
      List<ASXPriceDate> inputList = new List<ASXPriceDate>();
      while (priceReader.Read())
      { 
        ASXPriceDate PrcDte = new ASXPriceDate();
                PrcDte.ID = priceReader.GetInt32(0);
                PrcDte.PriceDate = priceReader.GetDateTime(1);
                PrcDte.ASXCode = priceReader.GetString(2); ;
                PrcDte.PrcOpen = priceReader.GetDecimal(3);
                PrcDte.PrcHigh = priceReader.GetDecimal(4);
                PrcDte.PrcLow = priceReader.GetDecimal(5);
                PrcDte.PrcClose = priceReader.GetDecimal(6);
                PrcDte.Volume = priceReader.GetInt32(7);
                PrcDte.AdjClose = priceReader.GetDecimal(8);
                PrcDte.Day5Min = priceReader.GetDecimal(9);
                PrcDte.Day5Max = priceReader.GetDecimal(10);
                PrcDte.Day5Pct = priceReader.GetDecimal(11);
                PrcDte.Day30Min = priceReader.GetDecimal(12);
                PrcDte.Day30Max = priceReader.GetDecimal(13);
                PrcDte.Day30Pct = priceReader.GetDecimal(14);
                PrcDte.Day60Min = priceReader.GetDecimal(15);
                PrcDte.Day60Max = priceReader.GetDecimal(16);
                PrcDte.Day60Pct = priceReader.GetDecimal(17);
                PrcDte.Day90Min = priceReader.GetDecimal(18);
                PrcDte.Day90Max = priceReader.GetDecimal(19);
                PrcDte.Day90Pct = priceReader.GetDecimal(20);
                PrcDte.DateCreated = priceReader.GetDateTime(21);
                PrcDte.DateModified = priceReader.GetDateTime(22);
                PrcDte.DateDeleted = priceReader.GetDateTime(23);

        inputList.Add(PrcDte);
      }

      return inputList;
    }

    public static ASXPriceDate GetSpecificASXPriceRecord(string ASXCode, DateTime dt)
    {
      List<ASXPriceDate> list;
      if (GetAllPrices(ASXCode, dt, out list, ASXPriceDateFieldList))
        return list[0];
      else
        return null;
    }


    public static bool ASXprcInsert(ASXPriceDate myASXPrcRecord)
    {
      return DBInsert(myASXPrcRecord, "asxpricedate", typeof(ASXPriceDate));
    }

    public static bool ASXprcUpdate(ASXPriceDate myASXPrcRecord)
    {
      return DBUpdate(myASXPrcRecord, "asxpricedate", typeof(ASXPriceDate));
    }

    public static bool ASXprcDelete(ASXPriceDate myASXPrcRecord)
    {
      return DBDelete(myASXPrcRecord, "asxpricedate", typeof(ASXPriceDate));
    }


    public static decimal GetMinPrice(int Period, String ASXCode, DateTime thsdte)
    {
      DateTime previousDate = thsdte.AddDays(-(Double)Period);
      List<ASXPriceDate> list = new List<ASXPriceDate>();
      list = new List<ASXPriceDate>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          command.Parameters.Add("@P2", thsdte);
          string ASXCodeString = string.Empty;
          if (ASXCode != null)
          {
            command.Parameters.Add("@P3", ASXCode);
            ASXCodeString = string.Format(" AND apd_ASXCode = @P3 ", ASXCode);
          }
          command.Parameters.Add("@P4", previousDate);
          command.CommandText = string.Format("SELECT MIN(apd_prclow) FROM asxpricedate WHERE apd_datedeleted = @P1 AND apd_pricedate BETWEEN @P4 AND @P2 AND apd_ASXCode = @P3 ORDER BY apd_PriceDate, apd_ASXCode ",  ASXCodeString);
          command.Prepare();
          try
          {
            PgSqlDataReader priceReader = command.ExecuteReader();
            list = GetNextPriceDate(priceReader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return 0M;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count <= 0)
        return 0M;

      return list[0].PrcLow;
    }

    public static decimal GetMaxPrice(int Period, String ASXCode, DateTime thsdte)
    {
      DateTime previousDate = thsdte.AddDays(-(Double)Period);
      List<ASXPriceDate> list = new List<ASXPriceDate>();
      list = new List<ASXPriceDate>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P1", DateTime.MinValue);
          command.Parameters.Add("@P2", thsdte);
          string ASXCodeString = string.Empty;
          if (ASXCode != null)
          {
            command.Parameters.Add("@P3", ASXCode);
            ASXCodeString = string.Format(" AND apd_ASXCode = @P3 ", ASXCode);
          }
          command.Parameters.Add("@P4", previousDate);
          command.CommandText = string.Format("SELECT MAX(apd_prchigh) FROM asxpricedate WHERE apd_datedeleted = @P1 AND apd_pricedate BETWEEN @P4 AND @P2 AND apd_ASXCode = @P3 ORDER BY apd_PriceDate, apd_ASXCode ", ASXCodeString);
          command.Prepare();
          try
          {
            PgSqlDataReader priceReader = command.ExecuteReader();
            list = GetNextPriceDate(priceReader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return 0M;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count <= 0)
        return 0M;

      return list[0].PrcHigh;
    }

    public static bool GetPriceRecords(List<PgSqlParameter> paramList, out List<ASXPriceDate> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
    {
      list = new List<ASXPriceDate>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          if (!incDeleted)
            command.Parameters.Add("@P0", DateTime.MinValue);
          if (paramList != null)
            command.Parameters.AddRange(paramList.ToArray());

          command.CommandText = string.Format("SELECT {1} FROM {0} WHERE apd_datedeleted = @P0 {2} {3} ", "asxpricedate", reqdFields, extraWhere, orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetNextPriceDate(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count <= 0)
        return false;

      return true;
    }

    // *****************************************

    // Just get the company codes
    public class CompanyDetails
    {
      public int ID { get; set; }
      public DateTime PriceDate { get; set; }
      public String ASXCode { get; set; }
      public string CompanyName { get; set; }
      public DateTime FirstDividendDate { get; set; }
      public Decimal FirstDividendPerShare { get; set; }
      public DateTime SecondDividendDate { get; set; }
      public Decimal SecondDividendPerShare { get; set; }
      public Decimal EarningsPerShare { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }

    public static  string  CompanyDetailsFieldList
    {
      get
      {
        return string.Join(",",GetColumnNames("companydetails").ToArray());
      }
    }

    public static Boolean GetCompanyDetails(string ASXCode, out List<CompanyDetails> list)
    {
      list = new List<CompanyDetails>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          string ASXCodeString = string.Empty;
          if (ASXCode != null)
          {
            command.Parameters.Add("@P1", ASXCode);
            ASXCodeString = string.Format(" AND cod_ASXCode = '@P1' ", ASXCode);
          }
          command.CommandText = string.Format("SELECT {0} FROM companydetails WHERE cod_datedeleted = @P0 {1} ORDER BY cod_ASXCode ", CompanyDetailsFieldList.Replace("\r\n", ""), ASXCodeString);
          command.Prepare();
          try
          {
            PgSqlDataReader priceReader = command.ExecuteReader();
            list = GetCompanyDetails(priceReader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      return true;
    }

    public static List<CompanyDetails> GetCompanyDetails(PgSqlDataReader reader)
    {
      List<CompanyDetails> inputList = new List<CompanyDetails>();
      while (reader.Read())
      {
        CompanyDetails dtls = new CompanyDetails();
        dtls.ID = reader.GetInt32(0);
        dtls.PriceDate = reader.GetDateTime(1);
        dtls.ASXCode = reader.GetString(2); ;
        dtls.CompanyName = reader.GetString(3);
        dtls.FirstDividendDate = reader.GetDateTime(4);
        dtls.FirstDividendPerShare = reader.GetDecimal(5);
        dtls.SecondDividendDate = reader.GetDateTime(6);
        dtls.SecondDividendPerShare = reader.GetDecimal(7);
        dtls.EarningsPerShare = reader.GetDecimal(8);
        dtls.DateCreated = reader.GetDateTime(9);
        dtls.DateModified = reader.GetDateTime(10);
        dtls.DateDeleted = reader.GetDateTime(11);

        inputList.Add(dtls);
      }
      return inputList;
    }




        //****************************************************************************
        public class TransImport
        {
            public int ID { get; set; }
            public String ASXCode { get; set; }
            public DateTime TranDate { get; set; }
            public String BuySell { get; set; }
            public int TransQty { get; set; }
            public Decimal UnitPrice { get; set; }
            public String NABOrderNmbr { get; set; }
        }
    public static string TransImportFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("transimport").ToArray());
      }
    }
    public static Boolean GetAllTransImpRecords(out List<TransImport> list)
        {
            //OleDbCommand command = new OleDbCommand();
            //command.Connection = connectionTransImpRecs;
            //if (connectionTransImpRecs.State == System.Data.ConnectionState.Open)
            //    connectionTransImpRecs.Close();
            //connectionTransImpRecs.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Dvl\Rays Projects\Shares\ShareAnalV2.accdb; Persist Security Info = False;";
            //connectionTransImpRecs.Open();
            //command.CommandText = "Select * from TransImport order by TranDate asc";
            //try
            //{
            //    TransImportReader = command.ExecuteReader();
            //}
            //catch (Exception ex)
            //{
            //    Console.Write("Exception " + ex.ToString());
            //    return false;
            //}
            //return true;
      list = new List<TransImport>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          command.CommandText = string.Format("SELECT {0} FROM transimport {1} ORDER BY cod_ASXCode ", TransImportFieldList.Replace("\r\n", ""));
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetTransImpRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      return true;
    }


   public static List<TransImport> GetTransImpRecords(PgSqlDataReader reader)
   {
      List<TransImport> inputList = new List<TransImport>();
      while (reader.Read())
      {

        TransImport TransRecord = new TransImport();
        TransRecord.ID = reader.GetInt32(0);
        TransRecord.TranDate = reader.GetDateTime(1);
        TransRecord.BuySell = reader.GetString(2);
        TransRecord.ASXCode = reader.GetString(3);
        TransRecord.UnitPrice = reader.GetDecimal(4);
        TransRecord.TransQty = reader.GetInt32(5);
        TransRecord.NABOrderNmbr = reader.GetString(6);
        inputList.Add(TransRecord);
      }
      return inputList;
   }

   public static void TransImpDelete(TransImport myTransImpRecord)
   {
      DBDelete(myTransImpRecord, "transimport", typeof(TransImport));
    }

    // **************************************************************************
    //
    //      General Ledger Codes
    public class GLCodes
    {
      public int ID { get; set; }
      public GLType Type { get; set; }
      public string GLCode { get; set; }
      public bool GSTApplies { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
    public static string GLFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("glcodes").ToArray());
      }
    }

    public static bool GetGLRecords(List<PgSqlParameter> paramList, out List<GLCodes> list, string reqdFields, string extraWhere, string orderBy)
    {
      list = null;
      return GetGLRecords(paramList, out list, reqdFields, extraWhere, orderBy, false);
    }
    public static bool GetGLRecords(List<PgSqlParameter> paramList, out List<GLCodes> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
    {
      list = new List<GLCodes>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          if (!incDeleted)
            command.Parameters.Add("@P0", DateTime.MinValue);
          if (paramList != null)
            command.Parameters.AddRange(paramList.ToArray());

          if (incDeleted)
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE 1 = 1  {2} {3} ", "glcodes", reqdFields, extraWhere, orderBy);
          else
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE gl_datedeleted = @P0 {2} {3} ",  "glcodes", reqdFields, extraWhere, orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetGLRecords(reader);
          }
          catch (Exception ex)
          {
            Console.Write("Exception " + ex.ToString());
            return false;
          }
        }
        catch (System.Data.Common.DbException pex)
        {
          throw new DatabaseIOException(pex.Message, pex);
        }
        finally
        {
          if (conn != null)
            conn.Close();
        }
      }
      if (list.Count <= 0)
        return false;

      return true;
    }

    public static List<GLCodes> GetGLRecords(PgSqlDataReader reader)
    {
      List<GLCodes> inputList = new List<GLCodes>();
      while (reader.Read())
      {
        GLCodes dtls = new GLCodes();
        dtls.ID = reader.GetInt32(0);
        dtls.Type = (GLType) reader.GetInt32(1);
        dtls.GLCode = reader.GetString(2); ;
        dtls.GSTApplies = reader.GetString(3) == "Y";
        dtls.DateCreated = reader.GetDateTime(4);
        dtls.DateModified = reader.GetDateTime(5);
        dtls.DateDeleted = reader.GetDateTime(6);

        inputList.Add(dtls);
      }
      return inputList;
    }

    public static string getGLCode(int v)
    {
      List<GLCodes> list = new List<GLCodes>();
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", v));
      if (DBAccess.GetGLRecords(paramList, out list, GLFieldList, " AND gl_type = @P1 ", string.Empty))
        return list[0].GLCode.Replace("-", "");
      else
        return string.Empty;
    }
    public static string getTaxCode(int v)
    {
      List<GLCodes> list = new List<GLCodes>();
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", v));
      if (DBAccess.GetGLRecords(paramList, out list, GLFieldList, " AND gl_type = @P1 ", string.Empty))
        return list[0].GSTApplies ? "GST" : "N-T"; 
      else
        return string.Empty;
    }

    public enum GLType
    {
      [Description("Trading Chq A/c")]
      ShareChqAct = 1,
      [Description("Trading Asset")]
      ShareAsset = 2,
      [Description("Trading Profit")]
      TradingProfit = 3,
      [Description("Trading Brokerage")]
      TradingBrokerage = 4,
      [Description("Dividend Chq A/c")]
      DivChqAct = 5,
      [Description("Dividend Franking Cr")]
      DivFrCredit = 6,
      [Description("Dividend W/Hold Tax")]
      DivWHoldTax = 7
    }
  }
}
// *************************************************************************************************



