﻿Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing

Public Class ItemCheckByBattery
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Public AuthAccess As Boolean = False
    Public AuthInsert As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public dt As DataTable
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            GetFactoryCode()
            '    dsRegNo.SelectParameters("Type").DefaultValue = "6"
            '    dsRegNo.SelectParameters("FactoryCode1").DefaultValue = "F001"
            '    'FillCBRegNoGrid()
            'Else
            '    dsRegNo.SelectParameters("Type").DefaultValue = "6"
            '    dsRegNo.SelectParameters("FactoryCode1").DefaultValue = cboFactory.Value
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu("A020")
        Master.SiteTitle = sGlobal.menuName

        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "A020")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "A020")
        If AuthUpdate = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowEditButton = False
            commandColumn.ShowNewButtonInHeader = False
        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "A020")
        If AuthDelete = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowDeleteButton = False
        End If

    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then
            up_GridLoad(cboFactory.Value, cboType.Text, cboLine.Text)
        End If
    End Sub
    Private Sub GetFactoryCode()
        cboFactory.DataSource = clsFactoryDB.GetList
        cboFactory.DataBind()
    End Sub
    Private Sub FillCBRegNoGrid()
        Dim DT As New DataTable
        DT = ClsSPCItemCheckByTypeDB.GetRegNo(cboFactory.Value)
        'Dim myCombo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
        'CType(TryCast(sender, ASPxGridView).Columns("RegistrationNo"), GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = DT
        'myCombo.DataSource = DT
        'myCombo.DataBindItems()

        Dim comboColumn = CType(Grid.Columns("RegistrationNo"), GridViewDataComboBoxColumn)
        comboColumn.PropertiesComboBox.DataSource = DT
        comboColumn.PropertiesComboBox.TextField = "Description"
        comboColumn.PropertiesComboBox.ValueField = "RegistrationNo"
        comboColumn.PropertiesComboBox.ValueType = GetType(String)

    End Sub
    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles Grid.RowInserting
        e.Cancel = True
        Dim pErr As String = ""
        Dim LineCode As String = ""
        Dim ItemCheck As String = ""
        LineCode = e.NewValues("LineName")
        ItemCheck = e.NewValues("ItemCheck")
        Dim BatteryType As New ClsSPCItemCheckByType With {
            .FactoryCode = e.NewValues("FactoryCode"),
            .FactoryName = cboFactory.Text,
            .ItemTypeCode = e.NewValues("ItemTypeCode"),
            .LineCode = LineCode.Substring(0, LineCode.IndexOf(" -")),
            .ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -")),
            .FrequencyCode = e.NewValues("FrequencyCode"),
            .RegistrationNo = e.NewValues("RegistrationNo"),
            .SampleSize = e.NewValues("SampleSize"),
            .Remark = e.NewValues("Remark"),
            .Evaluation = e.NewValues("Evaluation"),
            .CharacteristicItem = e.NewValues("CharacteristicStatus"),
            .ActiveStatus = e.NewValues("ActiveStatus"),
            .UpdateUser = pUser,
            .CreateUser = pUser
        }
        Try
            If IsNothing(BatteryType.Remark) Then
                BatteryType.Remark = ""
            End If
            If IsNothing(BatteryType.Evaluation) Then
                BatteryType.Evaluation = ""
            End If
            Dim CheckDataBattery As ClsSPCItemCheckByType = ClsSPCItemCheckByTypeDB.GetData(BatteryType.FactoryCode, BatteryType.ItemTypeCode, BatteryType.LineCode, BatteryType.ItemCheck)
            If CheckDataBattery IsNot Nothing Then
                show_error(MsgTypeEnum.ErrorMsg, "Can't insert data, Battery type '" + CheckDataBattery.ItemTypeName + "' for item check '" + BatteryType.ItemCheck + "' on machine '" + BatteryType.LineCode + "' in factory '" + BatteryType.FactoryName + "' is already registered", 1)
                Return
            End If
            ClsSPCItemCheckByTypeDB.Insert(BatteryType)
            Grid.CancelEdit()
            up_GridLoad(cboFactory.Value, cboType.Text, cboLine.Text)
            show_error(MsgTypeEnum.Success, "Save data successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles Grid.RowUpdating
        e.Cancel = True
        Dim pErr As String = ""
        Dim LineCode As String = ""
        Dim ItemCheck As String = ""
        LineCode = e.NewValues("LineName")
        ItemCheck = e.NewValues("ItemCheck")
        Dim BatteryType As New ClsSPCItemCheckByType With {
            .FactoryCode = e.NewValues("FactoryCode"),
            .ItemTypeCode = e.NewValues("ItemTypeCode"),
            .LineCode = LineCode.Substring(0, LineCode.IndexOf(" -")),
            .ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -")),
            .FrequencyCode = e.NewValues("FrequencyCode"),
            .RegistrationNo = e.NewValues("RegistrationNo"),
            .SampleSize = e.NewValues("SampleSize"),
            .Remark = e.NewValues("Remark"),
            .Evaluation = e.NewValues("Evaluation"),
            .CharacteristicItem = e.NewValues("CharacteristicStatus"),
            .ActiveStatus = e.NewValues("ActiveStatus"),
            .UpdateUser = pUser,
            .CreateUser = pUser
        }
        Try
            If IsNothing(BatteryType.Remark) Then
                BatteryType.Remark = ""
            End If
            If IsNothing(BatteryType.Evaluation) Then
                BatteryType.Evaluation = ""
            End If
            ClsSPCItemCheckByTypeDB.Update(BatteryType)
            Grid.CancelEdit()
            up_GridLoad(cboFactory.Value, cboType.Text, cboLine.Text)
            show_error(MsgTypeEnum.Success, "Update data successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles Grid.RowDeleting
        e.Cancel = True
        Try
            Dim FactoryCode As String = e.Values("FactoryCode")
            Dim ItemTypeCode As String = e.Values("ItemTypeCode")
            Dim LineCode As String = e.Values("LineName")
            Dim ItemCheck As String = e.Values("ItemCheck")

            LineCode = LineCode.Substring(0, LineCode.IndexOf(" -"))
            ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

            Dim ValidationDelete As ClsSPCItemCheckByType = ClsSPCItemCheckByTypeDB.ValidationDelete(FactoryCode, ItemTypeCode, LineCode, ItemCheck)
            If ValidationDelete IsNot Nothing Then
                show_error(MsgTypeEnum.ErrorMsg, "Can't Delete, item check '" + ItemCheck + "' On machine '" + LineCode + "' has been used in Production Sample Input", 1)
                Return
            End If
            ClsSPCItemCheckByTypeDB.Delete(FactoryCode, ItemTypeCode, LineCode, ItemCheck)
            Grid.CancelEdit()
            up_GridLoad(cboFactory.Value, cboType.Text, cboLine.Text)
            show_error(MsgTypeEnum.Success, "Delete data successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Private Sub Grid_BeforeGetCallbackResult(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid.BeforeGetCallbackResult
        If Grid.IsNewRowEditing Then
            Grid.SettingsCommandButton.UpdateButton.Text = "Save"
        End If
    End Sub

    Private Sub Grid_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles Grid.CellEditorInitialize
        'If (e.Column.FieldName = "RegistrationNo") Then
        '    Dim DT As New DataTable
        '    DT = ClsSPCItemCheckByTypeDB.GetRegNo(cboFactory.Value)
        '    Dim myCombo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
        '    CType(TryCast(sender, ASPxGridView).Columns("RegistrationNo"), GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = DT
        '    myCombo.DataSource = DT
        '    myCombo.DataBindItems()
        'End If
        If Not Grid.IsNewRowEditing Then
            If e.Column.FieldName = "ItemCheckCode" Or e.Column.FieldName = "FactoryCode" Or e.Column.FieldName = "ItemTypeCode" Or e.Column.FieldName = "LineName" Or e.Column.FieldName = "ItemTypeCode" Or e.Column.FieldName = "ItemCheck" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = True
            End If
        End If

        If Grid.IsNewRowEditing Then
            'If e.Column.FieldName = "FactoryCode" Then
            '    e.Editor.Value = cboFactory.Value
            'End If
            'If e.Column.FieldName = "ItemTypeCode" Then
            '    e.Editor.Visible = True
            '    e.Editor.ForeColor = Color.Silver
            'End If
            If e.Column.FieldName = "ActiveStatus" Then
                TryCast(e.Editor, ASPxCheckBox).Checked = True
            End If
        End If
    End Sub
    Protected Sub Grid_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles Grid.RowValidating
        Dim GridColumn As New GridViewDataColumn

        GridColumn = Grid.DataColumns("FactoryCode")
        If IsNothing(e.NewValues("FactoryCode")) OrElse e.NewValues("FactoryCode").ToString.Trim = "" Then
            e.Errors(GridColumn) = "Factory Name Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Factory Name Must Be Filled !", 1)
            Return
        End If

        GridColumn = Grid.DataColumns("ItemTypeCode")
        If IsNothing(e.NewValues("ItemTypeCode")) OrElse e.NewValues("ItemTypeCode").ToString.Trim = "" Then
            e.Errors(GridColumn) = "Type Name Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Type Name Must Be Filled !", 1)
            Return
        End If

        GridColumn = Grid.DataColumns("LineName")
        If IsNothing(e.NewValues("LineName")) OrElse e.NewValues("LineName").ToString.Trim = "" Then
            e.Errors(GridColumn) = "Machine Proccess Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Machine Proccess Must Be Filled !", 1)
            Return
        End If

        GridColumn = Grid.DataColumns("ItemCheck")
        If IsNothing(e.NewValues("ItemCheck")) OrElse e.NewValues("ItemCheck").ToString.Trim = "" Then
            e.Errors(GridColumn) = "Item Check Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Item Check Must Be Filled !", 1)
            Return
        End If

        GridColumn = Grid.DataColumns("FrequencyCode")
        If IsNothing(e.NewValues("FrequencyCode")) OrElse e.NewValues("FrequencyCode").ToString.Trim = "" Then
            e.Errors(GridColumn) = "Frequency Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Frequency Must Be Filled !", 1)
            Return
        End If

        GridColumn = Grid.DataColumns("RegistrationNo")
        If IsNothing(e.NewValues("RegistrationNo")) OrElse e.NewValues("RegistrationNo").ToString.Trim = "" Then
            e.Errors(GridColumn) = "Registration Number Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Registration Number Must Be Filled !", 1)
            Return
        End If

        GridColumn = Grid.DataColumns("SampleSize")
        If IsNothing(e.NewValues("SampleSize")) OrElse e.NewValues("SampleSize").ToString.Trim = "" Then
            e.Errors(GridColumn) = "Sample Size Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Sample Size Must Be Filled !", 1)
            Return
        End If

    End Sub

    Protected Sub Grid_StartRowEditing(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles Grid.StartRowEditing
        If (Not Grid.IsNewRowEditing) Then
            Grid.DoRowValidation()
        End If
        show_error(MsgTypeEnum.Info, "", 0)
    End Sub
#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Grid.JSProperties("cp_message") = ErrMsg
        Grid.JSProperties("cp_type") = msgType
        Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_GridLoad(FactoryCode As String, ItemTypeDescription As String, MachineProccess As String)
        Dim dtItemCheckByType As DataTable
        Try
            If FactoryCode Is Nothing Then
                FactoryCode = ""
            End If
            If ItemTypeDescription Is Nothing Then
                ItemTypeDescription = ""
            End If
            If MachineProccess Is Nothing Then
                MachineProccess = ""
            End If

            If MachineProccess <> "ALL" AndAlso MachineProccess <> "" Then
                MachineProccess = MachineProccess.Substring(0, MachineProccess.IndexOf(" -"))
            End If

            dtItemCheckByType = ClsSPCItemCheckByTypeDB.GetList(pUser, FactoryCode, ItemTypeDescription, MachineProccess, cboType.Value)
            Grid.DataSource = dtItemCheckByType
            Grid.DataBind()

            hdUserLogin.Value = pUser
            hdFactoryCode.Value = FactoryCode
            hdItemTypeCode.Value = cboType.Value

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub Grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles Grid.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "Load" Then
                up_GridLoad(cboFactory.Value, cboType.Text, cboLine.Text)
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub cboType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboType.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        cboType.DataSource = clsItemTypeDB.GetList(FactoryCode)
        cboType.DataBind()
        'FillCBRegNoGrid()
        'dsRegNo.SelectParameters("FactoryCode1").DefaultValue = cboFactory.Value
    End Sub
    Private Sub cboLine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLine.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim UserID As String = Session("user") & ""
        cboLine.DataSource = ClsSPCItemCheckByTypeDB.GetMachineProccess(UserID, FactoryCode, ItemTypeCode)
        cboLine.DataBind()
    End Sub
#End Region

End Class