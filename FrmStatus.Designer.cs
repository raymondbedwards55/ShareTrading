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
      this.dgvStatus = new System.Windows.Forms.DataGridView();
      this.textDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.lastUpdateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.statusLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusLineBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvStatus
      // 
      this.dgvStatus.AutoGenerateColumns = false;
      this.dgvStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.textDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.lastUpdateDataGridViewTextBoxColumn});
      this.dgvStatus.DataSource = this.statusLineBindingSource;
      this.dgvStatus.Location = new System.Drawing.Point(74, 84);
      this.dgvStatus.Name = "dgvStatus";
      this.dgvStatus.Size = new System.Drawing.Size(594, 370);
      this.dgvStatus.TabIndex = 0;
      // 
      // textDataGridViewTextBoxColumn
      // 
      this.textDataGridViewTextBoxColumn.DataPropertyName = "text";
      this.textDataGridViewTextBoxColumn.HeaderText = "text";
      this.textDataGridViewTextBoxColumn.Name = "textDataGridViewTextBoxColumn";
      // 
      // statusDataGridViewTextBoxColumn
      // 
      this.statusDataGridViewTextBoxColumn.DataPropertyName = "status";
      this.statusDataGridViewTextBoxColumn.HeaderText = "status";
      this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
      this.statusDataGridViewTextBoxColumn.Width = 300;
      // 
      // lastUpdateDataGridViewTextBoxColumn
      // 
      this.lastUpdateDataGridViewTextBoxColumn.DataPropertyName = "lastUpdate";
      this.lastUpdateDataGridViewTextBoxColumn.HeaderText = "lastUpdate";
      this.lastUpdateDataGridViewTextBoxColumn.Name = "lastUpdateDataGridViewTextBoxColumn";
      this.lastUpdateDataGridViewTextBoxColumn.Width = 150;
      // 
      // statusLineBindingSource
      // 
      this.statusLineBindingSource.DataSource = typeof(ShareTrading.FrmStatus.statusLine);
      // 
      // FrmStatus
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(701, 538);
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
    private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDataGridViewTextBoxColumn;
    private System.Windows.Forms.BindingSource statusLineBindingSource;
  }
}