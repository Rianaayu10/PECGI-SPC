Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports OfficeOpenXml
Imports System.Web.Services
Imports OfficeOpenXml.Style

Public Class ProdSampleQCSummary
    Inherits System.Web.UI.Page

#Region "Declarations"
    Dim pUser As String = ""
    Dim pMenuID As String = "B050"
    Public AuthAccess As Boolean = False
    Private dt As DataTable
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            pUser = Session("user")
            HF.Set("Excel", "0")
            up_Fillcombo()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu(pMenuID)
        Master.SiteTitle = pMenuID & " - " & sGlobal.menuName
        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, pMenuID)
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If
    End Sub

    Private Sub cboMachine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboMachine.Callback
        Try
            up_FillcomboMachine()
        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "Machine Callback Error " & ex.Message, 1)
        End Try
    End Sub

    Private Sub cboFrequency_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboFrequency.Callback
        Try
            up_FillcomboFrequency()
        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "Frequency Callback Error " & ex.Message, 1)
        End Try
    End Sub

    Private Sub cboSequence_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboSequence.Callback
        Try
            up_FillcomboSequence()
        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "Sequence Callback Error " & ex.Message, 1)
        End Try
    End Sub

    Protected Sub Gridx_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Gridx.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
            Dim cls As New clsProdSampleQCSummary
            Dim dTime As DateTime = dtPeriod.Value
            With cls
                .FactoryCode = HF.Get("FactoryCode")
                .ItemTypeCode = HF.Get("TypeCode")
                .MachineCode = HF.Get("MachineCode")
                .Frequency = HF.Get("FrequencyCode")
                .Sequence = cboSequence.Value
                .Period = dTime.ToString("yyyy-MM-dd")
            End With
            up_GridLoad(cls)
        End If
    End Sub

    Private Sub Gridx_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles Gridx.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)
            Dim cls As New clsProdSampleQCSummary
            Dim dTime As DateTime = dtPeriod.Value

            If pAction = "Load" Then
                With cls
                    .FactoryCode = HF.Get("FactoryCode")
                    .ItemTypeCode = HF.Get("TypeCode")
                    .MachineCode = HF.Get("MachineCode")
                    .Frequency = HF.Get("FrequencyCode")
                    .Sequence = cboSequence.Value
                    .Period = dTime.ToString("yyyy-MM-dd")
                    .UserID = pUser
                End With

                up_GridLoad(cls)
            ElseIf pAction = "Kosong" Then
                With cls
                    .FactoryCode = ""
                    .ItemTypeCode = ""
                    .MachineCode = ""
                    .Frequency = ""
                    .Sequence = "0"
                    .Period = dTime.ToString("yyyy-MM-dd")
                    .UserID = pUser
                End With

                up_GridLoad(cls)
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Private Sub Gridx_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles Gridx.HtmlDataCellPrepared
        Dim Link As New HyperLink()
        Dim cSplit As Integer = 1

        If (e.DataColumn.FieldName <> "Type") Then
            Try
                Dim check As String = Split(e.CellValue, "|,|")(1)
            Catch ex As Exception
                cSplit = 0
            End Try

            If cSplit = 0 Then
                If e.CellValue.ToString.Contains("NG") Then
                    If Split(e.CellValue, "||").Count > 1 Then
                        e.Cell.Text = ""
                        e.Cell.BackColor = ColorTranslator.FromHtml(Split(e.CellValue, "||")(1))

                        Link.ForeColor = Color.Black
                        Link.Text = "NG " & Split(e.CellValue, "||")(3)
                        Link.NavigateUrl = Split(e.CellValue, "||")(2)
                        Link.Target = "_blank"

                        e.Cell.Controls.Add(Link)
                    End If
                ElseIf e.CellValue.ToString.Contains("NoProd") Or e.CellValue.ToString.Contains("NoResult") Then
                    If Split(e.CellValue, "||").Count > 1 Then
                        e.Cell.Text = ""
                        If e.CellValue.ToString.Contains("NoResult") Then
                            Link.ForeColor = Color.Black
                            Link.NavigateUrl = Split(e.CellValue, "||")(2)
                            Link.Text = "No Data " & Split(e.CellValue, "||")(3)
                            Link.Target = "_blank"

                            e.Cell.Controls.Add(Link)

                        End If

                        'e.Cell.Text = IIf(e.CellValue.ToString.Contains("NoResult"), "No Data " & Split(e.CellValue, "||")(3), "")
                        e.Cell.BackColor = ColorTranslator.FromHtml(Split(e.CellValue, "||")(1))
                        If Split(e.CellValue, "||")(1) = "#515151" Then e.Cell.BorderColor = ColorTranslator.FromHtml("#515151")
                    End If
                ElseIf e.CellValue.ToString.Contains("NoActive") Then
                    If Split(e.CellValue, "||").Count > 1 Then
                        e.Cell.Text = "No Active"
                        e.Cell.BackColor = ColorTranslator.FromHtml(Split(e.CellValue, "||")(1))
                        e.Cell.BorderColor = ColorTranslator.FromHtml(Split(e.CellValue, "||")(1))
                    End If
                ElseIf e.CellValue.ToString.Contains("NOK") Then
                    e.Cell.Text = ""
                Else
                    e.Cell.Text = ""
                    Link.ForeColor = Color.Black
                    Link.Text = "OK " & Split(e.CellValue, "||")(2)
                    Link.NavigateUrl = Split(e.CellValue, "||")(1)
                    Link.Target = "_blank"

                    e.Cell.Controls.Add(Link)
                End If
            Else
                Dim result = "", resultURL As String = ""
                e.Cell.BackColor = ColorTranslator.FromHtml("#ef6c00")
                e.Cell.BorderColor = ColorTranslator.FromHtml("#ef6c00")
                For i = 0 To Split(e.CellValue, "|,|").Count - 1
                    Dim strSplit = Split(e.CellValue, "|,|")(i)

                    If strSplit.Contains("NoProd") Or strSplit.Contains("NoResult") Or strSplit.Contains("NOK") Then
                        If strSplit.Contains("NoResult") Then
                            result += "No Data " & Split(strSplit, "||")(3) & "<br/>"
                            resultURL = IIf(resultURL = "", Split(strSplit, "||")(2), resultURL)
                        End If
                    ElseIf strSplit.Contains("NG") Then
                        result += "NG " & Split(strSplit, "||")(3) & "<br/>"
                        resultURL = IIf(resultURL = "", Split(strSplit, "||")(2), resultURL)
                    Else
                        result += "OK " & Split(strSplit, "||")(2) & "<br/>"
                        resultURL = IIf(resultURL = "", Split(strSplit, "||")(1), resultURL)
                    End If
                Next

                e.Cell.Text = ""
                If result.Contains("No Data") And result.Contains("NG") = False And result.Contains("OK") = False Then
                    e.Cell.BackColor = ColorTranslator.FromHtml("#FFFB00")
                    e.Cell.BorderColor = ColorTranslator.FromHtml("#FFFB00")
                ElseIf result.Contains("No Data") = False And result.Contains("NG") And result.Contains("OK") = False Then
                    e.Cell.BackColor = ColorTranslator.FromHtml("#FF0000")
                    e.Cell.BorderColor = ColorTranslator.FromHtml("#FF0000")
                ElseIf result.Contains("No Data") = False And result.Contains("NG") = False And result.Contains("OK") Then
                    e.Cell.BackColor = ColorTranslator.FromHtml("#FFFFFF")
                    e.Cell.BorderColor = ColorTranslator.FromHtml("#d2d2d2")
                End If
                Link.ForeColor = Color.Black
                Link.Text = result
                Link.NavigateUrl = resultURL
                Link.Target = "_blank"

                e.Cell.Controls.Add(Link)
            End If
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        DownloadExcel()
    End Sub

    Private Sub cboLine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLine.Callback
        cboLine.DataSource = clsLineGroupDB.GetList(pUser, cboFactory.Value, IIf(cboProcess.Value = "ALL", "", cboProcess.Value), True)
        cboLine.DataBind()
        cboLine.SelectedIndex = 0
    End Sub

    Private Sub cboMachineIOT_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboMachineIOT.Callback
        cboMachineIOT.DataSource = clsProcessDB.GetList(pUser, cboFactory.Value, IIf(cboProcess.Value = "ALL", "", cboProcess.Value), IIf(cboLine.Value = "ALL", "", cboLine.Value), True)
        cboMachineIOT.DataBind()
        cboMachineIOT.SelectedIndex = 0
    End Sub

    Private Sub cboProcess_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcess.Callback
        cboProcess.DataSource = clsProcessGroupDB.GetList(pUser, cboFactory.Value, True)
        cboProcess.DataBind()
        cboProcess.SelectedIndex = 0
    End Sub
#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Gridx.JSProperties("cp_message") = ErrMsg
        Gridx.JSProperties("cp_type") = msgType
        Gridx.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_Fillcombo()
        Try
            up_FillcomboFactory()

            cboProcess.DataSource = clsProcessGroupDB.GetList(pUser, cboFactory.Value, True)
            cboProcess.DataBind()
            cboProcess.SelectedIndex = 0

            cboLine.DataSource = clsLineGroupDB.GetList(pUser, cboFactory.Value, cboProcess.Value, True)
            cboLine.DataBind()
            cboLine.SelectedIndex = 0

            cboMachineIOT.DataSource = clsProcessDB.GetList(pUser, cboFactory.Value, cboProcess.Value, cboLine.Value, True)
            cboMachineIOT.DataBind()
            cboMachineIOT.SelectedIndex = 0

            up_FillcomboType()
            up_FillcomboMachine()
            up_FillcomboFrequency()
            up_FillcomboSequence()

        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "", 1)
        End Try
    End Sub

    Private Sub up_FillcomboFactory()
        If HF.Get("Excel") = "0" Then
            Dim a As String = ""
            dt = clsProdSampleQCSummaryDB.FillCombo("0", pUser)
            With cboFactory
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0 'IIf(dt.Rows.Count > 0, 0, -1)

                If .SelectedIndex < 0 Then
                    a = ""
                Else
                    a = .SelectedItem.GetFieldValue("Code")
                End If
            End With
            HF.Set("FactoryCode", a)
        End If
    End Sub

    Private Sub up_FillcomboType()
        If HF.Get("Excel") = "0" Then
            Dim a As String = ""
            dt = clsProdSampleQCSummaryDB.FillCombo("1")
            With cboType
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)

                If .SelectedIndex < 0 Then
                    a = ""
                Else
                    a = .SelectedItem.GetFieldValue("Code")
                End If
            End With
            HF.Set("TypeCode", a)
        End If
    End Sub

    Private Sub up_FillcomboMachine()
        If HF.Get("Excel") = "0" Then
            Dim a As String = ""
            dt = clsProdSampleQCSummaryDB.FillCombo("2", cboFactory.Value, cboMachineIOT.Value, pUser)
            With cboMachine
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)

                If .SelectedIndex < 0 Then
                    a = ""
                Else
                    a = .SelectedItem.GetFieldValue("Code")
                End If
            End With
            HF.Set("MachineCode", a)
        End If
    End Sub

    Private Sub up_FillcomboFrequency()
        If HF.Get("Excel") = "0" Then
            Dim a As String = ""
            dt = clsProdSampleQCSummaryDB.FillCombo("3", HF.Get("FactoryCode"), HF.Get("TypeCode"), HF.Get("MachineCode"), pUser)
            With cboFrequency
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)

                If .SelectedIndex < 0 Then
                    a = ""
                Else
                    a = .SelectedItem.GetFieldValue("Code")
                End If
            End With
            HF.Set("FrequencyCode", a)
        End If
    End Sub

    Private Sub up_FillcomboSequence()
        If HF.Get("Excel") = "0" Then
            dt = clsProdSampleQCSummaryDB.FillCombo("4", HF.Get("FactoryCode"), HF.Get("FrequencyCode"), HF.Get("TypeCode"), HF.Get("MachineCode"), pUser)
            With cboSequence
                .DataSource = dt
                .DataBind()
                .SelectedIndex = IIf(dt.Rows.Count > 0, 0, -1)
            End With
        End If
    End Sub

    Private Sub up_GridLoad(cls As clsProdSampleQCSummary)
        Dim OK As Integer = 0, NG As Integer = 0, no As Integer = 0, sampletime As String = "", ds As New DataSet
        With Gridx
            .Columns.Clear()

            Dim Col0 As New GridViewDataTextColumn
            Col0.FieldName = "Type"
            Col0.Caption = "Type"
            Col0.Width = 75

            Col0.FixedStyle = GridViewColumnFixedStyle.Left
            Col0.HeaderStyle.Wrap = DefaultBoolean.True
            Col0.HeaderStyle.VerticalAlign = VerticalAlign.Middle
            Col0.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            Col0.CellStyle.VerticalAlign = VerticalAlign.Middle
            Col0.CellStyle.HorizontalAlign = HorizontalAlign.Center
            .Columns.Add(Col0)

            ds = clsProdSampleQCSummaryDB.GetList(cls)
            dt = ds.Tables(0)

            If dt.Rows.Count > 0 Then
                For i = 1 To dt.Columns.Count - 1
                    Dim Col As New GridViewDataTextColumn

                    With Col
                        .FieldName = dt.Columns(i).ColumnName
                        .Caption = dt.Columns(i).ColumnName
                        .Width = 100

                        .HeaderStyle.Wrap = DefaultBoolean.True
                        .HeaderStyle.VerticalAlign = VerticalAlign.Middle
                        .HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                        .CellStyle.VerticalAlign = VerticalAlign.Middle
                        .CellStyle.HorizontalAlign = HorizontalAlign.Center
                    End With

                    .Columns.Add(Col)
                Next

                If ds.Tables(1).Rows.Count = 0 Then
                    sampletime = "-"
                Else
                    sampletime = ds.Tables(1).Rows(0)(0)
                End If
                If cls.Frequency = "ALL" Or cls.Sequence = "ALL" Then sampletime = "ALL"


                If ds.Tables(2).Select("Result Like '%OK%'").Length > 0 Then
                    OK = ds.Tables(2).Select("Result Like '%OK%'")(0)("Jumlah")
                End If

                If ds.Tables(2).Select("Result Like '%NG%'").Length > 0 Then
                    NG = ds.Tables(2).Select("Result Like '%NG%'")(0)("Jumlah")
                End If

                If ds.Tables(2).Select("Result Like '%Delay%'").Length > 0 Then
                    no = ds.Tables(2).Select("Result Like '%Delay%'")(0)("Jumlah")
                End If


                .JSProperties("cp_header") = "Yes"
                .JSProperties("cp_sampletime") = sampletime
                .JSProperties("cp_ok") = OK
                .JSProperties("cp_ng") = NG
                .JSProperties("cp_no") = no
                .JSProperties("cp_total") = OK + NG + no
            Else
                .JSProperties("cp_header") = "No"
                If HF.Get("Excel") = "2" Then show_error(MsgTypeEnum.Warning, "Data Not Found", 1)
            End If
            .DataSource = dt
            .DataBind()

        End With
    End Sub

    Private Sub DownloadExcel()
        Try
            Dim cls As New clsProdSampleQCSummary
            Dim cSplit As Integer = 1
            Dim dTime As DateTime = dtPeriod.Value

            With cls
                .FactoryCode = HF.Get("FactoryCode")
                .ItemTypeCode = HF.Get("TypeCode")
                .MachineCode = HF.Get("MachineCode")
                .Frequency = HF.Get("FrequencyCode")
                .Sequence = cboSequence.Value
                .Period = dTime.ToString("yyyy-MM-dd")
                .UserID = pUser
            End With
            dt = clsProdSampleQCSummaryDB.GetList(cls).Tables(0)

            Using excel As New ExcelPackage
                Dim ws As ExcelWorksheet = excel.Workbook.Worksheets.Add("B05 - Sample Control Quality Summary")
                Dim rowsExcel As Integer = 1
                Dim lastCol As Integer = dt.Columns.Count - 1

                With ws
                    'Start Filter Title
                    .Cells(1, 1).Value = "Factory"
                    .Cells(1, 1).Style.Font.Size = 12
                    .Cells(1, 1).Style.Font.Bold = True

                    .Cells(1, 2).Value = ": " + cboFactory.Text
                    .Cells(1, 2).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(1, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(1, 2).Style.Font.Size = 12
                    .Cells(1, 2).Style.Font.Bold = True

                    .Cells(1, 3).Value = "Machine"
                    .Cells(1, 3).Style.Font.Size = 12
                    .Cells(1, 3).Style.Font.Bold = True

                    .Cells(1, 4).Value = ": " + cboMachineIOT.Text
                    .Cells(1, 4).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(1, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(1, 4).Style.Font.Size = 12
                    .Cells(1, 4).Style.Font.Bold = True

                    .Cells(1, 5).Value = "Period"
                    .Cells(1, 5).Style.Font.Size = 12
                    .Cells(1, 5).Style.Font.Bold = True

                    .Cells(1, 6).Value = ": " + dTime.ToString("dd MMMM yyyy")
                    .Cells(1, 6).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(1, 6).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(1, 6).Style.Font.Size = 12
                    .Cells(1, 6).Style.Font.Bold = True

                    '----------------Moving Rows--------------------'

                    .Cells(2, 1).Value = "Process Group"
                    .Cells(2, 1).Style.Font.Size = 12
                    .Cells(2, 1).Style.Font.Bold = True

                    .Cells(2, 2).Value = ": " + cboProcess.Text
                    .Cells(2, 2).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(2, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(2, 2).Style.Font.Size = 12
                    .Cells(2, 2).Style.Font.Bold = True

                    .Cells(2, 3).Value = "Machine Process"
                    .Cells(2, 3).Style.Font.Size = 12
                    .Cells(2, 3).Style.Font.Bold = True

                    .Cells(2, 4).Value = ": " + cboMachine.Text
                    .Cells(2, 4).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(2, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(2, 4).Style.Font.Size = 12
                    .Cells(2, 4).Style.Font.Bold = True

                    .Cells(2, 5).Value = "Frequency"
                    .Cells(2, 5).Style.Font.Size = 12
                    .Cells(2, 5).Style.Font.Bold = True

                    .Cells(2, 6).Value = ": " + cboFrequency.Text
                    .Cells(2, 6).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(2, 6).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(2, 6).Style.Font.Size = 12
                    .Cells(2, 6).Style.Font.Bold = True
                    .Cells(2, 6).Style.Numberformat.Format = "@"

                    '----------------Moving Rows--------------------'

                    .Cells(3, 1).Value = "Line Group"
                    .Cells(3, 1).Style.Font.Size = 12
                    .Cells(3, 1).Style.Font.Bold = True

                    .Cells(3, 2).Value = ": " + cboLine.Text
                    .Cells(3, 2).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(3, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(3, 2).Style.Font.Size = 12
                    .Cells(3, 2).Style.Font.Bold = True

                    .Cells(3, 3).Value = "Type"
                    .Cells(3, 3).Style.Font.Size = 12
                    .Cells(3, 3).Style.Font.Bold = True

                    .Cells(3, 4).Value = ": " + cboType.Text
                    .Cells(3, 4).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(3, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(3, 4).Style.Font.Size = 12
                    .Cells(3, 4).Style.Font.Bold = True

                    .Cells(3, 5).Value = "Sequence"
                    .Cells(3, 5).Style.Font.Size = 12
                    .Cells(3, 5).Style.Font.Bold = True

                    .Cells(3, 6).Value = ": " + cboSequence.Text
                    .Cells(3, 6).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(3, 6).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(3, 6).Style.Font.Size = 12
                    .Cells(3, 6).Style.Font.Bold = True
                    .Cells(3, 6).Style.Numberformat.Format = "@"

                    'End Filter Title

                    'Start Info Title

                    .Cells(5, 1).Value = "Sample Time"
                    .Cells(5, 1).Style.Font.Size = 12

                    .Cells(5, 2).Value = ": " & HF.Get("sampletime")
                    .Cells(5, 2).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(5, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(5, 2).Style.Font.Size = 12

                    .Cells(5, 3).Value = "OK Result"
                    .Cells(5, 3).Style.Font.Size = 12

                    .Cells(5, 4).Value = ": " + HF.Get("ok").ToString()
                    .Cells(5, 4).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(5, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(5, 4).Style.Font.Size = 12

                    '----------------Moving Rows--------------------'

                    .Cells(6, 1).Value = "Total Sample"
                    .Cells(6, 1).Style.Font.Size = 12

                    .Cells(6, 2).Value = ": " + HF.Get("total").ToString()
                    .Cells(6, 2).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(6, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(6, 2).Style.Font.Size = 12

                    .Cells(6, 3).Value = "NG Result"
                    .Cells(6, 3).Style.Font.Size = 12

                    .Cells(6, 4).Value = ": " + HF.Get("ng").ToString()
                    .Cells(6, 4).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(6, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(6, 4).Style.Font.Size = 12
                    .Cells(6, 4).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                    .Cells(6, 4).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red)

                    '----------------Moving Rows--------------------'

                    .Cells(7, 3).Value = "Delay"
                    .Cells(7, 3).Style.Font.Size = 12

                    .Cells(7, 4).Value = ": " + HF.Get("NOK").ToString()
                    .Cells(7, 4).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                    .Cells(7, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                    .Cells(7, 4).Style.Font.Size = 12
                    .Cells(7, 4).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                    .Cells(7, 4).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)

                    'End Info Title
                    rowsExcel = 10
                    'Start Header FieldName

                    For i = 0 To lastCol
                        .Cells(rowsExcel, i + 1).Value = dt.Columns(i).ColumnName
                        .Cells(rowsExcel, i + 1).Style.Font.Bold = True
                        .Cells(rowsExcel, i + 1).Style.Font.Size = 12
                        .Cells(rowsExcel, i + 1).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                        .Cells(rowsExcel, i + 1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                    Next

                    .Column(5).Width = 11 'Lebar kolom E
                    Dim Hdr As ExcelRange = .Cells(rowsExcel, 1, rowsExcel, lastCol + 1)
                    Hdr.Style.Font.Color.SetColor(Color.White)
                    Hdr.Style.Fill.PatternType = ExcelFillStyle.Solid
                    Hdr.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)

                    rowsExcel += 1
                    If dt.Rows.Count > 0 Then
                        For i = 0 To dt.Rows.Count - 1
                            'dataCount = ""
                            For j = 0 To lastCol
                                .Cells(rowsExcel, j + 1).Style.Font.Size = 10
                                .Cells(rowsExcel, j + 1).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
                                .Cells(rowsExcel, j + 1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                                If j = 0 Then
                                    .Cells(rowsExcel, j + 1).Value = dt.Rows(i)(j).ToString()
                                    .Column(j + 1).Width = 15
                                ElseIf j <> 0 Then
                                    cSplit = 1
                                    Dim value As String = dt.Rows(i)(j).ToString()
                                    .Column(j + 1).Width = 17

                                    Try
                                        Dim check As String = Split(value, "|,|")(1)
                                    Catch ex As Exception
                                        cSplit = 0
                                    End Try

                                    If cSplit = 0 Then
                                        If value.Contains("NG") Then
                                            If Split(value, "||").Count > 1 Then
                                                .Cells(rowsExcel, j + 1).Value = "NG " + Split(value, "||")(3)
                                                .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                                .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Split(value, "||")(1)))
                                            End If
                                        ElseIf value.Contains("NoProd") Or value.Contains("NoResult") Then
                                            If Split(value, "||").Count > 1 Then
                                                .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                                If value.Contains("NoResult") Then
                                                    .Cells(rowsExcel, j + 1).Value = "No Data " & Split(value, "||")(3)
                                                    .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Split(value, "||")(1)))
                                                Else
                                                    .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Split(value, "||")(1)))
                                                End If
                                            End If
                                        ElseIf value.Contains("NoActive") Then
                                            If Split(value, "||").Count > 1 Then
                                                .Cells(rowsExcel, j + 1).Value = "No Active"
                                                .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                                .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Split(value, "||")(1)))
                                            End If
                                        ElseIf value.Contains("NOK") Then
                                            .Cells(rowsExcel, j + 1).Value = ""
                                        Else
                                            .Cells(rowsExcel, j + 1).Value = "OK" & Split(value, "||")(2)
                                        End If
                                    Else
                                        Dim result = "", resultURL As String = ""
                                        .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                        .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ef6c00"))
                                        For ii = 0 To Split(value, "|,|").Count - 1
                                            Dim strSplit = Split(value, "|,|")(ii)

                                            If strSplit.Contains("NoProd") Or strSplit.Contains("NoResult") Or strSplit.Contains("NOK") Then
                                                If strSplit.Contains("NoResult") Then
                                                    result += "No Data " & Split(strSplit, "||")(3) & vbCrLf
                                                End If
                                            ElseIf strSplit.Contains("NG") Then
                                                result += "NG " & Split(strSplit, "||")(3) & vbCrLf
                                            Else
                                                result += "OK " & Split(strSplit, "||")(2) & vbCrLf
                                            End If
                                        Next

                                        If result.Contains("OK") And result.Contains("NG") = False And result.Contains("No Data") = False Then
                                            .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                            .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffffff"))
                                        ElseIf result.Contains("OK") = False And result.Contains("NG") And result.Contains("No Data") = False Then
                                            .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                            .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"))
                                        ElseIf result.Contains("OK") = False And result.Contains("NG") = False And result.Contains("No Data") Then
                                            .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                            .Cells(rowsExcel, j + 1, rowsExcel, j + 1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFB00"))
                                        End If

                                        .Cells(rowsExcel, j + 1).Value = Left(result, result.Length - 2)
                                        .Cells(rowsExcel, j + 1).Style.WrapText = True
                                    End If
                                End If
                            Next
                            rowsExcel += 1
                        Next

                        .View.FreezePanes(11, 2)
                        .Cells(10, 2, 10, lastCol + 1).Style.WrapText = True
                        .Cells(10, 1, rowsExcel - 1, lastCol + 1).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
                        .Cells(10, 1, rowsExcel - 1, lastCol + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                        .Cells(10, 1, rowsExcel - 1, lastCol + 1).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                        .Cells(10, 1, rowsExcel - 1, lastCol + 1).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
                    End If
                End With

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment; filename=Sample Control Quality Summary_" & Format(Date.Now, "yyyy-MM-dd_HHmmss") & ".xlsx")
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
#End Region

End Class