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
  public partial class FrmSuggestions : Form
  {
    public FrmSuggestions()
    {
      InitializeComponent();
    }

    private void FrmSuggestions_Load(object sender, EventArgs e)
    {
      populateGrid();
    }

    private void populateGrid()
    {
      List<Suggestions> suggestList = new List<Suggestions>();
      // Get all the shares we have stock of
      List<DBAccess.TransRecords> transList = null;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();

      if (!DBAccess.GetTransRecords(null, out transList, DBAccess.TransRecordsFieldList, " AND trn_soh > 0 ", " ORDER BY trn_ASXCode, trn_transdate DESC", false))
      {
        MessageBox.Show("No transactions found", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      var uCode = transList.Select(x => x.ASXCode).Distinct().ToList();
      foreach (string code in uCode)
      {
        List<DBAccess.TransRecords> codeList = transList.FindAll(delegate (DBAccess.TransRecords r1) { return r1.ASXCode == code; });
        // For each ASXCode, get todays price and work out the percentages
        List<DBAccess.ASXPriceDate> todaysPrice = null;
        DateTime dt = DateTime.MinValue;
        Decimal prc = 0M;
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", code));
        if (DBAccess.GetPriceRecords(paramList, out todaysPrice, DBAccess.ASXPriceDateFieldList, " AND apd_asxcode = @P1 ", "ORDER BY apd_pricedate DESC ", false))
        {
          // no prices for this ASX Code so we'll pretend
          
        }
        if (todaysPrice != null && todaysPrice.Count > 0)
        {
          prc = todaysPrice[0].PrcClose;
          dt = todaysPrice[0].PriceDate;
        }
        // create records to display
        foreach (DBAccess.TransRecords transRec in codeList)
        {
          int daysHeld = (DateTime.Today - transRec.TranDate.Date).Days;
          if (daysHeld < 3)
            daysHeld = 3;         // takes 3 days for the transaction to be finalised
          Suggestions sug = new Suggestions();
          sug.ASXCode = transRec.ASXCode;
          sug.SOH = transRec.SOH;
          sug.UnitBuyPrice = transRec.UnitPrice;
          sug.TodaysUnitPrice = prc;
          sug.PctGain = 0M;
          sug.PctYear = 0M;
          if (prc != 0 && sug.UnitBuyPrice != 0)
          {
            sug.PctGain = Decimal.Round((sug.TodaysUnitPrice - sug.UnitBuyPrice) * 100 / sug.UnitBuyPrice, 2);
            sug.PctYear = Decimal.Round((sug.TodaysUnitPrice - sug.UnitBuyPrice) / sug.UnitBuyPrice * 365 * 100 / daysHeld, 2);
          }
          suggestList.Add(sug);
        }

      }
      suggestList = suggestList.OrderByDescending(x => x.PctGain).ToList();
      // 
      dgvSuggestions.DataSource = null;
      SuggestionsBindingSource.DataSource = suggestList;
      dgvSuggestions.DataSource = SuggestionsBindingSource;
      dgvSuggestions.Refresh();

    }

    public class Suggestions
    {
      public string ASXCode { get; set; }
      public int SOH { get; set; }
      public decimal UnitBuyPrice { get; set; }
      public decimal TodaysUnitPrice { get; set; }
      public decimal PctGain { get; set; }
      public decimal PctYear { get; set; }
    }

    private void toolStripButtonClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void toolStripButtonUpdate_Click(object sender, EventArgs e)
    {
      populateGrid();
    }

    private int _previousIndex;
    private bool _sortDirection;

    private void dgvSuggestions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == _previousIndex)
        _sortDirection ^= true; // toggle direction

      dgvSuggestions.DataSource = SortData(
          (List<Suggestions>)SuggestionsBindingSource.DataSource, dgvSuggestions.Columns[e.ColumnIndex].Name, _sortDirection);

      _previousIndex = e.ColumnIndex;
    }

    public List<Suggestions> SortData(List<Suggestions> list, string column, bool ascending)
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

    private void dgvSuggestions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridView dgv = sender as DataGridView;
      if (dgv.Columns[e.ColumnIndex].ValueType == typeof(decimal))
        if ((decimal)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value < 0M)
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
    }
  }
}
