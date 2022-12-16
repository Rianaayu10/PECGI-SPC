Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services

Public Class AlertDashboardInput
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
            GetComboBoxData()
            hdInterval.Value = 60000
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu("X020")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user")
        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "X020")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "X020")
        If AuthUpdate = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewDataTextColumn)
            commandColumn.Visible = False

        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "X020")
        If AuthDelete = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowDeleteButton = False
        End If

        lblDateNow.Text = DateTime.Now.ToString("dd-MMM-yyyy") 'HH:mm:ss

        If Not IsPostBack And Not IsCallback Then
            dtDate.Value = DateTime.Now
            rbAuto.Checked = True
        End If
    End Sub
    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then
            up_GridLoad(cboFactory.Value)
        End If
    End Sub

    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles Grid.RowInserting

    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles Grid.RowUpdating

    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles Grid.RowDeleting

    End Sub

    Private Sub Grid_BeforeGetCallbackResult(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid.BeforeGetCallbackResult
        If Grid.IsNewRowEditing Then
            Grid.SettingsCommandButton.UpdateButton.Text = "Save"
        End If
    End Sub

    Private Sub Grid_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles Grid.CellEditorInitialize
        If Not Grid.IsNewRowEditing Then
            If e.Column.FieldName = "ItemCheckCode" Or e.Column.FieldName = "FactoryCode" Or e.Column.FieldName = "ItemTypeCode" Or e.Column.FieldName = "LineCode" Or e.Column.FieldName = "ItemTypeName" Or e.Column.FieldName = "ItemCheck" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = True
            End If
        End If

        If Grid.IsNewRowEditing Then
            If e.Column.FieldName = "FactoryCode" Then
                e.Editor.Value = cboFactory.Value
            End If
            If e.Column.FieldName = "ItemTypeCode" Then
                e.Editor.Visible = False
                e.Editor.ForeColor = Color.Silver
            End If
        End If
    End Sub
    Protected Sub Grid_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles Grid.RowValidating
    End Sub

    Protected Sub Grid_StartRowEditing(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles Grid.StartRowEditing
        If (Not Grid.IsNewRowEditing) Then
            Grid.DoRowValidation()
        End If
        show_error(MsgTypeEnum.Info, "", 0)
    End Sub
    Private Sub GetComboBoxData()
        GetFactoryCode()
    End Sub
    Private Sub GetFactoryCode()
        cboFactory.DataSource = ClsSPCItemCheckByTypeDB.FillComboFactoryGrid("1", Session("user"))
        cboFactory.DataBind()
        cboFactory.SelectedIndex = 1
    End Sub

#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Grid.JSProperties("cp_message") = ErrMsg
        Grid.JSProperties("cp_type") = msgType
        Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_GridLoad(FactoryCode As String)
        Dim pProdDateType As Integer

        If rbAuto.Checked = True Then
            pProdDateType = 1
        ElseIf rbManual.Checked = True Then
            pProdDateType = 2
        End If

        Dim pProdDate As DateTime = Convert.ToDateTime(dtDate.Date)
        Dim pProdDate2 = dtDate.Date.ToString()

        LoadGridDelay(FactoryCode, pProdDateType, pProdDate)
    End Sub
    Private Sub LoadGridDelay(FactoryCode As String, pProdDateType As Integer, pProdDate As DateTime)
        Try
            Dim dtLoadGridDelay As DataTable
            dtLoadGridDelay = clsSPCAlertDashboardDB.GetList(pUser, FactoryCode, pProdDateType, pProdDate)

            'If dtLoadGridDelay.Rows.Count > 0 Then
            'End If
            Grid.DataSource = dtLoadGridDelay
            Grid.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub Grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles Grid.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "Load" Then
                up_GridLoad(cboFactory.Value)
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub Grid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles Grid.HtmlDataCellPrepared
        Dim Delay As String = ""
        Dim Link As New HyperLink()
        If e.CellValue Is Nothing Then
            Dim Ab As String = "Test"
        Else
            If e.CellValue.ToString.Contains("Edit") AndAlso e.CellValue.ToString IsNot Nothing Then
                e.Cell.Text = ""

                Link.ForeColor = Color.Blue
                Link.Text = "<label class='fa fa-edit'></label>"
                Link.NavigateUrl = Split(e.CellValue, "||")(1)
                Link.Target = "_blank"

                e.Cell.Controls.Add(Link)
            End If
        End If
        If e.DataColumn.FieldName = "Delay" Then
            Delay = (e.CellValue)
            Dim Test = TimeSpan.FromMinutes(e.CellValue)

            If Delay <= 60 Then
                e.Cell.BackColor = System.Drawing.Color.Yellow
            ElseIf Delay > 60 Then
                e.Cell.BackColor = System.Drawing.Color.Red
            End If

            Dim Days = Test.Days * 24
            Dim Hours = Days + Test.Hours

            If Days > 0 Then

                e.Cell.Text = Convert.ToString(Test.Days & " Day " & Test.Hours & " Hours " & Test.Minutes & " Minutes")

            Else

                If Hours > 0 Then
                    e.Cell.Text = Convert.ToString(Test.Hours & " Hours " & Test.Minutes & " Minutes")
                Else
                    e.Cell.Text = Convert.ToString(Test.Minutes & " Minutes")
                End If

            End If
            'e.Cell.Text = Convert.ToString(Hours & ":" & Test.Minutes & ":00")
        End If
    End Sub

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function DelayInput(pUser As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSiteMasterDB.GetDelayInput(pUser)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If msg <> "" Then msg = msg & ";"
                msg = msg & dt.Rows(i)("Label").ToString

                If msg <> "" Then msg = msg & "||"
                msg = msg & dt.Rows(i)("Link").ToString & "||"
            Next
        Else
            msg = ""
        End If
        Return msg
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function NGInput(pUser As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSiteMasterDB.GetNGInput(pUser)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If msg <> "" Then msg = msg & ";"
                msg = msg & dt.Rows(i)("Label").ToString

                If msg <> "" Then msg = msg & "||"
                msg = msg & dt.Rows(i)("Link").ToString & "||"
            Next
        Else
            msg = ""
        End If
        Return msg
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function DelayVerify(pUser As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSiteMasterDB.GetDelayVerify(pUser)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If msg <> "" Then msg = msg & ";"
                msg = msg & dt.Rows(i)("Label").ToString

                If msg <> "" Then msg = msg & "||"
                msg = msg & dt.Rows(i)("Link").ToString & "||"
            Next
        Else
            msg = ""
        End If
        Return msg
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function AlertNotif(pUser As String) As String
        Dim msg As String = ""

        Dim dtDI = clsSiteMasterDB.GetDelayInput(pUser)
        Dim dtDV = clsSiteMasterDB.GetDelayVerify(pUser)
        Dim dtNG = clsSiteMasterDB.GetNGInput(pUser)

        Dim TotDI = dtDI.Rows.Count
        Dim TotDV = dtDV.Rows.Count
        Dim TotNG = dtNG.Rows.Count
        Dim TotNotif = TotDI + TotDV + TotNG
        msg = TotNotif & "|" & TotDI & "|" & TotDV & "|" & TotNG & "|" & DateTime.Now.ToString("yyyy MMM dd HH:mm:ss")

        Return msg
    End Function


    Protected Sub Grid_CustomButtonCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomButtonCallbackEventArgs) Handles Grid.CustomButtonCallback

        Try

            Dim CountSendEmail As Integer
            Dim CheckAvailableData As DataTable
            Dim UserTo As String

            Dim FactoryCode = cboFactory.Value
            Dim ItemTypeName = Split(Grid.GetRowValues(e.VisibleIndex, "ItemTypeName"), "||")(0)
            'Dim ItemTypeCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeCode")
            Dim LineCode = Grid.GetRowValues(e.VisibleIndex, "LineCode")
            Dim LineName = Grid.GetRowValues(e.VisibleIndex, "LineName")
            Dim ItemCheck = Grid.GetRowValues(e.VisibleIndex, "ItemCheck")
            Dim LinkDate = Grid.GetRowValues(e.VisibleIndex, "LinkDate")
            Dim ShiftCode = Grid.GetRowValues(e.VisibleIndex, "ShiftCode")
            Dim SequenceNo = Grid.GetRowValues(e.VisibleIndex, "SequenceNo")
            Dim ScheduleStart = Grid.GetRowValues(e.VisibleIndex, "StartTime")
            Dim ScheduleEnd = Grid.GetRowValues(e.VisibleIndex, "EndTime")
            Dim DelayTime = Grid.GetRowValues(e.VisibleIndex, "Delay")

            UserTo = clsSPCAlertDashboardDB.GetUserLine(FactoryCode, LineCode, "1")

            CountSendEmail = clsSPCAlertDashboardDB.SendEmail(FactoryCode, ItemTypeName, LineName, ItemCheck, LinkDate, ShiftCode, SequenceNo, "2", "", "", "", "", "", "", "", "", ScheduleStart, ScheduleEnd, "", DelayTime, UserTo)

            If CountSendEmail > 0 Then
                show_error(MsgTypeEnum.Success, "Send Email Success", 1)
            Else
                show_error(MsgTypeEnum.ErrorMsg, "Send Email Failed", 1)
            End If

            'CheckAvailableData = clsSPCAlertDashboardDB.CheckDataSendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            'If CheckAvailableData.Rows.Count <= 0 Then

            '    CountSendEmail = clsSPCAlertDashboardDB.SendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            'Else
            '    Return
            'End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Shared Function GetUserInLine(pUserLine As String) As String
        Dim ListData As String
        Dim DataUser As DataTable

        Return ListData
    End Function
#End Region

End Class