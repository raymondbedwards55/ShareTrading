namespace ShareTrading
{
  partial class FrmEditCompanyDetails
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.chbOnWatchList = new System.Windows.Forms.CheckBox();
      this.tbxCompanyName = new System.Windows.Forms.TextBox();
      this.cbxASXCode = new System.Windows.Forms.ComboBox();
      this.CoBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.chbIncDeleted = new System.Windows.Forms.CheckBox();
      this.dgvCompanyDetails = new System.Windows.Forms.DataGridView();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonUnDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonPrev = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
      this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ASXCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CompanyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.OnWatchList = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.DateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DateModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DateDeleted = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.CoBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyDetails)).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.chbOnWatchList);
      this.groupBox1.Controls.Add(this.tbxCompanyName);
      this.groupBox1.Controls.Add(this.cbxASXCode);
      this.groupBox1.Location = new System.Drawing.Point(72, 91);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(939, 112);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Edit ...";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(394, 52);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(82, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Company Name";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(76, 52);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(56, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "ASX Code";
      // 
      // chbOnWatchList
      // 
      this.chbOnWatchList.AutoSize = true;
      this.chbOnWatchList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.chbOnWatchList.Location = new System.Drawing.Point(711, 48);
      this.chbOnWatchList.Name = "chbOnWatchList";
      this.chbOnWatchList.Size = new System.Drawing.Size(93, 17);
      this.chbOnWatchList.TabIndex = 7;
      this.chbOnWatchList.Text = "On Watchlist?";
      this.chbOnWatchList.UseVisualStyleBackColor = true;
      // 
      // tbxCompanyName
      // 
      this.tbxCompanyName.Location = new System.Drawing.Point(502, 46);
      this.tbxCompanyName.Name = "tbxCompanyName";
      this.tbxCompanyName.Size = new System.Drawing.Size(173, 20);
      this.tbxCompanyName.TabIndex = 5;
      this.tbxCompanyName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // cbxASXCode
      // 
      this.cbxASXCode.FormattingEnabled = true;
      this.cbxASXCode.Location = new System.Drawing.Point(184, 44);
      this.cbxASXCode.Name = "cbxASXCode";
      this.cbxASXCode.Size = new System.Drawing.Size(173, 21);
      this.cbxASXCode.TabIndex = 3;
      // 
      // CoBindingSource
      // 
      this.CoBindingSource.AllowNew = false;
      this.CoBindingSource.DataSource = typeof(ShareTrading.DBAccess.CompanyDetails);
      // 
      // chbIncDeleted
      // 
      this.chbIncDeleted.AutoSize = true;
      this.chbIncDeleted.Location = new System.Drawing.Point(151, 209);
      this.chbIncDeleted.Name = "chbIncDeleted";
      this.chbIncDeleted.Size = new System.Drawing.Size(101, 17);
      this.chbIncDeleted.TabIndex = 11;
      this.chbIncDeleted.Text = "Include Deleted";
      this.chbIncDeleted.UseVisualStyleBackColor = true;
      this.chbIncDeleted.CheckedChanged += new System.EventHandler(this.chbIncDeleted_CheckedChanged);
      // 
      // dgvCompanyDetails
      // 
      this.dgvCompanyDetails.AllowUserToAddRows = false;
      this.dgvCompanyDetails.AllowUserToDeleteRows = false;
      this.dgvCompanyDetails.AutoGenerateColumns = false;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvCompanyDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dgvCompanyDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCompanyDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ASXCode,
            this.CompanyName,
            this.OnWatchList,
            this.DateCreated,
            this.DateModified,
            this.DateDeleted});
      this.dgvCompanyDetails.DataSource = this.CoBindingSource;
      this.dgvCompanyDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dgvCompanyDetails.Location = new System.Drawing.Point(151, 233);
      this.dgvCompanyDetails.Name = "dgvCompanyDetails";
      this.dgvCompanyDetails.ReadOnly = true;
      this.dgvCompanyDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvCompanyDetails.Size = new System.Drawing.Size(792, 457);
      this.dgvCompanyDetails.TabIndex = 10;
      this.dgvCompanyDetails.TabStop = false;
      this.dgvCompanyDetails.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCompanyDetails_CellFormatting);
      this.dgvCompanyDetails.SelectionChanged += new System.EventHandler(this.dgvCompanyDetails_SelectionChanged);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonUnDelete,
            this.toolStripSeparator1,
            this.toolStripButtonSave,
            this.toolStripButtonCancel,
            this.toolStripSeparator2,
            this.toolStripButtonNext,
            this.toolStripButtonPrev,
            this.toolStripSeparator3,
            this.toolStripButtonClose});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1087, 28);
      this.toolStrip1.TabIndex = 8;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripButtonAdd
      // 
      this.toolStripButtonAdd.Image = global::ShareTrading.Properties.Resources.AddTableHS;
      this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAdd.Name = "toolStripButtonAdd";
      this.toolStripButtonAdd.Size = new System.Drawing.Size(61, 25);
      this.toolStripButtonAdd.Text = "Add";
      this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
      // 
      // toolStripButtonEdit
      // 
      this.toolStripButtonEdit.Image = global::ShareTrading.Properties.Resources.EditInformationHS;
      this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEdit.Name = "toolStripButtonEdit";
      this.toolStripButtonEdit.Size = new System.Drawing.Size(58, 25);
      this.toolStripButtonEdit.Text = "Edit";
      this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
      // 
      // toolStripButtonDelete
      // 
      this.toolStripButtonDelete.Image = global::ShareTrading.Properties.Resources.DeleteHS;
      this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDelete.Name = "toolStripButtonDelete";
      this.toolStripButtonDelete.Size = new System.Drawing.Size(78, 25);
      this.toolStripButtonDelete.Text = "Delete";
      this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
      // 
      // toolStripButtonUnDelete
      // 
      this.toolStripButtonUnDelete.Image = global::ShareTrading.Properties.Resources.Edit_UndoHS;
      this.toolStripButtonUnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonUnDelete.Name = "toolStripButtonUnDelete";
      this.toolStripButtonUnDelete.Size = new System.Drawing.Size(98, 25);
      this.toolStripButtonUnDelete.Text = "UnDelete";
      this.toolStripButtonUnDelete.Click += new System.EventHandler(this.toolStripButtonUnDelete_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
      // 
      // toolStripButtonSave
      // 
      this.toolStripButtonSave.Image = global::ShareTrading.Properties.Resources.saveHS;
      this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonSave.Name = "toolStripButtonSave";
      this.toolStripButtonSave.Size = new System.Drawing.Size(64, 25);
      this.toolStripButtonSave.Text = "Save";
      this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
      // 
      // toolStripButtonCancel
      // 
      this.toolStripButtonCancel.Image = global::ShareTrading.Properties.Resources.cancel;
      this.toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonCancel.Name = "toolStripButtonCancel";
      this.toolStripButtonCancel.Size = new System.Drawing.Size(78, 25);
      this.toolStripButtonCancel.Text = "Cancel";
      this.toolStripButtonCancel.Click += new System.EventHandler(this.toolStripButtonCancel_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
      // 
      // toolStripButtonNext
      // 
      this.toolStripButtonNext.Image = global::ShareTrading.Properties.Resources.NavForward;
      this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonNext.Name = "toolStripButtonNext";
      this.toolStripButtonNext.Size = new System.Drawing.Size(65, 25);
      this.toolStripButtonNext.Text = "Next";
      this.toolStripButtonNext.Click += new System.EventHandler(this.toolStripButtonNext_Click);
      // 
      // toolStripButtonPrev
      // 
      this.toolStripButtonPrev.Image = global::ShareTrading.Properties.Resources.NavBack;
      this.toolStripButtonPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonPrev.Name = "toolStripButtonPrev";
      this.toolStripButtonPrev.Size = new System.Drawing.Size(62, 25);
      this.toolStripButtonPrev.Text = "Prev";
      this.toolStripButtonPrev.Click += new System.EventHandler(this.toolStripButtonPrev_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
      // 
      // toolStripButtonClose
      // 
      this.toolStripButtonClose.Image = global::ShareTrading.Properties.Resources.door_out;
      this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonClose.Name = "toolStripButtonClose";
      this.toolStripButtonClose.Size = new System.Drawing.Size(70, 25);
      this.toolStripButtonClose.Text = "Close";
      this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
      // 
      // ID
      // 
      this.ID.DataPropertyName = "ID";
      this.ID.HeaderText = "ID";
      this.ID.Name = "ID";
      this.ID.ReadOnly = true;
      this.ID.Visible = false;
      // 
      // ASXCode
      // 
      this.ASXCode.DataPropertyName = "ASXCode";
      this.ASXCode.HeaderText = "ASX Code";
      this.ASXCode.Name = "ASXCode";
      this.ASXCode.ReadOnly = true;
      // 
      // CompanyName
      // 
      this.CompanyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.CompanyName.DataPropertyName = "CompanyName";
      this.CompanyName.HeaderText = "Company Name";
      this.CompanyName.Name = "CompanyName";
      this.CompanyName.ReadOnly = true;
      // 
      // OnWatchList
      // 
      this.OnWatchList.DataPropertyName = "OnWatchList";
      this.OnWatchList.HeaderText = "On Watch List?";
      this.OnWatchList.Name = "OnWatchList";
      this.OnWatchList.ReadOnly = true;
      // 
      // DateCreated
      // 
      this.DateCreated.DataPropertyName = "DateCreated";
      this.DateCreated.HeaderText = "Date Created";
      this.DateCreated.Name = "DateCreated";
      this.DateCreated.ReadOnly = true;
      // 
      // DateModified
      // 
      this.DateModified.DataPropertyName = "DateModified";
      this.DateModified.HeaderText = "Date Modified";
      this.DateModified.Name = "DateModified";
      this.DateModified.ReadOnly = true;
      // 
      // DateDeleted
      // 
      this.DateDeleted.DataPropertyName = "DateDeleted";
      this.DateDeleted.HeaderText = "DateDeleted";
      this.DateDeleted.Name = "DateDeleted";
      this.DateDeleted.ReadOnly = true;
      this.DateDeleted.Visible = false;
      // 
      // FrmEditCompanyDetails
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1087, 723);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.chbIncDeleted);
      this.Controls.Add(this.dgvCompanyDetails);
      this.Controls.Add(this.toolStrip1);
      this.Name = "FrmEditCompanyDetails";
      this.Text = "FrmEditCompanyDetails";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEditCompanyDetails_FormClosing);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.CoBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyDetails)).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox chbOnWatchList;
    private System.Windows.Forms.TextBox tbxCompanyName;
    private System.Windows.Forms.ComboBox cbxASXCode;
    private System.Windows.Forms.BindingSource CoBindingSource;
    private System.Windows.Forms.CheckBox chbIncDeleted;
    private System.Windows.Forms.DataGridView dgvCompanyDetails;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
    private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
    private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
    private System.Windows.Forms.ToolStripButton toolStripButtonUnDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButtonSave;
    private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton toolStripButtonNext;
    private System.Windows.Forms.ToolStripButton toolStripButtonPrev;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.DataGridViewTextBoxColumn ID;
    private System.Windows.Forms.DataGridViewTextBoxColumn ASXCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn CompanyName;
    private System.Windows.Forms.DataGridViewCheckBoxColumn OnWatchList;
    private System.Windows.Forms.DataGridViewTextBoxColumn DateCreated;
    private System.Windows.Forms.DataGridViewTextBoxColumn DateModified;
    private System.Windows.Forms.DataGridViewTextBoxColumn DateDeleted;
  }
}