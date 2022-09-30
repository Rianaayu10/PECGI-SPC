﻿Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports OfficeOpenXml
Imports System.Web.Services
Imports OfficeOpenXml.Style

Public Class ProdSampleQCSummaryDetail
    Inherits System.Web.UI.Page

#Region "Declarations"
    Dim pUser As String = ""
    Dim pMenuID As String = "B050"
    Dim Factory, Type, ItemCheck, Prod, Line, Frequency, Sequence As String
    Dim cls As clsProdSampleQCSummary
    Private dt As DataTable
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu(pMenuID)
        Master.SiteTitle = sGlobal.menuName
        pUser = Session("user")
        '{menu=ProdIDSummary&FactoryCode=F001&ItemTypeCode=TPMSBR011&ItemCheckCode=IC022&ProdDate=2022-08-04&Frequency=03&Sequence=5&Line=ALL}
        Factory = Request.QueryString("FactoryCode") & ""
        Type = Request.QueryString("ItemTypeCode") & ""
        ItemCheck = Request.QueryString("ItemCheckCode") & ""
        Prod = Request.QueryString("ProdDate") & ""
        Line = Request.QueryString("Line") & ""
        Frequency = Request.QueryString("Frequency") & ""
        Sequence = Request.QueryString("Sequence") & ""

        If Not IsPostBack And Not IsCallback Then
            If Factory <> "" And Type <> "" And ItemCheck <> "" And Prod <> "" And Line <> "" And Frequency <> "" And Sequence <> "" Then
                up_GridLoad()
            Else
                Response.Redirect("~/Main.aspx")
            End If
        End If
    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
            up_GridLoad()
        End If
    End Sub

    Private Sub Grid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles Grid.HtmlDataCellPrepared
        If (e.DataColumn.FieldName <> "Link") Then
            e.Cell.Text = Split(e.CellValue, "||")(0)
            e.Cell.BackColor = ColorTranslator.FromHtml(Split(e.CellValue, "||")(1))
        End If
    End Sub
#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Grid.JSProperties("cp_message") = ErrMsg
        Grid.JSProperties("cp_type") = msgType
        Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_GridLoad()
        With Grid
            cls = New clsProdSampleQCSummary

            cls.FactoryCode = Factory
            cls.ItemTypeCode = Type
            cls.ItemCheckCode = ItemCheck
            cls.Period = Prod
            cls.MachineCode = Line
            cls.Frequency = Frequency
            cls.Sequence = Sequence
            cls.UserID = pUser

            dt = clsProdSampleQCSummaryDB.GetListDetail(cls)
            .DataSource = dt
            .DataBind()
        End With
    End Sub
#End Region

End Class