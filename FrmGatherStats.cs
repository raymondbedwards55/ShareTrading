using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devart.Data.PostgreSql;
using ShareTrading.Common.Src;

namespace ShareTrading
{
  public partial class FrmGatherStats : Form
  {
    public FrmGatherStats()
    {
      InitializeComponent();
    }

    private void FrmGatherStats_Load(object sender, EventArgs e)
    {
      List<string> descList = new List<string>();
      for (int i = 0; i <= (int)StatsType.TradeDay; i++)
        descList.Add(EnumHelper.GetEnumDescription((StatsType)i));
      cbxStatsType.DataSource = descList;  // System.Enum.GetNames(typeof(StatsType));
      cbxStatsType.Focus();
      updateASXCodeComboBox();
      statusLabel.Visible = false;
      progressBar.Visible = false;
    }


    private void toolStripButtonClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void processPriceChange(bool todays, WorkState ws)
    {
      List<DBAccess.CompanyDetails> coList = new List<DBAccess.CompanyDetails>();
      if (!DBAccess.GetCompanyDetails(ws.asxcode, out coList, false, ws.onWatchListOnly))
        return;
      if (!string.IsNullOrEmpty(ws.asxcode))
        coList = coList.FindAll(delegate (DBAccess.CompanyDetails r1) { return r1.ASXCode == ws.asxcode; });

      if (coList == null || coList.Count <= 0)
          return;

      foreach (DBAccess.CompanyDetails co in coList)
      {
        Console.WriteLine(string.Format("Type >{0}< ASX Code >{1}<", ws.type.ToString(), co.ASXCode)) ;
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        // get all ASXPriceRecords for this company in the date range
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", co.ASXCode));
        paramList.Add(new PgSqlParameter("@P2", ws.dateFrom));
        paramList.Add(new PgSqlParameter("@P3", ws.dateTo));
        List<DBAccess.ASXPriceDate> priceList = new List<DBAccess.ASXPriceDate>();
        if (DBAccess.GetPriceRecords(paramList, out priceList, DBAccess.ASXPriceDateFieldList, " AND apd_asxcode = @P1 AND apd_pricedate BETWEEN @P2 AND @P3 " , " ORDER BY apd_pricedate ASC ", false))
        {
          // foreach of the records, create a stats record
          foreach (DBAccess.ASXPriceDate rec in priceList)
          {
            DBAccess.Statistics statsRec = new DBAccess.Statistics();
            // Does the record already exist?
            List<DBAccess.Statistics> statsList = new List<DBAccess.Statistics>();
            paramList = new List<PgSqlParameter>();
            paramList.Add(new PgSqlParameter("@P1", rec.ASXCode));
            paramList.Add(new PgSqlParameter("@P2", rec.PriceDate));
            paramList.Add(new PgSqlParameter("@P3", ws.type));
            if (DBAccess.GetStatRecords(paramList, out statsList, DBAccess.StatFieldList, " AND st_asxcode = @P1 AND st_startdate = @P2 AND st_type = @P3 ", string.Empty))
              statsRec = statsList[0];
            else
            {
              statsRec.ASXCode = rec.ASXCode;
              statsRec.StartDate = rec.PriceDate;
              statsRec.Type = (StatsType)ws.type;
            }

            statsRec.Stat = rec.PrcOpen == 0 ? 0M : todays ? Decimal.Round((rec.PrcClose - rec.PrcOpen) / rec.PrcOpen * 100, 6) : getOvernightStat(rec, priceList);
            
            if (statsRec.ID == 0)
              DBAccess.DBInsert(statsRec, "statistics", typeof(DBAccess.Statistics));
            else
              DBAccess.DBUpdate(statsRec, "statistics", typeof(DBAccess.Statistics));
          }

        }

      }

    }

    private decimal getOvernightStat(DBAccess.ASXPriceDate currentRec, List<DBAccess.ASXPriceDate> priceList)
    {
      decimal stat = 0M;
      int currentIdx = priceList.FindIndex(delegate (DBAccess.ASXPriceDate r1) { return r1.ID == currentRec.ID; });
      if (currentIdx < 0 || currentIdx >= priceList.Count - 1)
        return 0M;
      stat = currentRec.PrcClose == 0M ? 0M :  Decimal.Round((priceList[currentIdx + 1].PrcOpen - currentRec.PrcClose) / currentRec.PrcClose * 100, 6);
      return stat;
    }

    private void processPriceVsFirst(WorkState ws)
    {
      List<DBAccess.CompanyDetails> coList = new List<DBAccess.CompanyDetails>();
      if (!DBAccess.GetCompanyDetails(ws.asxcode, out coList, false, ws.onWatchListOnly))
        return;
      if (!string.IsNullOrEmpty(ws.asxcode))
        coList = coList.FindAll(delegate (DBAccess.CompanyDetails r1) { return r1.ASXCode == ws.asxcode; });

      if (coList == null || coList.Count <= 0)
        return;
      foreach (DBAccess.CompanyDetails co in coList)
      {
        Console.WriteLine(string.Format("Type >{0}< ASX Code >{1}<", ws.type.ToString(), co.ASXCode));

        // foreach Company
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", co.ASXCode));
        paramList.Add(new PgSqlParameter("@P2", ws.dateFrom));
        paramList.Add(new PgSqlParameter("@P3", ws.dateTo));
        List<DBAccess.ASXPriceDate> priceList = null;
        //   get all price records 
        string orderBy = " ORDER BY apd_pricedate ASC ";
        string where = " AND apd_asxcode = @P1 AND apd_pricedate BETWEEN @P2 AND @P3  ";
        if (!DBAccess.GetAllPrices(paramList, out priceList, DBAccess.ASXPriceDateFieldList, where, orderBy))
          continue;
        // get close price from first record
        decimal firstClosePrice = priceList[0].PrcClose;
        foreach (DBAccess.ASXPriceDate rec in priceList)
        {
          //   foreach price record
          //     get current stat record if it is already there
          List<DBAccess.Statistics> statList = null;
          List<PgSqlParameter> statParams = new List<PgSqlParameter>();
          statParams.Add(new PgSqlParameter("@P6", rec.ASXCode));
          statParams.Add(new PgSqlParameter("@P4", rec.PriceDate));
          statParams.Add(new PgSqlParameter("@P5", (int)StatsType.Price));
          where = " AND st_asxcode = @P6 AND st_startdate = @P4 AND st_type = @P5 ";
          orderBy = string.Empty;
          DBAccess.Statistics statsRec = new DBAccess.Statistics();
          if (!DBAccess.GetStatsRecords(statParams, out statList, DBAccess.StatFieldList, where, orderBy, false))
          {
            statsRec.ASXCode = rec.ASXCode;
            statsRec.StartDate = rec.PriceDate;
            statsRec.Type = StatsType.Price;
          }
          else
            statsRec = statList[0];
          //     calculate stats
          statsRec.Stat = rec.PrcClose == 0 ? 0M : Decimal.Round((rec.PrcClose - firstClosePrice) / firstClosePrice * 100, 6);
          //     insert / update record
          if (statsRec.ID == 0)
            DBAccess.DBInsert(statsRec, "statistics", typeof(DBAccess.Statistics));
          else
            DBAccess.DBUpdate(statsRec, "statistics", typeof(DBAccess.Statistics));
        }
      }

    }
    private StatsType getStatsType()
    {
      string txt = cbxStatsType.SelectedValue.ToString();
      StatsType val = 0;
      Enum.TryParse(txt, out val);
      return val;

    }



    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void cbxStatsType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void updateASXCodeComboBox()
    {
      List<string> incAll = new List<string>();
      incAll.Add(">>ALL<<");
      incAll.AddRange(DBAccess.GetASXCodes(chbOnWatchList.Checked));
      cbxASXCode.DataSource = incAll;
      cbxASXCode.SelectedIndex = 0;
      

    }
    public class chartEntry
    {
      public string date { get; set; }
      public Decimal sumPct { get; set; }
    }
    private void toolStripButtonChart_Click(object sender, EventArgs e)
    {
      //int start = cbxStatsType.SelectedIndex == 0 ? 0 : (int)getStatsType();
      //int fin = cbxStatsType.SelectedIndex == 0 ? 3 : start;
      for (int i = 0; i < 3 ; i++)
      {
        if ((int)getStatsType() != 0 && i != (int) getStatsType() - 1)
        {
          chart1.Series[i].Enabled = false;
          continue;
        }
        chart1.Series[i].Enabled = true;
        List<DBAccess.Statistics> statsList = null;
        string whereClause = " AND st_type = @P1 AND st_startdate BETWEEN @P2 AND @P4 ";
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", i + 1 /* (int)StatsType.Overnight) */));
        paramList.Add(new PgSqlParameter("@P2", DateTime.Compare(dtpFrom.Value, dtpTo.Value) == 0 ? DateTime.MinValue : dtpFrom.Value));
        paramList.Add(new PgSqlParameter("@P4", dtpTo.Value));

        if (cbxASXCode.SelectedIndex > 0)
        {
          whereClause += " AND st_asxcode = @P3 ";
          paramList.Add(new PgSqlParameter("@P3", cbxASXCode.Text));
        }
        if (chbOnWatchList.Checked && cbxASXCode.SelectedIndex <= 0)
        {
          whereClause += " AND st_asxcode IN (@P3) ";
          paramList.Add(new PgSqlParameter("@P3", string.Format("'{0}'", string.Join("','", DBAccess.GetASXCodes(true).ToArray()))));
          //paramList.Add(new PgSqlParameter("@P4", "'ANZ'"));
        }
        if (!DBAccess.GetStatsRecords(paramList, out statsList, DBAccess.StatFieldList, whereClause, " ORDER BY st_startdate, st_asxcode ", false))
        {
          MessageBox.Show("Unable to find stats to update chart", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
          continue;
        }
        var codeList = statsList.Select(x => x.ASXCode).Distinct();
        List<DateTime> dateList = statsList.Select(x => x.StartDate).Distinct().ToList();
        decimal yValue = 0M;
        decimal prevValue = 0M;
        decimal firstPct = 0M;
        bool firstPctSet = false;
        List<chartEntry> chartXY = new List<chartEntry>();
        foreach (DateTime dt in dateList)
        {
          chartEntry eXY = new chartEntry();
          eXY.date = dt.ToShortDateString();
          yValue = statsList.FindAll(delegate (DBAccess.Statistics r1) { return r1.StartDate == dt; }).Select(x => x.Stat).Sum();
          if (!firstPctSet)
          {
            firstPct =  yValue ;
            firstPctSet = true;
          }

          yValue -= firstPct;
          prevValue += yValue;
          eXY.sumPct = i == 2 ? yValue : prevValue;
          chartXY.Add(eXY);
        }
        chart1.Series[i].XValueMember = "date";
        chart1.Series[i].YValueMembers = "sumPct";
        List<string> xValuesList = chartXY.Select(x => x.date).ToList();
        List<decimal> yValuesList = chartXY.Select(x => x.sumPct).ToList();
        chart1.Series[i].Points.DataBindXY(xValuesList, yValuesList);
      }
      
    }

    private void label2_Click(object sender, EventArgs e)
    {

    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void chbOnWatchList_CheckedChanged(object sender, EventArgs e)
    {
      updateASXCodeComboBox();

    }

    private void label3_Click(object sender, EventArgs e)
    {

    }

//  *******************  Generate  *********************

    private void toolStripButtonGenerate_Click(object sender, EventArgs e)
    {
      if (backgroundWorker1.IsBusy)
        return;

      progressBar.Visible = true;
      statusLabel.Visible = true;
      toolStripButtonGenerate.Enabled = false;
      toolStripButtonChart.Enabled = false;

      WorkState ws = new WorkState();
      ws.asxcode = cbxASXCode.SelectedIndex <= 0 ? null : cbxASXCode.SelectedItem.ToString();
      ws.onWatchListOnly = chbOnWatchList.Checked;
      ws.dateFrom = dtpFrom.Value;
      ws.dateTo = dtpTo.Value;
      int x = (int)getStatsType();
      ws.type = x;

      startProgressBarMarquee();
      statusLabel.Text = "Generating ...";
      backgroundWorker1.RunWorkerAsync(ws);

    }

    internal class WorkState
    {
      public string status { get; set; }
      public string asxcode { get; set; }
      public bool onWatchListOnly { get; set; }
      public DateTime dateFrom { get; set; }
      public DateTime dateTo { get; set; }

      public  int type { get; set; }
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      WorkState ws = e.Argument as WorkState;
      switch (ws.type)
      {
        case 1:     // ( Next day's open - today's close ) / today's close
          processPriceChange(false, ws);
          break;
        case 2:     // (today's close - today's open) / today's open
          processPriceChange(true, ws);
          break;
        case 3:     // today's close price - first day's close price / todays close price
          processPriceVsFirst(ws);
          break;
        case 0:
          ws.type = (int)StatsType.Overnight;
          processPriceChange(false, ws);
          ws.type = (int)StatsType.TradeDay;
          processPriceChange(true, ws);
          ws.type = (int)StatsType.Price;
          processPriceVsFirst(ws);
          ws.type = (int)StatsType.All;
          break;
        default:
          break;
      }


      e.Result = true;
    }

    private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      WorkState ws = e.UserState as WorkState;
      statusLabel.Text = ws.status;

    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      endProgressBarMarquee();
      if ((bool)e.Result)
        statusLabel.Text = "Complete";
      else
        statusLabel.Text = "Error generating stats ...";
      toolStripButtonGenerate.Enabled = true;
      toolStripButtonChart.Enabled = true;
      progressBar.Visible = false;
      statusLabel.Visible = false;

    }

    private void startProgressBarMarquee()
    {
      progressBar.Style = ProgressBarStyle.Marquee;
      progressBar.Minimum = 0;
      progressBar.Value = 0;
      progressBar.Visible = true;
      progressBar.Minimum = 1;
      progressBar.Maximum = 100;
      progressBar.Step = 1;
    }

    private void endProgressBarMarquee()
    {
      progressBar.Style = ProgressBarStyle.Blocks;
      progressBar.MarqueeAnimationSpeed = 0;
      progressBar.Value = progressBar.Maximum;
    }
  }


  public enum StatsType
  {
    [Description(">>ALL<<")]
    All = 0,
    [Description("Price Diff Overnight")]
    Overnight = 1,                            // ( Next day's open - today's close ) / today's close
    [Description("Trading Day Price Diff")]
    TradeDay = 2,                             // (today's close - today's open) / today's open
    [Description("Price wrt First Price")]
    Price = 3,                                // today's price - first price found / today's price
  }
}
