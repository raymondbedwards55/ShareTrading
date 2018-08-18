using System;
using System.Collections.Generic;
using Devart.Data.PostgreSql;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using ShareTrading.Common.Src;
using System.Drawing;


namespace ShareTrading
{
  public partial class FrmBrokersRec : Form
  {
    public FrmBrokersRec()
    {
      InitializeComponent();
      getData();
    }
 
    private void getData()
    {
      List<recommendation> displayLines = new List<recommendation>();
      // get list of dates
      List<DBAccess.BrokersRecommendations> dtList = new List<DBAccess.BrokersRecommendations>();
      string extraWhere = string.Empty;
      string orderBy = string.Empty;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      int count = 0;
      if (DBAccess.GetBrokersRecommendationsRecords(null, out dtList, " DISTINCT(br_transdate) ", string.Empty, " ORDER BY br_transdate DESC ", false))
      {
          dgvRecommendation.Columns[5].HeaderText = dtList.Count > 0 ? dtList[0].HistoryDate.ToShortDateString() : string.Empty;
        dgvRecommendation.Columns[8].HeaderText = dtList.Count > 1 ? dtList[1].HistoryDate.ToShortDateString() : string.Empty;
        dgvRecommendation.Columns[11].HeaderText = dtList.Count > 2 ? dtList[2].HistoryDate.ToShortDateString() : string.Empty;
        dgvRecommendation.Columns[14].HeaderText = dtList.Count > 3 ? dtList[3].HistoryDate.ToShortDateString() : string.Empty;
        dgvRecommendation.Columns[17].HeaderText = dtList.Count > 4 ? dtList[4].HistoryDate.ToShortDateString() : string.Empty;
        foreach (DBAccess.BrokersRecommendations dt in dtList)
          {
          count++;
          if (count > 5)
            break;
          if (count > dtList.Count)
            break;
            paramList = new List<PgSqlParameter>();
            paramList.Add(new PgSqlParameter(string.Format("@P{0}", (count).ToString()), dtList[count-1].HistoryDate));
            extraWhere = string.Format(" AND br_transdate = @P{0} ", (count).ToString());

            List<DBAccess.BrokersRecommendations> varList = new List<DBAccess.BrokersRecommendations>();
            if (DBAccess.GetBrokersRecommendationsRecords(paramList, out varList, DBAccess.BrokersRecommendationsList, extraWhere, orderBy, false))
            {
              foreach (DBAccess.BrokersRecommendations rec in varList)
              {
                recommendation existing = displayLines.Find(delegate (recommendation r1) { return r1.RecASXCode == rec.ASXCode; });
                // get record with ASX Code if any else create one
                if (existing == null)
                {
                  existing = new recommendation();
                  existing.RecASXCode = rec.ASXCode;
                existing.RecCurrentPrice = getCurrentPrice(rec.ASXCode);
                
                  displayLines.Add(existing);
                }
              switch (count)
              {
                case 1:
                  existing.Rec1 = rec.Consensus;
                  existing.RecDate1 = rec.HistoryDate;
                  existing.RecPrice1 = rec.Price;
                  existing.RecDiff = existing.RecCurrentPrice == 0M ? 0M : Decimal.Round((existing.RecCurrentPrice - rec.Price) / existing.RecCurrentPrice, 2);
                  break;
                case 2:
                  existing.Rec2 = rec.Consensus;
                  existing.RecDate2 = rec.HistoryDate;
                  existing.RecPrice2 = rec.Price;
                  if (existing.RecPrice1 == 0M)
                    existing.RecDiff = existing.RecCurrentPrice == 0M ? 0M : Decimal.Round((existing.RecCurrentPrice - rec.Price ) / existing.RecCurrentPrice, 2);
                  else
                    //  latest 2 dates have recommendations so set Recommendation Type to indicate if recommendation has changed
                    existing.RecChanged = getValue(existing.Rec1) == getValue(existing.Rec2) ? "" : getValue(existing.Rec1) > getValue(existing.Rec2) ? "U" : "D";
                    break;
                  case 3:
                    existing.Rec3 = rec.Consensus;
                    existing.RecDate3 = rec.HistoryDate;
                    existing.RecPrice3 = rec.Price;
                    break;
                  case 4:
                    existing.Rec4 = rec.Consensus;
                    existing.RecDate4 = rec.HistoryDate;
                    existing.RecPrice4 = rec.Price;
                    break;
                  case 5:
                    existing.Rec5 = rec.Consensus;
                    existing.RecDate5 = rec.HistoryDate;
                    existing.RecPrice5 = rec.Price;
                    break;
                  default:
                    break;
                }
              }
            }
          }
        }
      dgvRecommendation.DataSource = null;
      recommendationBindingSource.DataSource = displayLines;
      dgvRecommendation.DataSource = recommendationBindingSource;


      dgvRecommendation.Refresh();
    }
    private RecommendationType getValue(string recommendation)
    {
      for (int i = 0; i < (int) RecommendationType.max; i++)
      {
        if (recommendation == EnumHelper.GetEnumDescription((RecommendationType)i))
          return (RecommendationType)i;
      }
      return RecommendationType.strongSell;
    }
  private decimal getCurrentPrice(string ASXCode)
  {
      List<DBAccess.ASXPriceDate> prcList = new List<DBAccess.ASXPriceDate>();
      string extraWhere = " AND apd_pricedate = (SELECT MAX(apd_pricedate) FROM asxpricedate WHERE apd_asxcode = @P1 ) AND apd_asxcode = @P1 " ;
      string orderBy = string.Empty;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", ASXCode));
      if (DBAccess.GetPriceRecords(paramList, out prcList, DBAccess.ASXPriceDateFieldList, extraWhere, string.Empty, false))
      {
        return prcList[0].PrcClose;
      }
      else
        return 0M;
  }
    private int _previousIndex;
    private bool _sortDirection;


    private void dgvRecommendation_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == _previousIndex)
        _sortDirection ^= true; // toggle direction

      dgvRecommendation.DataSource = SortDirectorsData(
          (List<recommendation>)recommendationBindingSource.DataSource, dgvRecommendation.Columns[e.ColumnIndex].Name, _sortDirection);

      _previousIndex = e.ColumnIndex;
    }

    public List<recommendation> SortDirectorsData(List<recommendation> list, string column, bool ascending)
    {
      try
      {
        return ascending ?
           /* RefreshId( */ list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /* ) */ :
           /* RefreshId( */ list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() /*) */;
      }
      catch
      { }
      return list;
    }
    private void dgvRecommendations_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridView dgv = sender as DataGridView;
      if (dgv.Columns[e.ColumnIndex].ValueType == typeof(decimal))
        if ((decimal)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value < 0M)
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
      if (dgv.Columns[e.ColumnIndex].Name == "RecChanged" )
      {
        if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
          return;
          if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "U")
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.ForestGreen;
        else
          if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "D")
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Fuchsia;
      }

    }

  }


  public class recommendation
  {
    public string RecASXCode { get; set; }
    public string RecChanged { get; set; }
    public decimal RecCurrentPrice { get; set; }
    public decimal RecDiff { get; set; }
    public DateTime RecDate1 { get; set; }
    public decimal RecPrice1 { get; set; }
    public string Rec1 { get; set; }
    public DateTime RecDate2 { get; set; }
    public decimal RecPrice2 { get; set; }
    public string Rec2 { get; set; }
    public DateTime RecDate3 { get; set; }
    public decimal RecPrice3 { get; set; }
    public string Rec3 { get; set; }
    public DateTime RecDate4 { get; set; }
    public decimal RecPrice4 { get; set; }
    public string Rec4 { get; set; }
    public DateTime RecDate5 { get; set; }
    public decimal RecPrice5 { get; set; }
    public string Rec5 { get; set; }

  }

 
  public enum RecommendationType
  {
    [Description("No Match")]
    noMatch = 0,
    [Description ("Strong Sell")]
    strongSell = 1,
    [Description("Sell")]
    sell = 2,
    [Description("Hold")]
    hold = 3,
    [Description("Buy")]
    buy = 4,
    [Description("Strong Buy")]
    strongBuy = 5,
    [Description("Max")]
    max = 6
  }
}
