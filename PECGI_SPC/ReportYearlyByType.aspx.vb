Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports OfficeOpenXml.Drawing

Public Class ReportYearlyByType
    Inherits System.Web.UI.Page
    Dim Factory_ComboType = "1"
    Dim ProcessGroup_ComboType = "2"
    Dim LineGroup_ComboType = "3"
    Dim ProcessCode_ComboType = "4"
    Dim LineCode_ComboType = "5"
    Dim ItemType_ComboType = "6"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("user") Is Nothing Then
                Response.Redirect("~/Default.aspx")
            End If
            UpFillCombo()
            Master.SiteTitle = "C040 - FTA Report Yearly By Type"
            'Dim dfrom As Date
            'dfrom = Format(Now, "yyyy-MM-01")
            'TmPeriod_From.Value = dfrom
        End If
    End Sub

    Private Sub cboProcessGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcessGroup.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = e.Parameter
            data.UserID = Session("user").ToString

            dt = clsReportYearlyByType.FillCombo(ProcessGroup_ComboType, data)
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
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ProcessGroup = Split(e.Parameter, "|")(1)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(LineGroup_ComboType, data)
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
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ProcessGroup = Split(e.Parameter, "|")(1)
            data.LineGroup = Split(e.Parameter, "|")(2)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(ProcessCode_ComboType, data)
            With cboProcessCode
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub cboLineCode_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineCode.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ProcessCode = e.Parameter.Split("|")(1)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(LineCode_ComboType, data)
            With cboLineCode
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub cboItemType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemType.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ProcessCode = e.Parameter.Split("|")(1)
            data.LineCode = e.Parameter.Split("|")(2)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(ItemType_ComboType, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        cb.JSProperties("cp_message") = ErrMsg
        cb.JSProperties("cp_type") = msgType
        cb.JSProperties("cp_val") = pVal
    End Sub

    Private Sub UpFillCombo()
        Try
            Dim HD As String
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.UserID = Session("user").ToString

            'FILL COMBO FACTORY CODE'
            dt = clsReportYearlyByType.FillCombo(Factory_ComboType, data)
            With cboFactory
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboFactory.SelectedItem.GetFieldValue("Code")
            HideValue.Set("UserID", data.UserID)
            HideValue.Set("FactoryCode", HD)
            data.FactoryCode = HideValue.Get("FactoryCode")

            'FILL COMBO PROCESS GROUP'
            dt = clsReportYearlyByType.FillCombo(ProcessGroup_ComboType, data)
            With cboProcessGroup
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboProcessGroup.SelectedItem.GetFieldValue("Code")
            HideValue.Set("ProcessGroup", HD)
            data.ProcessGroup = HideValue.Get("ProcessGroup")

            'FILL COMBO LINE GROUP'
            dt = clsReportYearlyByType.FillCombo(LineGroup_ComboType, data)
            With cboLineGroup
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboLineGroup.SelectedItem.GetFieldValue("Code")
            HideValue.Set("LineGroup", HD)
            data.LineGroup = HideValue.Get("LineGroup")

            'FILL COMBO PROCESS CODE'
            dt = clsReportYearlyByType.FillCombo(ProcessCode_ComboType, data)
            With cboProcessCode
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboProcessCode.SelectedItem.GetFieldValue("Code")
            HideValue.Set("ProcessCode", HD)
            data.ProcessCode = HideValue.Get("ProcessCode")

            'FILL COMBO LINECODE'
            dt = clsReportYearlyByType.FillCombo(LineCode_ComboType, data)
            With cboLineCode
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboLineCode.SelectedItem.GetFieldValue("Code")
            HideValue.Set("LineCode", HD)
            data.LineCode = HideValue.Get("LineCode")

            'FILL ITEM TYPE'
            dt = clsReportYearlyByType.FillCombo(ItemType_ComboType, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With


        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "", 0)
        End Try
    End Sub

    'Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click

    'End Sub

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function LoadData(User As String, FactoryCode As String, ProcessGroup As String, LineGroup As String, ProcessCode As String, LineCode As String, ItemType As String, ProdDate_From As String, ProdDate_To As String) As clsContent
        Dim content As New clsContent
        Try
            Dim dt As New DataTable
            Dim resp As New List(Of clsReportYearlyByType)
            Dim myTable As List(Of String()) = New List(Of String())()
            Dim data As New clsReportYearlyByType
            data.UserID = User
            data.FactoryCode = FactoryCode
            data.ProcessGroup = ProcessGroup
            data.LineGroup = LineGroup
            data.ProcessCode = ProcessCode
            data.LineCode = LineCode
            data.ItemType = ItemType
            data.ProdDateFrom = ProdDate_From
            data.ProdDateTo = ProdDate_To

            dt = clsReportYearlyByType.LoadData(data)

            If dt.Rows(0)(0).ToString <> "" Then
                For n = 0 To dt.Rows.Count - 1
                    Dim columnCount As Integer = 0
                    Dim myTableRow As String() = New String(dt.Columns.Count - 1) {}

                    For Each dc As DataColumn In dt.Columns
                        myTableRow(columnCount) = dt.Rows(n)(dc.ToString())
                        columnCount += 1
                    Next
                    myTable.Add(myTableRow)
                Next

                Dim table2DArray = myTable.ToArray()
                content.Message = "Success"
                content.Contents = table2DArray
            Else
                content.Message = "Data Not Found!"
                content.Contents = ""
            End If
        Catch ex As Exception
            content.Message = "Error"
            content.Contents = ex.Message
        End Try
        Return content
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function LoadDetail(User As String, ProcessCode As String, LineCode As String, LineGroup As String, Periode As String, Qty As String) As clsContent
        Dim content As New clsContent
        Try
            Dim dt As New DataTable
            Dim myTable As List(Of String()) = New List(Of String())()
            Dim data As New clsReportYearlyByType
            data.UserID = User
            data.ProcessCode = ProcessCode
            data.LineCode = LineCode
            data.LineGroup = LineGroup
            data.Periode = Periode
            data.QtyFTA = Qty

            dt = clsReportYearlyByType.LoadDetail(data)

            For n = 0 To dt.Rows.Count - 1
                Dim columnCount As Integer = 0
                Dim myTableRow As String() = New String(dt.Columns.Count - 1) {}

                For Each dc As DataColumn In dt.Columns
                    myTableRow(columnCount) = dt.Rows(n)(dc.ToString())
                    columnCount += 1
                Next
                myTable.Add(myTableRow)
            Next

            Dim table2DArray = myTable.ToArray()

            content.Message = "Success"
            content.Contents = table2DArray
        Catch ex As Exception
            content.Message = "Error"
            content.Contents = ex.Message
        End Try
        Return content
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function ChartLineGroup(User As String, FactoryCode As String, ProcessGroup As String, LineGroup As String, ProcessCode As String, LineCode As String, ItemType As String, ProdDate_From As String, ProdDate_To As String) As clsContent
        Dim content As New clsContent
        Try
            Dim dt As New DataTable
            Dim resp As New List(Of clsReportYearlyByType)
            Dim myTable As List(Of String()) = New List(Of String())()
            Dim data As New clsReportYearlyByType
            data.UserID = User
            data.FactoryCode = FactoryCode
            data.ProcessGroup = ProcessGroup
            data.LineGroup = LineGroup
            data.ProcessCode = ProcessCode
            data.LineCode = LineCode
            data.ItemType = ItemType
            data.ProdDateFrom = ProdDate_From
            data.ProdDateTo = ProdDate_To

            dt = clsReportYearlyByType.ChartLineGroup(data)
            For n = 0 To dt.Rows.Count
                Dim columnCount As Integer = 0
                Dim myTableRow As String() = New String(dt.Columns.Count - 1) {}

                If n = 0 Then
                    For Each dc As DataColumn In dt.Columns
                        myTableRow(columnCount) = dc.ToString()
                        columnCount += 1
                    Next
                Else
                    For Each dc As DataColumn In dt.Columns
                        myTableRow(columnCount) = dt.Rows(n - 1)(dc.ToString())
                        columnCount += 1
                    Next
                End If

                myTable.Add(myTableRow)
            Next
            Dim table2DArray = myTable.ToArray()

            content.Message = "Success"
            content.Contents = table2DArray

        Catch ex As Exception
            content.Message = "Error"
            content.Contents = ex.Message
        End Try
        Return content
    End Function

    <WebMethod()>
    Public Shared Function ChartLineDetail(User As String, ProcessCode As String, LineCode As String, LineGroup As String, Periode As String, Qty As String) As clsReportYearlyByType_ChartLineSetting
        Dim data As New clsReportYearlyByType
        data.UserID = User
        data.ProcessCode = ProcessCode
        data.LineCode = LineCode
        data.LineGroup = LineGroup
        data.Periode = Periode
        data.QtyFTA = Qty
        Dim dt As List(Of clsReportYearlyByType_ChartLineDetail) = clsReportYearlyByType.ChartLineDetail(data)
        Dim labels As Object() = dt.Select(Function(t) t.LABELNAME).Distinct.ToArray()
        Dim message As String = "["
        Dim axis As String = "["
        Dim max As Integer

        For i As Integer = 0 To dt.Count - 1
            max = max + dt(i).DATAVAL
        Next

        For i As Integer = 0 To labels.Length - 1
            message += "{'color':'#525454','data':["
            Dim d = dt.Where(Function(x) x.LABELNAME = labels(i).ToString()).Select(Function(x) New With {Key .axisValue = x.AXISVAL, Key .dataValue = x.DATAVAL, Key .axisName = x.AXISNAME})

            For Each item In d
                message += "[" & item.axisValue & "," + item.dataValue & "],"

                If i = 0 Then
                    axis += "[" & item.axisValue & ",'" + item.axisValue & "'],"
                End If
            Next

            message = message.Substring(0, message.Length - 1) & "], 'bars': {'show': true,  'clickable': true, 'align': 'center', 'barWidth' : 1, 'fillColor': '#fdbff9' , 'lineWidth':1 }}, { 'color':'#525454','data': [[0.5,0],"
            Dim e = dt.Where(Function(x) x.LABELNAME = labels(i).ToString()).Select(Function(x) New With {Key .axisValue = x.AXISVAL2, Key .dataValue = x.DATAVAL2, Key .axisName = x.AXISNAME})

            For Each item In e
                message += "[" & item.axisValue & "," + item.dataValue & "],"
            Next

            message = message.Substring(0, message.Length - 1) & "],  'yaxis': 2,  'points': { 'symbol': 'circle', 'fillColor': '#5fa9f5','radius' : 1, 'show': true }, 'lines': {'show':true,'lineWidth':0.5}},"

        Next

        message = message.Substring(0, message.Length - 1)
        message += "]"
        axis = axis.Substring(0, axis.Length - 1)
        axis += "]"


        Dim chart As clsReportYearlyByType_ChartLineSetting = New clsReportYearlyByType_ChartLineSetting()
        chart.data = message
        chart.xaxisTicks = axis
        chart.Max = max.ToString

        Return chart
    End Function

    Private Sub ExcelContent(ChartGroup As String, ChartDetail As String)
        Try

            Using excel As New ExcelPackage

                Dim ws As ExcelWorksheet
                ws = excel.Workbook.Worksheets.Add("C040 - FTA Report Yearly By Type")

                Dim data As New clsReportYearlyByType
                data.UserID = Session("user")
                data.FactoryCode = cboFactory.Value
                data.ProcessGroup = cboProcessGroup.Value
                data.LineGroup = cboLineGroup.Value
                data.ProcessCode = cboProcessCode.Value
                data.LineCode = cboLineCode.Value
                data.ItemType = cboItemType.Value
                data.ProdDateFrom = Date.Parse(dtFromDate.Text).ToString("yyyy-MM-dd")
                data.ProdDateTo = Date.Parse(dtToDate.Text).ToString("yyyy-MM-dd")

                Dim period = DateDiff(DateInterval.Month, Date.Parse(dtFromDate.Text), Date.Parse(dtToDate.Text))

                Dim dt As New DataTable
                dt = clsReportYearlyByType.LoadData(data)

                With ws
                    Dim irow = 1

                    .Cells(irow, 1).Value = "FTA Report Yearly By Type"
                    .Cells(irow, 1).Style.Font.Size = 14
                    .Cells(irow, 1).Style.Font.Name = "Segoe UI"
                    .Cells(irow, 1).Style.Font.Bold = True
                    irow = irow + 2

                    .Cells(irow, 1).Value = "Table Resume FTA SPC"
                    .Cells(irow, 1).Style.Font.Size = 12
                    .Cells(irow, 1).Style.Font.Name = "Segoe UI"
                    .Cells(irow, 1).Style.Font.Bold = True
                    irow = irow + 1

                    Dim bytesGroup() As Byte
                    bytesGroup = System.Convert.FromBase64String(ChartGroup)
                    Dim msGroup As New MemoryStream(bytesGroup)
                    Dim imageGroup As Image
                    imageGroup = Image.FromStream(msGroup)
                    Dim excelpictureGroup As ExcelPicture
                    excelpictureGroup = ws.Drawings.AddPicture("SymbolGroup", imageGroup)
                    excelpictureGroup.SetSize(700, 400)
                    'excelpictureGroup.SetPosition(380, 0)
                    excelpictureGroup.SetPosition(irow, 0, 0, 0)
                    irow = irow + 23

                    Dim irowStart = irow
                    .Cells(irow, 1).Value = "No"
                    .Cells(irow, 2).Value = "Item FTA by Line Group"

                    Dim nCol = 3
                    For i = 0 To period
                        Dim sDate = DateAdd(DateInterval.Month, i, Date.Parse(dtFromDate.Text))
                        .Cells(irow, nCol).Value = sDate.ToString("MMM-yy")
                        nCol = nCol + 1
                    Next

                    Dim Hdr As ExcelRange = .Cells(irowStart, 1, irowStart, nCol - 1)
                    Hdr.Style.HorizontalAlignment = HorzAlignment.Far
                    Hdr.Style.VerticalAlignment = VertAlignment.Center
                    Hdr.Style.Font.Size = 10
                    Hdr.Style.Font.Name = "Segoe UI"
                    Hdr.Style.Font.Color.SetColor(Color.White)
                    Hdr.Style.Fill.PatternType = ExcelFillStyle.Solid
                    Hdr.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)

                    irow = irow + 1
                    irowStart = irow

                    For i = 0 To dt.Rows.Count - 1
                        Try
                            Dim n = 1
                            For Each dc As DataColumn In dt.Columns
                                Try
                                    If n = 1 Then
                                        .Cells(irow, n).Value = dt.Rows(i)(dc.ToString())
                                        .Cells(irow, n).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                                        .Column(1).Width = 5
                                    ElseIf n = 2 Then
                                        .Cells(irow, n).Value = dt.Rows(i)(dc.ToString())
                                        .Cells(irow, n).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left
                                        .Column(2).Width = 20
                                    Else
                                        .Cells(irow, n).Value = dt.Rows(i)(dc.ToString()).ToString.Split("|")(4)
                                        .Cells(irow, n).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                                        .Column(n).Width = 10
                                    End If

                                    n = n + 1
                                Catch ex As Exception
                                    Throw New Exception(ex.Message)
                                End Try
                            Next
                            irow = irow + 1
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    Next

                    Dim Dtl As ExcelRange = .Cells(irowStart, 1, irow - 1, nCol - 1)
                    Dtl.Style.Font.Size = 10
                    Dtl.Style.Font.Name = "Segoe UI"
                    Dtl.Style.WrapText = True
                    Dtl.Style.VerticalAlignment = ExcelVerticalAlignment.Center


                    Dim Border As ExcelRange = .Cells(irowStart, 1, irow - 1, nCol - 1)
                    Border.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    Border.Style.Border.Left.Style = ExcelBorderStyle.Thin


                    irow = irow + 4
                    If HideValue.Get("sProcessCode") IsNot Nothing Then



                        Dim dtDet As New DataTable
                        Dim det As New clsReportYearlyByType
                        det.UserID = Session("user")
                        det.ProcessCode = HideValue.Get("sProcessCode")
                        det.LineCode = HideValue.Get("sLineCode")
                        det.LineGroup = HideValue.Get("sLineGroup")
                        det.Periode = HideValue.Get("sPeriode")
                        det.QtyFTA = HideValue.Get("sQty")

                        dtDet = clsReportYearlyByType.LoadDetail(det)
                        .Cells(irow, 1).Value = "Table Summary FTA SPC " & det.Periode
                        .Cells(irow, 1).Style.Font.Size = 12
                        .Cells(irow, 1).Style.Font.Name = "Segoe UI"
                        .Cells(irow, 1).Style.Font.Bold = True
                        irow = irow + 1

                        Dim bytesDetail() As Byte
                        bytesDetail = System.Convert.FromBase64String(ChartDetail)
                        Dim msDetail As New MemoryStream(bytesDetail)
                        Dim imageDetail As Image
                        imageDetail = Image.FromStream(msDetail)
                        Dim excelpictureDetail As ExcelPicture
                        excelpictureDetail = ws.Drawings.AddPicture("SymbolDetail", imageDetail)
                        excelpictureDetail.SetSize(700, 400)
                        excelpictureDetail.SetPosition(irow, 0, 0, 0)
                        irow = irow + 23

                        irowStart = irow
                        .Cells(irow, 1).Value = "No"
                        .Cells(irow, 2).Value = "Item FTA by Line"
                        .Cells(irow, 3).Value = "QtyTotal"
                        .Cells(irow, 4).Value = "Percentage"

                        Hdr = .Cells(irowStart, 1, irowStart, 4)
                        Hdr.Style.HorizontalAlignment = HorzAlignment.Far
                        Hdr.Style.VerticalAlignment = VertAlignment.Center
                        Hdr.Style.Font.Size = 10
                        Hdr.Style.Font.Name = "Segoe UI"
                        Hdr.Style.Font.Color.SetColor(Color.White)
                        Hdr.Style.Fill.PatternType = ExcelFillStyle.Solid
                        Hdr.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)

                        irow = irow + 1
                        irowStart = irow

                        For i = 0 To dtDet.Rows.Count - 1
                            Try
                                Dim n = 1
                                For Each dc As DataColumn In dtDet.Columns
                                    Try

                                        If n = 1 Then
                                            .Column(1).Width = 5
                                            .Cells(irow, n).Value = dtDet.Rows(i)(dc.ToString())
                                            .Cells(irow, n).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                                        ElseIf n = 2 Then
                                            .Column(2).Width = 20
                                            .Cells(irow, n).Value = dtDet.Rows(i)(dc.ToString())
                                            .Cells(irow, n).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left
                                        ElseIf n = 3 Then
                                            .Cells(irow, n).Value = dtDet.Rows(i)(dc.ToString())
                                            .Cells(irow, n).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                                        ElseIf n = 4 Then
                                            .Column(n).Width = 15
                                            .Cells(irow, n).Value = dtDet.Rows(i)(dc.ToString()) & "%"
                                            .Cells(irow, n).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                                        End If
                                        n = n + 1
                                    Catch ex As Exception
                                        Throw New Exception(ex.Message)
                                    End Try
                                Next
                                irow = irow + 1
                            Catch ex As Exception
                                Throw New Exception(ex.Message)
                            End Try
                        Next

                        Dtl = .Cells(irowStart, 1, irow - 1, 4)
                        Dtl.Style.VerticalAlignment = VertAlignment.Center
                        Dtl.Style.Font.Size = 10
                        Dtl.Style.Font.Name = "Segoe UI"
                        Dtl.Style.WrapText = True
                        Dtl.Style.VerticalAlignment = ExcelVerticalAlignment.Center

                        Border = .Cells(irowStart, 1, irow - 1, 4)
                        Border.Style.Border.Top.Style = ExcelBorderStyle.Thin
                        Border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                        Border.Style.Border.Right.Style = ExcelBorderStyle.Thin
                        Border.Style.Border.Left.Style = ExcelBorderStyle.Thin

                    End If
                End With

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment; filename=Report Yearly By Type_" & Format(Date.Now, "yyyy-MM-dd_HHmmss") & ".xlsx")
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

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Dim ChartGroup = HideValue.Get("ChartGroup")
        Dim ChartDetail = HideValue.Get("ChartDetail")
        ExcelContent(ChartGroup, ChartDetail)
    End Sub
End Class