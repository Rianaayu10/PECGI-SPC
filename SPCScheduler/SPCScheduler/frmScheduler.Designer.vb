<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmScheduler
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmScheduler))
        Me.lvwLog = New System.Windows.Forms.ListView()
        Me.colTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colMessage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.dtpProcessTime = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblStart = New System.Windows.Forms.Label()
        Me.tmrExecute = New System.Windows.Forms.Timer(Me.components)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.lblVer = New System.Windows.Forms.Label()
        Me.chkNGResult = New System.Windows.Forms.CheckBox()
        Me.chkDelayVerification = New System.Windows.Forms.CheckBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.chkDelayInput = New System.Windows.Forms.CheckBox()
        Me.txtFactory = New System.Windows.Forms.Label()
        Me.cboFactory = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'lvwLog
        '
        Me.lvwLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTime, Me.colMessage})
        Me.lvwLog.FullRowSelect = True
        Me.lvwLog.GridLines = True
        Me.lvwLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvwLog.HideSelection = False
        Me.lvwLog.Location = New System.Drawing.Point(12, 93)
        Me.lvwLog.MultiSelect = False
        Me.lvwLog.Name = "lvwLog"
        Me.lvwLog.Size = New System.Drawing.Size(695, 190)
        Me.lvwLog.TabIndex = 15
        Me.lvwLog.TabStop = False
        Me.lvwLog.UseCompatibleStateImageBehavior = False
        Me.lvwLog.View = System.Windows.Forms.View.Details
        '
        'colTime
        '
        Me.colTime.Text = "Time"
        Me.colTime.Width = 135
        '
        'colMessage
        '
        Me.colMessage.Text = "Message"
        Me.colMessage.Width = 1000
        '
        'dtpProcessTime
        '
        Me.dtpProcessTime.CustomFormat = "HH:mm"
        Me.dtpProcessTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpProcessTime.Location = New System.Drawing.Point(643, 57)
        Me.dtpProcessTime.Margin = New System.Windows.Forms.Padding(1)
        Me.dtpProcessTime.Name = "dtpProcessTime"
        Me.dtpProcessTime.ShowUpDown = True
        Me.dtpProcessTime.Size = New System.Drawing.Size(65, 21)
        Me.dtpProcessTime.TabIndex = 0
        Me.dtpProcessTime.Value = New Date(2020, 2, 11, 0, 0, 0, 0)
        Me.dtpProcessTime.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(626, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(11, 13)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = ":"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label1.Visible = False
        '
        'lblStart
        '
        Me.lblStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStart.BackColor = System.Drawing.Color.Gray
        Me.lblStart.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStart.ForeColor = System.Drawing.Color.White
        Me.lblStart.Location = New System.Drawing.Point(598, 15)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(109, 24)
        Me.lblStart.TabIndex = 43
        Me.lblStart.Text = "STOPPED"
        Me.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tmrExecute
        '
        Me.tmrExecute.Interval = 5000
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(542, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 13)
        Me.Label3.TabIndex = 54
        Me.Label3.Text = "Execute Time"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label3.Visible = False
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.Image = CType(resources.GetObject("btnClear.Image"), System.Drawing.Image)
        Me.btnClear.Location = New System.Drawing.Point(623, 289)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(84, 29)
        Me.btnClear.TabIndex = 45
        Me.btnClear.Text = "Clear Log"
        Me.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Enabled = False
        Me.btnStop.Image = Global.SPCScheduler.My.Resources.Resources.control_stop
        Me.btnStop.Location = New System.Drawing.Point(522, 13)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(74, 26)
        Me.btnStop.TabIndex = 14
        Me.btnStop.Text = "Stop"
        Me.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Image = Global.SPCScheduler.My.Resources.Resources.control_play_blue
        Me.btnStart.Location = New System.Drawing.Point(446, 13)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(74, 26)
        Me.btnStart.TabIndex = 13
        Me.btnStart.Text = "Start"
        Me.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'lblVer
        '
        Me.lblVer.AutoSize = True
        Me.lblVer.ForeColor = System.Drawing.Color.Gray
        Me.lblVer.Location = New System.Drawing.Point(193, 297)
        Me.lblVer.Name = "lblVer"
        Me.lblVer.Size = New System.Drawing.Size(42, 13)
        Me.lblVer.TabIndex = 67
        Me.lblVer.Text = "Version"
        '
        'chkNGResult
        '
        Me.chkNGResult.AutoSize = True
        Me.chkNGResult.Location = New System.Drawing.Point(17, 17)
        Me.chkNGResult.Name = "chkNGResult"
        Me.chkNGResult.Size = New System.Drawing.Size(73, 17)
        Me.chkNGResult.TabIndex = 1
        Me.chkNGResult.Text = "NG Result"
        Me.chkNGResult.UseMnemonic = False
        Me.chkNGResult.UseVisualStyleBackColor = True
        '
        'chkDelayVerification
        '
        Me.chkDelayVerification.AutoSize = True
        Me.chkDelayVerification.Location = New System.Drawing.Point(251, 18)
        Me.chkDelayVerification.Name = "chkDelayVerification"
        Me.chkDelayVerification.Size = New System.Drawing.Size(109, 17)
        Me.chkDelayVerification.TabIndex = 2
        Me.chkDelayVerification.Text = "Delay Verification"
        Me.chkDelayVerification.UseMnemonic = False
        Me.chkDelayVerification.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'chkDelayInput
        '
        Me.chkDelayInput.AutoSize = True
        Me.chkDelayInput.Location = New System.Drawing.Point(131, 18)
        Me.chkDelayInput.Name = "chkDelayInput"
        Me.chkDelayInput.Size = New System.Drawing.Size(82, 17)
        Me.chkDelayInput.TabIndex = 8
        Me.chkDelayInput.Text = "Delay Input"
        Me.chkDelayInput.UseMnemonic = False
        Me.chkDelayInput.UseVisualStyleBackColor = True
        '
        'txtFactory
        '
        Me.txtFactory.AutoSize = True
        Me.txtFactory.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.txtFactory.Location = New System.Drawing.Point(14, 49)
        Me.txtFactory.Name = "txtFactory"
        Me.txtFactory.Size = New System.Drawing.Size(44, 13)
        Me.txtFactory.TabIndex = 68
        Me.txtFactory.Text = "Factory"
        '
        'cboFactory
        '
        Me.cboFactory.FormattingEnabled = True
        Me.cboFactory.Location = New System.Drawing.Point(65, 46)
        Me.cboFactory.Name = "cboFactory"
        Me.cboFactory.Size = New System.Drawing.Size(81, 21)
        Me.cboFactory.TabIndex = 69
        '
        'frmScheduler
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(719, 331)
        Me.Controls.Add(Me.cboFactory)
        Me.Controls.Add(Me.txtFactory)
        Me.Controls.Add(Me.chkDelayInput)
        Me.Controls.Add(Me.chkDelayVerification)
        Me.Controls.Add(Me.chkNGResult)
        Me.Controls.Add(Me.lblVer)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lvwLog)
        Me.Controls.Add(Me.dtpProcessTime)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.lblStart)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnStart)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmScheduler"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SPC Alert Scheduler"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvwLog As ListView
    Friend WithEvents colTime As ColumnHeader
    Friend WithEvents colMessage As ColumnHeader
    Friend WithEvents dtpProcessTime As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents btnClear As Button
    Friend WithEvents lblStart As Label
    Friend WithEvents btnStop As Button
    Friend WithEvents btnStart As Button
    Friend WithEvents tmrExecute As Timer
    Friend WithEvents Label3 As Label
    Friend WithEvents lblVer As System.Windows.Forms.Label
    Friend WithEvents chkNGResult As CheckBox
    Friend WithEvents chkDelayVerification As CheckBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents chkDelayInput As System.Windows.Forms.CheckBox
    Friend WithEvents txtFactory As System.Windows.Forms.Label
    Friend WithEvents cboFactory As System.Windows.Forms.ComboBox
End Class
