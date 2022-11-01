Public Class Main
    Inherits System.Web.UI.Page
    Dim UserID As String = ""
    Dim Password As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("user")) Then
            Response.Redirect("~/Default.aspx")
        End If
        UserID = Session("user").ToString
        Password = Session("password").ToString
        Dim dt As New DataTable
        dt = clsAccount.GetURL(UserID, Password)
        Me.Account.DataSource = dt
        Me.Account.DataBind()
    End Sub

End Class