Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.Data
Imports DevExpress.Web
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Data

Public Class ProductionSampleVerificationList
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim pUser As String = ""
    Private dt As DataTable
    Dim MenuID As String = ""

    ' AUTHORIZATION
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False

    ' FILL COMBO BOX FILTER
    Dim Factory_Sel As String = "1"
    Dim ItemType_Sel As String = "2"
    Dim Line_Sel As String = "3"
    Dim ItemCheck_Sel As String = "4"
    Dim MK_Sel As String = "5"
    Dim QC_Sel As String = "6"

    ' PARAMETER GRID COLOR
    Dim nMinColor As String = ""
    Dim nMaxColor As String = ""
    Dim nAvgColor As String = ""
    Dim ResultColor As String = ""
    Dim CorStsColor As String = ""
    Dim MKColor As String = ""
    Dim QCColor As String = ""

    ' PARAMETER SESSION
    Dim sFactoryCode = ""
    Dim sItemType = ""
    Dim sLineCode = ""
    Dim sItemCheck = ""
    Dim sMKVerification = ""
    Dim sQCVerification = ""
    Dim sProdDateTo = ""
    Dim sProdDateFrom = ""

#End Region

#Region "Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pUser = Session("user")
        MenuID = "B030"

        sGlobal.getMenu(MenuID)
        Master.SiteTitle = MenuID & " - " & sGlobal.menuName
        show_error(MsgTypeEnum.Info, "", 0)

        AuthAccess = sGlobal.Auth_UserAccess(pUser, MenuID)
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        If Not Page.IsPostBack Then
            If Request.QueryString("menu") IsNot Nothing Then
                LoadForm_ByAnotherform()
            Else
                LoadForm()
            End If
        End If
    End Sub
    Private Sub cboLineID_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineID.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ItemType_Code = e.Parameter.Split("|")(1)
            data.UserID = pUser

            Dim ErrMsg As String = ""
            dt = clsProductionSampleVerificationListDB.FillCombo(Line_Sel, data)
            With cboLineID
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboItemCheck_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemCheck.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ItemType_Code = Split(e.Parameter, "|")(1)
            data.LineCode = Split(e.Parameter, "|")(2)
            data.UserID = pUser

            Dim ErrMsg As String = ""
            dt = clsProductionSampleVerificationListDB.FillCombo(ItemCheck_Sel, data)
            With cboItemCheck
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Protected Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Dim cls As New clsProductionSampleVerificationList
        Dim Factory As String = cboFactory.Value
        Dim FactoryName As String = cboFactory.Text
        Dim Itemtype As String = cboItemType.Value
        Dim Itemtype_Name As String = cboItemType.Text
        Dim Line As String = cboLineID.Value
        Dim LineName As String = cboLineID.Text
        Dim ItemCheck As String = cboItemCheck.Value
        Dim ItemCheck_Name As String = cboItemCheck.Text
        Dim ProdDateFrom As String = Convert.ToDateTime(dtFromDate.Value).ToString("yyyy-MM-dd")
        Dim ProdDateTo As String = Convert.ToDateTime(dtToDate.Value).ToString("yyyy-MM-dd")
        Dim Period As String = Convert.ToDateTime(dtFromDate.Value).ToString("dd MMM yyyy") & " - " & Convert.ToDateTime(dtToDate.Value).ToString("dd MMM yyyy")
        Dim MKVerification As String = cboMK.Value
        Dim QCVerification As String = cboQC.Value
        Dim MKVerification_Name As String = cboMK.Text
        Dim QCVerification_Name As String = cboQC.Text

        cls.FactoryCode = Factory
        cls.FactoryName = FactoryName
        cls.ItemType_Code = Itemtype
        cls.ItemType_Name = Itemtype_Name
        cls.LineCode = Line
        cls.LineName = LineName
        cls.ItemCheck_Code = ItemCheck
        cls.ItemCheck_Name = ItemCheck_Name
        cls.ProdDateFrom = ProdDateFrom
        cls.ProdDateTo = ProdDateTo
        cls.Period = Period
        cls.MKVerification = MKVerification
        cls.MKVerification_Name = MKVerification_Name
        cls.QCVerification = QCVerification
        cls.QCVerification_Name = QCVerification_Name

        up_Excel(cls)
    End Sub
    Private Sub Grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles Grid.CustomCallback
        Try
            Dim cls As New clsProductionSampleVerificationList
            Dim msgErr As String = ""
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "Load" Then

                cls.FactoryCode = Split(e.Parameters, "|")(1)
                cls.ItemType_Code = Split(e.Parameters, "|")(2)
                cls.LineCode = Split(e.Parameters, "|")(3)
                cls.ItemCheck_Code = Split(e.Parameters, "|")(4)
                cls.ProdDateFrom = Convert.ToDateTime(Split(e.Parameters, "|")(5)).ToString("yyyy-MM-dd")
                cls.ProdDateTo = Convert.ToDateTime(Split(e.Parameters, "|")(6)).ToString("yyyy-MM-dd")
                cls.MKVerification = Split(e.Parameters, "|")(7)
                cls.QCVerification = Split(e.Parameters, "|")(8)

                UpGridLoad(cls)

            ElseIf pAction = "Clear" Then
                dt = clsProductionSampleVerificationListDB.LoadGrid(pUser, cls)
                Grid.DataSource = dt
                Grid.DataBind()
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub Grid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles Grid.HtmlDataCellPrepared
        Try

            If e.DataColumn.FieldName = "nMinColor" Then
                nMinColor = e.CellValue
            ElseIf e.DataColumn.FieldName = "nMaxColor" Then
                nMaxColor = e.CellValue
            ElseIf e.DataColumn.FieldName = "nAvgColor" Then
                nAvgColor = e.CellValue
            ElseIf e.DataColumn.FieldName = "ResultColor" Then
                ResultColor = e.CellValue
            ElseIf e.DataColumn.FieldName = "CorStsColor" Then
                CorStsColor = e.CellValue
            ElseIf e.DataColumn.FieldName = "MKColor" Then
                MKColor = e.CellValue
            ElseIf e.DataColumn.FieldName = "QCColor" Then
                QCColor = e.CellValue
            ElseIf e.DataColumn.FieldName = "nMin" Then
                e.Cell.BackColor = ColorTranslator.FromHtml(nMinColor)
            ElseIf e.DataColumn.FieldName = "nMax" Then
                e.Cell.BackColor = ColorTranslator.FromHtml(nMaxColor)
            ElseIf e.DataColumn.FieldName = "nAvg" Then
                e.Cell.BackColor = ColorTranslator.FromHtml(nAvgColor)
            ElseIf (e.DataColumn.FieldName = "Cor_Sts") Then
                e.Cell.BackColor = ColorTranslator.FromHtml(CorStsColor)
            ElseIf (e.DataColumn.FieldName = "Result") Then
                e.Cell.BackColor = ColorTranslator.FromHtml(ResultColor)
            ElseIf (e.DataColumn.FieldName = "MK_PIC") Then
                e.Cell.BackColor = ColorTranslator.FromHtml(MKColor)
            ElseIf (e.DataColumn.FieldName = "MK_Time") Then
                e.Cell.BackColor = ColorTranslator.FromHtml(MKColor)
            ElseIf (e.DataColumn.FieldName = "QC_PIC") Then
                e.Cell.BackColor = ColorTranslator.FromHtml(QCColor)
            ElseIf (e.DataColumn.FieldName = "QC_Time") Then
                e.Cell.BackColor = ColorTranslator.FromHtml(QCColor)
            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then

            Dim cls As New clsProductionSampleVerificationList
            cls.FactoryCode = cboFactory.Value
            cls.ItemType_Code = cboItemType.Value
            cls.LineCode = cboLineID.Value
            cls.ItemCheck_Code = cboItemCheck.Value
            cls.ProdDateFrom = Convert.ToDateTime(dtFromDate.Value).ToString("yyyy-MM-dd")
            cls.ProdDateTo = Convert.ToDateTime(dtToDate.Value).ToString("yyyy-MM-dd")
            cls.MKVerification = cboMK.Value
            cls.QCVerification = cboQC.Value
            cls.UserID = pUser

            UpGridLoad(cls)
        End If
    End Sub
#End Region
#Region "Procedure"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Grid.JSProperties("cp_message") = ErrMsg
        Grid.JSProperties("cp_type") = msgType
        Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_Fillcombo()
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.UserID = pUser
            Dim ErrMsg As String = ""
            Dim a As String

            '============ FILL COMBO FACTORY CODE ================'
            dt = clsProductionSampleVerificationListDB.FillCombo(Factory_Sel, data)
            With cboFactory
                .DataSource = dt
                .DataBind()
            End With
            If sFactoryCode <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sFactoryCode Then
                        cboFactory.SelectedIndex = i
                        Exit For
                    End If
                Next
            Else
                cboFactory.SelectedIndex = 0
            End If
            If cboFactory.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboFactory.SelectedItem.GetFieldValue("CODE")
            End If
            data.FactoryCode = a
            '======================================================'


            '============== FILL COMBO ITEM TYPE =================='
            dt = clsProductionSampleVerificationListDB.FillCombo(ItemType_Sel, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With
            If sItemType <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sItemType Then
                        cboItemType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
            If cboItemType.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboItemType.SelectedItem.GetFieldValue("CODE")
            End If
            data.ItemType_Code = a
            '======================================================'


            '============== FILL COMBO LINE CODE =================='         

            dt = clsProductionSampleVerificationListDB.FillCombo(Line_Sel, data)
            With cboLineID
                .DataSource = dt
                .DataBind()
            End With
            If sLineCode <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sLineCode Then
                        cboLineID.SelectedIndex = i
                        Exit For
                    End If
                Next

                If cboLineID.SelectedIndex < 0 Then
                    a = ""
                Else
                    a = cboLineID.SelectedItem.GetFieldValue("CODE")
                End If
            End If
            data.LineCode = a
            '======================================================'


            '============== FILL COMBO ITEM CHECK =================='         

            dt = clsProductionSampleVerificationListDB.FillCombo(ItemCheck_Sel, data)
            With cboItemCheck
                .DataSource = dt
                .DataBind()
            End With
            If sItemCheck <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sItemCheck Then
                        cboItemCheck.SelectedIndex = i
                        Exit For
                    End If
                Next

                If cboItemCheck.SelectedIndex < 0 Then
                    a = ""
                Else
                    a = cboItemCheck.SelectedItem.GetFieldValue("CODE")
                End If
            End If
            data.ItemCheck_Code = a
            '======================================================'

            '============== FILL MK VERIFICATION =================='
            dt = clsProductionSampleVerificationListDB.FillCombo(MK_Sel, data)
            With cboMK
                .DataSource = dt
                .DataBind()
            End With
            If sMKVerification <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sMKVerification Then
                        cboMK.SelectedIndex = i
                        Exit For
                    End If
                Next
            Else
                cboMK.SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End If
            If cboMK.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboMK.SelectedItem.GetFieldValue("CODE")
            End If
            data.MKVerification = a
            '======================================================'


            '============== FILL QC VERIFICATION =================='
            dt = clsProductionSampleVerificationListDB.FillCombo(QC_Sel, data)
            With cboQC
                .DataSource = dt
                .DataBind()
            End With
            If sQCVerification <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sQCVerification Then
                        cboQC.SelectedIndex = i
                        Exit For
                    End If
                Next
            Else
                cboQC.SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End If
            If cboQC.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboQC.SelectedItem.GetFieldValue("CODE")
            End If
            data.QCVerification = a
            '======================================================''

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Private Sub UpGridLoad(cls As clsProductionSampleVerificationList)
        Try
            dt = clsProductionSampleVerificationListDB.LoadGrid(pUser, cls)
            Grid.DataSource = dt
            Grid.DataBind()

            If dt.Rows.Count > 0 Then
                Grid.JSProperties("cp_GridTot") = dt.Rows.Count
            Else
                show_error(MsgTypeEnum.Warning, "Data Not Found !", 1)
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub LoadForm_ByAnotherform()
        sFactoryCode = Request.QueryString("FactoryCode")
        sItemType = Request.QueryString("ItemTypeCode")
        sLineCode = Request.QueryString("Line")
        sItemCheck = Request.QueryString("ItemCheckCode")
        sMKVerification = Request.QueryString("MK")
        sQCVerification = Request.QueryString("QC")
        sProdDateFrom = Request.QueryString("FromDate")
        sProdDateTo = Request.QueryString("ToDate")

        Dim cls As New clsProductionSampleVerificationList
        cls.FactoryCode = sFactoryCode
        cls.ItemType_Code = sItemType
        cls.LineCode = sLineCode
        cls.ItemCheck_Code = sItemCheck
        cls.ProdDateFrom = Convert.ToDateTime(sProdDateFrom).ToString("yyyy-MM-dd")
        cls.ProdDateTo = Convert.ToDateTime(sProdDateTo).ToString("yyyy-MM-dd")
        cls.MKVerification = sMKVerification
        cls.QCVerification = sQCVerification
        cls.UserID = pUser

        up_Fillcombo()
        UpGridLoad(cls)

        dtFromDate.Value = Convert.ToDateTime(sProdDateFrom)
        dtToDate.Value = Convert.ToDateTime(sProdDateTo)
    End Sub
    Private Sub LoadForm()
        up_Fillcombo()

        Dim ProdDate = DateTime.Now
        dtFromDate.Value = ProdDate
        dtToDate.Value = ProdDate

        'for disabled button Verify and Download Excel
        Grid.JSProperties("cp_GridTot") = 0
    End Sub

#End Region
#Region "Download To Excel"
    Private Sub up_Excel(cls As clsProductionSampleVerificationList)
        Try

            Using excel As New ExcelPackage

                Dim ws As ExcelWorksheet
                ws = excel.Workbook.Worksheets.Add("BO3 - Prod Sample Verifiaction List")

                dt = clsProductionSampleVerificationListDB.LoadGrid(pUser, cls)
                With ws
                    InsertHeader(ws, cls)

                    .Cells(11, 1, 12, 1).Value = "Date"
                    .Cells(11, 1, 12, 1).Merge = True

                    .Cells(11, 2, 12, 2).Value = "Shift"
                    .Cells(11, 2, 12, 2).Merge = True

                    .Cells(11, 3, 12, 3).Value = "seq"
                    .Cells(11, 3, 12, 3).Merge = True

                    .Cells(11, 4, 12, 4).Value = "Item Check"
                    .Cells(11, 4, 12, 4).Merge = True

                    .Cells(11, 5, 12, 5).Value = "Min"
                    .Cells(11, 5, 12, 5).Merge = True

                    .Cells(11, 6, 12, 6).Value = "Max"
                    .Cells(11, 6, 12, 6).Merge = True

                    .Cells(11, 7, 12, 7).Value = "Avg"
                    .Cells(11, 7, 12, 7).Merge = True

                    .Cells(11, 8, 12, 8).Value = "R"
                    .Cells(11, 8, 12, 8).Merge = True

                    .Cells(11, 9, 12, 9).Value = "Correction Status"
                    .Cells(11, 9, 12, 9).Merge = True
                    .Cells(11, 9, 12, 9).Style.WrapText = True

                    .Cells(11, 10, 12, 10).Value = "Result"
                    .Cells(11, 10, 12, 10).Merge = True

                    .Cells(11, 11, 12, 11).Value = "Sample Time"
                    .Cells(11, 11, 12, 11).Merge = True

                    .Cells(11, 12, 12, 12).Value = "Operator"
                    .Cells(11, 12, 12, 12).Merge = True

                    .Cells(11, 13, 11, 14).Value = "Verification by MK"
                    .Cells(11, 13, 11, 14).Merge = True

                    .Cells(11, 15, 11, 16).Value = "Verification by QC"
                    .Cells(11, 15, 11, 16).Merge = True

                    .Cells(12, 13).Value = "PIC"
                    .Cells(12, 14).Value = "Time"
                    .Cells(12, 15).Value = "PIC"
                    .Cells(12, 16).Value = "Time"

                    Dim Hdr As ExcelRange = .Cells(11, 1, 12, 16)
                    Hdr.Style.HorizontalAlignment = HorzAlignment.Far
                    Hdr.Style.VerticalAlignment = VertAlignment.Center
                    Hdr.Style.Font.Size = 10
                    Hdr.Style.Font.Name = "Segoe UI"
                    Hdr.Style.Font.Color.SetColor(Color.White)
                    Hdr.Style.Fill.PatternType = ExcelFillStyle.Solid
                    Hdr.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)

                    .Column(1).Width = 15
                    .Column(2).Width = 5
                    .Column(3).Width = 5
                    .Column(4).Width = 35

                    .Column(5).Width = 10
                    .Column(6).Width = 10
                    .Column(7).Width = 10
                    .Column(8).Width = 10

                    .Column(9).Width = 10
                    .Column(10).Width = 10
                    .Column(11).Width = 18
                    .Column(12).Width = 18

                    .Column(13).Width = 18
                    .Column(14).Width = 18
                    .Column(15).Width = 18
                    .Column(16).Width = 18

                    Dim irow = 13
                    For i = 0 To dt.Rows.Count - 1
                        Try
                            .Cells(irow, 1).Value = dt.Rows(i)("ProdDate")
                            .Cells(irow, 1).Style.HorizontalAlignment = HorizontalAlign.Center

                            .Cells(irow, 2).Value = dt.Rows(i)("ShiftCode")
                            .Cells(irow, 2).Style.HorizontalAlignment = HorizontalAlign.Center

                            .Cells(irow, 3).Value = dt.Rows(i)("SequenceNo")
                            .Cells(irow, 3).Style.HorizontalAlignment = HorizontalAlign.Center

                            .Cells(irow, 4).Value = dt.Rows(i)("ItemCheck")
                            .Cells(irow, 4).Style.HorizontalAlignment = HorizontalAlign.Left

                            .Cells(irow, 5).Value = dt.Rows(i)("nMin")
                            .Cells(irow, 5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                            .Cells(irow, 5).Style.Numberformat.Format = "####0.000"
                            .Cells(irow, 5).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("nMinColor")))

                            .Cells(irow, 6).Value = dt.Rows(i)("nMax")
                            .Cells(irow, 6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                            .Cells(irow, 6).Style.Numberformat.Format = "####0.000"
                            .Cells(irow, 6).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 6).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("nMaxColor")))

                            .Cells(irow, 7).Value = dt.Rows(i)("nAvg")
                            .Cells(irow, 7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                            .Cells(irow, 7).Style.Numberformat.Format = "####0.000"
                            .Cells(irow, 7).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 7).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("nAvgColor")))

                            .Cells(irow, 8).Value = dt.Rows(i)("nR")
                            .Cells(irow, 8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                            .Cells(irow, 8).Style.Numberformat.Format = "####0.000"

                            .Cells(irow, 9).Value = dt.Rows(i)("Cor_Sts")
                            .Cells(irow, 9).Style.HorizontalAlignment = HorizontalAlign.Center
                            .Cells(irow, 9).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 9).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("CorStsColor")))

                            .Cells(irow, 10).Value = dt.Rows(i)("Result")
                            .Cells(irow, 10).Style.HorizontalAlignment = HorizontalAlign.Center
                            .Cells(irow, 10).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 10).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("ResultColor")))

                            .Cells(irow, 11).Value = dt.Rows(i)("SampleTime")
                            .Cells(irow, 11).Style.HorizontalAlignment = HorizontalAlign.Center

                            .Cells(irow, 12).Value = dt.Rows(i)("Operator")
                            .Cells(irow, 12).Style.HorizontalAlignment = HorizontalAlign.Left

                            .Cells(irow, 13).Value = dt.Rows(i)("MK_PIC")
                            .Cells(irow, 13).Style.HorizontalAlignment = HorizontalAlign.Left
                            .Cells(irow, 13).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 13).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("MKColor")))

                            .Cells(irow, 14).Value = dt.Rows(i)("MK_Time")
                            .Cells(irow, 14).Style.HorizontalAlignment = HorizontalAlign.Center
                            .Cells(irow, 14).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 14).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("MKColor")))

                            .Cells(irow, 15).Value = dt.Rows(i)("QC_PIC")
                            .Cells(irow, 15).Style.HorizontalAlignment = HorizontalAlign.Left
                            .Cells(irow, 15).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 15).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("QCColor")))

                            .Cells(irow, 16).Value = dt.Rows(i)("QC_Time")
                            .Cells(irow, 16).Style.HorizontalAlignment = HorizontalAlign.Center
                            .Cells(irow, 16).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 16).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("QCColor")))

                            irow = irow + 1
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    Next

                    Dim Dtl As ExcelRange = .Cells(13, 1, irow - 1, 16)
                    Hdr.Style.VerticalAlignment = VertAlignment.Center
                    Hdr.Style.Font.Size = 10
                    Hdr.Style.Font.Name = "Segoe UI"


                    Dim Border As ExcelRange = .Cells(11, 1, irow - 1, 16)
                    Border.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Left.Style = ExcelBorderStyle.Thin

                End With
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment; filename=Production Sample Verification List_" & Format(Date.Now, "yyyy-MM-dd_HHmmss") & ".xlsx")
                Using MyMemoryStream As New MemoryStream()
                    excel.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.End()
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Private Sub InsertHeader(ByVal pExl As ExcelWorksheet, cls As clsProductionSampleVerificationList)
        With pExl
            .Cells(1, 1).Value = "Product Sample Verification List"
            .Cells(1, 1, 1, 16).Merge = True
            .Cells(1, 1, 1, 16).Style.HorizontalAlignment = HorzAlignment.Near
            .Cells(1, 1, 1, 16).Style.VerticalAlignment = VertAlignment.Center
            .Cells(1, 1, 1, 16).Style.Font.Bold = True
            .Cells(1, 1, 1, 16).Style.Font.Size = 16
            .Cells(1, 1, 1, 16).Style.Font.Name = "Segoe UI"

            .Cells(3, 1, 3, 2).Value = "Factory Code"
            .Cells(3, 1, 3, 2).Merge = True
            .Cells(3, 3).Value = ": " & cls.FactoryName

            .Cells(4, 1, 4, 2).Value = "Item Type Code"
            .Cells(4, 1, 4, 2).Merge = True
            .Cells(4, 3).Value = ": " & cls.ItemType_Name

            .Cells(5, 1, 5, 2).Value = "Line Code"
            .Cells(5, 1, 5, 2).Merge = True
            .Cells(5, 3).Value = ": " & cls.LineName

            .Cells(6, 1, 6, 2).Value = "Item Check Code"
            .Cells(6, 1, 6, 2).Merge = True
            .Cells(6, 3).Value = ": " & cls.ItemCheck_Name

            .Cells(7, 1, 7, 2).Value = "Prod Date"
            .Cells(7, 1, 7, 2).Merge = True
            .Cells(7, 3).Value = ": " & cls.Period
            .Cells(9, 1, 9, 2).Value = "MK Verification"
            .Cells(9, 1, 9, 2).Merge = True
            .Cells(9, 3).Value = ": " & cls.MKVerification_Name

            .Cells(8, 1, 8, 2).Value = "QC Verification"
            .Cells(8, 1, 8, 2).Merge = True
            .Cells(8, 3).Value = ": " & cls.QCVerification_Name

            Dim rgHeader As ExcelRange = .Cells(3, 3, 9, 4)
            rgHeader.Style.HorizontalAlignment = HorzAlignment.Near
            rgHeader.Style.VerticalAlignment = VertAlignment.Center
            rgHeader.Style.Font.Size = 10
            rgHeader.Style.Font.Name = "Segoe UI"

        End With
    End Sub
#End Region


End Class