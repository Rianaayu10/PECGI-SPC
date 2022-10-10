<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInboxNotifications
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInboxNotifications))
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.pctClose = New System.Windows.Forms.PictureBox()
        Me.tabNotification = New System.Windows.Forms.TabControl()
        Me.tabNGInput = New System.Windows.Forms.TabPage()
        Me.gridNG = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tabDelayInput = New System.Windows.Forms.TabPage()
        Me.gridInput = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tabDelayVerification = New System.Windows.Forms.TabPage()
        Me.gridVerification = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.pctClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabNotification.SuspendLayout()
        Me.tabNGInput.SuspendLayout()
        CType(Me.gridNG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDelayInput.SuspendLayout()
        CType(Me.gridInput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDelayVerification.SuspendLayout()
        CType(Me.gridVerification, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeader
        '
        Me.lblHeader.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblHeader.Location = New System.Drawing.Point(0, 0)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(661, 23)
        Me.lblHeader.TabIndex = 0
        Me.lblHeader.Text = "Inbox Notification Alert"
        Me.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pctClose
        '
        Me.pctClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pctClose.BackColor = System.Drawing.Color.Red
        Me.pctClose.Image = Global.spc_alert_notifications.My.Resources.Resources.close_icon
        Me.pctClose.Location = New System.Drawing.Point(641, 1)
        Me.pctClose.Name = "pctClose"
        Me.pctClose.Size = New System.Drawing.Size(19, 20)
        Me.pctClose.TabIndex = 1
        Me.pctClose.TabStop = False
        '
        'tabNotification
        '
        Me.tabNotification.Controls.Add(Me.tabNGInput)
        Me.tabNotification.Controls.Add(Me.tabDelayInput)
        Me.tabNotification.Controls.Add(Me.tabDelayVerification)
        Me.tabNotification.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabNotification.Location = New System.Drawing.Point(0, 23)
        Me.tabNotification.Name = "tabNotification"
        Me.tabNotification.SelectedIndex = 0
        Me.tabNotification.Size = New System.Drawing.Size(661, 427)
        Me.tabNotification.TabIndex = 2
        '
        'tabNGInput
        '
        Me.tabNGInput.Controls.Add(Me.gridNG)
        Me.tabNGInput.Location = New System.Drawing.Point(4, 24)
        Me.tabNGInput.Name = "tabNGInput"
        Me.tabNGInput.Padding = New System.Windows.Forms.Padding(3)
        Me.tabNGInput.Size = New System.Drawing.Size(653, 399)
        Me.tabNGInput.TabIndex = 0
        Me.tabNGInput.Text = "NG INPUT"
        Me.tabNGInput.UseVisualStyleBackColor = True
        '
        'gridNG
        '
        Me.gridNG.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.gridNG.AllowEditing = False
        Me.gridNG.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.gridNG.AutoClipboard = True
        Me.gridNG.BackColor = System.Drawing.Color.White
        Me.gridNG.ColumnInfo = "3,0,0,0,0,100,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridNG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridNG.ExtendLastCol = True
        Me.gridNG.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridNG.Location = New System.Drawing.Point(3, 3)
        Me.gridNG.Name = "gridNG"
        Me.gridNG.Rows.Count = 5
        Me.gridNG.Rows.DefaultSize = 20
        Me.gridNG.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridNG.Size = New System.Drawing.Size(647, 393)
        Me.gridNG.StyleInfo = resources.GetString("gridNG.StyleInfo")
        Me.gridNG.TabIndex = 41
        '
        'tabDelayInput
        '
        Me.tabDelayInput.Controls.Add(Me.gridInput)
        Me.tabDelayInput.Location = New System.Drawing.Point(4, 24)
        Me.tabDelayInput.Name = "tabDelayInput"
        Me.tabDelayInput.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDelayInput.Size = New System.Drawing.Size(653, 399)
        Me.tabDelayInput.TabIndex = 1
        Me.tabDelayInput.Text = "DELAY INPUT"
        Me.tabDelayInput.UseVisualStyleBackColor = True
        '
        'gridInput
        '
        Me.gridInput.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.gridInput.AllowEditing = False
        Me.gridInput.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.gridInput.AutoClipboard = True
        Me.gridInput.BackColor = System.Drawing.Color.White
        Me.gridInput.ColumnInfo = "3,0,0,0,0,100,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridInput.ExtendLastCol = True
        Me.gridInput.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridInput.Location = New System.Drawing.Point(3, 3)
        Me.gridInput.Name = "gridInput"
        Me.gridInput.Rows.Count = 5
        Me.gridInput.Rows.DefaultSize = 20
        Me.gridInput.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridInput.Size = New System.Drawing.Size(647, 393)
        Me.gridInput.StyleInfo = resources.GetString("gridInput.StyleInfo")
        Me.gridInput.TabIndex = 42
        '
        'tabDelayVerification
        '
        Me.tabDelayVerification.Controls.Add(Me.gridVerification)
        Me.tabDelayVerification.Location = New System.Drawing.Point(4, 24)
        Me.tabDelayVerification.Name = "tabDelayVerification"
        Me.tabDelayVerification.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDelayVerification.Size = New System.Drawing.Size(653, 399)
        Me.tabDelayVerification.TabIndex = 2
        Me.tabDelayVerification.Text = "DELAY VERIFICATION"
        Me.tabDelayVerification.UseVisualStyleBackColor = True
        '
        'gridVerification
        '
        Me.gridVerification.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.gridVerification.AllowEditing = False
        Me.gridVerification.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.gridVerification.AutoClipboard = True
        Me.gridVerification.BackColor = System.Drawing.Color.White
        Me.gridVerification.ColumnInfo = "3,0,0,0,0,100,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridVerification.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridVerification.ExtendLastCol = True
        Me.gridVerification.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridVerification.Location = New System.Drawing.Point(3, 3)
        Me.gridVerification.Name = "gridVerification"
        Me.gridVerification.Rows.Count = 5
        Me.gridVerification.Rows.DefaultSize = 20
        Me.gridVerification.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.gridVerification.Size = New System.Drawing.Size(647, 393)
        Me.gridVerification.StyleInfo = resources.GetString("gridVerification.StyleInfo")
        Me.gridVerification.TabIndex = 42
        '
        'frmInboxNotifications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(661, 450)
        Me.Controls.Add(Me.tabNotification)
        Me.Controls.Add(Me.pctClose)
        Me.Controls.Add(Me.lblHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmInboxNotifications"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmInboxNotifications"
        CType(Me.pctClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabNotification.ResumeLayout(False)
        Me.tabNGInput.ResumeLayout(False)
        CType(Me.gridNG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDelayInput.ResumeLayout(False)
        CType(Me.gridInput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDelayVerification.ResumeLayout(False)
        CType(Me.gridVerification, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblHeader As Label
    Friend WithEvents pctClose As PictureBox
    Friend WithEvents tabNotification As TabControl
    Friend WithEvents tabNGInput As TabPage
    Friend WithEvents tabDelayInput As TabPage
    Friend WithEvents tabDelayVerification As TabPage
    Public WithEvents gridNG As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents gridInput As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents gridVerification As C1.Win.C1FlexGrid.C1FlexGrid
End Class
