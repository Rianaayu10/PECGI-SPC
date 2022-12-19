Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Imports DevExpress
Imports DevExpress.Web

Public Class ChangePassword
    Inherits System.Web.UI.Page

#Region "DECLARATION"
    Public lb_AuthUpdate As Boolean = False
    Dim UserID As String = ""
    Dim MenuID As String = ""

    Dim clsDESEncryption As New clsDESEncryption("TOS")

    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False
#End Region

#Region "PROCEDURE"
    Private Function ValidasiInput() As Boolean
        Dim UserID As String = Session("user").ToString.Trim
        Dim MenuID As String = ""

        Dim cUser As clsUserSetup = clsUserSetupDB.GetData(UserID)
        If txtCurrentPassword.Text <> cUser.Password Then
            cbProgress.JSProperties("cpMessage") = "Current Password is not correct!"
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub up_GridLoad(user As String)
        Dim data As New clsUserSetup
        If user <> "" Then
            data = clsUserSetupDB.GetData(user)
            txtUserID.Text = user
            txtFullName.Text = data.FullName
            txtCurrentPassword.Text = data.Password
        End If
        cbCurrentPassword.Checked = True
        cbNewPassword.Checked = True
    End Sub

#End Region

#Region "FORM EVENTS"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        MenuID = "Z040"
        sGlobal.getMenu(MenuID)
        Master.SiteTitle = MenuID & " - " & sGlobal.menuName

        If Session("user") IsNot Nothing Then
            UserID = Session("user")
        End If

        AuthAccess = sGlobal.Auth_UserAccess(UserID, MenuID)
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(UserID, MenuID)
        If AuthUpdate = False Then
            btnSubmit.Enabled = False
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not Page.IsPostBack) AndAlso (Not Page.IsCallback) Then
            txtNewPassword.Focus()
            up_GridLoad(UserID)
        End If
        lb_AuthUpdate = sGlobal.Auth_UserUpdate(Session("user"), "Z040")
        If lb_AuthUpdate = False Then
            btnClear.Enabled = False
            btnSubmit.Enabled = False
        End If

    End Sub

    Private Sub cbProgress_Callback(ByVal source As Object, ByVal e As DevExpress.Web.CallbackEventArgs) Handles cbProgress.Callback
        Session("BA020Cancel") = ""
        Try
            Dim Err As String = ""
            Dim lb_IsUpdate As Boolean = ValidasiInput()
            If lb_IsUpdate = True Then
                Dim UserID As String = Session("user")
                Dim NewPassword As String = txtNewPassword.Text
                clsUserSetupDB.UpdatePassword(UserID, NewPassword)
                cbProgress.JSProperties("cpMessage") = "Update data successful."
            End If
        Catch ex As Exception
            cbProgress.JSProperties("cpError") = ex.Message.ToString
        End Try
    End Sub
#End Region
End Class