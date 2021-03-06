﻿
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
          {
            if (memberList[i -1].memberName.Contains("ate"))
            { }
            command.Parameters.Add(memberList[i - 1].paramName, memberList[i - 1].memberName.Contains("DateCreated") || memberList[i - 1].memberName.Contains("DateModified") ? DateTime.Now : memberList[i - 1].memberValue);
          }
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
            command.Parameters.Add(memberList[i - 1].paramName, memberList[i - 1].memberName.Contains("DateModified") ? DateTime.Now : memberList[i - 1].memberValue);

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
      public Decimal WithholdingTax { get; set; }
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
    public static void DividendHistoryUpdate(DividendHistory myDividend)
    {
      DBUpdate(myDividend, "dividendhistory", typeof(DividendHistory));
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
    public static Boolean GetDividends(String ASXCode, DateTime thsdte, out List<DividendHistory> list, dirn op)
    {
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      string extraWhere = string.Empty;
      string orderBy = " ORDER BY dvh_exdivdate DESC, dvh_ASXCode ";

      if (DateTime.Compare(thsdte, DateTime.MinValue) != 0)
      {
        paramList.Add(new PgSqlParameter("@P1", thsdte));
        extraWhere += string.Format(" AND dvh_exdivdate {0} @P1 ", getMathOp(op));
      }
      if (ASXCode != null)
      {
        paramList.Add(new PgSqlParameter("@P2", ASXCode));
        extraWhere += " AND dvh_asxcode = @P2 ";
      }
      return GetDividends(paramList, out list, extraWhere, orderBy);
    }

    public static Boolean GetDividends(List<PgSqlParameter> paramList, out List<DividendHistory> list, string extraWhere, string orderBy)
    { 
      list = new List<DividendHistory>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          string whereString = extraWhere;
          command.Parameters.AddRange(paramList.ToArray());
          command.CommandText = string.Format("SELECT {0} FROM {2} WHERE dvh_datedeleted = @P0  {1} {3} ", DividendHistoryFieldList, whereString, "dividendhistory", orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetDividendHistory(reader);
            return list.Count() > 0;
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
        dividendRecord.WithholdingTax = reader.GetDecimal(9);
        dividendRecord.DateCreated = reader.GetDateTime(10);
        dividendRecord.DateModified = reader.GetDateTime(11);
        dividendRecord.DateDeleted = reader.GetDateTime(12);
        list.Add(dividendRecord);
      }
      return list;
    }

    public static  int CalculateSOHOnDivDate(DBAccess.DividendHistory divHistoryRec)
    {
      if (divHistoryRec.ASXCode == "BXB")
      { }
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
          if (SOHonDividendDate > 1000)
          { }
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
      return SOHonDividendDate;

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
      public DateTime ExDividendDate { get; set; }
      public DateTime DatePaid { get; set; }
      public Decimal GrossDividendPerShare { get; set; }
      public Decimal FrCreditPerShare { get; set; }
      public Decimal AmtPaidPerShare { get; set; }
      public int QtyShares { get; set; }
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


    public static Boolean GetDividendPaidRecords(string ASXCode, DateTime divPdDate, out List<DivPaid> list, bool runningSimulation)
    {
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      string where = string.Empty;
      string orderBy = string.Empty;

      where += "AND dvp_datepaid = @P1 ";
      paramList.Add(new PgSqlParameter("@P1", divPdDate));
      if (!string.IsNullOrEmpty(ASXCode))
      {
        where += " AND dvp_asxcode = @P2 ";
        paramList.Add(new PgSqlParameter("@P2", ASXCode));
      }
      return GetDividendPaidRecords(paramList, out list, DividendPaidFieldList,  where, orderBy, runningSimulation);

    }
    public static Boolean GetDividendPaidRecords(List<PgSqlParameter> paramList, out List<DivPaid> list, string fieldList, string whereClause, string orderByClause, bool runningSimulation)
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
          if (paramList != null)
            command.Parameters.AddRange(paramList.ToArray());

          command.CommandText = string.Format("SELECT {0} FROM {3} WHERE dvp_datedeleted = @P0  {1} {2} ", fieldList, whereClause, orderByClause, !runningSimulation ? "dividendpaid" : "simulationdividendpaid");
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetDivPaidRecords(reader);
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

    public static List<DivPaid> GetDivPaidRecords(PgSqlDataReader reader)
    {
      List<DivPaid> list = new List<DivPaid>();
      while (reader.Read())
      {
        DivPaid DivPaid = new DivPaid();
         DivPaid.ID = reader.GetInt32(0);
         DivPaid.ASXCode = reader.GetString(1);
         DivPaid.ExDividendDate = reader.GetDateTime(2);
         DivPaid.DatePaid = reader.GetDateTime(3);
         DivPaid.GrossDividendPerShare = reader.GetDecimal(4);
         DivPaid.FrCreditPerShare = reader.GetDecimal(5);
         DivPaid.AmtPaidPerShare = reader.GetDecimal(6);
         DivPaid.QtyShares = reader.GetInt32(7);
         DivPaid.DateCreated = reader.GetDateTime(8);
         DivPaid.DateModified = reader.GetDateTime(9);
         DivPaid.DateDeleted = reader.GetDateTime(10);
         list.Add(DivPaid);
      }
      return list;
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
      public string SellConfirmation { get; set; }      // may be manually filled in to force a match with this buy
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
        if (reader.FieldCount == 1)               // SUM(trn_SOH)
          TransRecord.SOH = reader.GetInt32(0);
        else
        {
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
          TransRecord.SellConfirmation = reader.GetString(17);
        }
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
      return DBInsert(myTransRecord, "transrecord", typeof(TransRecords));
    }

    public static void TransUpdate(TransRecords myTransRecord)
    {
      DBUpdate(myTransRecord, "transrecord", typeof(TransRecords));
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
      public Decimal SaleBrokerage { get; set; }
    }
    public static string RelatedTransFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("relatedbuyselltrans").ToArray()).Replace("\r\n", "");
      }
    }

    public static Boolean GetAllRelated(int buyId, int sellId, out List<RelatedBuySellTrans> list)
    {
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      string where = string.Empty;
      if (buyId != 0)
      {
        paramList.Add(new PgSqlParameter("@P2", buyId));
        where += " AND rbs_buyid = @P2 ";
      }
      if (sellId != 0)
      {
        paramList.Add(new PgSqlParameter("@P3", sellId));
        where += " AND rbs_sellid = @P3 ";
      }
      list = new List<RelatedBuySellTrans>();
      return GetAllRelated(paramList, out list, RelatedTransFieldList, where, string.Empty);


    }
    public static Boolean GetAllRelated(List<PgSqlParameter> paramList, out List<RelatedBuySellTrans> list, string fieldList, string whereClause, string orderBy)
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
          command.Parameters.AddRange(paramList.ToArray());
          command.CommandText = string.Format("SELECT {0} FROM relatedbuyselltrans WHERE rbs_datedeleted = @P1  {1} {2}  ", fieldList, whereClause, orderBy);
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
        rec.SellId = priceReader.GetInt32(2);
        rec.TransQty = priceReader.GetInt32(3);
        rec.TradeProfit = priceReader.GetDecimal(4);
        rec.DaysHeld = priceReader.GetDecimal(5);
        rec.DateCreated = priceReader.GetDateTime(6);
        rec.DateModified = priceReader.GetDateTime(7);
        rec.DateDeleted = priceReader.GetDateTime(8);
        rec.SaleBrokerage = priceReader.GetDecimal(9);
        inputList.Add(rec);
      }

      return inputList;
    }

    //  **************************************************************************************************
    public class ASXPriceDate
    {
      public long ID { get; set; }
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
      public String RecalcReqd { get; set; }

      public ASXPriceDate()
      {
        RecalcReqd = "N";
      }
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
      string orderBy = " ORDER BY apd_PriceDate, apd_ASXCode ";
      string extraWhere = "  AND apd_PriceDate >= @P1 ";
      
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", thsdte));
      if (ASXCode != null)
      {
        paramList.Add(new PgSqlParameter("@P2", ASXCode));
        extraWhere += string.Format(" AND apd_ASXCode = @P2 ", ASXCode);
      }

      return GetAllPrices(paramList, out list, reqdFields, extraWhere, orderBy);
    }

    public static Boolean GetAllPrices(List<PgSqlParameter> paramList, out List<ASXPriceDate> list, string reqdFields, string extraWhere, string orderBy)
    { 
      list = new List<ASXPriceDate>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          command.Parameters.Add("@P0", DateTime.MinValue);
          command.Parameters.AddRange(paramList.ToArray());
          string ASXCodeString = string.Empty;
          command.CommandText = string.Format("SELECT {0} FROM asxpricedate WHERE apd_datedeleted = @P0  {1} {2} ", reqdFields, extraWhere, orderBy);
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
      return GetNextPriceDate(priceReader, false);
    }
    public static List<ASXPriceDate> GetNextPriceDate(PgSqlDataReader priceReader, bool maxPricedate)
    {
      List<ASXPriceDate> inputList = new List<ASXPriceDate>();
      while (priceReader.Read())
      { 
        ASXPriceDate PrcDte = new ASXPriceDate();
        if (priceReader.FieldCount == 1 && !maxPricedate)
          PrcDte.ASXCode = priceReader.GetString(0);
        else
          if (priceReader.FieldCount == 1 && maxPricedate)
            PrcDte.PriceDate = priceReader.GetDateTime(0);
        else
        {

          PrcDte.ID = priceReader.GetInt64(0);
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
          PrcDte.RecalcReqd = priceReader.GetString(24);
        }

        inputList.Add(PrcDte);
      }

      return inputList;
    }
    public static ASXPriceDate GetSpecificASXPriceRecord(string ASXCode, DateTime dt)
    {
      return GetSpecificASXPriceRecord(ASXCode, dt, false);
    }

    public static ASXPriceDate GetSpecificASXPriceRecord(string ASXCode, DateTime dt, bool exact)
    {
      List<ASXPriceDate> list;
      if (GetAllPrices(ASXCode, dt, out list, ASXPriceDateFieldList))
      {
        if (!exact)
          return list[0];
        else
        {
          List<ASXPriceDate> spec = list.FindAll(delegate (ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, dt) == 0; });
          if (spec.Count > 0)
            return spec[0];
          else
            return null;
        }
      }
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
      bool maxPricedate = reqdFields.Contains("MAX(apd_pricedate)");
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
            list = GetNextPriceDate(reader, maxPricedate);
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
      public String ASXCode { get; set; }
      public string CompanyName { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
      public Boolean OnWatchList { get; set; }
      public int GICSSubIndusId { get; set; }
      public Boolean InMarketIndex { get; set; }
    }

    public static  string  CompanyDetailsFieldList
    {
      get
      {
        return string.Join(",",GetColumnNames("companydetails").ToArray());
      }
    }

    public static List<string> GetASXCodes()
    {
      return GetASXCodes(false);
    }
    public static List<string> GetASXCodes(bool onlyWatchList)
    { 
      List<CompanyDetails> list = null;
      List<string> codeList = new List<string>();
      if (DBAccess.GetCompanyDetails(null, out list, false, onlyWatchList))
        if (onlyWatchList)
          codeList = list.Where(z => z.OnWatchList == onlyWatchList).Select(x => x.ASXCode).Distinct().OrderBy(y => y).ToList();
      else
          codeList = list.Select(x => x.ASXCode).Distinct().OrderBy(y => y).ToList();
      return codeList;
    }

    public static Boolean GetCompanyDetails(string ASXCode, out List<CompanyDetails> list)
    {
      
      return GetCompanyDetails(ASXCode, out list, false, false);
    }
    public static Boolean GetCompanyDetails(string ASXCode, out List<CompanyDetails> list, bool incDeleted, bool onWatchlistOnly)
    {
      list = new List<CompanyDetails>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          string ASXCodeString = string.Empty;
          if (!incDeleted)
          {
            command.Parameters.Add("@P0", DateTime.MinValue);
            ASXCodeString += " AND cod_datedeleted = @P0 ";
          }
          if (!string.IsNullOrEmpty(ASXCode))
          {
            command.Parameters.Add("@P1", ASXCode);
            ASXCodeString += " AND cod_ASXCode = @P1 ";
          }

          if (onWatchlistOnly)
          {
            command.Parameters.Add("@P2", "Y");
            ASXCodeString += " AND cod_isonwatchlist = @P2 ";
          }
          command.CommandText = string.Format("SELECT {0} FROM companydetails WHERE 1 = 1 {1} ORDER BY cod_ASXCode ", CompanyDetailsFieldList.Replace("\r\n", ""), ASXCodeString);
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
      if (list.Count <= 0)
        return false;

      return true;
    }

    public static List<CompanyDetails> GetCompanyDetails(PgSqlDataReader reader)
    {
      List<CompanyDetails> inputList = new List<CompanyDetails>();
      while (reader.Read())
      {
        CompanyDetails dtls = new CompanyDetails();
        dtls.ID = reader.GetInt32(0);
        dtls.ASXCode = reader.GetString(1); ;
        dtls.CompanyName = reader.GetString(2);
        dtls.DateCreated = reader.GetDateTime(3);
        dtls.DateModified = reader.GetDateTime(4);
        dtls.DateDeleted = reader.GetDateTime(5);
        dtls.OnWatchList = reader.GetBoolean(6);
        dtls.GICSSubIndusId = reader.GetInt32(7);
        dtls.InMarketIndex = reader.GetBoolean(8);

        inputList.Add(dtls);
      }
      return inputList;
    }


    // *****************************************  Directors Transactions
    public class DirectorsTransactions
    {
      public long ID { get; set; }
      public String ASXCodeDirectors { get; set; }
      public DateTime TransDateDirectors { get; set; }
      public string NameDirectors { get; set; }
      public string Type { get; set; }
      public int QtyShares { get; set; }
      public decimal Price { get; set; }
      public decimal Value { get; set; }
      public string Notes { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }

    public static string DirectorsTransactionsFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("directors_transactions").ToArray());
      }
    }

    public static Boolean GetDirectorsTransactions(DBAccess.DirectorsTransactions inRec, out List<DirectorsTransactions> list)
    {
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", inRec.ASXCodeDirectors));
      paramList.Add(new PgSqlParameter("@P2", inRec.TransDateDirectors));
      paramList.Add(new PgSqlParameter("@P3", inRec.NameDirectors));
      paramList.Add(new PgSqlParameter("@P4", inRec.Type));
      //paramList.Add(new PgSqlParameter("@P5", inRec.QtyShares));
      //paramList.Add(new PgSqlParameter("@P6", inRec.Price));

      string extraWhere = string.Empty;
      extraWhere += " AND dt_asxcode = @P1 ";
      extraWhere += " AND dt_transdate = @P2 ";
      extraWhere += " AND dt_type = @P4 ";
      extraWhere += " AND dt_name = @P3 ";
      //extraWhere += " AND dt_qty = @P5 ";
      //extraWhere += " AND dt_price = @P6 ";
      return GetDirectorsTransactions(paramList, out list, DBAccess.DirectorsTransactionsFieldList, extraWhere, string.Empty, false);
    }

    public static Boolean GetDirectorsTransactions(string ASXCode, out List<DirectorsTransactions> list)
    {

      return GetDirectorsTransactions(ASXCode, out list, false, false);
    }
    public static Boolean GetDirectorsTransactions(string ASXCode, out List<DirectorsTransactions> list, bool incDeleted, bool onWatchlistOnly)
    {
      list = new List<DirectorsTransactions>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          string ASXCodeString = string.Empty;
          if (!incDeleted)
          {
            command.Parameters.Add("@P0", DateTime.MinValue);
            ASXCodeString += " AND dt_datedeleted = @P0 ";
          }
          if (!string.IsNullOrEmpty(ASXCode))
          {
            command.Parameters.Add("@P1", ASXCode);
            ASXCodeString += " AND dt_ASXCode = @P1 ";
          }

          //if (onWatchlistOnly)
          //{
          //  command.Parameters.Add("@P2", "Y");
          //  ASXCodeString += " AND cod_isonwatchlist = @P2 ";
          //}
          command.CommandText = string.Format("SELECT {0} FROM directors_transactions WHERE 1 = 1 {1} ORDER BY dt_ASXCode ", DirectorsTransactionsFieldList.Replace("\r\n", ""), ASXCodeString);
          command.Prepare();
          try
          {
            PgSqlDataReader priceReader = command.ExecuteReader();
            list = GetDirectorsTransactions(priceReader);
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

    public static bool GetDirectorsTransactions(List<PgSqlParameter> paramList, out List<DirectorsTransactions> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
    {
      list = new List<DirectorsTransactions>();
      using (PgSqlConnection conn = new PgSqlConnection(DBConnectString()))
      {
        try
        {
          conn.Open();
          PgSqlCommand command = new PgSqlCommand();
          command.Connection = conn;
          List<PgSqlParameter> allParams = new List<PgSqlParameter>();
          if (!incDeleted)
            allParams.Add(new PgSqlParameter("@P0", DateTime.MinValue));
          if (paramList != null)
            allParams.AddRange(paramList.ToArray());
          command.Parameters.AddRange(allParams.ToArray());
          command.CommandText = string.Format("SELECT {1} FROM {0} WHERE dt_datedeleted = @P0 {2} {3} ", "directors_transactions", reqdFields, extraWhere, orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetDirectorsTransactions(reader);
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
    public static List<DirectorsTransactions> GetDirectorsTransactions(PgSqlDataReader reader)
    {
      List<DirectorsTransactions> inputList = new List<DirectorsTransactions>();
      while (reader.Read())
      {
        DirectorsTransactions dtls = new DirectorsTransactions();
        dtls.ID = reader.GetInt64(0);
        dtls.ASXCodeDirectors = reader.GetString(1);
        dtls.TransDateDirectors = reader.GetDateTime(2);
        dtls.NameDirectors = reader.GetString(3);
        dtls.Type = reader.GetString(4);
        dtls.QtyShares = reader.GetInt32(5);
        dtls.Price = reader.GetDecimal(6);
        dtls.Value = reader.GetDecimal(7);
        dtls.Notes = reader.GetString(8);
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

    // *************************************  Statistics  ******************************************
    public class Statistics
    {
      public int ID { get; set; }
      public StatsType Type { get; set; }
      public string ASXCode { get; set; }
      public DateTime  StartDate { get; set; }
      public Decimal Stat { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
    public static string StatFieldList
    {
      get
      {
        return string.Join(",", GetColumnNames("statistics").ToArray());
      }
    }

    public static bool GetStatRecords(List<PgSqlParameter> paramList, out List<Statistics> list, string reqdFields, string extraWhere, string orderBy)
    {
      list = null;
      return GetStatsRecords(paramList, out list, reqdFields, extraWhere, orderBy, false);
    }
    public static bool GetStatsRecords(List<PgSqlParameter> paramList, out List<Statistics> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
    {
      list = new List<Statistics>();
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
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE 1 = 1  {2} {3} ", "statistics", reqdFields, extraWhere, orderBy);
          else
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE st_datedeleted = @P0 {2} {3} ", "statistics", reqdFields, extraWhere, orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetStatsRecords(reader, reqdFields.Contains("MAX(st_startdate)"));
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

    public static List<Statistics> GetStatsRecords(PgSqlDataReader reader, bool dateOnly)
    {
      List<Statistics> inputList = new List<Statistics>();
      while (reader.Read())
      {
        Statistics dtls = new Statistics();
        if (dateOnly)
          dtls.StartDate = reader.GetDateTime(3);
        else
        {
          dtls.ID = reader.GetInt32(0);
          dtls.Type = (StatsType)reader.GetInt32(1);
          dtls.ASXCode = reader.GetString(2);
          dtls.StartDate = reader.GetDateTime(3);
          dtls.Stat = reader.GetDecimal(4);
          dtls.DateCreated = reader.GetDateTime(5);
          dtls.DateModified = reader.GetDateTime(6);
          dtls.DateDeleted = reader.GetDateTime(7);
        }

        inputList.Add(dtls);
      }
      return inputList;
    }
 
// *************************************  GICS Sub Industry Code  ******************************************
public class GICSSubIndustryCode
{
  public int ID { get; set; }
  public string Name { get; set; }
  public int IndusCodeId { get; set; }
  public DateTime DateCreated { get; set; }
  public DateTime DateModified { get; set; }
  public DateTime DateDeleted { get; set; }
}
public static string GICSSubIndusList
{
  get
  {
    return string.Join(",", GetColumnNames("sub_industry_code").ToArray());
  }
}

    public static GICSSubIndustryCode GetSpecificSubIndusRecord(string name)
    {
      List<GICSSubIndustryCode> list;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", name));
      string where = " AND sic_name = @P1 ";

      if (GetSubIndusRecords(paramList, out list, GICSSubIndusList, where, string.Empty))
        return list[0];
      else
      {
        List<PgSqlParameter> paramList1 = new List<PgSqlParameter>();
        paramList1.Add(new PgSqlParameter("@P1", name));

        GICSSubIndustryCode entry = new GICSSubIndustryCode();
        entry.DateCreated = DateTime.Now;
        entry.DateCreated = DateTime.MinValue;
        entry.DateModified = entry.DateCreated;
        entry.IndusCodeId = 0;
        entry.Name = name;
        if (DBInsert(entry, "sub_industry_code", typeof(GICSSubIndustryCode)))
        {
          if (GetSubIndusRecords(paramList1, out list, GICSSubIndusList, where, string.Empty))
            return list[0];
          else
            return null;
        }
        else
          return null;

      }
        
    }

    public static bool GetSubIndusRecords(List<PgSqlParameter> paramList, out List<GICSSubIndustryCode> list, string reqdFields, string extraWhere, string orderBy)
{
  list = null;
  return GetSubIndusRecords(paramList, out list, reqdFields, extraWhere, orderBy, false);
}
public static bool GetSubIndusRecords(List<PgSqlParameter> paramList, out List<GICSSubIndustryCode> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
{
  list = new List<GICSSubIndustryCode>();
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
        command.CommandText = string.Format("SELECT {1} FROM {0} WHERE 1 = 1  {2} {3} ", "sub_industry_code", reqdFields, extraWhere, orderBy);
      else
        command.CommandText = string.Format("SELECT {1} FROM {0} WHERE sic_datedeleted = @P0 {2} {3} ", "sub_industry_code", reqdFields, extraWhere, orderBy);
      command.Prepare();
      try
      {
        PgSqlDataReader reader = command.ExecuteReader();
        list = GetSubIndusRecords(reader);
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

public static List<GICSSubIndustryCode> GetSubIndusRecords(PgSqlDataReader reader)
{
  List<GICSSubIndustryCode> inputList = new List<GICSSubIndustryCode>();
  while (reader.Read())
  {
        GICSSubIndustryCode dtls = new GICSSubIndustryCode();

      dtls.ID = reader.GetInt32(0);
      dtls.Name = reader.GetString(1);
      dtls.IndusCodeId = reader.GetInt32(2);
      dtls.DateCreated = reader.GetDateTime(3);
      dtls.DateModified = reader.GetDateTime(4);
      dtls.DateDeleted = reader.GetDateTime(5);

    inputList.Add(dtls);
  }
  return inputList;
}

    // *************************************  Company History  ******************************************
    public class CompanyHistory
    {
      public long ID { get; set; }
      public string ASXCode { get; set; }
      public DateTime HistoryDate { get; set; }
      public string MarketCap { get; set; }
      public int AvgVol { get; set; }
      public decimal TrailingPE { get; set; }
      public decimal ForwardPE { get; set; }
      public decimal Beta { get; set; }
      public decimal DebtEquity { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
    public static string CompanyHistoryList
    {
      get
      {
        return string.Join(",", GetColumnNames("company_history").ToArray());
      }
    }

    public static bool GetCompanyHistoryRecords(List<PgSqlParameter> paramList, out List<CompanyHistory> list, string reqdFields, string extraWhere, string orderBy)
    {
      list = null;
      return GetCompanyHistoryRecords(paramList, out list, reqdFields, extraWhere, orderBy, false);
    }
    public static bool GetCompanyHistoryRecords(List<PgSqlParameter> paramList, out List<CompanyHistory> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
    {
      list = new List<CompanyHistory>();
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
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE 1 = 1  {2} {3} ", "company_history", reqdFields, extraWhere, orderBy);
          else
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE ch_datedeleted = @P0 {2} {3} ", "company_history", reqdFields, extraWhere, orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetCompanyHistoryRecords(reader);
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

    public static List<CompanyHistory> GetCompanyHistoryRecords(PgSqlDataReader reader)
    {
      List<CompanyHistory> inputList = new List<CompanyHistory>();
      while (reader.Read())
      {
        CompanyHistory dtls = new CompanyHistory();

        dtls.ID = reader.GetInt64(0);
        dtls.ASXCode = reader.GetString(1);
        dtls.HistoryDate = reader.GetDateTime(2);
        dtls.MarketCap = reader.GetString(3);
        dtls.AvgVol = reader.GetInt32(4);
        dtls.TrailingPE = reader.GetDecimal(5);
        dtls.ForwardPE = reader.GetDecimal(6);
        dtls.Beta = reader.GetDecimal(7);
        dtls.DebtEquity = reader.GetDecimal(8);
        dtls.DateCreated = reader.GetDateTime(9);
        dtls.DateModified = reader.GetDateTime(10);
        dtls.DateDeleted = reader.GetDateTime(11);

        inputList.Add(dtls);
      }
      return inputList;
    }
    // *************************************  Brokers recommendations  ******************************************
    public class BrokersRecommendations
    {
      public long ID { get; set; }
      public string ASXCode { get; set; }
      public DateTime HistoryDate { get; set; }
      public string Consensus { get; set; }
      public int Buy { get; set; }
      public int Sell { get; set; }
      public int Hold { get; set; }
      public decimal Price { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
    public static string BrokersRecommendationsList
    {
      get
      {
        return string.Join(",", GetColumnNames("brokers_recommendations").ToArray());
      }
    }

    public static BrokersRecommendations GetSpecificBrokersRecommendationRecord(BrokersRecommendations rec)
    {
      List<BrokersRecommendations> list = new List<BrokersRecommendations>();
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", rec.HistoryDate));
      paramList.Add(new PgSqlParameter("@P2", rec.ASXCode));
      string sqlWhere = " AND br_transdate = @P1 AND br_asxcode = @P2 ";
      if (GetBrokersrecommendationsRecords(paramList, out list, BrokersRecommendationsList, sqlWhere, string.Empty))
        return list[0];
      else
        return null;
    }
    public static bool GetBrokersrecommendationsRecords(List<PgSqlParameter> paramList, out List<BrokersRecommendations> list, string reqdFields, string extraWhere, string orderBy)
    {
      list = null;
      return GetBrokersRecommendationsRecords(paramList, out list, reqdFields, extraWhere, orderBy, false);
    }
    public static bool GetBrokersRecommendationsRecords(List<PgSqlParameter> paramList, out List<BrokersRecommendations> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
    {
      list = new List<BrokersRecommendations>();
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
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE 1 = 1  {2} {3} ", "brokers_recommendations", reqdFields, extraWhere, orderBy);
          else
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE br_datedeleted = @P0 {2} {3} ", "brokers_recommendations", reqdFields, extraWhere, orderBy);
          Console.WriteLine(command.CommandText);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            bool justDate = reqdFields.Contains("DISTINCT(br_transdate)");

            list = GetBrokersRecommendationsRecords(reader, justDate);
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

    public static List<BrokersRecommendations> GetBrokersRecommendationsRecords(PgSqlDataReader reader, bool justDate)
    {

      List<BrokersRecommendations> inputList = new List<BrokersRecommendations>();
      while (reader.Read())
      {
        BrokersRecommendations dtls = new BrokersRecommendations();
        if (justDate)
        {
          dtls.HistoryDate = reader.GetDateTime(0);
          inputList.Add(dtls);
          continue;
        }
        dtls.ID = reader.GetInt64(0);
        dtls.ASXCode = reader.GetString(1);
        dtls.HistoryDate = reader.GetDateTime(2);
        dtls.Consensus = reader.GetString(3);
        dtls.Buy = reader.GetInt32(4);
        dtls.Sell = reader.GetInt32(5);
        dtls.Hold = reader.GetInt32(6);
        dtls.Price = reader.GetDecimal(7);
        dtls.DateCreated = reader.GetDateTime(8);
        dtls.DateModified = reader.GetDateTime(9);
        dtls.DateDeleted = reader.GetDateTime(10);

        inputList.Add(dtls);
      }
      return inputList;
    }
    // *************************************  System Variables  ******************************************
    public class SystemVars
    {
      public int ID { get; set; }
      public string Description { get; set; }
      public string Status { get; set; }
      public DateTime VarDate { get; set; }
      public string Notes { get; set; }
      public int IntVar1 { get; set; }
      public int IntVar2 { get; set; }
      public decimal DecVar1 { get; set; }
      public decimal DecVar2 { get; set; }
      public DateTime DateCreated { get; set; }
      public DateTime DateModified { get; set; }
      public DateTime DateDeleted { get; set; }
    }
    public static string SystemVarsList
    {
      get
      {
        return string.Join(",", GetColumnNames("system_vars").ToArray());
      }
    }

    public static SystemVars GetSpecificSystemVarRecord(SystemVars rec)
    {
      List<SystemVars> list = new List<SystemVars>();
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", rec.Description));
      string sqlWhere = " AND sv_desc = @P1  ";
      if (GetSystemVarRecords(paramList, out list, SystemVarsList, sqlWhere, string.Empty))
        return list[0];
      else
        return null;
    }
    public static bool GetSystemVarRecords(List<PgSqlParameter> paramList, out List<SystemVars> list, string reqdFields, string extraWhere, string orderBy)
    {
      list = null;
      return GetSystemVarRecords(paramList, out list, reqdFields, extraWhere, orderBy, false);
    }
    public static bool GetSystemVarRecords(List<PgSqlParameter> paramList, out List<SystemVars> list, string reqdFields, string extraWhere, string orderBy, bool incDeleted)
    {
      list = new List<SystemVars>();
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
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE 1 = 1  {2} {3} ", "system_vars", reqdFields, extraWhere, orderBy);
          else
            command.CommandText = string.Format("SELECT {1} FROM {0} WHERE sv_datedeleted = @P0 {2} {3} ", "system_vars", reqdFields, extraWhere, orderBy);
          command.Prepare();
          try
          {
            PgSqlDataReader reader = command.ExecuteReader();
            list = GetSystemVarsRecords(reader);
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

    public static List<SystemVars> GetSystemVarsRecords(PgSqlDataReader reader)
    {
      List<SystemVars> inputList = new List<SystemVars>();
      while (reader.Read())
      {
        SystemVars dtls = new SystemVars();

        dtls.ID = reader.GetInt32(0);
        dtls.Description = reader.GetString(1);
        dtls.Status = reader.GetString(2);
        dtls.VarDate = reader.GetDateTime(3);
        dtls.Notes = reader.GetString(4);
        dtls.IntVar1 = reader.GetInt32(5);
        dtls.IntVar2 = reader.GetInt32(6);
        dtls.DecVar1 = reader.GetInt32(7);
        dtls.DecVar2 = reader.GetDecimal(8);
        dtls.DateCreated = reader.GetDateTime(9);
        dtls.DateModified = reader.GetDateTime(10);
        dtls.DateDeleted = reader.GetDateTime(11);

        inputList.Add(dtls);
      }
      return inputList;
    }
    // *************************************************************************************************

  }
}



