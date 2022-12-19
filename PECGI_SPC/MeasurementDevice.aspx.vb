Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing

Public Class MeasurementDevice
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Dim pMenuID As String = "Y010 "
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False
    Private dt As DataTable
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
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

    Private Sub Grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles Grid.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "Load" Then
                up_GridLoad()
            ElseIf pAction = "Kosong" Then
                dt = clsMeasurementDeviceDB.GetList("")
                Grid.DataSource = dt
                Grid.DataBind()
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
            up_GridLoad()
        End If
    End Sub

    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles Grid.RowInserting
        e.Cancel = True
        Try
            Call up_InsUpd("0", _
                e.NewValues("FactoryCode"), _
                e.NewValues("RegNo"), _
                e.NewValues("Description"), _
                e.NewValues("ToolName"), _
                e.NewValues("ToolFunction"), _
                e.NewValues("BaudRate"), _
                e.NewValues("DataBits"), _
                e.NewValues("Parity"), _
                e.NewValues("StopBits"), _
                e.NewValues("Stable"), _
                e.NewValues("Passive"), _
                e.NewValues("GetResult"), _
                IIf(e.NewValues("ActiveStatus") Is Nothing, "0", e.NewValues("ActiveStatus")),
                pUser)
            Grid.CancelEdit()
            up_GridLoad()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles Grid.RowUpdating
        e.Cancel = True
        Try
            Call up_InsUpd("1", _
                 e.NewValues("FactoryCode"), _
                 e.NewValues("RegNo"), _
                 e.NewValues("Description"), _
                 e.NewValues("ToolName"), _
                 e.NewValues("ToolFunction"), _
                 e.NewValues("BaudRate"), _
                 e.NewValues("DataBits"), _
                 e.NewValues("Parity"), _
                 e.NewValues("StopBits"), _
                 e.NewValues("Stable"), _
                 e.NewValues("Passive"), _
                 e.NewValues("GetResult"), _
                 IIf(e.NewValues("ActiveStatus") Is Nothing, "0", e.NewValues("ActiveStatus")),
                 pUser)
            Grid.CancelEdit()
            up_GridLoad()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles Grid.RowDeleting
        e.Cancel = True
        Try
            Dim cls As New clsMeasurementDevice With
            {
                .FactoryCode = e.Values("FactoryCode"),
                .RegNo = e.Values("RegNo")
            }

            clsMeasurementDeviceDB.Delete(cls)
            Grid.CancelEdit()
            up_GridLoad()
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
        If Not Grid.IsNewRowEditing Then
            If e.Column.FieldName = "RegNo" Or e.Column.FieldName = "FactoryCode" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
            End If
        ElseIf Grid.IsNewRowEditing Then
            If e.Column.FieldName = "ActiveStatus" Then
                TryCast(e.Editor, ASPxCheckBox).Checked = True
            End If
        End If

        If e.Column.FieldName = "RegNo" Or e.Column.FieldName = "Description" Or e.Column.FieldName = "ToolName" Or e.Column.FieldName = "ToolFunction" Then
            e.Editor.Width = "200"
        Else
            e.Editor.Width = "75"
        End If
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

            If dataColumn.FieldName = "FactoryCode" Then
                If IsNothing(e.NewValues("FactoryCode")) OrElse e.NewValues("FactoryCode").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Factory!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "RegNo" Then
                If IsNothing(e.NewValues("RegNo")) OrElse e.NewValues("RegNo").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Registraion No!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "Description" Then
                If IsNothing(e.NewValues("Description")) OrElse e.NewValues("Description").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Description!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "ToolName" Then
                If IsNothing(e.NewValues("ToolName")) OrElse e.NewValues("ToolName").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Tool Name!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "ToolFunction" Then
                If IsNothing(e.NewValues("ToolFunction")) OrElse e.NewValues("ToolFunction").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Tool Function!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "BaudRate" Then
                If IsNothing(e.NewValues("BaudRate")) OrElse e.NewValues("BaudRate").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Baud Rate!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "DataBits" Then
                If IsNothing(e.NewValues("DataBits")) OrElse e.NewValues("DataBits").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Data Bits!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "Parity" Then
                If IsNothing(e.NewValues("Parity")) OrElse e.NewValues("Parity").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Parity!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "StopBits" Then
                If IsNothing(e.NewValues("StopBits")) OrElse e.NewValues("StopBits").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Stop Bits!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "Passive" Then
                If IsNothing(e.NewValues("Passive")) OrElse e.NewValues("Passive").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Passive!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "LastUser" Then
                dataCol = dataColumn
            End If
        Next column

        tmpdataCol = Grid.DataColumns("Stable")
        If IsNothing(e.NewValues("Stable")) OrElse e.NewValues("Stable").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "Please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        tmpdataCol = Grid.DataColumns("GetResult")
        If IsNothing(e.NewValues("GetResult")) OrElse e.NewValues("GetResult").ToString.Trim = "" Then
            e.Errors(tmpdataCol) = "Please Input a Number!"
            show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
            AdaError = True
        End If

        If Not AdaError And e.IsNewRow Then
            Dim pErr As String = ""

            Dim cls As New clsMeasurementDevice With
            {
                .FactoryCode = e.NewValues("FactoryCode"),
                .RegNo = e.NewValues("RegNo")
            }

            clsMeasurementDeviceDB.Check(cls, pErr)
            If pErr <> "" Then show_error(MsgTypeEnum.Warning, pErr, 1) : e.Errors(dataCol) = ""
        End If
    End Sub

    Protected Sub Grid_StartRowEditing(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles Grid.StartRowEditing
        If (Not Grid.IsNewRowEditing) Then
            Grid.DoRowValidation()
        End If
        show_error(MsgTypeEnum.Info, "", 0)
    End Sub

    Private Sub cboFactory_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboFactory.Callback
        Try
            dt = clsMeasurementDeviceDB.GetList("")
            Grid.DataSource = dt
            Grid.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Grid.JSProperties("cp_message") = ErrMsg
        Grid.JSProperties("cp_type") = msgType
        Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_Fillcombo()
        Dim a As String = ""
        Try
            dt = clsMeasurementDeviceDB.FillCombo(0)

            With cboFactory
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)

                If .SelectedIndex < 0 Then
                    a = ""
                Else
                    a = .SelectedItem.GetFieldValue("Code")
                End If
            End With
            
            HF.Set("FactoryCode", a)
        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "", 0)
        End Try
    End Sub

    Private Sub up_GridLoad()
        Try
            Dim a As String = HF.Get("FactoryCode")

            dt = clsMeasurementDeviceDB.GetList(a)
            Grid.DataSource = dt
            Grid.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Private Function up_InsUpd(Type As String, Factory As String, regno As String, desc As String, toolname As String, toolfunc As String, baud As String, databit As String, parity As String, stopbit As String, stable As String, passive As String, getresult As String, active As String, User As String) As Boolean
        Dim message As String = IIf(Type = "0", "Save data successfully!", "Update data successfully!") '0 Save | 1 Update
        Try
            Dim cls As New clsMeasurementDevice With
            {
                .FactoryCode = Factory,
                .RegNo = regno,
                .Description = desc,
                .ToolName = toolname,
                .ToolFunction = toolfunc,
                .BaudRate = baud,
                .DataBit = databit,
                .Parity = parity,
                .StopBit = stopbit,
                .Stable = stable,
                .Passive = passive,
                .GetResult = getresult,
                .Active = active,
                .User = User
            }
            clsMeasurementDeviceDB.InsertUpdate(cls, Type)
            show_error(MsgTypeEnum.Success, message, 1)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

End Class