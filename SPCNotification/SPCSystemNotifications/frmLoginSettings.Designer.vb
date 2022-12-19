<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLoginSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLoginSettings))
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblcopyright = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabLogin = New System.Windows.Forms.TabPage()
        Me.txtLink = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkStartUp = New System.Windows.Forms.CheckBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtUserID = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tabDB = New System.Windows.Forms.TabPage()
        Me.btnTestConnect = New System.Windows.Forms.Button()
        Me.txtDBPassword = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtDBUserID = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtDBDatabase = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDBServer = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tabInterval = New System.Windows.Forms.TabPage()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtInterval = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.tabLogin.SuspendLayout()
        Me.tabDB.SuspendLayout()
        Me.tabInterval.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnSave.ForeColor = System.Drawing.SystemColors.Control
        Me.btnSave.Location = New System.Drawing.Point(65, 263)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(232, 33)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "&SAVE"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Red
        Me.btnExit.FlatAppearance.BorderSize = 0
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnExit.ForeColor = System.Drawing.SystemColors.Control
        Me.btnExit.Location = New System.Drawing.Point(65, 302)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(232, 33)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "&EXIT"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'lblcopyright
        '
        Me.lblcopyright.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblcopyright.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblcopyright.Location = New System.Drawing.Point(0, 367)
        Me.lblcopyright.Name = "lblcopyright"
        Me.lblcopyright.Size = New System.Drawing.Size(376, 23)
        Me.lblcopyright.TabIndex = 9
        Me.lblcopyright.Text = "Copyright"
        Me.lblcopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblVersion
        '
        Me.lblVersion.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblVersion.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblVersion.Location = New System.Drawing.Point(0, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(376, 17)
        Me.lblVersion.TabIndex = 10
        Me.lblVersion.Text = "version"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(0, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(376, 27)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "APPLICATION SETTINGS"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblMsg
        '
        Me.lblMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblMsg.Location = New System.Drawing.Point(0, 341)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(376, 27)
        Me.lblMsg.TabIndex = 11
        Me.lblMsg.Text = "Message!"
        Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabLogin)
        Me.TabControl1.Controls.Add(Me.tabDB)
        Me.TabControl1.Controls.Add(Me.tabInterval)
        Me.TabControl1.Location = New System.Drawing.Point(0, 49)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(376, 210)
        Me.TabControl1.TabIndex = 13
        '
        'tabLogin
        '
        Me.tabLogin.Controls.Add(Me.txtLink)
        Me.tabLogin.Controls.Add(Me.Label8)
        Me.tabLogin.Controls.Add(Me.chkStartUp)
        Me.tabLogin.Controls.Add(Me.txtPassword)
        Me.tabLogin.Controls.Add(Me.txtUserID)
        Me.tabLogin.Controls.Add(Me.Label3)
        Me.tabLogin.Controls.Add(Me.Label2)
        Me.tabLogin.Location = New System.Drawing.Point(4, 22)
        Me.tabLogin.Name = "tabLogin"
        Me.tabLogin.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLogin.Size = New System.Drawing.Size(368, 184)
        Me.tabLogin.TabIndex = 0
        Me.tabLogin.Text = "Login"
        Me.tabLogin.UseVisualStyleBackColor = True
        '
        'txtLink
        '
        Me.txtLink.Location = New System.Drawing.Point(62, 94)
        Me.txtLink.Name = "txtLink"
        Me.txtLink.Size = New System.Drawing.Size(233, 20)
        Me.txtLink.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label8.Location = New System.Drawing.Point(60, 78)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(87, 15)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "LINK SPC WEB"
        '
        'chkStartUp
        '
        Me.chkStartUp.AutoSize = True
        Me.chkStartUp.Location = New System.Drawing.Point(62, 117)
        Me.chkStartUp.Name = "chkStartUp"
        Me.chkStartUp.Size = New System.Drawing.Size(153, 17)
        Me.chkStartUp.TabIndex = 17
        Me.chkStartUp.Text = "Set as Start Up Application"
        Me.chkStartUp.UseVisualStyleBackColor = True
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(62, 55)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(233, 20)
        Me.txtPassword.TabIndex = 16
        '
        'txtUserID
        '
        Me.txtUserID.Location = New System.Drawing.Point(62, 17)
        Me.txtUserID.MaxLength = 50
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.Size = New System.Drawing.Size(233, 20)
        Me.txtUserID.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(60, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 15)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "PASSWORD"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(60, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 15)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "USER ID"
        '
        'tabDB
        '
        Me.tabDB.Controls.Add(Me.btnTestConnect)
        Me.tabDB.Controls.Add(Me.txtDBPassword)
        Me.tabDB.Controls.Add(Me.Label6)
        Me.tabDB.Controls.Add(Me.txtDBUserID)
        Me.tabDB.Controls.Add(Me.Label7)
        Me.tabDB.Controls.Add(Me.txtDBDatabase)
        Me.tabDB.Controls.Add(Me.Label4)
        Me.tabDB.Controls.Add(Me.txtDBServer)
        Me.tabDB.Controls.Add(Me.Label5)
        Me.tabDB.Location = New System.Drawing.Point(4, 22)
        Me.tabDB.Name = "tabDB"
        Me.tabDB.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDB.Size = New System.Drawing.Size(368, 184)
        Me.tabDB.TabIndex = 1
        Me.tabDB.Text = "Database"
        Me.tabDB.UseVisualStyleBackColor = True
        '
        'btnTestConnect
        '
        Me.btnTestConnect.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnTestConnect.FlatAppearance.BorderSize = 0
        Me.btnTestConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTestConnect.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnTestConnect.ForeColor = System.Drawing.SystemColors.Control
        Me.btnTestConnect.Location = New System.Drawing.Point(261, 159)
        Me.btnTestConnect.Name = "btnTestConnect"
        Me.btnTestConnect.Size = New System.Drawing.Size(106, 23)
        Me.btnTestConnect.TabIndex = 23
        Me.btnTestConnect.Text = "&Check Connection"
        Me.btnTestConnect.UseVisualStyleBackColor = False
        '
        'txtDBPassword
        '
        Me.txtDBPassword.Location = New System.Drawing.Point(62, 134)
        Me.txtDBPassword.Name = "txtDBPassword"
        Me.txtDBPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtDBPassword.Size = New System.Drawing.Size(233, 20)
        Me.txtDBPassword.TabIndex = 22
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.Location = New System.Drawing.Point(60, 118)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 15)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "PASSWORD"
        '
        'txtDBUserID
        '
        Me.txtDBUserID.Location = New System.Drawing.Point(62, 94)
        Me.txtDBUserID.Name = "txtDBUserID"
        Me.txtDBUserID.Size = New System.Drawing.Size(233, 20)
        Me.txtDBUserID.TabIndex = 20
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(60, 81)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 15)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "USER NAME"
        '
        'txtDBDatabase
        '
        Me.txtDBDatabase.Location = New System.Drawing.Point(62, 57)
        Me.txtDBDatabase.Name = "txtDBDatabase"
        Me.txtDBDatabase.Size = New System.Drawing.Size(233, 20)
        Me.txtDBDatabase.TabIndex = 18
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(60, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 15)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "DATABASE"
        '
        'txtDBServer
        '
        Me.txtDBServer.Location = New System.Drawing.Point(62, 17)
        Me.txtDBServer.Name = "txtDBServer"
        Me.txtDBServer.Size = New System.Drawing.Size(233, 20)
        Me.txtDBServer.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(60, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 15)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "SERVER "
        '
        'tabInterval
        '
        Me.tabInterval.Controls.Add(Me.Label10)
        Me.tabInterval.Controls.Add(Me.txtInterval)
        Me.tabInterval.Controls.Add(Me.Label9)
        Me.tabInterval.Location = New System.Drawing.Point(4, 22)
        Me.tabInterval.Name = "tabInterval"
        Me.tabInterval.Padding = New System.Windows.Forms.Padding(3)
        Me.tabInterval.Size = New System.Drawing.Size(368, 184)
        Me.tabInterval.TabIndex = 2
        Me.tabInterval.Text = "Intercal Refresh"
        Me.tabInterval.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(93, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(50, 15)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "MInutes"
        '
        'txtInterval
        '
        Me.txtInterval.Location = New System.Drawing.Point(62, 17)
        Me.txtInterval.MaxLength = 3
        Me.txtInterval.Name = "txtInterval"
        Me.txtInterval.Size = New System.Drawing.Size(30, 20)
        Me.txtInterval.TabIndex = 17
        Me.txtInterval.Text = "1"
        Me.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label9.Location = New System.Drawing.Point(60, 3)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(46, 15)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Inerval"
        '
        'frmLoginSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(376, 390)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.lblMsg)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblcopyright)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLoginSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Application Settings"
        Me.TabControl1.ResumeLayout(False)
        Me.tabLogin.ResumeLayout(False)
        Me.tabLogin.PerformLayout()
        Me.tabDB.ResumeLayout(False)
        Me.tabDB.PerformLayout()
        Me.tabInterval.ResumeLayout(False)
        Me.tabInterval.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpfooter As GroupBox
    Friend WithEvents lblcopyright As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents lblVersion As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblMsg As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tabLogin As TabPage
    Friend WithEvents tabDB As TabPage
    Friend WithEvents chkStartUp As CheckBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtUserID As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtDBServer As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtDBDatabase As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDBPassword As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtDBUserID As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents btnTestConnect As Button
    Friend WithEvents txtLink As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents tabInterval As System.Windows.Forms.TabPage
    Friend WithEvents txtInterval As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class
