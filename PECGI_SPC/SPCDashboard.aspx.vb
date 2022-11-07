Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services

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
            'up_GridLoad()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu("X010")
        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "X010")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        'LoadGridNG("1")
        'LoadDataDelayInput()
        'LoadGridDelayVerif()
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
            Dim dtLoadGridDelay As DataTable
            dtLoadGridDelay = clsSPCAlertDashboardDB.GetList(pUser, "F001", "1", VarDateTime)

            If dtLoadGridDelay.Rows.Count > 0 Then
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
    Private Sub LoadGridNG(VarDateTime As String)
        Try
            Dim dtLoadGridNG As DataTable
            dtLoadGridNG = clsSPCAlertDashboardDB.GetNGDataList(pUser, "F001", "1", VarDateTime)

            If dtLoadGridNG.Rows.Count > 0 Then
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
                rptDelayVerification.DataSource = dtLoadGridDelayVerif
                rptDelayVerification.DataBind()
            Else
                rptDelayVerification.DataSource = ""
                rptDelayVerification.DataBind()
            End If

            'For Each rptDV As RepeaterItem In rptDelayVerification.Items

            '    Dim lbl As Integer = Convert.ToInt32(DirectCast(rptDV.FindControl("Delay"), Label))

            '    If lbl > 60 Then
            '        Dim Test = "Delay"
            '    End If
            'Next

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        'Grid.JSProperties("cp_message") = ErrMsg
        'Grid.JSProperties("cp_type") = msgType
        'Grid.JSProperties("cp_val") = pVal
    End Sub

    Protected Sub TimerNGResult_Tick(sender As Object, e As EventArgs)
        up_GridLoad()
    End Sub

#End Region

End Class