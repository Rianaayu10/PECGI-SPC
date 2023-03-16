
namespace SPCMeasurement
{
    partial class frmMeasurement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeasurement));
            this.label11 = new System.Windows.Forms.Label();
            this.cboProcess = new C1.Win.C1List.C1Combo();
            this.cboLineGroup = new C1.Win.C1List.C1Combo();
            this.label10 = new System.Windows.Forms.Label();
            this.cboProcessGroup = new C1.Win.C1List.C1Combo();
            this.label9 = new System.Windows.Forms.Label();
            this.cboType = new C1.Win.C1List.C1Combo();
            this.cboReg = new C1.Win.C1List.C1Combo();
            this.label8 = new System.Windows.Forms.Label();
            this.dtProd = new System.Windows.Forms.DateTimePicker();
            this.cboSeq = new C1.Win.C1List.C1Combo();
            this.label7 = new System.Windows.Forms.Label();
            this.cboShift = new C1.Win.C1List.C1Combo();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboItemCheck = new C1.Win.C1List.C1Combo();
            this.cboLine = new C1.Win.C1List.C1Combo();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboFactory = new C1.Win.C1List.C1Combo();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.stMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerCurr = new System.Windows.Forms.Timer(this.components);
            this.lblScaleCon = new System.Windows.Forms.Label();
            this.lblScaleStatus = new System.Windows.Forms.Label();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.grid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtfTerminal = new System.Windows.Forms.RichTextBox();
            this.lblArg = new System.Windows.Forms.Label();
            this.pnlValue = new System.Windows.Forms.Panel();
            this.opt2 = new System.Windows.Forms.RadioButton();
            this.opt1 = new System.Windows.Forms.RadioButton();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.stUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.stServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLineGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSeq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboItemCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFactory)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(319, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 17);
            this.label11.TabIndex = 22;
            this.label11.Text = "Process";
            // 
            // cboProcess
            // 
            this.cboProcess.AddItemSeparator = ';';
            this.cboProcess.Caption = "";
            this.cboProcess.CaptionHeight = 17;
            this.cboProcess.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboProcess.ColumnCaptionHeight = 17;
            this.cboProcess.ColumnFooterHeight = 17;
            this.cboProcess.ContentHeight = 18;
            this.cboProcess.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboProcess.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboProcess.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProcess.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboProcess.EditorHeight = 18;
            this.cboProcess.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProcess.Images.Add(((System.Drawing.Image)(resources.GetObject("cboProcess.Images"))));
            this.cboProcess.ItemHeight = 15;
            this.cboProcess.Location = new System.Drawing.Point(440, 10);
            this.cboProcess.MatchEntryTimeout = ((long)(2000));
            this.cboProcess.MaxDropDownItems = ((short)(5));
            this.cboProcess.MaxLength = 32767;
            this.cboProcess.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboProcess.Name = "cboProcess";
            this.cboProcess.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboProcess.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboProcess.Size = new System.Drawing.Size(232, 24);
            this.cboProcess.TabIndex = 4;
            this.cboProcess.TextChanged += new System.EventHandler(this.cboProcess_TextChanged);
            this.cboProcess.PropBag = resources.GetString("cboProcess.PropBag");
            // 
            // cboLineGroup
            // 
            this.cboLineGroup.AddItemSeparator = ';';
            this.cboLineGroup.Caption = "";
            this.cboLineGroup.CaptionHeight = 17;
            this.cboLineGroup.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboLineGroup.ColumnCaptionHeight = 17;
            this.cboLineGroup.ColumnFooterHeight = 17;
            this.cboLineGroup.ContentHeight = 18;
            this.cboLineGroup.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboLineGroup.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboLineGroup.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLineGroup.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboLineGroup.EditorHeight = 18;
            this.cboLineGroup.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLineGroup.Images.Add(((System.Drawing.Image)(resources.GetObject("cboLineGroup.Images"))));
            this.cboLineGroup.ItemHeight = 18;
            this.cboLineGroup.Location = new System.Drawing.Point(118, 70);
            this.cboLineGroup.MatchEntryTimeout = ((long)(2000));
            this.cboLineGroup.MaxDropDownItems = ((short)(5));
            this.cboLineGroup.MaxLength = 32767;
            this.cboLineGroup.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboLineGroup.Name = "cboLineGroup";
            this.cboLineGroup.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboLineGroup.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboLineGroup.Size = new System.Drawing.Size(188, 24);
            this.cboLineGroup.TabIndex = 3;
            this.cboLineGroup.TextChanged += new System.EventHandler(this.cboLineGroup_TextChanged);
            this.cboLineGroup.PropBag = resources.GetString("cboLineGroup.PropBag");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(11, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "Line Group";
            // 
            // cboProcessGroup
            // 
            this.cboProcessGroup.AddItemSeparator = ';';
            this.cboProcessGroup.Caption = "";
            this.cboProcessGroup.CaptionHeight = 17;
            this.cboProcessGroup.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboProcessGroup.ColumnCaptionHeight = 17;
            this.cboProcessGroup.ColumnFooterHeight = 17;
            this.cboProcessGroup.ContentHeight = 18;
            this.cboProcessGroup.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboProcessGroup.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboProcessGroup.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProcessGroup.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboProcessGroup.EditorHeight = 18;
            this.cboProcessGroup.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProcessGroup.Images.Add(((System.Drawing.Image)(resources.GetObject("cboProcessGroup.Images"))));
            this.cboProcessGroup.ItemHeight = 18;
            this.cboProcessGroup.Location = new System.Drawing.Point(118, 40);
            this.cboProcessGroup.MatchEntryTimeout = ((long)(2000));
            this.cboProcessGroup.MaxDropDownItems = ((short)(5));
            this.cboProcessGroup.MaxLength = 32767;
            this.cboProcessGroup.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboProcessGroup.Name = "cboProcessGroup";
            this.cboProcessGroup.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboProcessGroup.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboProcessGroup.Size = new System.Drawing.Size(188, 24);
            this.cboProcessGroup.TabIndex = 2;
            this.cboProcessGroup.TextChanged += new System.EventHandler(this.cboProcessGroup_TextChanged);
            this.cboProcessGroup.PropBag = resources.GetString("cboProcessGroup.PropBag");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 17);
            this.label9.TabIndex = 17;
            this.label9.Text = "Process Group";
            // 
            // cboType
            // 
            this.cboType.AddItemSeparator = ';';
            this.cboType.Caption = "";
            this.cboType.CaptionHeight = 17;
            this.cboType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboType.ColumnCaptionHeight = 17;
            this.cboType.ColumnFooterHeight = 17;
            this.cboType.ContentHeight = 18;
            this.cboType.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboType.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboType.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboType.EditorHeight = 18;
            this.cboType.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.Images.Add(((System.Drawing.Image)(resources.GetObject("cboType.Images"))));
            this.cboType.ItemHeight = 18;
            this.cboType.Location = new System.Drawing.Point(440, 70);
            this.cboType.MatchEntryTimeout = ((long)(2000));
            this.cboType.MaxDropDownItems = ((short)(5));
            this.cboType.MaxLength = 32767;
            this.cboType.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboType.Name = "cboType";
            this.cboType.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboType.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboType.Size = new System.Drawing.Size(232, 24);
            this.cboType.TabIndex = 6;
            this.cboType.TextChanged += new System.EventHandler(this.cboType_TextChanged);
            this.cboType.PropBag = resources.GetString("cboType.PropBag");
            // 
            // cboReg
            // 
            this.cboReg.AddItemSeparator = ';';
            this.cboReg.Caption = "";
            this.cboReg.CaptionHeight = 17;
            this.cboReg.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboReg.ColumnCaptionHeight = 17;
            this.cboReg.ColumnFooterHeight = 17;
            this.cboReg.ContentHeight = 18;
            this.cboReg.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboReg.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboReg.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboReg.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboReg.EditorHeight = 18;
            this.cboReg.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboReg.Images.Add(((System.Drawing.Image)(resources.GetObject("cboReg.Images"))));
            this.cboReg.ItemHeight = 15;
            this.cboReg.Location = new System.Drawing.Point(1139, 10);
            this.cboReg.MatchEntryTimeout = ((long)(2000));
            this.cboReg.MaxDropDownItems = ((short)(5));
            this.cboReg.MaxLength = 32767;
            this.cboReg.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboReg.Name = "cboReg";
            this.cboReg.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboReg.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboReg.Size = new System.Drawing.Size(124, 24);
            this.cboReg.TabIndex = 11;
            this.cboReg.PropBag = resources.GetString("cboReg.PropBag");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1076, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Reg. No";
            // 
            // dtProd
            // 
            this.dtProd.CustomFormat = "dd MMM yyyy";
            this.dtProd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtProd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtProd.Location = new System.Drawing.Point(772, 41);
            this.dtProd.Name = "dtProd";
            this.dtProd.Size = new System.Drawing.Size(118, 23);
            this.dtProd.TabIndex = 8;
            this.dtProd.ValueChanged += new System.EventHandler(this.dtProd_ValueChanged);
            // 
            // cboSeq
            // 
            this.cboSeq.AddItemSeparator = ';';
            this.cboSeq.Caption = "";
            this.cboSeq.CaptionHeight = 17;
            this.cboSeq.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboSeq.ColumnCaptionHeight = 17;
            this.cboSeq.ColumnFooterHeight = 17;
            this.cboSeq.ContentHeight = 18;
            this.cboSeq.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboSeq.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboSeq.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSeq.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboSeq.EditorHeight = 18;
            this.cboSeq.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSeq.Images.Add(((System.Drawing.Image)(resources.GetObject("cboSeq.Images"))));
            this.cboSeq.ItemHeight = 15;
            this.cboSeq.Location = new System.Drawing.Point(938, 70);
            this.cboSeq.MatchEntryTimeout = ((long)(2000));
            this.cboSeq.MaxDropDownItems = ((short)(5));
            this.cboSeq.MaxLength = 32767;
            this.cboSeq.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboSeq.Name = "cboSeq";
            this.cboSeq.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboSeq.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboSeq.Size = new System.Drawing.Size(66, 24);
            this.cboSeq.TabIndex = 10;
            this.cboSeq.TextChanged += new System.EventHandler(this.cboSeq_TextChanged);
            this.cboSeq.PropBag = resources.GetString("cboSeq.PropBag");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(856, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 19);
            this.label7.TabIndex = 11;
            this.label7.Text = "Sequence";
            // 
            // cboShift
            // 
            this.cboShift.AddItemSeparator = ';';
            this.cboShift.Caption = "";
            this.cboShift.CaptionHeight = 17;
            this.cboShift.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboShift.ColumnCaptionHeight = 17;
            this.cboShift.ColumnFooterHeight = 17;
            this.cboShift.ContentHeight = 18;
            this.cboShift.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboShift.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboShift.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboShift.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboShift.EditorHeight = 18;
            this.cboShift.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboShift.Images.Add(((System.Drawing.Image)(resources.GetObject("cboShift.Images"))));
            this.cboShift.ItemHeight = 15;
            this.cboShift.Location = new System.Drawing.Point(772, 70);
            this.cboShift.MatchEntryTimeout = ((long)(2000));
            this.cboShift.MaxDropDownItems = ((short)(5));
            this.cboShift.MaxLength = 32767;
            this.cboShift.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboShift.Name = "cboShift";
            this.cboShift.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboShift.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboShift.Size = new System.Drawing.Size(66, 24);
            this.cboShift.TabIndex = 9;
            this.cboShift.TextChanged += new System.EventHandler(this.cboShift_TextChanged);
            this.cboShift.PropBag = resources.GetString("cboShift.PropBag");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(690, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Shift";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(688, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Date";
            // 
            // cboItemCheck
            // 
            this.cboItemCheck.AddItemSeparator = ';';
            this.cboItemCheck.Caption = "";
            this.cboItemCheck.CaptionHeight = 17;
            this.cboItemCheck.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboItemCheck.ColumnCaptionHeight = 17;
            this.cboItemCheck.ColumnFooterHeight = 17;
            this.cboItemCheck.ContentHeight = 18;
            this.cboItemCheck.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboItemCheck.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboItemCheck.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboItemCheck.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboItemCheck.EditorHeight = 18;
            this.cboItemCheck.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboItemCheck.Images.Add(((System.Drawing.Image)(resources.GetObject("cboItemCheck.Images"))));
            this.cboItemCheck.ItemHeight = 15;
            this.cboItemCheck.Location = new System.Drawing.Point(772, 10);
            this.cboItemCheck.MatchEntryTimeout = ((long)(2000));
            this.cboItemCheck.MaxDropDownItems = ((short)(5));
            this.cboItemCheck.MaxLength = 32767;
            this.cboItemCheck.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboItemCheck.Name = "cboItemCheck";
            this.cboItemCheck.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboItemCheck.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboItemCheck.Size = new System.Drawing.Size(290, 24);
            this.cboItemCheck.TabIndex = 7;
            this.cboItemCheck.TextChanged += new System.EventHandler(this.cboItemCheck_TextChanged);
            this.cboItemCheck.PropBag = resources.GetString("cboItemCheck.PropBag");
            // 
            // cboLine
            // 
            this.cboLine.AddItemSeparator = ';';
            this.cboLine.Caption = "";
            this.cboLine.CaptionHeight = 17;
            this.cboLine.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboLine.ColumnCaptionHeight = 17;
            this.cboLine.ColumnFooterHeight = 17;
            this.cboLine.ContentHeight = 18;
            this.cboLine.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboLine.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboLine.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLine.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboLine.EditorHeight = 18;
            this.cboLine.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLine.Images.Add(((System.Drawing.Image)(resources.GetObject("cboLine.Images"))));
            this.cboLine.ItemHeight = 15;
            this.cboLine.Location = new System.Drawing.Point(440, 41);
            this.cboLine.MatchEntryTimeout = ((long)(2000));
            this.cboLine.MaxDropDownItems = ((short)(5));
            this.cboLine.MaxLength = 32767;
            this.cboLine.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboLine.Name = "cboLine";
            this.cboLine.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboLine.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboLine.Size = new System.Drawing.Size(232, 24);
            this.cboLine.TabIndex = 5;
            this.cboLine.TextChanged += new System.EventHandler(this.cboLine_TextChanged);
            this.cboLine.PropBag = resources.GetString("cboLine.PropBag");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(319, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Item Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(319, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Machine Process";
            // 
            // cboFactory
            // 
            this.cboFactory.AddItemSeparator = ';';
            this.cboFactory.Caption = "";
            this.cboFactory.CaptionHeight = 17;
            this.cboFactory.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboFactory.ColumnCaptionHeight = 17;
            this.cboFactory.ColumnFooterHeight = 17;
            this.cboFactory.ContentHeight = 18;
            this.cboFactory.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboFactory.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboFactory.EditorFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFactory.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboFactory.EditorHeight = 18;
            this.cboFactory.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFactory.Images.Add(((System.Drawing.Image)(resources.GetObject("cboFactory.Images"))));
            this.cboFactory.ItemHeight = 18;
            this.cboFactory.Location = new System.Drawing.Point(118, 10);
            this.cboFactory.MatchEntryTimeout = ((long)(2000));
            this.cboFactory.MaxDropDownItems = ((short)(5));
            this.cboFactory.MaxLength = 32767;
            this.cboFactory.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboFactory.Name = "cboFactory";
            this.cboFactory.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboFactory.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboFactory.Size = new System.Drawing.Size(188, 24);
            this.cboFactory.TabIndex = 1;
            this.cboFactory.TextChanged += new System.EventHandler(this.cboFactory_TextChanged);
            this.cboFactory.PropBag = resources.GetString("cboFactory.PropBag");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(688, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Item Check";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Factory Code";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1208, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(45, 21);
            this.textBox1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgress,
            this.stUser,
            this.stMsg,
            this.stServer});
            this.statusStrip1.Location = new System.Drawing.Point(0, 625);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1335, 25);
            this.statusStrip1.TabIndex = 93;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsProgress
            // 
            this.tsProgress.BackColor = System.Drawing.Color.White;
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(0, 20);
            // 
            // stMsg
            // 
            this.stMsg.BackColor = System.Drawing.Color.White;
            this.stMsg.Name = "stMsg";
            this.stMsg.Size = new System.Drawing.Size(1000, 20);
            this.stMsg.Spring = true;
            this.stMsg.Text = "Message";
            // 
            // lblScaleCon
            // 
            this.lblScaleCon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScaleCon.BackColor = System.Drawing.Color.LimeGreen;
            this.lblScaleCon.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScaleCon.ForeColor = System.Drawing.Color.White;
            this.lblScaleCon.Location = new System.Drawing.Point(926, 206);
            this.lblScaleCon.Name = "lblScaleCon";
            this.lblScaleCon.Size = new System.Drawing.Size(355, 37);
            this.lblScaleCon.TabIndex = 97;
            this.lblScaleCon.Text = "Connected";
            this.lblScaleCon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScaleStatus
            // 
            this.lblScaleStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScaleStatus.BackColor = System.Drawing.Color.Red;
            this.lblScaleStatus.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScaleStatus.ForeColor = System.Drawing.Color.White;
            this.lblScaleStatus.Location = new System.Drawing.Point(926, 252);
            this.lblScaleStatus.Name = "lblScaleStatus";
            this.lblScaleStatus.Size = new System.Drawing.Size(355, 49);
            this.lblScaleStatus.TabIndex = 101;
            this.lblScaleStatus.Text = "...";
            this.lblScaleStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtScale
            // 
            this.txtScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScale.BackColor = System.Drawing.Color.LightGray;
            this.txtScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtScale.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScale.ForeColor = System.Drawing.Color.Black;
            this.txtScale.Location = new System.Drawing.Point(926, 494);
            this.txtScale.MaxLength = 12;
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(355, 65);
            this.txtScale.TabIndex = 106;
            this.txtScale.TabStop = false;
            this.txtScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtScale.ReadOnlyChanged += new System.EventHandler(this.txtScale_ReadOnlyChanged);
            this.txtScale.EnabledChanged += new System.EventHandler(this.txtScale_EnabledChanged);
            this.txtScale.TextChanged += new System.EventHandler(this.txtScale_TextChanged);
            this.txtScale.Enter += new System.EventHandler(this.txtScale_Enter);
            this.txtScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtScale_KeyPress);
            // 
            // grid
            // 
            this.grid.AllowEditing = false;
            this.grid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoGenerateColumns = false;
            this.grid.ColumnInfo = resources.GetString("grid.ColumnInfo");
            this.grid.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None;
            this.grid.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grid.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never;
            this.grid.Location = new System.Drawing.Point(12, 206);
            this.grid.Name = "grid";
            this.grid.Rows.Count = 1;
            this.grid.Rows.DefaultSize = 22;
            this.grid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grid.Size = new System.Drawing.Size(899, 353);
            this.grid.StyleInfo = resources.GetString("grid.StyleInfo");
            this.grid.TabIndex = 3;
            this.grid.Click += new System.EventHandler(this.grid_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.cboFactory);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboProcess);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cboLineGroup);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cboProcessGroup);
            this.panel1.Controls.Add(this.cboLine);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cboItemCheck);
            this.panel1.Controls.Add(this.cboType);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cboReg);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.cboShift);
            this.panel1.Controls.Add(this.dtProd);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cboSeq);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1311, 107);
            this.panel1.TabIndex = 0;
            // 
            // rtfTerminal
            // 
            this.rtfTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfTerminal.BackColor = System.Drawing.Color.LightGray;
            this.rtfTerminal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtfTerminal.Location = new System.Drawing.Point(926, 355);
            this.rtfTerminal.Name = "rtfTerminal";
            this.rtfTerminal.ReadOnly = true;
            this.rtfTerminal.Size = new System.Drawing.Size(355, 133);
            this.rtfTerminal.TabIndex = 108;
            this.rtfTerminal.Text = "";
            // 
            // lblArg
            // 
            this.lblArg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblArg.AutoSize = true;
            this.lblArg.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArg.ForeColor = System.Drawing.Color.Gray;
            this.lblArg.Location = new System.Drawing.Point(134, 565);
            this.lblArg.Name = "lblArg";
            this.lblArg.Size = new System.Drawing.Size(75, 17);
            this.lblArg.TabIndex = 109;
            this.lblArg.Text = "Arguments";
            this.lblArg.Visible = false;
            // 
            // pnlValue
            // 
            this.pnlValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlValue.Controls.Add(this.opt2);
            this.pnlValue.Controls.Add(this.opt1);
            this.pnlValue.Location = new System.Drawing.Point(348, 138);
            this.pnlValue.Name = "pnlValue";
            this.pnlValue.Size = new System.Drawing.Size(211, 52);
            this.pnlValue.TabIndex = 110;
            this.pnlValue.Visible = false;
            // 
            // opt2
            // 
            this.opt2.Appearance = System.Windows.Forms.Appearance.Button;
            this.opt2.BackColor = System.Drawing.SystemColors.Control;
            this.opt2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opt2.Location = new System.Drawing.Point(108, 3);
            this.opt2.Name = "opt2";
            this.opt2.Size = new System.Drawing.Size(97, 45);
            this.opt2.TabIndex = 1;
            this.opt2.TabStop = true;
            this.opt2.Text = "Value 2";
            this.opt2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.opt2.UseVisualStyleBackColor = false;
            this.opt2.CheckedChanged += new System.EventHandler(this.opt2_CheckedChanged);
            // 
            // opt1
            // 
            this.opt1.Appearance = System.Windows.Forms.Appearance.Button;
            this.opt1.BackColor = System.Drawing.SystemColors.Control;
            this.opt1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opt1.Location = new System.Drawing.Point(3, 3);
            this.opt1.Name = "opt1";
            this.opt1.Size = new System.Drawing.Size(97, 45);
            this.opt1.TabIndex = 0;
            this.opt1.TabStop = true;
            this.opt1.Text = "Value 1";
            this.opt1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.opt1.UseVisualStyleBackColor = false;
            this.opt1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // btnRead
            // 
            this.btnRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRead.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRead.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRead.Location = new System.Drawing.Point(626, 565);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(163, 49);
            this.btnRead.TabIndex = 111;
            this.btnRead.Text = "Read Data (space)";
            this.btnRead.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Image = global::SPCMeasurement.Properties.Resources.cross;
            this.btnClear.Location = new System.Drawing.Point(1199, 319);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(82, 30);
            this.btnClear.TabIndex = 107;
            this.btnClear.Text = "Clear";
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Image = global::SPCMeasurement.Properties.Resources.search;
            this.btnSearch.Location = new System.Drawing.Point(1160, 45);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(103, 49);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Image = global::SPCMeasurement.Properties.Resources.print1;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(795, 565);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(116, 49);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfig.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnConfig.Image")));
            this.btnConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfig.Location = new System.Drawing.Point(926, 565);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(133, 49);
            this.btnConfig.TabIndex = 6;
            this.btnConfig.Text = "Configuration";
            this.btnConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Visible = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::SPCMeasurement.Properties.Resources.disk;
            this.btnSave.Location = new System.Drawing.Point(1111, 319);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Test";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // stUser
            // 
            this.stUser.AutoSize = false;
            this.stUser.BackColor = System.Drawing.SystemColors.Control;
            this.stUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stUser.Image = global::SPCMeasurement.Properties.Resources.user;
            this.stUser.Name = "stUser";
            this.stUser.Size = new System.Drawing.Size(120, 20);
            this.stUser.Text = "User";
            this.stUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stServer
            // 
            this.stServer.AutoSize = false;
            this.stServer.BackColor = System.Drawing.SystemColors.Control;
            this.stServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stServer.Image = global::SPCMeasurement.Properties.Resources.server_connect;
            this.stServer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.stServer.Name = "stServer";
            this.stServer.Size = new System.Drawing.Size(200, 20);
            this.stServer.Text = "Server";
            this.stServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(12, 565);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 49);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Exit";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(163, 138);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(145, 49);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop Reading";
            this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStart.Location = new System.Drawing.Point(12, 138);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(145, 49);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start Reading";
            this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // frmMeasurement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1335, 650);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.pnlValue);
            this.Controls.Add(this.lblArg);
            this.Controls.Add(this.rtfTerminal);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtScale);
            this.Controls.Add(this.lblScaleStatus);
            this.Controls.Add(this.lblScaleCon);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMeasurement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SPC Measurement Process";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMeasurement_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.frmMeasurement_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMeasurement_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmMeasurement_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.cboProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLineGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSeq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboItemCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFactory)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlValue.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsProgress;
        private System.Windows.Forms.ToolStripStatusLabel stMsg;
        private System.Windows.Forms.Timer timerCurr;
        private System.Windows.Forms.Label lblScaleCon;
        private System.Windows.Forms.Label lblScaleStatus;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1List.C1Combo cboFactory;
        private System.Windows.Forms.Label label2;
        private C1.Win.C1List.C1Combo cboItemCheck;
        private C1.Win.C1List.C1Combo cboLine;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private C1.Win.C1List.C1Combo cboSeq;
        private System.Windows.Forms.Label label7;
        private C1.Win.C1List.C1Combo cboShift;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtProd;
        private System.Windows.Forms.Label label8;
        private C1.Win.C1List.C1Combo cboReg;
        private C1.Win.C1List.C1Combo cboType;
        private C1.Win.C1List.C1Combo cboProcessGroup;
        private System.Windows.Forms.Label label9;
        private C1.Win.C1List.C1Combo cboLineGroup;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private C1.Win.C1List.C1Combo cboProcess;
        private C1.Win.C1FlexGrid.C1FlexGrid grid;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripStatusLabel stServer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripStatusLabel stUser;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox rtfTerminal;
        private System.Windows.Forms.Label lblArg;
        private System.Windows.Forms.Panel pnlValue;
        private System.Windows.Forms.RadioButton opt1;
        private System.Windows.Forms.RadioButton opt2;
        private System.Windows.Forms.Button btnRead;
    }
}

