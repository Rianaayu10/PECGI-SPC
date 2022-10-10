<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSPCInboxNotification
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSPCInboxNotification))
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboFactory = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.gridDelayVerification = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.gridDelayInput = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.gridNGResult = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlHeader.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.gridDelayVerification, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.gridDelayInput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.gridNGResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlHeader.Controls.Add(Me.lblTime)
        Me.pnlHeader.Controls.Add(Me.lblDate)
        Me.pnlHeader.Controls.Add(Me.Label3)
        Me.pnlHeader.Controls.Add(Me.Label2)
        Me.pnlHeader.Controls.Add(Me.cboFactory)
        Me.pnlHeader.Controls.Add(Me.Label1)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(980, 74)
        Me.pnlHeader.TabIndex = 0
        '
        'lblTime
        '
        Me.lblTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lblTime.Location = New System.Drawing.Point(855, 37)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(67, 16)
        Me.lblTime.TabIndex = 5
        Me.lblTime.Text = "HH:mm:ss"
        '
        'lblDate
        '
        Me.lblDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lblDate.Location = New System.Drawing.Point(855, 15)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(83, 16)
        Me.lblDate.TabIndex = 4
        Me.lblDate.Text = "dd MMM yyyy"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Label3.Location = New System.Drawing.Point(815, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Time :"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Label2.Location = New System.Drawing.Point(815, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Date  :"
        '
        'cboFactory
        '
        Me.cboFactory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboFactory.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.cboFactory.FormattingEnabled = True
        Me.cboFactory.Location = New System.Drawing.Point(68, 24)
        Me.cboFactory.Name = "cboFactory"
        Me.cboFactory.Size = New System.Drawing.Size(121, 24)
        Me.cboFactory.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Label1.Location = New System.Drawing.Point(12, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Factory"
        '
        'pnlContent
        '
        Me.pnlContent.Controls.Add(Me.Panel3)
        Me.pnlContent.Controls.Add(Me.Label18)
        Me.pnlContent.Controls.Add(Me.gridDelayVerification)
        Me.pnlContent.Controls.Add(Me.Panel2)
        Me.pnlContent.Controls.Add(Me.Label13)
        Me.pnlContent.Controls.Add(Me.gridDelayInput)
        Me.pnlContent.Controls.Add(Me.Panel1)
        Me.pnlContent.Controls.Add(Me.Label4)
        Me.pnlContent.Controls.Add(Me.gridNGResult)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.pnlContent.Location = New System.Drawing.Point(0, 74)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(980, 615)
        Me.pnlContent.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.Label15)
        Me.Panel3.Controls.Add(Me.Label16)
        Me.Panel3.Controls.Add(Me.Label17)
        Me.Panel3.Location = New System.Drawing.Point(3, 560)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(974, 25)
        Me.Panel3.TabIndex = 49
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(215, 6)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(96, 13)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "Delay > 60 Minutes"
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.Red
        Me.Label15.Location = New System.Drawing.Point(166, 1)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(45, 23)
        Me.Label15.TabIndex = 2
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(61, 6)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(96, 13)
        Me.Label16.TabIndex = 1
        Me.Label16.Text = "Delay < 60 Minutes"
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.Yellow
        Me.Label17.Location = New System.Drawing.Point(12, 1)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(45, 23)
        Me.Label17.TabIndex = 0
        '
        'Label18
        '
        Me.Label18.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.BackColor = System.Drawing.Color.DimGray
        Me.Label18.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label18.ForeColor = System.Drawing.SystemColors.Window
        Me.Label18.Location = New System.Drawing.Point(3, 412)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(974, 25)
        Me.Label18.TabIndex = 47
        Me.Label18.Text = "Production Sample - Delay Verification"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridDelayVerification
        '
        Me.gridDelayVerification.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.gridDelayVerification.AllowEditing = False
        Me.gridDelayVerification.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.gridDelayVerification.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridDelayVerification.AutoClipboard = True
        Me.gridDelayVerification.BackColor = System.Drawing.Color.White
        Me.gridDelayVerification.ColumnInfo = "3,0,0,0,0,100,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridDelayVerification.ExtendLastCol = True
        Me.gridDelayVerification.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridDelayVerification.Location = New System.Drawing.Point(3, 437)
        Me.gridDelayVerification.Name = "gridDelayVerification"
        Me.gridDelayVerification.Rows.Count = 5
        Me.gridDelayVerification.Rows.DefaultSize = 20
        Me.gridDelayVerification.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridDelayVerification.Size = New System.Drawing.Size(976, 117)
        Me.gridDelayVerification.StyleInfo = resources.GetString("gridDelayVerification.StyleInfo")
        Me.gridDelayVerification.TabIndex = 48
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Location = New System.Drawing.Point(3, 371)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(974, 25)
        Me.Panel2.TabIndex = 46
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(215, 6)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(96, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Delay > 60 Minutes"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(166, 1)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(45, 23)
        Me.Label10.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(61, 6)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(96, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Delay < 60 Minutes"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Yellow
        Me.Label12.Location = New System.Drawing.Point(12, 1)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(45, 23)
        Me.Label12.TabIndex = 0
        '
        'Label13
        '
        Me.Label13.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.BackColor = System.Drawing.Color.DimGray
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label13.ForeColor = System.Drawing.SystemColors.Window
        Me.Label13.Location = New System.Drawing.Point(3, 223)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(974, 25)
        Me.Label13.TabIndex = 44
        Me.Label13.Text = "Production Sample - Delay Input"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        Me.gridDelayInput.Location = New System.Drawing.Point(3, 248)
        Me.gridDelayInput.Name = "gridDelayInput"
        Me.gridDelayInput.Rows.Count = 5
        Me.gridDelayInput.Rows.DefaultSize = 20
        Me.gridDelayInput.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridDelayInput.Size = New System.Drawing.Size(976, 117)
        Me.gridDelayInput.StyleInfo = resources.GetString("gridDelayInput.StyleInfo")
        Me.gridDelayInput.TabIndex = 45
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Location = New System.Drawing.Point(3, 175)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(974, 25)
        Me.Panel1.TabIndex = 43
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(215, 6)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(125, 13)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Out of Specification Value"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(166, 1)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(45, 23)
        Me.Label8.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(61, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Out of Control Value"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Yellow
        Me.Label5.Location = New System.Drawing.Point(12, 1)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 23)
        Me.Label5.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.DimGray
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(3, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(974, 25)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Production Sample - NG Result"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        Me.gridNGResult.Location = New System.Drawing.Point(3, 52)
        Me.gridNGResult.Name = "gridNGResult"
        Me.gridNGResult.Rows.Count = 5
        Me.gridNGResult.Rows.DefaultSize = 20
        Me.gridNGResult.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridNGResult.Size = New System.Drawing.Size(976, 117)
        Me.gridNGResult.StyleInfo = resources.GetString("gridNGResult.StyleInfo")
        Me.gridNGResult.TabIndex = 42
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'frmSPCInboxNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(980, 689)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSPCInboxNotification"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SPC inbox notification"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlContent.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.gridDelayVerification, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.gridDelayInput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridNGResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents pnlContent As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents cboFactory As ComboBox
    Friend WithEvents lblTime As Label
    Friend WithEvents lblDate As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Public WithEvents gridNGResult As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Public WithEvents gridDelayInput As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label18 As Label
    Public WithEvents gridDelayVerification As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Timer1 As Timer
End Class
