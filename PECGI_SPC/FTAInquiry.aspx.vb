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

Public Class FTAInquiry
    Inherits System.Web.UI.Page
    Dim pUser As String = ""
    Public AuthApprove As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public ValueType As String
    Dim GlobalPrm As String = ""
    Dim row_GridTitle = 0
    Dim row_HeaderResult = 0
    Dim row_HeaderActivity = 0
    Dim row_CellResult = 0
    Dim row_CellChart = 0
    Dim row_CellActivity = 0
    Dim col_HeaderResult = 0
    Dim col_HeaderActivity = 0
    Dim col_CellResult = 0
    Dim col_CellActivity = 0
    Dim RowIndexName As String = ""
    Dim CharacteristicSts As String = ""
    Dim LastRow As Integer
    Dim ChartType As String

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
        Public Property ShiftCode As String
        Public Property Shiftname As String
        Public Property Seq As Integer
        Public Property VerifiedOnly As String
    End Class

    Dim dtXR As DataTable
    Dim dtLSL As DataTable
    Dim dtUSL As DataTable
    Dim dtLCL As DataTable
    Dim dtUCL As DataTable
    Dim dtCP As DataTable
    Dim dtRUCL As DataTable
    Dim dtRLCL As DataTable
    Dim LastNG As Integer

    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        grid.JSProperties("cp_message") = ErrMsg
        grid.JSProperties("cp_type") = msgType
        grid.JSProperties("cp_val") = pVal
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GlobalPrm = Request.QueryString("FactoryCode") & ""
        sGlobal.getMenu("C020 ")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user") & ""
        hfUserID.Set("UserID", pUser)
        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "C020 ")
        grid.SettingsDataSecurity.AllowInsert = True
        grid.SettingsDataSecurity.AllowEdit = True

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
                Dim Shift As String = Request.QueryString("Shift")
                Dim Sequence As String = Request.QueryString("Sequence")

                InitCombo(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence, ProcessGroup, LineGroup, ProcessCode)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "GridLoad();", True)
            Else
                pUser = Session("user") & ""
                dtDate.Value = Now.Date
                dtTo.Value = dtDate.Value
                If pUser <> "" Then
                    Dim User As clsUserSetup = clsUserSetupDB.GetData(pUser)
                    If User IsNot Nothing Then
                        Dim FactoryCode As String = User.FactoryCode
                        Dim ProdDate As String = Session("C02ProdDate") & ""
                        Dim ProcessGroup As String = Session("C02ProcessGroup") & ""
                        Dim LineGroup As String = Session("C02LineGroup") & ""
                        Dim ProcessCode As String = Session("C02ProcessCode") & ""
                        Dim ItemTypeCode As String = Session("C02ItemTypeCode") & ""
                        Dim LineCode As String = Session("C02LineCode") & ""
                        Dim ItemCheckCode As String = Session("C02ItemCheckCode") & ""
                        Dim ShiftCode As String = Session("C02ShiftCode") & ""
                        Dim Sequence As String = Session("C02Sequence") & ""
                        If ProcessGroup <> "" Then
                            InitCombo(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ShiftCode, Sequence, ProcessGroup, LineGroup, ProcessCode)
                        Else
                            DefaultCombo(User.FactoryCode)
                        End If
                    End If
                End If
                'InitCombo(User.FactoryCode, "TPMSBR011", "015", "IC021", "2022-08-04", "SH001", 1)
            End If
            btnFTA.ClientEnabled = False
            btnSample.ClientEnabled = False
        End If
    End Sub

    Private Sub InitCombo(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ShiftCode As String, Sequence As String, ProcessGroup As String, LineGroup As String, ProcessCode As String)
        pUser = Session("user") & ""
        dtDate.Value = CDate(ProdDate)
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

        cboLine.DataSource = ClsLineDB.GetListByProcess(pUser, FactoryCode, ProcessCode)
        cboLine.DataBind()
        cboLine.Value = Line

        cboType.DataSource = clsItemTypeDB.GetList(FactoryCode, Line, pUser)
        cboType.DataBind()
        cboType.Value = ItemTypeCode

        cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, Line)
        cboItemCheck.DataBind()
        cboItemCheck.Value = ItemCheckCode

    End Sub

    Private Sub DefaultCombo(FactoryCode As String)
        Dim ProdDate As String = Format(Now.Date, "yyyy-MM-dd")
        Dim ProcessGroup As String
        Dim LineGroup As String
        Dim ProcessCode As String
        Dim Line As String
        Dim ItemTypeCode As String
        Dim ItemCheckCode As String
        Dim ShiftCode As String

        pUser = Session("user") & ""
        dtDate.Value = Now.Date
        cboFactory.Value = FactoryCode

        cboProcessGroup.DataSource = clsProcessGroupDB.GetList(pUser, FactoryCode)
        cboProcessGroup.DataBind()
        If cboProcessGroup.Items.Count > 0 Then
            cboProcessGroup.SelectedIndex = 0
            ProcessGroup = cboProcessGroup.Value

            cboLineGroup.DataSource = clsLineGroupDB.GetList(pUser, FactoryCode, ProcessGroup)
            cboLineGroup.DataBind()
            cboLineGroup.SelectedIndex = 0
            LineGroup = cboLineGroup.Value

            cboProcess.DataSource = clsProcessDB.GetList(pUser, FactoryCode, ProcessGroup, LineGroup)
            cboProcess.DataBind()
            cboProcess.SelectedIndex = 0
            ProcessCode = cboProcess.Value

            cboLine.DataSource = ClsLineDB.GetListByProcess(pUser, FactoryCode, ProcessCode)
            cboLine.DataBind()
            If cboLine.Items.Count > 0 Then
                cboLine.SelectedIndex = 0
                Line = cboLine.Value
                cboType.DataSource = clsItemTypeDB.GetList(FactoryCode, Line, pUser)
                cboType.DataBind()
                If cboType.Items.Count > 0 Then
                    cboType.SelectedIndex = 0
                    ItemTypeCode = cboType.Value

                    cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, Line)
                    cboItemCheck.DataBind()
                    If cboItemCheck.Items.Count = 1 Then
                        cboItemCheck.SelectedIndex = 0
                        ItemCheckCode = cboItemCheck.Value
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub up_FillCombo()
        cboFactory.DataSource = clsFactoryDB.GetList
        cboFactory.DataBind()
    End Sub

    Private Sub up_ClearGrid()
        grid.DataSource = Nothing
        grid.DataBind()
    End Sub
    Protected Sub grid_AfterPerformCallback(sender As Object, e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
            GridLoad(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), Format(dtTo.Value, "yyyy-MM-dd"), cboMK.Value, cboQC.Value)
        End If
    End Sub

    Private Function ValidateChartSetup(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String) As Boolean
        Dim SetupFound As Boolean = True
        Dim cs As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)
        If cs Is Nothing Then
            SetupFound = False
        End If
        Return SetupFound
    End Function

    Private Sub GridLoad(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String, MKVerification As Integer, QCVerification As Integer)
        Dim ErrMsg As String = ""
        Dim dt As DataTable = clsFTAResultDB.GetInquiry(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, ProdDate2, MKVerification, QCVerification)
        grid.DataSource = dt
        grid.DataBind()
        Dim UserID As String = Session("user")
        grid.SettingsDataSecurity.AllowInsert = AuthUpdate
        grid.SettingsDataSecurity.AllowEdit = AuthUpdate
        If grid.SettingsDataSecurity.AllowInsert Then
            grid.JSProperties("cpAllowInsert") = "1"
        Else
            grid.JSProperties("cpAllowInsert") = "0"
        End If
        If AuthUpdate Then
            grid.JSProperties("cpAllowUpdate") = "1"
        Else
            grid.JSProperties("cpAllowUpdate") = "0"
        End If
        Session("C02User") = UserID
        Session("C02ProdDate") = ProdDate
        Session("C02ProcessGroup") = cboProcessGroup.Value
        Session("C02LineGroup") = cboLineGroup.Value
        Session("C02ProcessCode") = cboProcess.Value
        Session("C02ItemTypeCode") = ItemTypeCode
        Session("C02LineCode") = Line
        Session("C02ItemCheckCode") = ItemCheckCode
    End Sub

    Private Function ADbl(v As Object) As Object
        Dim decimalSeparator As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

        If v Is Nothing OrElse IsDBNull(v) Then
            Return Nothing
        Else
            v = Replace(v, ".", decimalSeparator)
            Return CDbl(v)
        End If
    End Function

    Private Function AFormat(v As Object) As String
        If v Is Nothing OrElse IsDBNull(v) Then
            Return ""
        Else
            Return Format(v, "0.000")
        End If
    End Function


    Private Sub grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles grid.CustomCallback
        Dim pFunction As String = Split(e.Parameters, "|")(0)
        Select Case pFunction
            Case "clear"
                up_ClearGrid()
            Case "load"
                Dim pFactory As String = Split(e.Parameters, "|")(1)
                Dim pItemType As String = Split(e.Parameters, "|")(2)
                Dim pLine As String = Split(e.Parameters, "|")(3)
                Dim pItemCheck As String = Split(e.Parameters, "|")(4)
                Dim pDate As String = Split(e.Parameters, "|")(5)
                Dim pDate2 As String = Split(e.Parameters, "|")(6)
                Dim pMK As String = Split(e.Parameters, "|")(7)
                Dim pQC As String = Split(e.Parameters, "|")(8)
                pMK = Val(pMK)
                pQC = Val(pQC)
                GridLoad(pFactory, pItemType, pLine, pItemCheck, pDate, pDate2, pMK, pQC)
        End Select
    End Sub

    Private Sub cboType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboType.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim LineCode As String = Split(e.Parameter, "|")(1)
        Dim UserID As String = Session("user")
        cboType.DataSource = clsItemTypeDB.GetList(FactoryCode, LineCode, UserID)
        cboType.DataBind()
    End Sub

    Private Sub cboLine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLine.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ProcessCode As String = Split(e.Parameter, "|")(1)
        Dim UserID As String = Session("user") & ""
        cboLine.DataSource = ClsLineDB.GetListByProcess(UserID, FactoryCode, ProcessCode)
        cboLine.DataBind()
    End Sub

    Private Sub GridTitle(ByVal pExl As ExcelWorksheet, cls As clsHeader)
        With pExl
            Try
                .Cells(1, 1).Value = "Production Sample Input"
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

                .Cells(3, 6, 3, 6).Value = "Machine"
                .Cells(3, 6, 3, 7).Merge = True
                .Cells(3, 8).Value = ": " & cls.ProcessCode

                .Cells(4, 6, 4, 6).Value = "Machine Process"
                .Cells(4, 6, 4, 7).Merge = True
                .Cells(4, 8).Value = ": " & cls.LineName

                .Cells(5, 6, 5, 6).Value = "Type"
                .Cells(5, 6, 5, 7).Merge = True
                .Cells(5, 8).Value = ": " & cls.ItemTypeName

                .Cells(3, 12, 3, 12).Value = "Item Check"
                .Cells(3, 12, 3, 13).Merge = True
                .Cells(3, 14).Value = ": " & cls.ItemCheckName

                .Cells(4, 12, 4, 12).Value = "Prod Date"
                .Cells(4, 12, 4, 13).Merge = True
                .Cells(4, 14).Value = ": " & cls.ProdDate

                .Cells(5, 12, 5, 12).Value = "Shift"
                .Cells(5, 12, 5, 13).Merge = True
                .Cells(5, 14).Value = ": " & cls.ShiftCode

                .Cells(6, 12, 6, 12).Value = "Sequence"
                .Cells(6, 12, 6, 13).Merge = True
                .Cells(6, 14).Value = ": " & cls.Seq

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

    Private Sub cboItemCheck_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemCheck.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim LineCode As String = Split(e.Parameter, "|")(2)
        cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, LineCode)
        cboItemCheck.DataBind()
    End Sub

    Protected Sub grid_BatchUpdate(sender As Object, e As ASPxDataBatchUpdateEventArgs)
        If e.UpdateValues.Count > 0 Then

        End If
    End Sub

    Private Sub DownloadExcel()
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
                Hdr.ShiftCode = cboMK.Value
                Hdr.Shiftname = cboMK.Text
                Hdr.Seq = cboQC.Value
                Hdr.ProcessGroup = cboProcessGroup.Text
                Hdr.LineGroup = cboLineGroup.Text
                Hdr.ProcessCode = cboProcess.Text

                GridTitle(ws, Hdr)
                GridExcelInput(ws, Hdr)
                GridExcel(ws, Hdr)
                .InsertRow(LastRow, 22)
            End With

            Dim stream As MemoryStream = New MemoryStream(Pck.GetAsByteArray())
            Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            Response.AppendHeader("Content-Disposition", "attachment; filename=ProdSampleInput_" & Format(Date.Now, "yyyy-MM-dd") & ".xlsx")
            Response.BinaryWrite(stream.ToArray())
            Response.End()

        End Using
    End Sub

    Private Sub GridExcel(pExl As ExcelWorksheet, Hdr As clsHeader)
        Dim dt As DataTable
        Dim iRow As Integer = LastRow
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
            PrevDate = clsSPCResultDB.GetPrevDate(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate)
            If PrevDate = "" Then
                PrevDate = Hdr.ProdDate
            End If
            Dim ds As DataSet = clsSPCResultDetailDB.GetSampleByPeriod(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, PrevDate, Hdr.ProdDate, Hdr.VerifiedOnly, True)

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

            StartRow = iRow
            .Cells(iRow, 1).Value = "Date"
            .Cells(iRow + 1, 1).Value = "Shift"
            .Cells(iRow + 2, 1).Value = "Time"

            StartCol1 = 2
            Dim MaxCol As Integer
            For iDay = 0 To dtDay.Rows.Count - 1
                Dim SelDay As Date = dtDay.Rows(iDay)("ProdDate")
                Dim dDay As String = Format(SelDay, "yyyy-MM-dd")
                Dim Seq As Integer = dtDay.Rows(iDay)("SeqNo")

                If SelDay = Hdr.ProdDate And Seq > Hdr.Seq Then
                    Exit For
                End If
                MaxCol = iDay + 2

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
            For j = 0 To dt.Rows.Count - 1
                iCol = 1
                Dim colDes As Integer = 2
                If dt.Rows(j)(colDes) & "" = "-" Or dt.Rows(j)(colDes) & "" = "--" Then
                    .Row(iRow).Height = 2
                End If
                Dim Seq As String = dt.Rows(j)(0)
                For k = colDes To MaxCol
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
                        .Cells(iRow, iCol).Style.Numberformat.Format = "0.000"
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
            EndCol = MaxCol
            EndRow = iRow - 1

            ExcelHeader(pExl, StartRow, 1, StartRow + 2, EndCol)
            ExcelBorder(pExl, StartRow, 1, EndRow, EndCol)
            ExcelFont(pExl, StartRow, 1, EndRow, EndCol, 8)
            LastRow = iRow + 1
        End With
    End Sub

    Private Sub GridExcelInput(pExl As ExcelWorksheet, Hdr As clsHeader)
        Dim dt As DataTable = clsSPCResultDetailDB.GetTable(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate, Hdr.ShiftCode, Hdr.Seq, Hdr.VerifiedOnly)
        Dim iRow As Integer = 8
        Dim StartRow As Integer = iRow
        Dim EndRow As Integer
        Dim MKUser As String = "", MKDate As String = ""
        Dim QCUser As String = "", QCDate As String = ""
        Dim USL As String = "", LSL As String = "", CL As String = "", UCL As String = "", LCL As String = "", NG As String = "", C As String = "", RLCL As Double, RUCL As Double
        Dim XBarUCL As String = "", XBarLCL As String = ""
        Dim vMin As String = "", vMax As String = "", vAvg As String = "", vR As String = "", SubLotNo As String = "", Remarks As String = ""
        Dim LightYellow As Color = Color.FromArgb(255, 255, 153)
        Dim cs As New clsSPCColor

        Dim Measure2Cls As String = ""
        Dim Item As clsItemCheck = clsItemCheckDB.GetData(Hdr.ItemCheckCode)
        If Item IsNot Nothing Then
            Measure2Cls = Item.Measure2Cls
        End If

        Dim colNo As Integer = 1
        Dim colValue1 As Integer = 2
        Dim colValue2 As Integer = 3
        Dim colValue As Integer
        If Measure2Cls = "1" Then
            colValue = 4
        Else
            colValue = 2
        End If
        Dim colJudgement As Integer = colValue + 1
        Dim colOperator As Integer = colJudgement + 1
        Dim colDate As Integer = colOperator + 1
        Dim colDelete As Integer = colDate + 1
        Dim colRemark As Integer = colDelete + 1
        Dim colUser As Integer = colRemark + 1
        Dim colLastUpdate As Integer = colUser + 1
        Dim colCount As Integer = colLastUpdate + 1

        With pExl
            .Cells(iRow, colNo).Value = "Data"
            If Measure2Cls = "1" Then
                .Cells(iRow, colValue1).Value = "Value 1"
                .Cells(iRow, colValue2).Value = "Value 2"
            End If
            .Cells(iRow, colValue).Value = "Value"
            .Cells(iRow, colJudgement).Value = "Judgement"
            .Cells(iRow, colOperator).Value = "Operator"
            .Cells(iRow, colDate).Value = "Sample Date"
            .Cells(iRow, colDelete).Value = "Delete Status"
            .Cells(iRow, colRemark).Value = "Remarks"
            .Cells(iRow, colUser).Value = "Last User"
            .Cells(iRow, colLastUpdate).Value = "Last Update"
            .Cells(iRow, colLastUpdate, iRow, colCount).Merge = True
            .Cells(iRow, 1, iRow, colCount).Style.Fill.PatternType = ExcelFillStyle.Solid
            .Cells(iRow, 1, iRow, colCount).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#878787"))
            .Cells(iRow, 1, iRow, colCount).Style.Font.Color.SetColor(Color.White)
            .Cells(iRow, 1, iRow, colCount).Style.WrapText = True
            .Cells(iRow, 1, iRow, colCount).Style.VerticalAlignment = ExcelVerticalAlignment.Center
            .Column(colNo).Width = 13
            .Column(colJudgement).Width = 11

            .Column(colDate).Width = 11
            .Column(colRemark).Width = 11
            If dt.Rows.Count > 0 Then
                MKDate = dt.Rows(0)("MKDate") & ""
                MKUser = dt.Rows(0)("MKUser") & ""
                QCDate = dt.Rows(0)("QCDate") & ""
                QCUser = dt.Rows(0)("QCUser") & ""
                USL = dt.Rows(0)("SpecUSL") & ""
                LSL = dt.Rows(0)("SpecLSL") & ""
                UCL = dt.Rows(0)("CPUCL") & ""
                CL = dt.Rows(0)("CPCL") & ""
                LCL = dt.Rows(0)("CPLCL") & ""
                XBarUCL = dt.Rows(0)("XBarUCL") & ""
                XBarLCL = dt.Rows(0)("XBarLCL") & ""
                vMin = dt.Rows(0)("MinValue") & ""
                vMax = dt.Rows(0)("MaxValue") & ""
                vAvg = dt.Rows(0)("AvgValue") & ""
                vR = dt.Rows(0)("RValue") & ""
                NG = dt.Rows(0)("NGValue") & ""
                C = dt.Rows(0)("CValue") & ""
                RLCL = dt.Rows(0)("RLCL")
                RUCL = dt.Rows(0)("RUCL")
                SubLotNo = dt.Rows(0)("SubLotNo") & ""
                Remarks = dt.Rows(0)("Remarks") & ""
            End If
            For i = 0 To dt.Rows.Count - 1
                iRow = iRow + 1
                .Cells(iRow, colNo).Value = dt.Rows(i)("SeqNo")

                .Cells(iRow, colValue1, iRow, colValue).Style.Numberformat.Format = "0.000"
                .Cells(iRow, colValue1).Value = dt.Rows(i)("Value1")
                .Cells(iRow, colValue2).Value = dt.Rows(i)("Value2")
                .Cells(iRow, colValue).Value = dt.Rows(i)("Value")

                .Cells(iRow, colJudgement).Value = dt.Rows(i)("Judgement")
                .Cells(iRow, colUser).Value = dt.Rows(i)("RegisterUser")
                .Cells(iRow, colDate).Style.Numberformat.Format = "HH:mm"
                .Cells(iRow, colDate).Value = dt.Rows(i)("RegisterDate")
                .Cells(iRow, colDelete).Value = dt.Rows(i)("DelStatus")
                .Cells(iRow, colRemark).Value = dt.Rows(i)("Remark")
                .Cells(iRow, colUser).Value = dt.Rows(i)("RegisterUser")
                .Cells(iRow, colLastUpdate).Value = dt.Rows(i)("RegisterDate")
                .Cells(iRow, colLastUpdate).Style.Numberformat.Format = "dd MMM yyyy HH:mm"
                .Cells(iRow, colLastUpdate, iRow, colCount).Merge = True
                EndRow = iRow
            Next

            If EndRow > StartRow Then
                Dim Range1 As ExcelRange = .Cells(StartRow, 1, EndRow, colCount)
                Range1.Style.Border.Top.Style = ExcelBorderStyle.Thin
                Range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                Range1.Style.Border.Right.Style = ExcelBorderStyle.Thin
                Range1.Style.Border.Left.Style = ExcelBorderStyle.Thin
                Range1.Style.Font.Size = 10
                Range1.Style.Font.Name = "Segoe UI"
                Range1.Style.HorizontalAlignment = HorzAlignment.Center
            End If

            iRow = iRow + 2
            .Cells(iRow, 1).Value = "Lot No"
            .Cells(iRow, 2).Value = SubLotNo
            .Cells(iRow + 1, 1).Value = "Remarks"
            .Cells(iRow + 1, 2).Value = Remarks

            .Cells(iRow, 5).Value = "Verification"
            .Cells(iRow + 1, 5).Value = "MK"
            .Cells(iRow + 2, 5).Value = "QC"

            .Cells(iRow, 6).Value = "PIC"
            .Cells(iRow + 1, 6).Value = MKUser
            .Cells(iRow + 2, 6).Value = QCUser

            .Cells(iRow, 7).Value = "Date"
            .Cells(iRow + 1, 7).Value = MKDate
            .Cells(iRow + 2, 7).Value = QCDate

            ExcelHeader(pExl, iRow, 5, iRow, 7)
            ExcelBorder(pExl, iRow, 5, iRow + 2, 7)

            iRow = iRow + 4

            .Cells(iRow, 2).Value = "Specification"
            .Cells(iRow, 2, iRow, 3).Merge = True
            .Cells(iRow, 4).Value = "Control Plan"
            .Cells(iRow, 4, iRow, 6).Merge = True

            Dim AddCol As Integer = 0
            If ChartType = "1" Or ChartType = "2" Then
                AddCol = 2
                .Cells(iRow, 7).Value = "X Bar Control"
                .Cells(iRow, 7, iRow, 8).Merge = True
            End If
            .Cells(iRow, 7 + AddCol).Value = "Result"
            .Cells(iRow, 7 + AddCol, iRow, 12).Merge = True

            .Cells(iRow + 1, 2).Value = "USL"
            .Cells(iRow + 2, 2).Value = ADbl(USL)
            .Cells(iRow + 1, 3).Value = "LSL"
            .Cells(iRow + 2, 3).Value = ADbl(LSL)
            .Cells(iRow + 1, 4).Value = "UCL"
            .Cells(iRow + 2, 4).Value = ADbl(UCL)
            .Cells(iRow + 1, 5).Value = "CL"
            .Cells(iRow + 2, 5).Value = ADbl(CL)
            .Cells(iRow + 1, 6).Value = "LCL"
            .Cells(iRow + 2, 6).Value = ADbl(LCL)

            If ChartType = "1" Or ChartType = "2" Then
                .Cells(iRow + 1, 7).Value = "UCL"
                .Cells(iRow + 2, 7).Value = ADbl(XBarUCL)
                .Cells(iRow + 1, 8).Value = "LCL"
                .Cells(iRow + 2, 8).Value = ADbl(XBarLCL)
            End If
            .Cells(iRow + 1, 7 + AddCol).Value = "Min"
            .Cells(iRow + 2, 7 + AddCol).Value = ADbl(vMin)
            .Cells(iRow + 1, 8 + AddCol).Value = "Max"
            .Cells(iRow + 2, 8 + AddCol).Value = ADbl(vMax)
            .Cells(iRow + 1, 9 + AddCol).Value = "Ave"
            .Cells(iRow + 2, 9 + AddCol).Value = ADbl(vAvg)
            .Cells(iRow + 1, 10 + AddCol).Value = "R"
            .Cells(iRow + 2, 10 + AddCol).Value = ADbl(vR)
            .Cells(iRow + 2, 2, iRow + 2, 10 + AddCol).Style.Numberformat.Format = "0.000"

            Dim Col As Integer = 7 + AddCol
            If vMin <> "" Then
                If ADbl(vMin) < ADbl(LSL) Or ADbl(vMin) > ADbl(USL) Then
                    .Cells(iRow + 2, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                    .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Red)
                ElseIf ADbl(vMin) < ADbl(LCL) Or ADbl(vMin) > ADbl(UCL) Then
                    .Cells(iRow + 2, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                    If ChartType = "0" Then
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                    Else
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(LightYellow)
                    End If
                End If
            End If
            If vMax <> "" Then
                Col = 8 + AddCol
                If ADbl(vMax) < ADbl(LSL) Or ADbl(vMax) > ADbl(USL) Then
                    .Cells(iRow + 2, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                    .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Red)
                ElseIf ADbl(vMax) < ADbl(LCL) Or ADbl(vMax) > ADbl(UCL) Then
                    .Cells(iRow + 2, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                    If ChartType = "0" Then
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                    Else
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(LightYellow)
                    End If
                End If
            End If
            If vAvg <> "" Then
                Col = 9 + AddCol
                If ADbl(vAvg) < ADbl(LSL) Or ADbl(vAvg) > ADbl(USL) Then
                    .Cells(iRow + 2, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                    .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Red)
                ElseIf ADbl(vAvg) < ADbl(LCL) Or ADbl(vAvg) > ADbl(UCL) Then
                    .Cells(iRow + 2, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                    If ChartType = "0" Then
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                    Else
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(LightYellow)
                    End If
                End If
            End If

            Dim dt2 As DataTable = clsSPCResultDetailDB.GetLastR(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate, Hdr.Seq, Hdr.VerifiedOnly)
            If dt2.Rows.Count > 0 Then
                LastNG = dt2.Rows(0)("NG")
            End If

            If vR <> "" Then
                Col = 10 + AddCol
                If (ADbl(vR) < RLCL Or ADbl(vR) > RUCL) And (ChartType = "1" Or ChartType = "2") Then
                    .Cells(iRow + 2, Col).Style.Fill.PatternType = ExcelFillStyle.Solid
                    If LastNG = 1 Then
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                    Else
                        .Cells(iRow + 2, Col).Style.Fill.BackgroundColor.SetColor(Color.Yellow)
                    End If
                End If
            End If

            .Cells(iRow + 1, 11 + AddCol, iRow + 1, 12 + AddCol).Style.Font.Size = 14
            .Cells(iRow + 1, 11 + AddCol, iRow + 1, 12 + AddCol).Style.Font.Bold = True
            If NG = "2" Then
                .Cells(iRow + 1, 12 + AddCol).Value = "NG"
                .Cells(iRow + 1, 12 + AddCol).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(iRow + 1, 12 + AddCol).Style.Fill.BackgroundColor.SetColor(Color.Red)
            ElseIf NG = "1" Or NG = "0" Then
                .Cells(iRow + 1, 12 + AddCol).Value = "OK"
                .Cells(iRow + 1, 12 + AddCol).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(iRow + 1, 12 + AddCol).Style.Fill.BackgroundColor.SetColor(Color.Green)
            Else
                .Cells(iRow + 1, 12 + AddCol).Value = ""
            End If

            .Cells(iRow + 1, 11 + AddCol).Value = C
            If C <> "" Then
                .Cells(iRow + 1, 11 + AddCol).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(iRow + 1, 11 + AddCol).Style.Fill.BackgroundColor.SetColor(Color.Orange)
            End If

            If ChartType = "1" Then
                .Cells(iRow, 1, iRow + 1, 1).Value = "@"
            End If
            .Cells(iRow, 1, iRow + 2, 1).Merge = True
            .Cells(iRow, 1, iRow + 2, 1).Style.VerticalAlignment = ExcelVerticalAlignment.Center
            .Cells(iRow + 1, 11 + AddCol, iRow + 2, 11 + AddCol).Merge = True
            .Cells(iRow + 1, 12 + AddCol, iRow + 2, 12 + AddCol).Merge = True
            .Cells(iRow + 1, 11 + AddCol, iRow + 2, 12 + AddCol).Style.VerticalAlignment = ExcelVerticalAlignment.Center

            .Cells(iRow + 2, 2, iRow + 2, 10 + AddCol).Style.Numberformat.Format = "0.000"

            ExcelHeader(pExl, iRow, 2, iRow + 1, 10 + AddCol)
            ExcelHeader(pExl, iRow, 11 + AddCol, iRow, 12 + AddCol)
            ExcelBorder(pExl, iRow, 1, iRow + 2, 12 + AddCol)

            .Cells(iRow, 1, iRow + 2, 1).Style.Font.Size = 14
            .Cells(iRow, 1, iRow + 2, 1).Style.Font.Bold = True

            LastRow = iRow + 4

            'Dim SelDay As Object = clsSPCResultDB.GetPrevDate(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate)
            'Dim dDay As String = Format(CDate(SelDay), "yyyy-MM-dd")

            'dt2 = clsSPCResultDetailDB.GetLastR(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, dDay, 1, Hdr.VerifiedOnly)
            'If dt2.Rows.Count > 0 Then
            '    LastNG = dt2.Rows(0)("NG")
            'Else
            '    LastNG = 0
            'End If

            '.Cells(iRow, 1).Value = "Date"
            '.Cells(iRow + 1, 1).Value = "Shift"
            '.Cells(iRow + 2, 1).Value = "Time"
            'StartRow = iRow
            'Dim StartCol1 As Integer, EndCol1 As Integer
            'Dim StartCol2 As Integer, EndCol2 As Integer
            'Dim FieldNames As New List(Of String)
            'Dim iCol As Integer = 1
            'Dim AlreadyNG As Boolean = False
            'For iDay = 1 To 2
            '    If Not IsDBNull(SelDay) Then
            '        iRow = StartRow
            '        iCol = iCol + 1
            '        dDay = Format(CDate(SelDay), "yyyy-MM-dd")
            '        .Cells(iRow, iCol).Value = Format(SelDay, "dd MMM yyyy")
            '        StartCol1 = iCol
            '        Dim Shiftlist As List(Of clsShift) = clsFrequencyDB.GetShift(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, dDay)
            '        For Each Shift In Shiftlist
            '            .Cells(iRow + 1, iCol).Value = Shift.ShiftCode
            '            StartCol2 = iCol
            '            Dim SeqList As List(Of clsSequenceNo) = clsFrequencyDB.GetSequence(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Shift.ShiftCode, dDay)
            '            For Each Seq In SeqList
            '                .Cells(iRow + 2, iCol).Value = Seq.StartTime
            '                FieldNames.Add(iDay.ToString + "_" + Shift.ShiftName.ToString + "_" + Seq.SequenceNo.ToString)
            '                iCol = iCol + 1
            '            Next
            '            EndCol2 = iCol - 1
            '            If EndCol2 > StartCol2 Then
            '                .Cells(iRow + 1, StartCol2, iRow + 1, EndCol2).Merge = True
            '            End If
            '        Next
            '        EndCol1 = iCol - 1
            '        If EndCol1 > StartCol1 Then
            '            .Cells(iRow, StartCol1, iRow, EndCol1).Merge = True
            '        End If
            '    End If
            '    iCol = iCol - 1
            '    SelDay = CDate(Hdr.ProdDate)
            'Next

            'iRow = StartRow + 3
            'dt = clsSPCResultDetailDB.GetTableXR(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate, Hdr.VerifiedOnly)
            'For j = 0 To dt.Rows.Count - 1
            '    iCol = 1
            '    EndCol = FieldNames.Count + 1
            '    If dt.Rows(j)(1) = "-" Or dt.Rows(j)(1) = "--" Then
            '        .Row(iRow).Height = 2
            '    Else
            '        .Cells(iRow, iCol).Value = dt.Rows(j)(1)
            '        Select Case .Cells(iRow, 1).Value
            '            Case "1", "2", "3", "4", "5", "6"
            '                .Cells(iRow, 1).Style.Fill.PatternType = ExcelFillStyle.Solid
            '                .Cells(iRow, 1).Style.Fill.BackgroundColor.SetColor(cs.Color(.Cells(iRow, 1).Value))
            '        End Select

            '        For Each Fn In FieldNames
            '            iCol = iCol + 1
            '            Dim objValue As Object = dt.Rows(j)(Fn)
            '            Dim Value As Double
            '            If iCol > 1 And objValue & "" <> "" Then
            '                Select Case .Cells(iRow, 1).Value
            '                    Case "1", "2", "3", "4", "5", "6", "Min", "Max", "Avg", "R"
            '                        Value = ADbl(objValue)
            '                        .Cells(iRow, iCol).Value = Value
            '                        .Cells(iRow, iCol).Style.Numberformat.Format = "0.000"
            '                    Case Else
            '                        .Cells(iRow, iCol).Value = objValue & ""
            '                End Select
            '                Select Case .Cells(iRow, 1).Value
            '                    Case "1", "2", "3", "4", "5", "6"
            '                        Value = ADbl(objValue)
            '                        If Value < ADbl(LSL) Or Value > ADbl(USL) Then
            '                            .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            '                            .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Red)
            '                        ElseIf Value < ADbl(LCL) Or Value > ADbl(UCL) Then
            '                            .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            '                            .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Pink)
            '                        End If
            '                    Case "Min", "Max", "Avg"
            '                        Value = ADbl(objValue)
            '                        If Value < ADbl(LSL) Or Value > ADbl(USL) Then
            '                            .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            '                            .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Red)
            '                        ElseIf Value < ADbl(LCL) Or Value > ADbl(UCL) Then
            '                            .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            '                            .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(LightYellow)
            '                        End If
            '                    Case "R"
            '                        Value = ADbl(objValue)
            '                        If Value < ADbl(RLCL) Or Value > ADbl(RUCL) And ChartType <> "0" Then
            '                            .Cells(iRow, iCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            '                            Dim rgb As String = .Cells(iRow, iCol - 1).Style.Fill.BackgroundColor.Rgb
            '                            If rgb = "FFFFFF00" Or rgb = "FFFFC0CB" Then
            '                                .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Pink)
            '                            ElseIf LastNG = 1 Then
            '                                .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Pink)
            '                                AlreadyNG = True
            '                            ElseIf AlreadyNG = False Then
            '                                .Cells(iRow, iCol).Style.Fill.BackgroundColor.SetColor(Color.Yellow)
            '                            End If
            '                        End If
            '                        LastNG = 0
            '                End Select
            '            Else
            '                .Cells(iRow, iCol).Value = dt.Rows(j)(Fn)
            '            End If
            '        Next
            '    End If
            '    iRow = iRow + 1
            'Next

            'ExcelHeader(pExl, StartRow, 1, StartRow + 2, EndCol)
            'ExcelBorder(pExl, StartRow, 1, iRow - 1, EndCol)
            'ExcelFont(pExl, StartRow, 1, iRow - 1, EndCol, 8)

            'LastRow = iRow + 1
        End With
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

    Private Sub ExcelFont(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer, FontSize As Integer)
        With Exl
            Dim Range As ExcelRange = .Cells(StartRow, StartCol, EndRow, EndCol)
            Range.Style.Font.Size = FontSize
            Range.Style.Font.Name = "Segoe UI"
        End With
    End Sub

    Private Sub grid_CellEditorInitialize(sender As Object, e As ASPxGridViewEditorEventArgs) Handles grid.CellEditorInitialize
        If e.Column.FieldName = "OK" Or e.Column.FieldName = "NG" Or e.Column.FieldName = "NoCheck" Then
            e.Editor.ReadOnly = False
        Else
            e.Editor.ReadOnly = True
            e.Editor.ForeColor = System.Drawing.Color.Silver
        End If
    End Sub

    Protected Sub IKLink_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim templatecontainer As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim UserID As String = ""
        UserID = templatecontainer.Grid.GetRowValues(templatecontainer.VisibleIndex, "No") & ""
        If UserID <> "" Then
            link.ClientSideEvents.Click = "ShowPopUpIK"
        End If
    End Sub

    Protected Sub IKLink2_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim templatecontainer As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim UserID As String = ""
        UserID = templatecontainer.Grid.GetRowValues(templatecontainer.VisibleIndex, "FTAID") & ""
        If UserID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {window.open('UserLine.aspx?prm=" + UserID + "','_self'); }"
        End If
    End Sub

    Protected Sub ActionLink_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim templatecontainer As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim UserID As String = ""
        UserID = templatecontainer.Grid.GetRowValues(templatecontainer.VisibleIndex, "FTAID") & ""
        If UserID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {window.open('UserLine.aspx?prm=" + UserID + "','_self'); }"
        End If
    End Sub

    Private Sub grid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles grid.HtmlDataCellPrepared
        If e.DataColumn.FieldName <> "OK" And e.DataColumn.FieldName <> "NG" And e.DataColumn.FieldName <> "NoCheck" Then
            e.Cell.Attributes.Add("onclick", "event.cancelBubble = true")
        End If
    End Sub

    Private Sub grid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles grid.HtmlRowPrepared

    End Sub


    Protected Sub grid_RowValidating(sender As Object, e As ASPxDataValidationEventArgs)

    End Sub

    Private Sub AddError(ByVal errors As Dictionary(Of GridViewColumn, String), ByVal column As GridViewColumn, ByVal errorText As String)
        If errors.ContainsKey(column) Then
            Return
        End If
        errors(column) = errorText
    End Sub

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