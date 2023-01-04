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
    Dim ProcessGroup_Sel As String = "7"
    Dim LineGroup_Sel As String = "8"
    Dim ProcessCode_Sel As String = "9"
    Dim GetFilter As String = "10"

    ' PARAMETER GRID COLOR
    Dim nMinColor As String = ""
    Dim nMaxColor As String = ""
    Dim nAvgColor As String = ""
    Dim nRColor As String = ""
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
    Dim sProcessGroup = ""
    Dim sLineGroup = ""
    Dim sProcessCode = ""

    ' PARAMETE EXCEL
    Dim totRowHdr = 0

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

    Private Sub cboProcessGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcessGroup.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = e.Parameter
            data.UserID = pUser

            Dim ErrMsg As String = ""
            dt = clsProductionSampleVerificationListDB.FillCombo(ProcessGroup_Sel, data)
            With cboProcessGroup
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboLineGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineGroup.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ProcessGroup = Split(e.Parameter, "|")(1)
            data.UserID = pUser

            Dim ErrMsg As String = ""
            dt = clsProductionSampleVerificationListDB.FillCombo(LineGroup_Sel, data)
            With cboLineGroup
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboProcessCode_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcessCode.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ProcessGroup = Split(e.Parameter, "|")(1)
            data.LineGroup = Split(e.Parameter, "|")(2)
            data.UserID = pUser

            Dim ErrMsg As String = ""
            dt = clsProductionSampleVerificationListDB.FillCombo(ProcessCode_Sel, data)
            With cboProcessCode
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboLineID_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineID.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ProcessCode = e.Parameter.Split("|")(1)
            data.UserID = pUser

            Dim ErrMsg As String = ""
            dt = clsProductionSampleVerificationListDB.FillCombo(Line_Sel, data)
            With cboLineID
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboItemType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemType.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ProcessCode = e.Parameter.Split("|")(1)
            data.LineCode = e.Parameter.Split("|")(2)
            data.UserID = pUser

            Dim ErrMsg As String = ""
            dt = clsProductionSampleVerificationListDB.FillCombo(ItemType_Sel, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboItemCheck_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemCheck.Callback
        Try
            Dim data As New clsProductionSampleVerificationList()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.LineCode = Split(e.Parameter, "|")(1)
            data.ItemType_Code = Split(e.Parameter, "|")(2)
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
        cls.FactoryCode = cboFactory.Value
        cls.FactoryName = cboFactory.Text
        cls.ProcessGroup = cboProcessGroup.Value
        cls.ProcessGroupName = cboProcessGroup.Text
        cls.LineGroup = cboLineGroup.Value
        cls.LineGroupName = cboLineGroup.Text
        cls.ProcessCode = cboProcessCode.Value
        cls.ProcessName = cboProcessCode.Text
        cls.ItemType_Code = cboItemType.Value
        cls.ItemType_Name = cboItemType.Text
        cls.LineCode = cboLineID.Value
        cls.LineName = cboLineID.Text
        cls.ItemCheck_Code = cboItemCheck.Value
        cls.ItemCheck_Name = cboItemCheck.Text
        cls.ProdDateFrom = Convert.ToDateTime(dtFromDate.Value).ToString("yyyy-MM-dd")
        cls.ProdDateTo = Convert.ToDateTime(dtToDate.Value).ToString("yyyy-MM-dd")
        cls.Period = Convert.ToDateTime(dtFromDate.Value).ToString("dd MMM yyyy") & " - " & Convert.ToDateTime(dtToDate.Value).ToString("dd MMM yyyy")
        cls.MKVerification = cboMK.Value
        cls.QCVerification = cboQC.Value
        cls.MKVerification_Name = cboMK.Text
        cls.QCVerification_Name = cboQC.Text

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
            ElseIf e.DataColumn.FieldName = "nRColor" Then
                nRColor = e.CellValue
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
            ElseIf e.DataColumn.FieldName = "nR" Then
                e.Cell.BackColor = ColorTranslator.FromHtml(nRColor)
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
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
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

            '============ FILL COMBO PROCESS GROUP ================'
            dt = clsProductionSampleVerificationListDB.FillCombo(ProcessGroup_Sel, data)
            With cboProcessGroup
                .DataSource = dt
                .DataBind()
            End With

            If sProcessGroup <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sProcessGroup Then
                        cboProcessGroup.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
            If cboProcessGroup.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboProcessGroup.SelectedItem.GetFieldValue("CODE")
            End If
            data.ProcessGroup = a
            '======================================================'

            '============ FILL COMBO LINE GROUP ================'
            dt = clsProductionSampleVerificationListDB.FillCombo(LineGroup_Sel, data)
            With cboLineGroup
                .DataSource = dt
                .DataBind()
            End With

            If sLineGroup <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sLineGroup Then
                        cboLineGroup.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
            If cboLineGroup.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboLineGroup.SelectedItem.GetFieldValue("CODE")
            End If
            data.LineGroup = a
            '======================================================'

            '============ FILL COMBO PROCESS CODE ================'
            dt = clsProductionSampleVerificationListDB.FillCombo(ProcessCode_Sel, data)
            With cboProcessCode
                .DataSource = dt
                .DataBind()
            End With

            If sProcessCode <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sProcessCode Then
                        cboProcessCode.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
            If cboProcessCode.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboProcessCode.SelectedItem.GetFieldValue("CODE")
            End If
            data.ProcessCode = a
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

            '============== FILL COMBO ITEM CODE =================='         

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

                If cboItemType.SelectedIndex < 0 Then
                    a = ""
                Else
                    a = cboItemType.SelectedItem.GetFieldValue("CODE")
                End If
            End If
            data.ItemType_Code = a
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
    Private Sub LoadForm()
        up_Fillcombo()

        Dim ProdDate = DateTime.Now
        dtFromDate.Value = ProdDate
        dtToDate.Value = ProdDate

        'for disabled button Verify and Download Excel
        Grid.JSProperties("cp_GridTot") = 0
    End Sub
    Private Sub LoadForm_ByAnotherform()
        Dim dt As New DataTable
        Dim cls As New clsProductionSampleVerificationList
        cls.FactoryCode = Request.QueryString("FactoryCode")
        cls.ItemType_Code = Request.QueryString("ItemTypeCode")
        cls.LineCode = Request.QueryString("Line")
        cls.ItemCheck_Code = Request.QueryString("ItemCheckCode")
        cls.ProdDateFrom = Convert.ToDateTime(Request.QueryString("FromDate")).ToString("yyyy-MM-dd")
        cls.ProdDateTo = Convert.ToDateTime(Request.QueryString("ToDate")).ToString("yyyy-MM-dd")
        cls.MKVerification = Request.QueryString("MK")
        cls.QCVerification = Request.QueryString("QC")
        cls.UserID = pUser

        dt = clsProductionSampleVerificationListDB.GetFilterCombo(GetFilter, cls)
        sFactoryCode = dt.Rows(0)("FactoryCode").ToString
        sProcessGroup = dt.Rows(0)("ProcessGroup").ToString
        sLineGroup = dt.Rows(0)("LineGroup").ToString
        sProcessCode = dt.Rows(0)("ProcessCode").ToString
        sLineCode = dt.Rows(0)("LineCode").ToString
        sItemType = dt.Rows(0)("ItemTypeCode").ToString
        sItemCheck = Request.QueryString("ItemCheckCode")
        sMKVerification = Request.QueryString("MK")
        sQCVerification = Request.QueryString("QC")
        sProdDateFrom = Request.QueryString("FromDate")
        sProdDateTo = Request.QueryString("ToDate")


        up_Fillcombo()
        UpGridLoad(cls)

        dtFromDate.Value = Convert.ToDateTime(sProdDateFrom)
        dtToDate.Value = Convert.ToDateTime(sProdDateTo)
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

                    Dim rowStart = totRowHdr + 2
                    Dim irow = rowStart

                    .Cells(irow, 1, irow + 1, 1).Value = "Date"
                    .Cells(irow, 1, irow + 1, 1).Merge = True

                    .Cells(irow, 2, irow + 1, 2).Value = "Shift"
                    .Cells(irow, 2, irow + 1, 2).Merge = True

                    .Cells(irow, 3, irow + 1, 3).Value = "seq"
                    .Cells(irow, 3, irow + 1, 3).Merge = True

                    .Cells(irow, 4, irow + 1, 4).Value = "Item Check"
                    .Cells(irow, 4, irow + 1, 4).Merge = True

                    .Cells(irow, 5, irow + 1, 5).Value = "Min"
                    .Cells(irow, 5, irow + 1, 5).Merge = True

                    .Cells(irow, 6, irow + 1, 6).Value = "Max"
                    .Cells(irow, 6, irow + 1, 6).Merge = True

                    .Cells(irow, 7, irow + 1, 7).Value = "Avg"
                    .Cells(irow, 7, irow + 1, 7).Merge = True

                    .Cells(irow, 8, irow + 1, 8).Value = "R"
                    .Cells(irow, 8, irow + 1, 8).Merge = True

                    .Cells(irow, 9, irow + 1, 9).Value = "Correction Status"
                    .Cells(irow, 9, irow + 1, 9).Merge = True
                    .Cells(irow, 9, irow + 1, 9).Style.WrapText = True

                    .Cells(irow, 10, irow + 1, 10).Value = "Result"
                    .Cells(irow, 10, irow + 1, 10).Merge = True

                    .Cells(irow, 11, irow + 1, 11).Value = "Sample Time"
                    .Cells(irow, 11, irow + 1, 11).Merge = True

                    .Cells(irow, 12, irow + 1, 12).Value = "Operator"
                    .Cells(irow, 12, irow + 1, 12).Merge = True

                    .Cells(irow, 13, irow + 1, 14).Value = "Verification by MK"
                    .Cells(irow, 13, irow + 1, 14).Merge = True

                    .Cells(irow, 15, irow + 1, 16).Value = "Verification by QC"
                    .Cells(irow, 15, irow + 1, 16).Merge = True

                    .Cells(irow + 1, 13).Value = "PIC"
                    .Cells(irow + 1, 14).Value = "Time"
                    .Cells(irow + 1, 15).Value = "PIC"
                    .Cells(irow + 1, 16).Value = "Time"

                    .Cells(irow, 17, irow + 1, 17).Value = "Lot No"
                    .Cells(irow, 17, irow + 1, 17).Merge = True

                    .Cells(irow, 18, irow + 1, 18).Value = "Remark"
                    .Cells(irow, 18, irow + 1, 18).Merge = True

                    Dim Hdr As ExcelRange = .Cells(irow, 1, irow + 1, 18)
                    Hdr.Style.HorizontalAlignment = HorzAlignment.Far
                    Hdr.Style.VerticalAlignment = VertAlignment.Center
                    Hdr.Style.Font.Size = 10
                    Hdr.Style.Font.Name = "Segoe UI"
                    Hdr.Style.Font.Color.SetColor(Color.White)
                    Hdr.Style.Fill.PatternType = ExcelFillStyle.Solid
                    Hdr.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)

                    .Column(1).Width = 15
                    .Column(2).Width = 10
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

                    .Column(17).Width = 25
                    .Column(18).Width = 30

                    irow = irow + 2

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
                            .Cells(irow, 8).Style.Fill.PatternType = ExcelFillStyle.Solid
                            .Cells(irow, 8).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(dt.Rows(i)("nRColor")))

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

                            .Cells(irow, 17).Value = dt.Rows(i)("LotNo")
                            .Cells(irow, 17).Style.HorizontalAlignment = HorizontalAlign.Left

                            .Cells(irow, 18).Value = dt.Rows(i)("Remark")
                            .Cells(irow, 18).Style.HorizontalAlignment = HorizontalAlign.Left

                            irow = irow + 1
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    Next

                    Dim Dtl As ExcelRange = .Cells(irow, 1, irow - 1, 18)
                    Hdr.Style.VerticalAlignment = VertAlignment.Center
                    Hdr.Style.Font.Size = 10
                    Hdr.Style.Font.Name = "Segoe UI"


                    Dim Border As ExcelRange = .Cells(rowStart, 1, irow - 1, 18)
                    Border.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Left.Style = ExcelBorderStyle.Thin
                    Border.Style.WrapText = True
                    Border.Style.VerticalAlignment = ExcelVerticalAlignment.Center

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
            Dim irow = 1

            .Cells(irow, 1).Value = "Product Sample Verification List"
            .Cells(irow, 1, irow, 16).Merge = True
            .Cells(irow, 1, irow, 16).Style.HorizontalAlignment = HorzAlignment.Near
            .Cells(irow, 1, irow, 16).Style.VerticalAlignment = VertAlignment.Center
            .Cells(irow, 1, irow, 16).Style.Font.Bold = True
            .Cells(irow, 1, irow, 16).Style.Font.Size = 16
            .Cells(irow, 1, irow, 16).Style.Font.Name = "Segoe UI"
            irow = irow + 2

            .Cells(irow, 1).Value = "Factory"
            .Cells(irow, 3).Value = ": " & cls.FactoryName
            .Cells(irow, 5).Value = "Machine"
            .Cells(irow, 7).Value = ": " & cls.ProcessName
            .Cells(irow, 11).Value = "Item Check"
            .Cells(irow, 12).Value = ": " & cls.ItemCheck_Name
            irow = irow + 1

            .Cells(irow, 1).Value = "Process Group"
            .Cells(irow, 3).Value = ": " & cls.ProcessGroupName
            .Cells(irow, 5).Value = "Machine Process"
            .Cells(irow, 7).Value = ": " & cls.LineName
            .Cells(irow, 11).Value = "Prod. Date"
            .Cells(irow, 12).Value = ": " & cls.Period
            irow = irow + 1

            .Cells(irow, 1).Value = "Line Group"
            .Cells(irow, 3).Value = ": " & cls.LineGroupName
            .Cells(irow, 5).Value = "Type"
            .Cells(irow, 7).Value = ": " & cls.ItemType_Name
            .Cells(irow, 11).Value = "QC Verification"
            .Cells(irow, 12).Value = ": " & cls.QCVerification_Name
            .Cells(irow, 13).Value = "MK Verification"
            .Cells(irow, 14).Value = ": " & cls.MKVerification_Name
            irow = irow + 1

            Dim rgHeader As ExcelRange = .Cells(3, 1, 6, 15)
            rgHeader.Style.HorizontalAlignment = HorzAlignment.Near
            rgHeader.Style.VerticalAlignment = VertAlignment.Center
            rgHeader.Style.Font.Bold = True
            rgHeader.Style.Font.Size = 10
            rgHeader.Style.Font.Name = "Segoe UI"
            totRowHdr = irow

        End With
    End Sub
#End Region


End Class