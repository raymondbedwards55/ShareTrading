namespace ShareTrading
{
  partial class ExportBuysSellsToMYOB
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
      this.dtpFrom = new System.Windows.Forms.DateTimePicker();
      this.dtpTo = new System.Windows.Forms.DateTimePicker();
      this.rbBuy = new System.Windows.Forms.RadioButton();
      this.rbSell = new System.Windows.Forms.RadioButton();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButton_Export = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
      this.rbDividend = new System.Windows.Forms.RadioButton();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dtpFrom
      // 
      this.dtpFrom.Location = new System.Drawing.Point(74, 178);
      this.dtpFrom.Name = "dtpFrom";
      this.dtpFrom.Size = new System.Drawing.Size(246, 22);
      this.dtpFrom.TabIndex = 0;
      this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
      // 
      // dtpTo
      // 
      this.dtpTo.Location = new System.Drawing.Point(74, 223);
      this.dtpTo.Name = "dtpTo";
      this.dtpTo.Size = new System.Drawing.Size(246, 22);
      this.dtpTo.TabIndex = 1;
      this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
      // 
      // rbBuy
      // 
      this.rbBuy.AutoSize = true;
      this.rbBuy.Location = new System.Drawing.Point(62, 45);
      this.rbBuy.Name = "rbBuy";
      this.rbBuy.Size = new System.Drawing.Size(52, 20);
      this.rbBuy.TabIndex = 2;
      this.rbBuy.Text = "Buy";
      this.rbBuy.UseVisualStyleBackColor = true;
      this.rbBuy.CheckedChanged += new System.EventHandler(this.rbBuySell_CheckedChanged);
      // 
      // rbSell
      // 
      this.rbSell.AutoSize = true;
      this.rbSell.Checked = true;
      this.rbSell.Location = new System.Drawing.Point(62, 23);
      this.rbSell.Name = "rbSell";
      this.rbSell.Size = new System.Drawing.Size(53, 20);
      this.rbSell.TabIndex = 3;
      this.rbSell.TabStop = true;
      this.rbSell.Text = "Sell";
      this.rbSell.UseVisualStyleBackColor = true;
      this.rbSell.CheckedChanged += new System.EventHandler(this.rbBuySell_CheckedChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.rbDividend);
      this.groupBox1.Controls.Add(this.rbSell);
      this.groupBox1.Controls.Add(this.rbBuy);
      this.groupBox1.Location = new System.Drawing.Point(38, 37);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(200, 100);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Buy or Sell?";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.dtpFrom);
      this.groupBox2.Controls.Add(this.groupBox1);
      this.groupBox2.Controls.Add(this.dtpTo);
      this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox2.Location = new System.Drawing.Point(48, 35);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(360, 311);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Export Transactions";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(25, 228);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(27, 16);
      this.label2.TabIndex = 6;
      this.label2.Text = "To";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(25, 183);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 16);
      this.label1.TabIndex = 5;
      this.label1.Text = "From";
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Export,
            this.toolStripButtonClose});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(448, 25);
      this.toolStrip1.TabIndex = 6;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripButton_Export
      // 
      this.toolStripButton_Export.Image = global::ShareTrading.Properties.Resources.gear_in;
      this.toolStripButton_Export.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton_Export.Name = "toolStripButton_Export";
      this.toolStripButton_Export.Size = new System.Drawing.Size(60, 22);
      this.toolStripButton_Export.Text = "Export";
      this.toolStripButton_Export.Click += new System.EventHandler(this.toolStripButton_Export_Click);
      // 
      // toolStripButtonClose
      // 
      this.toolStripButtonClose.Image = global::ShareTrading.Properties.Resources.door_out;
      this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonClose.Name = "toolStripButtonClose";
      this.toolStripButtonClose.Size = new System.Drawing.Size(56, 22);
      this.toolStripButtonClose.Text = "Close";
      this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButton_Close_Click);
      // 
      // rbDividend
      // 
      this.rbDividend.AutoSize = true;
      this.rbDividend.Location = new System.Drawing.Point(62, 67);
      this.rbDividend.Name = "rbDividend";
      this.rbDividend.Size = new System.Drawing.Size(96, 20);
      this.rbDividend.TabIndex = 4;
      this.rbDividend.Text = "Dividends";
      this.rbDividend.UseVisualStyleBackColor = true;
      // 
      // ExportBuysSellsToMYOB
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(448, 396);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.groupBox2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExportBuysSellsToMYOB";
      this.ShowIcon = false;
      this.Text = "Export Buys/Sells To MYOB";
      this.Load += new System.EventHandler(this.ExportBuysSellsToMYOB_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.RadioButton rbBuy;
    private System.Windows.Forms.RadioButton rbSell;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButton_Export;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.RadioButton rbDividend;
  }
}