Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services

Public Class AlertDashboard
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
        sGlobal.getMenu("X010")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user")
        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "X010")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "X010")
        If AuthUpdate = False Then

            Dim commandColumn2 = TryCast(GridNG.Columns(0), GridViewDataTextColumn)
            commandColumn2.Visible = False
        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "X010")
        If AuthDelete = False Then
        End If

        lblDateNow.Text = DateTime.Now.ToString("dd-MMM-yyyy") 'HH:mm:ss

        If Not IsPostBack And Not IsCallback Then
            dtDate.Value = DateTime.Now
            rbAuto.Checked = True
        End If
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

    End Sub

    Private Sub up_GridLoad(FactoryCode As String)
        Dim pProdDateType As Integer

        'If ProdDateSelection.SelectedValue = "rbAuto" Then
        '    rbProdDate = 1
        'ElseIf ProdDateSelection.SelectedValue = "rbManual" Then
        '    rbProdDate = 2
        'End If
        If rbAuto.Checked = True Then
            pProdDateType = 1
        ElseIf rbManual.Checked = True Then
            pProdDateType = 2
        End If

        Dim pProdDate As DateTime = Convert.ToDateTime(dtDate.Date)
        Dim pProdDate2 = dtDate.Date.ToString()

        LoadGridNG(FactoryCode, pProdDateType, pProdDate)
    End Sub
    Private Sub LoadGridNG(FactoryCode As String, pProdDateType As Integer, pProdDate As DateTime)
        Try
            Dim dtLoadNGData As DataTable
            'dtLoadNGData = clsSPCAlertDashboardDB.GetNGDataList(FactoryCode)
            GridNG.DataSource = clsSPCAlertDashboardDB.GetNGDataList(pUser, FactoryCode, pProdDateType, pProdDate)
            GridNG.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub GridNG_RowValidating(sender As Object, e As ASPxDataValidationEventArgs)

    End Sub

    Protected Sub GridNG_StartRowEditing(sender As Object, e As ASPxStartRowEditingEventArgs) Handles GridNG.StartRowEditing
        If (Not GridNG.IsNewRowEditing) Then
            GridNG.DoRowValidation()
        End If
        show_error(MsgTypeEnum.Info, "", 0)
    End Sub

    Protected Sub GridNG_RowInserting(sender As Object, e As ASPxDataInsertingEventArgs) Handles GridNG.RowInserting

    End Sub

    Protected Sub GridNG_RowDeleting(sender As Object, e As ASPxDataDeletingEventArgs)

    End Sub

    Protected Sub GridNG_AfterPerformCallback(sender As Object, e As ASPxGridViewAfterPerformCallbackEventArgs) Handles GridNG.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then
            up_GridLoad(cboFactory.Value)
        End If
    End Sub
    Private Sub GridNG_BeforeGetCallbackResult(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridNG.BeforeGetCallbackResult
        If GridNG.IsNewRowEditing Then
            GridNG.SettingsCommandButton.UpdateButton.Text = "Save"
        End If
    End Sub
    Private Sub GridNG_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles GridNG.CellEditorInitialize
        If Not GridNG.IsNewRowEditing Then
            If e.Column.FieldName = "ItemCheckCode" Or e.Column.FieldName = "FactoryCode" Or e.Column.FieldName = "ItemTypeCode" Or e.Column.FieldName = "LineCode" Or e.Column.FieldName = "ItemTypeName" Or e.Column.FieldName = "ItemCheck" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = True
            End If
        End If

        If GridNG.IsNewRowEditing Then
            If e.Column.FieldName = "FactoryCode" Then
                e.Editor.Value = cboFactory.Value
            End If
            If e.Column.FieldName = "ItemTypeCode" Then
                e.Editor.Visible = False
                e.Editor.ForeColor = Color.Silver
            End If
        End If
    End Sub
    Private Sub GridNG_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles GridNG.HtmlDataCellPrepared
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
        If e.DataColumn.FieldName = "LSL" Then
            LSL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "USL" Then
            USL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "UCL" Then
            UCL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "LCL" Then
            LCL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "MinValue" Then
            MinValue = (e.CellValue)
            If MinValue < LSL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf MinValue < LCL Then
                e.Cell.BackColor = System.Drawing.Color.Yellow
            End If
        ElseIf e.DataColumn.FieldName = "MaxValue" Then
            MaxValue = (e.CellValue)
            If MaxValue > USL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf MaxValue > UCL Then
                e.Cell.BackColor = System.Drawing.Color.Yellow
            End If
        ElseIf e.DataColumn.FieldName = "Average" Then
            Average = (e.CellValue)
            If Average > USL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf Average > UCL Then
                e.Cell.BackColor = System.Drawing.Color.Yellow
            ElseIf Average < LSL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf Average < LCL Then
                e.Cell.BackColor = System.Drawing.Color.Yellow
            End If
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

    Protected Sub GridNG_CustomButtonCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomButtonCallbackEventArgs) Handles GridNG.CustomButtonCallback

        Try

            Dim CountSendEmail As Integer
            Dim CheckAvailableData As DataTable
            Dim UserTo As String

            Dim FactoryCode = cboFactory.Value
            Dim ItemTypeName = GridNG.GetRowValues(e.VisibleIndex, "ItemTypeName")
            'Dim ItemTypeCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeCode")
            Dim LineCode = GridNG.GetRowValues(e.VisibleIndex, "LineCode")
            Dim ItemCheck = GridNG.GetRowValues(e.VisibleIndex, "ItemCheck")
            Dim LinkDate = GridNG.GetRowValues(e.VisibleIndex, "LinkDate")
            Dim ShiftCode = GridNG.GetRowValues(e.VisibleIndex, "ShiftCode")
            Dim SequenceNo = GridNG.GetRowValues(e.VisibleIndex, "SequenceNo")

            Dim LSL = GridNG.GetRowValues(e.VisibleIndex, "LSL")
            Dim USL = GridNG.GetRowValues(e.VisibleIndex, "USL")
            Dim LCL = GridNG.GetRowValues(e.VisibleIndex, "LCL")
            Dim UCL = GridNG.GetRowValues(e.VisibleIndex, "UCL")
            Dim MinValue = GridNG.GetRowValues(e.VisibleIndex, "MinValue")
            Dim MaxValue = GridNG.GetRowValues(e.VisibleIndex, "MaxValue")
            Dim Average = GridNG.GetRowValues(e.VisibleIndex, "Average")
            ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

            'Dim Test = GridDelayVerif.GetRowValues(e.VisibleIndex, GridDelayVerif.KeyFieldName, "LinkDate")
            'Dim Test = DirectCast(GridDelayVerif.GetRowValues(e.VisibleIndex, GridDelayVerif.KeyFieldName, "LinkDate"), Object())(1)

            UserTo = clsSPCAlertDashboardDB.GetUserLine(FactoryCode, LineCode, "1")

            CountSendEmail = clsSPCAlertDashboardDB.SendEmail(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo, "1", LSL, USL, LCL, UCL, MinValue, MaxValue, Average, "NG", "", "", "", "", UserTo)

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