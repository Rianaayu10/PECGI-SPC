Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Public Class UserPrivilege
    Inherits System.Web.UI.Page
    Dim pUser = ""

#Region "Declaration"
    Dim RegisterUser As String = ""
    Dim UserID As String = ""
    Dim MenuID As String = ""

    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False

    Dim dt As New DataTable
#End Region

#Region "Procedure"
    Private Sub up_GridLoad(ByVal pUserID As String)
        'PENGECEKAN VALIDASI TOKEN
        If Session("Action") = "SSO" Then
            Dim token = Session("token")
            Dim SSOHost As String = ConfigurationManager.AppSettings("SSOUrl").ToString()
            If sGlobal.VerifyToken(token, SSOHost) = False Then
                Response.Redirect(SSOHost + "/account/login?logout=1")
            End If
        End If

        'PENGECEKAN SESSION USER
        If Session("user") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If

        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, MenuID)
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        Dim pErr As String = ""
        Dim dsMenu As List(Of Cls_ss_UserMenu)
        dsMenu = clsUserSetupDB.GetListMenu(pUserID, pErr)
        If pErr = "" Then
            gridMenu.SettingsPager.PageSize = dsMenu.Count
            gridMenu.DataSource = dsMenu
            gridMenu.DataBind()
            If dsMenu Is Nothing Then
                show_error(MsgTypeEnum.Warning, "Data is not found!")
            End If
        Else
            show_error(MsgTypeEnum.ErrorMsg, pErr)
        End If
    End Sub

    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, Optional ByVal pVal As Integer = 1)
        gridMenu.JSProperties("cp_message") = ErrMsg
        gridMenu.JSProperties("cp_type") = msgType
        gridMenu.JSProperties("cp_val") = pVal
    End Sub

    Private Sub Up_FillCombo(UserID As String)
        Dim a As String = ""

        dt = clsUserSetupDB.GetUserID()
        With cboUserID
            .DataSource = dt
            .DataBind()
        End With

        If UserID <> "" Then
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("UserID").ToString.ToLower = UserID.ToLower Then
                    cboUserID.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            cboUserID.SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
        End If
        If cboUserID.SelectedIndex < 0 Then
            a = ""
        Else
            a = cboUserID.SelectedItem.GetFieldValue("UserID")
        End If
        HideValue.Set("UserID", a)
        HideValue.Set("Userload", a)
    End Sub

#End Region

#Region "Initialization"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        MenuID = "Z020"
        sGlobal.getMenu(MenuID)
        Master.SiteTitle = MenuID & " - " & sGlobal.menuName
        show_error(MsgTypeEnum.Info, "", 0)
        RegisterUser = Session("user")

        AuthAccess = sGlobal.Auth_UserAccess(RegisterUser, MenuID)
        If AuthAccess = False Then
            If Not Page.IsPostBack Then
                Response.Redirect("~/Main.aspx")
            End If
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(RegisterUser, MenuID)
        If AuthUpdate = False Then
            btnSave.Enabled = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("prm") Is Nothing Then
                UserID = RegisterUser
                btnCancel.Visible = False
            Else
                btnCancel.Visible = True
                UserID = Request.QueryString("prm").ToString()
                cboUserID.Enabled = False
            End If
            Up_FillCombo(UserID)
            up_GridLoad(UserID)
        End If
    End Sub
#End Region

#Region "Control Event"
    Private Sub gridMenu_BatchUpdate(sender As Object, e As DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs) Handles gridMenu.BatchUpdate
        Dim ls_AllowAccess As String = "", ls_AllowUpdate As String = "", ls_AllowSpecial As String = "", ls_Active As String = ""
        Dim MenuID As String = "", ls_AllowDelete As String = ""

        Dim a As Integer = e.UpdateValues.Count
        For iLoop = 0 To a - 1
            Try
                ls_AllowAccess = (e.UpdateValues(iLoop).NewValues("AllowAccess").ToString())
                ls_AllowUpdate = (e.UpdateValues(iLoop).NewValues("AllowUpdate").ToString())
                ls_AllowDelete = (e.UpdateValues(iLoop).NewValues("AllowDelete").ToString())

                If ls_AllowAccess = True Then ls_AllowAccess = "1" Else ls_AllowAccess = "0"
                If ls_AllowUpdate = True Then ls_AllowUpdate = "1" Else ls_AllowUpdate = "0"
                If ls_AllowDelete = True Then ls_AllowDelete = "1" Else ls_AllowDelete = "0"

                MenuID = Trim(e.UpdateValues(iLoop).NewValues("MenuID").ToString())
                Dim UserPrevilege As New Cls_ss_UserPrivilege With {
                    .AppID = "SPC",
                    .UserID = cboUserID.Value,
                    .MenuID = MenuID,
                    .AllowAccess = ls_AllowAccess,
                    .AllowUpdate = ls_AllowUpdate,
                    .AllowDelete = ls_AllowDelete,
                    .RegisterUser = RegisterUser
                }
                Dim pErr As String = ""
                pErr = Cls_ss_UserPrivilegeDB.Save(UserPrevilege, pErr)
                If pErr <> "" Then
                    Exit For
                End If
            Catch ex As Exception
                Throw ex
            End Try
        Next iLoop
        gridMenu.EndUpdate()
        up_GridLoad(cboUserID.Value)
    End Sub

    Private Sub gridMenu_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gridMenu.CustomCallback

        Dim pAction As String = Split(e.Parameters, "|")(0)
        Dim pUser = Split(e.Parameters, "|")(1)

        'up_GridLoad(prm_UserID)
        If pAction = "save" Then
            show_error(MsgTypeEnum.Success, "Update data successful", 1)
            HideValue.Set("Userload", pUser)
        End If

        gridMenu.JSProperties("cp_Show") = AuthAccess
    End Sub

    Private Sub cbkValid_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles cbkValid.Callback
        Dim pAction As String = Split(e.Parameter, "|")(0)
        Dim FromUserID As String = Split(e.Parameter, "|")(1)
        Dim TouserID As String = Split(e.Parameter, "|")(2)

        If FromUserID <> "null" Then
            Cls_ss_UserPrivilegeDB.Copy(FromUserID, TouserID, RegisterUser)
        End If
    End Sub

    Private Sub gridMenu_RowUpdating(sender As Object, e As ASPxDataUpdatingEventArgs) Handles gridMenu.RowUpdating
        e.Cancel = True
    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles gridMenu.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then
            Dim a = HideValue.Get("Userload")
            up_GridLoad(a)
        End If
    End Sub
#End Region
End Class