﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSPCNotification
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSPCNotification))
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.cboFactory = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblTotalDelayVerification = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.gridDelayVerification = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblTotalDelayInput = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.gridDelayInput = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTotalNGResult = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.gridNGResult = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.pnlHeader.SuspendLayout()
        Me.Panel4.SuspendLayout()
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
        Me.pnlHeader.BackColor = System.Drawing.Color.CornflowerBlue
        Me.pnlHeader.Controls.Add(Me.Label20)
        Me.pnlHeader.Controls.Add(Me.Label19)
        Me.pnlHeader.Controls.Add(Me.Panel4)
        Me.pnlHeader.Controls.Add(Me.btnRefresh)
        Me.pnlHeader.Controls.Add(Me.cboFactory)
        Me.pnlHeader.Controls.Add(Me.Label1)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(980, 56)
        Me.pnlHeader.TabIndex = 0
        '
        'Label20
        '
        Me.Label20.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label20.Font = New System.Drawing.Font("Tahoma", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Black
        Me.Label20.Location = New System.Drawing.Point(159, 12)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(541, 35)
        Me.Label20.TabIndex = 9
        Me.Label20.Text = "SPC MONITORING"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Tahoma", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.White
        Me.Label19.Location = New System.Drawing.Point(4, 10)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(163, 35)
        Me.Label19.TabIndex = 8
        Me.Label19.Text = "Panasonic"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Black
        Me.Panel4.Controls.Add(Me.Label22)
        Me.Panel4.Controls.Add(Me.Label21)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.lblTime)
        Me.Panel4.Controls.Add(Me.lblDate)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel4.Location = New System.Drawing.Point(784, 0)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(196, 56)
        Me.Panel4.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(35, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Date"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(36, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Time"
        '
        'lblTime
        '
        Me.lblTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.ForeColor = System.Drawing.Color.White
        Me.lblTime.Location = New System.Drawing.Point(89, 31)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(72, 16)
        Me.lblTime.TabIndex = 5
        Me.lblTime.Text = "HH:mm:ss"
        '
        'lblDate
        '
        Me.lblDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.Color.White
        Me.lblDate.Location = New System.Drawing.Point(89, 9)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(97, 16)
        Me.lblDate.TabIndex = 4
        Me.lblDate.Text = "dd MMM yyyy"
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.btnRefresh.FlatAppearance.BorderSize = 0
        Me.btnRefresh.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(700, 14)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 30)
        Me.btnRefresh.TabIndex = 6
        Me.btnRefresh.Text = "&Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'cboFactory
        '
        Me.cboFactory.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.cboFactory.FormattingEnabled = True
        Me.cboFactory.Location = New System.Drawing.Point(548, 22)
        Me.cboFactory.Name = "cboFactory"
        Me.cboFactory.Size = New System.Drawing.Size(121, 24)
        Me.cboFactory.TabIndex = 1
        Me.cboFactory.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(492, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Factory"
        Me.Label1.Visible = False
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
        Me.pnlContent.Font = New System.Drawing.Font("Calibri", 8.25!)
        Me.pnlContent.Location = New System.Drawing.Point(0, 56)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(980, 591)
        Me.pnlContent.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.lblTotalDelayVerification)
        Me.Panel3.Controls.Add(Me.Label15)
        Me.Panel3.Controls.Add(Me.Label16)
        Me.Panel3.Controls.Add(Me.Label17)
        Me.Panel3.Location = New System.Drawing.Point(3, 552)
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
        'lblTotalDelayVerification
        '
        Me.lblTotalDelayVerification.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalDelayVerification.AutoSize = True
        Me.lblTotalDelayVerification.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalDelayVerification.Location = New System.Drawing.Point(887, 6)
        Me.lblTotalDelayVerification.Name = "lblTotalDelayVerification"
        Me.lblTotalDelayVerification.Size = New System.Drawing.Size(84, 13)
        Me.lblTotalDelayVerification.TabIndex = 5
        Me.lblTotalDelayVerification.Text = "Total : 0 Record"
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
        Me.Label18.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.BackColor = System.Drawing.Color.CornflowerBlue
        Me.Label18.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.White
        Me.Label18.Location = New System.Drawing.Point(3, 381)
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
        Me.gridDelayVerification.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridDelayVerification.AutoClipboard = True
        Me.gridDelayVerification.BackColor = System.Drawing.Color.White
        Me.gridDelayVerification.ColumnInfo = "3,0,0,0,0,105,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridDelayVerification.ExtendLastCol = True
        Me.gridDelayVerification.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridDelayVerification.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gridDelayVerification.Location = New System.Drawing.Point(3, 408)
        Me.gridDelayVerification.Name = "gridDelayVerification"
        Me.gridDelayVerification.Rows.Count = 5
        Me.gridDelayVerification.Rows.DefaultSize = 21
        Me.gridDelayVerification.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
        Me.gridDelayVerification.Size = New System.Drawing.Size(976, 141)
        Me.gridDelayVerification.StyleInfo = resources.GetString("gridDelayVerification.StyleInfo")
        Me.gridDelayVerification.TabIndex = 48
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.lblTotalDelayInput)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Location = New System.Drawing.Point(3, 347)
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
        'lblTotalDelayInput
        '
        Me.lblTotalDelayInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalDelayInput.AutoSize = True
        Me.lblTotalDelayInput.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalDelayInput.Location = New System.Drawing.Point(887, 6)
        Me.lblTotalDelayInput.Name = "lblTotalDelayInput"
        Me.lblTotalDelayInput.Size = New System.Drawing.Size(84, 13)
        Me.lblTotalDelayInput.TabIndex = 4
        Me.lblTotalDelayInput.Text = "Total : 0 Record"
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
        Me.Label13.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.BackColor = System.Drawing.Color.CornflowerBlue
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.White
        Me.Label13.Location = New System.Drawing.Point(3, 179)
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
        Me.gridDelayInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridDelayInput.AutoClipboard = True
        Me.gridDelayInput.BackColor = System.Drawing.Color.White
        Me.gridDelayInput.ColumnInfo = "3,0,0,0,0,105,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridDelayInput.ExtendLastCol = True
        Me.gridDelayInput.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridDelayInput.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gridDelayInput.Location = New System.Drawing.Point(3, 206)
        Me.gridDelayInput.Name = "gridDelayInput"
        Me.gridDelayInput.Rows.Count = 5
        Me.gridDelayInput.Rows.DefaultSize = 21
        Me.gridDelayInput.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
        Me.gridDelayInput.Size = New System.Drawing.Size(976, 138)
        Me.gridDelayInput.StyleInfo = resources.GetString("gridDelayInput.StyleInfo")
        Me.gridDelayInput.TabIndex = 45
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.lblTotalNGResult)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Location = New System.Drawing.Point(3, 146)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(974, 25)
        Me.Panel1.TabIndex = 43
        '
        'lblTotalNGResult
        '
        Me.lblTotalNGResult.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalNGResult.AutoSize = True
        Me.lblTotalNGResult.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalNGResult.Location = New System.Drawing.Point(887, 6)
        Me.lblTotalNGResult.Name = "lblTotalNGResult"
        Me.lblTotalNGResult.Size = New System.Drawing.Size(84, 13)
        Me.lblTotalNGResult.TabIndex = 5
        Me.lblTotalNGResult.Text = "Total : 0 Record"
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
        Me.Label4.BackColor = System.Drawing.Color.CornflowerBlue
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(3, 3)
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
        Me.gridNGResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridNGResult.AutoClipboard = True
        Me.gridNGResult.BackColor = System.Drawing.Color.White
        Me.gridNGResult.ColumnInfo = "3,0,0,0,0,105,Columns:1{Width:118;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.gridNGResult.ExtendLastCol = True
        Me.gridNGResult.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.gridNGResult.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gridNGResult.Location = New System.Drawing.Point(3, 30)
        Me.gridNGResult.Name = "gridNGResult"
        Me.gridNGResult.Rows.Count = 5
        Me.gridNGResult.Rows.DefaultSize = 21
        Me.gridNGResult.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
        Me.gridNGResult.Size = New System.Drawing.Size(976, 113)
        Me.gridNGResult.StyleInfo = resources.GetString("gridNGResult.StyleInfo")
        Me.gridNGResult.TabIndex = 42
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'Label21
        '
        Me.Label21.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.White
        Me.Label21.Location = New System.Drawing.Point(75, 9)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(13, 16)
        Me.Label21.TabIndex = 6
        Me.Label21.Text = ":"
        '
        'Label22
        '
        Me.Label22.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.White
        Me.Label22.Location = New System.Drawing.Point(75, 31)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(13, 16)
        Me.Label22.TabIndex = 7
        Me.Label22.Text = ":"
        '
        'frmSPCNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(980, 647)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSPCNotification"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SPC Notification"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
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
    Friend WithEvents lblTotalNGResult As Label
    Friend WithEvents lblTotalDelayVerification As Label
    Friend WithEvents lblTotalDelayInput As Label
    Friend WithEvents btnRefresh As Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
End Class