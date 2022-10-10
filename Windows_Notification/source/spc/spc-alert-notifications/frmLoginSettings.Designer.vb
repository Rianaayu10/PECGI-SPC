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
        Me.TabControl1.SuspendLayout()
        Me.tabLogin.SuspendLayout()
        Me.tabDB.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnSave.ForeColor = System.Drawing.SystemColors.Control
        Me.btnSave.Location = New System.Drawing.Point(76, 304)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(271, 38)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "&SAVE"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Red
        Me.btnExit.FlatAppearance.BorderSize = 0
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnExit.ForeColor = System.Drawing.SystemColors.Control
        Me.btnExit.Location = New System.Drawing.Point(76, 348)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(271, 38)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "&EXIT"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'lblcopyright
        '
        Me.lblcopyright.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblcopyright.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblcopyright.Location = New System.Drawing.Point(0, 424)
        Me.lblcopyright.Name = "lblcopyright"
        Me.lblcopyright.Size = New System.Drawing.Size(439, 26)
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
        Me.lblVersion.Size = New System.Drawing.Size(439, 20)
        Me.lblVersion.TabIndex = 10
        Me.lblVersion.Text = "version"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label1.Location = New System.Drawing.Point(0, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(439, 31)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "APPLICATION SETTINGS"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblMsg
        '
        Me.lblMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblMsg.Location = New System.Drawing.Point(0, 393)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(439, 31)
        Me.lblMsg.TabIndex = 11
        Me.lblMsg.Text = "Message!"
        Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabLogin)
        Me.TabControl1.Controls.Add(Me.tabDB)
        Me.TabControl1.Location = New System.Drawing.Point(0, 56)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(439, 242)
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
        Me.tabLogin.Location = New System.Drawing.Point(4, 24)
        Me.tabLogin.Name = "tabLogin"
        Me.tabLogin.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLogin.Size = New System.Drawing.Size(431, 214)
        Me.tabLogin.TabIndex = 0
        Me.tabLogin.Text = "Login"
        Me.tabLogin.UseVisualStyleBackColor = True
        '
        'txtLink
        '
        Me.txtLink.Location = New System.Drawing.Point(72, 108)
        Me.txtLink.Name = "txtLink"
        Me.txtLink.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtLink.PlaceholderText = "http://192.168.0.8:8080/"
        Me.txtLink.Size = New System.Drawing.Size(271, 23)
        Me.txtLink.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label8.Location = New System.Drawing.Point(70, 90)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(87, 15)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "LINK SPC WEB"
        '
        'chkStartUp
        '
        Me.chkStartUp.AutoSize = True
        Me.chkStartUp.Location = New System.Drawing.Point(72, 135)
        Me.chkStartUp.Name = "chkStartUp"
        Me.chkStartUp.Size = New System.Drawing.Size(165, 19)
        Me.chkStartUp.TabIndex = 17
        Me.chkStartUp.Text = "Set as Start Up Application"
        Me.chkStartUp.UseVisualStyleBackColor = True
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(72, 64)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(271, 23)
        Me.txtPassword.TabIndex = 16
        '
        'txtUserID
        '
        Me.txtUserID.Location = New System.Drawing.Point(72, 20)
        Me.txtUserID.MaxLength = 50
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.Size = New System.Drawing.Size(271, 23)
        Me.txtUserID.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label3.Location = New System.Drawing.Point(70, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 15)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "PASSWORD"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label2.Location = New System.Drawing.Point(70, 4)
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
        Me.tabDB.Location = New System.Drawing.Point(4, 24)
        Me.tabDB.Name = "tabDB"
        Me.tabDB.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDB.Size = New System.Drawing.Size(431, 214)
        Me.tabDB.TabIndex = 1
        Me.tabDB.Text = "Database"
        Me.tabDB.UseVisualStyleBackColor = True
        '
        'btnTestConnect
        '
        Me.btnTestConnect.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnTestConnect.FlatAppearance.BorderSize = 0
        Me.btnTestConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTestConnect.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnTestConnect.ForeColor = System.Drawing.SystemColors.Control
        Me.btnTestConnect.Location = New System.Drawing.Point(304, 184)
        Me.btnTestConnect.Name = "btnTestConnect"
        Me.btnTestConnect.Size = New System.Drawing.Size(124, 27)
        Me.btnTestConnect.TabIndex = 23
        Me.btnTestConnect.Text = "&Check Connection"
        Me.btnTestConnect.UseVisualStyleBackColor = False
        '
        'txtDBPassword
        '
        Me.txtDBPassword.Location = New System.Drawing.Point(72, 155)
        Me.txtDBPassword.Name = "txtDBPassword"
        Me.txtDBPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtDBPassword.Size = New System.Drawing.Size(271, 23)
        Me.txtDBPassword.TabIndex = 22
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label6.Location = New System.Drawing.Point(70, 136)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 15)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "PASSWORD"
        '
        'txtDBUserID
        '
        Me.txtDBUserID.Location = New System.Drawing.Point(72, 109)
        Me.txtDBUserID.Name = "txtDBUserID"
        Me.txtDBUserID.Size = New System.Drawing.Size(271, 23)
        Me.txtDBUserID.TabIndex = 20
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label7.Location = New System.Drawing.Point(70, 93)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 15)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "USER NAME"
        '
        'txtDBDatabase
        '
        Me.txtDBDatabase.Location = New System.Drawing.Point(72, 66)
        Me.txtDBDatabase.Name = "txtDBDatabase"
        Me.txtDBDatabase.Size = New System.Drawing.Size(271, 23)
        Me.txtDBDatabase.TabIndex = 18
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label4.Location = New System.Drawing.Point(70, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 15)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "DATABASE"
        '
        'txtDBServer
        '
        Me.txtDBServer.Location = New System.Drawing.Point(72, 20)
        Me.txtDBServer.Name = "txtDBServer"
        Me.txtDBServer.Size = New System.Drawing.Size(271, 23)
        Me.txtDBServer.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label5.Location = New System.Drawing.Point(70, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 15)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "SERVER "
        '
        'frmLoginSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(439, 450)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.lblMsg)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblcopyright)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmLoginSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.tabLogin.ResumeLayout(False)
        Me.tabLogin.PerformLayout()
        Me.tabDB.ResumeLayout(False)
        Me.tabDB.PerformLayout()
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
End Class
