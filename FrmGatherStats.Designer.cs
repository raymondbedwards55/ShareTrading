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
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
      System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
      System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
      this.dtpFrom = new System.Windows.Forms.DateTimePicker();
      this.label3 = new System.Windows.Forms.Label();
      this.dtpTo = new System.Windows.Forms.DateTimePicker();
      this.label4 = new System.Windows.Forms.Label();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.statusLabel = new System.Windows.Forms.Label();
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
      this.toolStrip1.Size = new System.Drawing.Size(1186, 25);
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
      this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      chartArea1.Name = "ChartArea1";
      this.chart1.ChartAreas.Add(chartArea1);
      legend1.Name = "Legend1";
      this.chart1.Legends.Add(legend1);
      this.chart1.Location = new System.Drawing.Point(32, 115);
      this.chart1.Name = "chart1";
      series1.ChartArea = "ChartArea1";
      series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series1.Legend = "Legend1";
      series1.Name = "Overnight";
      series2.ChartArea = "ChartArea1";
      series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series2.Legend = "Legend1";
      series2.Name = "TradeDay";
      series3.ChartArea = "ChartArea1";
      series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series3.Legend = "Legend1";
      series3.Name = "Price";
      this.chart1.Series.Add(series1);
      this.chart1.Series.Add(series2);
      this.chart1.Series.Add(series3);
      this.chart1.Size = new System.Drawing.Size(1104, 563);
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
      this.chbOnWatchList.Location = new System.Drawing.Point(721, 87);
      this.chbOnWatchList.Name = "chbOnWatchList";
      this.chbOnWatchList.Size = new System.Drawing.Size(156, 20);
      this.chbOnWatchList.TabIndex = 8;
      this.chbOnWatchList.Text = "On Watchlist Only?";
      this.chbOnWatchList.UseVisualStyleBackColor = true;
      this.chbOnWatchList.CheckedChanged += new System.EventHandler(this.chbOnWatchList_CheckedChanged);
      // 
      // dtpFrom
      // 
      this.dtpFrom.Checked = false;
      this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtpFrom.Location = new System.Drawing.Point(412, 83);
      this.dtpFrom.Name = "dtpFrom";
      this.dtpFrom.Size = new System.Drawing.Size(117, 22);
      this.dtpFrom.TabIndex = 9;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(409, 64);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(67, 16);
      this.label3.TabIndex = 10;
      this.label3.Text = "Between";
      this.label3.Click += new System.EventHandler(this.label3_Click);
      // 
      // dtpTo
      // 
      this.dtpTo.Checked = false;
      this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtpTo.Location = new System.Drawing.Point(565, 83);
      this.dtpTo.Name = "dtpTo";
      this.dtpTo.Size = new System.Drawing.Size(117, 22);
      this.dtpTo.TabIndex = 11;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(540, 84);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(18, 16);
      this.label4.TabIndex = 12;
      this.label4.Text = "&&";
      // 
      // backgroundWorker1
      // 
      this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
      this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      // 
      // progressBar
      // 
      this.progressBar.Location = new System.Drawing.Point(949, 674);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(237, 34);
      this.progressBar.TabIndex = 49;
      // 
      // statusLabel
      // 
      this.statusLabel.AutoSize = true;
      this.statusLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.statusLabel.Location = new System.Drawing.Point(973, 684);
      this.statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new System.Drawing.Size(100, 16);
      this.statusLabel.TabIndex = 50;
      this.statusLabel.Text = "Generating ...";
      // 
      // FrmGatherStats
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1186, 709);
      this.Controls.Add(this.statusLabel);
      this.Controls.Add(this.progressBar);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.dtpTo);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.dtpFrom);
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
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.Label label4;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.Label statusLabel;
  }
}