
namespace SPCMeasurement
{
    partial class FrmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.txtStableTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboStopBits = new System.Windows.Forms.ComboBox();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.cboDataBits = new System.Windows.Forms.ComboBox();
            this.cboBaud = new System.Windows.Forms.ComboBox();
            this.cboCOM = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtSQLCmdTimeOut = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.txtSQLDBTimeOut = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.chkViewPassSQL = new System.Windows.Forms.CheckBox();
            this.cboSQLAuth = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSQLPort = new System.Windows.Forms.TextBox();
            this.txtSQLPwd = new System.Windows.Forms.TextBox();
            this.txtSQLUser = new System.Windows.Forms.TextBox();
            this.txtSQLDB = new System.Windows.Forms.TextBox();
            this.txtSQLHost = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.chkRTS = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCOM = new System.Windows.Forms.Button();
            this.btnSQLTest = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 25);
            this.label1.TabIndex = 31;
            this.label1.Text = "Application Configuration";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(10, 55);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(563, 285);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkRTS);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.txtStableTime);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.cboStopBits);
            this.tabPage1.Controls.Add(this.cboParity);
            this.tabPage1.Controls.Add(this.cboDataBits);
            this.tabPage1.Controls.Add(this.cboBaud);
            this.tabPage1.Controls.Add(this.cboCOM);
            this.tabPage1.Controls.Add(this.label28);
            this.tabPage1.Controls.Add(this.btnCOM);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(555, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Device";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(191, 224);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(46, 13);
            this.label17.TabIndex = 37;
            this.label17.Text = "seconds";
            // 
            // txtStableTime
            // 
            this.txtStableTime.Location = new System.Drawing.Point(142, 221);
            this.txtStableTime.Name = "txtStableTime";
            this.txtStableTime.Size = new System.Drawing.Size(43, 21);
            this.txtStableTime.TabIndex = 6;
            this.txtStableTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtStableTime.TextChanged += new System.EventHandler(this.txtStableTime_TextChanged);
            this.txtStableTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStableTime_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Stable Condition";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label4.Location = new System.Drawing.Point(19, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Weight Scale Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label2.Location = new System.Drawing.Point(19, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "COM Serials Settings";
            // 
            // cboStopBits
            // 
            this.cboStopBits.FormattingEnabled = true;
            this.cboStopBits.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cboStopBits.Location = new System.Drawing.Point(142, 146);
            this.cboStopBits.Name = "cboStopBits";
            this.cboStopBits.Size = new System.Drawing.Size(119, 21);
            this.cboStopBits.TabIndex = 4;
            // 
            // cboParity
            // 
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd"});
            this.cboParity.Location = new System.Drawing.Point(142, 120);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new System.Drawing.Size(119, 21);
            this.cboParity.TabIndex = 3;
            // 
            // cboDataBits
            // 
            this.cboDataBits.FormattingEnabled = true;
            this.cboDataBits.Items.AddRange(new object[] {
            "7",
            "8",
            "9"});
            this.cboDataBits.Location = new System.Drawing.Point(142, 94);
            this.cboDataBits.Name = "cboDataBits";
            this.cboDataBits.Size = new System.Drawing.Size(119, 21);
            this.cboDataBits.TabIndex = 2;
            // 
            // cboBaud
            // 
            this.cboBaud.FormattingEnabled = true;
            this.cboBaud.Items.AddRange(new object[] {
            "150",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboBaud.Location = new System.Drawing.Point(142, 68);
            this.cboBaud.Name = "cboBaud";
            this.cboBaud.Size = new System.Drawing.Size(119, 21);
            this.cboBaud.TabIndex = 1;
            // 
            // cboCOM
            // 
            this.cboCOM.FormattingEnabled = true;
            this.cboCOM.Location = new System.Drawing.Point(142, 42);
            this.cboCOM.Name = "cboCOM";
            this.cboCOM.Size = new System.Drawing.Size(119, 21);
            this.cboCOM.TabIndex = 0;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(38, 149);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(49, 13);
            this.label28.TabIndex = 14;
            this.label28.Text = "Stop Bits";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Parity";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Data Bits";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Baud Rate";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "COM Port";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label19);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.txtSQLCmdTimeOut);
            this.tabPage2.Controls.Add(this.label30);
            this.tabPage2.Controls.Add(this.txtSQLDBTimeOut);
            this.tabPage2.Controls.Add(this.label31);
            this.tabPage2.Controls.Add(this.chkViewPassSQL);
            this.tabPage2.Controls.Add(this.cboSQLAuth);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.btnSQLTest);
            this.tabPage2.Controls.Add(this.txtSQLPort);
            this.tabPage2.Controls.Add(this.txtSQLPwd);
            this.tabPage2.Controls.Add(this.txtSQLUser);
            this.tabPage2.Controls.Add(this.txtSQLDB);
            this.tabPage2.Controls.Add(this.txtSQLHost);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(555, 259);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Database";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(167, 188);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 13);
            this.label19.TabIndex = 34;
            this.label19.Text = "seconds";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(167, 161);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(46, 13);
            this.label18.TabIndex = 33;
            this.label18.Text = "seconds";
            // 
            // txtSQLCmdTimeOut
            // 
            this.txtSQLCmdTimeOut.Location = new System.Drawing.Point(115, 185);
            this.txtSQLCmdTimeOut.Name = "txtSQLCmdTimeOut";
            this.txtSQLCmdTimeOut.Size = new System.Drawing.Size(46, 21);
            this.txtSQLCmdTimeOut.TabIndex = 8;
            this.txtSQLCmdTimeOut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSQLCmdTimeOut_KeyPress);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(16, 188);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(95, 13);
            this.label30.TabIndex = 31;
            this.label30.Text = "Command Timeout";
            // 
            // txtSQLDBTimeOut
            // 
            this.txtSQLDBTimeOut.Location = new System.Drawing.Point(115, 158);
            this.txtSQLDBTimeOut.Name = "txtSQLDBTimeOut";
            this.txtSQLDBTimeOut.Size = new System.Drawing.Size(46, 21);
            this.txtSQLDBTimeOut.TabIndex = 7;
            this.txtSQLDBTimeOut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSQLDBTimeOut_KeyPress);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(16, 161);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(94, 13);
            this.label31.TabIndex = 29;
            this.label31.Text = "Database Timeout";
            // 
            // chkViewPassSQL
            // 
            this.chkViewPassSQL.AutoSize = true;
            this.chkViewPassSQL.Location = new System.Drawing.Point(246, 133);
            this.chkViewPassSQL.Name = "chkViewPassSQL";
            this.chkViewPassSQL.Size = new System.Drawing.Size(101, 17);
            this.chkViewPassSQL.TabIndex = 6;
            this.chkViewPassSQL.Text = "Show Password";
            this.chkViewPassSQL.UseVisualStyleBackColor = true;
            this.chkViewPassSQL.CheckedChanged += new System.EventHandler(this.chkViewPassSQL_CheckedChanged);
            // 
            // cboSQLAuth
            // 
            this.cboSQLAuth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSQLAuth.FormattingEnabled = true;
            this.cboSQLAuth.Location = new System.Drawing.Point(115, 74);
            this.cboSQLAuth.Name = "cboSQLAuth";
            this.cboSQLAuth.Size = new System.Drawing.Size(228, 21);
            this.cboSQLAuth.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 77);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 13);
            this.label16.TabIndex = 26;
            this.label16.Text = "Auth. Type";
            // 
            // txtSQLPort
            // 
            this.txtSQLPort.Location = new System.Drawing.Point(411, 17);
            this.txtSQLPort.Name = "txtSQLPort";
            this.txtSQLPort.Size = new System.Drawing.Size(100, 21);
            this.txtSQLPort.TabIndex = 1;
            this.txtSQLPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSQLPort_KeyPress);
            // 
            // txtSQLPwd
            // 
            this.txtSQLPwd.Location = new System.Drawing.Point(115, 131);
            this.txtSQLPwd.Name = "txtSQLPwd";
            this.txtSQLPwd.PasswordChar = '*';
            this.txtSQLPwd.Size = new System.Drawing.Size(125, 21);
            this.txtSQLPwd.TabIndex = 5;
            // 
            // txtSQLUser
            // 
            this.txtSQLUser.Location = new System.Drawing.Point(115, 102);
            this.txtSQLUser.Name = "txtSQLUser";
            this.txtSQLUser.Size = new System.Drawing.Size(228, 21);
            this.txtSQLUser.TabIndex = 4;
            // 
            // txtSQLDB
            // 
            this.txtSQLDB.Location = new System.Drawing.Point(115, 47);
            this.txtSQLDB.Name = "txtSQLDB";
            this.txtSQLDB.Size = new System.Drawing.Size(228, 21);
            this.txtSQLDB.TabIndex = 2;
            // 
            // txtSQLHost
            // 
            this.txtSQLHost.Location = new System.Drawing.Point(115, 17);
            this.txtSQLHost.Name = "txtSQLHost";
            this.txtSQLHost.Size = new System.Drawing.Size(228, 21);
            this.txtSQLHost.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 134);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Password";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "User ID";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Database";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(379, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "Port";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(39, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Server";
            // 
            // chkRTS
            // 
            this.chkRTS.AutoSize = true;
            this.chkRTS.Location = new System.Drawing.Point(142, 173);
            this.chkRTS.Name = "chkRTS";
            this.chkRTS.Size = new System.Drawing.Size(45, 17);
            this.chkRTS.TabIndex = 5;
            this.chkRTS.Text = "RTS";
            this.chkRTS.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = global::SPCMeasurement.Properties.Resources.cancel;
            this.btnClose.Location = new System.Drawing.Point(471, 348);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BackColor = System.Drawing.Color.White;
            this.pictureBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.BackgroundImage")));
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.Location = new System.Drawing.Point(192, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(402, 47);
            this.pictureBox3.TabIndex = 35;
            this.pictureBox3.TabStop = false;
            // 
            // btnApply
            // 
            this.btnApply.Image = global::SPCMeasurement.Properties.Resources.accept;
            this.btnApply.Location = new System.Drawing.Point(367, 348);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(98, 30);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(-4, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(598, 47);
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // btnCOM
            // 
            this.btnCOM.Image = global::SPCMeasurement.Properties.Resources.arrow_refresh;
            this.btnCOM.Location = new System.Drawing.Point(281, 42);
            this.btnCOM.Name = "btnCOM";
            this.btnCOM.Size = new System.Drawing.Size(99, 30);
            this.btnCOM.TabIndex = 7;
            this.btnCOM.Text = "Refresh";
            this.btnCOM.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCOM.UseVisualStyleBackColor = true;
            this.btnCOM.Click += new System.EventHandler(this.btnCOM_Click);
            // 
            // btnSQLTest
            // 
            this.btnSQLTest.Image = global::SPCMeasurement.Properties.Resources.server_connect;
            this.btnSQLTest.Location = new System.Drawing.Point(409, 217);
            this.btnSQLTest.Name = "btnSQLTest";
            this.btnSQLTest.Size = new System.Drawing.Size(129, 32);
            this.btnSQLTest.TabIndex = 9;
            this.btnSQLTest.Text = "Test Connection";
            this.btnSQLTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSQLTest.UseVisualStyleBackColor = true;
            this.btnSQLTest.Click += new System.EventHandler(this.btnSQLTest_Click);
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(585, 390);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Configuration";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cboSQLAuth;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnSQLTest;
        private System.Windows.Forms.TextBox txtSQLPort;
        private System.Windows.Forms.TextBox txtSQLPwd;
        private System.Windows.Forms.TextBox txtSQLUser;
        private System.Windows.Forms.TextBox txtSQLDB;
        private System.Windows.Forms.TextBox txtSQLHost;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkViewPassSQL;
        private System.Windows.Forms.TextBox txtSQLCmdTimeOut;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox txtSQLDBTimeOut;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button btnCOM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboStopBits;
        private System.Windows.Forms.ComboBox cboParity;
        private System.Windows.Forms.ComboBox cboDataBits;
        private System.Windows.Forms.ComboBox cboBaud;
        private System.Windows.Forms.ComboBox cboCOM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtStableTime;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.CheckBox chkRTS;
    }
}