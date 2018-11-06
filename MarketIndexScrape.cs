

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Devart.Data.PostgreSql;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.ComponentModel;
using System.Data;
using System.Drawing;


using System.Windows.Forms;
using ShareTrading.Common.Src;
namespace ShareTrading
{
  public class MarketIndexScrape
  {

    public static void Run(DBAccess.CompanyDetails _thisCompany, DateTime _thisDate )
    {
     
      string response = string.Empty;
      if (_thisCompany.InMarketIndex && _thisCompany.GICSSubIndusId == 0)
      {
        response = null;
        //response = GetPage(string.Format("https://www.marketindex.com.au/asx/{0}", _thisCompany.ASXCode));    // Daily Company Stats
        if (response != null)
        {
          string GICSIndustrySubCode = parseResearch(response);
          if (!string.IsNullOrEmpty(GICSIndustrySubCode))
          {
            _thisCompany.GICSSubIndusId = DBAccess.GetSpecificSubIndusRecord(GICSIndustrySubCode).ID;
            DBAccess.DBUpdate(_thisCompany, "companydetails", typeof(DBAccess.CompanyDetails));
          }
        }
        else
        {
          _thisCompany.InMarketIndex = false;
          DBAccess.DBUpdate(_thisCompany, "companydetails", typeof(DBAccess.CompanyDetails));

        }
        System.Threading.Thread.Sleep(6000);
      }

      response = GetPage(string.Format("https://finance.yahoo.com/quote/{0}.AX/history?p={0}.AX", _thisCompany.ASXCode));    // Price History
      if (response != null)
        parseHistoricalData(response, _thisCompany);  // Update prices

      response = GetPage(string.Format("https://finance.yahoo.com/quote/{0}.AX/key-statistics?p={0}.AX", _thisCompany.ASXCode));    // Daily Company Stats
      if (response != null)
        parseCompanyData(response, _thisCompany, _thisDate);

      //response = GetPage(string.Format("https://www.asx.com.au/asx/share-price-research/company/{0}/details", _thisCompany.ASXCode));    // Daily Company Stats
      ////if (response != null)
      ////  parseResearch  (response);
      updateMinMaxPrices(_thisCompany);

    }

    public static void updateMinMaxPrices(DBAccess.CompanyDetails _thisCompany)
    {
      // get price records for this ASX Code for 180 days
      List<DBAccess.ASXPriceDate> list;
      if (DBAccess.GetAllPrices(_thisCompany.ASXCode, DateTime.Today.AddDays(-180), out list, DBAccess.ASXPriceDateFieldList))
      {
        //Get list of records for which the min / max / pct needs to be calculated

       List <DBAccess.ASXPriceDate> recalcList = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return r1.RecalcReqd == "Y"; });
        foreach (DBAccess.ASXPriceDate entry in recalcList)
        {

          // Get max of last 90 days
          decimal val = 0M;
          try
          {
            //List<DBAccess.ASXPriceDate> interm = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-90)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; });
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-90)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Max(x => x.PrcHigh);
            entry.Day90Max = val;
            // Get min of last 90 days
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-90)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Min(x => x.PrcLow);
            entry.Day90Min = val;
            // (max - min) / current close
            entry.Day90Pct = entry.PrcClose == 0 ? 1 : Decimal.Round((entry.Day90Max - entry.Day90Min) / entry.PrcClose, 2);
          }
          catch { }
          // Get max of last 60 days
          try
          {
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-60)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Max(x => x.PrcHigh);
            entry.Day60Max = val;
            // Get min of last 60 days
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-60)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Min(x => x.PrcLow);
            entry.Day60Min = val;
            entry.Day60Pct = entry.PrcClose == 0 ? 1 : Decimal.Round((entry.Day60Max - entry.Day60Min) / entry.PrcClose, 2);
          }
          catch { }
          // Get max of last 30 days
          try
          {
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-30)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Max(x => x.PrcHigh);
            entry.Day30Max = val;
            // Get min of last 30 days
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-30)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Min(x => x.PrcLow);
            entry.Day30Min = val;
            entry.Day30Pct = entry.PrcClose == 0 ? 1 : Decimal.Round((entry.Day30Max - entry.Day30Min) / entry.PrcClose, 2);
          }
          catch
          {

          }
          try
          {
            // Get max of last 7 days
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-7)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Max(x => x.PrcHigh);
            entry.Day5Max = val;
            // Get min of last 7 days
            val = list.FindAll(delegate (DBAccess.ASXPriceDate r1) { return DateTime.Compare(r1.PriceDate, entry.PriceDate.AddDays(-7)) >= 0 && DateTime.Compare(r1.PriceDate, entry.PriceDate) <= 0; }).Min(x => x.PrcLow);
            entry.Day5Min = val;
            entry.Day5Pct = entry.PrcClose == 0 ? 1 : Decimal.Round((entry.Day5Max - entry.Day5Min) / entry.PrcClose, 2);
          }
          catch { }
          entry.RecalcReqd = "N";
          DBAccess.DBUpdate(entry, "asxpricedate", typeof(DBAccess.ASXPriceDate));
        }
      }
      // 
    }

    public static void Recommendations()
    {
      return;
      DateTime startDate = DateTime.Today;
      // get last record written in recommendations table
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      List<DBAccess.BrokersRecommendations> list = new List<DBAccess.BrokersRecommendations>();
      string sqlWhere = " AND br_transdate = (SELECT MAX(br_transdate) FROM brokers_recommendations) ";
      if (!DBAccess.GetBrokersRecommendationsRecords(paramList, out list, DBAccess.BrokersRecommendationsList, sqlWhere, string.Empty, false))
      {
        // no records found => set date to today minus 14
        startDate = DateTime.Today.AddDays(-14);
      }
      else
      {
        // set start date to max date + 1
        startDate = list[0].HistoryDate.AddDays(0);
      }

      while (startDate <= DateTime.Today)
      {
        string response = GetPage(string.Format("https://www.marketindex.com.au/analysis/consensus-recommendations-{0}", startDate.ToString("dd-MMMM-yyyy")));    // Brokers Recommendations
        if (response != null)
          parseRecommendations(response, startDate);
        startDate = startDate.AddDays(1);
      }

    }

    public static string  getDirectorsTransactions()
    {
      string filename = string.Format("c://Users//{0}////Downloads//view-source_https___www.marketindex.com.au_directors-transactions.html", Environment.UserName); // getfilename("WatchlistData");
      if (string.IsNullOrEmpty(filename))
      {
        MessageBox.Show("Unable to open file selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }
      try
      {

        List<string> exceptionCodes = new List<string>();
        System.IO.StreamReader sr = new System.IO.StreamReader(System.IO.File.Open(filename, System.IO.FileMode.Open));
        string response = sr.ReadToEnd();

        return response;
      }
      catch
      {
        return null;
      }
    }
    //  **** can't get market index data automatically anymore

    public static void Run()
    {
      //return;
      //String response = GetPage( "https://www.marketindex.com.au/directors-transactions"/* ASXCode */);
      string response = getDirectorsTransactions();
      if (response == null)
      {
        return;
      }
      // ***************

      response = response.Replace("option", "my_option");
      HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
      htmlDoc.OptionFixNestedTags = true;
      htmlDoc.LoadHtml(response);

      // ParseErrors is an ArrayList containing any errors from the Load statement
      if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
      {
        // Handle any parse errors as required

      }
      else
      {

        if (htmlDoc.DocumentNode != null)
        {
          HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

          if (bodyNode != null)
          {
            // Do something with bodyNode
            var td_nodes = bodyNode.Descendants("td").ToList();
            int counter = 0;
            List<DBAccess.DirectorsTransactions> list = new List<DBAccess.DirectorsTransactions>();
            DBAccess.DirectorsTransactions rec = new DBAccess.DirectorsTransactions();
            rec.Notes = string.Empty;
            bool validLine = true;
            foreach (var td_node in td_nodes)
            {
              //Console.WriteLine(">Directors>>" + td_node.InnerText + "<<");
              //if (td_node.ParentNode.ParentNode.ParentNode.Id != "director-transactions-table")
              //  continue;
              switch (counter)
              {
                case 0:
                  //DateTime t = DateTime.MinValue;
                  //validLine = DateTime.TryParse(td_node.InnerText.Trim(), out t);
                  //rec.TransDateDirectors = t;
                  rec.ASXCodeDirectors = td_node.InnerText.Trim();
                  break;
                case 1:
                  //rec.NameDirectors = td_node.InnerText.Trim();
                  break;
                case 2:
                  //rec.Type = td_node.InnerText.Trim();
                  DateTime t = DateTime.MinValue;
                  validLine = DateTime.TryParse(td_node.InnerText.Trim(), out t);
                  rec.TransDateDirectors = t;
                  break;
                case 3:
                  rec.NameDirectors = td_node.InnerText.Trim();
                  //int qty = 0;
                  //validLine = int.TryParse(td_node.InnerText.Trim().Replace(",",""), out qty);
                  //rec.QtyShares = qty;
                  break;
                case 4:
                  rec.Type = td_node.InnerText.Trim(); ;
                  //decimal price = 0M;
                  //validLine = decimal.TryParse(td_node.InnerText.Trim().Replace(",", "").Replace("$", ""), out price);
                  //rec.Price = price;
                  break;
                case 5:
                  int qty = 0;
                  validLine = int.TryParse(td_node.InnerText.Trim().Replace(",", ""), out qty);
                  rec.QtyShares = qty;

                  //decimal value = 0M;
                  //validLine = decimal.TryParse(td_node.InnerText.Trim().Replace(",", "").Replace("$", ""), out value);
                  //rec.Value = value;
                  break;
                case 6:
                  decimal price = 0M;
                  validLine = decimal.TryParse(td_node.InnerText.Trim().Replace(",", "").Replace("$", ""), out price);
                  rec.Price = price;
                  //rec.Notes = td_node.InnerText.Trim();
                  break;
                case 7:
                  price = 0M;
                  validLine = decimal.TryParse(td_node.InnerText.Trim().Replace(",", "").Replace("$", ""), out price);
                  rec.Value = price;
                  break;
                default:
                  break;
              }
              if (counter >= 7)
              {

                if (validLine)
                {
                  if (!DBAccess.GetDirectorsTransactions(rec, out list))
                  {
                    if (rec.Type.Contains("Buy") || rec.Type.Contains("Sell") || rec.Type.Contains("Exercise"))
                    {

                      rec.DateCreated = DateTime.Now;
                      rec.DateModified = DateTime.Now;
                      rec.DateDeleted = DateTime.MinValue;
                      DBAccess.DBInsert(rec, "directors_transactions", typeof(DBAccess.DirectorsTransactions));
                      // Make sure that the company list has this company in it
                      List<DBAccess.CompanyDetails> coList = new List<DBAccess.CompanyDetails>();
                      if (!DBAccess.GetCompanyDetails(rec.ASXCodeDirectors, out coList))
                      {
                        DBAccess.CompanyDetails cod = new DBAccess.CompanyDetails();
                        cod.ASXCode = rec.ASXCodeDirectors;
                        cod.DateCreated = DateTime.Now;
                        cod.DateModified = DateTime.Now;
                        cod.DateDeleted = DateTime.MinValue;
                        cod.OnWatchList = false;
                        DBAccess.DBInsert(cod, "companydetails", typeof(DBAccess.CompanyDetails));
                      }

                    }
                  }
                  else
                  {
                    rec.ID = list[0].ID;
                    rec.DateCreated = list[0].DateCreated;
                    rec.DateDeleted = list[0].DateDeleted;
                    rec.DateModified = list[0].DateModified;
                    DBAccess.DBUpdate(rec, "directors_transactions", typeof(DBAccess.DirectorsTransactions));
                  }
                }
                else
                {
                }
              
                rec = new DBAccess.DirectorsTransactions();
                rec.Notes = string.Empty;
                counter = 0;
                validLine = true;
              }
              else
                counter++;

              //Console.WriteLine(" =======  td =============");
              //Console.WriteLine("LINK: {0}", td_node.GetAttributeValue("href", ""));
              //Console.WriteLine("TEXT: {0}", td_node.InnerText.Trim());
            }
          }
        }
      }
    }

    public class coValues
    {
      public string coText { get; set; }
      public string coValue { get; set; }
    }
    public static void parseCompanyData(string response, DBAccess.CompanyDetails companyRecord, DateTime thisDate)
    {
      if (companyRecord.ASXCode == "AAL")
      { }
      List<coValues> valList = new List<coValues>();
      coValues newVal = new coValues() { coText = "Market Cap", coValue = string.Empty };
      valList.Add(newVal);
      newVal = new coValues() { coText = "Avg Vol (3 month)", coValue = string.Empty };
      valList.Add(newVal);
      newVal = new coValues() { coText = "Trailing P/E", coValue = string.Empty };
      valList.Add(newVal);
      newVal = new coValues() { coText = "Forward P/E", coValue = string.Empty };
      valList.Add(newVal);
      newVal = new coValues() { coText = "Beta", coValue = string.Empty };
      valList.Add(newVal);
      newVal = new coValues() { coText = "Debt/Equity", coValue = string.Empty };
      valList.Add(newVal);


      //response = response.Replace("option", "my_option");
      HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
      htmlDoc.OptionFixNestedTags = true;
      htmlDoc.LoadHtml(response.Replace("/option", ""));

      // ParseErrors is an ArrayList containing any errors from the Load statement
      if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
      {
        // Handle any parse errors as required

      }
      else
      {

        if (htmlDoc.DocumentNode != null)
        {
          HtmlAgilityPack.HtmlNodeCollection tdNodeCollection = htmlDoc.DocumentNode.SelectNodes("//td");
          Boolean needNext = false;

          int idx = 0;
          foreach (HtmlAgilityPack.HtmlNode tdEntry in tdNodeCollection)
          {
            if (needNext)
            {
              valList[idx].coValue = tdEntry.InnerText.Replace("\n", "").Replace("%", "").Replace("$", "").Replace(",", "").Replace(" ", "");
              needNext = false;
            }
            //Console.WriteLine("CompanyData**" + tdEntry.InnerText + "**");
            for (int j = 0; j < valList.Count; j++)
              if (tdEntry.InnerText.Contains(valList[j].coText))
              {
                needNext = true;
                idx = j;
                break;
              }
            //Console.WriteLine(">" + tdEntry.InnerHtml + "<>" + tdEntry.InnerText + "<");
          }
          idx = 0;
          needNext = false;

        }
        if (string.IsNullOrEmpty(valList[0].coValue))
          return;
        //  Write data for this company using today's date
        List<DBAccess.CompanyHistory> list = new List<DBAccess.CompanyHistory>();
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", companyRecord.ASXCode));
        paramList.Add(new PgSqlParameter("@P2", DateTime.Today));

        if (!DBAccess.GetCompanyHistoryRecords(paramList, out list, DBAccess.CompanyHistoryList, " AND ch_asxcode = @P1 AND ch_transdate = @P2 ", string.Empty, false))
        {
          // Add Record
          int i = 0;
          decimal d = 0M;
          DBAccess.CompanyHistory rec = new DBAccess.CompanyHistory();
          rec.ASXCode = companyRecord.ASXCode;
          rec.HistoryDate = thisDate;
          rec.MarketCap = valList[0].coValue;
          rec.AvgVol = convertValue(valList[1].coValue.Trim().ToUpper());
          decimal.TryParse(valList[2].coValue, out d);
          rec.TrailingPE = d;
          decimal.TryParse(valList[3].coValue, out d);
          rec.ForwardPE = d;
          decimal.TryParse(valList[4].coValue, out d);
          rec.Beta = d;
          decimal.TryParse(valList[5].coValue, out d);
          rec.DebtEquity = d;
          rec.DateCreated = DateTime.Now;
          rec.DateModified = DateTime.Now;
          rec.DateDeleted = DateTime.MinValue;
          DBAccess.DBInsert(rec, "company_history", typeof(DBAccess.CompanyHistory));
          return;
        }
        else
        {
          // Update Record
          
          int i = 0;
          decimal d = 0M;

          list[0].MarketCap = valList[0].coValue;
          int.TryParse(valList[1].coValue, out i);
          list[0].AvgVol = i;
          decimal.TryParse(valList[2].coValue, out d);
          list[0].TrailingPE = d;
          decimal.TryParse(valList[3].coValue, out d);
          list[0].ForwardPE = d;
          decimal.TryParse(valList[4].coValue, out d);
          list[0].Beta = d;
          decimal.TryParse(valList[5].coValue, out d);
          list[0].DebtEquity = d;
          list[0].DateModified = DateTime.Now;
          DBAccess.DBUpdate(list[0], "company_history", typeof(DBAccess.CompanyHistory));
          return;

        }
      }
    }
    static public int convertValue(string s)
    {
      double val = s.Contains("N/A") ? 0 : s.EndsWith("K") ? double.Parse(s.Replace("K","")) * 1000 : s.EndsWith("M") ? double.Parse(s.Replace("M", "")) * 1000000 : s.EndsWith("B") ? double.Parse(s.Replace("B", "")) * 1000000000 : double.Parse(s);
      return (int)val;
    }
    public class dailyStats
    {
      public DateTime dt { get; set; }
      public decimal open { get; set; }
      public decimal high { get; set; }
      public decimal low { get; set; }
      public decimal close { get; set; }
      public decimal adjClose { get; set; }
      public int volume { get; set; }
    }
    public static void parseHistoricalData(string response, DBAccess.CompanyDetails thisCompany)
    {
      int counter = 0;
      List<dailyStats> statsList = new List<dailyStats>();

      HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
      htmlDoc.OptionFixNestedTags = true;
      htmlDoc.LoadHtml(response.Replace("/option", ""));

      // ParseErrors is an ArrayList containing any errors from the Load statement
      if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
      {
        // Handle any parse errors as required

      }
      else
      {

        if (htmlDoc.DocumentNode != null)
        {
          HtmlAgilityPack.HtmlNodeCollection spanNodeCollection = htmlDoc.DocumentNode.SelectNodes("//span");
          int countUpdates = 0;
          bool tableFound = false;
          dailyStats rec = new dailyStats();
          foreach (HtmlAgilityPack.HtmlNode spanEntry in spanNodeCollection)
          {
            //Console.WriteLine("Historical>>" + spanEntry.InnerText + "<<");
            if (spanEntry.InnerText.Equals("Volume"))
            {
              tableFound = true;
              continue;
            }
            if (!tableFound)
              continue;
            if (string.IsNullOrEmpty(spanEntry.InnerText))
              continue;
            //if (spanEntry.InnerText.Contains("Dividend"))
            //{
            //  counter = 0;
            //  continue;
            //}
            bool validLine = true;
            switch (counter)
            {
              case 0:           // Date
                DateTime t = DateTime.MinValue;
                validLine = DateTime.TryParse(spanEntry.InnerText.Trim(), out t);
                if (validLine)
                  rec.dt = t;
                else
                  counter = -1;
                break;
              case 1:         // Open
                if (validLine)
                {
                  decimal prc = 0M;
                  validLine = decimal.TryParse(spanEntry.InnerText.Trim(), out prc);
                  if (validLine)
                    rec.open = prc;
                  else
                    counter = -1;
                }
                break;
              case 2:         // High
                if (validLine)
                {
                  decimal prc = 0M;
                  validLine = decimal.TryParse(spanEntry.InnerText.Trim(), out prc);
                  if (validLine)
                    rec.high = prc;
                  else
                    counter = -1;
                }
                break;
              case 3:         // Low
                if (validLine)
                {
                  decimal prc = 0M;
                  validLine = decimal.TryParse(spanEntry.InnerText.Trim(), out prc);
                  if (validLine)
                    rec.low = prc;
                  else
                    counter = -1;
                }
                break;
              case 4:         // Close
                if (validLine)
                {
                  decimal prc = 0M;
                  validLine = decimal.TryParse(spanEntry.InnerText.Trim(), out prc);
                  if (validLine)
                    rec.close = prc;
                  else
                    counter = -1;
                }
                break;
              case 5:       // Adj Close
                if (validLine)
                {
                  decimal prc = 0M;
                  validLine = decimal.TryParse(spanEntry.InnerText.Trim(), out prc);
                  if (validLine)
                    rec.adjClose = prc;
                  else
                    counter = -1;
                }
                break;
              case 6:       // Volume
                if (spanEntry.InnerText.Trim().Contains("-"))
                  rec.volume = 0;
                else
                {
                  int vol = 0;
                  validLine = int.TryParse(spanEntry.InnerText.Trim().Replace(",", ""), out vol);
                  if (validLine)
                    rec.volume = vol;
                  else
                    counter = -1;
                }
                break;
              default:
                break;

            }
            if (!validLine)
            {
              if (counter < 0)
                validLine = true;
            }
            if (counter >= 6)
            {
              List<DBAccess.ASXPriceDate> list = new List<DBAccess.ASXPriceDate>();
              if (validLine)
              {
                DBAccess.ASXPriceDate apd = DBAccess.GetSpecificASXPriceRecord(thisCompany.ASXCode, rec.dt, true);
                if (apd == null)
                {

                  apd = new DBAccess.ASXPriceDate();
                  apd.ASXCode = thisCompany.ASXCode;
                  apd.PrcOpen = rec.open;
                  apd.PriceDate = rec.dt;
                  apd.PrcHigh = rec.high;
                  apd.PrcLow = rec.low;

                  apd.PrcLow = rec.low;
                  apd.PrcClose = rec.close;
                  apd.AdjClose = rec.adjClose;
                  apd.Volume = rec.volume;
                  apd.DateCreated = DateTime.Now;
                  apd.DateModified = DateTime.Now;
                  apd.DateDeleted = DateTime.MinValue;
                  apd.RecalcReqd = "Y";
                  DBAccess.DBInsert(apd, "asxpricedate", typeof(DBAccess.ASXPriceDate));
                  rec = new dailyStats();
                }
                else
                {
                  apd.RecalcReqd = apd.PrcClose != rec.close || apd.PrcHigh != rec.high || apd.PrcLow != rec.low || apd.RecalcReqd == "Y" ? "Y" : "N";
                  apd.PrcOpen = rec.open;
                  apd.AdjClose = rec.adjClose;
                  apd.PrcClose = rec.close;
                  apd.PrcHigh = rec.high;
                  apd.PrcLow = rec.low;
                  apd.Volume = rec.volume;
                  apd.DateModified = DateTime.Now;
                  DBAccess.DBUpdate(apd, "asxpricedate", typeof(DBAccess.ASXPriceDate));
                  rec = new dailyStats();
                  countUpdates++;
                  if (countUpdates > 5)
                    return;                     //  only update a max of 5 historical records
                }
              }
              else
              {
              }

                counter = 0;
                validLine = true;
            }
            else
              counter++;
            
              //Console.WriteLine(" =======  td =============");
              //Console.WriteLine("LINK: {0}", td_node.GetAttributeValue("href", ""));
              //Console.WriteLine("TEXT: {0}", td_node.InnerText.Trim());
          }
        }
      }

    }

    public static string parseResearch(string response)
    {
      
      HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
      htmlDoc.OptionFixNestedTags = true;
      htmlDoc.LoadHtml(response.Replace("/option", ""));

      // ParseErrors is an ArrayList containing any errors from the Load statement
      if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
      {
        // Handle any parse errors as required

      }
      else
      {
        HtmlAgilityPack.HtmlNodeCollection tdNodeCollection = htmlDoc.DocumentNode.SelectNodes("//td");
        bool needNext = false;
        string GICSSubCode = string.Empty;
        foreach (HtmlAgilityPack.HtmlNode tdEntry in tdNodeCollection)
        {
          if (needNext)
          {
            GICSSubCode = tdEntry.InnerText.Replace("\n", "").Replace("%", "").Replace("$", "").Replace(",", "").Replace(" ", "");
            needNext = false;
            break;
          }
            if (tdEntry.InnerText.Contains("GICS"))
              needNext = true;
          //Console.WriteLine("Research>" + tdEntry.InnerHtml + "<>" + tdEntry.InnerText + "<");
        }
        needNext = false;
        return GICSSubCode;

      }
      return null;
    }

    public static void parseRecommendations(string response, DateTime recommendationDate)
    {
      HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
      htmlDoc.OptionFixNestedTags = true;
      htmlDoc.LoadHtml(response.Replace("/option", ""));

      // ParseErrors is an ArrayList containing any errors from the Load statement
      if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
      {
        // Handle any parse errors as required

      }
      else
      {
        if (htmlDoc.DocumentNode != null)
        {
          HtmlAgilityPack.HtmlNodeCollection tdNodeCollection = htmlDoc.DocumentNode.SelectNodes("//td");
          if (tdNodeCollection != null)
          {
            int counter = 0;
            DBAccess.BrokersRecommendations rec = new DBAccess.BrokersRecommendations();
            foreach (HtmlAgilityPack.HtmlNode tdEntry in tdNodeCollection)
            {
              //Console.WriteLine("Recommendations>>" + tdEntry.InnerText + "<<");
              if (counter == 0 && tdEntry.InnerText.Contains("Prev"))
                break;
              rec.HistoryDate = recommendationDate;
              bool validLine = true;
              switch (counter)
              {
                case 0:           //  ASX Code

                  rec.ASXCode = tdEntry.InnerText;
                  if (rec.ASXCode == "APT")
                  {

                  }
                  break;
                case 1:         //  Company Name
                  break;
                case 2:        // Price
                  decimal prc = 0M;
                  validLine = decimal.TryParse(tdEntry.InnerText.Trim().Replace("$", ""), out prc);
                  if (validLine)
                    rec.Price = prc;
                  //}
                  break;
                case 3:         // Buy
                  if (validLine)
                  {
                    int i = 0;
                    validLine = int.TryParse(tdEntry.InnerText.Trim(), out i);
                    if (validLine)
                      rec.Buy = i;
                  }
                  break;
                case 4:         // Hold
                  if (validLine)
                  {
                    int i = 0;
                    validLine = int.TryParse(tdEntry.InnerText.Trim(), out i);
                    if (validLine)
                      rec.Hold = i;
                  }
                  break;
                case 5:       // Sell
                  if (validLine)
                  {
                    int i = 0;
                    validLine = int.TryParse(tdEntry.InnerText.Trim(), out i);
                    if (validLine)
                      rec.Sell = i;
                  }
                  break;
                case 6:       // Consensus
                  rec.Consensus = tdEntry.InnerText;
                  break;
                default:
                  break;

              }
              if (counter >= 6)
              {
                List<DBAccess.BrokersRecommendations> list = new List<DBAccess.BrokersRecommendations>();
                if (validLine)
                {
                  DBAccess.BrokersRecommendations br = DBAccess.GetSpecificBrokersRecommendationRecord(rec);
                  if (br == null)
                  {
                    rec.DateCreated = DateTime.Now;
                    rec.DateModified = DateTime.Now;
                    rec.DateDeleted = DateTime.MinValue;
                    DBAccess.DBInsert(rec, "brokers_recommendations", typeof(DBAccess.BrokersRecommendations));
                    // Make sure that the company list has this company in it
                    List<DBAccess.CompanyDetails> coList = new List<DBAccess.CompanyDetails>();
                    if (!DBAccess.GetCompanyDetails(rec.ASXCode, out coList))
                    {
                      DBAccess.CompanyDetails cod = new DBAccess.CompanyDetails();
                      cod.ASXCode = rec.ASXCode;
                      cod.DateCreated = DateTime.Now;
                      cod.DateModified = DateTime.Now;
                      cod.DateDeleted = DateTime.MinValue;
                      cod.OnWatchList = false;
                      DBAccess.DBInsert(cod, "companydetails", typeof(DBAccess.CompanyDetails));
                    }
                  }
                  else
                  {
                    br.Buy = rec.Buy;
                    br.Sell = rec.Sell;
                    br.Hold = rec.Hold;
                    br.Price = rec.Price;
                    br.Consensus = rec.Consensus;
                    br.DateModified = DateTime.Now;
                    DBAccess.DBUpdate(br, "brokers_recommendations", typeof(DBAccess.BrokersRecommendations));
                  }
                }
                else
                {
                }

                counter = 0;
                validLine = true;
                rec = new DBAccess.BrokersRecommendations();

              }
              else
                counter++;

              //Console.WriteLine(" =======  td =============");
              //Console.WriteLine("LINK: {0}", td_node.GetAttributeValue("href", ""));
              //Console.WriteLine("TEXT: {0}", td_node.InnerText.Trim());
            }
          }
        }
      }
    }

    public static String GetPage( string url)
    {

      // Create a request for the URL. 
      WebRequest request = WebRequest.Create(url);
//        "https://www.marketindex.com.au/asx/" + ThisASXCode);
//      "https://www.marketindex.com.au/directors-transactions");
      // If required by the server, set the credentials.
      request.Credentials = CredentialCache.DefaultCredentials;
      // Get the response.
      try
      {
        WebResponse response = request.GetResponse();
        if ((((HttpWebResponse)response).StatusDescription) != "OK")
        {
          //Console.WriteLine("getPage  FAILED " + url + "< Status>" + ((HttpWebResponse)response).StatusDescription);
          return null;
        }
        // Display the status.
        //Console.WriteLine("getPage" + url + "< Status>" + ((HttpWebResponse)response).StatusDescription);
        // Get the stream containing content returned by the server.
        Stream dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        StreamReader reader = new StreamReader(dataStream);
        // Read the content.
        string responseFromServer = reader.ReadToEnd();
        // Display the content.
        //                Console.WriteLine(responseFromServer);
        //                // Clean up the streams and the response.
        reader.Close();
        response.Close();
        return responseFromServer;
      }
      catch
      {

      }

      return null;
    }
 
  }
}

