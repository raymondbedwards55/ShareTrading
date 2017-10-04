namespace ShareTrading
{
    partial class Form1
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
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
      this.button5 = new System.Windows.Forms.Button();
      this.dataGridView2 = new System.Windows.Forms.DataGridView();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.TbProfit = new System.Windows.Forms.TextBox();
      this.lblProfit = new System.Windows.Forms.Label();
      this.lblSOH = new System.Windows.Forms.Label();
      this.TbSOH = new System.Windows.Forms.TextBox();
      this.importRecentPricesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.importRecentPricesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importYahooDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem_importDivHistory = new System.Windows.Forms.ToolStripMenuItem();
      this.importNABPricesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importNABTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importNABDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.suggestionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.BtnImportTransactions = new System.Windows.Forms.Button();
      this.lblSuggested = new System.Windows.Forms.Label();
      this.DgvSuggestedBuys = new System.Windows.Forms.DataGridView();
      this.BtnSuggestedSells = new System.Windows.Forms.Label();
      this.BtnGenerateSuggestions = new System.Windows.Forms.Button();
      this.button6 = new System.Windows.Forms.Button();
      this.DisplayGrid = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.TestTran = new System.Windows.Forms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.Edits = new System.Windows.Forms.ToolStripMenuItem();
      this.GLCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toMYOBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.DgvSuggestedSells = new System.Windows.Forms.DataGridView();
      this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.CSVopenFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.shareAnalV2DataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.enterConfirmationNrToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.gatherStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.DgvSuggestedBuys)).BeginInit();
      this.menuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DgvSuggestedSells)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.shareAnalV2DataSetBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(682, 316);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(75, 23);
      this.button5.TabIndex = 46;
      this.button5.Text = "button5";
      this.button5.UseVisualStyleBackColor = true;
      // 
      // dataGridView2
      // 
      this.dataGridView2.AllowUserToAddRows = false;
      this.dataGridView2.AllowUserToDeleteRows = false;
      this.dataGridView2.AllowUserToOrderColumns = true;
      this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView2.Location = new System.Drawing.Point(912, 407);
      this.dataGridView2.Name = "dataGridView2";
      this.dataGridView2.Size = new System.Drawing.Size(303, 216);
      this.dataGridView2.TabIndex = 45;
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AllowUserToOrderColumns = true;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Location = new System.Drawing.Point(912, 175);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.Size = new System.Drawing.Size(303, 216);
      this.dataGridView1.TabIndex = 44;
      // 
      // timer1
      // 
      this.timer1.Enabled = true;
      this.timer1.Interval = 3000;
      // 
      // TbProfit
      // 
      this.TbProfit.Location = new System.Drawing.Point(10, 142);
      this.TbProfit.Name = "TbProfit";
      this.TbProfit.Size = new System.Drawing.Size(100, 20);
      this.TbProfit.TabIndex = 43;
      // 
      // lblProfit
      // 
      this.lblProfit.AutoSize = true;
      this.lblProfit.Location = new System.Drawing.Point(-83, 148);
      this.lblProfit.Name = "lblProfit";
      this.lblProfit.Size = new System.Drawing.Size(31, 13);
      this.lblProfit.TabIndex = 42;
      this.lblProfit.Text = "Profit";
      // 
      // lblSOH
      // 
      this.lblSOH.AutoSize = true;
      this.lblSOH.Location = new System.Drawing.Point(-400, 148);
      this.lblSOH.Name = "lblSOH";
      this.lblSOH.Size = new System.Drawing.Size(30, 13);
      this.lblSOH.TabIndex = 41;
      this.lblSOH.Text = "SOH";
      // 
      // TbSOH
      // 
      this.TbSOH.Location = new System.Drawing.Point(-199, 142);
      this.TbSOH.Name = "TbSOH";
      this.TbSOH.Size = new System.Drawing.Size(100, 20);
      this.TbSOH.TabIndex = 40;
      // 
      // importRecentPricesToolStripMenuItem1
      // 
      this.importRecentPricesToolStripMenuItem1.Name = "importRecentPricesToolStripMenuItem1";
      this.importRecentPricesToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
      this.importRecentPricesToolStripMenuItem1.Text = "Import Recent Prices";
      this.importRecentPricesToolStripMenuItem1.Click += new System.EventHandler(this.importRecentPricesToolStripMenuItem1_Click);
      // 
      // importRecentPricesToolStripMenuItem
      // 
      this.importRecentPricesToolStripMenuItem.Name = "importRecentPricesToolStripMenuItem";
      this.importRecentPricesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
      this.importRecentPricesToolStripMenuItem.Text = "Import TodaysPrices";
      // 
      // importYahooDataToolStripMenuItem
      // 
      this.importYahooDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importRecentPricesToolStripMenuItem,
            this.importRecentPricesToolStripMenuItem1,
            this.toolStripMenuItem_importDivHistory});
      this.importYahooDataToolStripMenuItem.Name = "importYahooDataToolStripMenuItem";
      this.importYahooDataToolStripMenuItem.Size = new System.Drawing.Size(119, 20);
      this.importYahooDataToolStripMenuItem.Text = "Import Yahoo Data";
      // 
      // toolStripMenuItem_importDivHistory
      // 
      this.toolStripMenuItem_importDivHistory.Name = "toolStripMenuItem_importDivHistory";
      this.toolStripMenuItem_importDivHistory.Size = new System.Drawing.Size(201, 22);
      this.toolStripMenuItem_importDivHistory.Text = "Import Dividend History";
      this.toolStripMenuItem_importDivHistory.Click += new System.EventHandler(this.toolStripMenuItemImportDivHistory_Click);
      // 
      // importNABPricesToolStripMenuItem
      // 
      this.importNABPricesToolStripMenuItem.Name = "importNABPricesToolStripMenuItem";
      this.importNABPricesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.importNABPricesToolStripMenuItem.Text = "Import NAB Prices";
      this.importNABPricesToolStripMenuItem.Click += new System.EventHandler(this.importNABPricesToolStripMenuItem_Click);
      // 
      // importNABTransactionsToolStripMenuItem
      // 
      this.importNABTransactionsToolStripMenuItem.Name = "importNABTransactionsToolStripMenuItem";
      this.importNABTransactionsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.importNABTransactionsToolStripMenuItem.Text = "Import NAB Transactions";
      this.importNABTransactionsToolStripMenuItem.Click += new System.EventHandler(this.importNABTransactionsToolStripMenuItem_Click);
      // 
      // importNABDataToolStripMenuItem
      // 
      this.importNABDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importNABTransactionsToolStripMenuItem,
            this.importNABPricesToolStripMenuItem});
      this.importNABDataToolStripMenuItem.Name = "importNABDataToolStripMenuItem";
      this.importNABDataToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
      this.importNABDataToolStripMenuItem.Text = "Import NAB Data";
      // 
      // homeToolStripMenuItem
      // 
      this.homeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.suggestionsToolStripMenuItem});
      this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
      this.homeToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
      this.homeToolStripMenuItem.Text = "Home";
      // 
      // suggestionsToolStripMenuItem
      // 
      this.suggestionsToolStripMenuItem.Name = "suggestionsToolStripMenuItem";
      this.suggestionsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
      this.suggestionsToolStripMenuItem.Text = "Suggestions";
      this.suggestionsToolStripMenuItem.Click += new System.EventHandler(this.suggestionsToolStripMenuItem_Click);
      // 
      // BtnImportTransactions
      // 
      this.BtnImportTransactions.Location = new System.Drawing.Point(1284, 204);
      this.BtnImportTransactions.Name = "BtnImportTransactions";
      this.BtnImportTransactions.Size = new System.Drawing.Size(75, 23);
      this.BtnImportTransactions.TabIndex = 38;
      this.BtnImportTransactions.Text = "ImportTransactions";
      this.BtnImportTransactions.UseVisualStyleBackColor = true;
      this.BtnImportTransactions.Click += new System.EventHandler(this.BtnImportTransactions_Click);
      // 
      // lblSuggested
      // 
      this.lblSuggested.AutoSize = true;
      this.lblSuggested.Location = new System.Drawing.Point(-427, 185);
      this.lblSuggested.Name = "lblSuggested";
      this.lblSuggested.Size = new System.Drawing.Size(84, 13);
      this.lblSuggested.TabIndex = 37;
      this.lblSuggested.Text = "Suggested Buys";
      // 
      // DgvSuggestedBuys
      // 
      this.DgvSuggestedBuys.AllowUserToAddRows = false;
      this.DgvSuggestedBuys.AllowUserToDeleteRows = false;
      this.DgvSuggestedBuys.AllowUserToOrderColumns = true;
      this.DgvSuggestedBuys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DgvSuggestedBuys.Location = new System.Drawing.Point(594, -71);
      this.DgvSuggestedBuys.Name = "DgvSuggestedBuys";
      this.DgvSuggestedBuys.Size = new System.Drawing.Size(621, 216);
      this.DgvSuggestedBuys.TabIndex = 36;
      // 
      // BtnSuggestedSells
      // 
      this.BtnSuggestedSells.AutoSize = true;
      this.BtnSuggestedSells.Location = new System.Drawing.Point(-428, -64);
      this.BtnSuggestedSells.Name = "BtnSuggestedSells";
      this.BtnSuggestedSells.Size = new System.Drawing.Size(83, 13);
      this.BtnSuggestedSells.TabIndex = 35;
      this.BtnSuggestedSells.Text = "Suggested Sells";
      // 
      // BtnGenerateSuggestions
      // 
      this.BtnGenerateSuggestions.Location = new System.Drawing.Point(1303, 34);
      this.BtnGenerateSuggestions.Name = "BtnGenerateSuggestions";
      this.BtnGenerateSuggestions.Size = new System.Drawing.Size(75, 43);
      this.BtnGenerateSuggestions.TabIndex = 34;
      this.BtnGenerateSuggestions.Text = "Generate Suggestions";
      this.BtnGenerateSuggestions.UseVisualStyleBackColor = true;
      this.BtnGenerateSuggestions.Click += new System.EventHandler(this.BtnGenerateSuggestions_Click);
      // 
      // button6
      // 
      this.button6.Location = new System.Drawing.Point(633, 204);
      this.button6.Name = "button6";
      this.button6.Size = new System.Drawing.Size(124, 44);
      this.button6.TabIndex = 33;
      this.button6.Text = "Import Bank transactions";
      this.button6.UseVisualStyleBackColor = true;
      // 
      // DisplayGrid
      // 
      this.DisplayGrid.Location = new System.Drawing.Point(682, 263);
      this.DisplayGrid.Name = "DisplayGrid";
      this.DisplayGrid.Size = new System.Drawing.Size(75, 23);
      this.DisplayGrid.TabIndex = 32;
      this.DisplayGrid.Text = "DisplayGrid";
      this.DisplayGrid.UseVisualStyleBackColor = true;
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(1241, 233);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(134, 23);
      this.button4.TabIndex = 31;
      this.button4.Text = "Import Recent Prices";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(1241, 83);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(137, 23);
      this.button3.TabIndex = 30;
      this.button3.Text = "Import Todays Prices";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(1293, 112);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 29;
      this.button2.Text = "GetDividends";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click_1);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(1284, 175);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 28;
      this.button1.Text = "RunSimulation";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.TrialSimulation);
      // 
      // TestTran
      // 
      this.TestTran.Location = new System.Drawing.Point(1284, 142);
      this.TestTran.Name = "TestTran";
      this.TestTran.Size = new System.Drawing.Size(75, 23);
      this.TestTran.TabIndex = 27;
      this.TestTran.Text = "SetMinMaxs";
      this.TestTran.UseVisualStyleBackColor = true;
      this.TestTran.Click += new System.EventHandler(this.setMinMaxS);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.importNABDataToolStripMenuItem,
            this.importYahooDataToolStripMenuItem,
            this.Edits,
            this.exportsToolStripMenuItem,
            this.statisticsToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1384, 24);
      this.menuStrip1.TabIndex = 39;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // Edits
      // 
      this.Edits.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GLCodeToolStripMenuItem,
            this.enterConfirmationNrToolStripMenuItem1});
      this.Edits.Name = "Edits";
      this.Edits.Size = new System.Drawing.Size(44, 20);
      this.Edits.Text = "Edits";
      // 
      // GLCodeToolStripMenuItem
      // 
      this.GLCodeToolStripMenuItem.Name = "GLCodeToolStripMenuItem";
      this.GLCodeToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
      this.GLCodeToolStripMenuItem.Text = "Edit General Ledger Codes";
      this.GLCodeToolStripMenuItem.Click += new System.EventHandler(this.GLCodeToolStripMenuItem_Click);
      // 
      // exportsToolStripMenuItem
      // 
      this.exportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toMYOBToolStripMenuItem});
      this.exportsToolStripMenuItem.Name = "exportsToolStripMenuItem";
      this.exportsToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
      this.exportsToolStripMenuItem.Text = "Exports";
      // 
      // toMYOBToolStripMenuItem
      // 
      this.toMYOBToolStripMenuItem.Name = "toMYOBToolStripMenuItem";
      this.toMYOBToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
      this.toMYOBToolStripMenuItem.Text = "ToMYOB";
      this.toMYOBToolStripMenuItem.Click += new System.EventHandler(this.toMYOBToolStripMenuItem_Click);
      // 
      // DgvSuggestedSells
      // 
      this.DgvSuggestedSells.AllowUserToAddRows = false;
      this.DgvSuggestedSells.AllowUserToDeleteRows = false;
      this.DgvSuggestedSells.AllowUserToOrderColumns = true;
      this.DgvSuggestedSells.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DgvSuggestedSells.Location = new System.Drawing.Point(-440, -71);
      this.DgvSuggestedSells.Name = "DgvSuggestedSells";
      this.DgvSuggestedSells.Size = new System.Drawing.Size(738, 216);
      this.DgvSuggestedSells.TabIndex = 26;
      this.DgvSuggestedSells.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSuggestedSells_CellContentClick);
      // 
      // chart1
      // 
      chartArea3.Name = "ChartArea1";
      this.chart1.ChartAreas.Add(chartArea3);
      legend3.Name = "Legend1";
      this.chart1.Legends.Add(legend3);
      this.chart1.Location = new System.Drawing.Point(12, 142);
      this.chart1.Name = "chart1";
      series3.ChartArea = "ChartArea1";
      series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series3.Legend = "Legend1";
      series3.Name = "Series1";
      this.chart1.Series.Add(series3);
      this.chart1.Size = new System.Drawing.Size(797, 350);
      this.chart1.TabIndex = 47;
      this.chart1.Text = "Suggested Sells";
      // 
      // CSVopenFileDialog
      // 
      this.CSVopenFileDialog.FileName = "CSVopenFileDialog";
      // 
      // progressBar
      // 
      this.progressBar.Location = new System.Drawing.Point(30, 596);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(237, 23);
      this.progressBar.TabIndex = 48;
      // 
      // backgroundWorker1
      // 
      this.backgroundWorker1.WorkerReportsProgress = true;
      this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
      this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      // 
      // statisticsToolStripMenuItem
      // 
      this.statisticsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gatherStatisticsToolStripMenuItem});
      this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
      this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
      this.statisticsToolStripMenuItem.Text = "Statistics";
      // 
      // enterConfirmationNrToolStripMenuItem1
      // 
      this.enterConfirmationNrToolStripMenuItem1.Name = "enterConfirmationNrToolStripMenuItem1";
      this.enterConfirmationNrToolStripMenuItem1.Size = new System.Drawing.Size(212, 22);
      this.enterConfirmationNrToolStripMenuItem1.Text = "Enter Confirmation Nr";
      this.enterConfirmationNrToolStripMenuItem1.Click += new System.EventHandler(this.enterConfirmationNrToolStripMenuItem_Click);
      // 
      // gatherStatisticsToolStripMenuItem
      // 
      this.gatherStatisticsToolStripMenuItem.Name = "gatherStatisticsToolStripMenuItem";
      this.gatherStatisticsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.gatherStatisticsToolStripMenuItem.Text = "Gather Statistics";
      this.gatherStatisticsToolStripMenuItem.Click += new System.EventHandler(this.gatherStatisticsToolStripMenuItem_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1384, 670);
      this.Controls.Add(this.progressBar);
      this.Controls.Add(this.chart1);
      this.Controls.Add(this.button5);
      this.Controls.Add(this.dataGridView2);
      this.Controls.Add(this.dataGridView1);
      this.Controls.Add(this.TbProfit);
      this.Controls.Add(this.lblProfit);
      this.Controls.Add(this.lblSOH);
      this.Controls.Add(this.TbSOH);
      this.Controls.Add(this.BtnImportTransactions);
      this.Controls.Add(this.lblSuggested);
      this.Controls.Add(this.DgvSuggestedBuys);
      this.Controls.Add(this.BtnSuggestedSells);
      this.Controls.Add(this.BtnGenerateSuggestions);
      this.Controls.Add(this.button6);
      this.Controls.Add(this.DisplayGrid);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.TestTran);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this.DgvSuggestedSells);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.DgvSuggestedBuys)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DgvSuggestedSells)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.shareAnalV2DataSetBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource shareAnalV2DataSetBindingSource;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox TbProfit;
        private System.Windows.Forms.Label lblProfit;
        private System.Windows.Forms.Label lblSOH;
        private System.Windows.Forms.TextBox TbSOH;
        private System.Windows.Forms.ToolStripMenuItem importRecentPricesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importRecentPricesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importYahooDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importNABPricesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importNABTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importNABDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.Button BtnImportTransactions;
        private System.Windows.Forms.Label lblSuggested;
        private System.Windows.Forms.DataGridView DgvSuggestedBuys;
        private System.Windows.Forms.Label BtnSuggestedSells;
        private System.Windows.Forms.Button BtnGenerateSuggestions;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button DisplayGrid;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button TestTran;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridView DgvSuggestedSells;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    private System.Windows.Forms.OpenFileDialog CSVopenFileDialog;
    private System.Windows.Forms.ToolStripMenuItem Edits;
    private System.Windows.Forms.ToolStripMenuItem GLCodeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toMYOBToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem suggestionsToolStripMenuItem;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_importDivHistory;
    private System.Windows.Forms.ToolStripMenuItem enterConfirmationNrToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem gatherStatisticsToolStripMenuItem;
  }
}

