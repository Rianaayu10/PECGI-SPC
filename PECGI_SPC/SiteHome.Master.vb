Option Strict On
Imports System.Data.SqlClient
Public Class SiteHome
    Inherits System.Web.UI.MasterPage

#Region "DECLARATION"
    Dim sqlstring As String
    Dim dtView As DataTable
    Dim constr As New SqlClient.SqlConnection(Sconn.Stringkoneksi)
    Public SiteTitle As String
    Public MenuIndex As Integer
    Public IdMenu As String
    Dim script As String
    Dim Title As String
    Public notifQCSA As String
    Public notifTCCSA As String
    Public notifTCCSRA As String
    Public notifQCSResult As String
    Dim pUser As String = ""
    Public StatusApproval1 As String
    Public StatusApproval2 As String
    Public StatusApproval3 As String
    Public StatusApproval4 As String
#End Region

#Region "CONTROL EVENTS"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        MenuIndex = sGlobal.indexMenu
        IdMenu = sGlobal.idMenu.Trim()
        If SiteTitle = "" Then
            Title = ""
        Else
            Title = SiteTitle & " - "
        End If
        PageTitle.InnerText = Title & "PANASONIC SPC SYSTEM"

        ''notif approve
        If IsNothing(Session("user")) Then
            pUser = ""
        Else
            pUser = Session("user").ToString
        End If


        If IsNothing(Session("user")) Then
            If Page.IsCallback Then
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Default.aspx")
            Else
                Response.Redirect("~/Default.aspx")
            End If
            Exit Sub
        End If

        With Me
            Dim dt As New DataTable()
            .BindMenu()
            lblUser.Text = Session("user").ToString
            lblFullName.Text = Session("FullName").ToString
            lblAdmin.Text = Session("AdminStatus").ToString
        End With

        Session.Timeout = 600 'in minutes
    End Sub

#End Region

#Region "FUNCTION"

    Private Function checkPrivilege() As Boolean
        Dim page As String = Server.HtmlEncode(Request.Path)

        Dim resultSplit() As String = page.Split(CChar("/"))
        Dim sqlstring As String = " SELECT * FROM dbo.spc_UserPrivilege UP " &
                                  " JOIN dbo.spc_UserMenu UM ON UP.MenuID = UM.MenuID " &
                                  " WHERE UP.AllowAccess = 1 AND UserID='" & Session("User").ToString & "' "
        Dim dt As DataTable = sGlobal.GetData(sqlstring)
        If dt.Rows.Count > 0 Then
            Return True
        End If

        Return False
    End Function

    Private Sub BindMenu()
        Dim dt As New DataTable
        Dim AdminStatus = Session("AdminStatus").ToString

        dt = clsSiteMasterDB.GetMenu(AdminStatus, pUser)

        Me.rptMenu.DataSource = dt
        Me.rptMenu.DataBind()
    End Sub

    Public Sub rptMenu_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptMenu.ItemDataBound
        Dim dt As New DataTable
        Dim AdminStatus = Session("AdminStatus").ToString

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim rptSubMenu As Repeater = TryCast(e.Item.FindControl("rptChildMenu"), Repeater)
            Dim GroupID As String = DirectCast(e.Item.DataItem, System.Data.DataRowView).Row(0).ToString

            dt = clsSiteMasterDB.GetSubMenu(AdminStatus, pUser, IdMenu, MenuIndex.ToString, GroupID)

            rptSubMenu.DataSource = dt
            rptSubMenu.DataBind()
        End If
    End Sub

#End Region

End Class