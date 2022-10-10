Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports DevExpress.XtraCharts
Imports DevExpress.XtraCharts.Web
Imports System.Drawing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports DevExpress.XtraCharts.Native
Imports OfficeOpenXml
Imports System.IO
Imports DevExpress.Utils
Imports OfficeOpenXml.Style

Public Class SampleControlQuality
    Inherits System.Web.UI.Page
    Dim pUser As String = ""
    Public AuthApprove As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public ValueType As String
    Dim GlobalPrm As String = ""
    Dim LastRow As Integer

    Private Class clsHeader
        Public Property FactoryCode As String
        Public Property FactoryName As String
        Public Property ItemTypeCode As String
        Public Property ItemTypeName As String
        Public Property LineCode As String
        Public Property LineName As String
        Public Property ItemCheckCode As String
        Public Property ItemCheckName As String

        Public Property ProdDate As String
        Public Property ProdDate2 As String
        Public Property ShiftCode As String
        Public Property Shiftname As String
        Public Property Seq As Integer
        Public Property VerifiedOnly As String
    End Class

    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        gridX.JSProperties("cp_message") = ErrMsg
        gridX.JSProperties("cp_type") = msgType
        gridX.JSProperties("cp_val") = pVal
    End Sub

    Private Sub ExcelHeader(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer)
        With Exl
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#878787"))
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Font.Color.SetColor(Color.White)
        End With
    End Sub

    Private Sub ExcelBorder(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer)
        With Exl
            Dim Range As ExcelRange = .Cells(StartRow, StartCol, EndRow, EndCol)
            Range.Style.Border.Top.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Right.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Left.Style = ExcelBorderStyle.Thin
            Range.Style.Font.Size = 10
            Range.Style.Font.Name = "Segoe UI"
            Range.Style.HorizontalAlignment = HorzAlignment.Center
        End With
    End Sub

    Private Sub ExcelBorder(Exl As ExcelWorksheet, Range As ExcelRange)
        With Exl
            Range.Style.Border.Top.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Right.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Left.Style = ExcelBorderStyle.Thin
            Range.Style.Font.Size = 10
            Range.Style.Font.Name = "Segoe UI"
            Range.Style.HorizontalAlignment = HorzAlignment.Center
        End With
    End Sub

    Private Sub DownloadExcel()
        Dim ps As New PrintingSystem()
        LoadChartX(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), Format(dtTo.Value, "yyyy-MM-dd"))
        Dim linkX As New PrintableComponentLink(ps)
        linkX.Component = (CType(chartX, IChartContainer)).Chart

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {linkX})
        compositeLink.CreateDocument()
        Dim Path As String = Server.MapPath("Download")
        Dim streamImg As New MemoryStream
        compositeLink.ExportToImage(streamImg)

        Using Pck As New ExcelPackage
            Dim ws As ExcelWorksheet = Pck.Workbook.Worksheets.Add("Sheet1")
            With ws
                Dim iDay As Integer = 2
                Dim iCol As Integer = 2
                Dim lastCol As Integer = 1
                Dim Seq As Integer = 0
                Dim SelDay As Date = dtDate.Value

                Dim Hdr As New clsHeader
                Hdr.FactoryCode = cboFactory.Value
                Hdr.FactoryName = cboFactory.Text
                Hdr.ItemTypeCode = cboType.Value
                Hdr.ItemTypeName = cboType.Text
                Hdr.LineCode = cboLine.Value
                Hdr.LineName = cboLine.Text
                Hdr.ItemCheckCode = cboItemCheck.Value
                Hdr.ItemCheckName = cboItemCheck.Value
                Hdr.ProdDate = Convert.ToDateTime(dtDate.Value).ToString("yyyy-MM-dd")
                Hdr.ProdDate2 = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd")

                GridTitle(ws, Hdr)
                GridExcel(ws, Hdr)
                .InsertRow(LastRow, 22)
                Dim fi As New FileInfo(Path & "\chart.png")
                Dim Picture As OfficeOpenXml.Drawing.ExcelPicture
                Picture = .Drawings.AddPicture("chart", Image.FromStream(streamImg))
                Picture.SetPosition(LastRow, 0, 0, 0)
            End With

            Dim stream As MemoryStream = New MemoryStream(Pck.GetAsByteArray())
            Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            Response.AppendHeader("Content-Disposition", "attachment; filename=SampleControlQuality_" & Format(Date.Now, "yyyy-MM-dd") & ".xlsx")
            Response.BinaryWrite(stream.ToArray())
            Response.End()

        End Using
    End Sub

    Private Sub GridExcel(pExl As ExcelWorksheet, Hdr As clsHeader)
        Dim dt As DataTable
        Dim iRow As Integer = 12
        Dim iCol As Integer
        Dim StartRow As Integer = iRow
        Dim EndRow As Integer
        Dim EndCol As Integer
        Dim StartCol1 As Integer, EndCol1 As Integer
        Dim StartCol2 As Integer, EndCol2 As Integer

        With pExl
            Dim ds As DataSet = clsSPCResultDetailDB.GetSampleByPeriod(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate, Hdr.ProdDate2, Hdr.VerifiedOnly)
            Dim dtDay As DataTable = ds.Tables(0)
            StartRow = iRow
            .Cells(iRow, 1).Value = "Date"
            .Cells(iRow + 1, 1).Value = "Shift"
            .Cells(iRow + 2, 1).Value = "Time"

            For iDay = 0 To dtDay.Rows.Count - 1
                Dim SelDay As Date = dtDay.Rows(iDay)("ProdDate")
                Dim dDay As String = Format(SelDay, "yyyy-MM-dd")

                iCol = iDay + 2
                .Cells(iRow, iCol).Value = Format(SelDay, "dd MMM yyyy")
                .Cells(iRow + 1, iCol).Value = dtDay.Rows(iDay)("ShiftCode")
                .Cells(iRow + 2, iCol).Value = dtDay.Rows(iDay)("SeqNo")
            Next
            iRow = iRow + 3
            dt = ds.Tables(1)
            For j = 0 To dt.Rows.Count - 1
                iCol = 1
                If dt.Rows(j)(1) = "-" Or dt.Rows(j)(1) = "--" Then
                    .Row(iRow).Height = 2
                End If
                For k = 1 To dt.Columns.Count - 1
                    .Cells(iRow, iCol).Value = dt.Rows(j)(k)
                    If k > 1 Then
                        .Cells(iRow, iCol).Style.Numberformat.Format = "0.0000"
                    End If
                    iCol = iCol + 1
                Next
                iRow = iRow + 1
            Next
            EndCol = dt.Columns.Count - 1
            EndRow = iRow - 1

            ExcelHeader(pExl, StartRow, 1, StartRow + 2, EndCol)
            ExcelBorder(pExl, StartRow, 1, EndRow, EndCol)
            LastRow = iRow + 4
        End With
    End Sub

    Private Sub GridTitle(ByVal pExl As ExcelWorksheet, cls As clsHeader)
        With pExl
            Try
                .Cells(1, 1).Value = "Sample Control Quality by Period"
                .Cells(1, 1, 1, 13).Merge = True
                .Cells(1, 1, 1, 13).Style.HorizontalAlignment = HorzAlignment.Near
                .Cells(1, 1, 1, 13).Style.VerticalAlignment = VertAlignment.Center
                .Cells(1, 1, 1, 13).Style.Font.Bold = True
                .Cells(1, 1, 1, 13).Style.Font.Size = 16
                .Cells(1, 1, 1, 13).Style.Font.Name = "Segoe UI"

                .Cells(3, 1, 3, 2).Value = "Factory Code"
                .Cells(3, 1, 3, 2).Merge = True
                .Cells(3, 3).Value = ": " & cls.FactoryName

                .Cells(4, 1, 4, 2).Value = "Item Type Code"
                .Cells(4, 1, 4, 2).Merge = True
                .Cells(4, 3).Value = ": " & cls.ItemTypeName

                .Cells(5, 1, 5, 2).Value = "Line Code"
                .Cells(5, 1, 5, 2).Merge = True
                .Cells(5, 3).Value = ": " & cls.LineName

                .Cells(6, 1, 6, 2).Value = "Item Check Code"
                .Cells(6, 1, 6, 2).Merge = True
                .Cells(6, 3).Value = ": " & cls.ItemCheckName

                .Cells(7, 1, 7, 2).Value = "Prod Date"
                .Cells(7, 1, 7, 2).Merge = True
                .Cells(7, 3).Value = ": " & cls.ProdDate

                .Cells(8, 1, 8, 2).Value = "Shift Code"
                .Cells(8, 1, 8, 2).Merge = True
                .Cells(8, 3).Value = ": " & cls.Shiftname

                .Cells(9, 1, 9, 2).Value = "Sequence No"
                .Cells(9, 1, 9, 2).Merge = True
                .Cells(9, 3).Value = ": " & cls.Seq

                Dim rgHdr As ExcelRange = .Cells(3, 3, 9, 4)
                rgHdr.Style.HorizontalAlignment = HorzAlignment.Near
                rgHdr.Style.VerticalAlignment = VertAlignment.Center
                rgHdr.Style.Font.Size = 10
                rgHdr.Style.Font.Name = "Segoe UI"
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GlobalPrm = Request.QueryString("FactoryCode") & ""
        sGlobal.getMenu("B060")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user") & ""
        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "B060")
        gridX.SettingsDataSecurity.AllowInsert = AuthUpdate
        gridX.SettingsDataSecurity.AllowEdit = AuthUpdate
        show_error(MsgTypeEnum.Info, "", 0)
        If Not IsPostBack And Not IsCallback Then
            up_FillCombo()
            If GlobalPrm <> "" Then
                dtDate.Value = CDate(Request.QueryString("ProdDate"))
                Dim FactoryCode As String = Request.QueryString("FactoryCode")
                Dim ItemTypeCode As String = Request.QueryString("ItemTypeCode")
                Dim Line As String = Request.QueryString("Line")
                Dim ItemCheckCode As String = Request.QueryString("ItemCheckCode")
                Dim ProdDate As String = Request.QueryString("ProdDate")
                Dim ProdDate2 As String = Request.QueryString("ProdDate2")
                Dim Shift As String = Request.QueryString("Shift")
                Dim Sequence As String = Request.QueryString("Sequence")

                InitCombo(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence, ProdDate2)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "GridLoad();", True)
            Else
                dtDate.Value = Now.Date
                pUser = Session("user") & ""
                dtDate.Value = Now.Date
                dtTo.Value = Now.Date
                If pUser <> "" Then
                    Dim User As clsUserSetup = clsUserSetupDB.GetData(pUser)
                    If User IsNot Nothing Then
                        cboFactory.Value = User.FactoryCode
                        cboType.DataSource = clsItemTypeDB.GetList(cboFactory.Value, pUser)
                        cboType.DataBind()
                    End If
                End If
                'InitCombo("F001", "TPMSBR011", "015", "IC021", "2022-08-03", "SH001", 1, "2022-09-01")
            End If
        End If
    End Sub

    Private Sub InitCombo(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ShiftCode As String, Sequence As String, ProdDate2 As String)
        pUser = Session("user") & ""
        dtDate.Value = CDate(ProdDate)
        dtTo.Value = CDate(ProdDate2)
        cboFactory.Value = FactoryCode

        cboType.DataSource = clsItemTypeDB.GetList(cboFactory.Value, pUser)
        cboType.DataBind()
        cboType.Value = ItemTypeCode

        cboLine.DataSource = ClsLineDB.GetList("admin", cboFactory.Value, cboType.Value)
        cboLine.DataBind()
        cboLine.Value = Line

        cboItemCheck.DataSource = clsItemCheckDB.GetList(cboFactory.Value, cboType.Value, cboLine.Value)
        cboItemCheck.DataBind()
        cboItemCheck.Value = ItemCheckCode
    End Sub

    Private Sub up_FillCombo()
        cboFactory.DataSource = clsFactoryDB.GetList
        cboFactory.DataBind()
    End Sub

    Dim dtXR As DataTable
    Dim dtLSL As DataTable
    Dim dtUSL As DataTable
    Dim dtLCL As DataTable
    Dim dtUCL As DataTable
    Dim dtChart As DataTable

    Private Sub GridXLoad(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String, VerifiedOnly As Integer)
        With gridX
            .Columns.Clear()
            Dim Band1 As New GridViewBandColumn
            Band1.Caption = "DATE"
            Band1.HeaderStyle.Height = 90
            .Columns.Add(Band1)

            Dim Band2 As New GridViewBandColumn
            Band2.Caption = "SHIFT"
            Band1.HeaderStyle.Height = 40
            Band1.Columns.Add(Band2)

            Dim Col1 As New GridViewDataTextColumn
            Col1.FieldName = "Des"
            Col1.Caption = "TIME"
            Col1.Width = 90
            Col1.FixedStyle = GridViewColumnFixedStyle.Left
            Col1.CellStyle.HorizontalAlign = HorizontalAlign.Center
            Band2.Columns.Add(Col1)

            Dim ds As DataSet = clsSPCResultDetailDB.GetSampleByPeriod(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ProdDate2, VerifiedOnly)
            Dim dtDay As DataTable = ds.Tables(0)

            Dim PrevDay As String = ""
            Dim PrevShift As String = ""
            For iDay = 0 To dtDay.Rows.Count - 1
                Dim SelDay As Date = dtDay.Rows(iDay)("ProdDate")
                Dim dDay As String = Format(SelDay, "yyyy-MM-dd")

                Dim BandDay As GridViewBandColumn
                Dim BandShift As GridViewBandColumn

                If dDay <> PrevDay Then
                    BandDay = New GridViewBandColumn
                    BandDay.Caption = Format(SelDay, "dd MMM yyyy")
                    .Columns.Add(BandDay)

                End If

                Dim SelShift As String = dtDay.Rows(iDay)("ShiftCode")
                If SelShift <> PrevShift Or dDay <> PrevDay Then
                    BandShift = New GridViewBandColumn
                    BandShift.Caption = "S-" & SelShift
                    BandDay.Columns.Add(BandShift)
                End If

                Dim colTime As New GridViewDataTextColumn
                colTime.Caption = dtDay.Rows(iDay)("RegisterDate")
                colTime.FieldName = dtDay.Rows(iDay)("ColName")
                colTime.Width = 90
                colTime.CellStyle.HorizontalAlign = HorizontalAlign.Center
                BandShift.Columns.Add(colTime)

                PrevDay = dDay
                PrevShift = SelShift
            Next
            dtXR = ds.Tables(1)
            gridX.DataSource = dtXR
            gridX.DataBind()

            If ds.Tables.Count > 2 Then
                dtLSL = ds.Tables(2)
                dtUSL = ds.Tables(3)
                dtLCL = ds.Tables(4)
                dtUCL = ds.Tables(5)
                dtChart = ds.Tables(6)
            End If
        End With
    End Sub

    Private Sub gridX_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles gridX.CustomCallback
        Dim FactoryCode As String = Split(e.Parameters, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameters, "|")(1)
        Dim LineCode As String = Split(e.Parameters, "|")(2)
        Dim ItemCheckCode As String = Split(e.Parameters, "|")(3)
        Dim ProdDate As String = Split(e.Parameters, "|")(4)
        Dim ProdDate2 As String = Split(e.Parameters, "|")(5)
        Dim VerifiedOnly As Integer = Split(e.Parameters, "|")(6)
        GridXLoad(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ProdDate2, VerifiedOnly)
    End Sub

    Private Sub gridX_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles gridX.HtmlDataCellPrepared
        Dim LCL As Double
        Dim UCL As Double
        Dim LSL As Double
        Dim USL As Double

        Dim ColName As String = e.DataColumn.FieldName
        If Not IsDBNull(e.CellValue) AndAlso ColName <> "Seq" AndAlso ColName <> "Des" AndAlso (e.GetValue("Seq") = "1" Or e.GetValue("Seq") = "3" Or e.GetValue("Seq") = "4" Or e.GetValue("Seq") = "5") Then
            LSL = dtLSL.Rows(0)(ColName)
            USL = dtUSL.Rows(0)(ColName)
            LCL = dtLCL.Rows(0)(ColName)
            UCL = dtUCL.Rows(0)(ColName)
            Dim Value As Double = clsSPCResultDB.ADecimal(e.CellValue)
            If Value < LSL Or Value > USL Then
                e.Cell.BackColor = Color.Red
            ElseIf Value < LCL Or Value > UCL Then
                e.Cell.BackColor = Color.Pink
            End If
        End If
        Dim cs As New clsSPCColor
        If e.DataColumn.FieldName = "Des" Then
            If e.CellValue = "1" Then
                e.Cell.BackColor = cs.Color1
            ElseIf e.CellValue = "2" Then
                e.Cell.BackColor = cs.Color2
            ElseIf e.CellValue = "3" Then
                e.Cell.BackColor = cs.Color3
            ElseIf e.CellValue = "4" Then
                e.Cell.BackColor = cs.Color4
            ElseIf e.CellValue = "5" Then
                e.Cell.BackColor = cs.Color5
            End If
        End If
        If e.KeyValue = "-" Or e.KeyValue = "--" Then
            e.Cell.Text = ""
        End If
    End Sub

    Private Sub gridX_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles gridX.HtmlRowPrepared
        If e.KeyValue = "-" Or e.KeyValue = "--" Then
            e.Row.BackColor = System.Drawing.Color.FromArgb(112, 112, 112)
        End If
    End Sub

    Private Sub chartX_CustomDrawSeries(sender As Object, e As CustomDrawSeriesEventArgs) Handles chartX.CustomDrawSeries
        Dim cs As New clsSPCColor
        Dim s As String = e.Series.Name
        If s = "#1" Then
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Circle
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor1
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color1
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color1
        ElseIf s = "#2" Then
            'CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Diamond
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor2
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color2
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color2
        ElseIf s = "#3" Then
            'CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Triangle
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor3
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color3
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color3
        ElseIf s = "#4" Then
            'CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Square
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor4
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color4
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color4
        ElseIf s = "#5" Then
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Circle
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor5
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color5
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color5
        End If
    End Sub

    Private Sub LoadHistogram(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String)
        With Histogram
            Dim ht As List(Of clsHistogram) = clsXRChartDB.GetHistogram(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, ProdDate2)
            .DataSource = ht
            .DataBind()
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            If ht.Count > 0 Then
                diagram.AxisX.WholeRange.MaxValue = ht(0).MaxValue + 1
                diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones
            End If

        End With
    End Sub

    Private Sub LoadChartX(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartXRMonthly(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, ProdDate2)
        With chartX
            .DataSource = xr
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            diagram.AxisX.WholeRange.MinValue = 0
            diagram.AxisX.WholeRange.MaxValue = 12

            diagram.AxisX.GridLines.LineStyle.DashStyle = DashStyle.Solid
            diagram.AxisX.GridLines.MinorVisible = True
            diagram.AxisX.MinorCount = 1
            diagram.AxisX.GridLines.Visible = False

            diagram.AxisY.NumericScaleOptions.CustomGridAlignment = 0.005
            diagram.AxisY.GridLines.MinorVisible = False

            Dim ChartType As String = clsXRChartDB.GetChartType(FactoryCode, ItemTypeCode, Line, ItemCheckCode)
            If ChartType = "1" Or ChartType = "2" Then
                .Titles(0).Text = "X Bar Control Chart"
            Else
                .Titles(0).Text = "Graph Monitoring"
            End If

            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)
            diagram.AxisY.ConstantLines.Clear()
            If Setup IsNot Nothing Then
                Dim LCL As New ConstantLine("LCL")
                LCL.Color = System.Drawing.Color.Purple
                LCL.LineStyle.Thickness = 2
                LCL.LineStyle.DashStyle = DashStyle.DashDot
                diagram.AxisY.ConstantLines.Add(LCL)
                LCL.AxisValue = Setup.XBarLCL

                Dim UCL As New ConstantLine("UCL")
                UCL.Color = System.Drawing.Color.Purple
                UCL.LineStyle.Thickness = 2
                UCL.LineStyle.DashStyle = DashStyle.DashDot
                diagram.AxisY.ConstantLines.Add(UCL)
                UCL.AxisValue = Setup.XBarUCL

                Dim CL As New ConstantLine("CL")
                CL.Color = System.Drawing.Color.Black
                CL.LineStyle.Thickness = 2
                CL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(CL)
                CL.AxisValue = Setup.XBarCL

                Dim LSL As New ConstantLine("LSL")
                LSL.Color = System.Drawing.Color.Red
                LSL.LineStyle.Thickness = 2
                LSL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(LSL)
                LSL.AxisValue = Setup.SpecLSL

                Dim USL As New ConstantLine("USL")
                USL.Color = System.Drawing.Color.Red
                USL.LineStyle.Thickness = 2
                USL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(USL)
                USL.AxisValue = Setup.SpecUSL

                Dim MinValue As Double, MaxValue As Double
                If xr.Count > 0 Then
                    MinValue = xr(0).MinValue
                    MaxValue = xr(0).MaxValue
                End If
                If Setup.SpecLSL < MinValue Then
                    MinValue = Setup.SpecLSL
                End If
                If Setup.SpecUSL > MaxValue Then
                    MaxValue = Setup.SpecUSL
                End If

                diagram.AxisY.WholeRange.MinValue = MinValue
                diagram.AxisY.WholeRange.MaxValue = MaxValue
                diagram.AxisY.WholeRange.EndSideMargin = 0.015

                diagram.AxisY.VisualRange.MinValue = MinValue
                diagram.AxisY.VisualRange.MaxValue = MaxValue
                diagram.AxisY.VisualRange.EndSideMargin = 0.015

                Dim diff As Double = MaxValue - MinValue
                Dim gridAlignment As Double = Math.Round(diff / 15, 3)
                diagram.AxisY.NumericScaleOptions.CustomGridAlignment = gridAlignment

                CType(.Diagram, XYDiagram).SecondaryAxesY.Clear()
                Dim myAxisY As New SecondaryAxisY("my Y-Axis")
                myAxisY.Visibility = DevExpress.Utils.DefaultBoolean.False
                CType(.Diagram, XYDiagram).SecondaryAxesY.Add(myAxisY)
                CType(.Series("Rule").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                CType(.Series("RuleYellow").View, XYDiagramSeriesViewBase).AxisY = myAxisY
            End If
            .DataBind()
            Dim ChartWidth As Integer = xr.Count * 12
            If ChartWidth < 400 Then
                ChartWidth = 400
            End If
            '.Width = 
        End With
    End Sub

    Private Sub chartX_CustomCallback(sender As Object, e As CustomCallbackEventArgs) Handles chartX.CustomCallback
        Dim Prm As String = e.Parameter
        If Prm = "" Then
            Prm = "F001|TPMSBR011|015|IC021|03 Aug 2022"
        End If
        Dim FactoryCode As String = Split(Prm, "|")(0)
        Dim ItemTypeCode As String = Split(Prm, "|")(1)
        Dim LineCode As String = Split(Prm, "|")(2)
        Dim ItemCheckCode As String = Split(Prm, "|")(3)
        Dim ProdDate As String = Split(Prm, "|")(4)
        Dim ProdDate2 As String = Split(Prm, "|")(5)
        LoadChartX(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ProdDate2)
    End Sub

    Private Sub chartX_BoundDataChanged(sender As Object, e As EventArgs) Handles chartX.BoundDataChanged
        'With chartX
        '    Dim view As FullStackedBarSeriesView = TryCast(.Series("Rule").View, FullStackedBarSeriesView)
        '    If view IsNot Nothing Then
        '        view.Color = Color.Red
        '        view.FillStyle.FillMode = FillMode.Solid
        '        view.Transparency = 200
        '        view.Border.Thickness = 1
        '    End If
        '    Dim view2 As FullStackedBarSeriesView = TryCast(.Series("RuleYellow").View, FullStackedBarSeriesView)
        '    If view2 IsNot Nothing Then
        '        view2.Color = Color.Yellow
        '        view2.FillStyle.FillMode = FillMode.Solid
        '        view2.Transparency = 200
        '        view2.Border.Thickness = 1
        '    End If
        'End With
    End Sub

    Private Sub cboType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboType.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim pUser As String = Session("user")
        cboType.DataSource = clsItemTypeDB.GetList(FactoryCode, pUser)
        cboType.DataBind()
    End Sub

    Private Sub cboLine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLine.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim UserID As String = Session("user") & ""
        cboLine.DataSource = ClsLineDB.GetList(UserID, FactoryCode, ItemTypeCode)
        cboLine.DataBind()
    End Sub

    Private Sub cboItemCheck_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemCheck.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim LineCode As String = Split(e.Parameter, "|")(2)
        cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, LineCode)
        cboItemCheck.DataBind()
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        DownloadExcel()
    End Sub

    Private Sub Histogram_CustomCallback(sender As Object, e As CustomCallbackEventArgs) Handles Histogram.CustomCallback
        Dim Prm As String = e.Parameter
        Dim FactoryCode As String = Split(Prm, "|")(0)
        Dim ItemTypeCode As String = Split(Prm, "|")(1)
        Dim LineCode As String = Split(Prm, "|")(2)
        Dim ItemCheckCode As String = Split(Prm, "|")(3)
        Dim ProdDate As String = Split(Prm, "|")(4)
        Dim ProdDate2 As String = Split(Prm, "|")(5)
        LoadHistogram(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ProdDate2)
    End Sub
End Class