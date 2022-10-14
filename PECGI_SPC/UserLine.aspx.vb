Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data

Public Class UserLine
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim RegisterUser As String = ""
    Dim UserID As String
    Dim MenuID As String = ""

    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False

    Dim dt As New DataTable
#End Region

#Region "Procedure"
    Private Sub up_GridLoad(ByVal pUserID As String)
        Dim dsMenu As List(Of clsUserLine)
        dsMenu = clsUserLineDB.GetList(pUserID)
        gridMenu.SettingsPager.PageSize = dsMenu.Count
        gridMenu.DataSource = dsMenu
        gridMenu.DataBind()
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
    End Sub
#End Region

#Region "Initialization"
    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        MenuID = "Z040"
        sGlobal.getMenu(MenuID)
        Master.SiteTitle = MenuID & " - " & sGlobal.menuName
        show_error(MsgTypeEnum.Info, "", 0)
        RegisterUser = Session("user")

        AuthAccess = sGlobal.Auth_UserAccess(RegisterUser, MenuID)
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(RegisterUser, MenuID)
        If AuthUpdate = False Then
            btnSave.Enabled = False
        End If

        If Request.QueryString("prm") Is Nothing Then
            UserID = RegisterUser
            Up_FillCombo(RegisterUser)
            btnCancel.Visible = False
            Exit Sub
        Else
            btnCancel.Visible = True
            UserID = Request.QueryString("prm").ToString()
            Up_FillCombo(UserID)
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            up_GridLoad(UserID)
        End If
    End Sub
#End Region

#Region "Control Event"
    Private Sub gridMenu_BatchUpdate(sender As Object, e As DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs) Handles gridMenu.BatchUpdate

        Dim ls_AllowShow As String = ""
        Dim ls_AllowUpdate As String = ""
        Dim ls_AllowVerify As String = ""
        Dim LineID As String = ""

        Dim a As Integer = e.UpdateValues.Count
        For iLoop = 0 To a - 1
            ls_AllowShow = If(e.UpdateValues(iLoop).NewValues("AllowShow").ToString() = True, "1", "0")
            ls_AllowUpdate = If(e.UpdateValues(iLoop).NewValues("AllowUpdate").ToString() = True, "1", "0")
            ls_AllowVerify = If(e.UpdateValues(iLoop).NewValues("AllowVerify").ToString() = True, "1", "0")

            LineID = Trim(e.UpdateValues(iLoop).NewValues("LineID").ToString())
            Dim UserLine As New clsUserLine With {
                .UserID = cboUserID.Value,
                .LineID = LineID,
                .AllowShow = ls_AllowShow,
                .AllowUpdate = ls_AllowUpdate,
                .AllowVerify = ls_AllowVerify,
                .RegisterUser = RegisterUser
            }
            Dim pErr As String = ""
            Dim iUpd As Integer = clsUserLineDB.InsertUpdate(UserLine)
            If pErr <> "" Then
                Exit For
            End If
        Next iLoop
        gridMenu.EndUpdate()
    End Sub



    Private Sub gridMenu_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gridMenu.CustomCallback
        Dim pAction As String = Split(e.Parameters, "|")(0)
        Dim pUserID As String = Split(e.Parameters, "|")(1)
        If pAction = "save" Then
            show_error(MsgTypeEnum.Success, "Update data successful", 1)
        End If
        up_GridLoad(pUserID)
    End Sub

    Private Sub cbkValid_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles cbkValid.Callback
        Dim pAction = Split(e.Parameter, "|")(0)
        Dim FromUserID = Split(e.Parameter, "|")(1)
        Dim TouserID = Split(e.Parameter, "|")(2)
        If FromUserID <> "null" Then
            clsUserLineDB.Copy(FromUserID, TouserID, RegisterUser)
        End If
    End Sub

    Private Sub gridMenu_RowUpdating(sender As Object, e As ASPxDataUpdatingEventArgs) Handles gridMenu.RowUpdating
        e.Cancel = True
    End Sub
#End Region
End Class