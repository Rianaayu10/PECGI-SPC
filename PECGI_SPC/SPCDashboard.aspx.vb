Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services
Imports System.Configuration

Public Class SPCDashboard
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Public AuthInsert As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False
    Public dt As DataTable
    Dim UCL As String = ""
    Dim LCL As String = ""
    Dim USL As String = ""
    Dim LSL As String = ""
    Dim MinValue As String = ""
    Dim MaxValue As String = ""
    Dim Average As String = ""
    Dim pCharacteristicStatus As Integer
    Dim LinkDelayType As String
    Dim RColorBeforeNG As Integer
    Dim RColorBeforeDV As Integer
    Dim Color As Color

    'Param For Merge Data SPC Verification
    Dim mSPCResultID As String = ""
    Dim mType As String = ""
    Dim mMachineProcess As String = ""
    Dim mItemCheck As String = ""
    Dim mDate As String = ""
    Dim mShift As String = ""
    Dim mSeq As String = ""
    Dim mUSL As String = ""
    Dim mLSL As String = ""
    Dim mUCL As String = ""
    Dim mLCL As String = ""
    Dim mMin As String = ""
    Dim mMax As String = ""
    Dim mAverage As String = ""
    Dim mR As String = ""
    Dim mOperator As String = ""
    Dim mMachineKeeper As String = ""
    Dim mQC As String = ""
    Dim CountDataNG As Integer = 0
    Dim RowSpanMergeNG As Integer = 0
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            pUser = Session("user")
            up_GridLoad()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu("B010")
        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "B010")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        lblDateNow.Text = DateTime.Now.ToString("dd-MMM-yyyy") 'HH:mm:ss
        RColorBeforeNG = 0
        RColorBeforeDV = 0
    End Sub

#End Region

#Region "Functions"
    Private Sub up_GridLoad()
        Dim TimeNow = DateTime.Now.ToString("HH:mm")
        Dim DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim DateTimeYesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
        Dim VarDateTime = DateTime.Now

        If TimeNow < "07:00" Then
            VarDateTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
        ElseIf TimeNow > "07:00" Then
            VarDateTime = DateTime.Now.ToString("yyyy-MM-dd")
        End If

        LoadGridNG(VarDateTime)
        LoadDataDelayInput(VarDateTime)
        LoadGridDelayVerif(VarDateTime)
    End Sub
    Private Sub LoadDataDelayInput(VarDateTime As String)
        Try
            Dim Test As Integer
            Dim Test2 As String = ""
            Dim dtLoadGridDelay As DataTable
            dtLoadGridDelay = clsSPCAlertDashboardDB.GetListForDashboard(pUser, "F001", "1", VarDateTime)

            If dtLoadGridDelay.Rows.Count > 0 Then
                lblCountDelayInput.Text = dtLoadGridDelay.Rows.Count
                rptDdelayInput.DataSource = dtLoadGridDelay
                rptDdelayInput.DataBind()
            Else
                rptDdelayInput.DataSource = ""
                rptDdelayInput.DataBind()
            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub rptNGInput_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptNGInput.ItemCommand
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'Reference the Repeater Item.
            Dim item As RepeaterItem = e.Item

            'Reference the Controls.
            Dim MinValue As String = (TryCast(item.FindControl("lblMin"), Label)).Text
            Dim MaxValue As String = (TryCast(item.FindControl("lblMax"), Label)).Text
        End If
    End Sub
    Private Sub LoadGridNG(VarDateTime As String)
        Try
            Dim dtLoadGridNG As DataTable
            dtLoadGridNG = clsSPCAlertDashboardDB.GetNGDataListForDashboard(pUser, "F001", "1", VarDateTime)

            If dtLoadGridNG.Rows.Count > 0 Then
                'lblCountNGresult.Text = dtLoadGridNG.Rows.Count
                rptNGInput.DataSource = dtLoadGridNG
                rptNGInput.DataBind()
            Else
                rptNGInput.DataSource = ""
                rptNGInput.DataBind()
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub LoadGridDelayVerif(VarDateTime As String)
        Try
            Dim dtLoadGridDelayVerif As DataTable
            dtLoadGridDelayVerif = clsSPCAlertDashboardDB.GetDelayVerificationGridForDashboard(pUser, "F001", "1", VarDateTime, "B010")

            If dtLoadGridDelayVerif.Rows.Count > 0 Then
                lblCountDelayVerif.Text = dtLoadGridDelayVerif.Rows.Count
                rptDelayVerification.DataSource = dtLoadGridDelayVerif
                rptDelayVerification.DataBind()
            Else
                rptDelayVerification.DataSource = ""
                rptDelayVerification.DataBind()
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
    End Sub

    Protected Sub TimerNGResult_Tick(sender As Object, e As EventArgs)
        up_GridLoad()
    End Sub

    Protected Sub rptNGInput_OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'Reference the Repeater Item.
            Dim item As RepeaterItem = e.Item
            Dim FormatDigit As String = ""
            Dim IDCell As String = ""



            'Declare Label Repeater
            'Dim lblType As Label = TryCast(item.FindControl("lblType"), Label)
            Dim lblMachineProcess As Label = TryCast(item.FindControl("lblMachineProcess"), Label)
            Dim ItemCheck As Label = TryCast(item.FindControl("lblItemCheck"), Label)
            Dim lblDate As Label = TryCast(item.FindControl("lblDate"), Label)
            Dim lblShift As Label = TryCast(item.FindControl("lblShift"), Label)
            Dim lblSeq As Label = TryCast(item.FindControl("lblSeq"), Label)
            Dim lblAve As Label = TryCast(item.FindControl("lblAve"), Label)
            Dim lblUSL As Label = TryCast(item.FindControl("lblUSL"), Label)
            Dim lblLSL As Label = TryCast(item.FindControl("lblLSL"), Label)
            Dim lblUCL As Label = TryCast(item.FindControl("lblUCL"), Label)
            Dim lblLCL As Label = TryCast(item.FindControl("lblLCL"), Label)
            Dim lblMin As Label = TryCast(item.FindControl("lblMin"), Label)
            Dim lblMax As Label = TryCast(item.FindControl("lblMax"), Label)
            Dim lblRValue As Label = TryCast(item.FindControl("lblRValue"), Label)
            Dim lblOperator As Label = TryCast(item.FindControl("lblOperator"), Label)
            Dim lblMK As Label = TryCast(item.FindControl("lblMK"), Label)
            Dim lblQC As Label = TryCast(item.FindControl("lblQC"), Label)
            Dim lblTypeNGInput As Label = TryCast(item.FindControl("lblTypeNGInput"), Label)

            'MergeCell
            Dim cellMachineProcess As HtmlTableCell = TryCast(item.FindControl("cellMachineProcess"), HtmlTableCell)
            Dim cellItemCheck As HtmlTableCell = TryCast(item.FindControl("cellItemCheck"), HtmlTableCell)
            Dim cellDate As HtmlTableCell = TryCast(item.FindControl("cellDate"), HtmlTableCell)
            Dim cellShift As HtmlTableCell = TryCast(item.FindControl("cellShift"), HtmlTableCell)
            Dim cellSeq As HtmlTableCell = TryCast(item.FindControl("cellSeq"), HtmlTableCell)
            Dim cellUSL As HtmlTableCell = TryCast(item.FindControl("cellUSL"), HtmlTableCell)
            Dim cellLSL As HtmlTableCell = TryCast(item.FindControl("cellLSL"), HtmlTableCell)
            Dim cellUCL As HtmlTableCell = TryCast(item.FindControl("cellUCL"), HtmlTableCell)
            Dim cellLCL As HtmlTableCell = TryCast(item.FindControl("cellLCL"), HtmlTableCell)
            Dim MinValueNG As HtmlTableCell = TryCast(item.FindControl("MinValueNG"), HtmlTableCell)
            Dim MaxValueNG As HtmlTableCell = TryCast(item.FindControl("MaxValueNG"), HtmlTableCell)
            Dim AveValueNG As HtmlTableCell = TryCast(item.FindControl("AveValueNG"), HtmlTableCell)
            Dim RValue As HtmlTableCell = TryCast(item.FindControl("RValue"), HtmlTableCell)
            Dim cellOperator As HtmlTableCell = TryCast(item.FindControl("cellOperator"), HtmlTableCell)
            Dim cellMK As HtmlTableCell = TryCast(item.FindControl("cellMK"), HtmlTableCell)
            Dim cellQC As HtmlTableCell = TryCast(item.FindControl("cellQC"), HtmlTableCell)
            Dim cellTypeNG As HtmlTableCell = TryCast(item.FindControl("cellTypeNG"), HtmlTableCell)

            Dim ItemCheckCode As String = ItemCheck.Text
            ItemCheckCode = ItemCheckCode.Substring(0, ItemCheckCode.IndexOf(" -"))

            Dim Digit As Integer = ClsSPCItemCheckMasterDB.GetDigit(ItemCheckCode)

            If Digit = 3 Then
                FormatDigit = "0.000"
            ElseIf Digit = 4 Then
                FormatDigit = "0.0000"
            End If

            If lblTypeNGInput.Text = mType AndAlso lblMachineProcess.Text = mMachineProcess AndAlso ItemCheck.Text = mItemCheck AndAlso lblDate.Text = mDate AndAlso lblShift.Text = mShift AndAlso lblSeq.Text = mSeq AndAlso lblUSL.Text = mUSL AndAlso lblLSL.Text = mLSL AndAlso lblUCL.Text = mUCL AndAlso lblLCL.Text = mLCL AndAlso lblMin.Text = mMin AndAlso lblMax.Text = mMax AndAlso lblAve.Text = mAverage AndAlso lblRValue.Text = mR AndAlso lblOperator.Text = mOperator AndAlso lblMK.Text = mMachineKeeper AndAlso lblQC.Text = mQC Then
                lblMachineProcess.Text = ""
                ItemCheck.Text = ""
                lblDate.Text = ""
                lblShift.Text = ""
                lblSeq.Text = ""
                lblAve.Text = ""
                lblUSL.Text = ""
                lblLSL.Text = ""
                lblUCL.Text = ""
                lblLCL.Text = ""
                lblMin.Text = ""
                lblMax.Text = ""
                lblRValue.Text = ""
                lblOperator.Text = ""
                lblMK.Text = ""
                lblQC.Text = ""
                lblTypeNGInput.Text = ""

                cellMachineProcess.Visible = False
                cellItemCheck.Visible = False
                cellDate.Visible = False
                cellShift.Visible = False
                cellSeq.Visible = False
                cellUSL.Visible = False
                cellLSL.Visible = False
                cellUCL.Visible = False
                cellLCL.Visible = False
                MinValueNG.Visible = False
                MaxValueNG.Visible = False
                AveValueNG.Visible = False
                RValue.Visible = False
                cellOperator.Visible = False
                cellMK.Visible = False
                cellQC.Visible = False
                'cellTypeNG.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellTypeNG.ColSpan = 17

                'Dim test = cellTypeNG.ID

                'If RowSpanMergeNG > 0 Then
                cellTypeNG.Visible = False
                'End If

                RowSpanMergeNG = RowSpanMergeNG + 1

            Else

                mMachineProcess = lblMachineProcess.Text
                mItemCheck = ItemCheck.Text
                mDate = lblDate.Text
                mShift = lblShift.Text
                mSeq = lblSeq.Text
                mAverage = lblAve.Text
                mUSL = lblUSL.Text
                mLSL = lblLSL.Text
                mUCL = lblUCL.Text
                mLCL = lblLCL.Text
                mMin = lblMin.Text
                mMax = lblMax.Text
                mR = lblRValue.Text
                mOperator = lblOperator.Text
                mMachineKeeper = lblMK.Text
                mQC = lblQC.Text
                mType = lblTypeNGInput.Text

                cellMachineProcess.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellItemCheck.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellDate.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellShift.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellSeq.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellUSL.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellLSL.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellUCL.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellLCL.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                MinValueNG.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                MaxValueNG.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                AveValueNG.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                RValue.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellOperator.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellMK.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellQC.RowSpan = Split(lblTypeNGInput.Text, "||")(4)
                cellTypeNG.RowSpan = Split(lblTypeNGInput.Text, "||")(4)

                lblMin.Text = Format(Val(lblMin.Text), FormatDigit)
                lblMax.Text = Format(Val(lblMax.Text), FormatDigit)
                lblAve.Text = Format(Val(lblAve.Text), FormatDigit)
                lblUSL.Text = Format(Val(lblUSL.Text), FormatDigit)
                lblLSL.Text = Format(Val(lblLSL.Text), FormatDigit)
                lblUCL.Text = Format(Val(lblUCL.Text), FormatDigit)
                lblLCL.Text = Format(Val(lblLCL.Text), FormatDigit)

                pCharacteristicStatus = Split(lblTypeNGInput.Text, "||")(1)

                Dim SplitType = Split(lblTypeNGInput.Text, "||")(0)
                'lblTypeNGInput.Text = "<a href='" + Split(lblTypeNGInput.Text, "||")(2) + "' target='_blank'>" + Split(lblTypeNGInput.Text, "||")(0) + "</a>"
                lblTypeNGInput.Text = "<a href='" + Split(lblTypeNGInput.Text, "||")(2) + "' target='_blank'>SPC</a> | <a href='" + Split(lblTypeNGInput.Text, "||")(3) + "' target='_blank'>Corrective Action</a>"


                'Dim lblType As LinkButton = TryCast(item.FindControl("linkType"), LinkButton)
                'lblType.Text = Split(lblType.Text, "||")(0)

                'LinkDelayType = Split(lblType.Text, "||")(1)



                'Check If MinValue Is Out Of Spec Or Out Of Control
                If lblMin.Text < lblLSL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueNG"), HtmlTableCell)

                    CellMin.BgColor = "Red"
                    lblMin.ForeColor = Color.White
                ElseIf lblMin.Text > lblUSL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueNG"), HtmlTableCell)

                    CellMin.BgColor = "Red"
                    lblMin.ForeColor = Color.White
                ElseIf lblMin.Text > lblUCL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueNG"), HtmlTableCell)

                    If pCharacteristicStatus = 0 Then
                        CellMin.BgColor = "Pink"
                    ElseIf pCharacteristicStatus = 1 Then
                        CellMin.BgColor = "#ffff99"
                    End If
                    lblMin.ForeColor = Color.Black
                ElseIf lblMin.Text < lblLCL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueNG"), HtmlTableCell)

                    If pCharacteristicStatus = 0 Then
                        CellMin.BgColor = "Pink"
                    ElseIf pCharacteristicStatus = 1 Then
                        CellMin.BgColor = "#ffff99"
                    End If
                    lblMin.ForeColor = Color.Black
                End If

                'Check If MaxValue Is Out Of Spec Or Out Of Control
                If lblMax.Text < lblLSL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueNG"), HtmlTableCell)

                    CellMin.BgColor = "Red"
                    lblMax.ForeColor = Color.White
                ElseIf lblMax.Text > lblUSL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueNG"), HtmlTableCell)

                    CellMin.BgColor = "Red"
                    lblMax.ForeColor = Color.White
                ElseIf lblMax.Text > lblUCL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueNG"), HtmlTableCell)

                    If pCharacteristicStatus = 0 Then
                        CellMin.BgColor = "Pink"
                    ElseIf pCharacteristicStatus = 1 Then
                        CellMin.BgColor = "#ffff99"
                    End If
                    lblMax.ForeColor = Color.Black
                ElseIf lblMax.Text < lblLCL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueNG"), HtmlTableCell)

                    If pCharacteristicStatus = 0 Then
                        CellMin.BgColor = "Pink"
                    ElseIf pCharacteristicStatus = 1 Then
                        CellMin.BgColor = "#ffff99"
                    End If
                    lblMax.ForeColor = Color.Black
                End If

                'Check If Average Value Is Out Of Spec Or Out Of Control
                If lblAve.Text < lblLSL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueNG"), HtmlTableCell)

                    CellMin.BgColor = "Red"
                    lblAve.ForeColor = Color.White
                ElseIf lblAve.Text > lblUSL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueNG"), HtmlTableCell)

                    CellMin.BgColor = "Red"
                    lblAve.ForeColor = Color.White
                ElseIf lblAve.Text > lblUCL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueNG"), HtmlTableCell)

                    If pCharacteristicStatus = 0 Then
                        CellMin.BgColor = "Pink"
                    ElseIf pCharacteristicStatus = 1 Then
                        CellMin.BgColor = "#ffff99"
                    End If
                    lblAve.ForeColor = Color.Black
                ElseIf lblAve.Text < lblLCL.Text Then
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueNG"), HtmlTableCell)

                    If pCharacteristicStatus = 0 Then
                        CellMin.BgColor = "Pink"
                    ElseIf pCharacteristicStatus = 1 Then
                        CellMin.BgColor = "#ffff99"
                    End If
                    lblAve.ForeColor = Color.Black
                End If

                'Dim lblRValue As Label = TryCast(item.FindControl("lblRValue"), Label)
                Dim RCellValue = Split(lblRValue.Text, "||")(1)

                If RCellValue > 0 Then

                    If RCellValue >= RColorBeforeNG AndAlso RColorBeforeNG <> 0 Then
                        'Color = System.Drawing.Color.Pink
                        Dim CellMin As HtmlTableCell = TryCast(item.FindControl("RValue"), HtmlTableCell)

                        CellMin.BgColor = "Pink"
                    Else
                        'Color = System.Drawing.Color.Yellow
                        Dim CellMin As HtmlTableCell = TryCast(item.FindControl("RValue"), HtmlTableCell)

                        CellMin.BgColor = "Yellow"
                    End If

                Else

                    'Dim CellMin As HtmlTableCell = TryCast(item.FindControl("RValue"), HtmlTableCell)

                    'CellMin.BgColor = "White"

                End If

                RColorBeforeNG = RCellValue
                lblRValue.Text = Split(lblRValue.Text, "||")(0)

                CountDataNG = CountDataNG + 1
                lblCountNGresult.Text = CountDataNG
                cellMachineProcess.Visible = True
                cellItemCheck.Visible = True
                cellDate.Visible = True
                cellShift.Visible = True
                cellSeq.Visible = True
                cellUSL.Visible = True
                cellLSL.Visible = True
                cellUCL.Visible = True
                cellLCL.Visible = True
                MinValueNG.Visible = True
                MaxValueNG.Visible = True
                AveValueNG.Visible = True
                RValue.Visible = True
                cellOperator.Visible = True
                cellMK.Visible = True
                cellQC.Visible = True
                cellTypeNG.Visible = True

                
                RowSpanMergeNG = 0
            End If


        End If
    End Sub

    Protected Sub rptDelayVerification_OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim item As RepeaterItem = e.Item
            Dim FormatDigit As String = ""

            'Declare Label Repeater
            'Dim lblType As Label = TryCast(item.FindControl("lblType"), Label)
            Dim ItemCheck As Label = TryCast(item.FindControl("lblItemCheck"), Label)
            Dim lblMin As Label = TryCast(item.FindControl("lblMin"), Label)
            Dim lblMax As Label = TryCast(item.FindControl("lblMax"), Label)
            Dim lblAve As Label = TryCast(item.FindControl("lblAve"), Label)
            Dim lblUSL As Label = TryCast(item.FindControl("lblUSL"), Label)
            Dim lblLSL As Label = TryCast(item.FindControl("lblLSL"), Label)
            Dim lblUCL As Label = TryCast(item.FindControl("lblUCL"), Label)
            Dim lblLCL As Label = TryCast(item.FindControl("lblLCL"), Label)
            Dim lblDelayVerif As Label = TryCast(item.FindControl("lblDelayVerif"), Label)
            Dim TimeSpanDV = TimeSpan.FromMinutes(lblDelayVerif.Text)

            Dim lblTypeDelayVerification As Label = TryCast(item.FindControl("lblTypeDelayVerification"), Label)

            Dim ItemCheckCode As String = ItemCheck.Text
            ItemCheckCode = ItemCheckCode.Substring(0, ItemCheckCode.IndexOf(" -"))

            Dim Digit As Integer = ClsSPCItemCheckMasterDB.GetDigit(ItemCheckCode)

            If Digit = 3 Then
                FormatDigit = "0.000"
            ElseIf Digit = 4 Then
                FormatDigit = "0.0000"
            End If

            lblMin.Text = Format(Val(lblMin.Text), FormatDigit)
            lblMax.Text = Format(Val(lblMax.Text), FormatDigit)
            lblAve.Text = Format(Val(lblAve.Text), FormatDigit)
            lblUSL.Text = Format(Val(lblUSL.Text), FormatDigit)
            lblLSL.Text = Format(Val(lblLSL.Text), FormatDigit)
            lblUCL.Text = Format(Val(lblUCL.Text), FormatDigit)
            lblLCL.Text = Format(Val(lblLCL.Text), FormatDigit)


            Dim SplitType = Split(lblTypeDelayVerification.Text, "||")(0)
            pCharacteristicStatus = Split(lblTypeDelayVerification.Text, "||")(1)
            lblTypeDelayVerification.Text = "<a href='" + Split(lblTypeDelayVerification.Text, "||")(2) + "' target='_blank'>" + Split(lblTypeDelayVerification.Text, "||")(0) + "</a>"


            'Check If Delay Higher Than 60 Minute Or Not 
            If lblDelayVerif.Text < 60 Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("DelayVerification"), HtmlTableCell)
                If pCharacteristicStatus = 0 Then
                    CellMin.BgColor = "Pink"
                ElseIf pCharacteristicStatus = 1 Then
                    CellMin.BgColor = "#ffff99"
                End If
                lblDelayVerif.ForeColor = Color.Black

                'Change Format Delay Time
                Dim Days = TimeSpanDV.Days * 24
                Dim Hours = Days + TimeSpanDV.Hours

                If Days > 0 Then

                    lblDelayVerif.Text = Convert.ToString(TimeSpanDV.Days & "d " & TimeSpanDV.Hours & "h " & TimeSpanDV.Minutes & "m")

                Else

                    If Hours > 0 Then
                        lblDelayVerif.Text = Convert.ToString(TimeSpanDV.Hours & "h " & TimeSpanDV.Minutes & "m")
                    Else
                        lblDelayVerif.Text = Convert.ToString(TimeSpanDV.Minutes & "m")
                    End If

                End If
                'End Change Format Delay Time

            ElseIf lblDelayVerif.Text > 60 Then

                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("DelayVerification"), HtmlTableCell)
                CellMin.BgColor = "Red"
                lblDelayVerif.ForeColor = Color.White

                'Change Format Delay Time
                Dim Days = TimeSpanDV.Days * 24
                Dim Hours = Days + TimeSpanDV.Hours

                If Days > 0 Then

                    lblDelayVerif.Text = Convert.ToString(TimeSpanDV.Days & "d " & TimeSpanDV.Hours & "h " & TimeSpanDV.Minutes & "m")

                Else

                    If Hours > 0 Then
                        lblDelayVerif.Text = Convert.ToString(TimeSpanDV.Hours & "h " & TimeSpanDV.Minutes & "m")
                    Else
                        lblDelayVerif.Text = Convert.ToString(TimeSpanDV.Minutes & "m")
                    End If

                End If
                'End Change Format Delay Time

            End If

            'Check If MinValue Is Out Of Spec Or Out Of Control
            If lblMin.Text < lblLSL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueDV"), HtmlTableCell)

                CellMin.BgColor = "Red"
                lblMin.ForeColor = Color.White
            ElseIf lblMin.Text > lblUSL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueDV"), HtmlTableCell)

                CellMin.BgColor = "Red"
                lblMin.ForeColor = Color.White
            ElseIf lblMin.Text > lblUCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueDV"), HtmlTableCell)

                If pCharacteristicStatus = 0 Then
                    CellMin.BgColor = "Pink"
                ElseIf pCharacteristicStatus = 1 Then
                    CellMin.BgColor = "#ffff99"
                End If
                lblMin.ForeColor = Color.Black
            ElseIf lblMin.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueDV"), HtmlTableCell)

                If pCharacteristicStatus = 0 Then
                    CellMin.BgColor = "Pink"
                ElseIf pCharacteristicStatus = 1 Then
                    CellMin.BgColor = "#ffff99"
                End If
                lblMin.ForeColor = Color.Black
            End If

            'Check If MaxValue Is Out Of Spec Or Out Of Control
            If lblMax.Text < lblLSL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueDV"), HtmlTableCell)

                CellMin.BgColor = "Red"
                lblMax.ForeColor = Color.White
            ElseIf lblMax.Text > lblUSL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueDV"), HtmlTableCell)

                CellMin.BgColor = "Red"
                lblMax.ForeColor = Color.White
            ElseIf lblMax.Text > lblUCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueDV"), HtmlTableCell)

                If pCharacteristicStatus = 0 Then
                    CellMin.BgColor = "Pink"
                ElseIf pCharacteristicStatus = 1 Then
                    CellMin.BgColor = "#ffff99"
                End If
                lblMax.ForeColor = Color.Black
            ElseIf lblMax.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueDV"), HtmlTableCell)

                If pCharacteristicStatus = 0 Then
                    CellMin.BgColor = "Pink"
                ElseIf pCharacteristicStatus = 1 Then
                    CellMin.BgColor = "#ffff99"
                End If
                lblMax.ForeColor = Color.Black
            End If

            'Check If Average Value Is Out Of Spec Or Out Of Control
            If lblAve.Text < lblLSL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueDV"), HtmlTableCell)

                CellMin.BgColor = "Red"
                lblAve.ForeColor = Color.White
            ElseIf lblAve.Text > lblUSL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueDV"), HtmlTableCell)

                CellMin.BgColor = "Red"
                lblAve.ForeColor = Color.White
            ElseIf lblAve.Text > lblUCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueDV"), HtmlTableCell)

                If pCharacteristicStatus = 0 Then
                    CellMin.BgColor = "Pink"
                ElseIf pCharacteristicStatus = 1 Then
                    CellMin.BgColor = "#ffff99"
                End If
                lblAve.ForeColor = Color.Black
            ElseIf lblAve.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueDV"), HtmlTableCell)

                If pCharacteristicStatus = 0 Then
                    CellMin.BgColor = "Pink"
                ElseIf pCharacteristicStatus = 1 Then
                    CellMin.BgColor = "#ffff99"
                End If
                lblAve.ForeColor = Color.Black
            End If


            Dim lblRValue As Label = TryCast(item.FindControl("lblRValue"), Label)
            Dim RCellValue = Split(lblRValue.Text, "||")(1)

            If RCellValue > 0 Then

                If RCellValue >= RColorBeforeDV AndAlso RColorBeforeDV <> 0 Then
                    'Color = System.Drawing.Color.Pink
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("RValue"), HtmlTableCell)

                    CellMin.BgColor = "Pink"
                Else
                    'Color = System.Drawing.Color.Yellow
                    Dim CellMin As HtmlTableCell = TryCast(item.FindControl("RValue"), HtmlTableCell)

                    CellMin.BgColor = "Yellow"
                End If

            Else

                'Dim CellMin As HtmlTableCell = TryCast(item.FindControl("RValue"), HtmlTableCell)

                'CellMin.BgColor = "White"

            End If

            RColorBeforeDV = RCellValue
            lblRValue.Text = Split(lblRValue.Text, "||")(0)

        End If
    End Sub

    Protected Sub rptDelayInput_OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then


            Dim item As RepeaterItem = e.Item
            Dim Link As New HyperLink()

            'Declare Label Repeater
            Dim lblDelay As Label = TryCast(item.FindControl("lblDelay"), Label)
            Dim TimeSpanDI = TimeSpan.FromMinutes(lblDelay.Text)
            Dim lblTypeDelayInput As Label = TryCast(item.FindControl("lblTypeDelayInput"), Label)

            lblTypeDelayInput.Text = "<a href='" + Split(lblTypeDelayInput.Text, "||")(1) + "' target='_blank'>" + Split(lblTypeDelayInput.Text, "||")(0) + "</a>"
            'Link.Text = Split(lblType.Text, "||")(0)
            ''lblDelay.Text = Split(lblType.Text, "||")(0)
            'Link.NavigateUrl = Split(lblType.Text, "||")(1)
            'Link.Target = "_blank"

            'Dim lblType As LinkButton = TryCast(item.FindControl("linkType"), LinkButton)
            'lblType.Text = Split(lblType.Text, "||")(0)

            'LinkDelayType = Split(lblType.Text, "||")(1)

            'e.Item.Controls.Add(Link)

            'Check If Delay Higher Than 60 Minute Or Not 
            If lblDelay.Text < 60 Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("DelayInput"), HtmlTableCell)
                CellMin.BgColor = "Yellow"
                lblDelay.ForeColor = Color.Black

                'Change Format Delay Time
                Dim Days = TimeSpanDI.Days * 24
                Dim Hours = Days + TimeSpanDI.Hours

                If Days > 0 Then

                    lblDelay.Text = Convert.ToString(TimeSpanDI.Days & "d " & TimeSpanDI.Hours & "h " & TimeSpanDI.Minutes & "m ")

                Else

                    If Hours > 0 Then
                        lblDelay.Text = Convert.ToString(TimeSpanDI.Hours & "h " & TimeSpanDI.Minutes & "m ")
                    Else
                        lblDelay.Text = Convert.ToString(TimeSpanDI.Minutes & "m")
                    End If

                End If
                'End Change Format Delay Time

            ElseIf lblDelay.Text > 60 Then

                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("DelayInput"), HtmlTableCell)
                CellMin.BgColor = "Red"
                lblDelay.ForeColor = Color.White

                'Change Format Delay Time
                Dim Days = TimeSpanDI.Days * 24
                Dim Hours = Days + TimeSpanDI.Hours

                If Days > 0 Then

                    lblDelay.Text = Convert.ToString(TimeSpanDI.Days & "d " & TimeSpanDI.Hours & "h " & TimeSpanDI.Minutes & "m")

                Else

                    If Hours > 0 Then
                        lblDelay.Text = Convert.ToString(TimeSpanDI.Hours & "h " & TimeSpanDI.Minutes & "m")
                    Else
                        lblDelay.Text = Convert.ToString(TimeSpanDI.Minutes & "m")
                    End If

                End If
                'End Change Format Delay Time

            End If

        End If
    End Sub
    Private Sub linkType_ItemCommand()
        Response.Redirect(LinkDelayType)
    End Sub
#End Region

End Class