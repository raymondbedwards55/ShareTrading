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
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonGenerate = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
      this.cbxStatsType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonGenerate,
            this.toolStripButtonClose});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new System.Drawing.Size(912, 25);
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
      // cbxStatsType
      // 
      this.cbxStatsType.FormattingEnabled = true;
      this.cbxStatsType.Location = new System.Drawing.Point(252, 151);
      this.cbxStatsType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.cbxStatsType.Name = "cbxStatsType";
      this.cbxStatsType.Size = new System.Drawing.Size(202, 24);
      this.cbxStatsType.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(249, 131);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(111, 16);
      this.label1.TabIndex = 2;
      this.label1.Text = "Statistics Type";
      // 
      // FrmGatherStats
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(912, 481);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbxStatsType);
      this.Controls.Add(this.toolStrip1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmGatherStats";
      this.ShowIcon = false;
      this.Text = "Gather Statistics";
      this.Load += new System.EventHandler(this.FrmGatherStats_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonGenerate;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.ComboBox cbxStatsType;
    private System.Windows.Forms.Label label1;
  }
}