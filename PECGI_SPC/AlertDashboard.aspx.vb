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
        Master.SiteTitle = sGlobal.menuName
        pUser = Session("user")
        'AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "X010")
        'show_error(MsgTypeEnum.Info, "", 0)
        'If AuthUpdate = False Then
        '    Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
        '    'commandColumn.Visible = False
        'End If
        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "X010")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "X010")
        If AuthUpdate = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewDataTextColumn)
            commandColumn.Visible = False

            Dim commandColumn2 = TryCast(GridNG.Columns(0), GridViewDataTextColumn)
            commandColumn2.Visible = False
        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "X010")
        If AuthDelete = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowDeleteButton = False
        End If

        lblDateNow.Text = DateTime.Now.ToString("dd-MMM-yyyy") 'HH:mm:ss
    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then
            up_GridLoad(cboFactory.Value)
        End If
    End Sub

    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles Grid.RowInserting
        'e.Cancel = True
        'Dim pErr As String = ""
        'Dim User As New ClsSPCItemCheckByType With {
        '    .FactoryCode = e.NewValues("FactoryCode"),
        '    .FactoryName = cboFactory.Text,
        '    .ItemTypeCode = e.NewValues("ItemTypeName"),
        '    .LineCode = e.NewValues("LineCode"),
        '    .ItemCheck = e.NewValues("ItemCheck"),
        '    .FrequencyCode = e.NewValues("FrequencyCode"),
        '    .RegistrationNo = e.NewValues("RegistrationNo"),
        '    .SampleSize = e.NewValues("SampleSize"),
        '    .Remark = e.NewValues("Remark"),
        '    .Evaluation = e.NewValues("Evaluation"),
        '    .CharacteristicItem = e.NewValues("CharacteristicItem"),
        '    .ActiveStatus = e.NewValues("ActiveStatus"),
        '    .UpdateUser = pUser,
        '    .CreateUser = pUser
        '}
        'Try
        '    clsSPCAlertDashboardDB.Insert(User)
        '    Grid.CancelEdit()
        '    up_GridLoad(cboFactory.Value)
        '    show_error(MsgTypeEnum.Success, "Save data successfully!", 1)
        'Catch ex As Exception
        '    show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        'End Try
    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles Grid.RowUpdating
        'e.Cancel = True
        'Dim pErr As String = ""
        'Dim LineCode As String = ""
        'Dim ItemCheck As String = ""
        'LineCode = e.NewValues("LineCode")
        'ItemCheck = e.NewValues("ItemCheck")
        'Dim User As New ClsSPCItemCheckByType With {
        '    .FactoryCode = e.NewValues("FactoryCode"),
        '    .ItemTypeCode = e.NewValues("ItemTypeCode"),
        '    .LineCode = LineCode.Substring(0, LineCode.IndexOf(" -")),
        '    .ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -")),
        '    .FrequencyCode = e.NewValues("FrequencyCode"),
        '    .RegistrationNo = e.NewValues("RegistrationNo"),
        '    .SampleSize = e.NewValues("SampleSize"),
        '    .Remark = e.NewValues("Remark"),
        '    .Evaluation = e.NewValues("Evaluation"),
        '    .CharacteristicItem = e.NewValues("CharacteristicStatus"),
        '    .ActiveStatus = e.NewValues("ActiveStatus"),
        '    .UpdateUser = pUser,
        '    .CreateUser = pUser
        '}
        'Try
        '    clsSPCAlertDashboardDB.Update(User)
        '    Grid.CancelEdit()
        '    up_GridLoad(cboFactory.Value)
        '    show_error(MsgTypeEnum.Success, "Update data successfully!", 1)
        'Catch ex As Exception
        '    show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        'End Try
    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles Grid.RowDeleting
        'e.Cancel = True
        'Try
        '    Dim FactoryCode As String = e.Values("FactoryCode")
        '    Dim ItemTypeCode As String = e.Values("ItemTypeCode")
        '    Dim LineCode As String = e.Values("LineCode")
        '    Dim ItemCheck As String = e.Values("ItemCheck")

        '    LineCode = LineCode.Substring(0, LineCode.IndexOf(" -"))
        '    ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

        '    clsSPCAlertDashboardDB.Delete(FactoryCode, ItemTypeCode, LineCode, ItemCheck)
        '    Grid.CancelEdit()
        '    up_GridLoad(cboFactory.Value)
        '    show_error(MsgTypeEnum.Success, "Delete data successfully!", 1)
        'Catch ex As Exception
        '    show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        'End Try
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
        LoadGridDelay(FactoryCode)
        LoadGridNG(FactoryCode)
        LoadGridDelayVerif(FactoryCode)
    End Sub
    Private Sub LoadGridDelayVerif(FactoryCode As String)
        Try
            GridDelayVerif.DataSource = clsSPCAlertDashboardDB.GetDelayVerificationGrid(pUser, FactoryCode)
            GridDelayVerif.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub LoadGridNG(FactoryCode As String)
        Try
            Dim dtLoadNGData As DataTable
            'dtLoadNGData = clsSPCAlertDashboardDB.GetNGDataList(FactoryCode)
            GridNG.DataSource = clsSPCAlertDashboardDB.GetNGDataList(pUser, FactoryCode)
            GridNG.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub LoadGridDelay(FactoryCode As String)
        Try
            Dim dtLoadGridDelay As DataTable
            dtLoadGridDelay = clsSPCAlertDashboardDB.GetList(pUser, FactoryCode)

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
    'Private Sub GridNG_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles GridNG.HtmlRowPrepared
    '    If e.GetValue("ItemTypeName") IsNot Nothing AndAlso e.GetValue("ItemTypeName").ToString = "BR2450A" Then
    '        e.Row.BackColor = System.Drawing.Color.Red
    '    End If
    'End Sub
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
    Public Shared Function DelayInput(pUser As String, pFactory As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSPCAlertDashboardDB.GetList(pUser, pFactory)
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
    Public Shared Function NGInput(pUser As String, pFactory As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSPCAlertDashboardDB.GetNGDataList(pUser, pFactory)
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
    Public Shared Function DelayVerify(pUser As String, pFactory As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSPCAlertDashboardDB.GetVerifyDataList(pUser, pFactory)
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

    Protected Sub GridDelayVerif_RowInserting(sender As Object, e As ASPxDataInsertingEventArgs)

    End Sub

    Protected Sub GridDelayVerif_RowDeleting(sender As Object, e As ASPxDataDeletingEventArgs)

    End Sub

    Protected Sub GridDelayVerif_AfterPerformCallback(sender As Object, e As ASPxGridViewAfterPerformCallbackEventArgs)
        If e.CallbackName <> "CANCELEDIT" Then
            up_GridLoad(cboFactory.Value)
        End If
    End Sub

    Protected Sub GridDelayVerif_StartRowEditing(sender As Object, e As ASPxStartRowEditingEventArgs)

    End Sub

    Protected Sub GridDelayVerif_RowValidating(sender As Object, e As ASPxDataValidationEventArgs)

    End Sub
    Private Sub GridDelayVerif_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles GridDelayVerif.HtmlDataCellPrepared
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

            e.Cell.Text = Convert.ToString(Test.Days & " Day " & Test.Hours & " Hours " & Test.Minutes & " Minutes")
            'e.Cell.Text = Convert.ToString(Hours & ":" & Test.Minutes & ":00")
        End If
    End Sub
    Protected Sub GridDelayVerif_CustomButtonCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomButtonCallbackEventArgs) Handles GridDelayVerif.CustomButtonCallback

        Try

            Dim CountSendEmail As Integer
            Dim CheckAvailableData As DataTable

            Dim FactoryCode = cboFactory.Value
            Dim ItemTypeName = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeName")
            'Dim ItemTypeCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeCode")
            Dim LineCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "LineCode")
            Dim ItemCheck = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemCheck")
            Dim LinkDate = GridDelayVerif.GetRowValues(e.VisibleIndex, "LinkDate")
            Dim ShiftCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ShiftCode")
            Dim SequenceNo = GridDelayVerif.GetRowValues(e.VisibleIndex, "SequenceNo")
            ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

            CountSendEmail = clsSPCAlertDashboardDB.SendEmail(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            CheckAvailableData = clsSPCAlertDashboardDB.CheckDataSendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            If CheckAvailableData.Rows.Count <= 0 Then

                CountSendEmail = clsSPCAlertDashboardDB.SendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

                'If CountSendEmail > 1 Then
                '    show_error(MsgTypeEnum.Success, "Send Email Success !", 1)
                'Else
                '    show_error(MsgTypeEnum.ErrorMsg, "Send Email Failed !", 1)
                'End If

            Else
                show_error(MsgTypeEnum.ErrorMsg, "Data Already Sended ", 1)
                Return
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Protected Sub GridNG_CustomButtonCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomButtonCallbackEventArgs) Handles GridNG.CustomButtonCallback

        Try

            Dim CountSendEmail As Integer
            Dim CheckAvailableData As DataTable

            Dim FactoryCode = cboFactory.Value
            Dim ItemTypeName = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeName")
            'Dim ItemTypeCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeCode")
            Dim LineCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "LineCode")
            Dim ItemCheck = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemCheck")
            Dim LinkDate = GridDelayVerif.GetRowValues(e.VisibleIndex, "LinkDate")
            Dim ShiftCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ShiftCode")
            Dim SequenceNo = GridDelayVerif.GetRowValues(e.VisibleIndex, "SequenceNo")
            ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

            CountSendEmail = clsSPCAlertDashboardDB.SendEmail(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            CheckAvailableData = clsSPCAlertDashboardDB.CheckDataSendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            If CheckAvailableData.Rows.Count <= 0 Then

                CountSendEmail = clsSPCAlertDashboardDB.SendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

                'If CountSendEmail > 1 Then
                '    show_error(MsgTypeEnum.Success, "Send Email Success !", 1)
                'Else
                '    show_error(MsgTypeEnum.ErrorMsg, "Send Email Failed !", 1)
                'End If

            Else
                show_error(MsgTypeEnum.ErrorMsg, "Data Already Sended ", 1)
                Return
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Protected Sub Grid_CustomButtonCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomButtonCallbackEventArgs) Handles Grid.CustomButtonCallback

        Try

            Dim CountSendEmail As Integer
            Dim CheckAvailableData As DataTable

            Dim FactoryCode = cboFactory.Value
            Dim ItemTypeName = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeName")
            'Dim ItemTypeCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeCode")
            Dim LineCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "LineCode")
            Dim ItemCheck = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemCheck")
            Dim LinkDate = GridDelayVerif.GetRowValues(e.VisibleIndex, "LinkDate")
            Dim ShiftCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ShiftCode")
            Dim SequenceNo = GridDelayVerif.GetRowValues(e.VisibleIndex, "SequenceNo")
            ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

            CountSendEmail = clsSPCAlertDashboardDB.SendEmail(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            CheckAvailableData = clsSPCAlertDashboardDB.CheckDataSendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            If CheckAvailableData.Rows.Count <= 0 Then

                CountSendEmail = clsSPCAlertDashboardDB.SendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

                'If CountSendEmail > 1 Then
                '    show_error(MsgTypeEnum.Success, "Send Email Success !", 1)
                'Else
                '    show_error(MsgTypeEnum.ErrorMsg, "Send Email Failed !", 1)
                'End If

            Else
                show_error(MsgTypeEnum.ErrorMsg, "Data Already Sended ", 1)
                Return
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Sub GridDelayVerif_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles GridDelayVerif.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "SendEmail" Then
                Dim A = "Test"
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
#End Region

End Class