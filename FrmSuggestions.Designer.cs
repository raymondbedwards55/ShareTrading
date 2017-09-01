namespace ShareTrading
{
  partial class FrmSuggestions
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dgvSuggestions = new System.Windows.Forms.DataGridView();
      this.SuggestionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonUpdate = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
      this.ASXCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SOH = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.UnitBuyPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TodaysUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.PctGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.PctYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvSuggestions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.SuggestionsBindingSource)).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dgvSuggestions
      // 
      this.dgvSuggestions.AllowUserToAddRows = false;
      this.dgvSuggestions.AllowUserToDeleteRows = false;
      this.dgvSuggestions.AutoGenerateColumns = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvSuggestions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvSuggestions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvSuggestions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ASXCode,
            this.SOH,
            this.UnitBuyPrice,
            this.TodaysUnitPrice,
            this.PctGain,
            this.PctYear});
      this.dgvSuggestions.DataSource = this.SuggestionsBindingSource;
      this.dgvSuggestions.Location = new System.Drawing.Point(39, 67);
      this.dgvSuggestions.Name = "dgvSuggestions";
      this.dgvSuggestions.Size = new System.Drawing.Size(682, 656);
      this.dgvSuggestions.TabIndex = 0;
      this.dgvSuggestions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSuggestions_CellFormatting);
      this.dgvSuggestions.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSuggestions_ColumnHeaderMouseClick);
      // 
      // SuggestionsBindingSource
      // 
      this.SuggestionsBindingSource.AllowNew = false;
      this.SuggestionsBindingSource.DataSource = typeof(ShareTrading.FrmSuggestions.Suggestions);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonUpdate,
            this.toolStripButtonClose});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(763, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripButtonUpdate
      // 
      this.toolStripButtonUpdate.Image = global::ShareTrading.Properties.Resources.gear_in;
      this.toolStripButtonUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonUpdate.Name = "toolStripButtonUpdate";
      this.toolStripButtonUpdate.Size = new System.Drawing.Size(65, 22);
      this.toolStripButtonUpdate.Text = "Update";
      this.toolStripButtonUpdate.Click += new System.EventHandler(this.toolStripButtonUpdate_Click);
      // 
      // toolStripButtonClose
      // 
      this.toolStripButtonClose.Image = global::ShareTrading.Properties.Resources.door_out;
      this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonClose.Name = "toolStripButtonClose";
      this.toolStripButtonClose.Size = new System.Drawing.Size(56, 22);
      this.toolStripButtonClose.Text = "Close";
      this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
      // 
      // ASXCode
      // 
      this.ASXCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.ASXCode.DataPropertyName = "ASXCode";
      this.ASXCode.HeaderText = "ASX Code";
      this.ASXCode.Name = "ASXCode";
      // 
      // SOH
      // 
      this.SOH.DataPropertyName = "SOH";
      dataGridViewCellStyle2.Format = "N0";
      dataGridViewCellStyle2.NullValue = "0";
      this.SOH.DefaultCellStyle = dataGridViewCellStyle2;
      this.SOH.HeaderText = "SOH";
      this.SOH.Name = "SOH";
      // 
      // UnitBuyPrice
      // 
      this.UnitBuyPrice.DataPropertyName = "UnitBuyPrice";
      dataGridViewCellStyle3.Format = "C2";
      dataGridViewCellStyle3.NullValue = "0";
      this.UnitBuyPrice.DefaultCellStyle = dataGridViewCellStyle3;
      this.UnitBuyPrice.HeaderText = "Buy";
      this.UnitBuyPrice.Name = "UnitBuyPrice";
      // 
      // TodaysUnitPrice
      // 
      this.TodaysUnitPrice.DataPropertyName = "TodaysUnitPrice";
      dataGridViewCellStyle4.Format = "C2";
      dataGridViewCellStyle4.NullValue = "0";
      this.TodaysUnitPrice.DefaultCellStyle = dataGridViewCellStyle4;
      this.TodaysUnitPrice.HeaderText = "Todays";
      this.TodaysUnitPrice.Name = "TodaysUnitPrice";
      // 
      // PctGain
      // 
      this.PctGain.DataPropertyName = "PctGain";
      dataGridViewCellStyle5.Format = "N2";
      dataGridViewCellStyle5.NullValue = "0";
      this.PctGain.DefaultCellStyle = dataGridViewCellStyle5;
      this.PctGain.HeaderText = "%";
      this.PctGain.Name = "PctGain";
      // 
      // PctYear
      // 
      this.PctYear.DataPropertyName = "PctYear";
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      dataGridViewCellStyle6.Format = "N2";
      dataGridViewCellStyle6.NullValue = "0";
      dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
      this.PctYear.DefaultCellStyle = dataGridViewCellStyle6;
      this.PctYear.HeaderText = "Year %";
      this.PctYear.Name = "PctYear";
      // 
      // FrmSuggestions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(763, 747);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.dgvSuggestions);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmSuggestions";
      this.ShowIcon = false;
      this.Text = "Buy / Sell Suggestions";
      this.Load += new System.EventHandler(this.FrmSuggestions_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvSuggestions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.SuggestionsBindingSource)).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvSuggestions;
    private System.Windows.Forms.BindingSource SuggestionsBindingSource;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonUpdate;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.DataGridViewTextBoxColumn ASXCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn SOH;
    private System.Windows.Forms.DataGridViewTextBoxColumn UnitBuyPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn TodaysUnitPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn PctGain;
    private System.Windows.Forms.DataGridViewTextBoxColumn PctYear;
  }
}