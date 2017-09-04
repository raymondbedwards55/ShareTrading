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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dgvToSell = new System.Windows.Forms.DataGridView();
      this.ASXCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SOH = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BuyDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DaysHeld = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.UnitBuyPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TodaysUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.PctGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.PctYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ToSellBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonUpdate = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
      this.tabSells = new System.Windows.Forms.TabControl();
      this.Sells = new System.Windows.Forms.TabPage();
      this.Buys = new System.Windows.Forms.TabPage();
      this.dgvToBuy = new System.Windows.Forms.DataGridView();
      this.ToBuyBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.BuyASXCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BuySOH = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SellDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BuyDaysHeld = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.UnitSellPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BuyTodaysUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BuyPctGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BuyPctYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvToSell)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ToSellBindingSource)).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.tabSells.SuspendLayout();
      this.Sells.SuspendLayout();
      this.Buys.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvToBuy)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ToBuyBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvToSell
      // 
      this.dgvToSell.AllowUserToAddRows = false;
      this.dgvToSell.AllowUserToDeleteRows = false;
      this.dgvToSell.AutoGenerateColumns = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvToSell.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvToSell.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvToSell.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ASXCode,
            this.SOH,
            this.BuyDate,
            this.DaysHeld,
            this.UnitBuyPrice,
            this.TodaysUnitPrice,
            this.PctGain,
            this.PctYear});
      this.dgvToSell.DataSource = this.ToSellBindingSource;
      this.dgvToSell.Location = new System.Drawing.Point(6, 7);
      this.dgvToSell.Name = "dgvToSell";
      this.dgvToSell.Size = new System.Drawing.Size(822, 656);
      this.dgvToSell.TabIndex = 0;
      this.dgvToSell.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvToSell_CellFormatting);
      this.dgvToSell.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvToSell_ColumnHeaderMouseClick);
      // 
      // ASXCode
      // 
      this.ASXCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.ASXCode.DataPropertyName = "ASXCode";
      this.ASXCode.HeaderText = "ASX Code";
      this.ASXCode.Name = "ASXCode";
      this.ASXCode.ReadOnly = true;
      // 
      // SOH
      // 
      this.SOH.DataPropertyName = "SOH";
      dataGridViewCellStyle2.Format = "N0";
      dataGridViewCellStyle2.NullValue = "0";
      this.SOH.DefaultCellStyle = dataGridViewCellStyle2;
      this.SOH.HeaderText = "SOH";
      this.SOH.Name = "SOH";
      this.SOH.ReadOnly = true;
      // 
      // BuyDate
      // 
      this.BuyDate.DataPropertyName = "BuyDate";
      dataGridViewCellStyle3.Format = "d";
      dataGridViewCellStyle3.NullValue = null;
      this.BuyDate.DefaultCellStyle = dataGridViewCellStyle3;
      this.BuyDate.HeaderText = "Buy Date";
      this.BuyDate.Name = "BuyDate";
      this.BuyDate.ReadOnly = true;
      // 
      // DaysHeld
      // 
      this.DaysHeld.DataPropertyName = "DaysHeld";
      dataGridViewCellStyle4.Format = "N2";
      dataGridViewCellStyle4.NullValue = "0";
      this.DaysHeld.DefaultCellStyle = dataGridViewCellStyle4;
      this.DaysHeld.HeaderText = "Days Held";
      this.DaysHeld.Name = "DaysHeld";
      this.DaysHeld.ReadOnly = true;
      // 
      // UnitBuyPrice
      // 
      this.UnitBuyPrice.DataPropertyName = "UnitBuyPrice";
      dataGridViewCellStyle5.Format = "C2";
      dataGridViewCellStyle5.NullValue = "0";
      this.UnitBuyPrice.DefaultCellStyle = dataGridViewCellStyle5;
      this.UnitBuyPrice.HeaderText = "Unit Buy Price";
      this.UnitBuyPrice.Name = "UnitBuyPrice";
      this.UnitBuyPrice.ReadOnly = true;
      // 
      // TodaysUnitPrice
      // 
      this.TodaysUnitPrice.DataPropertyName = "TodaysUnitPrice";
      dataGridViewCellStyle6.Format = "C2";
      dataGridViewCellStyle6.NullValue = "0";
      this.TodaysUnitPrice.DefaultCellStyle = dataGridViewCellStyle6;
      this.TodaysUnitPrice.HeaderText = "Todays Unit Price";
      this.TodaysUnitPrice.Name = "TodaysUnitPrice";
      this.TodaysUnitPrice.ReadOnly = true;
      // 
      // PctGain
      // 
      this.PctGain.DataPropertyName = "PctGain";
      dataGridViewCellStyle7.Format = "N2";
      dataGridViewCellStyle7.NullValue = "0";
      this.PctGain.DefaultCellStyle = dataGridViewCellStyle7;
      this.PctGain.HeaderText = "%";
      this.PctGain.Name = "PctGain";
      this.PctGain.ReadOnly = true;
      // 
      // PctYear
      // 
      this.PctYear.DataPropertyName = "PctYear";
      dataGridViewCellStyle8.Format = "N2";
      dataGridViewCellStyle8.NullValue = "0";
      this.PctYear.DefaultCellStyle = dataGridViewCellStyle8;
      this.PctYear.HeaderText = "% Year";
      this.PctYear.Name = "PctYear";
      this.PctYear.ReadOnly = true;
      // 
      // ToSellBindingSource
      // 
      this.ToSellBindingSource.AllowNew = false;
      this.ToSellBindingSource.DataSource = typeof(ShareTrading.FrmSuggestions.SellSuggestions);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonUpdate,
            this.toolStripButtonClose});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1661, 25);
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
      // tabSells
      // 
      this.tabSells.Controls.Add(this.Sells);
      this.tabSells.Controls.Add(this.Buys);
      this.tabSells.Location = new System.Drawing.Point(789, 28);
      this.tabSells.Name = "tabSells";
      this.tabSells.SelectedIndex = 0;
      this.tabSells.Size = new System.Drawing.Size(842, 695);
      this.tabSells.TabIndex = 2;
      // 
      // Sells
      // 
      this.Sells.Controls.Add(this.dgvToSell);
      this.Sells.Location = new System.Drawing.Point(4, 22);
      this.Sells.Name = "Sells";
      this.Sells.Padding = new System.Windows.Forms.Padding(3);
      this.Sells.Size = new System.Drawing.Size(834, 669);
      this.Sells.TabIndex = 0;
      this.Sells.Text = "Sells";
      this.Sells.UseVisualStyleBackColor = true;
      // 
      // Buys
      // 
      this.Buys.Controls.Add(this.dgvToBuy);
      this.Buys.Location = new System.Drawing.Point(4, 22);
      this.Buys.Name = "Buys";
      this.Buys.Padding = new System.Windows.Forms.Padding(3);
      this.Buys.Size = new System.Drawing.Size(834, 669);
      this.Buys.TabIndex = 1;
      this.Buys.Text = "Buys";
      this.Buys.UseVisualStyleBackColor = true;
      // 
      // dgvToBuy
      // 
      this.dgvToBuy.AutoGenerateColumns = false;
      this.dgvToBuy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvToBuy.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BuyASXCode,
            this.BuySOH,
            this.SellDate,
            this.BuyDaysHeld,
            this.UnitSellPrice,
            this.BuyTodaysUnitPrice,
            this.BuyPctGain,
            this.BuyPctYear});
      this.dgvToBuy.DataSource = this.ToBuyBindingSource;
      this.dgvToBuy.Location = new System.Drawing.Point(6, 6);
      this.dgvToBuy.Name = "dgvToBuy";
      this.dgvToBuy.Size = new System.Drawing.Size(825, 660);
      this.dgvToBuy.TabIndex = 0;
      this.dgvToBuy.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvToBuy_CellFormatting);
      this.dgvToBuy.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvToBuy_ColumnHeaderMouseClick);
      // 
      // ToBuyBindingSource
      // 
      this.ToBuyBindingSource.AllowNew = false;
      this.ToBuyBindingSource.DataSource = typeof(ShareTrading.FrmSuggestions.BuySuggestions);
      // 
      // BuyASXCode
      // 
      this.BuyASXCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.BuyASXCode.DataPropertyName = "BuyASXCode";
      this.BuyASXCode.HeaderText = "ASX Code";
      this.BuyASXCode.Name = "BuyASXCode";
      this.BuyASXCode.ReadOnly = true;
      // 
      // BuySOH
      // 
      this.BuySOH.DataPropertyName = "BuySOH";
      dataGridViewCellStyle9.Format = "N0";
      dataGridViewCellStyle9.NullValue = "0";
      this.BuySOH.DefaultCellStyle = dataGridViewCellStyle9;
      this.BuySOH.HeaderText = "Qty Sold";
      this.BuySOH.Name = "BuySOH";
      this.BuySOH.ReadOnly = true;
      // 
      // SellDate
      // 
      this.SellDate.DataPropertyName = "SellDate";
      dataGridViewCellStyle10.Format = "d";
      dataGridViewCellStyle10.NullValue = null;
      this.SellDate.DefaultCellStyle = dataGridViewCellStyle10;
      this.SellDate.HeaderText = "Date Sold";
      this.SellDate.Name = "SellDate";
      // 
      // BuyDaysHeld
      // 
      this.BuyDaysHeld.DataPropertyName = "BuyDaysHeld";
      dataGridViewCellStyle11.Format = "N0";
      dataGridViewCellStyle11.NullValue = "0";
      this.BuyDaysHeld.DefaultCellStyle = dataGridViewCellStyle11;
      this.BuyDaysHeld.HeaderText = "Days Held";
      this.BuyDaysHeld.Name = "BuyDaysHeld";
      this.BuyDaysHeld.ReadOnly = true;
      // 
      // UnitSellPrice
      // 
      this.UnitSellPrice.DataPropertyName = "UnitSellPrice";
      dataGridViewCellStyle12.Format = "C2";
      dataGridViewCellStyle12.NullValue = "0";
      this.UnitSellPrice.DefaultCellStyle = dataGridViewCellStyle12;
      this.UnitSellPrice.HeaderText = "Unit Sell Price";
      this.UnitSellPrice.Name = "UnitSellPrice";
      this.UnitSellPrice.ReadOnly = true;
      // 
      // BuyTodaysUnitPrice
      // 
      this.BuyTodaysUnitPrice.DataPropertyName = "BuyTodaysUnitPrice";
      dataGridViewCellStyle13.Format = "C2";
      dataGridViewCellStyle13.NullValue = "0";
      this.BuyTodaysUnitPrice.DefaultCellStyle = dataGridViewCellStyle13;
      this.BuyTodaysUnitPrice.HeaderText = "Todays Unit Price";
      this.BuyTodaysUnitPrice.Name = "BuyTodaysUnitPrice";
      this.BuyTodaysUnitPrice.ReadOnly = true;
      // 
      // BuyPctGain
      // 
      this.BuyPctGain.DataPropertyName = "BuyPctGain";
      dataGridViewCellStyle14.Format = "N2";
      dataGridViewCellStyle14.NullValue = "0";
      this.BuyPctGain.DefaultCellStyle = dataGridViewCellStyle14;
      this.BuyPctGain.HeaderText = "%";
      this.BuyPctGain.Name = "BuyPctGain";
      // 
      // BuyPctYear
      // 
      this.BuyPctYear.DataPropertyName = "BuyPctYear";
      dataGridViewCellStyle15.Format = "N2";
      dataGridViewCellStyle15.NullValue = "0";
      this.BuyPctYear.DefaultCellStyle = dataGridViewCellStyle15;
      this.BuyPctYear.HeaderText = "% Year";
      this.BuyPctYear.Name = "BuyPctYear";
      // 
      // FrmSuggestions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1661, 747);
      this.Controls.Add(this.tabSells);
      this.Controls.Add(this.toolStrip1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmSuggestions";
      this.ShowIcon = false;
      this.Text = "Buy / Sell Suggestions";
      this.Load += new System.EventHandler(this.FrmSuggestions_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvToSell)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ToSellBindingSource)).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.tabSells.ResumeLayout(false);
      this.Sells.ResumeLayout(false);
      this.Buys.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgvToBuy)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ToBuyBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvToSell;
    private System.Windows.Forms.BindingSource ToSellBindingSource;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonUpdate;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.TabControl tabSells;
    private System.Windows.Forms.TabPage Sells;
    private System.Windows.Forms.TabPage Buys;
    private System.Windows.Forms.DataGridViewTextBoxColumn ASXCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn SOH;
    private System.Windows.Forms.DataGridViewTextBoxColumn BuyDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn DaysHeld;
    private System.Windows.Forms.DataGridViewTextBoxColumn UnitBuyPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn TodaysUnitPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn PctGain;
    private System.Windows.Forms.DataGridViewTextBoxColumn PctYear;
    private System.Windows.Forms.DataGridView dgvToBuy;
    private System.Windows.Forms.BindingSource ToBuyBindingSource;
    private System.Windows.Forms.DataGridViewTextBoxColumn BuyASXCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn BuySOH;
    private System.Windows.Forms.DataGridViewTextBoxColumn SellDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn BuyDaysHeld;
    private System.Windows.Forms.DataGridViewTextBoxColumn UnitSellPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn BuyTodaysUnitPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn BuyPctGain;
    private System.Windows.Forms.DataGridViewTextBoxColumn BuyPctYear;
  }
}