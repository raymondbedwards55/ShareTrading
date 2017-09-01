namespace ShareTrading
{
  partial class EditGeneralLedger
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
      this.tbxGLCode = new System.Windows.Forms.TextBox();
      this.cbxGLType = new System.Windows.Forms.ComboBox();
      this.chbGSTApplies = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.dgvGeneralLedger = new System.Windows.Forms.DataGridView();
      this.GLBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.chbIncDeleted = new System.Windows.Forms.CheckBox();
      this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.gLCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.gSTAppliesDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.dateCreatedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dateModifiedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DateDeleted = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.toolStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvGeneralLedger)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.GLBindingSource)).BeginInit();
      this.SuspendLayout();
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
      this.toolStrip1.Size = new System.Drawing.Size(1060, 28);
      this.toolStrip1.TabIndex = 0;
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
      // tbxGLCode
      // 
      this.tbxGLCode.Location = new System.Drawing.Point(502, 46);
      this.tbxGLCode.Name = "tbxGLCode";
      this.tbxGLCode.Size = new System.Drawing.Size(173, 22);
      this.tbxGLCode.TabIndex = 5;
      this.tbxGLCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // cbxGLType
      // 
      this.cbxGLType.FormattingEnabled = true;
      this.cbxGLType.Location = new System.Drawing.Point(184, 44);
      this.cbxGLType.Name = "cbxGLType";
      this.cbxGLType.Size = new System.Drawing.Size(173, 24);
      this.cbxGLType.TabIndex = 3;
      // 
      // chbGSTApplies
      // 
      this.chbGSTApplies.AutoSize = true;
      this.chbGSTApplies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.chbGSTApplies.Location = new System.Drawing.Point(711, 48);
      this.chbGSTApplies.Name = "chbGSTApplies";
      this.chbGSTApplies.Size = new System.Drawing.Size(123, 20);
      this.chbGSTApplies.TabIndex = 7;
      this.chbGSTApplies.Text = "GST Applies?";
      this.chbGSTApplies.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.chbGSTApplies);
      this.groupBox1.Controls.Add(this.tbxGLCode);
      this.groupBox1.Controls.Add(this.cbxGLType);
      this.groupBox1.Location = new System.Drawing.Point(72, 58);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(939, 112);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Edit ...";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(394, 52);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(68, 16);
      this.label2.TabIndex = 6;
      this.label2.Text = "GL Code";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(76, 52);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(67, 16);
      this.label1.TabIndex = 5;
      this.label1.Text = "GL Type";
      // 
      // dgvGeneralLedger
      // 
      this.dgvGeneralLedger.AllowUserToAddRows = false;
      this.dgvGeneralLedger.AllowUserToDeleteRows = false;
      this.dgvGeneralLedger.AutoGenerateColumns = false;
      this.dgvGeneralLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvGeneralLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.typeDataGridViewTextBoxColumn,
            this.gLCodeDataGridViewTextBoxColumn,
            this.gSTAppliesDataGridViewCheckBoxColumn,
            this.dateCreatedDataGridViewTextBoxColumn,
            this.dateModifiedDataGridViewTextBoxColumn,
            this.DateDeleted,
            this.ID});
      this.dgvGeneralLedger.DataSource = this.GLBindingSource;
      this.dgvGeneralLedger.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dgvGeneralLedger.Location = new System.Drawing.Point(151, 200);
      this.dgvGeneralLedger.Name = "dgvGeneralLedger";
      this.dgvGeneralLedger.ReadOnly = true;
      this.dgvGeneralLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvGeneralLedger.Size = new System.Drawing.Size(792, 457);
      this.dgvGeneralLedger.TabIndex = 6;
      this.dgvGeneralLedger.TabStop = false;
      this.dgvGeneralLedger.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGeneralLedger_CellFormatting);
      this.dgvGeneralLedger.SelectionChanged += new System.EventHandler(this.dgvGeneralLedger_SelectionChanged);
      // 
      // GLBindingSource
      // 
      this.GLBindingSource.AllowNew = false;
      this.GLBindingSource.DataSource = typeof(ShareTrading.DBAccess.GLCodes);
      // 
      // chbIncDeleted
      // 
      this.chbIncDeleted.AutoSize = true;
      this.chbIncDeleted.Location = new System.Drawing.Point(151, 176);
      this.chbIncDeleted.Name = "chbIncDeleted";
      this.chbIncDeleted.Size = new System.Drawing.Size(136, 20);
      this.chbIncDeleted.TabIndex = 7;
      this.chbIncDeleted.Text = "Include Deleted";
      this.chbIncDeleted.UseVisualStyleBackColor = true;
      this.chbIncDeleted.CheckedChanged += new System.EventHandler(this.chbIncDeleted_CheckedChanged);
      // 
      // typeDataGridViewTextBoxColumn
      // 
      this.typeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
      this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
      this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
      this.typeDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // gLCodeDataGridViewTextBoxColumn
      // 
      this.gLCodeDataGridViewTextBoxColumn.DataPropertyName = "GLCode";
      this.gLCodeDataGridViewTextBoxColumn.HeaderText = "GL Code";
      this.gLCodeDataGridViewTextBoxColumn.Name = "gLCodeDataGridViewTextBoxColumn";
      this.gLCodeDataGridViewTextBoxColumn.ReadOnly = true;
      this.gLCodeDataGridViewTextBoxColumn.Width = 150;
      // 
      // gSTAppliesDataGridViewCheckBoxColumn
      // 
      this.gSTAppliesDataGridViewCheckBoxColumn.DataPropertyName = "GSTApplies";
      this.gSTAppliesDataGridViewCheckBoxColumn.HeaderText = "GST Applies";
      this.gSTAppliesDataGridViewCheckBoxColumn.Name = "gSTAppliesDataGridViewCheckBoxColumn";
      this.gSTAppliesDataGridViewCheckBoxColumn.ReadOnly = true;
      this.gSTAppliesDataGridViewCheckBoxColumn.Width = 120;
      // 
      // dateCreatedDataGridViewTextBoxColumn
      // 
      this.dateCreatedDataGridViewTextBoxColumn.DataPropertyName = "DateCreated";
      this.dateCreatedDataGridViewTextBoxColumn.HeaderText = "Date Created";
      this.dateCreatedDataGridViewTextBoxColumn.Name = "dateCreatedDataGridViewTextBoxColumn";
      this.dateCreatedDataGridViewTextBoxColumn.ReadOnly = true;
      this.dateCreatedDataGridViewTextBoxColumn.Width = 150;
      // 
      // dateModifiedDataGridViewTextBoxColumn
      // 
      this.dateModifiedDataGridViewTextBoxColumn.DataPropertyName = "DateModified";
      this.dateModifiedDataGridViewTextBoxColumn.HeaderText = "Date Modified";
      this.dateModifiedDataGridViewTextBoxColumn.Name = "dateModifiedDataGridViewTextBoxColumn";
      this.dateModifiedDataGridViewTextBoxColumn.ReadOnly = true;
      this.dateModifiedDataGridViewTextBoxColumn.Width = 150;
      // 
      // DateDeleted
      // 
      this.DateDeleted.DataPropertyName = "DateDeleted";
      this.DateDeleted.HeaderText = "DateDeleted";
      this.DateDeleted.Name = "DateDeleted";
      this.DateDeleted.ReadOnly = true;
      this.DateDeleted.Visible = false;
      this.DateDeleted.Width = 200;
      // 
      // ID
      // 
      this.ID.DataPropertyName = "ID";
      this.ID.HeaderText = "ID";
      this.ID.Name = "ID";
      this.ID.ReadOnly = true;
      this.ID.Visible = false;
      // 
      // EditGeneralLedger
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1060, 682);
      this.Controls.Add(this.chbIncDeleted);
      this.Controls.Add(this.dgvGeneralLedger);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.toolStrip1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "EditGeneralLedger";
      this.ShowIcon = false;
      this.Text = "Edit General Ledger";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditGeneralLedger_FormClosing);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvGeneralLedger)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.GLBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
    private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
    private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
    private System.Windows.Forms.ToolStripButton toolStripButtonUnDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButtonNext;
    private System.Windows.Forms.ToolStripButton toolStripButtonPrev;
    private System.Windows.Forms.ToolStripButton toolStripButtonClose;
    private System.Windows.Forms.TextBox tbxGLCode;
    private System.Windows.Forms.ComboBox cbxGLType;
    private System.Windows.Forms.CheckBox chbGSTApplies;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.DataGridView dgvGeneralLedger;
    private System.Windows.Forms.BindingSource GLBindingSource;
    private System.Windows.Forms.ToolStripButton toolStripButtonSave;
    private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.CheckBox chbIncDeleted;
    private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn gLCodeDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewCheckBoxColumn gSTAppliesDataGridViewCheckBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn dateCreatedDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn dateModifiedDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn DateDeleted;
    private System.Windows.Forms.DataGridViewTextBoxColumn ID;
  }
}