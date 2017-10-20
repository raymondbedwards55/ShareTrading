
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devart.Common;
using Devart.Data.PostgreSql;

namespace ShareTrading
{
  public partial class FrmEditCompanyDetails : Form
  {
    bool editing = false;
    DBAccess.CompanyDetails currentRecord = new DBAccess.CompanyDetails();
    public FrmEditCompanyDetails()
    {
      InitializeComponent();
      initialiseForm();
    }

    private void initialiseForm()
    {
      whileEditing(false);
      
      cbxASXCode.DataSource = DBAccess.GetASXCodes();

      // set up list of existing records
      refreshGrid(0);
      cbxASXCode.Focus();
    }

    private void displayHighlightedRow()
    {
      try
      {
        int rowIdx = dgvCompanyDetails.CurrentCell.RowIndex;
        currentRecord = ((List<DBAccess.CompanyDetails>)CoBindingSource.DataSource)[rowIdx];
        tbxCompanyName.Text = currentRecord.CompanyName;
        chbOnWatchList.Checked = currentRecord.OnWatchList;
        cbxASXCode.SelectedIndex = ((List<string>)cbxASXCode.DataSource).FindIndex(x => x == currentRecord.ASXCode);
        chbOnWatchList.Checked = currentRecord.OnWatchList;
      }
      catch { }

    }

    private void FrmEditCompanyDetails_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (editing)
      {
        MessageBox.Show("Please save or Cancel your changes", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      Close();
    }

    private void whileEditing(bool state)
    {
      toolStripButtonClose.Enabled = !state;
      toolStripButtonAdd.Enabled = !state;
      toolStripButtonEdit.Enabled = !state;
      toolStripButtonDelete.Enabled = !state;
      toolStripButtonUnDelete.Enabled = false;
      toolStripButtonNext.Enabled = !state;
      toolStripButtonPrev.Enabled = !state;

      toolStripButtonCancel.Enabled = state;
      toolStripButtonSave.Enabled = state;

      editing = state;
      cbxASXCode.Enabled = state;
      tbxCompanyName.Enabled = state;
      chbOnWatchList.Enabled = state;

    }

    private void editingDeleted()
    {
      toolStripButtonClose.Enabled = true;
      toolStripButtonAdd.Enabled = false;
      toolStripButtonEdit.Enabled = false;
      toolStripButtonDelete.Enabled = false;
      toolStripButtonUnDelete.Enabled = true;
      toolStripButtonNext.Enabled = true;
      toolStripButtonPrev.Enabled = true;

      toolStripButtonCancel.Enabled = false;
      toolStripButtonSave.Enabled = false;

      editing = false;
      cbxASXCode.Enabled = false;
      tbxCompanyName.Enabled = false;
      chbOnWatchList.Enabled = false;

    }
    private void toolStripButtonAdd_Click(object sender, EventArgs e)
    {
      whileEditing(true);

      cbxASXCode.SelectedIndex = -1;
      tbxCompanyName.Text = string.Empty;
      chbOnWatchList.Checked = false;
      currentRecord = new DBAccess.CompanyDetails();
    }



    private void toolStripButtonSave_Click(object sender, EventArgs e)
    {
      if (!editing)
        return;
      // Adding a record so just make sure it's not a duplicate
      List<DBAccess.CompanyDetails> list = new List<DBAccess.CompanyDetails>();
      if (currentRecord.ID == 0)
      {
        if (DBAccess.GetCompanyDetails(cbxASXCode.Text, out list,  false))
        {
          MessageBox.Show("Duplicate ASX Code", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
        // Time to add a record
        currentRecord = new DBAccess.CompanyDetails();
        currentRecord.CompanyName = tbxCompanyName.Text;
        currentRecord.OnWatchList = chbOnWatchList.Checked;
        currentRecord.ASXCode = cbxASXCode.Text;
        DBAccess.DBInsert(currentRecord, "companydetails", typeof(DBAccess.CompanyDetails));
        if (DBAccess.GetCompanyDetails(cbxASXCode.Text, out list, false))
          currentRecord.ID = list[0].ID;
      }
      else
      {
        // Update record

        if (!DBAccess.GetCompanyDetails(cbxASXCode.Text, out list, false))
        {
          MessageBox.Show("Unable to find ASX Code", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
        // Time to add a record
        currentRecord.CompanyName = tbxCompanyName.Text;
        currentRecord.OnWatchList = chbOnWatchList.Checked;
        DBAccess.DBUpdate(currentRecord, "companydetails", typeof(DBAccess.CompanyDetails));
      }
      whileEditing(false);
      // refresh the grid
      refreshGrid(currentRecord.ID);
    }

    private void refreshGrid(int id)
    {
      CoBindingSource.DataSource = null;
      List<DBAccess.CompanyDetails> list = new List<DBAccess.CompanyDetails>();

      DBAccess.GetCompanyDetails(null, out list, chbIncDeleted.Checked);
      CoBindingSource.DataSource = list;
      if (CoBindingSource != null && CoBindingSource.Count > 0)
      {
        // display in data grid
        dgvCompanyDetails.DataSource = CoBindingSource;
        dgvCompanyDetails.Refresh();
        highlightSelectedRow(id);
        displayHighlightedRow();
      }
      else
        dgvCompanyDetails.Enabled = false;
    }

    private void highlightSelectedRow(int id)
    {
      //
      try
      {
        dgvCompanyDetails.ClearSelection();
        if (id == 0)
          dgvCompanyDetails.Rows[0].Selected = true;
        else
        {
          foreach (DataGridViewRow row in dgvCompanyDetails.Rows)
          {
            if (row.Cells["ID"].Value.Equals(id))
            {
              dgvCompanyDetails.Rows[row.Index].Cells[0].Selected = true;
              dgvCompanyDetails.CurrentCell = dgvCompanyDetails.Rows[row.Index].Cells[0];
              break;
            }
          }
        }
      }
      catch
      { }
    }
    private void dgvCompanyDetails_SelectionChanged(object sender, EventArgs e)
    {

      displayHighlightedRow();
      if (DateTime.Compare(currentRecord.DateDeleted, DateTime.MinValue) == 0)
        whileEditing(editing);
      else
        editingDeleted();
    }

    private void toolStripButtonEdit_Click(object sender, EventArgs e)
    {
      if (currentRecord.ID == 0)
      {
        MessageBox.Show("Please select row to edit", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      whileEditing(true);
    }

    private void toolStripButtonCancel_Click(object sender, EventArgs e)
    {
      // Cancel Insert or Update
      displayHighlightedRow();
      whileEditing(false);
    }

    private void toolStripButtonDelete_Click(object sender, EventArgs e)
    {
      // 
      currentRecord.DateDeleted = DateTime.Now;
      DBAccess.DBUpdate(currentRecord, "companydetails", typeof(DBAccess.CompanyDetails));
      refreshGrid(chbIncDeleted.Checked ? currentRecord.ID : 0);
      displayHighlightedRow();
    }

    private void toolStripButtonClose_Click(object sender, EventArgs e)
    {
      if (editing)
      {
        MessageBox.Show("Please save or Cancel your changes", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      this.Close();

    }

    private void toolStripButtonPrev_Click(object sender, EventArgs e)
    {
      try
      {
        int rowIdx = dgvCompanyDetails.CurrentCell.RowIndex;
        if (rowIdx <= 0)
          return;
        rowIdx -= 1;
        dgvCompanyDetails.ClearSelection();
        dgvCompanyDetails.Rows[rowIdx].Cells[0].Selected = true;
        dgvCompanyDetails.CurrentCell = dgvCompanyDetails.Rows[rowIdx].Cells[0];
        displayHighlightedRow();
      }
      catch { }
    }

    private void toolStripButtonNext_Click(object sender, EventArgs e)
    {
      try
      {
        int rowIdx = dgvCompanyDetails.CurrentCell.RowIndex;
        if (rowIdx >= dgvCompanyDetails.RowCount - 1)
          return;
        rowIdx += 1;
        dgvCompanyDetails.ClearSelection();
        dgvCompanyDetails.Rows[rowIdx].Cells[0].Selected = true;
        dgvCompanyDetails.CurrentCell = dgvCompanyDetails.Rows[rowIdx].Cells[0];
        displayHighlightedRow();
      }
      catch { }

    }

    private void chbIncDeleted_CheckedChanged(object sender, EventArgs e)
    {
      refreshGrid(currentRecord.ID);
    }

    private void dgvCompanyDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridViewRow row = dgvCompanyDetails.Rows[e.RowIndex];
      if (DateTime.Compare(DateTime.Parse(row.Cells["DateDeleted"].Value.ToString()), DateTime.MinValue) != 0)
        row.DefaultCellStyle.ForeColor = Color.Red;

    }

    private void toolStripButtonUnDelete_Click(object sender, EventArgs e)
    {
      currentRecord.DateDeleted = DateTime.MinValue;
      DBAccess.DBUpdate(currentRecord, "companydetails", typeof(DBAccess.CompanyDetails));
      refreshGrid(currentRecord.ID);
      displayHighlightedRow();
    }
  }
}

