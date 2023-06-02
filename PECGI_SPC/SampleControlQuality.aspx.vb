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
    Dim Digit As Integer

    Private Class clsHeader
        Public Property FactoryCode As String
        Public Property FactoryName As String
        Public Property ProcessGroup As String
        Public Property LineGroup As String
        Public Property ProcessCode As String
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

    Private Sub ExcelFont(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer, FontSize As Integer)
        With Exl
            Dim Range As ExcelRange = .Cells(StartRow, StartCol, EndRow, EndCol)
            Range.Style.Font.Size = FontSize
            Range.Style.Font.Name = "Segoe UI"
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
        Digit = ClsSPCItemCheckMasterDB.GetDigit(cboItemCheck.Value)
        Dim ps As New PrintingSystem()
        LoadChartX(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), Format(dtTo.Value, "yyyy-MM-dd"), cboShow.Value)
        Dim linkX As New PrintableComponentLink(ps)
        linkX.Component = (CType(chartX, IChartContainer)).Chart

        Dim linkR As New PrintableComponentLink(ps)
        ChartType = clsXRChartDB.GetChartType(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value)
        If ChartType = "1" Or ChartType = "2" Then
            LoadChartR(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), Format(dtTo.Value, "yyyy-MM-dd"), cboShow.Value)
            linkR.Component = (CType(chartR, IChartContainer)).Chart
        End If

        Dim linkH As New PrintableComponentLink(ps)
        LoadHistogram(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), Format(dtTo.Value, "yyyy-MM-dd"), cboShow.Value)
        linkH.Component = (CType(Histogram, IChartContainer)).Chart

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {linkX})
        compositeLink.CreateDocument()
        Dim Path As String = Server.MapPath("Download")
        Dim streamImg As New MemoryStream
        compositeLink.ExportToImage(streamImg)

        Dim compositeLink2 As New CompositeLink(ps)
        compositeLink2.Links.AddRange(New Object() {linkR})
        compositeLink2.CreateDocument()
        Dim streamImg2 As New MemoryStream
        compositeLink2.ExportToImage(streamImg2)

        Dim compositeLink3 As New CompositeLink(ps)
        compositeLink3.Links.AddRange(New Object() {linkH})
        compositeLink3.CreateDocument()
        Dim streamImg3 As New MemoryStream
        compositeLink3.ExportToImage(streamImg3)

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
                Hdr.ItemCheckName = cboItemCheck.Text
                Hdr.ProdDate = Convert.ToDateTime(dtDate.Value).ToString("yyyy-MM-dd")
                Hdr.ProdDate2 = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd")
                Hdr.VerifiedOnly = cboShow.Value
                Hdr.ProcessGroup = cboProcessGroup.Text
                Hdr.LineGroup = cboLineGroup.Text
                Hdr.ProcessCode = cboProcess.Text

                GridTitle(ws, Hdr)
                GridExcel(ws, Hdr)
                .InsertRow(LastRow, 22)
                Dim fi As New FileInfo(Path & "\chart.png")
                Dim Picture As OfficeOpenXml.Drawing.ExcelPicture
                Picture = .Drawings.AddPicture("chart", Image.FromStream(streamImg))
                Picture.SetPosition(LastRow, 0, 0, 0)

                Dim RowHistogram As Integer
                If ChartType = "1" Or ChartType = "2" Then
                    Dim fi2 As New FileInfo(Path & "\chartR.png")
                    Dim Picture2 As OfficeOpenXml.Drawing.ExcelPicture
                    Picture2 = .Drawings.AddPicture("chartR", Image.FromStream(streamImg2))
                    Picture2.SetPosition(LastRow + 22, 0, 0, 0)
                    RowHistogram = LastRow + 34
                Else
                    RowHistogram = LastRow + 22
                End If

                Dim fi3 As New FileInfo(Path & "\histogram.png")
                Dim Picture3 As OfficeOpenXml.Drawing.ExcelPicture
                Picture3 = .Drawings.AddPicture("histogram", Image.FromStream(streamImg3))
                Picture3.SetPosition(RowHistogram, 0, 0, 0)

                ExcelCPK(ws, RowHistogram + 1)
            End With

            Dim stream As MemoryStream = New MemoryStream(Pck.GetAsByteArray())
            Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            Response.AppendHeader("Content-Disposition", "attachment; filename=SampleControlQuality_" & Format(Date.Now, "yyyy-MM-dd") & ".xlsx")
            Response.BinaryWrite(stream.ToArray())
            Response.End()

        End Using
    End Sub


    Private Sub ExcelCPK(pExl As ExcelWorksheet, StartRow As Integer)
        With pExl
            Dim Row As Integer = StartRow
            Dim Col As Integer = 10
            .Cells(Row, Col).Value = "Factory"
            .Cells(Row + 1, Col).Value = "Type"
            .Cells(Row + 2, Col).Value = "Machine Process"
            .Cells(Row + 3, Col).Value = "Item Check"
            .Cells(Row + 4, Col).Value = "Prod Date"
            .Cells(Row + 5, Col).Value = "Unit Measurement"

            .Cells(Row + 6, Col).Value = "Specification USL"
            .Cells(Row + 7, Col).Value = "Specification LSL"
            .Cells(Row + 8, Col).Value = "Control Plan UCL"
            .Cells(Row + 9, Col).Value = "Control Plan CL"
            .Cells(Row + 10, Col).Value = "Control Plan LCL"

            .Cells(Row + 11, Col).Value = "Min"
            .Cells(Row + 12, Col).Value = "Max"
            Dim Noj As String = ""
            If ChartType = "1" Then
                Noj = " (No Judgement)"
            End If
            .Cells(Row + 13, Col).Value = "Xbarbar" & Noj
            .Cells(Row + 14, Col).Value = "Rbar" & Noj
            .Cells(Row, Col, Row + 14, Col + 2).Style.Fill.PatternType = ExcelFillStyle.Solid
            .Cells(Row, Col, Row + 14, Col + 2).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242))
            For iRow = Row To Row + 14
                .Cells(iRow, Col, iRow, Col + 2).Merge = True
            Next
            For irow = Row To Row + 7
                .Cells(irow, Col + 3, irow, Col + 6).Merge = True
            Next

            Col = 14
            .Cells(Row + 7, Col).Value = "d2"
            .Cells(Row + 8, Col).Value = "Xbar UCL" & Noj
            .Cells(Row + 9, Col).Value = "Xbar LCL" & Noj
            .Cells(Row + 10, Col).Value = "R UCL" & Noj

            .Cells(Row + 11, Col).Value = "Cp"
            .Cells(Row + 12, Col).Value = "Cpu"
            .Cells(Row + 13, Col).Value = "Cpl"
            .Cells(Row + 14, Col).Value = "Cpk"
            .Cells(Row + 7, Col, Row + 14, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
            .Cells(Row + 7, Col, Row + 14, Col).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242))
            For iRow = Row + 8 To Row + 14
                .Cells(iRow, 14, iRow, 15).Merge = True
            Next
            If dtCP.Rows.Count > 0 Then
                Col = 13
                .Cells(Row, Col).Value = dtCP.Rows(0)("FactoryName") & ""
                .Cells(Row + 1, Col).Value = dtCP.Rows(0)("ItemTypeName") & ""
                .Cells(Row + 2, Col).Value = dtCP.Rows(0)("LineName") & ""
                .Cells(Row + 3, Col).Value = dtCP.Rows(0)("ItemCheckName") & ""
                .Cells(Row + 4, Col).Value = Format(dtDate.Value, "dd MMM yyyy") & " - " & Format(dtTo.Value, "dd MMM yyyy")
                .Cells(Row + 5, Col).Value = dtCP.Rows(0)("Unit") & ""

                .Cells(Row + 6, Col).Value = dtCP.Rows(0)("USL")
                .Cells(Row + 7, Col).Value = dtCP.Rows(0)("LSL")
                .Cells(Row + 8, Col).Value = dtCP.Rows(0)("UCL")
                .Cells(Row + 9, Col).Value = dtCP.Rows(0)("CL")
                .Cells(Row + 10, Col).Value = dtCP.Rows(0)("LCL")

                .Cells(Row + 11, Col).Value = dtCP.Rows(0)("Min")
                .Cells(Row + 12, Col).Value = dtCP.Rows(0)("Max")
                .Cells(Row + 13, Col).Value = dtCP.Rows(0)("Xbarbar")
                .Cells(Row + 14, Col).Value = dtCP.Rows(0)("Rbar")
                .Cells(Row + 6, Col, Row + 14, Col).Style.Numberformat.Format = FormatDigit(Digit)

                If ChartType = "1" Then
                    If Not IsDBNull(dtCP.Rows(0)("Xbarbar")) And (dtCP.Rows(0)("Xbarbar") < dtCP.Rows(0)("XbarLCL") Or dtCP.Rows(0)("Xbarbar") > dtCP.Rows(0)("XbarUCL")) Then
                        .Cells(Row + 13, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                        .Cells(Row + 13, Col).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                    End If
                    If Not IsDBNull(dtCP.Rows(0)("Rbar")) And dtCP.Rows(0)("Rbar") > dtCP.Rows(0)("RUCL") Then
                        .Cells(Row + 14, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                        .Cells(Row + 14, Col).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                    End If
                End If
                Col = 16
                .Cells(Row + 7, Col).Value = dtCP.Rows(0)("D2")
                .Cells(Row + 8, Col).Value = dtCP.Rows(0)("XbarUCL")
                .Cells(Row + 9, Col).Value = dtCP.Rows(0)("XbarLCL")
                .Cells(Row + 10, Col).Value = dtCP.Rows(0)("RUCL")

                .Cells(Row + 11, Col).Value = dtCP.Rows(0)("CP")
                .Cells(Row + 12, Col).Value = dtCP.Rows(0)("CPK1")
                .Cells(Row + 13, Col).Value = dtCP.Rows(0)("CPK2")
                .Cells(Row + 14, Col).Value = dtCP.Rows(0)("CPKMin")
                .Cells(Row + 6, Col, Row + 14, Col).Style.Numberformat.Format = FormatDigit(Digit)
            End If

            Dim rg As ExcelRange = .Cells(Row, 10, Row + 14, 16)
            ExcelBorder(pExl, rg)
            rg.Style.HorizontalAlignment = HorzAlignment.Near
        End With
    End Sub

    Private Sub GridExcel(pExl As ExcelWorksheet, Hdr As clsHeader)
        Dim dt As DataTable
        Dim iRow As Integer = 7
        Dim iCol As Integer
        Dim StartRow As Integer = iRow
        Dim EndRow As Integer
        Dim EndCol As Integer
        Dim LCL As Double
        Dim UCL As Double
        Dim LSL As Double
        Dim USL As Double
        Dim RUCL As Double
        Dim RLCL As Double
        Dim LightYellow As Color = Color.FromArgb(255, 255, 153)
        Dim PrevDate As String = ""
        Dim PrevShift As String = ""
        Dim ShiftCode As String
        Dim StartCol1 As Integer, EndCol1 As Integer
        Dim StartCol2 As Integer, EndCol2 As Integer
        Dim cs As New clsSPCColor

        With pExl
            Dim ds As DataSet = clsSPCResultDetailDB.GetSampleByPeriod(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate, Hdr.ProdDate2, Hdr.VerifiedOnly, True, True)

            Dim dt2 As DataTable = clsSPCResultDetailDB.GetLastR(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate, 1, Hdr.VerifiedOnly)
            If dt2.Rows.Count > 0 Then
                LastNG = dt2.Rows(0)("NG")
            Else
                LastNG = 0
            End If

            Dim dtDay As DataTable = ds.Tables(0)
            dtLSL = ds.Tables(2)
            dtUSL = ds.Tables(3)
            dtLCL = ds.Tables(4)
            dtUCL = ds.Tables(5)
            dtCP = ds.Tables(6)
            dtRUCL = ds.Tables(7)
            dtRLCL = ds.Tables(8)
            dtDel = ds.Tables(9)

            StartRow = iRow
            .Cells(iRow, 1).Value = "Date"
            .Cells(iRow + 1, 1).Value = "Shift"
            .Cells(iRow + 2, 1).Value = "Time"

            For iDay = 0 To dtDay.Rows.Count - 1
                Dim SelDay As Date = dtDay.Rows(iDay)("ProdDate")
                Dim dDay As String = Format(SelDay, "yyyy-MM-dd")

                iCol = iDay + 2
                .Cells(iRow, iCol).Value = Format(SelDay, "dd MMM yy")
                ShiftCode = dtDay.Rows(iDay)("ShiftCode2")
                .Cells(iRow + 1, iCol).Value = ShiftCode
                .Cells(iRow + 2, iCol).Value = dtDay.Rows(iDay)("RegisterDate")

                If PrevDate <> dDay Then
                    StartCol1 = iCol
                    StartCol2 = iCol
                ElseIf PrevShift <> ShiftCode Then
                    StartCol2 = iCol
                End If
                EndCol1 = iCol
                EndCol2 = iCol
                If EndCol1 > StartCol1 Then
                    .Cells(iRow, StartCol1, iRow, EndCol1).Merge = True
                End If
                If EndCol2 > StartCol2 Then
                    .Cells(iRow + 1, StartCol2, iRow + 1, EndCol2).Merge = True
                End If
                PrevDate = dDay
                PrevShift = ShiftCode
            Next
            iRow = iRow + 3
            dt = ds.Tables(1)
            For k = 1 To dt.Columns.Count - 1
                .Column(k).Width = 7.3
            Next
            For j = 0 To dt.Rows.Count - 1
                iCol = 1
                Dim colDes As Integer = 2
                If dt.Rows(j)(colDes) & "" = "-" Or dt.Rows(j)(colDes) & "" = "--" Then
                    .Row(iRow).Height = 2
                End If
                Dim Seq As String = dt.Rows(j)(0)
                For k = colDes To dt.Columns.Count - 1
                    Dim IsNum As Boolean = Seq < 7 And Seq <> 2 And k > colDes
                    If IsNum Then
                        .Cells(iRow, iCol).Value = ADbl(dt.Rows(j)(k))
                    Else
                        .Cells(iRow, iCol).Value = dt.Rows(j)(k)
                    End If
                    If k = colDes Then
                        Select Case .Cells(iRow, 1).Value
                            Case "1", "2", "3", "4", "5", "6"
                                .Cells(iRow, 1).Style.Fill.PatternType = ExcelFillStyle.Solid
                                .Cells(iRow, 1).Style.Fill.BackgroundColor.SetColor(cs.Color(.Cells(iRow, 1).Value))
                        End Select
                    ElseIf k > colDes Then
                        .Cells(iRow, iCol).Style.Numberformat.Format = FormatDigit(Digit)
                        LSL = dtLSL.Rows(0)(iCol)
                        USL = dtUSL.Rows(0)(iCol)
                        LCL = dtLCL.Rows(0)(iCol)
                        UCL = dtUCL.Rows(0)(iCol)
                        RLCL = dtRLCL.Rows(0)(iCol)
                        RUCL = dtRUCL.Rows(0)(iCol)
                        If Not IsDBNull(dt.Rows(j)(k)) And IsNum Then
                            Dim Value As Double = ADbl(dt.Rows(j)(k))
                            Dim PrevValue As Double
                            If k > colDes + 1 Then
                                PrevValue = ADbl(dt.Rows(j)(k - 1))
                            Else
                                PrevValue = ADbl(dt.Rows(j)(k))
                            End If
                            If ChartType <> "0" AndAlso dt.Rows(j)(0) = "6" AndAlso (Value < RLCL Or Value > RUCL) Then
                                .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
                                If k > colDes + 1 AndAlso (PrevValue < RLCL Or PrevValue > RUCL) Then
                                    .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                                ElseIf LastNG = 1 Then
                                    .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                                Else
                                    .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Yellow)
                                End If
                                LastNG = 0
                            ElseIf (Value < LSL Or Value > USL) Then
                                If dt.Rows(j)(0) = "1" Or dt.Rows(j)(0) = "3" Or dt.Rows(j)(0) = "4" Or dt.Rows(j)(0) = "5" Then
                                    .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
                                    .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Red)
                                End If
                            ElseIf Value < LCL Or Value > UCL Then
                                If dt.Rows(j)(0) = "1" Then
                                    .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
                                    .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                                ElseIf dt.Rows(j)(0) = "3" Or dt.Rows(j)(0) = "4" Or dt.Rows(j)(0) = "5" Then
                                    .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
                                    If ChartType = "0" Then
                                        .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                                    Else
                                        .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(LightYellow)
                                    End If
                                End If
                            End If
                            LastNG = 0
                        End If
                    End If
                    iCol = iCol + 1
                Next
                iRow = iRow + 1
            Next
            EndCol = dt.Columns.Count - 2
            EndRow = iRow - 1

            ExcelHeader(pExl, StartRow, 1, StartRow + 2, EndCol)
            ExcelBorder(pExl, StartRow, 1, EndRow, EndCol)
            ExcelFont(pExl, StartRow, 1, EndRow, EndCol, 8)
            LastRow = iRow + 1
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

                .Cells(3, 1, 3, 2).Value = "Factory"
                .Cells(3, 1, 3, 2).Merge = True
                .Cells(3, 3).Value = ": " & cls.FactoryName

                .Cells(4, 1, 4, 2).Value = "Process Group"
                .Cells(4, 1, 4, 2).Merge = True
                .Cells(4, 3).Value = ": " & cls.ProcessGroup

                .Cells(5, 1, 5, 2).Value = "Line Group"
                .Cells(5, 1, 5, 2).Merge = True
                .Cells(5, 3).Value = ": " & cls.LineGroup

                .Cells(3, 7, 3, 7).Value = "Machine"
                .Cells(3, 7, 3, 8).Merge = True
                .Cells(3, 9).Value = ": " & cls.ProcessCode

                .Cells(4, 7, 4, 7).Value = "Machine Process"
                .Cells(4, 7, 4, 8).Merge = True
                .Cells(4, 9).Value = ": " & cls.LineName

                .Cells(5, 7, 5, 7).Value = "Type"
                .Cells(5, 7, 5, 8).Merge = True
                .Cells(5, 9).Value = ": " & cls.ItemTypeName

                .Cells(3, 13, 3, 13).Value = "Item Check"
                .Cells(3, 13, 3, 14).Merge = True
                .Cells(3, 15).Value = ": " & cls.ItemCheckName

                .Cells(4, 13, 4, 13).Value = "Prod Date"
                .Cells(4, 13, 4, 14).Merge = True
                .Cells(4, 15).Value = ": " & Format(dtDate.Value, "dd MMM yyyy") & " to " & Format(dtTo.Value, "dd MMM yyyy")

                .Cells(5, 13, 5, 13).Value = "Verified Only"
                .Cells(5, 13, 5, 14).Merge = True
                .Cells(5, 15).Value = ": " & cboShow.Text

                Dim rgHdr As ExcelRange = .Cells(3, 1, 5, 15)
                rgHdr.Style.HorizontalAlignment = HorzAlignment.Near
                rgHdr.Style.VerticalAlignment = VertAlignment.Center
                rgHdr.Style.Font.Size = 9
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
                Dim ProcessGroup As String = ""
                Dim LineGroup As String = ""
                Dim ProcessCode As String = ""

                Dim Ln As ClsLine = ClsLineDB.GetData(FactoryCode, Line)
                If Ln IsNot Nothing Then
                    ProcessCode = Ln.ProcessCode
                    LineGroup = Ln.LineGroup
                    ProcessGroup = Ln.ProcessGroup
                End If
                Dim ItemCheckCode As String = Request.QueryString("ItemCheckCode")
                Dim ProdDate As String = Request.QueryString("ProdDate")
                Dim ProdDate2 As String = Request.QueryString("ProdDate2")
                Dim Shift As String = Request.QueryString("Shift")
                Dim Sequence As String = Request.QueryString("Sequence")

                InitCombo(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence, ProdDate2, ProcessGroup, LineGroup, ProcessCode)
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
                        cboProcessGroup.DataSource = clsProcessGroupDB.GetList(pUser, User.FactoryCode)
                        cboProcessGroup.DataBind()
                    End If
                End If
                'InitCombo("F001", "TPMSBR011", "015", "IC021", "2022-08-03", "SH001", 1, "2022-09-01")
            End If
        End If
    End Sub

    Private Sub InitCombo(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ShiftCode As String, Sequence As String, ProdDate2 As String, ProcessGroup As String, LineGroup As String, ProcessCode As String)
        pUser = Session("user") & ""
        dtDate.Value = CDate(ProdDate)
        dtTo.Value = CDate(ProdDate2)
        cboFactory.Value = FactoryCode

        cboProcessGroup.DataSource = clsProcessGroupDB.GetList(pUser, FactoryCode)
        cboProcessGroup.DataBind()
        cboProcessGroup.Value = ProcessGroup

        cboLineGroup.DataSource = clsLineGroupDB.GetList(pUser, FactoryCode, ProcessGroup)
        cboLineGroup.DataBind()
        cboLineGroup.Value = LineGroup

        cboProcess.DataSource = clsProcessDB.GetList(pUser, FactoryCode, ProcessGroup, LineGroup)
        cboProcess.DataBind()
        cboProcess.Value = ProcessCode

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

    Private Function AFormat(v As Object) As String
        If v Is Nothing OrElse IsDBNull(v) Then
            Return ""
        Else
            Return Format(v, FormatDigit(Digit))
        End If
    End Function

    Dim dtXR As DataTable
    Dim dtLSL As DataTable
    Dim dtUSL As DataTable
    Dim dtLCL As DataTable
    Dim dtUCL As DataTable
    Dim dtCP As DataTable
    Dim dtRUCL As DataTable
    Dim dtRLCL As DataTable
    Dim dtDel As DataTable
    Dim LastNG As Integer
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

            Digit = ClsSPCItemCheckMasterDB.GetDigit(ItemCheckCode)
            Dim ds As DataSet = clsSPCResultDetailDB.GetSampleByPeriod(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ProdDate2, VerifiedOnly, True, True)
            Dim dtDay As DataTable = ds.Tables(0)

            Dim dt2 As DataTable = clsSPCResultDetailDB.GetLastR(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, 1, VerifiedOnly)
            If dt2.Rows.Count > 0 Then
                LastNG = dt2.Rows(0)("NG")
            Else
                LastNG = 0
            End If

            Dim PrevDay As String = ""
            Dim PrevShift As String = ""
            For iDay = 0 To dtDay.Rows.Count - 1
                Dim SelDay As Date = dtDay.Rows(iDay)("ProdDate")
                Dim dDay As String = Format(SelDay, "yyyy-MM-dd")

                Dim BandDay As GridViewBandColumn
                Dim BandShift As GridViewBandColumn

                If dDay <> PrevDay Then
                    BandDay = New GridViewBandColumn
                    BandDay.Caption = Format(SelDay, "dd MMM yy")
                    .Columns.Add(BandDay)

                End If

                Dim SelShift As String = dtDay.Rows(iDay)("ShiftCode2")
                If SelShift <> PrevShift Or dDay <> PrevDay Then
                    BandShift = New GridViewBandColumn
                    BandShift.Caption = SelShift
                    BandDay.Columns.Add(BandShift)
                End If

                Dim colTime As New GridViewDataTextColumn
                colTime.Caption = dtDay.Rows(iDay)("RegisterDate")
                colTime.FieldName = dtDay.Rows(iDay)("ColName")
                colTime.Width = 60
                colTime.CellStyle.HorizontalAlign = HorizontalAlign.Center
                BandShift.Columns.Add(colTime)

                PrevDay = dDay
                PrevShift = SelShift
            Next
            dtXR = ds.Tables(1)
            gridX.DataSource = dtXR
            gridX.DataBind()

            ChartType = clsXRChartDB.GetChartType(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode)
            If ds.Tables.Count > 2 Then
                dtLSL = ds.Tables(2)
                dtUSL = ds.Tables(3)
                dtLCL = ds.Tables(4)
                dtUCL = ds.Tables(5)
                dtCP = ds.Tables(6)
                dtRUCL = ds.Tables(7)
                dtRLCL = ds.Tables(8)
                dtDel = ds.Tables(9)

                Dim f As String = FormatDigit(Digit)
                If dtCP.Rows.Count > 0 Then
                    .JSProperties("cpFactory") = dtCP.Rows(0)("FactoryName") & ""
                    .JSProperties("cpType") = dtCP.Rows(0)("ItemTypeName") & ""
                    .JSProperties("cpLine") = dtCP.Rows(0)("LineName") & ""
                    .JSProperties("cpItemCheck") = dtCP.Rows(0)("ItemCheckName") & ""
                    .JSProperties("cpProdDate") = ProdDate & " - " & ProdDate2
                    .JSProperties("cpUnit") = dtCP.Rows(0)("Unit") & ""

                    .JSProperties("cpLSL") = AFormat(dtCP.Rows(0)("LSL"))
                    .JSProperties("cpUSL") = AFormat(dtCP.Rows(0)("USL"))

                    .JSProperties("cpLCL") = AFormat(dtCP.Rows(0)("LCL"))
                    .JSProperties("cpCL") = AFormat(dtCP.Rows(0)("CL"))
                    .JSProperties("cpUCL") = AFormat(dtCP.Rows(0)("UCL"))

                    .JSProperties("cpD2") = AFormat(dtCP.Rows(0)("D2"))
                    .JSProperties("cpRUCL") = AFormat(dtCP.Rows(0)("RUCL"))
                    .JSProperties("cpXLCL") = AFormat(dtCP.Rows(0)("XbarLCL"))
                    .JSProperties("cpXUCL") = AFormat(dtCP.Rows(0)("XbarUCL"))

                    .JSProperties("cpMin") = AFormat(dtCP.Rows(0)("Min"))
                    .JSProperties("cpMax") = AFormat(dtCP.Rows(0)("Max"))
                    .JSProperties("cpAvg") = AFormat(dtCP.Rows(0)("Avg"))
                    .JSProperties("cpSTD") = AFormat(dtCP.Rows(0)("STD"))
                    .JSProperties("cpCP") = AFormat(dtCP.Rows(0)("CP"))
                    .JSProperties("cpCPK1") = AFormat(dtCP.Rows(0)("CPK1"))
                    .JSProperties("cpCPK2") = AFormat(dtCP.Rows(0)("CPK2"))
                    .JSProperties("cpXBarBar") = AFormat(dtCP.Rows(0)("Xbarbar"))
                    .JSProperties("cpRBar") = AFormat(dtCP.Rows(0)("Rbar"))
                    .JSProperties("cpCPKMin") = AFormat(dtCP.Rows(0)("CPKMin"))
                    If ChartType = "1" Or ChartType = "2" Then
                        .JSProperties("cpChartType") = "1"
                        If Not IsDBNull(dtCP.Rows(0)("Xbarbar")) AndAlso Not IsDBNull(dtCP.Rows(0)("LCL")) AndAlso Not IsDBNull(dtCP.Rows(0)("UCL")) Then
                            Dim XBarBar As Double = dtCP.Rows(0)("Xbarbar")
                            Dim LCL As Double = dtCP.Rows(0)("XbarLCL")
                            Dim UCL As Double = dtCP.Rows(0)("XbarUCL")
                            If XBarBar < LCL Or XBarBar > UCL Then
                                .JSProperties("cpXBarColor") = "1"
                            Else
                                .JSProperties("cpXBarColor") = "0"
                            End If
                        End If
                        If Not IsDBNull(dtCP.Rows(0)("RUCL")) AndAlso Not IsDBNull(dtCP.Rows(0)("RBar")) Then
                            Dim RBar As Double = dtCP.Rows(0)("RBar")
                            Dim RUCL As Double = dtCP.Rows(0)("RUCL")
                            If RBar > RUCL Then
                                .JSProperties("cpRBarColor") = "1"
                            Else
                                .JSProperties("cpRBarColor") = "0"
                            End If
                        End If
                    Else
                        .JSProperties("cpChartType") = "0"
                        .JSProperties("cpXBarColor") = "0"
                        .JSProperties("cpRBarColor") = "0"
                    End If
                Else
                    .JSProperties("cpChartType") = "0"
                    .JSProperties("cpXBarColor") = "0"
                    .JSProperties("cpRBarColor") = "0"
                End If
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

    Dim ChartType As String
    Dim PrevYellow As Integer = 0

    Private Function FormatDigit(d As Integer) As String
        Return "0." + StrDup(d, "0")
    End Function

    Private Sub gridX_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles gridX.HtmlDataCellPrepared
        Dim LCL As Double
        Dim UCL As Double
        Dim LSL As Double
        Dim USL As Double
        Dim RUCL As Double
        Dim RLCL As Double
        Dim Del As String
        Dim LightYellow As Color = Color.FromArgb(255, 255, 153)
        Dim iSeq As String = e.GetValue("Seq") & ""
        Dim ColName As String = e.DataColumn.FieldName
        If Not IsDBNull(e.CellValue) AndAlso ColName <> "Seq" AndAlso ColName <> "Des" AndAlso (e.GetValue("Seq") = "1" Or e.GetValue("Seq") = "3" Or e.GetValue("Seq") = "4" Or e.GetValue("Seq") = "5") Then
            LSL = dtLSL.Rows(0)(ColName)
            USL = dtUSL.Rows(0)(ColName)
            LCL = dtLCL.Rows(0)(ColName)
            UCL = dtUCL.Rows(0)(ColName)
            Dim Seq As Integer
            For i = 0 To dtDel.Rows.Count - 1
                Dim seq1 As String = dtDel.Rows(i)("SequenceNo")
                Dim seq2 As String = e.GetValue("SequenceNo")
                If seq1 = seq2 Then
                    Seq = i
                    Exit For
                End If
            Next
            Del = dtDel.Rows(Seq)(ColName)
            Dim Value As Double = clsSPCResultDB.ADecimal(e.CellValue)
            If e.GetValue("Seq") = "5" Then 'untuk XBar
                e.Cell.Text = Format(Value, FormatDigit(Digit))
            End If
            If Del = "1" And iSeq = "1" Then
                e.Cell.BackColor = Color.Silver
            ElseIf Value < LSL Or Value > USL Then
                e.Cell.BackColor = Color.Red
            ElseIf Value < LCL Or Value > UCL Then
                If e.GetValue("Seq") = "1" Or ChartType = "0" Then
                    e.Cell.BackColor = Color.Pink
                Else
                    e.Cell.BackColor = LightYellow
                End If
            End If
        End If
        If Not IsDBNull(e.CellValue) AndAlso ColName <> "Seq" AndAlso ColName <> "Sequence" AndAlso ColName <> "Des" Then
            Dim s As String = e.GetValue("Seq")
            If e.CellValue = "OK" Then
                e.Cell.BackColor = Color.Green
            ElseIf e.CellValue = "NG" Then
                e.Cell.BackColor = Color.Red
            End If
        End If
        If Not IsDBNull(e.CellValue) AndAlso ColName <> "Seq" And ColName <> "Des" And e.GetValue("Seq") = "6" And ChartType <> "0" Then
            RUCL = dtRUCL.Rows(0)(ColName)
            RLCL = dtRLCL.Rows(0)(ColName)
            Dim Value As Double = clsSPCResultDB.ADecimal(e.CellValue)
            If Value < RLCL Or Value > RUCL Then
                If PrevYellow = 1 Then
                    e.Cell.BackColor = Color.Pink
                Else
                    If LastNG = 1 Then
                        e.Cell.BackColor = Color.Pink
                    Else
                        e.Cell.BackColor = Color.Yellow
                    End If
                    PrevYellow = 1
                End If
            Else
                PrevYellow = 0
            End If
            LastNG = 0
        End If
        Dim cs As New clsSPCColor
        'If e.DataColumn.FieldName = "Des" Then
        '    If e.CellValue = "1" Then
        '        e.Cell.BackColor = cs.Color1
        '    ElseIf e.CellValue = "2" Then
        '        e.Cell.BackColor = cs.Color2
        '    ElseIf e.CellValue = "3" Then
        '        e.Cell.BackColor = cs.Color3
        '    ElseIf e.CellValue = "4" Then
        '        e.Cell.BackColor = cs.Color4
        '    ElseIf e.CellValue = "5" Then
        '        e.Cell.BackColor = cs.Color5
        '    End If
        'End If
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

    Private Sub LoadHistogram(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String, VerifiedOnly As String)
        With Histogram
            Dim ht As List(Of clsHistogram) = clsXRChartDB.GetHistogram(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, ProdDate2, VerifiedOnly)
            .DataSource = ht
            .DataBind()
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            If ht.Count > 0 Then
                Dim ht1 As clsHistogram = ht(0)
                'diagram.AxisX.NumericScaleOptions.IntervalOptions.OverflowValue = ht1.SpecUSL
                'diagram.AxisX.NumericScaleOptions.IntervalOptions.UnderflowValue = ht1.SpecLSL
                'diagram.AxisX.WholeRange.SetMinMaxValues(ht1.SpecLSL, ht1.SpecUSL)

                'diagram.AxisX.WholeRange.MaxValue = ht1.SpecUSL
                'diagram.AxisX.WholeRange.MinValue = ht1.SpecLSL
                'diagram.AxisX.WholeRange.EndSideMargin = 0.1
                'diagram.AxisX.WholeRange.StartSideMargin = 0.1

                diagram.AxisX.ConstantLines.Clear()
                Dim LCL As New ConstantLine("LCL")
                LCL.Title.TextColor = Color.Black
                LCL.Color = System.Drawing.Color.Red
                LCL.LineStyle.Thickness = 1
                LCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisX.ConstantLines.Add(LCL)
                LCL.AxisValue = ht1.CPLCL
                LCL.ShowInLegend = True

                Dim UCL As New ConstantLine("UCL")
                UCL.Title.TextColor = Color.Black
                UCL.Color = System.Drawing.Color.Red
                UCL.LineStyle.Thickness = 1
                UCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisX.ConstantLines.Add(UCL)
                UCL.AxisValue = ht1.CPUCL
                UCL.ShowInLegend = True

                Dim DarkGray As Color = Color.FromArgb(33, 33, 33)

                Dim CL As New ConstantLine("CL")
                CL.Title.TextColor = DarkGray
                CL.Color = System.Drawing.Color.Red
                CL.LineStyle.Thickness = 1
                CL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisX.ConstantLines.Add(CL)
                CL.AxisValue = ht1.CPCL
                CL.ShowInLegend = True

                Dim LSL As New ConstantLine("      LSL")
                LSL.Title.TextColor = DarkGray
                LSL.Color = System.Drawing.Color.Red
                LSL.LineStyle.Thickness = 1
                LSL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisX.ConstantLines.Add(LSL)
                LSL.AxisValue = ht1.SpecLSL
                LSL.ShowInLegend = True

                Dim USL As New ConstantLine("      USL")
                USL.Title.TextColor = DarkGray
                USL.Color = System.Drawing.Color.Red
                USL.LineStyle.Thickness = 1
                USL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisX.ConstantLines.Add(USL)
                USL.AxisValue = ht1.SpecUSL
                USL.ShowInLegend = True

                'Dim MaxValue As Single = ht(0).MaxValue
                'diagram.AxisX.WholeRange.SideMarginsValue = MaxValue
                diagram.AxisY.NumericScaleOptions.GridSpacing = 5
            End If
        End With
    End Sub

    Private Sub LoadChartX(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String, VerifiedOnly As String)
        Digit = ClsSPCItemCheckMasterDB.GetDigit(ItemCheckCode)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartXRMonthly(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, ProdDate2, VerifiedOnly)
        Dim MinValue As Double, MaxValue As Double, CountSeq As Integer
        With chartX
            .DataSource = xr
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            diagram.AxisX.WholeRange.MinValue = 0
            diagram.AxisX.WholeRange.MaxValue = 12

            diagram.AxisX.GridLines.LineStyle.DashStyle = DashStyle.Solid
            diagram.AxisX.GridLines.MinorVisible = True
            diagram.AxisX.MinorCount = 1
            diagram.AxisX.GridLines.Visible = False
            diagram.AxisY.Label.TextPattern = "{V:" + FormatDigit(Digit) + "}"

            diagram.AxisY.NumericScaleOptions.CustomGridAlignment = 0.005
            diagram.AxisY.GridLines.MinorVisible = False

            Dim ChartType As String = clsXRChartDB.GetChartType(FactoryCode, ItemTypeCode, Line, ItemCheckCode)
            If ChartType = "1" Or ChartType = "2" Then
                .Titles(0).Text = "Xbar Control Chart"
            Else
                .Titles(0).Text = "Graph Monitoring"
            End If

            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate2)
            diagram.AxisY.ConstantLines.Clear()
            If Setup IsNot Nothing Then
                Dim LCL As New ConstantLine("LCL")
                LCL.Color = System.Drawing.Color.Red
                LCL.LineStyle.Thickness = 1
                LCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(LCL)
                LCL.AxisValue = Setup.CPLCL

                Dim UCL As New ConstantLine("UCL")
                UCL.Color = System.Drawing.Color.Red
                UCL.LineStyle.Thickness = 1
                UCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(UCL)
                UCL.AxisValue = Setup.CPUCL

                Dim CL As New ConstantLine("CL")
                CL.Color = System.Drawing.Color.Red
                CL.LineStyle.Thickness = 1
                CL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(CL)
                CL.AxisValue = Setup.CPCL


                Dim Spasi As String
                If Setup.SpecLSL = Setup.CPLCL Or Setup.SpecUSL = Setup.CPUCL Then
                    Spasi = "      "
                Else
                    Spasi = ""
                End If

                If ChartType = "1" Or ChartType = "2" Then
                    Dim XBarLCL As New ConstantLine(Spasi + "XbarLCL")
                    XBarLCL.Color = System.Drawing.Color.Orange
                    XBarLCL.LineStyle.Thickness = 1
                    XBarLCL.LineStyle.DashStyle = DashStyle.Dash
                    diagram.AxisY.ConstantLines.Add(XBarLCL)
                    XBarLCL.AxisValue = Setup.XBarLCL

                    Dim XBarUCL As New ConstantLine(Spasi + "XbarUCL")
                    XBarUCL.Color = System.Drawing.Color.Orange
                    XBarUCL.LineStyle.Thickness = 1
                    XBarUCL.LineStyle.DashStyle = DashStyle.Dash
                    diagram.AxisY.ConstantLines.Add(XBarUCL)
                    XBarUCL.AxisValue = Setup.XBarUCL
                End If

                Dim LSL As New ConstantLine(Spasi + "LSL")
                LSL.Color = System.Drawing.Color.Red
                LSL.LineStyle.Thickness = 1
                LSL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(LSL)
                LSL.AxisValue = Setup.SpecLSL

                Dim USL As New ConstantLine(Spasi + "USL")
                USL.Color = System.Drawing.Color.Red
                USL.LineStyle.Thickness = 1
                USL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(USL)
                USL.AxisValue = Setup.SpecUSL

                If xr.Count > 0 Then
                    MinValue = xr(0).MinValue
                    MaxValue = xr(0).MaxValue
                    CountSeq = xr(0).CountSeq
                    If Setup.SpecLSL < MinValue Then
                        MinValue = Setup.SpecLSL
                    End If
                    If Setup.SpecUSL > MaxValue Then
                        MaxValue = Setup.SpecUSL
                    End If
                Else
                    CountSeq = 4
                    MinValue = Setup.SpecLSL
                    MaxValue = Setup.SpecUSL
                End If
                Dim EndSideMargin As Single = Math.Round((MaxValue - MinValue) / 20, 3)

                diagram.AxisY.WholeRange.MinValue = MinValue
                diagram.AxisY.WholeRange.MaxValue = MaxValue
                diagram.AxisY.WholeRange.EndSideMargin = EndSideMargin

                diagram.AxisY.VisualRange.MinValue = MinValue
                diagram.AxisY.VisualRange.MaxValue = MaxValue
                diagram.AxisY.VisualRange.EndSideMargin = EndSideMargin

                Dim diff As Double = MaxValue - MinValue
                Dim gridAlignment As Double = Math.Round(diff / 15, 3)
                diagram.AxisY.NumericScaleOptions.CustomGridAlignment = gridAlignment

                CType(.Diagram, XYDiagram).SecondaryAxesY.Clear()
                Dim myAxisY As New SecondaryAxisY("my Y-Axis")
                myAxisY.Visibility = DevExpress.Utils.DefaultBoolean.False
                myAxisY.WholeRange.EndSideMargin = 0
                myAxisY.WholeRange.StartSideMargin = 0
                CType(.Diagram, XYDiagram).SecondaryAxesY.Add(myAxisY)
                CType(.Series("Rule").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                CType(.Series("RuleYellow").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                .SeriesTemplate.CrosshairLabelPattern = "{S}: {V:" + FormatDigit(Digit) + "}"
            End If
            .DataBind()
            Dim ChartWidth As Integer = CountSeq * 80
            If ChartWidth < 400 Then
                ChartWidth = 400
            ElseIf ChartWidth > 1080 Then
                ChartWidth = 1080
            End If
            '.Width = ChartWidth
        End With
    End Sub

    Private Sub chartR_CustomCallback(sender As Object, e As CustomCallbackEventArgs) Handles chartR.CustomCallback
        Dim Prm As String = e.Parameter
        If Prm = "" Then
            Prm = "F001|TPMSBR011|015|IC021|03 Aug 2022"
        End If
        Dim FactoryCode As String = Split(Prm, "|")(0)
        Dim ItemTypeCode As String = Split(Prm, "|")(1)
        Dim LineCode As String = Split(Prm, "|")(2)
        Dim ItemCheckCode As String = Split(Prm, "|")(3)
        Dim PrevDate As String = Split(Prm, "|")(4)
        Dim ProdDate As String = Split(Prm, "|")(5)
        Dim VerifiedOnly As String = Split(Prm, "|")(6)
        LoadChartR(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, PrevDate, ProdDate, VerifiedOnly)
    End Sub

    Private Sub LoadChartR1(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, PrevDate As String, ProdDate As String, VerifiedOnly As String)
        Digit = ClsSPCItemCheckMasterDB.GetDigit(ItemCheckCode)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartR(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, PrevDate, VerifiedOnly,, True)
        If xr.Count = 0 Then
            chartR.JSProperties("cpShow") = "0"
        Else
            chartR.JSProperties("cpShow") = "1"
        End If
        With chartR
            .DataSource = xr
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            diagram.AxisX.WholeRange.MinValue = 0
            diagram.AxisX.WholeRange.MaxValue = 12

            diagram.AxisX.GridLines.LineStyle.DashStyle = DashStyle.Solid
            diagram.AxisX.GridLines.MinorVisible = True
            diagram.AxisX.MinorCount = 1
            diagram.AxisX.GridLines.Visible = False

            Dim MaxValue As Double, CountSeq As Integer
            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)
            diagram.AxisY.ConstantLines.Clear()
            If Setup IsNot Nothing Then
                Dim RUCL As New ConstantLine("R UCL")
                RUCL.Color = System.Drawing.Color.Orange
                RUCL.LineStyle.Thickness = 1
                RUCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(RUCL)
                RUCL.AxisValue = Setup.RUCL

                If xr.Count > 0 Then
                    CountSeq = xr(0).CountSeq
                    If xr(0).MaxValue > Setup.RUCL Then
                        MaxValue = xr(0).MaxValue
                    Else
                        MaxValue = Setup.RUCL
                    End If
                End If
                diagram.AxisY.WholeRange.MaxValue = MaxValue
                diagram.AxisY.VisualRange.MaxValue = MaxValue

                CType(.Diagram, XYDiagram).SecondaryAxesY.Clear()
                Dim myAxisY As New SecondaryAxisY("my Y-Axis")
                myAxisY.Visibility = DevExpress.Utils.DefaultBoolean.False
                myAxisY.WholeRange.EndSideMargin = 0
                myAxisY.WholeRange.StartSideMargin = 0
                CType(.Diagram, XYDiagram).SecondaryAxesY.Add(myAxisY)
                CType(.Series("RuleYellow").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                CType(.Series("RuleRed").View, XYDiagramSeriesViewBase).AxisY = myAxisY

                Dim EndSideMargin As Single = Math.Round(MaxValue / 5, 3)
                diagram.AxisY.VisualRange.EndSideMargin = EndSideMargin
                diagram.AxisY.WholeRange.EndSideMargin = EndSideMargin
                If MaxValue > 0 Then
                    Dim GridAlignment As Double = Math.Round(MaxValue / 20, 4)
                    diagram.AxisY.NumericScaleOptions.CustomGridAlignment = GridAlignment
                End If
                .SeriesTemplate.CrosshairLabelPattern = "{S}: {V:" + FormatDigit(Digit) + "}"
            End If
            .DataBind()
            Dim ChartWidth As Integer = CountSeq * 80
            If ChartWidth < 400 Then
                ChartWidth = 400
            ElseIf ChartWidth > 1080 Then
                ChartWidth = 1080
            End If
            '.Width = ChartWidth
        End With
    End Sub

    Private Sub LoadChartR(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, PrevDate As String, ProdDate As String, VerifiedOnly As String)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartR(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, PrevDate, VerifiedOnly,, True)
        Digit = ClsSPCItemCheckMasterDB.GetDigit(ItemCheckCode)
        If xr.Count = 0 Then
            chartR.JSProperties("cpShow") = "0"
        Else
            chartR.JSProperties("cpShow") = "1"
        End If
        With chartR
            .DataSource = xr
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            diagram.AxisX.WholeRange.MinValue = 0
            diagram.AxisX.WholeRange.MaxValue = 12

            diagram.AxisX.GridLines.LineStyle.DashStyle = DashStyle.Solid
            diagram.AxisX.GridLines.MinorVisible = True
            diagram.AxisX.MinorCount = 1
            diagram.AxisX.GridLines.Visible = False
            diagram.AxisY.Label.TextPattern = "{V:" + FormatDigit(Digit) + "}"

            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)
            diagram.AxisY.ConstantLines.Clear()
            If Setup IsNot Nothing Then
                Dim RUCL As New ConstantLine("R UCL")
                RUCL.Color = System.Drawing.Color.Orange
                RUCL.LineStyle.Thickness = 1
                RUCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(RUCL)
                RUCL.AxisValue = Setup.RUCL

                CType(.Diagram, XYDiagram).SecondaryAxesY.Clear()
                Dim myAxisY As New SecondaryAxisY("my Y-Axis")
                myAxisY.Visibility = DevExpress.Utils.DefaultBoolean.False
                myAxisY.WholeRange.EndSideMargin = 0
                myAxisY.WholeRange.StartSideMargin = 0
                CType(.Diagram, XYDiagram).SecondaryAxesY.Add(myAxisY)
                CType(.Series("RuleYellow").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                CType(.Series("RuleRed").View, XYDiagramSeriesViewBase).AxisY = myAxisY

                Dim MaxValue As Double
                If xr.Count > 0 Then
                    If xr(0).MaxValue > Setup.RUCL Then
                        MaxValue = xr(0).MaxValue
                    Else
                        MaxValue = Setup.RUCL
                    End If
                End If
                diagram.AxisY.WholeRange.MaxValue = MaxValue
                diagram.AxisY.VisualRange.MaxValue = MaxValue
                If MaxValue > 0 Then
                    Dim GridAlignment As Double = Math.Round(MaxValue / 10, 4)
                    diagram.AxisY.NumericScaleOptions.CustomGridAlignment = GridAlignment
                End If
                Dim EndSideMargin As Single = Math.Round(MaxValue / 5, 3)
                diagram.AxisY.VisualRange.EndSideMargin = EndSideMargin
                diagram.AxisY.WholeRange.EndSideMargin = EndSideMargin
                .SeriesTemplate.CrosshairLabelPattern = "{S}: {V:" + FormatDigit(Digit) + "}"
            End If
            .DataBind()
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
        Dim VerifiedOnly As String = Split(Prm, "|")(6)
        LoadChartX(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ProdDate2, VerifiedOnly)
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
        Dim LineCode As String = Split(e.Parameter, "|")(1)
        Dim pUser As String = Session("user")
        cboType.DataSource = clsItemTypeDB.GetList(FactoryCode, LineCode, pUser)
        cboType.DataBind()
        If cboType.Items.Count = 1 Then
            cboType.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboLine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLine.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ProcessCode As String = Split(e.Parameter, "|")(1)
        Dim UserID As String = Session("user") & ""
        cboLine.DataSource = ClsLineDB.GetListByProcess(UserID, FactoryCode, ProcessCode)
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
        Dim VerifiedOnly As String = Split(Prm, "|")(6)
        LoadHistogram(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ProdDate2, VerifiedOnly)
    End Sub

    Private Function ADbl(v As Object) As Object
        Dim decimalSeparator As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

        If v Is Nothing OrElse IsDBNull(v) Then
            Return Nothing
        ElseIf Not IsNumeric(v) Then
            Return Nothing
        Else
            v = Replace(v, ".", decimalSeparator)
            Return CDbl(v)
        End If
    End Function

    Private Sub cboProcessGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcessGroup.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim UserID As String = Session("user") & ""
        cboProcessGroup.DataSource = clsProcessGroupDB.GetList(UserID, FactoryCode)
        cboProcessGroup.DataBind()
    End Sub

    Private Sub cboLineGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineGroup.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ProcessGroup As String = Split(e.Parameter, "|")(1)
        Dim UserID As String = Session("user") & ""
        cboLineGroup.DataSource = clsLineGroupDB.GetList(UserID, FactoryCode, ProcessGroup)
        cboLineGroup.DataBind()
        If cboLineGroup.Items.Count = 1 Then
            cboLineGroup.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboProcess_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcess.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ProcessGroup As String = Split(e.Parameter, "|")(1)
        Dim LineGroup As String = Split(e.Parameter, "|")(2)
        Dim UserID As String = Session("user") & ""
        cboProcess.DataSource = clsProcessDB.GetList(UserID, FactoryCode, ProcessGroup, LineGroup)
        cboProcess.DataBind()
    End Sub
End Class