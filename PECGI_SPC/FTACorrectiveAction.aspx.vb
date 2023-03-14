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

Public Class FTACorrectiveAction
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

    Private Sub GridEditShowMsg(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        gridEdit.JSProperties("cp_message") = ErrMsg
        gridEdit.JSProperties("cp_type") = msgType
        gridEdit.JSProperties("cp_val") = pVal
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GlobalPrm = Request.QueryString("FactoryCode") & ""
        sGlobal.getMenu("C010 ")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user") & ""
        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "C010")
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
                dtDate.Value = "2023-02-07"
                If pUser <> "" Then
                    Dim User As clsUserSetup = clsUserSetupDB.GetData(pUser)
                    If User IsNot Nothing Then
                        Dim FactoryCode As String = User.FactoryCode
                        Dim ProdDate As String = Session("C01ProdDate") & ""
                        Dim ProcessGroup As String = Session("C01ProcessGroup") & ""
                        Dim LineGroup As String = Session("C01LineGroup") & ""
                        Dim ProcessCode As String = Session("C01ProcessCode") & ""
                        Dim ItemTypeCode As String = Session("C01ItemTypeCode") & ""
                        Dim LineCode As String = Session("C01LineCode") & ""
                        Dim ItemCheckCode As String = Session("C01ItemCheckCode") & ""
                        Dim ShiftCode As String = Session("C01ShiftCode") & ""
                        Dim Sequence As String = Session("C01Sequence") & ""
                        If ProcessGroup <> "" Then
                            InitCombo(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, ShiftCode, Sequence, ProcessGroup, LineGroup, ProcessCode)
                        Else
                            DefaultCombo(User.FactoryCode)
                        End If
                        GridLoad(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, dtDate.Value, cboShift.Value, cboSeq.Value)
                    End If
                End If
                btnMK.ClientEnabled = False
                btnQC.ClientEnabled = False
            End If
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

        cboShift.DataSource = clsFrequencyDB.GetShift(FactoryCode, ItemTypeCode, Line, ItemCheckCode)
        cboShift.DataBind()
        cboShift.Value = ShiftCode

        cboSeq.DataSource = clsFrequencyDB.GetSequence(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ShiftCode)
        cboSeq.DataBind()
        cboSeq.Value = Sequence
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

                        cboShift.DataSource = clsFrequencyDB.GetShift(FactoryCode, ItemTypeCode, Line, ItemCheckCode)
                        cboShift.DataBind()
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub up_FillCombo()
        cboFactory.DataSource = clsFactoryDB.GetList
        cboFactory.DataBind()
    End Sub

    Private Sub up_ClearJS()
        grid.JSProperties("cpUSL") = " "
        grid.JSProperties("cpLSL") = " "
        grid.JSProperties("cpUCL") = " "
        grid.JSProperties("cpCL") = " "
        grid.JSProperties("cpLCL") = " "
        grid.JSProperties("cpRUCL") = " "
        grid.JSProperties("cpMin") = ""
        grid.JSProperties("cpMax") = ""
        grid.JSProperties("cpAve") = ""
        grid.JSProperties("cpR") = ""
        grid.JSProperties("cpRNG") = ""
        grid.JSProperties("cpC") = " "
        grid.JSProperties("cpNG") = " "
        grid.JSProperties("cpMKUser") = " "
        grid.JSProperties("cpMKDate") = " "
        grid.JSProperties("cpQCUser") = " "
        grid.JSProperties("cpQCDate") = " "
        grid.JSProperties("cpSubLotNo") = ""
        grid.JSProperties("cpRemarks") = ""
        grid.JSProperties("cpRefresh") = ""
    End Sub

    Private Sub up_ClearGrid()
        grid.DataSource = Nothing
        grid.DataBind()
        up_ClearJS()
    End Sub
    Protected Sub grid_AfterPerformCallback(sender As Object, e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
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
            Hdr.ShiftCode = cboShift.Value
            Hdr.Shiftname = cboShift.Text
            Hdr.Seq = cboSeq.Value
            GridLoad(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), cboShift.Value, cboSeq.Value)
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

    Private Sub GridLoadFTA(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String)
        Dim ErrMsg As String = ""
        Dim dt As DataTable = clsFTAResultDB.GetFTAMaster(FactoryCode, ItemTypeCode, Line, ItemCheckCode)
        gridFTA.DataSource = dt
        gridFTA.DataBind()
    End Sub

    Private Sub GridLoadAction(FTAID As String)
        Dim ErrMsg As String = ""
        Dim dt As DataTable = clsFTAResultDB.GetFTAAction(FTAID)
        gridAction.DataSource = dt
        gridAction.DataBind()
    End Sub

    Private Sub GridLoadEdit(FTAID As String)
        Dim ErrMsg As String = ""
        Dim dt As DataTable = clsFTAResultDB.GetFTAAction(FTAID, True)
        gridEdit.DataSource = dt
        gridEdit.DataBind()
    End Sub

    Private Sub GridLoad(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer)
        Dim ErrMsg As String = ""
        'Dim dt As DataTable = clsFTAResultDB.GetTable(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence)

        Dim FTAList As List(Of clsFTAResult) = clsFTAResultDB.GetList(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence)
        grid.DataSource = FTAList
        grid.DataBind()
        If FTAList.Count = 0 Then
            grid.JSProperties("cpCount") = ""
            grid.JSProperties("cpRemark") = ""
            grid.JSProperties("cpMKVerificationStatus") = ""
            grid.JSProperties("cpMKVerificationDate") = ""
            grid.JSProperties("cpMKVerificationUser") = ""
            grid.JSProperties("cpQCVerificationStatus") = ""
            grid.JSProperties("cpQCVerificationDate") = ""
            grid.JSProperties("cpQCVerificationUser") = ""
        Else
            Dim FTA As clsFTAResult = FTAList.Item(0)
            grid.JSProperties("cpCount") = FTAList.Count
            grid.JSProperties("cpRemark") = FTA.Remark
            grid.JSProperties("cpMKVerificationStatus") = FTA.MKVerificationStatus
            grid.JSProperties("cpMKVerificationDate") = FTA.MKVerificationDate
            grid.JSProperties("cpMKVerificationUser") = FTA.MKVerificationUser
            grid.JSProperties("cpQCVerificationStatus") = FTA.QCVerificationStatus
            grid.JSProperties("cpQCVerificationDate") = FTA.QCVerificationDate
            grid.JSProperties("cpQCVerificationUser") = FTA.QCVerificationUser
        End If
        Dim UserID As String = Session("user")
        Session("C01USer") = UserID
        Session("C01ProdDate") = ProdDate
        Session("C01ProcessGroup") = cboProcessGroup.Value
        Session("C01LineGroup") = cboLineGroup.Value
        Session("C01ProcessCode") = cboProcess.Value
        Session("C01ItemTypeCode") = ItemTypeCode
        Session("C01LineCode") = Line
        Session("C01ItemCheckCode") = ItemCheckCode
        Session("C01ShiftCode") = Shift
        Session("C01Sequence") = Sequence
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
            Case "load", "save", "mkverify", "qcverify"
                Dim pFactory As String = Split(e.Parameters, "|")(1)
                Dim pItemType As String = Split(e.Parameters, "|")(2)
                Dim pLine As String = Split(e.Parameters, "|")(3)
                Dim pItemCheck As String = Split(e.Parameters, "|")(4)
                Dim pDate As String = Split(e.Parameters, "|")(5)
                Dim pShift As String = Split(e.Parameters, "|")(6)
                Dim pSeq As String = Split(e.Parameters, "|")(7)
                pSeq = Val(pSeq)
                If pFunction = "save" Then
                    Dim pRemark As String = Split(e.Parameters, "|")(8)
                    pUser = Session("user") & ""
                    Dim n As Integer = hf.Count
                    For Each item In hf
                        Dim i As String = item.Key
                        Dim value As Boolean = item.Value
                        Dim pResult As String = ""
                        If i.StartsWith("OK") Then
                            If value = True Then
                                pResult = "1"
                            End If
                        ElseIf i.StartsWith("NG") Then
                            If value = True Then
                                pResult = "2"
                            End If
                        ElseIf i.StartsWith("No") Then
                            If value = True Then
                                pResult = "0"
                            End If
                        End If
                        If pResult <> "" Then
                            clsFTAResultDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pResult, pRemark, pUser)
                        End If
                    Next item
                    hf.Clear()
                    show_error(MsgTypeEnum.Success, "Update data successful!", 1)
                ElseIf pFunction = "mkverify" Or pFunction = "qcverify" Then
                    Dim i As Integer = clsFTAResultDB.Verify(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pUser)
                    Dim JobPos As String
                    If i = 0 Then
                        show_error(MsgTypeEnum.Warning, "You Do Not have privilege To Verify!", 1)
                    Else
                        If pFunction = "mkverify" Then
                            JobPos = "MK"
                        Else
                            JobPos = "QC"
                        End If
                        show_error(MsgTypeEnum.Success, JobPos & " Verification successful!", 1)
                    End If
                End If
                GridLoad(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq)
        End Select
    End Sub

    Private Sub cbkRefresh_Callback(source As Object, e As CallbackEventArgs) Handles cbkRefresh.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim LineCode As String = Split(e.Parameter, "|")(2)
        Dim ItemCheckCode As String = Split(e.Parameter, "|")(3)
        Dim ProdDate As String = Split(e.Parameter, "|")(4)

        Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate)
        If Setup Is Nothing Then
            cbkRefresh.JSProperties("cpUSL") = " "
            cbkRefresh.JSProperties("cpLSL") = " "
            cbkRefresh.JSProperties("cpUCL") = " "
            cbkRefresh.JSProperties("cpCL") = " "
            cbkRefresh.JSProperties("cpLCL") = " "
            cbkRefresh.JSProperties("cpRUCL") = " "
        Else
            cbkRefresh.JSProperties("cpUSL") = Setup.SpecUSL
            cbkRefresh.JSProperties("cpLSL") = Setup.SpecLSL
            cbkRefresh.JSProperties("cpUCL") = Setup.CPUCL
            cbkRefresh.JSProperties("cpCL") = Setup.CPCL
            cbkRefresh.JSProperties("cpLCL") = Setup.CPLCL
            cbkRefresh.JSProperties("cpRUCL") = Setup.RUCL
        End If
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
                Hdr.ShiftCode = cboShift.Value
                Hdr.Shiftname = cboShift.Text
                Hdr.Seq = cboSeq.Value
                Hdr.ProcessGroup = cboProcessGroup.Text
                Hdr.LineGroup = cboLineGroup.Text
                Hdr.ProcessCode = cboProcess.Text

                GridTitle(ws, Hdr)
                .InsertRow(LastRow, 22)
            End With

            Dim stream As MemoryStream = New MemoryStream(Pck.GetAsByteArray())
            Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            Response.AppendHeader("Content-Disposition", "attachment; filename=ProdSampleInput_" & Format(Date.Now, "yyyy-MM-dd") & ".xlsx")
            Response.BinaryWrite(stream.ToArray())
            Response.End()

        End Using
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

    Private Sub cboShift_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboShift.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim LineCode As String = Split(e.Parameter, "|")(2)
        Dim ItemCheckCode As String = Split(e.Parameter, "|")(3)
        cboShift.DataSource = clsFrequencyDB.GetShift(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode)
        cboShift.DataBind()
    End Sub

    Private Sub grid_CellEditorInitialize(sender As Object, e As ASPxGridViewEditorEventArgs) Handles grid.CellEditorInitialize
        e.Editor.ReadOnly = True
    End Sub

    Protected Sub IKLink_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim container As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim FTAID As String = ""
        FTAID = container.Grid.GetRowValues(container.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpIK('" + FTAID + "');}"
        End If
    End Sub



    Protected Sub IKLink2_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim container As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim FTAID As String = ""
        FTAID = container.Grid.GetRowValues(container.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpIK('" + FTAID + "');}"
        End If
    End Sub

    Protected Sub ActionLink_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim container As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim FTAID As String = ""
        FTAID = container.Grid.GetRowValues(container.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpAction('" + FTAID + "');}"
        End If
    End Sub

    Protected Sub EditLink_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim container As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim FTAID As String = container.Grid.GetRowValues(container.VisibleIndex, "FTAID") & ""
        Dim No As String = container.Grid.GetRowValues(container.VisibleIndex, "No") & ""
        If FTAID <> "" Then
            Dim i As String = container.VisibleIndex
            link.ClientInstanceName = String.Format("linkEdit{0}", i)
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpEdit('" + FTAID + "', '" + No + "');}"
        End If
    End Sub

    Private Sub grid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles grid.HtmlDataCellPrepared
        e.Cell.Attributes.Add("onclick", "event.cancelBubble = true")
    End Sub

    Private Sub cboSeq_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboSeq.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim LineCode As String = Split(e.Parameter, "|")(2)
        Dim ItemCheckCode As String = Split(e.Parameter, "|")(3)
        Dim ShiftCode As String = Split(e.Parameter, "|")(4)
        cboSeq.DataSource = clsFrequencyDB.GetSequence(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ShiftCode)
        cboSeq.DataBind()
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

    Private Sub gridFTA_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles gridFTA.CustomCallback
        Dim pFunction As String = Split(e.Parameters, "|")(0)
        Dim pFactory As String = Split(e.Parameters, "|")(1)
        Dim pItemType As String = Split(e.Parameters, "|")(2)
        Dim pLine As String = Split(e.Parameters, "|")(3)
        Dim pItemCheck As String = Split(e.Parameters, "|")(4)
        GridLoadFTA(pFactory, pItemType, pLine, pItemCheck)
    End Sub

    Private Sub cbkPanel_Callback(sender As Object, e As CallbackEventArgsBase) Handles cbkPanel.Callback
        Dim s As String = e.Parameter
        ShowIK(s)
    End Sub

    Protected Sub chkOK_Init(sender As Object, e As EventArgs)
        Dim chkOK As ASPxCheckBox = TryCast(sender, ASPxCheckBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(chkOK.NamingContainer, GridViewDataItemTemplateContainer)
        Dim FTAID As String = ""
        FTAID = container.Grid.GetRowValues(container.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            Dim i As String = container.VisibleIndex
            chkOK.ClientInstanceName = String.Format("chkOK{0}", i)
            chkOK.ClientSideEvents.CheckedChanged =
                "function(s, e) { " +
                "chkNG" + i + ".SetChecked(false); " +
                "chkNo" + i + ".SetChecked(false); " +
                "hf.Set('OK" + i + "', s.GetChecked()); " +
                "hf.Set('NG" + i + "', false); " +
                "hf.Set('No" + i + "', false); " +
                "linkEdit" + i + ".SetVisible(false); " +
                "}"
        End If
    End Sub

    Protected Sub chkNG_Init(sender As Object, e As EventArgs)
        Dim chkNG As ASPxCheckBox = TryCast(sender, ASPxCheckBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(chkNG.NamingContainer, GridViewDataItemTemplateContainer)
        Dim FTAID As String = ""
        FTAID = container.Grid.GetRowValues(container.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            Dim i As String = container.VisibleIndex
            chkNG.ClientInstanceName = String.Format("chkNG{0}", i)
            chkNG.ClientSideEvents.CheckedChanged =
                "function(s, e) { " +
                "chkOK" + i + ".SetChecked(false); " +
                "chkNo" + i + ".SetChecked(false); " +
                "hf.Set('NG" + i + "', s.GetChecked()); " +
                "hf.Set('OK" + i + "', false); " +
                "hf.Set('No" + i + "', false); " +
                "linkEdit" + i + ".SetVisible(s.GetChecked()); " +
                "SelectNoCheck(" + i + "); " +
                "}"
        End If
    End Sub

    Protected Sub chkNo_Init(sender As Object, e As EventArgs)
        Dim chkNo As ASPxCheckBox = TryCast(sender, ASPxCheckBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(chkNo.NamingContainer, GridViewDataItemTemplateContainer)
        Dim FTAID As String = ""
        FTAID = container.Grid.GetRowValues(container.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            Dim i As String = container.VisibleIndex
            chkNo.ClientInstanceName = String.Format("chkNo{0}", i)
            chkNo.ClientSideEvents.CheckedChanged =
                "function(s, e) { " +
                "chkOK" + i + ".SetChecked(false); " +
                "chkNG" + i + ".SetChecked(false); " +
                "hf.Set('No" + i + "', s.GetChecked()); " +
                "hf.Set('OK" + i + "', false); " +
                "hf.Set('NG" + i + "', false); " +
                "linkEdit" + i + ".SetVisible(false); " +
                "}"
        End If
    End Sub

    Public Sub ShowIK(FTAID As String)
        Dim Img As Object = clsFTAResultDB.GetIK(FTAID)
        Dim ImageUrl As String = "~/img/noimage.png"
        If Img IsNot Nothing AndAlso Not IsDBNull(Img) Then
            Dim fcByte As Byte() = Nothing
            fcByte = Img
            ImageUrl = "data:image;base64," + Convert.ToBase64String(fcByte)
        End If
        imgIK.ImageUrl = ImageUrl
    End Sub

    Private Sub gridAction_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles gridAction.CustomCallback
        Dim FTAID As String = Split(e.Parameters, "|")(0)
        GridLoadAction(FTAID)
    End Sub

    Private Sub gridEdit_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles gridEdit.CustomCallback
        Dim pFunction As String = Split(e.Parameters, "|")(0)
        Dim FTAID As String = Split(e.Parameters, "|")(1)
        If pFunction = "load" Then
            GridLoadEdit(FTAID)
        ElseIf pFunction = "save" Then
            Dim pFactory As String = Split(e.Parameters, "|")(2)
            Dim pItemType As String = Split(e.Parameters, "|")(3)
            Dim pLine As String = Split(e.Parameters, "|")(4)
            Dim pItemCheck As String = Split(e.Parameters, "|")(5)
            Dim pDate As String = Split(e.Parameters, "|")(6)
            Dim pShift As String = Split(e.Parameters, "|")(7)
            Dim pSeq As String = Split(e.Parameters, "|")(8)
            Dim pRemark As String = Split(e.Parameters, "|")(9)
            Dim pActionID As String = Split(e.Parameters, "|")(10)
            Dim pResult As String = Split(e.Parameters, "|")(11)
            Dim pDetailRemark As String = Split(e.Parameters, "|")(12)
            Dim pDetSeqNo As Integer = Split(e.Parameters, "|")(13)
            pUser = Session("user")

            clsFTAResultDetailDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pRemark, FTAID, pDetSeqNo, pActionID, pResult, pDetailRemark, pUser)
            GridLoad(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq)
            GridEditShowMsg(MsgTypeEnum.Success, "Update data successful!", 1)
        End If
    End Sub

    Private Sub grid_RowUpdating(sender As Object, e As ASPxDataUpdatingEventArgs) Handles grid.RowUpdating
        e.Cancel = True
    End Sub

    Private Sub gridAction_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles gridAction.HtmlDataCellPrepared
        e.Cell.Attributes.Add("onclick", "event.cancelBubble = true")
    End Sub

    Private Sub gridAction_CellEditorInitialize(sender As Object, e As ASPxGridViewEditorEventArgs) Handles gridAction.CellEditorInitialize
        e.Editor.ReadOnly = True
    End Sub

    Private Sub gridEdit_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles gridEdit.HtmlDataCellPrepared
        If e.DataColumn.FieldName <> "Select" Then
            e.Cell.Attributes.Add("onclick", "event.cancelBubble = true")
        End If
    End Sub

    Private Sub gridEdit_CellEditorInitialize(sender As Object, e As ASPxGridViewEditorEventArgs) Handles gridEdit.CellEditorInitialize
        If e.Column.FieldName <> "Select" Then
            e.Editor.ReadOnly = True
        Else
            e.Editor.ReadOnly = False
        End If
    End Sub

    Protected Sub gridEdit_BatchUpdate(sender As Object, e As ASPxDataBatchUpdateEventArgs)
        Dim pFactory As String = cboFactory.Value
        Dim pItemType As String = cboType.Value
        Dim pLine As String = cboLine.Value
        Dim pItemCheck As String = cboItemCheck.Value
        Dim pDate As String = Format(dtDate.Value, "yyyy-MM-dd")
        Dim pShift As String = cboShift.Value
        Dim pSeq As String = cboSeq.Value
        pUser = Session("user") & ""
        Dim DetSeqNo As Integer = hfDetail.Get("DetSeqNo")
        Dim pFTAID As String = hfDetail.Get("FTAID")
        Dim pDetailRemark As String = txtOther.Value
        Dim pRemark As String = txtRemark.Value
        For i = 0 To e.UpdateValues.Count - 1
            Dim pResult As String = "2"
            Dim pActionID As String = e.UpdateValues(i).NewValues("ActionID")
            clsFTAResultDetailDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pRemark, pFTAID, DetSeqNo, pActionID, pResult, pDetailRemark, pUser)
        Next
        e.Handled = True
        GridEditShowMsg(MsgTypeEnum.Success, "Update data successful!", 1)
    End Sub

    Private Sub gridEdit_RowUpdating(sender As Object, e As ASPxDataUpdatingEventArgs) Handles gridEdit.RowUpdating
        e.Cancel = True
    End Sub

    Private Sub gridEdit_DataBound(sender As Object, e As EventArgs) Handles gridEdit.DataBound
        gridEdit.Columns("FTAID").Visible = False
    End Sub
End Class