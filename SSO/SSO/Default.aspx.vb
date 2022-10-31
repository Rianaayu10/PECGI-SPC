Imports System.Reflection

Public Class _Default
    Inherits System.Web.UI.Page
    Dim clsDESEncryption As New clsDESEncryption("TOS")

#Region "Declaration"
    Dim userLock As Boolean
    Private divLinks As Object
#End Region

#Region "Control Events"
    Private Sub _Default_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnVersion.Visible = False
        btnVersion.Text = "Version: " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString()
    End Sub
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Try
            If validation() Then
                Session("user") = txtusername.Text
                Response.Redirect("Main.aspx")
            End If
        Catch ex As Exception
            lblInfo.Text = ex.Message
        End Try
    End Sub
#End Region

#Region "Function"
    Private Function validation() As Boolean
        Try
            If txtusername.Text.Trim = "" Then
                lblInfo.Visible = True
                lblInfo.Text = "Please input username!"
                txtusername.Focus()
                Return False
            End If

            If txtpassword.Text.Trim = "" Then
                lblInfo.Visible = True
                lblInfo.Text = "Please input password!"
                txtpassword.Focus()
                Return False
            End If

            Dim User As clsUserSetup = clsUserSetupDB.GetData(txtusername.Text)
            If User Is Nothing Then
                lblInfo.Visible = True
                lblInfo.Text = "Invalid User Name!"
                txtusername.Focus()
                Return False
            End If
            If User.LockStatus = "1" Then
                lblInfo.Visible = True
                If User.FailedLogin >= 12 Then
                    lblInfo.Text = "User is locked after 12 failed logins." & vbCrLf & "Please contact your administrator!"
                Else
                    lblInfo.Text = "User is locked." & vbCrLf & "Please contact your administrator!"
                End If
                txtusername.Focus()
                Return False
            End If
            If User.Password <> txtpassword.Text Then
                lblInfo.Visible = True
                lblInfo.Text = "Invalid Password!"
                clsUserSetupDB.AddFailedLogin(User.UserID)
                txtusername.Focus()
                Return False
            End If
            clsUserSetupDB.ResetFailedLogin(User.UserID)
            Session("AdminStatus") = User.AdminStatus
        Catch ex As Exception
            Response.Write("Validation Exception :<br>" & ex.ToString)
        End Try
        Return True
    End Function

    Sub Clear()
        Me.txtusername.Text = ""
        Me.txtpassword.Text = ""
        Me.txtusername.Focus()
    End Sub
#End Region

End Class