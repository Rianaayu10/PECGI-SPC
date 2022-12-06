Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing

Public Class TimeFrequencySetting
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Dim pMenuID As String = "A030"
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
                dt = clsTimeFrequencySettingDB.GetList("")
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
            Dim StTime As DateTime = Convert.ToDateTime(e.NewValues("Start"))
            Dim EnTime As DateTime = Convert.ToDateTime(e.NewValues("End"))
            Dim verifTime As DateTime = Convert.ToDateTime(e.NewValues("Verif"))

            Call up_InsUpd("0", _
                e.NewValues("Frequency"), _
                e.NewValues("No"), _
                e.NewValues("Shift"), _
                StTime.ToString("HH:mm"), _
                EnTime.ToString("HH:mm"), _
                verifTime.ToString("HH:mm"), _
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
            Dim StTime As DateTime = Convert.ToDateTime(e.NewValues("Start"))
            Dim EnTime As DateTime = Convert.ToDateTime(e.NewValues("End"))
            Dim StTimeOld As DateTime = Convert.ToDateTime(e.OldValues("Start"))
            Dim EnTimeOld As DateTime = Convert.ToDateTime(e.OldValues("End"))
            Dim verifTime As DateTime = Convert.ToDateTime(e.NewValues("Verif"))

            Call up_InsUpd("1", _
                e.NewValues("Frequency"), _
                e.NewValues("No"), _
                e.NewValues("Shift"), _
                StTime.ToString("HH:mm"), _
                EnTime.ToString("HH:mm"), _
                verifTime.ToString("HH:mm"), _
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
            Dim cls As New clsTimeFrequencySetting With
            {
                .FrequencyCode = e.Values("Frequency"),
                .Nomor = e.Values("No"),
                .Shift = e.Values("Frequency")
            }

            clsTimeFrequencySettingDB.Delete(cls)
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
            If e.Column.FieldName = "No" Or e.Column.FieldName = "Frequency" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
            End If
        End If

        If e.Column.FieldName = "Frequency" Or e.Column.FieldName = "Shift" Or e.Column.FieldName = "No" Then
            e.Editor.Width = "75"
        End If
    End Sub

    Protected Sub Grid_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles Grid.RowValidating
        Dim dataCol As New GridViewDataColumn
        Dim AdaError As Boolean = False

        For Each column As GridViewColumn In Grid.Columns
            Dim dataColumn As GridViewDataColumn = TryCast(column, GridViewDataColumn)
            If dataColumn Is Nothing Then
                Continue For
            End If

            If dataColumn.FieldName = "Frequency" Then
                If IsNothing(e.NewValues("Frequency")) OrElse e.NewValues("Frequency").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Frequency!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "Shift" Then
                If IsNothing(e.NewValues("Shift")) OrElse e.NewValues("Shift").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Choose Shift!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "Start" Then
                If IsNothing(e.NewValues("Start")) OrElse e.NewValues("Start").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Start Time!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "End" Then
                If IsNothing(e.NewValues("End")) OrElse e.NewValues("End").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input End Time!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "Verif" Then
                If IsNothing(e.NewValues("Verif")) OrElse e.NewValues("Verif").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Verif Time!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "LastUser" Then
                dataCol = dataColumn
            End If
        Next column

        If Not AdaError Then
            Dim pErr As String = ""
            Dim StTime As DateTime = Convert.ToDateTime(e.NewValues("Start"))
            Dim EnTime As DateTime = Convert.ToDateTime(e.NewValues("End"))

            If StTime = EnTime Then show_error(MsgTypeEnum.Warning, "Cannot Insert Start " + StTime.ToString("HH:mm") + " - " + EnTime.ToString("HH:mm") + " in This Frequency " + cboFreq.Text + ", Because it is Overlapping with Existing Data. Please Check Again", 1) : e.Errors(dataCol) = "" : Exit Sub

            Dim cls As New clsTimeFrequencySetting With
            {
                .FrequencyCode = e.NewValues("Frequency"),
                .Shift = e.NewValues("Shift"),
                .StartTime = StTime.ToString("HH:mm"),
                .EndTime = EnTime.ToString("HH:mm"),
                .Nomor = IIf(e.IsNewRow, 0, e.NewValues("No"))
            }

            If Convert.ToDateTime(e.NewValues("Start")).ToString("HH:mm") <> Convert.ToDateTime(e.OldValues("Start")).ToString("HH:mm") Or _
                Convert.ToDateTime(e.NewValues("End")).ToString("HH:mm") <> Convert.ToDateTime(e.OldValues("End")).ToString("HH:mm") Then
                clsTimeFrequencySettingDB.Check(cls, pErr)
            End If

            If pErr <> "" Then show_error(MsgTypeEnum.Warning, pErr, 1) : e.Errors(dataCol) = ""
        End If
    End Sub

    Protected Sub Grid_StartRowEditing(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles Grid.StartRowEditing
        If (Not Grid.IsNewRowEditing) Then
            Grid.DoRowValidation()
        End If
        show_error(MsgTypeEnum.Info, "", 0)
    End Sub

    Private Sub cboFreq_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboFreq.Callback
        Try
            dt = clsTimeFrequencySettingDB.GetList("")
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
            dt = clsTimeFrequencySettingDB.FillCombo(0)

            With cboFreq
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End With

            If cboFreq.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboFreq.SelectedItem.GetFieldValue("Code")
            End If
            HF.Set("Code", a)
        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "", 0)
        End Try
    End Sub

    Private Sub up_GridLoad()
        Try
            Dim a As String = HF.Get("Code")
            
            dt = clsTimeFrequencySettingDB.GetList(a)
            Grid.DataSource = dt
            Grid.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Private Function up_InsUpd(Type As String, Frequency As String, Nomor As String, Shift As String, Start As String, EndTime As String, verifTime As String, User As String) As Boolean
        Dim message As String = IIf(Type = "0", "Save data successfully!", "Update data successfully!") '0 Save | 1 Update
        Try
            Dim cls As New clsTimeFrequencySetting With
            {
                .FrequencyCode = Frequency,
                .Nomor = Nomor,
                .Shift = Shift,
                .StartTime = Start,
                .EndTime = EndTime,
                .VerifTime = verifTime,
                .User = User
            }
            clsTimeFrequencySettingDB.InsertUpdate(cls, Type)
            show_error(MsgTypeEnum.Success, message, 1)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region
    
End Class