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
            If validation(txtusername.Text, txtpassword.Text) Then
                Session("user") = txtusername.Text
                Response.Redirect("Main.aspx")
            End If
        Catch ex As Exception
            lblInfo.Text = ex.Message
        End Try
    End Sub
    Private Sub _Default_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("Link") IsNot Nothing Then
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            Dim prm = Request.QueryString("Link")
            prm = prm.Replace(" ", "+")
            Dim mod4 = prm.Length Mod 4
            If mod4 > 0 Then
                prm += New String("=", 4 - mod4)
            End If

            Dim link = clsDESEncryption.DecryptData(prm)
            Dim ActionForm = Link.Split("|")(0)
            Dim User = Link.Split("|")(1)
            Dim Password = Link.Split("|")(2)
            Dim ActionLink = Link.Split("|")(3)

            If validation(User, Password) Then
                Session("user") = User
                If ActionForm = "SPCNotification" Then
                    Response.Redirect(ActionLink)
                ElseIf ActionForm = "SPCSSO" Then
                    Response.Redirect("Main.aspx")
                End If
            End If
        End If
        btnVersion.Visible = False
        btnVersion.Text = "Version: " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString()
    End Sub
#End Region

#Region "Function"
    Private Function validation(User As String, Password As String) As Boolean
        Try
            If User = "" Then
                lblInfo.Visible = True
                lblInfo.Text = "Please input username!"
                txtusername.Focus()
                Return False
            End If

            If Password = "" Then
                lblInfo.Visible = True
                lblInfo.Text = "Please input password!"
                txtpassword.Focus()
                Return False
            End If

            Dim DataUser As clsUserSetup = clsUserSetupDB.GetData(User)

            If DataUser Is Nothing Then
                lblInfo.Visible = True
                lblInfo.Text = "User don't have access in this factory (Invalid User Name)!"
                txtusername.Focus()
                Return False
            End If

            If DataUser.LockStatus = "1" Then
                lblInfo.Visible = True
                If DataUser.FailedLogin >= 12 Then
                    lblInfo.Text = "User is locked after 12 failed logins." & vbCrLf & "Please contact your administrator!"
                Else
                    lblInfo.Text = "User is locked." & vbCrLf & "Please contact your administrator!"
                End If
                txtusername.Focus()
                Return False
            End If

            If DataUser.Password <> Password Then
                lblInfo.Visible = True
                lblInfo.Text = "Invalid Password!"
                clsUserSetupDB.AddFailedLogin(DataUser.UserID)
                txtusername.Focus()
                Return False
            End If

            clsUserSetupDB.ResetFailedLogin(DataUser.UserID)
            Session("AdminStatus") = DataUser.AdminStatus
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