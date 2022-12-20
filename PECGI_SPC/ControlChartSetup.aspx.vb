Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing

Public Class ControlChartSetup
    Inherits System.Web.UI.Page

#Region "Declarations"
    Dim pUser As String = ""
    Dim pMenuID As String = "A040"
    Public AuthAccess As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Private dt As DataTable
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            pUser = Session("user")
            up_Fillcombo()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu(pMenuID)
        Master.SiteTitle = pMenuID & " - " & sGlobal.menuName
        pUser = Session("user")

        Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
        AuthAccess = sGlobal.Auth_UserAccess(pUser, pMenuID)
        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, pMenuID)
        AuthDelete = sGlobal.Auth_UserDelete(pUser, pMenuID)

        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        If AuthUpdate = False Then
            commandColumn.ShowEditButton = False
            commandColumn.ShowNewButtonInHeader = False
        End If

        If AuthDelete = False Then
            commandColumn.ShowDeleteButton = False
        End If

        show_error(MsgTypeEnum.Info, "", 0)
    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
            up_GridLoad()
        End If
    End Sub

    Private Sub Grid_BeforeGetCallbackResult(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid.BeforeGetCallbackResult
        If Grid.IsNewRowEditing Then
            Grid.SettingsCommandButton.UpdateButton.Text = "Save"
        End If
    End Sub

    Private Sub Grid_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles Grid.CellEditorInitialize
        If Not Grid.IsNewRowEditing Then
            If e.Column.FieldName = "Factory" Or e.Column.FieldName = "TypeEditGrid" Or e.Column.FieldName = "MachineEditGrid" Or e.Column.FieldName = "ItemCheckEditGrid" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
            End If
        ElseIf Grid.IsNewRowEditing Then
            Dim Time As DateTime
            If e.Column.FieldName = "Start" Then
                Time = Now
                e.Editor.Value = Time
            ElseIf e.Column.FieldName = "End" Then
                Time = Convert.ToDateTime("9999-12-31")
                e.Editor.Value = Time
            End If
        End If

        If e.Column.FieldName = "Factory" Then
            Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            up_FillcomboGrid(combo, "0", pUser)
            If Grid.IsEditing Then combo.Value = e.Value : HF.Set("FactoryEdit", e.Value)
            If Grid.IsNewRowEditing Then combo.Value = cboFactory.Value
        ElseIf e.Column.FieldName = "TypeEditGrid" Then
            Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            up_FillcomboGrid(combo, "1")
            If Grid.IsEditing Then combo.Value = e.Value : HF.Set("TypeEditGrid", e.Value)
            If Grid.IsNewRowEditing Then combo.Value = cboType.Value
        ElseIf e.Column.FieldName = "MachineEditGrid" Then
            Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            AddHandler combo.Callback, AddressOf cmbGridMachine_OnCallback
            If Grid.IsEditing Then Call up_FillcomboGrid(combo, "3", HF.Get("FactoryEdit"), HF.Get("TypeEditGrid")) : combo.Value = e.Value : HF.Set("MachineEditGrid", e.Value)
            If Grid.IsNewRowEditing Then Call up_FillcomboGrid(combo, "3", cboFactory.Value, cboType.Value) : combo.Value = IIf(cboMachine.Text <> "ALL", cboMachine.Value, "")
        ElseIf e.Column.FieldName = "ItemCheckEditGrid" Then
            Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            AddHandler combo.Callback, AddressOf cmbGridItemCheck_OnCallback
            If Grid.IsEditing Then Call up_FillcomboGrid(combo, "5", HF.Get("FactoryEdit"), HF.Get("TypeEditGrid"), HF.Get("MachineEditGrid")) : combo.Value = e.Value
            If Grid.IsNewRowEditing Then Call up_FillcomboGrid(combo, "5", cboFactory.Value, cboType.Value, cboMachine.Value)
        End If

        If e.Column.FieldName = "Factory" Or e.Column.FieldName = "TypeEditGrid" Or e.Column.FieldName = "Start" Or e.Column.FieldName = "End" Then
            e.Editor.Width = "125"
        ElseIf e.Column.FieldName = "MachineEditGrid" Or e.Column.FieldName = "ItemCheckEditGrid" Or e.Column.FieldName = "Remark" Then
            e.Editor.Width = "200"
        ElseIf e.Column.FieldName = "SpecUSL" Or e.Column.FieldName = "SpecLSL" _
            Or e.Column.FieldName = "XUCL" Or e.Column.FieldName = "XLCL" _
            Or e.Column.FieldName = "CPCL" Or e.Column.FieldName = "CPUCL" Or e.Column.FieldName = "CPLCL" _
            Or e.Column.FieldName = "RCL" Or e.Column.FieldName = "RUCL" Or e.Column.FieldName = "RLCL" Then
            e.Editor.Width = "75"
            'Or e.Column.FieldName = "XCL"  di Hapus
        End If
    End Sub

    Private Sub Grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles Grid.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "Load" Then
                up_GridLoad()
            ElseIf pAction = "Kosong" Then
                Dim cls As New clsControlChartSetup With {
                    .Factory = "",
                    .Machine = "",
                    .Type = "",
                    .Period = ""
                }
                dt = clsControlChartSetupDB.GetList(cls)
                Grid.DataSource = dt
                Grid.DataBind()
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_StartRowEditing(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles Grid.StartRowEditing
        If (Not Grid.IsNewRowEditing) Then
            Grid.DoRowValidation()
        End If
        show_error(MsgTypeEnum.Info, "", 0)
    End Sub

    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles Grid.RowInserting
        e.Cancel = True
        Try
            Dim StTime As DateTime = Convert.ToDateTime(e.NewValues("Start"))
            Dim EnTime As DateTime = Convert.ToDateTime(e.NewValues("End"))

            Call up_InsUpd("0", _
                e.NewValues("Factory"), _
                e.NewValues("TypeEditGrid"), _
                e.NewValues("MachineEditGrid"), _
                e.NewValues("ItemCheckEditGrid"), _
                StTime.ToString("yyyy-MM-dd"), StTime.ToString("yyyy-MM-dd"), _
                EnTime.ToString("yyyy-MM-dd"), EnTime.ToString("yyyy-MM-dd"), _
                e.NewValues("SpecUSL"), e.NewValues("SpecUSL"), _
                e.NewValues("SpecLSL"), e.NewValues("SpecLSL"), _
                e.NewValues("XUCL"), e.NewValues("XUCL"), _
                e.NewValues("XLCL"), e.NewValues("XLCL"), _
                e.NewValues("CPCL"), e.NewValues("CPCL"), _
                e.NewValues("CPUCL"), e.NewValues("CPUCL"), _
                e.NewValues("CPLCL"), e.NewValues("CPLCL"), _
                e.NewValues("RCL"), e.NewValues("RCL"), _
                e.NewValues("RLCL"), e.NewValues("RLCL"), _
                e.NewValues("RUCL"), e.NewValues("RUCL"), _
                e.NewValues("Remark"), e.NewValues("Remark"), _
                pUser)
            'e.NewValues("XCL"), e.NewValues("XCL"), _ di Hapus

            Grid.CancelEdit()
            up_GridLoad()
        Catch ex As Exception
			Grid.CancelEdit()
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles Grid.RowUpdating
        e.Cancel = True
        Try
            Dim StTime As DateTime = Convert.ToDateTime(e.NewValues("Start")) : Dim StTimeOld As DateTime = Convert.ToDateTime(e.OldValues("Start"))
            Dim EnTime As DateTime = Convert.ToDateTime(e.NewValues("End")) : Dim EnTimeOld As DateTime = Convert.ToDateTime(e.OldValues("End"))

            Call up_InsUpd("1", _
                e.NewValues("Factory"), _
                e.NewValues("TypeEditGrid"), _
                e.NewValues("MachineEditGrid"), _
                e.NewValues("ItemCheckEditGrid"), _
                StTime.ToString("yyyy-MM-dd"), StTimeOld.ToString("yyyy-MM-dd"), _
                EnTime.ToString("yyyy-MM-dd"), EnTimeOld.ToString("yyyy-MM-dd"), _
                e.NewValues("SpecUSL"), e.OldValues("SpecUSL"), _
                e.NewValues("SpecLSL"), e.OldValues("SpecLSL"), _
                e.NewValues("XUCL"), e.OldValues("XUCL"), _
                e.NewValues("XLCL"), e.OldValues("XLCL"), _
                e.NewValues("CPCL"), e.OldValues("CPCL"), _
                e.NewValues("CPUCL"), e.OldValues("CPUCL"), _
                e.NewValues("CPLCL"), e.OldValues("CPLCL"), _
                e.NewValues("RCL"), e.OldValues("RCL"), _
                e.NewValues("RLCL"), e.OldValues("RLCL"), _
                e.NewValues("RUCL"), e.OldValues("RUCL"), _
                e.NewValues("Remark"), e.OldValues("Remark"), _
                pUser)
            'e.NewValues("XCL"), e.OldValues("XCL"), _ di Hapus

            Grid.CancelEdit()
            up_GridLoad()
        Catch ex As Exception
			Grid.CancelEdit()
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles Grid.RowDeleting
        e.Cancel = True
        Try
            Dim StTime As DateTime = Convert.ToDateTime(e.Values("Start"))

            Dim cls As New clsControlChartSetup With
            {
                .Factory = e.Values("Factory"),
                .ItemType = e.Values("TypeEditGrid"),
                .Machine = e.Values("MachineEditGrid"),
                .ItemCheck = e.Values("ItemCheckEditGrid"),
                .StartTime = StTime.ToString("yyyy-MM-dd")
            }

            clsControlChartSetupDB.Delete(cls)
            Grid.CancelEdit()
            up_GridLoad()
            show_error(MsgTypeEnum.Success, "Delete data successfully!", 1)
        Catch ex As Exception
			Grid.CancelEdit()
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles Grid.RowValidating
        Dim dataCol As New GridViewDataColumn
        Dim tmpdataCol As New GridViewDataColumn
        Dim AdaError As Boolean = False

        For Each column As GridViewColumn In Grid.Columns
            Dim dataColumn As GridViewDataColumn = TryCast(column, GridViewDataColumn)
            If dataColumn Is Nothing Then
                Continue For
            End If

            If dataColumn.FieldName = "Factory" Then
                If IsNothing(e.NewValues("Factory")) OrElse e.NewValues("Factory").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Factory!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "MachineEditGrid" Then
                If IsNothing(e.NewValues("MachineEditGrid")) OrElse e.NewValues("MachineEditGrid").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Machine!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "ItemCheckEditGrid" Then
                If IsNothing(e.NewValues("ItemCheckEditGrid")) OrElse e.NewValues("ItemCheckEditGrid").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Item Check!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "TypeEditGrid" Then
                If IsNothing(e.NewValues("TypeEditGrid")) OrElse e.NewValues("TypeEditGrid").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Type!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "LastUser" Then
                dataCol = dataColumn
            End If
        Next column

        tmpdataCol = Grid.DataColumns("Start")
        If IsNothing(e.NewValues("Start")) OrElse e.NewValues("Start").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "Please Choose Period Start!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("End")
        If IsNothing(e.NewValues("End")) OrElse e.NewValues("End").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "Please Choose Period End!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("SpecUSL")
        If IsNothing(e.NewValues("SpecUSL")) OrElse e.NewValues("SpecUSL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("SpecLSL")
        If IsNothing(e.NewValues("SpecLSL")) OrElse e.NewValues("SpecLSL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        'tmpdataCol = Grid.DataColumns("XCL")
        'If IsNothing(e.NewValues("XCL")) OrElse e.NewValues("XCL").ToString.Trim = "" Then
        '    e.Errors(tmpdataCol) = "please Input a Number!"
        '    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
        '    AdaError = True
        'End If

        tmpdataCol = Grid.DataColumns("XUCL")
        If IsNothing(e.NewValues("XUCL")) OrElse e.NewValues("XUCL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("XLCL")
        If IsNothing(e.NewValues("XLCL")) OrElse e.NewValues("XLCL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("CPCL")
        If IsNothing(e.NewValues("CPCL")) OrElse e.NewValues("CPCL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("CPUCL")
        If IsNothing(e.NewValues("CPUCL")) OrElse e.NewValues("CPUCL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("CPLCL")
        If IsNothing(e.NewValues("CPLCL")) OrElse e.NewValues("CPLCL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        'tmpdataCol = Grid.DataColumns("RCL")
        'If IsNothing(e.NewValues("RCL")) OrElse e.NewValues("RCL").ToString.Trim = "" Then
        '    e.Errors(tmpdataCol) = "please Input a Number!"
        '    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
        '    AdaError = True
        'End If

        tmpdataCol = Grid.DataColumns("RUCL")
        If IsNothing(e.NewValues("RUCL")) OrElse e.NewValues("RUCL").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        'tmpdataCol = Grid.DataColumns("RLCL")
        'If IsNothing(e.NewValues("RLCL")) OrElse e.NewValues("RLCL").ToString.Trim = "" Then
        '    e.Errors(tmpdataCol) = "please Input a Number!"
        '    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
        '    AdaError = True
        'End If

        If e.IsNewRow And Not AdaError Then
            Dim pErr As String = ""
            Dim StTime As DateTime = Convert.ToDateTime(e.NewValues("Start"))
            Dim EnTime As DateTime = Convert.ToDateTime(e.NewValues("End"))

            Dim cls As New clsControlChartSetup With
            {
                .Factory = e.NewValues("Factory"),
                .ItemType = e.NewValues("TypeEditGrid"),
                .Machine = e.NewValues("MachineEditGrid"),
                .ItemCheck = e.NewValues("ItemCheckEditGrid"),
                .StartTime = StTime.ToString("yyyy-MM-dd"),
                .EndTime = EnTime.ToString("yyyy-MM-dd")
            }

            clsControlChartSetupDB.Check(cls, pErr)
            If pErr <> "" Then show_error(MsgTypeEnum.Warning, pErr, 1) : e.Errors(dataCol) = ""
        End If
    End Sub

    Private Sub cboMachine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboMachine.Callback
        Try
            dt = clsControlChartSetupDB.FillCombo("2", cboFactory.Value, cboMachineIOT.Value)
            With cboMachine
                .Items.Clear() : .Columns.Clear()
                .DataSource = dt
                .Columns.Add("Code") : .Columns(0).Visible = False
                .Columns.Add("Description") : .Columns(1).Width = 225

                .TextField = "Description"
                .ValueField = "Code"
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, "", 0)
        End Try
    End Sub

    Private Sub cmbGridMachine_OnCallback(ByVal source As Object, ByVal e As CallbackEventArgsBase)
        Dim cmb As ASPxComboBox = source
        Dim Param As String = Split(e.Parameter, "|")(0)
        Dim ItemType As String = Split(e.Parameter, "|")(1)

        dt = clsControlChartSetupDB.FillCombo("3", Param, ItemType)
        With cmb
            .Items.Clear() : .Columns.Clear()
            .DataSource = dt
            .Columns.Add("Code") : .Columns(0).Visible = False
            .Columns.Add("Description") : .Columns(1).Width = 100

            .TextField = "Description"
            .ValueField = "Code"
            .DataBind()
            .SelectedIndex = -1
        End With
    End Sub

    Private Sub cmbGridItemCheck_OnCallback(ByVal source As Object, ByVal e As CallbackEventArgsBase)
        Dim cmb As ASPxComboBox = source
        Dim Param As String = Split(e.Parameter, "|")(0)
        Dim ItemType As String = Split(e.Parameter, "|")(1)
        Dim Machine As String = Split(e.Parameter, "|")(2)

        dt = clsControlChartSetupDB.FillCombo("5", Param, ItemType, Machine)
        With cmb
            .Items.Clear() : .Columns.Clear()
            .DataSource = dt
            .Columns.Add("Code") : .Columns(0).Visible = False
            .Columns.Add("Description") : .Columns(1).Width = 100

            .TextField = "Description"
            .ValueField = "Code"
            .DataBind()
            .SelectedIndex = -1
        End With
    End Sub

    Private Sub cboLine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLine.Callback
        cboLine.DataSource = clsLineGroupDB.GetList(pUser, cboFactory.Value, IIf(cboProcess.Value = "ALL", "", cboProcess.Value), True)
        cboLine.DataBind()
        cboLine.SelectedIndex = 0
    End Sub

    Private Sub cboMachineIOT_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboMachineIOT.Callback
        cboMachineIOT.DataSource = clsProcessDB.GetList(pUser, cboFactory.Value, IIf(cboProcess.Value = "ALL", "", cboProcess.Value), IIf(cboLine.Value = "ALL", "", cboLine.Value), True)
        cboMachineIOT.DataBind()
        cboMachineIOT.SelectedIndex = 0
    End Sub

    Private Sub cboProcess_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcess.Callback
        cboProcess.DataSource = clsProcessGroupDB.GetList(pUser, cboFactory.Value, True)
        cboProcess.DataBind()
        cboProcess.SelectedIndex = 0
    End Sub
#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Grid.JSProperties("cp_message") = ErrMsg
        Grid.JSProperties("cp_type") = msgType
        Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_Fillcombo()
        Try
            Dim a As String = ""
            dt = clsControlChartSetupDB.FillCombo("0", pUser)
            With cboFactory
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0 'IIf(dt.Rows.Count > 0, 0, -1)
            End With
            If cboFactory.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboFactory.SelectedItem.GetFieldValue("Code")
            End If
            HF.Set("FactoryCode", a)

            cboProcess.DataSource = clsProcessGroupDB.GetList(pUser, cboFactory.Value, True)
            cboProcess.DataBind()
            cboProcess.SelectedIndex = 0

            cboLine.DataSource = clsLineGroupDB.GetList(pUser, cboFactory.Value, cboProcess.Value, True)
            cboLine.DataBind()
            cboLine.SelectedIndex = 0

            cboMachineIOT.DataSource = clsProcessDB.GetList(pUser, cboFactory.Value, cboProcess.Value, cboLine.Value, True)
            cboMachineIOT.DataBind()
            cboMachineIOT.SelectedIndex = 0

            dt = clsControlChartSetupDB.FillCombo("2", cboFactory.Value, cboMachineIOT.Value)
            With cboMachine
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End With
            If cboMachine.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboMachine.SelectedItem.GetFieldValue("Code")
            End If
            HF.Set("MachineCode", a)

            dt = clsControlChartSetupDB.FillCombo("1")
            With cboType
                .DataSource = dt
                .DataBind()
                .SelectedIndex = -1 'IIf(dt.Rows.Count > 0, 0, -1)IIf(dt.Rows.Count > 0, 0, -1)
            End With
            If cboType.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboType.SelectedItem.GetFieldValue("Code")
            End If
            HF.Set("Type", a)

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, "", 0)
        End Try
    End Sub

    Private Sub up_FillcomboGrid(ByVal cmb As ASPxComboBox, Type As String, Optional Param As String = "", Optional Param2 As String = "", Optional Param3 As String = "")
        dt = clsControlChartSetupDB.FillCombo(Type, Param, Param2, Param3)
        With cmb
            .Items.Clear() : .Columns.Clear()
            .DataSource = dt
            .Columns.Add("Code") : .Columns(0).Visible = False
            .Columns.Add("Description") : .Columns(1).Width = 100

            .TextField = "Description"
            .ValueField = "Code"
            .DataBind()
            .SelectedIndex = IIf(Type = 0, 0, -1)
        End With
    End Sub

    Private Sub up_GridLoad()
        Try
            Dim Factory As String = HF.Get("FactoryCode")
            Dim Machine As String = HF.Get("MachineCode")
            Dim Type As String = HF.Get("Type")
            Dim Period As String = Convert.ToDateTime(dtPeriod.Value).ToString("yyyy-MM-dd")

            Dim cls As New clsControlChartSetup With {
                .Factory = Factory,
                .Machine = Machine,
                .Type = Type,
                .Period = Period
            }

            up_FillComboFilter(Factory, Machine, Type)

            dt = clsControlChartSetupDB.GetList(cls)
            Grid.DataSource = dt
            Grid.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Private Sub up_InsUpd(Type As String, Factory As String, ItemType As String, Machine As String, ItemCheck As String, _
                               Start As String, StartOld As String, _
                               EndTime As String, EndTimeOld As String, _
                               SpecUSL As String, SpecUSLOld As String, _
                               SpecLSL As String, SpecLSLOld As String, _
                               XBarUCL As String, XBarUCLOld As String, _
                               XBarLCL As String, XBarLCLOld As String, _
                               CPCL As String, CPCLOld As String, _
                               CPUCL As String, CPUCLOld As String, _
                               CPLCL As String, CPLCLOld As String, _
                               RCL As String, RCLOld As String, _
                               RLCL As String, RLCLOld As String, _
                               RUCL As String, RUCLOld As String, _
                               Remark As String, RemarkOld As String, _
                               User As String)
        'XBarCL As String, XBarCLOld As String, _ di Hapus Parameter nya

        Dim message As String = IIf(Type = "0", "Save data successfully!", "Update data successfully!") '0 Save | 1 Update
        Try
            '.XBarCL = XBarCL, .XBarCLOld = XBarCLOld, --di Hapus
            Dim cls As New clsControlChartSetup With
            {
                .Factory = Factory,
                .ItemType = ItemType,
                .ItemCheck = ItemCheck,
                .Machine = Machine,
                .StartTime = Start, .StartTimeOld = StartOld,
                .EndTime = EndTime, .EndTimeOld = EndTimeOld,
                .SpecUSL = SpecUSL, .SpecUSLOld = SpecUSLOld,
                .SpecLSL = SpecLSL, .SpecLSLOld = SpecLSLOld,
                .XBarUCL = XBarUCL, .XBarUCLOld = XBarUCLOld,
                .XBarLCL = XBarLCL, .XBarLCLOld = XBarLCLOld,
                .CPCL = CPCL, .CPCLOld = CPCLOld,
                .CPUCL = CPUCL, .CPUCLOld = CPUCLOld,
                .CPLCL = CPLCL, .CPLCLOld = CPLCLOld,
                .RCL = IIf(RCL Is Nothing, 0, RCL), .RCLOld = IIf(RCLOld Is Nothing, 0, RCLOld),
                .RLCL = IIf(RLCL Is Nothing, 0, RLCL), .RLCLOld = IIf(RLCLOld Is Nothing, 0, RLCLOld),
                .RUCL = RUCL, .RUCLOld = RUCLOld,
                .Remark = Remark, .RemarkOld = RemarkOld,
                .User = User
            }
            clsControlChartSetupDB.InsertUpdate(cls, Type)
            clsControlChartSetupDB.Email(cls, Type)
            show_error(MsgTypeEnum.Success, message, 1)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub up_FillComboFilter(Factory, Machine, Type)
        'Type
        Dim ds As New SqlDataSource
        With ds
            .ConnectionString = Sconn.Stringkoneksi
            .SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            .SelectCommand = "sp_SPC_ChartSetup_FillCombo"
            .SelectParameters.Add("Type", "7")
            .SelectParameters.Add("Param", Factory)
            .SelectParameters.Add("Param2", Machine)
            .SelectParameters.Add("Param3", Type)
        End With

        Dim combo As GridViewDataComboBoxColumn = TryCast(Grid.Columns("Type"), GridViewDataComboBoxColumn)
        With combo
            .PropertiesComboBox.ValueType = GetType(String)
            .PropertiesComboBox.DataSource = ds
            .PropertiesComboBox.TextField = "Description"
            .PropertiesComboBox.ValueField = "Code"
            .PropertiesComboBox.TextFormatString = "{0}"
            .PropertiesComboBox.IncrementalFilteringMode = IncrementalFilteringMode.Contains
        End With

        'Line
        Dim ds2 As New SqlDataSource
        With ds2
            .ConnectionString = Sconn.Stringkoneksi
            .SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            .SelectCommand = "sp_SPC_ChartSetup_FillCombo"
            .SelectParameters.Add("Type", "4")
            .SelectParameters.Add("Param", Factory)
            .SelectParameters.Add("Param2", Machine)
            .SelectParameters.Add("Param3", Type)
        End With

        Dim combo2 As GridViewDataComboBoxColumn = TryCast(Grid.Columns("Machine"), GridViewDataComboBoxColumn)
        With combo2
            .PropertiesComboBox.ValueType = GetType(String)
            .PropertiesComboBox.DataSource = ds2
            .PropertiesComboBox.TextField = "Description"
            .PropertiesComboBox.ValueField = "Code"
            .PropertiesComboBox.TextFormatString = "{0}"
            .PropertiesComboBox.IncrementalFilteringMode = IncrementalFilteringMode.Contains
        End With

        'Item Check
        Dim ds3 As New SqlDataSource
        With ds3
            .ConnectionString = Sconn.Stringkoneksi
            .SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            .SelectCommand = "sp_SPC_ChartSetup_FillCombo"
            .SelectParameters.Add("Type", "6")
            .SelectParameters.Add("Param", Factory)
            .SelectParameters.Add("Param2", Machine)
            .SelectParameters.Add("Param3", Type)
        End With

        Dim combo3 As GridViewDataComboBoxColumn = TryCast(Grid.Columns("ItemCheck"), GridViewDataComboBoxColumn)
        With combo3
            .PropertiesComboBox.ValueType = GetType(String)
            .PropertiesComboBox.DataSource = ds3
            .PropertiesComboBox.TextField = "Description"
            .PropertiesComboBox.ValueField = "Code"
            .PropertiesComboBox.TextFormatString = "{0}"
        End With

        'Characteristic Status
        Dim ds4 As New SqlDataSource
        With ds4
            .ConnectionString = Sconn.Stringkoneksi
            .SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            .SelectCommand = "sp_SPC_ChartSetup_FillCombo"
            .SelectParameters.Add("Type", "8")
        End With

        Dim combo4 As GridViewDataComboBoxColumn = TryCast(Grid.Columns("Characteristic"), GridViewDataComboBoxColumn)
        With combo4
            .PropertiesComboBox.ValueType = GetType(String)
            .PropertiesComboBox.DataSource = ds4
            .PropertiesComboBox.TextField = "Description"
            .PropertiesComboBox.ValueField = "Code"
            .PropertiesComboBox.TextFormatString = "{0}"
        End With

    End Sub
#End Region
    
End Class