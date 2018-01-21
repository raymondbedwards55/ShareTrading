

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

namespace ShareTrading
{
  public class MarketIndexScrape
  {

    public static void Run(string ASXCode)
    {
      String response = GetPage(ASXCode);
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
            rec.ASXCodeDirectors = ASXCode;
            bool validLine = true;
            foreach (var td_node in td_nodes)
            {
              Console.WriteLine(">id>>" + td_node.ParentNode.ParentNode.ParentNode.Id);
              if (td_node.ParentNode.ParentNode.ParentNode.Id != "director-transactions-table")
                continue;
              switch (counter)
              {
                case 0:
                  DateTime t = DateTime.MinValue;
                  validLine = DateTime.TryParse(td_node.InnerText.Trim(), out t);
                  rec.TransDateDirectors = t;
                  break;
                case 1:
                  rec.NameDirectors = td_node.InnerText.Trim();
                  break;
                case 2:
                  rec.Type = td_node.InnerText.Trim();
                  break;
                case 3:
                  int qty = 0;
                  validLine = int.TryParse(td_node.InnerText.Trim().Replace(",",""), out qty);
                  rec.QtyShares = qty;
                  break;
                case 4:
                  decimal price = 0M;
                  validLine = decimal.TryParse(td_node.InnerText.Trim().Replace(",", "").Replace("$", ""), out price);
                  rec.Price = price;
                  break;
                case 5:
                  decimal value = 0M;
                  validLine = decimal.TryParse(td_node.InnerText.Trim().Replace(",", "").Replace("$", ""), out value);
                  rec.Value = value;
                  break;
                case 6:
                  rec.Notes = td_node.InnerText.Trim();
                  break;
                default:
                  break;
              }
              if (counter >= 6)
              {

                if (validLine)
                {
                  if (!DBAccess.GetDirectorsTransactions(rec, out list))
                  {
                    if (!rec.Type.Contains("Issued"))
                      DBAccess.DBInsert(rec, "directors_transactions", typeof(DBAccess.DirectorsTransactions));
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
                { }
                rec = new DBAccess.DirectorsTransactions();
                rec.ASXCodeDirectors = ASXCode;
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




    public static String GetPage(String ThisASXCode)
    {

      // Create a request for the URL. 
      WebRequest request = WebRequest.Create(
        "https://www.marketindex.com.au/asx/" + ThisASXCode);
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

