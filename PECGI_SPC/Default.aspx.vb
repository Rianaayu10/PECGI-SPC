Public Class _Default
    Inherits System.Web.UI.Page
    Dim clsDESEncryption As New clsDESEncryption("TOS")

#Region "Declaration"
    Dim Useras As Cls_ss_UserSetup
    Dim userLock As Boolean
#End Region

#Region "Control Events"
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
    Private Sub _Default_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("UserID") IsNot Nothing And Request.QueryString("FactoryCode") IsNot Nothing Then
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            Dim UserID = clsDESEncryption.DecryptData(Request.QueryString("UserID"))
            Dim FactoryCode = clsDESEncryption.DecryptData(Request.QueryString("FactoryCode"))
            Dim User As clsUserSetup = clsUserSetupDB.GetData(UserID)

            If User Is Nothing Then
                lblInfo.Visible = True
                lblInfo.Text = "Invalid User Name!"
                txtusername.Focus()
            ElseIf User.LockStatus = "1" Then
                lblInfo.Visible = True
                If User.FailedLogin >= 12 Then
                    lblInfo.Text = "User is locked after 12 failed logins." & vbCrLf & "Please contact your administrator!"
                Else
                    lblInfo.Text = "User is locked." & vbCrLf & "Please contact your administrator!"
                End If
                txtusername.Focus()
            ElseIf User.FactoryCode <> FactoryCode Then
                lblInfo.Visible = True
                lblInfo.Text = "User don't have access in this factory!"
                txtusername.Focus()
            Else
                clsUserSetupDB.ResetFailedLogin(User.UserID)
                Session("AdminStatus") = User.AdminStatus
                Session("user") = UserID
                Response.Redirect("Main.aspx")
            End If
        End If
        'Session("user") = False
        btnVersion.Visible = False
        btnVersion.Text = "Version: " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString()
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