Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing

Public Class ItemCheck
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Public AuthInsert As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            up_GridLoad()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'PENGECEKAN VALIDASI TOKEN
        If Session("Action") IsNot Nothing Then
            If Session("Action").ToString = "SSO" Then
                Dim token = Session("token")
                Dim SSOHost As String = ConfigurationManager.AppSettings("SSOUrl").ToString()
                If sGlobal.VerifyToken(token, SSOHost) = False Then
                    Response.Redirect(SSOHost + "/account/login?logout=1")
                End If
            End If
        End If

        'PENGECEKAN SESSION USER
        If Session("user") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If

        sGlobal.getMenu("A010")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName

        'pUser = Session("user")
        'AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "A010")
        'show_error(MsgTypeEnum.Info, "", 0)
        'If AuthUpdate = False Then
        '    Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
        '    commandColumn.Visible = False
        'End If

        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "A010")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "A010")
        If AuthUpdate = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowEditButton = False
            commandColumn.ShowNewButtonInHeader = False
        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "A010")
        If AuthDelete = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowDeleteButton = False
        End If
    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
            up_GridLoad()
        End If
    End Sub

    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles Grid.RowInserting
        e.Cancel = True
        Dim pErr As String = ""
        Dim ItemCheck As New ClsSPCItemCheckMaster With {
            .ItemCheckCode = e.NewValues("ItemCheckCode"),
            .ItemCheck = e.NewValues("ItemCheck"),
            .UnitMeasurement = e.NewValues("UnitMeasurement"),
            .DecimalDigit = e.NewValues("DecimalDigit"),
            .Description = e.NewValues("Description"),
            .Measure2Cls = e.NewValues("Measure2Cls"),
            .ActiveStatus = e.NewValues("ActiveStatus"),
            .UpdateUser = pUser,
            .CreateUser = pUser
        }
        Try
            Dim CheckItemMaster As ClsSPCItemCheckMaster = ClsSPCItemCheckMasterDB.GetData(ItemCheck.ItemCheckCode)
            If CheckItemMaster IsNot Nothing Then
                show_error(MsgTypeEnum.ErrorMsg, "Can't insert data, item code '" + ItemCheck.ItemCheckCode + "' already exists!", 1)
                Return
            End If
            If IsNothing(ItemCheck.Description) Then
                ItemCheck.Description = ""
            End If
            ClsSPCItemCheckMasterDB.Insert(ItemCheck)
            Grid.CancelEdit()
            up_GridLoad()
            show_error(MsgTypeEnum.Success, "Save data successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles Grid.RowUpdating
        e.Cancel = True
        Dim User As New ClsSPCItemCheckMaster With {
            .ItemCheckCode = e.NewValues("ItemCheckCode"),
            .ItemCheck = e.NewValues("ItemCheck"),
            .UnitMeasurement = e.NewValues("UnitMeasurement"),
            .DecimalDigit = e.NewValues("DecimalDigit"),
            .Description = e.NewValues("Description"),
            .Measure2Cls = e.NewValues("Measure2Cls"),
            .ActiveStatus = e.NewValues("ActiveStatus"),
            .UpdateUser = pUser
        }
        Try
            If IsNothing(User.Description) Then
                User.Description = ""
            End If
            ClsSPCItemCheckMasterDB.Update(User)
            Grid.CancelEdit()
            up_GridLoad()
            show_error(MsgTypeEnum.Success, "Update data successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles Grid.RowDeleting
        e.Cancel = True
        Try
            Dim ItemCheckCode As String = e.Values("ItemCheckCode")
            Dim ItemCheck As String = e.Values("ItemCheck")
            Dim Description As String = e.Values("Description")
            Dim ActiveStatus As String = e.Values("ActiveStatus")
            Dim ValidationDelete As ClsSPCItemCheckMaster = ClsSPCItemCheckMasterDB.ValidationDelete(ItemCheckCode)
            If ValidationDelete IsNot Nothing Then
                show_error(MsgTypeEnum.ErrorMsg, "Can't Delete, item '" + ItemCheckCode + "' has been used in Item Check by Battery Type", 1)
                Return
            End If
            ClsSPCItemCheckMasterDB.Delete(ItemCheckCode)
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
            If e.Column.FieldName = "ItemCheckCode" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
            End If
        End If
        If Grid.IsNewRowEditing Then
            If e.Column.FieldName = "ActiveStatus" Then
                TryCast(e.Editor, ASPxCheckBox).Checked = True
            End If
        End If
    End Sub

    Protected Sub Grid_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles Grid.RowValidating
        Dim GridItemCheck As GridViewDataColumn

        GridItemCheck = Grid.DataColumns("ItemCheckCode")
        If IsNothing(e.NewValues("ItemCheckCode")) OrElse e.NewValues("ItemCheckCode").ToString.Trim = "" Then
            e.Errors(GridItemCheck) = "Code Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Code Must Be Filled !", 1)
            Return
        End If

        GridItemCheck = Grid.DataColumns("ItemCheck")
        If IsNothing(e.NewValues("ItemCheck")) OrElse e.NewValues("ItemCheck").ToString.Trim = "" Then
            e.Errors(GridItemCheck) = "Item Check Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Item Check Must Be Filled !", 1)
            Return
        End If

        GridItemCheck = Grid.DataColumns("UnitMeasurement")
        If IsNothing(e.NewValues("UnitMeasurement")) OrElse e.NewValues("UnitMeasurement").ToString.Trim = "" Then
            e.Errors(GridItemCheck) = "Measuring Unit Must Be Filled !"
            show_error(MsgTypeEnum.ErrorMsg, "Measuring Unit Must Be Filled !", 1)
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

    Private Sub up_GridLoad()
        Dim Users As List(Of ClsSPCItemCheckMaster)
        Try
            Users = ClsSPCItemCheckMasterDB.GetList()
            Grid.DataSource = Users
            Grid.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

#End Region

End Class