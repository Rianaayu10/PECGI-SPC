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
        LoadGridNG()
        LoadDataDelayInput()
        LoadGridDelayVerif()
    End Sub

#End Region

#Region "Functions"
    Private Sub LoadDataDelayInput()
        Try
            Dim dtLoadGridDelay As DataTable
            dtLoadGridDelay = clsSPCAlertDashboardDB.GetList("zqc", "F001", "1", "2022-11-01")

            rptDdelayInput.DataSource = dtLoadGridDelay
            rptDdelayInput.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub LoadGridNG()
        Try
            Dim dtLoadGridNG As DataTable
            dtLoadGridNG = clsSPCAlertDashboardDB.GetNGDataList("zqc", "F001", "1", "2022-11-01")

            rptDdelayInput.DataSource = dtLoadGridNG
            rptDdelayInput.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub LoadGridDelayVerif()
        Try
            Dim dtLoadGridDelayVerif As DataTable
            dtLoadGridDelayVerif = clsSPCAlertDashboardDB.GetDelayVerificationGrid("zqc", "F001", "1", "2022-11-01")

            rptDelayVerification.DataSource = dtLoadGridDelayVerif
            rptDelayVerification.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        'Grid.JSProperties("cp_message") = ErrMsg
        'Grid.JSProperties("cp_type") = msgType
        'Grid.JSProperties("cp_val") = pVal
    End Sub

#End Region

End Class