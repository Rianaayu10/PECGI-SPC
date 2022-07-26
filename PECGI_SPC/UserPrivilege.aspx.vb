﻿Public Class UserPrivilege
    Inherits System.Web.UI.Page
    Dim UserID As String

    Private Sub UserPrivilege_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Request.QueryString("prm") Is Nothing Then
            Exit Sub
        End If        
        UserID = Request.QueryString("prm").ToString()
        txtUser.Text = UserID
        up_GridLoad(UserID)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub up_GridLoad(ByVal pUserID As String)
        Dim pErr As String = ""
        Dim dsMenu As List(Of Cls_ss_UserMenu)
        dsMenu = clsUserSetupDB.GetListMenu(pUserID, pErr)
        If pErr = "" Then
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

    Private Sub gridMenu_BatchUpdate(sender As Object, e As DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs) Handles gridMenu.BatchUpdate
        Dim ls_AllowAccess As String = "", ls_AllowUpdate As String = "", ls_AllowSpecial As String = "", ls_Active As String = ""
        Dim MenuID As String = ""

        Dim a As Integer = e.UpdateValues.Count
        For iLoop = 0 To a - 1
            ls_AllowAccess = (e.UpdateValues(iLoop).NewValues("AllowAccess").ToString())
            ls_AllowUpdate = (e.UpdateValues(iLoop).NewValues("AllowUpdate").ToString())

            If ls_AllowAccess = True Then ls_AllowAccess = "1" Else ls_AllowAccess = "0"
            If ls_AllowUpdate = True Then ls_AllowUpdate = "1" Else ls_AllowUpdate = "0"

            MenuID = Trim(e.UpdateValues(iLoop).NewValues("MenuID").ToString())
            Dim UserPrevilege As New Cls_ss_UserPrivilege With {
                .AppID = "QCS",
                .UserID = UserID,
                .MenuID = MenuID,
                .AllowAccess = ls_AllowAccess,
                .AllowUpdate = ls_AllowUpdate
            }
            Dim pErr As String = ""
            Dim iUpd As Integer = Cls_ss_UserPrivilegeDB.Save(UserPrevilege, pErr)
            If pErr <> "" Then
                Exit For
            End If
        Next iLoop
        gridMenu.EndUpdate()
    End Sub

    Private Sub gridMenu_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs) Handles gridMenu.CustomCallback
        Dim pAction As String = Split(e.Parameters, "|")(0)
        Dim pUserID As String = Split(e.Parameters, "|")(1)
        up_GridLoad(pUserID)
        If pAction = "save" Then
            show_error(MsgTypeEnum.Success, "Update data successful", 1)
        End If
    End Sub

    Private Sub cbkValid_Callback(source As Object, e As DevExpress.Web.ASPxCallback.CallbackEventArgs) Handles cbkValid.Callback
        Dim pAction As String = Split(e.Parameter, "|")(0)
        Dim FromUserID As String = Split(e.Parameter, "|")(1)
        Dim TouserID As String = Split(e.Parameter, "|")(2)
        Dim CreateUser As String = Session("user") & ""
        If FromUserID <> "" Then
            Cls_ss_UserPrivilegeDB.Copy(FromUserID, TouserID, CreateUser)
        End If        
    End Sub
End Class