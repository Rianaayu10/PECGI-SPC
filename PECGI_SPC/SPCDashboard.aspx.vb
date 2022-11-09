﻿Imports Microsoft.VisualBasic
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
            dtLoadGridDelay = clsSPCAlertDashboardDB.GetList(pUser, "F001", "1", VarDateTime)

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
            dtLoadGridNG = clsSPCAlertDashboardDB.GetNGDataList(pUser, "F001", "1", VarDateTime)

            If dtLoadGridNG.Rows.Count > 0 Then
                lblCountNGresult.Text = dtLoadGridNG.Rows.Count
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
            dtLoadGridDelayVerif = clsSPCAlertDashboardDB.GetDelayVerificationGrid(pUser, "F001", "1", VarDateTime)

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

            'Declare Label Repeater
            Dim lblMin As Label = TryCast(item.FindControl("lblMin"), Label)
            Dim lblMax As Label = TryCast(item.FindControl("lblMax"), Label)
            Dim lblAve As Label = TryCast(item.FindControl("lblAve"), Label)
            Dim lblUSL As Label = TryCast(item.FindControl("lblUSL"), Label)
            Dim lblLSL As Label = TryCast(item.FindControl("lblLSL"), Label)
            Dim lblUCL As Label = TryCast(item.FindControl("lblUCL"), Label)
            Dim lblLCL As Label = TryCast(item.FindControl("lblLCL"), Label)

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

                CellMin.BgColor = "Yellow"
                lblMin.ForeColor = Color.Black
            ElseIf lblMin.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueNG"), HtmlTableCell)

                CellMin.BgColor = "Yellow"
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

                CellMin.BgColor = "Yellow"
                lblMax.ForeColor = Color.Black
            ElseIf lblMax.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueNG"), HtmlTableCell)

                CellMin.BgColor = "Yellow"
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

                CellMin.BgColor = "Yellow"
                lblAve.ForeColor = Color.Black
            ElseIf lblAve.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueNG"), HtmlTableCell)

                CellMin.BgColor = "Yellow"
                lblAve.ForeColor = Color.Black
            End If

        End If
    End Sub

    Protected Sub rptDelayVerification_OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim item As RepeaterItem = e.Item

            'Declare Label Repeater
            Dim lblMin As Label = TryCast(item.FindControl("lblMin"), Label)
            Dim lblMax As Label = TryCast(item.FindControl("lblMax"), Label)
            Dim lblAve As Label = TryCast(item.FindControl("lblAve"), Label)
            Dim lblUSL As Label = TryCast(item.FindControl("lblUSL"), Label)
            Dim lblLSL As Label = TryCast(item.FindControl("lblLSL"), Label)
            Dim lblUCL As Label = TryCast(item.FindControl("lblUCL"), Label)
            Dim lblLCL As Label = TryCast(item.FindControl("lblLCL"), Label)
            Dim lblDelayVerif As Label = TryCast(item.FindControl("lblDelayVerif"), Label)
            Dim TimeSpanDV = TimeSpan.FromMinutes(lblDelayVerif.Text)

            'Check If Delay Higher Than 60 Minute Or Not 
            If lblDelayVerif.Text < 60 Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("DelayVerification"), HtmlTableCell)
                CellMin.BgColor = "Yellow"
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

                CellMin.BgColor = "Yellow"
                lblMin.ForeColor = Color.Black
            ElseIf lblMin.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MinValueDV"), HtmlTableCell)

                CellMin.BgColor = "Yellow"
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

                CellMin.BgColor = "Yellow"
                lblMax.ForeColor = Color.Black
            ElseIf lblMax.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("MaxValueDV"), HtmlTableCell)

                CellMin.BgColor = "Yellow"
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

                CellMin.BgColor = "Yellow"
                lblAve.ForeColor = Color.Black
            ElseIf lblAve.Text < lblLCL.Text Then
                Dim CellMin As HtmlTableCell = TryCast(item.FindControl("AveValueDV"), HtmlTableCell)

                CellMin.BgColor = "Yellow"
                lblAve.ForeColor = Color.Black
            End If

        End If
    End Sub

    Protected Sub rptDelayInput_OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim item As RepeaterItem = e.Item

            'Declare Label Repeater
            Dim lblDelay As Label = TryCast(item.FindControl("lblDelay"), Label)
            Dim TimeSpanDI = TimeSpan.FromMinutes(lblDelay.Text)

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
#End Region

End Class