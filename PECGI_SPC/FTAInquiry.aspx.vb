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
        Public Property ProdDate2 As String
        Public Property MKVerification As String
        Public Property QCVerification As String
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

        cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, Line, True)
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

                    cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, Line, True)
                    cboItemCheck.DataBind()
                    If cboItemCheck.Items.Count = 1 Then
                        cboItemCheck.SelectedIndex = 0
                        ItemCheckCode = cboItemCheck.Value
                    Else
                        cboItemCheck.SelectedIndex = 0
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
        If cboLine.Items.Count = 1 Then
            cboLine.SelectedIndex = 0
        End If
    End Sub

    Private Sub GridTitle(ByVal pExl As ExcelWorksheet, cls As clsHeader)
        With pExl
            Try
                .Cells(1, 1).Value = "Corrective Action Inquiry"
                .Cells(1, 1, 1, 13).Merge = True
                .Cells(1, 1, 1, 13).Style.HorizontalAlignment = HorzAlignment.Near
                .Cells(1, 1, 1, 13).Style.VerticalAlignment = VertAlignment.Center
                .Cells(1, 1, 1, 13).Style.Font.Bold = True
                .Cells(1, 1, 1, 13).Style.Font.Size = 16
                .Cells(1, 1, 5, 13).Style.Font.Name = "Segoe UI"

                .Cells(3, 1, 3, 2).Value = "Factory"
                .Cells(3, 1, 3, 2).Merge = True
                .Cells(3, 3).Value = ": " & cls.FactoryName

                .Cells(4, 1, 4, 2).Value = "Process Group"
                .Cells(4, 1, 4, 2).Merge = True
                .Cells(4, 3).Value = ": " & cls.ProcessGroup

                .Cells(5, 1, 5, 2).Value = "Line Group"
                .Cells(5, 1, 5, 2).Merge = True
                .Cells(5, 3).Value = ": " & cls.LineGroup

                .Cells(3, 5).Value = "Machine"
                .Cells(3, 6).Value = ": " & cls.ProcessCode

                .Cells(4, 5).Value = "Machine Process"
                .Cells(4, 6).Value = ": " & cls.LineName

                .Cells(5, 5).Value = "Type"
                .Cells(5, 6).Value = ": " & cls.ItemTypeName

                .Cells(3, 9, 3, 10).Value = "Item Check"
                .Cells(3, 9, 3, 10).Merge = True
                .Cells(3, 11).Value = ": " & cls.ItemCheckName

                .Cells(4, 9, 4, 10).Value = "Prod Date"
                .Cells(4, 9, 4, 10).Merge = True
                .Cells(4, 11).Value = ": " & cls.ProdDate & " to " & cls.ProdDate2

                .Cells(5, 9, 5, 10).Value = "MK Verification"
                .Cells(5, 9, 5, 10).Merge = True
                .Cells(5, 11).Value = ": " & cls.MKVerification

                .Cells(6, 9, 6, 10).Value = "QC Verification"
                .Cells(6, 9, 6, 10).Merge = True
                .Cells(6, 11).Value = ": " & cls.QCVerification

                Dim rgHdr As ExcelRange = .Cells(3, 1, 6, 12)
                rgHdr.Style.HorizontalAlignment = HorzAlignment.Near
                rgHdr.Style.VerticalAlignment = VertAlignment.Center
                rgHdr.Style.Font.Size = 8
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
        cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, LineCode, True)
        cboItemCheck.DataBind()
        cboItemCheck.SelectedIndex = 0
    End Sub

    Protected Sub grid_BatchUpdate(sender As Object, e As ASPxDataBatchUpdateEventArgs)
        If e.UpdateValues.Count > 0 Then

        End If
    End Sub

    Private Sub DownloadExcel()
        Dim dt As DataTable = clsFTAResultDB.GetInquiry(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), Format(dtTo.Value, "yyyy-MM-dd"), cboMK.Value, cboQC.Value)
        Dim iRow As Integer
        Dim j As Integer
        Dim nRow As Integer = dt.Rows.Count
        Using Pck As New ExcelPackage
            Dim ws As ExcelWorksheet = Pck.Workbook.Worksheets.Add("Sheet1")
            With ws
                .Cells(1, 1, 2, 1).Value = "Prod Date"
                .Cells(1, 2, 2, 2).Value = "Shift"
                .Cells(1, 3, 2, 3).Value = "Sequence"
                .Cells(1, 4, 2, 4).Value = "Item"
                .Cells(1, 5, 2, 5).Value = "Action"
                .Cells(1, 6, 1, 7).Value = "MK Verification"
                .Cells(1, 6, 1, 7).Merge = True
                .Cells(2, 6).Value = "PIC"
                .Cells(2, 7).Value = "Date"
                .Cells(1, 8, 1, 9).Value = "QC Verification"
                .Cells(1, 8, 1, 9).Merge = True
                .Cells(2, 8).Value = "PIC"
                .Cells(2, 9).Value = "Date"
                .Cells(1, 10, 2, 12).Value = "Remark"
                .Column(1).Width = 12
                .Column(4).Width = 35
                .Column(5).Width = 27
                .Column(7).Width = 12
                .Column(9).Width = 12
                For iCol = 1 To 5
                    .Cells(1, iCol, 2, iCol).Merge = True
                Next
                .Cells(1, 10, 2, 12).Merge = True
                .Cells(3, 1, nRow + 2, 1).Style.Numberformat.Format = "dd MMM yyyy"
                .Cells(3, 7, nRow + 2, 7).Style.Numberformat.Format = "dd MMM yyyy"
                .Cells(3, 9, nRow + 2, 9).Style.Numberformat.Format = "dd MMM yyyy"

                .Cells(3, 1, nRow + 2, 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                .Cells(3, 3, nRow + 2, 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                .Cells(3, 7, nRow + 2, 7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                .Cells(3, 9, nRow + 2, 9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                For iRow = 0 To nRow - 1
                    j = iRow + 3
                    .Cells(j, 1).Value = dt.Rows(iRow)("ProdDate")
                    .Cells(j, 2).Value = dt.Rows(iRow)("ShiftCode")
                    .Cells(j, 3).Value = dt.Rows(iRow)("SequenceNo")
                    .Cells(j, 4).Value = dt.Rows(iRow)("ItemCheck")
                    .Cells(j, 5).Value = dt.Rows(iRow)("ActionName")
                    .Cells(j, 6).Value = dt.Rows(iRow)("MKVerificationUser")
                    .Cells(j, 7).Value = dt.Rows(iRow)("MKVerificationDate")
                    .Cells(j, 8).Value = dt.Rows(iRow)("QCVerificationUser")
                    .Cells(j, 9).Value = dt.Rows(iRow)("QCVerificationDate")
                    .Cells(j, 10).Value = dt.Rows(iRow)("Remark")
                    .Cells(j, 10, j, 12).Merge = True
                Next
                ExcelHeader(ws, 1, 1, 2, 12)
                ExcelBorder(ws, 1, 1, nRow + 2, 12)
                ExcelFont(ws, 1, 1, nRow + 2, 12, 8)

                .InsertRow(1, 6)

                Dim Hdr As New clsHeader
                Hdr.FactoryCode = cboFactory.Value
                Hdr.FactoryName = cboFactory.Text
                Hdr.ItemTypeCode = cboType.Value
                Hdr.ItemTypeName = cboType.Text
                Hdr.LineCode = cboLine.Value
                Hdr.LineName = cboLine.Text
                Hdr.ItemCheckCode = cboItemCheck.Value
                Hdr.ItemCheckName = cboItemCheck.Text
                Hdr.ProdDate = Convert.ToDateTime(dtDate.Value).ToString("dd MMM yyyy")
                Hdr.ProdDate2 = Convert.ToDateTime(dtTo.Value).ToString("dd MMM yyyy")
                Hdr.MKVerification = cboMK.Text
                Hdr.QCVerification = cboQC.Text
                Hdr.ProcessGroup = cboProcessGroup.Text
                Hdr.LineGroup = cboLineGroup.Text
                Hdr.ProcessCode = cboProcess.Text
                GridTitle(ws, Hdr)
            End With

            Dim stream As MemoryStream = New MemoryStream(Pck.GetAsByteArray())
            Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            Response.AppendHeader("Content-Disposition", "attachment; filename=FTAInquiry_" & Format(Date.Now, "yyyy-MM-dd") & ".xlsx")
            Response.BinaryWrite(stream.ToArray())
            Response.End()

        End Using
    End Sub

    Private Sub ExcelHeader(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer)
        With Exl
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#878787"))
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Font.Color.SetColor(Color.White)
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.VerticalAlignment = ExcelVerticalAlignment.Center
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
        If cboProcess.Items.Count = 1 Then
            cboProcess.SelectedIndex = 0
        End If
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        DownloadExcel()
    End Sub
End Class