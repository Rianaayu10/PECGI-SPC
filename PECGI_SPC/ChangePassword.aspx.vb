﻿Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Imports DevExpress
Imports DevExpress.Web.ASPxCallback

Public Class ChangePassword
    Inherits System.Web.UI.Page

#Region "DECLARATION"
    Public lb_AuthUpdate As Boolean = False
    Dim clsDESEncryption As New clsDESEncryption("TOS")
#End Region

#Region "FORM EVENTS"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.SiteTitle = "Change Password"
        If (Not Page.IsPostBack) AndAlso (Not Page.IsCallback) Then
            txtCurrentPassword.Focus()
        End If
        lb_AuthUpdate = sGlobal.Auth_UserUpdate(Session("user"), "Z020")
        If lb_AuthUpdate = False Then
            btnClear.Enabled = False
            btnSubmit.Enabled = False
        End If
    End Sub

    Private Sub cbProgress_Callback(ByVal source As Object, ByVal e As DevExpress.Web.ASPxCallback.CallbackEventArgs) Handles cbProgress.Callback
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

#Region "PROCEDURE"
    Private Function ValidasiInput() As Boolean
        Dim UserID As String = Session("user").ToString.Trim

        Dim cUser As clsUserSetup = clsUserSetupDB.GetData(UserID)
        If txtCurrentPassword.Text <> cUser.Password Then
            cbProgress.JSProperties("cpMessage") = "Current Password is not correct!"
            Return False
        Else
            Return True
        End If
    End Function
#End Region
End Class