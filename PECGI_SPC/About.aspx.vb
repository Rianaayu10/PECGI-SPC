Public Class About
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'PENGECEKAN VALIDASI TOKEN
        If Session("Action") IsNot Nothing Then
            If Session("Action").ToString = "SSO" Then
                Dim token = Session("token")
                Dim SSOHost As String = ConfigurationManager.AppSettings("SSOUrl").ToString()
                If sGlobal.VerifyToken(token, SSOHost) = False Then
                    Response.Redirect(SSOHost + "/account/login?logout=1")
                End If
            End If
        End If

        'PENGECEKAN SESSION USER
        If Session("user") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

End Class