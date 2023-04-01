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
        Dim SPCResultID As String = Request.QueryString("SPCResultID") & ""
        sGlobal.getMenu("C010 ")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user") & ""
        Dim User As clsUserSetup = clsUserSetupDB.GetData(pUser)

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "C010")
        grid.SettingsDataSecurity.AllowInsert = AuthUpdate
        grid.SettingsDataSecurity.AllowEdit = AuthUpdate
        If User Is Nothing Or AuthUpdate = False Then
            btnMK.Enabled = False
            btnQC.Enabled = False
        Else
            btnMK.Enabled = User.JobPosition = "MK"
            btnQC.Enabled = User.JobPosition = "QC"
        End If

        btnSubmit.Enabled = AuthUpdate


        show_error(MsgTypeEnum.Info, "", 0)
        Dim FactoryCode As String = ""
        Dim ItemTypeCode As String = ""
        Dim Line As String = ""
        Dim ProcessGroup As String = ""
        Dim LineGroup As String = ""
        Dim ProcessCode As String = ""
        Dim ItemCheckCode As String = ""
        Dim ProdDate As String = ""
        Dim Shift As String = ""
        Dim Sequence As String = ""
        If Not IsPostBack And Not IsCallback Then
            up_FillCombo()
            If GlobalPrm <> "" Then
                dtDate.Value = CDate(Request.QueryString("ProdDate"))
                FactoryCode = Request.QueryString("FactoryCode")
                ItemTypeCode = Request.QueryString("ItemTypeCode")
                Line = Request.QueryString("Line")

                Dim Ln As ClsLine = ClsLineDB.GetData(FactoryCode, Line)
                If Ln IsNot Nothing Then
                    ProcessCode = Ln.ProcessCode
                    LineGroup = Ln.LineGroup
                    ProcessGroup = Ln.ProcessGroup
                End If
                ItemCheckCode = Request.QueryString("ItemCheckCode")
                ProdDate = Request.QueryString("ProdDate")
                Shift = Request.QueryString("Shift")
                Sequence = Request.QueryString("Sequence")

                InitCombo(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence, ProcessGroup, LineGroup, ProcessCode)
                GridLoad(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, dtDate.Value, cboShift.Value, cboSeq.Value)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "GridLoad();", True)
            Else
                pUser = Session("user") & ""
                If SPCResultID <> "" Then
                    Dim Result As clsSPCResult = clsSPCResultDB.GetData(SPCResultID)
                    If Result IsNot Nothing Then
                        Dim Ln As ClsLine = ClsLineDB.GetData(Result.FactoryCode, Result.LineCode)
                        If Ln IsNot Nothing Then
                            ProcessCode = Ln.ProcessCode
                            LineGroup = Ln.LineGroup
                            ProcessGroup = Ln.ProcessGroup
                        End If
                        InitCombo(Result.FactoryCode, Result.ItemTypeCode, Result.LineCode, Result.ItemCheckCode, Result.ProdDate, Result.ShiftCode, Result.SequenceNo, ProcessGroup, LineGroup, ProcessCode)
                        GridLoad(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, dtDate.Value, cboShift.Value, cboSeq.Value)
                        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "GridLoad();", True)
                        Exit Sub
                    End If
                End If
                dtDate.Value = "2023-02-07"
                If pUser <> "" Then
                    If User IsNot Nothing Then
                        FactoryCode = User.FactoryCode
                        ProdDate = Session("C01ProdDate") & ""
                        ProcessGroup = Session("C01ProcessGroup") & ""
                        LineGroup = Session("C01LineGroup") & ""
                        ProcessCode = Session("C01ProcessCode") & ""
                        ItemTypeCode = Session("C01ItemTypeCode") & ""
                        Line = Session("C01LineCode") & ""
                        ItemCheckCode = Session("C01ItemCheckCode") & ""
                        Shift = Session("C01ShiftCode") & ""
                        Sequence = Session("C01Sequence") & ""
                        If ProcessGroup <> "" Then
                            InitCombo(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence, ProcessGroup, LineGroup, ProcessCode)
                        Else
                            DefaultCombo(User.FactoryCode)
                        End If
                        GridLoad(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, dtDate.Value, cboShift.Value, cboSeq.Value)
                        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "GridLoad();", True)
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

    Private Sub GridLoadFTA(FactoryCode As String, ItemTypeCode As String, ItemCheckCode As String)
        Dim ErrMsg As String = ""
        Dim dt As DataTable = clsFTAResultDB.GetFTAMaster(FactoryCode, ItemTypeCode, ItemCheckCode)
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
        Dim User As clsUserSetup = clsUserSetupDB.GetData(pUser)
        If User IsNot Nothing Then
            grid.JSProperties("cpJobPosition") = User.JobPosition
        Else
            grid.JSProperties("cpJobPosition") = ""
        End If
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
            btnMK.ClientEnabled = False
            btnQC.ClientEnabled = False
            btnSubmit.ClientEnabled = False
            txtRemark.Text = ""
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
            If FTA.MKVerificationStatus = "1" Then
                btnMK.ClientEnabled = False
            End If
            If FTA.QCVerificationStatus = "1" Then
                btnQC.ClientEnabled = False
            End If
            txtRemark.Text = FTA.Remark
            btnSubmit.ClientEnabled = True
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
                    clsFTAResultDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, "1", pRemark, pUser)

                    Dim n As Integer = hfID.Count
                    For Each item In hfID
                        Dim i As String = item.Key
                        Dim FTAID As String = item.Value
                        Dim pDetSeqNo As Integer = i + 1
                        Dim pResult As String
                        Dim pAction As String = hfAct.Item(i)

                        Dim valueOK = hfOK.Item(i)
                        Dim valueNG = hfNG.Item(i)

                        If valueOK = True Then
                            pResult = "1"
                        ElseIf valueNG = True Then
                            pResult = "2"
                        Else
                            pResult = "0"
                        End If
                        clsFTAResultDetailDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pRemark, FTAID, pDetSeqNo, pAction, pResult, pUser)
                    Next


                    'For Each item In hfOK
                    '    Dim i As String = item.Key
                    '    Dim value As Boolean
                    '    Dim pResult As String = ""
                    '    Dim pIndex As Integer = CInt(Mid(item.Key, 3, 2).Trim)

                    '    Dim FTAID As String = hfID.Item("FTAID" + pIndex)

                    '    If i.StartsWith("OK") Then
                    '        value = item.Value
                    '        If value = True Then
                    '            pResult = "1"
                    '        End If
                    '    ElseIf i.StartsWith("NG") Then
                    '        value = item.Value
                    '        If value = True Then
                    '            pResult = "2"
                    '        End If
                    '    ElseIf i.StartsWith("No") Then
                    '        value = item.Value
                    '        If value = True Then
                    '            pResult = "0"
                    '        End If
                    '    End If
                    '    If pResult <> "" Then
                    '        Dim pAction As String = ""
                    '        Dim pDetSeqNo As Integer = pIndex + 1
                    '        clsFTAResultDetailDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pRemark, FTAID, pDetSeqNo, pAction, pResult, pUser)
                    '    End If
                    'Next item
                    hfID.Clear()
                    hfOK.Clear()
                    hfNG.Clear()
                    hfNo.Clear()
                    hfAct.Clear()
                    show_error(MsgTypeEnum.Success, "Update data successful!", 1)
                ElseIf pFunction = "mkverify" Or pFunction = "qcverify" Then
                    Dim JobPos As String
                    If pFunction = "mkverify" Then
                        JobPos = "MK"
                    Else
                        JobPos = "QC"
                    End If
                    Dim i As Integer = clsFTAResultDB.Verify(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pUser, JobPos)
                    If i = 0 Then
                        show_error(MsgTypeEnum.Warning, "You do not have privilege to verify!", 1)
                    Else
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
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpEdit('" + FTAID + "', '" + No + "', " + i + ");}"
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
        GridLoadFTA(pFactory, pItemType, pItemCheck)
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
            Dim q As String = "function(s, e) { " +
                "chkNG" + i + ".SetChecked(false); " +
                "if(!chkOK" + i + ".GetChecked() & !chkNG" + i + ".GetChecked()) {chkNo" + i + ".SetChecked(true);} else {chkNo" + i + ".SetChecked(false); } " +
                "hfOK.Set('" + i + "', s.GetChecked()); " +
                "hfNG.Set('" + i + "', false); " +
                "hfNo.Set('" + i + "', false); " +
                "hfID.Set('" + i + "', '" + FTAID + "'); " +
                "hfAct.Set('" + i + "', ''); "

            q = q + "}"
            chkOK.ClientSideEvents.CheckedChanged = q
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
                "if(!chkOK" + i + ".GetChecked() & !chkNG" + i + ".GetChecked()) {chkNo" + i + ".SetChecked(true);} else {chkNo" + i + ".SetChecked(false); } " +
                "hfNG.Set('" + i + "', s.GetChecked()); " +
                "hfOK.Set('" + i + "', false); " +
                "hfNo.Set('" + i + "', false); " +
                "hfID.Set('" + i + "', '" + FTAID + "'); " +
                "hfAct.Set('" + i + "', ''); " +
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
                "hfNo.Set('" + i + "', s.GetChecked()); " +
                "hfOK.Set('" + i + "', false); " +
                "hfNG.Set('" + i + "', false); " +
                "hfID.Set('" + i + "', '" + FTAID + "'); " +
                "hfAct.Set('" + i + "', ''); " +
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

    Private Sub ValidateSave()
        cbkValid.JSProperties("cpErrMsg") = ""
        Dim n As Integer = hfID.Count
        Dim NGCount As Integer = 0
        For Each item In hfID
            Dim i As String = item.Key
            Dim FTAID As String = item.Value
            Dim pDetSeqNo As Integer = i + 1
            Dim pAction As String = hfAct.Item(i) + ""

            Dim valueOK = hfOK.Item(i)
            Dim valueNG = hfNG.Item(i)

            If valueNG Then
                If pAction = "" Then
                    cbkValid.JSProperties("cpErrMsg") = "Please input action for Sequence " + pDetSeqNo.ToString
                    Exit For
                End If
                NGCount = NGCount + 1
            End If
        Next
    End Sub

    Private Sub gridEdit_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles gridEdit.CustomCallback
        Dim pFunction As String = Split(e.Parameters, "|")(0)
        Dim FTAID As String = Split(e.Parameters, "|")(1)
        If pFunction = "load" Then
            GridLoadEdit(FTAID)
            gridEdit.JSProperties("cpUpdate") = ""
        ElseIf pFunction = "save" Then
            Dim pFactory As String = Split(e.Parameters, "|")(2)
            Dim pItemType As String = Split(e.Parameters, "|")(3)
            Dim pLine As String = Split(e.Parameters, "|")(4)
            Dim pItemCheck As String = Split(e.Parameters, "|")(5)
            Dim pDate As String = Split(e.Parameters, "|")(6)
            Dim pShift As String = Split(e.Parameters, "|")(7)
            Dim pSeq As String = Split(e.Parameters, "|")(8)
            Dim pRemark As String = Split(e.Parameters, "|")(9)
            Dim pAction As String = Split(e.Parameters, "|")(10)
            Dim pResult As String = Split(e.Parameters, "|")(11)
            Dim pDetSeqNo As Integer = Split(e.Parameters, "|")(12)
            pUser = Session("user")

            clsFTAResultDetailDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pRemark, FTAID, pDetSeqNo, pAction, pResult, pUser)
            GridEditShowMsg(MsgTypeEnum.Success, "Update data successful!", 1)
            gridEdit.JSProperties("cpUpdate") = "1"
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
        Dim pDetSeqNo As Integer = hfDetail.Get("DetSeqNo")
        Dim pFTAID As String = hfDetail.Get("FTAID")
        Dim pRemark As String = txtRemark.Value
        Dim n As Integer = e.UpdateValues.Count
        For i = 0 To e.UpdateValues.Count - 1
            Dim pResult As String = "2"
            Dim pAction As String = e.UpdateValues(i).NewValues("ActionName")
            hfID.Set(pDetSeqNo - 1, pFTAID)
            hfOK.Set(pDetSeqNo - 1, False)
            hfNG.Set(pDetSeqNo - 1, True)
            hfAct.Set(pDetSeqNo - 1, pAction)
            clsFTAResultDetailDB.Insert(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pRemark, pFTAID, pDetSeqNo, pAction, pResult, pUser)
        Next
        gridEdit.JSProperties("cpUpdate") = "1"
        e.Handled = True
    End Sub

    Private Sub gridEdit_RowUpdating(sender As Object, e As ASPxDataUpdatingEventArgs) Handles gridEdit.RowUpdating
        e.Cancel = True
    End Sub

    Private Sub gridEdit_DataBound(sender As Object, e As EventArgs) Handles gridEdit.DataBound
        gridEdit.Columns("FTAID").Visible = False
    End Sub

    Private Sub cbkValid_Callback(source As Object, e As CallbackEventArgs) Handles cbkValid.Callback
        ValidateSave()
    End Sub
End Class