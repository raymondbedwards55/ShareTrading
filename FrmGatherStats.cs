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
      cbxStatsType.DataSource = System.Enum.GetNames(typeof(StatsType));
      cbxStatsType.Focus();
      updateASXCodeComboBox();

    }
    private void toolStripButtonGenerate_Click(object sender, EventArgs e)
    {
      int t = (int) getStatsType();
      switch (t)
      {
        case 1:     // ( Next day's open - today's close ) / today's close
          processPriceChange(false);
          break;
        case 2:     // (today's close - today's open) / today's open
          processPriceChange(true);
          break;
        default:
        break;
      }
    }

    private void toolStripButtonClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void processOvernightPriceChange()
    { }

    private void processPriceChange(bool todays)
    {
      List<DBAccess.ASXPriceDate> coList = new List<DBAccess.ASXPriceDate>();
      if (!DBAccess.GetAllPrices(new List<PgSqlParameter>(), out coList, " DISTINCT apd_asxcode  ", string.Empty, string.Empty))
        return;
      foreach (DBAccess.ASXPriceDate co in coList)
      {
        if (co.ASXCode != "ANZ")
          continue;
        // get max date for this stats type & company
        DateTime maxDate = DateTime.MinValue;
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1",  co.ASXCode));
        List<DBAccess.Statistics> dateList = new List<DBAccess.Statistics>();
        if (DBAccess.GetStatRecords(paramList, out dateList, " MAX(st_startdate) ", " AND st_asxcode = @P1", string.Empty))
          maxDate = dateList[0].StartDate;
        // get all ASXPriceRecords for this company after this date
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", co.ASXCode));
        paramList.Add(new PgSqlParameter("@P2", maxDate));
        List<DBAccess.ASXPriceDate> priceList = new List<DBAccess.ASXPriceDate>();
        if (DBAccess.GetPriceRecords(paramList, out priceList, DBAccess.ASXPriceDateFieldList, " AND apd_asxcode = @P1 AND apd_pricedate >= @P2  " , " ORDER BY apd_pricedate ASC ", false))
        {
          // foreach of the records, create a stats record
          foreach (DBAccess.ASXPriceDate rec in priceList)
          {
            if (rec.PriceDate.Year != 2017 && rec.PriceDate.Month != 2)
              continue;
            DBAccess.Statistics statsRec = new DBAccess.Statistics();
            // Does the record already exist?
            List<DBAccess.Statistics> statsList = new List<DBAccess.Statistics>();
            paramList = new List<PgSqlParameter>();
            paramList.Add(new PgSqlParameter("@P1", rec.ASXCode));
            paramList.Add(new PgSqlParameter("@P2", rec.PriceDate));
            paramList.Add(new PgSqlParameter("@P3", (int)getStatsType()));
            if (DBAccess.GetStatRecords(paramList, out statsList, DBAccess.StatFieldList, " AND st_asxcode = @P1 AND st_startdate >= @P2 AND st_type = @P3 ", string.Empty))
              statsRec = statsList[0];
            else
            {
              statsRec.ASXCode = rec.ASXCode;
              statsRec.StartDate = rec.PriceDate;
              statsRec.Type = getStatsType();
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
      for (int i = 0; i < 2; i++)
      {
        List<DBAccess.Statistics> statsList = null;
        string whereClause = " AND st_type = @P1 AND st_startdate > @P2 ";
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", i + 1 /* (int)StatsType.Overnight) */));
        paramList.Add(new PgSqlParameter("@P2", DateTime.MinValue));
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
        List<chartEntry> chartXY = new List<chartEntry>();
        foreach (DateTime dt in dateList)
        {
          chartEntry eXY = new chartEntry();
          eXY.date = dt.ToShortDateString();
          yValue = statsList.FindAll(delegate (DBAccess.Statistics r1) { return r1.StartDate == dt; }).Select(x => x.Stat).Sum();
          prevValue += yValue;
          eXY.sumPct = prevValue;
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
  }


  public enum StatsType
  {
    [Description("Price Diff Overnight")]
    Overnight = 1,                            // ( Next day's open - today's close ) / today's close
    [Description("Trading Day Price Diff")]
    TradeDay = 2,                             // (today's close - today's open) / today's open
  }
}
