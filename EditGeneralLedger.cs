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
  public partial class EditGeneralLedger : Form
  {
    bool editing = false;
    DBAccess.GLCodes currentRecord = new DBAccess.GLCodes();
    public EditGeneralLedger()
    {
      InitializeComponent();
      initialiseForm();
    }

   private void initialiseForm()
    {
      whileEditing(false);
      cbxGLType.DataSource = System.Enum.GetNames(typeof(DBAccess.GLType));
      // set up list of existing records
      refreshGrid(0);
      cbxGLType.Focus();
    }

    private void displayHighlightedRow()
    {
      try
      {
        int rowIdx = dgvGeneralLedger.CurrentCell.RowIndex;
        currentRecord = ((List<DBAccess.GLCodes>)GLBindingSource.DataSource)[rowIdx];
        tbxGLCode.Text = currentRecord.GLCode;
        cbxGLType.SelectedIndex = ((List<string>)cbxGLType.DataSource).FindIndex(x => x == currentRecord.Type.ToString());
        chbGSTApplies.Checked = currentRecord.GSTApplies;
      }
      catch { }

    }

    private void EditGeneralLedger_FormClosing(object sender, FormClosingEventArgs e)
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
      cbxGLType.Enabled = state;
      tbxGLCode.Enabled = state;
      chbGSTApplies.Enabled = state;

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
      cbxGLType.Enabled = false;
      tbxGLCode.Enabled = false;
      chbGSTApplies.Enabled = false;

    }
    private void toolStripButtonAdd_Click(object sender, EventArgs e)
    {
      whileEditing(true);
      cbxGLType.SelectedIndex = 0;
      tbxGLCode.Text = string.Empty;
      chbGSTApplies.Checked = false;
      currentRecord = new DBAccess.GLCodes();
    }

    private DBAccess.GLType getType()
    {
      string txt = cbxGLType.SelectedValue.ToString();
      DBAccess.GLType val = 0;
      Enum.TryParse(txt, out val);
      return val;

    }

    private void toolStripButtonSave_Click(object sender, EventArgs e)
    {
      if (!editing)
        return;
        // Adding a record so just make sure it's not a duplicate
        List<DBAccess.GLCodes> list = new List<DBAccess.GLCodes>();
        List<PgSqlParameter> paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P2", tbxGLCode.Text));
          
        paramList.Add(new PgSqlParameter("@P3", (int) getType()));
      if (currentRecord.ID == 0)
      {
        if (DBAccess.GetGLRecords(paramList, out list, DBAccess.GLFieldList, " AND gl_code = @P2 AND gl_type = @P3 " /* extraWhere */, string.Empty /* orderBy */))
        {
          MessageBox.Show("Duplicate Type / Code combination", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
        // Time to add a record
        currentRecord = new DBAccess.GLCodes();
        currentRecord.GLCode = tbxGLCode.Text;
        currentRecord.GSTApplies = chbGSTApplies.Checked;
        currentRecord.Type = getType();
        DBAccess.DBInsert(currentRecord, "glcodes", typeof(DBAccess.GLCodes));
        paramList = new List<PgSqlParameter>();
        paramList.Add(new PgSqlParameter("@P4", tbxGLCode.Text));

        paramList.Add(new PgSqlParameter("@P5", (int)getType()));
        if (DBAccess.GetGLRecords(paramList, out list, DBAccess.GLFieldList, " AND gl_code = @P4 AND gl_type = @P5 " /* extraWhere */, string.Empty /* orderBy */))
          currentRecord.ID = list[0].ID;
      }
      else
      {
        // Update record
        paramList.Add(new PgSqlParameter("@P4", currentRecord.ID));

        if (DBAccess.GetGLRecords(paramList, out list, DBAccess.GLFieldList, " AND gl_code = @P2 AND gl_type = @P3 AND gl_id != @P4 " /* extraWhere */, string.Empty /* orderBy */))
        {
          MessageBox.Show("Duplicate Type / Code combination", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
        // Time to add a record
        currentRecord.GLCode = tbxGLCode.Text;
        currentRecord.GSTApplies = chbGSTApplies.Checked;
        currentRecord.Type = getType();
        DBAccess.DBUpdate(currentRecord, "glcodes", typeof(DBAccess.GLCodes));
      }
      whileEditing(false);
      // refresh the grid
      refreshGrid(currentRecord.ID);
    }

    private void refreshGrid(int id)
    {
      GLBindingSource.DataSource = null;
      List<DBAccess.GLCodes> list = new List<DBAccess.GLCodes>();

        DBAccess.GetGLRecords(new List<PgSqlParameter>(), out list, DBAccess.GLFieldList, string.Empty /* extraWhere */, " ORDER BY gl_type ", chbIncDeleted.Checked);
      GLBindingSource.DataSource = list;
      if (GLBindingSource != null && GLBindingSource.Count > 0)
      {
        // display in data grid
        dgvGeneralLedger.DataSource = GLBindingSource;
        dgvGeneralLedger.Refresh();
        highlightSelectedRow(id);
        displayHighlightedRow();
      }
      else
        dgvGeneralLedger.Enabled = false;
    }

    private void highlightSelectedRow(int id)
    {
      //
      try
      {
        dgvGeneralLedger.ClearSelection();
        if (id == 0)
          dgvGeneralLedger.Rows[0].Selected = true;
        else
        {
          foreach (DataGridViewRow row in dgvGeneralLedger.Rows)
          {
            if (row.Cells["ID"].Value.Equals(id))
            {
              dgvGeneralLedger.Rows[row.Index].Cells[0].Selected = true;
              dgvGeneralLedger.CurrentCell = dgvGeneralLedger.Rows[row.Index].Cells[0];
              break;
            }
          }
        }
      }
      catch
      { }
    }
    private void dgvGeneralLedger_SelectionChanged(object sender, EventArgs e)
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
      DBAccess.DBUpdate(currentRecord, "glcodes", typeof(DBAccess.GLCodes));
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
        int rowIdx = dgvGeneralLedger.CurrentCell.RowIndex;
        if (rowIdx <= 0)
          return;
        rowIdx -= 1;
        dgvGeneralLedger.ClearSelection();
        dgvGeneralLedger.Rows[rowIdx].Cells[0].Selected = true;
        dgvGeneralLedger.CurrentCell = dgvGeneralLedger.Rows[rowIdx].Cells[0];
        displayHighlightedRow();
      }
      catch { }
    }

    private void toolStripButtonNext_Click(object sender, EventArgs e)
    {
      try
      {
        int rowIdx = dgvGeneralLedger.CurrentCell.RowIndex;
        if (rowIdx >= dgvGeneralLedger.RowCount - 1)
          return;
        rowIdx += 1;
        dgvGeneralLedger.ClearSelection();
        dgvGeneralLedger.Rows[rowIdx].Cells[0].Selected = true;
        dgvGeneralLedger.CurrentCell = dgvGeneralLedger.Rows[rowIdx].Cells[0];
        displayHighlightedRow();
      }
      catch { }

    }

    private void chbIncDeleted_CheckedChanged(object sender, EventArgs e)
    {
      refreshGrid(currentRecord.ID);
    }

    private void dgvGeneralLedger_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridViewRow row = dgvGeneralLedger.Rows[e.RowIndex];
        if (DateTime.Compare(DateTime.Parse(row.Cells["DateDeleted"].Value.ToString()), DateTime.MinValue) != 0)
          row.DefaultCellStyle.ForeColor = Color.Red;

    }

    private void toolStripButtonUnDelete_Click(object sender, EventArgs e)
    {
      currentRecord.DateDeleted = DateTime.MinValue;
      DBAccess.DBUpdate(currentRecord, "glcodes", typeof(DBAccess.GLCodes));
      refreshGrid(currentRecord.ID);
      displayHighlightedRow();
    }
  }
}
