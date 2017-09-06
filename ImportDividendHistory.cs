﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace ShareTrading
{
  public class ImportDividendHistory
  {

    public static void ImportDividends(string ASXCode)
    {
      DBAccess.DividendHistory Dividend = new DBAccess.DividendHistory();
      String DateFld = @"<td>([0-9\-]+)</td>";
      String DlrsFld = @"<td>\$([0-9\-\.]+)</td>";
      String PctFld = @"<td>([0-9\-\.]+)\%</td>";

      string dlrs1 = @"<td>([\$0-9\-\.]*)</td>";
      string pct1 = @"<td>([0-9\-\.\%]*)</td>";
      String response = GetPage(ASXCode);
      Dividend.ASXCode = ASXCode;
      while (true)
      {
        Match match = Regex.Match(response, "<tr   style='background-color:#...;(.*)");

        // Here we check the Match instance.
        if (match.Success)
        {
          // Finally, we get the Group value and display it.
          string key = match.Groups[1].Value;
          Match yrDataMatch = Regex.Match(key, @">" + DateFld + DateFld + DateFld + DlrsFld + PctFld + DlrsFld + DlrsFld + dlrs1 + pct1 + pct1 +/*  DlrsFld + PctFld + PctFld  +*/  @"(.*)");
          if (yrDataMatch.Success)
          {
            String Fld1 = yrDataMatch.Groups[1].Value;
            String Fld2 = yrDataMatch.Groups[2].Value;
            String Fld3 = yrDataMatch.Groups[3].Value;
            String Fld4 = yrDataMatch.Groups[4].Value;
            String Fld5 = yrDataMatch.Groups[5].Value;
            String Fld6 = yrDataMatch.Groups[6].Value;
            String Fld7 = yrDataMatch.Groups[7].Value;
            String Fld8 = yrDataMatch.Groups[8].Value;
            String Fld9 = yrDataMatch.Groups[9].Value;
            String Fld10 = yrDataMatch.Groups[10].Value;

            Dividend.ExDividend = ConvertToDate(Fld1);
            // Check if data already in table
            List<DBAccess.DividendHistory> divList = new List<DBAccess.DividendHistory>();
            if (!DBAccess.GetDividends(ASXCode, Dividend.ExDividend, out divList, DBAccess.dirn.equals))
            {
              //
              Dividend.BooksClose = ConvertToDate(Fld2);
              Dividend.DatePayable = ConvertToDate(Fld3);
              Decimal val = 0M;
              Decimal.TryParse(Fld4, out val);
              Dividend.Amount = val;
              Decimal.TryParse(Fld5, out val);
              Dividend.Franking = val;
              Decimal.TryParse(Fld6, out val);
              Dividend.FrankingCredit = val;
              Decimal.TryParse(Fld7, out val);
              Dividend.GrossDividend = val;
              DBAccess.DividendHistoryInsert(Dividend);
            }
            response = yrDataMatch.Groups[11].Value;
            if (!yrDataMatch.Groups[11].Value.StartsWith("<tr   style='background-color"))
            {
              if (yrDataMatch.Groups[11].Value.Contains("<tr   style='background-color"))
                response = yrDataMatch.Groups[11].Value.Substring(yrDataMatch.Groups[11].Value.IndexOf("<tr   style='background-color"));
            }

          }
          else
            break;
        }
        else
          break;
      }
    }

    public static DateTime ConvertToDate(String Stringddmmyyyy)
    {
      int yyyy = 0;
      int mm = 0;
      int dd = 0;
      int.TryParse(Stringddmmyyyy.Substring(6, 4), out yyyy);
      int.TryParse(Stringddmmyyyy.Substring(3, 2), out mm);
      int.TryParse(Stringddmmyyyy.Substring(0, 2), out dd);

      DateTime x = new DateTime(yyyy, mm, dd);
      return x;
    }

    public static String GetPage(String ThisASXCode)
    {
      // Create a request for the URL. 
      WebRequest request = WebRequest.Create(
        "http://dividends.com.au/dividend-history/?enter_code=" + ThisASXCode);
      // If required by the server, set the credentials.
      request.Credentials = CredentialCache.DefaultCredentials;
      // Get the response.
      WebResponse response = request.GetResponse();
      // Display the status.
      Console.WriteLine(((HttpWebResponse)response).StatusDescription);
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
  }
}
