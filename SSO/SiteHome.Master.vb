Option Strict On
Imports System.Data.SqlClient
Public Class SiteHome
    Inherits System.Web.UI.MasterPage

#Region "DECLARATION"
    Dim sqlstring As String
    Dim dtView As DataTable
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
        If SiteTitle = "" Then
            Title = ""
        Else
            Title = SiteTitle & " - "
        End If
        PageTitle.InnerText = Title & "SSO SPC SYSTEM"

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
            lblUser.Text = Session("user").ToString
        End With

        Session.Timeout = 600 'in minutes
    End Sub

#End Region

End Class