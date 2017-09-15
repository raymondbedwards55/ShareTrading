namespace ShareTrading
{
  partial class FrmEnterSellConfrmationNr
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
      this.dgvBuyTransactions = new System.Windows.Forms.DataGridView();
      this.transRecordsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.BuyTransactionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
      this.tbxASXCode = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ASXCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TranDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.buySellDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TransQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.UnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.brokerageIncDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.gSTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SOH = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.tradeProfitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.relatedTransactionIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.daysHeldDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.nABOrderNmbrDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.transTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dateCreatedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dateModifiedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dateDeletedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SellConfirmation = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvBuyTransactions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.transRecordsBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.BuyTransactionsBindingSource)).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dgvBuyTransactions
      // 
      this.dgvBuyTransactions.AllowUserToAddRows = false;
      this.dgvBuyTransactions.AllowUserToDeleteRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      this.dgvBuyTransactions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvBuyTransactions.AutoGenerateColumns = false;
      this.dgvBuyTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBuyTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.ASXCode,
            this.TranDate,
            this.buySellDataGridViewTextBoxColumn,
            this.TransQty,
            this.UnitPrice,
            this.brokerageIncDataGridViewTextBoxColumn,
            this.gSTDataGridViewTextBoxColumn,
            this.SOH,
            this.tradeProfitDataGridViewTextBoxColumn,
            this.relatedTransactionIDDataGridViewTextBoxColumn,
            this.daysHeldDataGridViewTextBoxColumn,
            this.nABOrderNmbrDataGridViewTextBoxColumn,
            this.transTypeDataGridViewTextBoxColumn,
            this.dateCreatedDataGridViewTextBoxColumn,
            this.dateModifiedDataGridViewTextBoxColumn,
            this.dateDeletedDataGridViewTextBoxColumn,
            this.SellConfirmation});
      this.dgvBuyTransactions.DataSource = this.transRecordsBindingSource;
      this.dgvBuyTransactions.Location = new System.Drawing.Point(108, 149);
      this.dgvBuyTransactions.Margin = new System.Windows.Forms.Padding(4);
      this.dgvBuyTransactions.Name = "dgvBuyTransactions";
      this.dgvBuyTransactions.Size = new System.Drawing.Size(642, 297);
      this.dgvBuyTransactions.TabIndex = 0;
      this.dgvBuyTransactions.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBuyTransactions_CellEnter);
      this.dgvBuyTransactions.Enter += new System.EventHandler(this.dgvBuyTransactions_Enter);
      this.dgvBuyTransactions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvBuyTransactions_KeyDown);
      // 
      // transRecordsBindingSource
      // 
      this.transRecordsBindingSource.DataSource = typeof(ShareTrading.DBAccess.TransRecords);
      // 
      // BuyTransactionsBindingSource
      // 
      this.BuyTransactionsBindingSource.DataSource = typeof(ShareTrading.DBAccess.TransRecords);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonSave,
            this.toolStripButtonCancel,
            this.toolStripSeparator1,
            this.toolStripButtonClose});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new System.Drawing.Size(855, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripButtonNew
      // 
      this.toolStripButtonNew.Image = global::ShareTrading.Properties.Resources.AddTableHS;
      this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonNew.Name = "toolStripButtonNew";
      this.toolStripButtonNew.Size = new System.Drawing.Size(51, 22);
      this.toolStripButtonNew.Text = "New";
      this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
      // 
      // toolStripButtonSave
      // 
      this.toolStripButtonSave.Image = global::ShareTrading.Properties.Resources.saveHS;
      this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonSave.Name = "toolStripButtonSave";
      this.toolStripButtonSave.Size = new System.Drawing.Size(51, 22);
      this.toolStripButtonSave.Text = "Save";
      this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
      // 
      // toolStripButtonCancel
      // 
      this.toolStripButtonCancel.Image = global::ShareTrading.Properties.Resources.cancel;
      this.toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonCancel.Name = "toolStripButtonCancel";
      this.toolStripButtonCancel.Size = new System.Drawing.Size(63, 22);
      this.toolStripButtonCancel.Text = "Cancel";
      this.toolStripButtonCancel.Click += new System.EventHandler(this.toolStripButtonCancel_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
      // tbxASXCode
      // 
      this.tbxASXCode.Location = new System.Drawing.Point(108, 102);
      this.tbxASXCode.Margin = new System.Windows.Forms.Padding(4);
      this.tbxASXCode.Name = "tbxASXCode";
      this.tbxASXCode.Size = new System.Drawing.Size(211, 22);
      this.tbxASXCode.TabIndex = 2;
      this.tbxASXCode.Leave += new System.EventHandler(this.tbxASXCode_Leave);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(104, 82);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(122, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Enter ASX Code:";
      // 
      // iDDataGridViewTextBoxColumn
      // 
      this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
      this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
      this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
      this.iDDataGridViewTextBoxColumn.Visible = false;
      // 
      // ASXCode
      // 
      this.ASXCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.ASXCode.DataPropertyName = "ASXCode";
      this.ASXCode.HeaderText = "ASX Code";
      this.ASXCode.Name = "ASXCode";
      this.ASXCode.ReadOnly = true;
      // 
      // TranDate
      // 
      this.TranDate.DataPropertyName = "TranDate";
      dataGridViewCellStyle2.Format = "d";
      dataGridViewCellStyle2.NullValue = null;
      this.TranDate.DefaultCellStyle = dataGridViewCellStyle2;
      this.TranDate.HeaderText = "Transaction Date";
      this.TranDate.Name = "TranDate";
      this.TranDate.ReadOnly = true;
      // 
      // buySellDataGridViewTextBoxColumn
      // 
      this.buySellDataGridViewTextBoxColumn.DataPropertyName = "BuySell";
      this.buySellDataGridViewTextBoxColumn.HeaderText = "BuySell";
      this.buySellDataGridViewTextBoxColumn.Name = "buySellDataGridViewTextBoxColumn";
      this.buySellDataGridViewTextBoxColumn.Visible = false;
      // 
      // TransQty
      // 
      this.TransQty.DataPropertyName = "TransQty";
      dataGridViewCellStyle3.Format = "N0";
      dataGridViewCellStyle3.NullValue = "0";
      this.TransQty.DefaultCellStyle = dataGridViewCellStyle3;
      this.TransQty.HeaderText = "Qty Bought";
      this.TransQty.Name = "TransQty";
      this.TransQty.ReadOnly = true;
      // 
      // UnitPrice
      // 
      this.UnitPrice.DataPropertyName = "UnitPrice";
      dataGridViewCellStyle4.Format = "C2";
      dataGridViewCellStyle4.NullValue = "0";
      this.UnitPrice.DefaultCellStyle = dataGridViewCellStyle4;
      this.UnitPrice.HeaderText = "Unit Price";
      this.UnitPrice.Name = "UnitPrice";
      this.UnitPrice.ReadOnly = true;
      // 
      // brokerageIncDataGridViewTextBoxColumn
      // 
      this.brokerageIncDataGridViewTextBoxColumn.DataPropertyName = "BrokerageInc";
      this.brokerageIncDataGridViewTextBoxColumn.HeaderText = "BrokerageInc";
      this.brokerageIncDataGridViewTextBoxColumn.Name = "brokerageIncDataGridViewTextBoxColumn";
      this.brokerageIncDataGridViewTextBoxColumn.Visible = false;
      // 
      // gSTDataGridViewTextBoxColumn
      // 
      this.gSTDataGridViewTextBoxColumn.DataPropertyName = "GST";
      this.gSTDataGridViewTextBoxColumn.HeaderText = "GST";
      this.gSTDataGridViewTextBoxColumn.Name = "gSTDataGridViewTextBoxColumn";
      this.gSTDataGridViewTextBoxColumn.Visible = false;
      // 
      // SOH
      // 
      this.SOH.DataPropertyName = "SOH";
      this.SOH.HeaderText = "SOH";
      this.SOH.Name = "SOH";
      this.SOH.ReadOnly = true;
      // 
      // tradeProfitDataGridViewTextBoxColumn
      // 
      this.tradeProfitDataGridViewTextBoxColumn.DataPropertyName = "TradeProfit";
      this.tradeProfitDataGridViewTextBoxColumn.HeaderText = "TradeProfit";
      this.tradeProfitDataGridViewTextBoxColumn.Name = "tradeProfitDataGridViewTextBoxColumn";
      this.tradeProfitDataGridViewTextBoxColumn.Visible = false;
      // 
      // relatedTransactionIDDataGridViewTextBoxColumn
      // 
      this.relatedTransactionIDDataGridViewTextBoxColumn.DataPropertyName = "RelatedTransactionID";
      this.relatedTransactionIDDataGridViewTextBoxColumn.HeaderText = "RelatedTransactionID";
      this.relatedTransactionIDDataGridViewTextBoxColumn.Name = "relatedTransactionIDDataGridViewTextBoxColumn";
      this.relatedTransactionIDDataGridViewTextBoxColumn.Visible = false;
      // 
      // daysHeldDataGridViewTextBoxColumn
      // 
      this.daysHeldDataGridViewTextBoxColumn.DataPropertyName = "DaysHeld";
      this.daysHeldDataGridViewTextBoxColumn.HeaderText = "DaysHeld";
      this.daysHeldDataGridViewTextBoxColumn.Name = "daysHeldDataGridViewTextBoxColumn";
      this.daysHeldDataGridViewTextBoxColumn.Visible = false;
      // 
      // nABOrderNmbrDataGridViewTextBoxColumn
      // 
      this.nABOrderNmbrDataGridViewTextBoxColumn.DataPropertyName = "NABOrderNmbr";
      this.nABOrderNmbrDataGridViewTextBoxColumn.HeaderText = "NABOrderNmbr";
      this.nABOrderNmbrDataGridViewTextBoxColumn.Name = "nABOrderNmbrDataGridViewTextBoxColumn";
      this.nABOrderNmbrDataGridViewTextBoxColumn.Visible = false;
      // 
      // transTypeDataGridViewTextBoxColumn
      // 
      this.transTypeDataGridViewTextBoxColumn.DataPropertyName = "TransType";
      this.transTypeDataGridViewTextBoxColumn.HeaderText = "TransType";
      this.transTypeDataGridViewTextBoxColumn.Name = "transTypeDataGridViewTextBoxColumn";
      this.transTypeDataGridViewTextBoxColumn.Visible = false;
      // 
      // dateCreatedDataGridViewTextBoxColumn
      // 
      this.dateCreatedDataGridViewTextBoxColumn.DataPropertyName = "DateCreated";
      this.dateCreatedDataGridViewTextBoxColumn.HeaderText = "DateCreated";
      this.dateCreatedDataGridViewTextBoxColumn.Name = "dateCreatedDataGridViewTextBoxColumn";
      this.dateCreatedDataGridViewTextBoxColumn.Visible = false;
      // 
      // dateModifiedDataGridViewTextBoxColumn
      // 
      this.dateModifiedDataGridViewTextBoxColumn.DataPropertyName = "DateModified";
      this.dateModifiedDataGridViewTextBoxColumn.HeaderText = "DateModified";
      this.dateModifiedDataGridViewTextBoxColumn.Name = "dateModifiedDataGridViewTextBoxColumn";
      this.dateModifiedDataGridViewTextBoxColumn.Visible = false;
      // 
      // dateDeletedDataGridViewTextBoxColumn
      // 
      this.dateDeletedDataGridViewTextBoxColumn.DataPropertyName = "DateDeleted";
      this.dateDeletedDataGridViewTextBoxColumn.HeaderText = "DateDeleted";
      this.dateDeletedDataGridViewTextBoxColumn.Name = "dateDeletedDataGridViewTextBoxColumn";
      this.dateDeletedDataGridViewTextBoxColumn.Visible = false;
      // 
      // SellConfirmation
      // 
      this.SellConfirmation.DataPropertyName = "SellConfirmation";
      this.SellConfirmation.HeaderText = "Match Confirmation Number";
      this.SellConfirmation.Name = "SellConfirmation";
      // 
      // FrmEnterSellConfrmationNr
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(855, 537);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tbxASXCode);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.dgvBuyTransactions);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmEnterSellConfrmationNr";
      this.ShowIcon = false;
      this.Text = "Enter Sell Confrmation Nr";
      this.Load += new System.EventHandler(this.FrmEnterSellConfrmationNr_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvBuyTransactions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.transRecordsBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.BuyTransactionsBindingSource)).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvBuyTransactions;
    private System.Windows.Forms.BindingSource BuyTransactionsBindingSource;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonNew;
    private System.Windows.Forms.ToolStripButton toolStripButtonSave;
    private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.TextBox tbxASXCode;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.BindingSource transRecordsBindingSource;
    private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn ASXCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn TranDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn buySellDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn TransQty;
    private System.Windows.Forms.DataGridViewTextBoxColumn UnitPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn brokerageIncDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn gSTDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn SOH;
    private System.Windows.Forms.DataGridViewTextBoxColumn tradeProfitDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn relatedTransactionIDDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn daysHeldDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn nABOrderNmbrDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn transTypeDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn dateCreatedDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn dateModifiedDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn dateDeletedDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn SellConfirmation;
  }
}