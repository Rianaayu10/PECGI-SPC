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
                Session("password") = txtpassword.Text
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

            Dim UserF1 As clsUserSetup = clsUserSetupDB.GetUserF1(txtusername.Text) 'Check User in Factory 1
            If UserF1 IsNot Nothing Then 'if user is found in Factory 1 
                If UserF1.Password <> txtpassword.Text Then
                    lblInfo.Visible = True
                    lblInfo.Text = "Invalid Password!"
                    txtusername.Focus()
                    Return False
                End If

            Else 'if user is not found in Factory 1 then looking for in factory 2
                Dim UserF2 As clsUserSetup = clsUserSetupDB.GetUserF2(txtusername.Text) 'Check User in Factory 2
                If UserF2 IsNot Nothing Then 'if user is found in Factory 2
                    If UserF2.Password <> txtpassword.Text Then
                        lblInfo.Visible = True
                        lblInfo.Text = "Invalid Password!"
                        txtusername.Focus()
                        Return False
                    End If

                Else 'if user is not found in Factory 2 then looking for in factory 3
                    Dim UserF3 As clsUserSetup = clsUserSetupDB.GetUserF3(txtusername.Text) 'Check User in Factory 3
                    If UserF3 IsNot Nothing Then 'if user is found in Factory 3
                        If UserF3.Password <> txtpassword.Text Then
                            lblInfo.Visible = True
                            lblInfo.Text = "Invalid Password!"
                            txtusername.Focus()
                            Return False
                        End If

                    Else 'if user is not found in Factory 3 then looking for in factory 4
                        Dim UserF4 As clsUserSetup = clsUserSetupDB.GetUserF4(txtusername.Text)  'Check User in Factory 4
                        If UserF4 IsNot Nothing Then 'if user isfound in Factory 4
                            If UserF4.Password <> txtpassword.Text Then
                                lblInfo.Visible = True
                                lblInfo.Text = "Invalid Password!"
                                txtusername.Focus()
                                Return False
                            End If

                        Else 'if user ist not found in Factory 4
                            lblInfo.Visible = True
                            lblInfo.Text = "Invalid User Name!"
                            txtusername.Focus()
                            Return False
                        End If
                    End If
                End If
            End If
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