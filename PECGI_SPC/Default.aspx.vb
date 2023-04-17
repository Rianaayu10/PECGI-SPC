Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class _Default
    Inherits System.Web.UI.Page
    Dim clsDESEncryption As New clsDESEncryption("TOS")

#Region "Declaration"
    Dim Useras As Cls_ss_UserSetup
    Dim userLock As Boolean
#End Region

#Region "Control Events"
    Private Sub _Default_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim SSOHost As String = ConfigurationManager.AppSettings("SSOUrl").ToString()

        If Request.QueryString("LogOut") IsNot Nothing Then
            If Session("Token") IsNot Nothing Then
                Session.Clear()
                Response.Redirect(SSOHost + "/account/login?logout=1")
            End If
        End If

        If Request.QueryString("Link") IsNot Nothing Then
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            Dim prm = Request.QueryString("Link")
            prm = prm.Replace(" ", "+")
            Dim mod4 = prm.Length Mod 4
            If mod4 > 0 Then
                prm += New String("=", 4 - mod4)
            End If

            Dim link = clsDESEncryption.DecryptData(prm)
            Dim ActionForm = link.Split("|")(0)
            Dim User = link.Split("|")(1)
            Dim Password = link.Split("|")(2)
            Dim ActionLink = link.Split("|")(3)

            If validation(User, Password) Then
                Session("user") = User
                Session("Action") = ActionForm
                Response.Redirect(ActionLink)
            End If
        End If

        If Request.QueryString("Token") IsNot Nothing Then
            Dim token = Request.QueryString("Token").ToString()
            Dim URL As String = SSOHost & "/api/sso/verifytoken?token=" & token
            Dim web = CType(WebRequest.Create(URL), HttpWebRequest)

            Dim validToken As Boolean = False

            Using response = web.GetResponse()
                Using responseStream = response.GetResponseStream()
                    If responseStream IsNot Nothing Then
                        Using streamReader = New StreamReader(responseStream)
                            Dim rawResponse As String = streamReader.ReadToEnd()
                            Dim startChar As Char() = {"("c}
                            Dim json As JObject = JObject.Parse(rawResponse)

                            If json("Status").ToString() = "200" Then
                                Session("Token") = token
                                Dim userLogin As clsUser = json("Data").ToObject(Of clsUser)()
                                Session("user") = userLogin.UserID
                                Session("LogUserName") = userLogin.FullName
                                Session("AdminStatus") = userLogin.StatusAdmin
                                Session("FullName") = userLogin.FullName
                                Session("FactoryCode") = "F1"
                                validToken = True
                            End If
                        End Using
                    End If
                End Using
            End Using

            If validToken = True Then
                Session("Action") = "SSO"
                Response.Redirect("Main.aspx")
            Else
                Response.Redirect(SSOHost + "/account/login?logout=1")
            End If
        End If

        btnVersion.Visible = False
        btnVersion.Text = "Version: " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString()
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Try
            If validation(txtusername.Text, txtpassword.Text) Then
                Session("user") = txtusername.Text

                Dim B02User As String = Session("B02User") & ""
                If B02User.Trim.ToUpper <> txtusername.Text.Trim.ToUpper Then
                    Session("B02ProcessGroup") = ""
                End If

                Response.Redirect("Main.aspx")
                Session("Action") = "SPC"
            End If
        Catch ex As Exception
            lblInfo.Text = ex.Message
        End Try
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
                If DataUser.FailedLogin >= 3 Then
                    lblInfo.Text = "User is locked after 3 failed login." & vbCrLf & "Please contact your administrator!"
                Else
                    lblInfo.Text = "User is locked." & vbCrLf & "Please contact your administrator!"
                End If
                txtusername.Focus()
                Return False
            End If

            If DataUser.Password <> Password Then
                lblInfo.Visible = True
                clsUserSetupDB.AddFailedLogin(DataUser.UserID)
                Dim passfailed As clsUserSetup = clsUserSetupDB.GetData(User)
                If passfailed.LockStatus = "1" Then
                    lblInfo.Text = "User is locked after 3 failed login." & vbCrLf & "Please contact your administrator!"
                Else
                    lblInfo.Text = "Invalid Password (" & passfailed.FailedLogin & "X)" & vbCrLf & "User will be locked after 3 failed login"
                End If
                txtusername.Focus()
                Return False
            End If

            clsUserSetupDB.ResetFailedLogin(DataUser.UserID)
            Session("AdminStatus") = DataUser.AdminStatus
            Session("FullName") = DataUser.FullName
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