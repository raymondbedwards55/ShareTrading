namespace ShareTrading
{
  partial class FrmBrokersRec
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
      this.dgvRecommendation = new System.Windows.Forms.DataGridView();
      this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.recommendationBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.frmBrokersRecBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.RecASXCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecCurrentPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecDate1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecPrice1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Rec1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecDate2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecPrice2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Rec2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecDate3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecPrice3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Rec3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecDate4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecPrice4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Rec4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecDate5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RecPrice5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Rec5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvRecommendation)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.recommendationBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.frmBrokersRecBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvRecommendation
      // 
      this.dgvRecommendation.AllowUserToAddRows = false;
      this.dgvRecommendation.AllowUserToDeleteRows = false;
      this.dgvRecommendation.AutoGenerateColumns = false;
      this.dgvRecommendation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvRecommendation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecASXCode,
            this.RecChanged,
            this.RecCurrentPrice,
            this.RecDiff,
            this.RecDate1,
            this.RecPrice1,
            this.Rec1,
            this.RecDate2,
            this.RecPrice2,
            this.Rec2,
            this.RecDate3,
            this.RecPrice3,
            this.Rec3,
            this.RecDate4,
            this.RecPrice4,
            this.Rec4,
            this.RecDate5,
            this.RecPrice5,
            this.Rec5});
      this.dgvRecommendation.DataSource = this.recommendationBindingSource;
      this.dgvRecommendation.Location = new System.Drawing.Point(22, 53);
      this.dgvRecommendation.Name = "dgvRecommendation";
      this.dgvRecommendation.ReadOnly = true;
      this.dgvRecommendation.Size = new System.Drawing.Size(1389, 484);
      this.dgvRecommendation.TabIndex = 0;
      this.dgvRecommendation.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRecommendations_CellFormatting);
      this.dgvRecommendation.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRecommendation_ColumnHeaderMouseClick);
      // 
      // dataGridViewCheckBoxColumn1
      // 
      this.dataGridViewCheckBoxColumn1.DataPropertyName = "RecChanged";
      this.dataGridViewCheckBoxColumn1.HeaderText = "RecChanged";
      this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
      this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.dataGridViewCheckBoxColumn1.ThreeState = true;
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.DataPropertyName = "RecChanged";
      this.dataGridViewTextBoxColumn1.HeaderText = "RecChanged";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
      // 
      // dataGridViewTextBoxColumn2
      // 
      this.dataGridViewTextBoxColumn2.DataPropertyName = "RecChanged";
      this.dataGridViewTextBoxColumn2.HeaderText = "RecChanged";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
      // 
      // recommendationBindingSource
      // 
      this.recommendationBindingSource.AllowNew = false;
      this.recommendationBindingSource.DataSource = typeof(ShareTrading.recommendation);
      // 
      // frmBrokersRecBindingSource
      // 
      this.frmBrokersRecBindingSource.DataSource = typeof(ShareTrading.FrmBrokersRec);
      // 
      // RecASXCode
      // 
      this.RecASXCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.RecASXCode.DataPropertyName = "RecASXCode";
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
      this.RecASXCode.DefaultCellStyle = dataGridViewCellStyle1;
      this.RecASXCode.HeaderText = "ASX Code";
      this.RecASXCode.Name = "RecASXCode";
      this.RecASXCode.ReadOnly = true;
      // 
      // RecChanged
      // 
      this.RecChanged.DataPropertyName = "RecChanged";
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      this.RecChanged.DefaultCellStyle = dataGridViewCellStyle2;
      this.RecChanged.HeaderText = "Change";
      this.RecChanged.Name = "RecChanged";
      this.RecChanged.ReadOnly = true;
      this.RecChanged.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.RecChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
      this.RecChanged.Width = 50;
      // 
      // RecCurrentPrice
      // 
      this.RecCurrentPrice.DataPropertyName = "RecCurrentPrice";
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.Format = "C2";
      this.RecCurrentPrice.DefaultCellStyle = dataGridViewCellStyle3;
      this.RecCurrentPrice.HeaderText = "Current Price";
      this.RecCurrentPrice.Name = "RecCurrentPrice";
      this.RecCurrentPrice.ReadOnly = true;
      // 
      // RecDiff
      // 
      this.RecDiff.DataPropertyName = "RecDiff";
      dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      dataGridViewCellStyle4.Format = "#.00\\%";
      dataGridViewCellStyle4.NullValue = null;
      this.RecDiff.DefaultCellStyle = dataGridViewCellStyle4;
      this.RecDiff.HeaderText = "Diff %";
      this.RecDiff.Name = "RecDiff";
      this.RecDiff.ReadOnly = true;
      this.RecDiff.Width = 80;
      // 
      // RecDate1
      // 
      this.RecDate1.DataPropertyName = "RecDate1";
      dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      this.RecDate1.DefaultCellStyle = dataGridViewCellStyle5;
      this.RecDate1.HeaderText = "Date";
      this.RecDate1.Name = "RecDate1";
      this.RecDate1.ReadOnly = true;
      this.RecDate1.Visible = false;
      this.RecDate1.Width = 80;
      // 
      // RecPrice1
      // 
      this.RecPrice1.DataPropertyName = "RecPrice1";
      dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle6.Format = "C2";
      dataGridViewCellStyle6.NullValue = null;
      this.RecPrice1.DefaultCellStyle = dataGridViewCellStyle6;
      this.RecPrice1.HeaderText = "Price";
      this.RecPrice1.Name = "RecPrice1";
      this.RecPrice1.ReadOnly = true;
      // 
      // Rec1
      // 
      this.Rec1.DataPropertyName = "Rec1";
      dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      this.Rec1.DefaultCellStyle = dataGridViewCellStyle7;
      this.Rec1.HeaderText = "";
      this.Rec1.Name = "Rec1";
      this.Rec1.ReadOnly = true;
      // 
      // RecDate2
      // 
      this.RecDate2.DataPropertyName = "RecDate2";
      this.RecDate2.HeaderText = "Date";
      this.RecDate2.Name = "RecDate2";
      this.RecDate2.ReadOnly = true;
      this.RecDate2.Visible = false;
      // 
      // RecPrice2
      // 
      this.RecPrice2.DataPropertyName = "RecPrice2";
      dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      dataGridViewCellStyle8.Format = "C2";
      this.RecPrice2.DefaultCellStyle = dataGridViewCellStyle8;
      this.RecPrice2.HeaderText = "Price";
      this.RecPrice2.Name = "RecPrice2";
      this.RecPrice2.ReadOnly = true;
      // 
      // Rec2
      // 
      this.Rec2.DataPropertyName = "Rec2";
      dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      this.Rec2.DefaultCellStyle = dataGridViewCellStyle9;
      this.Rec2.HeaderText = "";
      this.Rec2.Name = "Rec2";
      this.Rec2.ReadOnly = true;
      // 
      // RecDate3
      // 
      this.RecDate3.DataPropertyName = "RecDate3";
      this.RecDate3.HeaderText = "Date";
      this.RecDate3.Name = "RecDate3";
      this.RecDate3.ReadOnly = true;
      this.RecDate3.Visible = false;
      // 
      // RecPrice3
      // 
      this.RecPrice3.DataPropertyName = "RecPrice3";
      dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle10.Format = "C2";
      this.RecPrice3.DefaultCellStyle = dataGridViewCellStyle10;
      this.RecPrice3.HeaderText = "Price";
      this.RecPrice3.Name = "RecPrice3";
      this.RecPrice3.ReadOnly = true;
      // 
      // Rec3
      // 
      this.Rec3.DataPropertyName = "Rec3";
      dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      this.Rec3.DefaultCellStyle = dataGridViewCellStyle11;
      this.Rec3.HeaderText = "";
      this.Rec3.Name = "Rec3";
      this.Rec3.ReadOnly = true;
      // 
      // RecDate4
      // 
      this.RecDate4.DataPropertyName = "RecDate4";
      this.RecDate4.HeaderText = "Date";
      this.RecDate4.Name = "RecDate4";
      this.RecDate4.ReadOnly = true;
      this.RecDate4.Visible = false;
      // 
      // RecPrice4
      // 
      this.RecPrice4.DataPropertyName = "RecPrice4";
      dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      dataGridViewCellStyle12.Format = "C2";
      this.RecPrice4.DefaultCellStyle = dataGridViewCellStyle12;
      this.RecPrice4.HeaderText = "Price";
      this.RecPrice4.Name = "RecPrice4";
      this.RecPrice4.ReadOnly = true;
      // 
      // Rec4
      // 
      this.Rec4.DataPropertyName = "Rec4";
      dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      this.Rec4.DefaultCellStyle = dataGridViewCellStyle13;
      this.Rec4.HeaderText = "";
      this.Rec4.Name = "Rec4";
      this.Rec4.ReadOnly = true;
      // 
      // RecDate5
      // 
      this.RecDate5.DataPropertyName = "RecDate5";
      this.RecDate5.HeaderText = "Date";
      this.RecDate5.Name = "RecDate5";
      this.RecDate5.ReadOnly = true;
      this.RecDate5.Visible = false;
      // 
      // RecPrice5
      // 
      this.RecPrice5.DataPropertyName = "RecPrice5";
      dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle14.Format = "C2";
      this.RecPrice5.DefaultCellStyle = dataGridViewCellStyle14;
      this.RecPrice5.HeaderText = "Price";
      this.RecPrice5.Name = "RecPrice5";
      this.RecPrice5.ReadOnly = true;
      // 
      // Rec5
      // 
      this.Rec5.DataPropertyName = "Rec5";
      dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      this.Rec5.DefaultCellStyle = dataGridViewCellStyle15;
      this.Rec5.HeaderText = "";
      this.Rec5.Name = "Rec5";
      this.Rec5.ReadOnly = true;
      // 
      // FrmBrokersRec
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1439, 598);
      this.Controls.Add(this.dgvRecommendation);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MinimizeBox = false;
      this.Name = "FrmBrokersRec";
      this.ShowIcon = false;
      this.Text = "Brokers Recommendations";
      ((System.ComponentModel.ISupportInitialize)(this.dgvRecommendation)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.recommendationBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.frmBrokersRecBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvRecommendation;
    private System.Windows.Forms.BindingSource recommendationBindingSource;
    private System.Windows.Forms.BindingSource frmBrokersRecBindingSource;
    private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecASXCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecChanged;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecCurrentPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecDiff;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecDate1;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecPrice1;
    private System.Windows.Forms.DataGridViewTextBoxColumn Rec1;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecDate2;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecPrice2;
    private System.Windows.Forms.DataGridViewTextBoxColumn Rec2;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecDate3;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecPrice3;
    private System.Windows.Forms.DataGridViewTextBoxColumn Rec3;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecDate4;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecPrice4;
    private System.Windows.Forms.DataGridViewTextBoxColumn Rec4;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecDate5;
    private System.Windows.Forms.DataGridViewTextBoxColumn RecPrice5;
    private System.Windows.Forms.DataGridViewTextBoxColumn Rec5;
  }
}