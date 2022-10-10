<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSPCAlertDashboard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSPCAlertDashboard))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboFactory = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.gridDelayInput = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.gridNGResult = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridDelayInput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.gridNGResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cboFactory)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.lblTime)
        Me.Panel1.Controls.Add(Me.lblDate)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1074, 68)
        Me.Panel1.TabIndex = 1
        '
        'cboFactory
        '
        Me.cboFactory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboFactory.FormattingEnabled = True
        Me.cboFactory.Location = New System.Drawing.Point(103, 20)
        Me.cboFactory.Name = "cboFactory"
        Me.cboFactory.Size = New System.Drawing.Size(121, 23)
        Me.cboFactory.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 15)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Factory"
        '
        'lblTime
        '
        Me.lblTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTime.AutoSize = True
        Me.lblTime.Location = New System.Drawing.Point(953, 34)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(49, 15)
        Me.lblTime.TabIndex = 3
        Me.lblTime.Text = "00:00:00"
        '
        'lblDate
        '
        Me.lblDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDate.AutoSize = True
        Me.lblDate.Location = New System.Drawing.Point(953, 12)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(84, 15)
        Me.lblDate.TabIndex = 2
        Me.lblDate.Text = "dd MMM yyyy"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(907, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Time :  "
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(907, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date  : "
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label15)
        Me.Panel2.Controls.Add(Me.Label14)
        Me.Panel2.Controls.Add(Me.Label13)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.gridDelayInput)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Location = New System.Drawing.Point(26, 106)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1023, 250)
        Me.Panel2.TabIndex = 42
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(214, 230)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(108, 15)
        Me.Label15.TabIndex = 46
        Me.Label15.Text = "Delay > 60 Minutes"
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Red
        Me.Label14.Location = New System.Drawing.Point(166, 226)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(46, 23)
        Me.Label14.TabIndex = 45
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(48, 230)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(108, 15)
        Me.Label13.TabIndex = 44
        Me.Label13.Text = "Delay < 60 Minutes"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Yellow
        Me.Label12.Location = New System.Drawing.Point(-1, 225)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(46, 23)
        Me.Label12.TabIndex = 43
        '
        'gridDelayInput
        '
        Me.gridDelayInput.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.gridDelayInput.AllowEditing = False
        Me.gridDelayInput.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.gridDelayInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridDelayInput.AutoClipboard = True
        Me.gridDelayInput.BackColor = System.Drawing.Color.White
        Me.gridDelayInput.ColumnInfo = "3,0,0,0,0,100,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridDelayInput.ExtendLastCol = True
        Me.gridDelayInput.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridDelayInput.Location = New System.Drawing.Point(-1, 0)
        Me.gridDelayInput.Name = "gridDelayInput"
        Me.gridDelayInput.Rows.Count = 5
        Me.gridDelayInput.Rows.DefaultSize = 20
        Me.gridDelayInput.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridDelayInput.Size = New System.Drawing.Size(1025, 203)
        Me.gridDelayInput.StyleInfo = resources.GetString("gridDelayInput.StyleInfo")
        Me.gridDelayInput.TabIndex = 42
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1791, 34)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 15)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "00:00:00"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(1791, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 15)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "dd MMM yyyy"
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(1745, 34)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 15)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Time :  "
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(1745, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 15)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Date  : "
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Label16)
        Me.Panel3.Controls.Add(Me.Label17)
        Me.Panel3.Controls.Add(Me.Label18)
        Me.Panel3.Controls.Add(Me.Label19)
        Me.Panel3.Controls.Add(Me.gridNGResult)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Location = New System.Drawing.Point(25, 371)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1023, 250)
        Me.Panel3.TabIndex = 43
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(225, 229)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(143, 15)
        Me.Label16.TabIndex = 50
        Me.Label16.Text = "Out of Specification Value"
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.Red
        Me.Label17.Location = New System.Drawing.Point(177, 225)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(46, 23)
        Me.Label17.TabIndex = 49
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(49, 229)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(115, 15)
        Me.Label18.TabIndex = 48
        Me.Label18.Text = "Out of Control Value"
        '
        'Label19
        '
        Me.Label19.BackColor = System.Drawing.Color.Yellow
        Me.Label19.Location = New System.Drawing.Point(0, 224)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(46, 23)
        Me.Label19.TabIndex = 47
        '
        'gridNGResult
        '
        Me.gridNGResult.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.gridNGResult.AllowEditing = False
        Me.gridNGResult.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.gridNGResult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridNGResult.AutoClipboard = True
        Me.gridNGResult.BackColor = System.Drawing.Color.White
        Me.gridNGResult.ColumnInfo = "3,0,0,0,0,100,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridNGResult.ExtendLastCol = True
        Me.gridNGResult.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridNGResult.Location = New System.Drawing.Point(-1, 0)
        Me.gridNGResult.Name = "gridNGResult"
        Me.gridNGResult.Rows.Count = 5
        Me.gridNGResult.Rows.DefaultSize = 20
        Me.gridNGResult.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridNGResult.Size = New System.Drawing.Size(1025, 203)
        Me.gridNGResult.StyleInfo = resources.GetString("gridNGResult.StyleInfo")
        Me.gridNGResult.TabIndex = 42
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2585, 34)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 15)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "00:00:00"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(2585, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 15)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "dd MMM yyyy"
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(2539, 34)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(45, 15)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Time :  "
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(2539, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(43, 15)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Date  : "
        '
        'frmSPCAlertDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ClientSize = New System.Drawing.Size(1074, 641)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmSPCAlertDashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmSPCAlertDashboard"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.gridDelayInput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.gridNGResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblTime As Label
    Friend WithEvents lblDate As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cboFactory As ComboBox
    Friend WithEvents Panel2 As Panel
    Public WithEvents gridDelayInput As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Panel3 As Panel
    Public WithEvents gridNGResult As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label4 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label19 As Label
End Class
