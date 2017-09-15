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
  public partial class FrmEnterSellConfrmationNr : Form
  {

    bool firstTimeEnter = true;
    public FrmEnterSellConfrmationNr()
    {
      InitializeComponent();
    }

    private void tbxASXCode_Leave(object sender, EventArgs e)
    {
      populateGrid(tbxASXCode.Text);
      toolStripButtonCancel.Enabled = true;
      toolStripButtonNew.Enabled = false;
      toolStripButtonClose.Enabled = false;
      toolStripButtonSave.Enabled = true;
      firstTimeEnter = true;
    }

    private void toolStripButtonNew_Click(object sender, EventArgs e)
    {
      tbxASXCode.Text = string.Empty;
      populateGrid(null);
      tbxASXCode.Focus();
    }

    private void toolStripButtonSave_Click(object sender, EventArgs e)
    {
      // Make sure there is a valid ASX Code and a confirmation number entered somewhere in the grid
      if (string.IsNullOrEmpty(tbxASXCode.Text))
      {
        populateGrid(null);
        tbxASXCode.Focus();
      }
      else
      {
        this.Validate();
        // Is there a confirmation number somewhere?
        foreach (DBAccess.TransRecords line in (List<DBAccess.TransRecords>) BuyTransactionsBindingSource.DataSource)
        {
          if (string.IsNullOrEmpty(line.SellConfirmation))
            continue;
          DBAccess.TransUpdate(line);
          break;
        }
      }
      toolStripButtonCancel.Enabled = false;
      toolStripButtonNew.Enabled = true;
      toolStripButtonClose.Enabled = true;
      toolStripButtonSave.Enabled = false;
    }

    private void toolStripButtonCancel_Click(object sender, EventArgs e)
    {
      tbxASXCode.Text = string.Empty;
      populateGrid(null);
      toolStripButtonCancel.Enabled = false;
      toolStripButtonNew.Enabled = true;
      toolStripButtonClose.Enabled = true;
      toolStripButtonSave.Enabled = false;
    }

    private void toolStripButtonClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void populateGrid(string ASXCode)
    {
      BuyTransactionsBindingSource.DataSource = null;
      dgvBuyTransactions.DataSource = null;
      if (!string.IsNullOrEmpty(ASXCode))
      {
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P1", ASXCode));
        List<DBAccess.TransRecords> buyList = new List<DBAccess.TransRecords>();
        // get buy transactions that still have SOH
        if (DBAccess.GetTransRecords(paramList, out buyList, DBAccess.TransRecordsFieldList, " AND trn_asxcode = @P1 AND trn_soh > 0 AND trn_buysell = 'Buy' ", " ORDER BY trn_transdate DESC ", false))
        {
          // display the buy transactions
          BuyTransactionsBindingSource.DataSource = buyList;
          dgvBuyTransactions.DataSource = BuyTransactionsBindingSource;
        }
      }
      dgvBuyTransactions.Refresh();
    }

    private void FrmEnterSellConfrmationNr_Load(object sender, EventArgs e)
    {
      populateGrid(null);
      toolStripButtonCancel.Enabled = false;
      toolStripButtonNew.Enabled = true;
      toolStripButtonClose.Enabled = true;
      toolStripButtonSave.Enabled = false;

    }

    private void dgvBuyTransactions_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (dgvBuyTransactions.CurrentRow.Cells[e.ColumnIndex].ReadOnly)
      {
        SendKeys.Send("{tab}");
      }
    }

    private void dgvBuyTransactions_Enter(object sender, EventArgs e)
    {
      // when entering the grid, throw away any tab that was pressed to get there
      firstTimeEnter = true;
    }

    private void dgvBuyTransactions_KeyDown(object sender, KeyEventArgs e)
    {
      if (firstTimeEnter)
      { 
        if (e.KeyCode == Keys.Tab)
        {
          e.Handled = true;
        }
        firstTimeEnter = false;
      }
    }
  }
}
