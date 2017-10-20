namespace ShareTrading
{
  partial class FrmGatherStats
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
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
      System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonGenerate = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonChart = new System.Windows.Forms.ToolStripButton();
      this.cbxStatsType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.cbxASXCode = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.chbOnWatchList = new System.Windows.Forms.CheckBox();
      this.toolStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonGenerate,
            this.toolStripButtonClose,
            this.toolStripButtonChart});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new System.Drawing.Size(940, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripButtonGenerate
      // 
      this.toolStripButtonGenerate.Image = global::ShareTrading.Properties.Resources.gear_in;
      this.toolStripButtonGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonGenerate.Name = "toolStripButtonGenerate";
      this.toolStripButtonGenerate.Size = new System.Drawing.Size(74, 22);
      this.toolStripButtonGenerate.Text = "Generate";
      this.toolStripButtonGenerate.Click += new System.EventHandler(this.toolStripButtonGenerate_Click);
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
      // toolStripButtonChart
      // 
      this.toolStripButtonChart.Image = global::ShareTrading.Properties.Resources.chart;
      this.toolStripButtonChart.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonChart.Name = "toolStripButtonChart";
      this.toolStripButtonChart.Size = new System.Drawing.Size(97, 22);
      this.toolStripButtonChart.Text = "Update Chart";
      this.toolStripButtonChart.Click += new System.EventHandler(this.toolStripButtonChart_Click);
      // 
      // cbxStatsType
      // 
      this.cbxStatsType.FormattingEnabled = true;
      this.cbxStatsType.Location = new System.Drawing.Point(32, 81);
      this.cbxStatsType.Margin = new System.Windows.Forms.Padding(4);
      this.cbxStatsType.Name = "cbxStatsType";
      this.cbxStatsType.Size = new System.Drawing.Size(202, 24);
      this.cbxStatsType.TabIndex = 1;
      this.cbxStatsType.SelectedIndexChanged += new System.EventHandler(this.cbxStatsType_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(29, 64);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(111, 16);
      this.label1.TabIndex = 2;
      this.label1.Text = "Statistics Type";
      this.label1.Click += new System.EventHandler(this.label1_Click);
      // 
      // chart1
      // 
      chartArea2.Name = "ChartArea1";
      this.chart1.ChartAreas.Add(chartArea2);
      legend2.Name = "Legend1";
      this.chart1.Legends.Add(legend2);
      this.chart1.Location = new System.Drawing.Point(32, 115);
      this.chart1.Name = "chart1";
      series3.ChartArea = "ChartArea1";
      series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series3.Legend = "Legend1";
      series3.Name = "Overnight";
      series4.ChartArea = "ChartArea1";
      series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series4.Legend = "Legend1";
      series4.Name = "TradeDay";
      this.chart1.Series.Add(series3);
      this.chart1.Series.Add(series4);
      this.chart1.Size = new System.Drawing.Size(845, 342);
      this.chart1.TabIndex = 3;
      this.chart1.Text = "Stats";
      // 
      // cbxASXCode
      // 
      this.cbxASXCode.FormattingEnabled = true;
      this.cbxASXCode.Location = new System.Drawing.Point(273, 81);
      this.cbxASXCode.Name = "cbxASXCode";
      this.cbxASXCode.Size = new System.Drawing.Size(75, 24);
      this.cbxASXCode.TabIndex = 4;
      this.cbxASXCode.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(270, 64);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(78, 16);
      this.label2.TabIndex = 5;
      this.label2.Text = "ASX Code";
      this.label2.Click += new System.EventHandler(this.label2_Click);
      // 
      // chbOnWatchList
      // 
      this.chbOnWatchList.AutoSize = true;
      this.chbOnWatchList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.chbOnWatchList.Location = new System.Drawing.Point(395, 85);
      this.chbOnWatchList.Name = "chbOnWatchList";
      this.chbOnWatchList.Size = new System.Drawing.Size(133, 20);
      this.chbOnWatchList.TabIndex = 8;
      this.chbOnWatchList.Text = "Watchlist Only?";
      this.chbOnWatchList.UseVisualStyleBackColor = true;
      this.chbOnWatchList.CheckedChanged += new System.EventHandler(this.chbOnWatchList_CheckedChanged);
      // 
      // FrmGatherStats
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(940, 551);
      this.Controls.Add(this.chbOnWatchList);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cbxASXCode);
      this.Controls.Add(this.chart1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbxStatsType);
      this.Controls.Add(this.toolStrip1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmGatherStats";
      this.ShowIcon = false;
      this.Text = "Gather Statistics";
      this.Load += new System.EventHandler(this.FrmGatherStats_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonGenerate;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.ComboBox cbxStatsType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ToolStripButton toolStripButtonChart;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    private System.Windows.Forms.ComboBox cbxASXCode;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox chbOnWatchList;
  }
}