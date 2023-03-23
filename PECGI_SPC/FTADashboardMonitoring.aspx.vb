Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services

Public Class FTADashboardMonitoring
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("user") Is Nothing Then
                Response.Redirect("~/Default.aspx")
            End If
            HideValue.Set("UserID", Session("user"))
        End If
    End Sub

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function LoadData(User As String) As clsContent
        Dim content As New clsContent
        Try
            Dim dt As New DataTable
            Dim det As New clsFTADashboardMonitoring
            dt = clsFTADashboardMonitoring.LoadData(User)
            If dt.Rows.Count > 0 Then
                det.Station1 = dt.Rows(0)("Station1").ToString
                det.Station2 = dt.Rows(0)("Station2").ToString
                det.Station3 = dt.Rows(0)("Station3").ToString
                det.Station4 = dt.Rows(0)("Station4").ToString
                det.Station5 = dt.Rows(0)("Station5").ToString
                det.Station6 = dt.Rows(0)("Station6").ToString
                det.Station7 = dt.Rows(0)("Station7").ToString
                det.Station8 = dt.Rows(0)("Station8").ToString
                det.Station9 = dt.Rows(0)("Station9").ToString
                det.Station10 = dt.Rows(0)("Station10").ToString
                det.Station11 = dt.Rows(0)("Station11").ToString
                det.Station12 = dt.Rows(0)("Station12").ToString
                det.Station13 = dt.Rows(0)("Station13").ToString
                det.Station14 = dt.Rows(0)("Station14").ToString
                det.Station15 = dt.Rows(0)("Station15").ToString
                det.Station16 = dt.Rows(0)("Station16").ToString
                det.DelayInput = dt.Rows(0)("DelayInputResult").ToString
                det.DelayNG = dt.Rows(0)("DelayNGResult").ToString
            End If

            content.Message = "Success"
            content.Contents = det

        Catch ex As Exception
            content.Message = "Error"
            content.Contents = ex.Message
        End Try
        Return content
    End Function


End Class