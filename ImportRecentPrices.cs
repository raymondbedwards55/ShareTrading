using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ShareTrading
{
    public partial class ImportRecentPrices : Form
    {
        public ImportRecentPrices()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DBAccess DB = new DBAccess();
            if (!DB.GetAllCodes())
                return;
            String ASXCode = "";


            DBAccess.DividendHistory Dividend = new DBAccess.DividendHistory();
            while ((ASXCode = DB.GetNextCode()) != null)
            {
                DBAccess.ASXPriceDate ASXPriceDate = new DBAccess.ASXPriceDate();
                ASXPriceDate.ASXCode = ASXCode;
                string key = "";
                String DateFld = @".*>([0-9]+ [a-zA-Z]+ [0-9]+)</td><td";
                String DlrsFld = @">\$([0-9\-\.]+)</td>";
                String QtyFld = @"([0-9\,N\/A]+)";
                String PrcFld = @"([0-9\.]+)";
                String response = GetPage(ASXCode);
                // Here we check the Match instance.
                Match match = Regex.Match(response, @"yfnc_tabledata1(.*)");
                if (match.Success)
                {
                    // Finally, we get the Group value and display it.
                    key = match.Groups[1].Value;
                }
                String key1 = "";
                while (true)
                {
                    //Match yrDataMatch = Regex.Match(key, @".*" + @"([0-9]+ [a-zA-Z]+ [0-9]+)" + @"(.*)");
                    Match yrDataMatch = Regex.Match(key, @"(.*)" + DateFld + "(.*)");

                    if (yrDataMatch.Success)
                    {
                        key = yrDataMatch.Groups[1].Value;
                        String Fld1 = yrDataMatch.Groups[2].Value;
                        DateTime.TryParse(Fld1, out ASXPriceDate.PriceDate);
                        key1 = yrDataMatch.Groups[3].Value;
                    }
                    else
                    {
                        Console.WriteLine("failed to parse");
                        break;
                    }

                    //Match yrDataMatch = Regex.Match(key, @".*" + @"([0-9]+ [a-zA-Z]+ [0-9]+)" + @"(.*)");
                    yrDataMatch = Regex.Match(key1, " .*>" + PrcFld + "</td>.*>" + PrcFld + "</td>.*>" + PrcFld + "</td>.*>" + PrcFld + "</td>.*>" + QtyFld + "</td>.*>" + PrcFld + "</td>.*");
                    //yrDataMatch = Regex.Match(key1, @".*" + QtyFld +  ".*" + PrcFld + ".*");// + PrcFld + ".*" + PrcFld + ".*" + PrcFld + ".*" + QtyFld + ".*" + PrcFld + @"(.*)");

                    if (yrDataMatch.Success)
                    {
                        Decimal.TryParse(yrDataMatch.Groups[1].Value, out ASXPriceDate.PrcOpen);
                        Decimal.TryParse(yrDataMatch.Groups[2].Value, out ASXPriceDate.PrcHigh);
                        Decimal.TryParse(yrDataMatch.Groups[3].Value, out ASXPriceDate.PrcLow);
                        Decimal.TryParse(yrDataMatch.Groups[4].Value, out ASXPriceDate.PrcClose);
                        String x = yrDataMatch.Groups[5].Value.Replace(",", "");
                        int.TryParse(x, out ASXPriceDate.Volume);
                        Decimal.TryParse(yrDataMatch.Groups[6].Value, out ASXPriceDate.AdjClose);
                        DB.ASXprcInsert(ASXPriceDate);
                    }
                    else
                        break;
                }
            }
        }
        

        //                                    @".*Prev Close:<.*" + @">([0-9\-\.]+)</td>" +
        //                                    @".*Open:.*" + @">([0-9\-\.]+)</td>" + 
        //                                    @".*Bid:.*" + @">([0-9\-\.]+)</span>" +
        //                                    @".*Ask:.*" + @">([0-9\-\.]+)</span>" +
        //                                    @".*1y.Target.Est.*");



        public DateTime ConvertToDate(String Stringddmmyyyy)
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

        public String GetPage(String ThisASXCode)
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(
              "https://au.finance.yahoo.com/quote/" + ThisASXCode + ".ax/history?ltr=1");
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

