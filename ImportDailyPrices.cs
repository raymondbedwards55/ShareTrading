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
    public partial class ImportDailyPrices : Form
    {
        public ImportDailyPrices()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
      //DBAccess DB = new DBAccess();
      List<DBAccess.CompanyDetails> list = null;
            if (!DBAccess.GetCompanyDetails(null, out  list))
                return;
            String ASXCode = "";
            String PrevClose = "";
            String OpenPrc = "";
            String AskPrc = "";
            String BidPrc = "";
            String PrcLow = "";
            String PrcHigh = "";
            String YrLow = "";
            String YrHigh = "";
            String ClosePrc = "";
            String YrTarget = "";
            String Beta = "";
            String ThsDate = "";
            String Volume = "";
            String AvgVol = "";
            String MarketCap = "";
            String PE = "";
            String EPS = "";
            String ForwardPE = "";
            String PriceSales = "";
            String ExDivDate = "";
            String MeanRecommendation = ""; // 1 = String Buy    5 = Sell
            String PEGrowth = ""; // Earning to Growth over 5 yrs in this case
            String DivYeild = "";
            

            foreach (DBAccess.CompanyDetails rec in list)
            {
        ASXCode = rec.ASXCode;
                String response = GetPage(rec.ASXCode);
                String x = @".*time_rtq_ticker.*>" + @"([0-9\-\.]+)" + "<";
                Match matchSect1 = Regex.Match(response, x);
                if (matchSect1.Success)
                {
                    ClosePrc = matchSect1.Groups[1].Value;
                }
                String x1 = @"yfs_market_time.*>....([0-9]+).([a-zA-Z]+).([0-9]+).([0-9:]+)";
                Match matchSect2 = Regex.Match(response, x1);
                DateTime dt = new DateTime();
                if (matchSect2.Success)
                {
                    String day = matchSect2.Groups[1].Value;
                    String Mnth = matchSect2.Groups[2].Value;
                    String Yr = matchSect2.Groups[3].Value;
                    String DataTime = matchSect2.Groups[4].Value;
                    string date = day + " " + Mnth + " " + Yr;
                    dt = Convert.ToDateTime(date);
                }
                else
                    Console.WriteLine("Failed to find Date in Test\n");
                Match match = Regex.Match(response,
                            @"Prev Close:<.*>([0-9\-\.]+)</td>.*.>Open:.*>([0-9\-\.]+)</td>.*>Bid:<.*>([0-9\-\.]+)</span>.*>Ask:<.*>([0-9\-\.]+)</span>.*>1y Target Est:</th>.*>([0-9\-\.N\/A]+).*>Beta:</th><td class=.*>([0-9\.]+)</td><.*");
                if (match.Success)
                {
                    PrevClose = match.Groups[1].Value;
                    OpenPrc = match.Groups[2].Value;
                    BidPrc = match.Groups[3].Value;
                    AskPrc = match.Groups[4].Value;
                    YrTarget = match.Groups[5].Value;
                    Beta = match.Groups[6].Value;
                }
                else
                    Console.WriteLine("Failed in getting prices from Text\n");


                match = Regex.Match(response, @"Day\'s Range:</th>.*>([0-9\.]+)<.* - .*>([0-9\.]+).*52wk Range:</th>.*>([0-9\.]+)<.*-.*>([0-9\.]+).*Volume:");
                //Day's Range:</th><td class="yfnc_tabledata1"><span><span id="yfs_g53_agl.ax">20.85</span></span> - <span><span id="yfs_h53_agl.ax">21.24</span></span></td></tr><tr><th scope="row" width="48%">52wk Range:</th><td class="yfnc_tabledata1"><span>16.37</span> - <span>21.25</span></td></tr><tr><th scope="row" width="48%">Volume:</th><td class="yfnc_tabledata1"><span id="yfs_v53_agl.ax">1,632,531</span></td></tr><tr><th scope="row" width="48%">Avg Vol <span class="small">(3m)</span>:</th><td class="yfnc_tabledata1">2,337,390</td></tr><tr><th scope="row" width="48%">Market Cap:</th><td class="yfnc_tabledata1"><span id="yfs_j10_agl.ax">14.23B</span></td></tr><tr><th scope="row" width="48%">P/E <span class="small">(ttm)</span>:</th><td class="yfnc_tabledata1">N/A</td></tr><tr><th scope="row" width="48%">EPS <span class="small">(ttm)</span>:</th><td class="yfnc_tabledata1">-0.61</td></tr><tr class="end"><th scope="row" width="48%">Div &amp; Yield:</th><td class="yfnc_tabledata1">0.73 (4.82%)
                if (match.Success)
                {
                    PrcLow = match.Groups[1].Value;
                    PrcHigh = match.Groups[2].Value;
                }
                else
                    Console.WriteLine("Failed in getting prices from Text\n");
                match = Regex.Match(response, @"Volume:</th.*ax.>([0-9\,]+)</.*>Avg Vol <.*1.>([0-9\,]+)<.*>Market Cap:<.*>([0-9\.BM]+)<.*>P.E <.*>([0-9\.\-N\/A]+)<.*>EPS <.*>([0-9\.\-N\/A]+)<.*Yield:<.*([0-9\.\-N\/A]+).*Trade Now"); //.><div id=.bd.><div class=.sel-container.><button id=.btn_trdnow. disabled class=.trdnowimg.></button><button id=.btn_selbrkr. class=.btn-broker-sel.><div class=.brokertxt.>Select your broker</div><div class=.downmenu. id=.btn_arrow.></div></button><span id=.trdnow_success.></span></div><div id=.trdnow_sel. style=display:none.><div id=.hd.>"); // The broker you select will become the <b>default broker</b> for Trade Now</div><div id=.bd.><span class=.broker-head.>Featured Brokers</span><ul style=.width:100.;. class=.premium clear.><li><input class=.r_selbrkr. name=.r_selbrkr. type=.radio. value=.commsec.>CommSec<img src=.https://s.yimg.com/os/mit/media/m/base/images/transparent-1093278.png. style=.background-image.url(.https://s.yimg.com/ao/a/tradenow_icon.jpg.). alt=.CommSec. class=. lzbg ImageLoader.></li></ul><ul style=.width:100.;. class=.nonpremium clear.><li><input class=.r_selbrkr custombrkr. name=.r_selbrkr. type=.radio.><input class=.customurl. type=.text. id=.txt_custombrkr. placeholder=.Enter your Broker.></li></ul><input type=.hidden. id=.trdnow_user_status. value=.0.></div><div id=.ft.><div class=.save-reset-container.><button type=.button. id=.btn_savebrkr.>Select Broker</button><span class=.resetlink.><a href=.#. id=.reset_brkr.>Reset</a></span><span class=.err_msg. id=.trdnow_err.></span></div><div class=.feedback-brkr.><a href=.http://feedback.yahoo.com/forums/170310-au-finance?.done=http.3A.2F.2Fau.finance.yahoo.com.2Fq.3Fs.3DAMC.ax. target=._blank.>Please provide feedback on the new Trade Now function</a></div></div></div></div><div id=.trdnow_container.></div></div><style>");
                //>([0-9\,]+)<");
                //Volume:</th><td class="yfnc_tabledata1"><span id="yfs_v53_amc.ax">3,308,721</span></td></tr><tr><th scope="row" width="48%">Avg Vol <span class="small">(3m)</span>:</th><td class="yfnc_tabledata1">2,973,100</td></tr><tr><th scope="row" width="48%">Market Cap:</th><td class="yfnc_tabledata1"><span id="yfs_j10_amc.ax">16.65B</span></td></tr><tr><th scope="row" width="48%">P/E <span class="small">(ttm)</span>:</th><td class="yfnc_tabledata1">69.81</td></tr><tr><th scope="row" width="48%">EPS <span class="small">(ttm)</span>:</th><td class="yfnc_tabledata1">0.21</td></tr><tr class="end"><th scope="row" width="48%">Div &amp; Yield:</th><td class="yfnc_tabledata1">N/A (N/A) </td></tr></table></div></div><div id="yfi_tradenow" class="yui-skin-sam" data-ylk="sec:Trade Now;slk:Trade Now;"><div id="bd"><div class="sel-container"><button id="btn_trdnow" disabled class="trdnowimg"></button><button id="btn_selbrkr" class="btn-broker-sel"><div class="brokertxt">Select your broker</div><div class="downmenu" id="btn_arrow"></div></button><span id="trdnow_success"></span></div><div id="trdnow_sel" style="display:none"><div id="hd">The broker you select will become the <b>default broker</b> for Trade Now</div><div id="bd"><span class="broker-head">Featured Brokers</span><ul style="width:100%;" class="premium clear"><li><input class="r_selbrkr" name="r_selbrkr" type="radio" value="commsec">CommSec<img src="https://s.yimg.com/os/mit/media/m/base/images/transparent-1093278.png" style="background-image:url('https://s.yimg.com/ao/a/tradenow_icon.jpg')" alt="CommSec" class=" lzbg ImageLoader"></li></ul><ul style="width:100%;" class="nonpremium clear"><li><input class="r_selbrkr custombrkr" name="r_selbrkr" type="radio"><input class="customurl" type="text" id="txt_custombrkr" placeholder="Enter your Broker"></li></ul><input type="hidden" id="trdnow_user_status" value="0"></div><div id="ft"><div class="save-reset-container"><button type="button" id="btn_savebrkr">Select Broker</button><span class="resetlink"><a href="#" id="reset_brkr">Reset</a></span><span class="err_msg" id="trdnow_err"></span></div><div class="feedback-brkr"><a href="http://feedback.yahoo.com/forums/170310-au-finance?.done=http%3A%2F%2Fau.finance.yahoo.com%2Fq%3Fs%3DAMC.ax" target="_blank">Please provide feedback on the new Trade Now function</a></div></div></div></div><div id="trdnow_container"></div></div><style>
                //>([0-9\,]+)<
                if (match.Success)
                {
                    Volume = (match.Groups[1].Value).Replace(",","");
                    AvgVol = match.Groups[2].Value;
                    MarketCap = match.Groups[3].Value;
                    PE = match.Groups[4].Value;
                    EPS = match.Groups[5].Value;
                    YrHigh = match.Groups[6].Value;
                }
                else
                    Console.WriteLine("Failed in getting prices from Text\n");
                match = Regex.Match(response, @">Key.Statistics.*>Forward.P.E.<.*>([0-9\.\-N\/A]+)<.*>P.S.*>([0-9\.\-N\/A]+)<.*>Ex-Dividend Date:.*>([0-9\.\-N\/A]+).*More Key Statistics.*Analysts.*Annual EPS Est."); 
                if (match.Success)
                {
                    ForwardPE = match.Groups[1].Value;
                    PriceSales = match.Groups[2].Value;
                    ExDivDate = match.Groups[3].Value;
                }
                else
                    Console.WriteLine("Failed in getting prices from Text\n");

                //               match = Regex.Match(response, @"Mean Recommendation.*([0-9\.\-N\/A]+).*>PEG Ratio<.*>([0-9\.\-N\/A]+)<.*Strong Buy");
                match = Regex.Match(response, @"Mean Recommendation.*>([0-9\.\-N\/A]+)</td></tr><tr><th scope=.*>PEG Ratio.*>([0-9\.\-N\/A]+)<.*Strong Buy\)"); // 1.0 - 5.0 (Sell)</p><a href="/q/ao?s=AMC.AX">Analyst Opinion
                if (match.Success)
                {
                    MeanRecommendation = match.Groups[1].Value;
                    PEGrowth = match.Groups[2].Value;
                }
                else
                    Console.WriteLine("Failed in getting prices from Text\n");


                DBAccess.ASXPriceDate ASXPriceDate = new DBAccess.ASXPriceDate();
                ASXPriceDate.ASXCode = ASXCode;
                int dow = (int) dt.DayOfWeek;
                if (dow == 6)
                    dt = dt.AddDays(-1);
                if (dow == 0)
                    dt = dt.AddDays(-2);
                ASXPriceDate.PriceDate = dt;
        Decimal prcClose = ASXPriceDate.PrcClose;
        decimal prcOpen = ASXPriceDate.PrcOpen;
        decimal prcHigh = ASXPriceDate.PrcHigh;
        decimal prcLow = ASXPriceDate.PrcLow;
        int vol = ASXPriceDate.Volume;

        Decimal.TryParse(ClosePrc ,out prcClose);
                Decimal.TryParse(PrcHigh,out prcHigh);
                Decimal.TryParse(PrcLow, out prcLow);
                Decimal.TryParse(OpenPrc, out prcOpen);
                int.TryParse(Volume, out vol);
        ASXPriceDate.RecalcReqd = "Y";
                DBAccess.ASXprcInsert(ASXPriceDate);
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
              "https://au.finance.yahoo.com/q?s=" + ThisASXCode +".ax");
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
