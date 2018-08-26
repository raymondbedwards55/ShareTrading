namespace ShareTrading
{
  partial class FrmStatus
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dgvStatus = new System.Windows.Forms.DataGridView();
      this.statusLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.textDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.lastUpdateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusLineBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvStatus
      // 
      this.dgvStatus.AutoGenerateColumns = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.textDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.lastUpdateDataGridViewTextBoxColumn,
            this.notesDataGridViewTextBoxColumn});
      this.dgvStatus.DataSource = this.statusLineBindingSource;
      this.dgvStatus.Location = new System.Drawing.Point(63, 50);
      this.dgvStatus.Name = "dgvStatus";
      this.dgvStatus.Size = new System.Drawing.Size(775, 370);
      this.dgvStatus.TabIndex = 0;
      // 
      // statusLineBindingSource
      // 
      this.statusLineBindingSource.DataSource = typeof(ShareTrading.FrmStatus.statusLine);
      // 
      // textDataGridViewTextBoxColumn
      // 
      this.textDataGridViewTextBoxColumn.DataPropertyName = "text";
      this.textDataGridViewTextBoxColumn.HeaderText = "Description";
      this.textDataGridViewTextBoxColumn.Name = "textDataGridViewTextBoxColumn";
      // 
      // statusDataGridViewTextBoxColumn
      // 
      this.statusDataGridViewTextBoxColumn.DataPropertyName = "status";
      this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
      this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
      // 
      // lastUpdateDataGridViewTextBoxColumn
      // 
      this.lastUpdateDataGridViewTextBoxColumn.DataPropertyName = "lastUpdate";
      this.lastUpdateDataGridViewTextBoxColumn.HeaderText = "Next Update";
      this.lastUpdateDataGridViewTextBoxColumn.Name = "lastUpdateDataGridViewTextBoxColumn";
      this.lastUpdateDataGridViewTextBoxColumn.Width = 200;
      // 
      // notesDataGridViewTextBoxColumn
      // 
      this.notesDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.notesDataGridViewTextBoxColumn.DataPropertyName = "notes";
      this.notesDataGridViewTextBoxColumn.HeaderText = "Notes";
      this.notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
      // 
      // FrmStatus
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(902, 488);
      this.Controls.Add(this.dgvStatus);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.Name = "FrmStatus";
      this.ShowIcon = false;
      this.Text = "Overnight Update Status";
      ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusLineBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvStatus;
    private System.Windows.Forms.BindingSource statusLineBindingSource;
    private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
  }
}