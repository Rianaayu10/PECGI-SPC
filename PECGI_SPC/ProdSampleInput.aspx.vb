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

Public Class ProdSampleInput
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
    Dim dtDel As DataTable
    Dim LastNG As Integer

    Private Sub GridXLoad(Hdr As clsHeader)
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

            Dim PrevDate As String = clsSPCResultDB.GetPrevDate(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, Hdr.ProdDate)
            If PrevDate = "" Then
                PrevDate = Hdr.ProdDate
            End If
            Dim ds As DataSet = clsSPCResultDetailDB.GetSampleByPeriod(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, PrevDate, Hdr.ProdDate, Hdr.VerifiedOnly, True, True)
            Dim dtDay As DataTable = ds.Tables(0)

            Dim dt2 As DataTable = clsSPCResultDetailDB.GetLastR(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode, PrevDate, 1, Hdr.VerifiedOnly)
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
                Dim SelSeq As Integer = dtDay.Rows(iDay)("SeqNo")
                If SelDay = Hdr.ProdDate And SelSeq > Hdr.Seq Then
                    Exit For
                ElseIf SelShift <> PrevShift Or dDay <> PrevDay Then
                    BandShift = New GridViewBandColumn
                    BandShift.Caption = SelShift
                    BandDay.Columns.Add(BandShift)
                End If

                Dim colTime As New GridViewDataTextColumn
                colTime.Caption = dtDay.Rows(iDay)("RegisterDate")
                colTime.FieldName = dtDay.Rows(iDay)("ColName")
                colTime.Width = 80
                colTime.CellStyle.HorizontalAlign = HorizontalAlign.Center
                BandShift.Columns.Add(colTime)

                PrevDay = dDay
                PrevShift = SelShift
            Next
            dtXR = ds.Tables(1)
            gridX.DataSource = dtXR
            gridX.DataBind()

            ChartType = clsXRChartDB.GetChartType(Hdr.FactoryCode, Hdr.ItemTypeCode, Hdr.LineCode, Hdr.ItemCheckCode)
            If ds.Tables.Count > 2 Then
                dtLSL = ds.Tables(2)
                dtUSL = ds.Tables(3)
                dtLCL = ds.Tables(4)
                dtUCL = ds.Tables(5)
                dtCP = ds.Tables(6)
                dtRUCL = ds.Tables(7)
                dtRLCL = ds.Tables(8)
                dtDel = ds.Tables(9)
            End If
            If ChartType = "0" Then
                .JSProperties("cpShow") = "0"
            Else
                .JSProperties("cpShow") = "1"
            End If
        End With
    End Sub

    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        grid.JSProperties("cp_message") = ErrMsg
        grid.JSProperties("cp_type") = msgType
        grid.JSProperties("cp_val") = pVal
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GlobalPrm = Request.QueryString("FactoryCode") & ""
        Dim SPCResultID As String
        SPCResultID = Request.QueryString("SPCResultID") & ""
        sGlobal.getMenu("B020 ")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user") & ""
        hfUserID.Set("UserID", pUser)
        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "B020 ")
        grid.SettingsDataSecurity.AllowInsert = True
        grid.SettingsDataSecurity.AllowEdit = True
        grid.Columns.Item("Value1").Visible = False
        grid.Columns.Item("Value2").Visible = False

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
                        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "GridLoad();", True)
                        Exit Sub
                    End If
                End If
                dtDate.Value = Now.Date
                If pUser <> "" Then
                    Dim User As clsUserSetup = clsUserSetupDB.GetData(pUser)
                    If User IsNot Nothing Then
                        FactoryCode = User.FactoryCode
                        ProdDate = Session("B02ProdDate") & ""
                        ProcessGroup = Session("B02ProcessGroup") & ""
                        LineGroup = Session("B02LineGroup") & ""
                        ProcessCode = Session("B02ProcessCode") & ""
                        ItemTypeCode = Session("B02ItemTypeCode") & ""
                        Line = Session("B02LineCode") & ""
                        ItemCheckCode = Session("B02ItemCheckCode") & ""
                        Shift = Session("B02ShiftCode") & ""
                        Sequence = Session("B02Sequence") & ""
                        If ProcessGroup <> "" Then
                            InitCombo(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence, ProcessGroup, LineGroup, ProcessCode)
                        Else
                            DefaultCombo(User.FactoryCode)
                        End If
                    End If
                End If
                'InitCombo(User.FactoryCode, "TPMSBR011", "015", "IC021", "2022-08-04", "SH001", 1)
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

    Protected Sub grid_RowInserting(sender As Object, e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles grid.RowInserting
        e.Cancel = True
        Dim Result As New clsSPCResult
        Result.FactoryCode = cboFactory.Value
        Result.ItemCheckCode = cboItemCheck.Value
        Result.ItemTypeCode = cboType.Value
        Result.LineCode = cboLine.Value
        Result.ProdDate = dtDate.Value
        Result.ShiftCode = cboShift.Value
        Result.SequenceNo = cboSeq.Value
        Result.SubLotNo = txtSubLotNo.Text
        Result.Remark = txtRemarks.Text
        Result.RegisterUser = Session("user") & ""

        Dim SetupFound As Boolean = ValidateChartSetup(Result.FactoryCode, Result.ItemTypeCode, Result.LineCode, Result.ItemCheckCode, Format(Result.ProdDate, "yyyy-MM-dd"))
        If SetupFound = False Then
            grid.CancelEdit()
            show_error(MsgTypeEnum.Warning, "Chart Setup has not been set for this item", 1)
            Return
        End If
        clsSPCResultDB.Insert(Result)

        Dim SeqNo As Integer = clsSPCResultDetailDB.GetSeqNo(Result.FactoryCode, Result.ItemTypeCode, Result.LineCode, Result.ItemCheckCode, Format(Result.ProdDate, "yyyy-MM-dd"), Result.ShiftCode, Result.SequenceNo)
        Dim Detail As New clsSPCResultDetail
        Detail.SPCResultID = Result.SPCResultID
        Detail.SequenceNo = SeqNo
        Detail.DeleteStatus = e.NewValues("DeleteStatus")
        Detail.Value = e.NewValues("Value")
        Detail.Remark = e.NewValues("Remark")
        Detail.RegisterUser = Result.RegisterUser
        clsSPCResultDetailDB.Insert(Detail)
        grid.CancelEdit()

        show_error(MsgTypeEnum.Success, "Update data successfull!", 1)
    End Sub

    Protected Sub grid_RowDeleting(sender As Object, e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles grid.RowDeleting
        e.Cancel = True
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
        grid.JSProperties("cpNoProd") = ""
        grid.JSProperties("cpRefresh") = ""
    End Sub

    Private Sub up_ClearGrid()
        grid.DataSource = Nothing
        grid.DataBind()
        up_ClearJS()
    End Sub
    Protected Sub grid_AfterPerformCallback(sender As Object, e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles grid.AfterPerformCallback
        If e.CallbackName = "CANCELEDIT" Then
            Dim Item As clsItemCheck = clsItemCheckDB.GetData(cboItemCheck.Value)
            If Item Is Nothing OrElse Item.Measure2Cls <> "1" Then
                grid.Columns.Item("Value1").Visible = False
                grid.Columns.Item("Value2").Visible = False
            Else
                grid.Columns.Item("Value1").Visible = True
                grid.Columns.Item("Value2").Visible = True
            End If
        ElseIf e.CallbackName <> "CANCELEDIT" And e.CallbackName <> "CUSTOMCALLBACK" Then
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
            GridLoad(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), cboShift.Value, cboSeq.Value, cboShow.Value)
            GridXLoad(Hdr)
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

    Private Sub GridLoad(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer, VerifiedOnly As Integer)
        Dim ErrMsg As String = ""
        Dim dt As DataTable = clsSPCResultDetailDB.GetTable(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence, VerifiedOnly)
        grid.DataSource = dt
        grid.DataBind()

        Dim Result As clsSPCResult = clsSPCResultDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Shift, Sequence)

        Dim Item As clsItemCheck = clsItemCheckDB.GetData(ItemCheckCode)
        If Item Is Nothing OrElse Item.Measure2Cls <> "1" Then
            grid.Columns.Item("Value1").Visible = False
            grid.Columns.Item("Value2").Visible = False
        Else
            grid.Columns.Item("Value1").Visible = True
            grid.Columns.Item("Value2").Visible = True
        End If

        Dim setupfound As Boolean = ValidateChartSetup(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)

        Dim dt2 As DataTable = clsSPCResultDetailDB.GetLastR(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Sequence, VerifiedOnly)
        If dt2.Rows.Count > 0 Then
            LastNG = dt2.Rows(0)("NG")
        Else
            LastNG = 0
        End If

        Dim UserID As String = Session("user")
        Dim pEmplooyeeID = clsIOT.GetEmployeeID(UserID)
        Dim AllowSkill As Boolean = clsIOT.AllowSkill(pEmplooyeeID, FactoryCode, Line, ItemTypeCode)
        If My.Computer.Name = "TOS56-ARI" Then
            AllowSkill = True
        End If
        ChartType = clsXRChartDB.GetChartType(FactoryCode, ItemTypeCode, Line, ItemCheckCode)
        grid.JSProperties("cpChartType") = ChartType

        Dim Verified As Boolean = False
        If dt.Rows.Count = 0 Then
            up_ClearJS()
            grid.JSProperties("cpRefresh") = "1"
            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)
            If Setup IsNot Nothing Then
                grid.JSProperties("cpUSL") = AFormat(Setup.SpecUSL)
                grid.JSProperties("cpLSL") = AFormat(Setup.SpecLSL)
                grid.JSProperties("cpUCL") = AFormat(Setup.CPUCL)
                grid.JSProperties("cpCL") = AFormat(Setup.CPCL)
                grid.JSProperties("cpLCL") = AFormat(Setup.CPLCL)
                grid.JSProperties("cpRUCL") = AFormat(Setup.RUCL)
                grid.JSProperties("cpXBarUCL") = AFormat(Setup.XBarUCL)
                grid.JSProperties("cpXBarLCL") = AFormat(Setup.XBarLCL)
                grid.JSProperties("cpRNG") = ""
            End If
        Else
            With dt.Rows(0)
                grid.JSProperties("cpUSL") = AFormat(.Item("SpecUSL"))
                grid.JSProperties("cpLSL") = AFormat(.Item("SpecLSL"))
                grid.JSProperties("cpUCL") = AFormat(.Item("CPUCL"))
                grid.JSProperties("cpCL") = AFormat(.Item("CPCL"))
                grid.JSProperties("cpLCL") = AFormat(.Item("CPLCL"))
                grid.JSProperties("cpRUCL") = AFormat(.Item("RUCL"))
                grid.JSProperties("cpXBarUCL") = AFormat(.Item("XBarUCL"))
                grid.JSProperties("cpXBarLCL") = AFormat(.Item("XBarLCL"))
                grid.JSProperties("cpMin") = AFormat(.Item("MinValue"))
                grid.JSProperties("cpMax") = AFormat(.Item("MaxValue"))
                grid.JSProperties("cpAve") = AFormat(.Item("AvgValue"))
                grid.JSProperties("cpR") = AFormat(.Item("RValue"))
                If ChartType = "0" Then
                    grid.JSProperties("cpRNG") = ""
                ElseIf LastNG = 1 And .Item("RValue") & "" <> "" And .Item("RValueNG") = "1" Then
                    grid.JSProperties("cpRNG") = "2"
                Else
                    grid.JSProperties("cpRNG") = .Item("RValueNG")
                End If
                grid.JSProperties("cpC") = .Item("CValue")
                grid.JSProperties("cpNG") = .Item("NGValue")
                grid.JSProperties("cpMKDate") = .Item("MKDate")
                grid.JSProperties("cpMKUser") = .Item("MKUser")
                grid.JSProperties("cpQCDate") = .Item("QCDate")
                grid.JSProperties("cpQCUser") = .Item("QCUser")
                grid.JSProperties("cpRefresh") = "1"
                If .Item("QCDate") & "" <> "" Then
                    Verified = True
                End If
            End With
        End If
        If Result IsNot Nothing Then
            grid.JSProperties("cpSubLotNo") = Result.SubLotNo
            grid.JSProperties("cpRemarks") = Result.Remark
            grid.JSProperties("cpNoProd") = Result.NoProductionStatus
        End If
        Dim dtVer As DataTable = clsSPCResultDB.GetLastVerification(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, Sequence)
        Dim LastVerification As Integer = dtVer.Rows(0)(0)
        If My.Computer.Name = "TOS56-ARI" Then
            LastVerification = 1
        End If
        grid.SettingsDataSecurity.AllowInsert = LastVerification = 1 And Not Verified And AuthUpdate And AllowSkill
        grid.SettingsDataSecurity.AllowEdit = LastVerification = 1 And Not Verified And AuthUpdate And AllowSkill
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
        Session("B02User") = UserID
        Session("B02ProdDate") = ProdDate
        Session("B02ProcessGroup") = cboProcessGroup.Value
        Session("B02LineGroup") = cboLineGroup.Value
        Session("B02ProcessCode") = cboProcess.Value
        Session("B02ItemTypeCode") = ItemTypeCode
        Session("B02LineCode") = Line
        Session("B02ItemCheckCode") = ItemCheckCode
        Session("B02ShiftCode") = Shift
        Session("B02Sequence") = Sequence

        If SetupFound = False Then
            show_error(MsgTypeEnum.Warning, "Chart Setup has not been set for this item", 1)
        ElseIf AllowSkill = False Then
            show_error(MsgTypeEnum.Warning, "You don't have skill to input result for this item. Please set SPC skill in IOT System", 1)
        ElseIf LastVerification = 0 Then
            ProdDate = Format(dtVer.Rows(0)(1), "dd MMM yyyy")
            Dim ShiftCode As String = dtVer.Rows(0)(2) & ""
            Sequence = dtVer.Rows(0)(3)
            show_error(MsgTypeEnum.Warning, "Previous Sequence is not yet verified (" & ProdDate & ", Shift " & ShiftCode & ", Sequence " & Sequence & ")", 1)
        End If
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
            Return Format(v, "0.0000")
        End If
    End Function


    Private Sub grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles grid.CustomCallback
        Dim pFunction As String = Split(e.Parameters, "|")(0)
        Select Case pFunction
            Case "clear"
                up_ClearGrid()
            Case "load", "save", "approve"
                Dim pFactory As String = Split(e.Parameters, "|")(1)
                Dim pItemType As String = Split(e.Parameters, "|")(2)
                Dim pLine As String = Split(e.Parameters, "|")(3)
                Dim pItemCheck As String = Split(e.Parameters, "|")(4)
                Dim pDate As String = Split(e.Parameters, "|")(5)
                Dim pShift As String = Split(e.Parameters, "|")(6)
                Dim pSeq As String = Split(e.Parameters, "|")(7)
                Dim pVerified As String = Split(e.Parameters, "|")(8)
                pSeq = Val(pSeq)
                If pFunction = "save" Then
                    Dim pSubLotNo As String = Split(e.Parameters, "|")(9)
                    Dim pRemark As String = Split(e.Parameters, "|")(10)
                    Dim pNoProd As String = Split(e.Parameters, "|")(11)
                    If pNoProd.Trim.ToLower = "true" Then
                        pNoProd = "1"
                    Else
                        pNoProd = "0"
                    End If
                    pUser = Session("user") & ""
                    clsSPCResultDB.Update(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pSubLotNo, pRemark, pNoProd, pUser)
                    show_error(MsgTypeEnum.Success, "Update data successfull!", 1)
                End If
                GridLoad(pFactory, pItemType, pLine, pItemCheck, pDate, pShift, pSeq, pVerified)
        End Select
    End Sub

    Private Sub grid_RowUpdating(sender As Object, e As ASPxDataUpdatingEventArgs) Handles grid.RowUpdating
        e.Cancel = True

        Dim Result As New clsSPCResult
        Result.FactoryCode = cboFactory.Value
        Result.ItemCheckCode = cboItemCheck.Value
        Result.ItemTypeCode = cboType.Value
        Result.LineCode = cboLine.Value
        Result.ProdDate = dtDate.Value
        Result.ShiftCode = cboShift.Value
        Result.SequenceNo = cboSeq.Value
        Result.SubLotNo = txtSubLotNo.Text
        Result.Remark = txtRemarks.Text
        Result.RegisterUser = Session("user") & ""

        Dim SetupFound As Boolean = ValidateChartSetup(Result.FactoryCode, Result.ItemTypeCode, Result.LineCode, Result.ItemCheckCode, Format(Result.ProdDate, "yyyy-MM-dd"))
        If SetupFound = False Then
            grid.CancelEdit()
            show_error(MsgTypeEnum.Warning, "Chart Setup has not been set for this item", 1)
            Return
        End If
        clsSPCResultDB.Insert(Result)

        Dim SeqNo As Integer = e.Keys("SeqNo")
        Dim Detail As New clsSPCResultDetail
        Detail.SPCResultID = Result.SPCResultID
        Detail.SequenceNo = SeqNo
        Detail.DeleteStatus = e.NewValues("DeleteStatus")
        Detail.Remark = e.NewValues("Remark")
        Detail.RegisterUser = Result.RegisterUser
        clsSPCResultDetailDB.Update(Detail)
        grid.CancelEdit()

        show_error(MsgTypeEnum.Success, "Update data successfull!", 1)
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

                .Cells(6, 6, 6, 6).Value = "Verified Only"
                .Cells(6, 6, 6, 7).Merge = True
                .Cells(6, 8).Value = ": " & cboShow.Text

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
        Dim Result As New clsSPCResult
        If e.UpdateValues.Count > 0 Then
            Result.FactoryCode = cboFactory.Value
            Result.ItemCheckCode = cboItemCheck.Value
            Result.ItemTypeCode = cboType.Value
            Result.LineCode = cboLine.Value
            Result.ProdDate = dtDate.Value
            Result.ShiftCode = cboShift.Value
            Result.SequenceNo = cboSeq.Value
            Result.SubLotNo = ""
            Result.Remark = ""
            Result.RegisterUser = Session("user") & ""

            clsSPCResultDB.Insert(Result)
            For i = 0 To e.UpdateValues.Count - 1
                Dim SeqNo As String = e.UpdateValues(i).Keys("SeqNo")
                Dim Detail As New clsSPCResultDetail
                Detail.SPCResultID = Result.SPCResultID
                Detail.SequenceNo = SeqNo
                Detail.DeleteStatus = e.UpdateValues(i).NewValues("DeleteStatus")
                Detail.Value = e.UpdateValues(i).NewValues("Value")
                Detail.Remark = e.UpdateValues(i).NewValues("Remark")
                Detail.RegisterUser = Result.RegisterUser
                clsSPCResultDetailDB.Insert(Detail)
            Next
        End If
    End Sub

    Private Sub DownloadExcel()
        Dim ps As New PrintingSystem()
        LoadChartX(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), cboShow.Value, cboSeq.Value)
        Dim linkX As New PrintableComponentLink(ps)
        linkX.Component = (CType(chartX, IChartContainer)).Chart

        Dim linkR As New PrintableComponentLink(ps)
        ChartType = clsXRChartDB.GetChartType(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value)
        If ChartType = "1" Or ChartType = "2" Then
            LoadChartR(cboFactory.Value, cboType.Value, cboLine.Value, cboItemCheck.Value, Format(dtDate.Value, "yyyy-MM-dd"), cboShow.Value, cboSeq.Value)
            linkR.Component = (CType(chartR, IChartContainer)).Chart
        End If

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
                Hdr.VerifiedOnly = cboShow.Value
                Hdr.ProcessGroup = cboProcessGroup.Text
                Hdr.LineGroup = cboLineGroup.Text
                Hdr.ProcessCode = cboProcess.Text

                GridTitle(ws, Hdr)
                GridExcelInput(ws, Hdr)
                GridExcel(ws, Hdr)
                .InsertRow(LastRow, 22)
                Dim fi As New FileInfo(Path & "\chart.png")
                Dim Picture As OfficeOpenXml.Drawing.ExcelPicture
                Picture = .Drawings.AddPicture("chart", Image.FromStream(streamImg))
                Picture.SetPosition(LastRow, 0, 0, 0)

                If ChartType = "1" Or ChartType = "2" Then
                    Dim fi2 As New FileInfo(Path & "\chartR.png")
                    Dim Picture2 As OfficeOpenXml.Drawing.ExcelPicture
                    Picture2 = .Drawings.AddPicture("chartR", Image.FromStream(streamImg2))
                    Picture2.SetPosition(LastRow + 25, 0, 0, 0)
                End If
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
            dtDel = ds.Tables(9)

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
                        .Cells(iRow, iCol).Style.Numberformat.Format = "0.0000"
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

                .Cells(iRow, colValue1, iRow, colValue).Style.Numberformat.Format = "0.0000"
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
                If dt.Rows(i)("DeleteStatus") & "" = "1" Then
                    .Cells(iRow, 1, iRow, colCount).Style.Fill.PatternType = ExcelFillStyle.Solid
                    .Cells(iRow, 1, iRow, colCount).Style.Fill.BackgroundColor.SetColor(Color.Silver)
                ElseIf dt.Rows(i)("JudgementColor") & "" = "1" Then
                    .Cells(iRow, 1, iRow, colCount).Style.Fill.PatternType = ExcelFillStyle.Solid
                    .Cells(iRow, 1, iRow, colCount).Style.Fill.BackgroundColor.SetColor(Color.Pink)
                ElseIf dt.Rows(i)("JudgementColor") & "" = "2" Then
                    .Cells(iRow, 1, iRow, colCount).Style.Fill.PatternType = ExcelFillStyle.Solid
                    .Cells(iRow, 1, iRow, colCount).Style.Fill.BackgroundColor.SetColor(Color.Red)
                End If
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
                .Cells(iRow, 7).Value = "Xbar Control"
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
            .Cells(iRow + 2, 2, iRow + 2, 10 + AddCol).Style.Numberformat.Format = "0.0000"

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

            .Cells(iRow + 2, 2, iRow + 2, 10 + AddCol).Style.Numberformat.Format = "0.0000"

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

    Private Sub cboShift_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboShift.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim LineCode As String = Split(e.Parameter, "|")(2)
        Dim ItemCheckCode As String = Split(e.Parameter, "|")(3)
        cboShift.DataSource = clsFrequencyDB.GetShift(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode)
        cboShift.DataBind()
    End Sub

    Private Sub grid_CellEditorInitialize(sender As Object, e As ASPxGridViewEditorEventArgs) Handles grid.CellEditorInitialize
        If e.Column.FieldName = "Value" Then
            If IsDBNull(e.Value) Or grid.IsNewRowEditing Then
                e.Editor.ReadOnly = False
            Else
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = System.Drawing.Color.Silver
            End If
        ElseIf (e.Column.FieldName = "Remark" Or e.Column.FieldName = "DeleteStatus") Then
            e.Editor.ReadOnly = False
        Else
            e.Editor.ReadOnly = True
            e.Editor.ForeColor = System.Drawing.Color.Silver
        End If
    End Sub

    Private Sub grid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles grid.HtmlDataCellPrepared
        If e.DataColumn.FieldName <> "Value" And e.DataColumn.FieldName <> "Remark" And e.DataColumn.FieldName <> "DeleteStatus" Then
            e.Cell.Attributes.Add("onclick", "event.cancelBubble = true")
        End If
    End Sub

    Private Sub grid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles grid.HtmlRowPrepared
        If e.GetValue("DeleteStatus") IsNot Nothing AndAlso e.GetValue("DeleteStatus").ToString = "1" Then
            e.Row.BackColor = System.Drawing.Color.Silver
        ElseIf e.GetValue("JudgementColor") IsNot Nothing AndAlso Not IsDBNull(e.GetValue("JudgementColor")) Then
            If e.GetValue("JudgementColor") = "1" Then
                e.Row.BackColor = System.Drawing.Color.Pink
            ElseIf e.GetValue("JudgementColor") = "2" Then
                e.Row.BackColor = System.Drawing.Color.Red
            End If
        End If
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

    Private Sub gridX_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles gridX.CustomCallback
        Dim Hdr As New clsHeader
        Hdr.FactoryCode = Split(e.Parameters, "|")(0)
        Hdr.ItemTypeCode = Split(e.Parameters, "|")(1)
        Hdr.LineCode = Split(e.Parameters, "|")(2)
        Hdr.ItemCheckCode = Split(e.Parameters, "|")(3)
        Hdr.ProdDate = Split(e.Parameters, "|")(4)
        Hdr.VerifiedOnly = Split(e.Parameters, "|")(5)
        Hdr.Seq = Split(e.Parameters, "|")(6)
        Hdr.ShiftCode = Split(e.Parameters, "|")(7)
        GridXLoad(Hdr)
    End Sub

    Private Sub gridX_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles gridX.HtmlRowPrepared
        If e.KeyValue = "Min" Or e.KeyValue = "Max" Or e.KeyValue = "Avg" Or e.KeyValue = "R" Then
            'e.Row.BackColor = System.Drawing.Color.LightGray
            e.Row.BorderStyle = BorderStyle.Double
            e.Row.BorderWidth = 50
        End If
        If e.KeyValue = "-" Or e.KeyValue = "--" Then
            e.Row.BackColor = System.Drawing.Color.FromArgb(112, 112, 112)
            e.Row.Height = 5
        End If
    End Sub

    Private Sub LoadChartR(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, VerifiedOnly As String, SeqNo As Integer)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartR(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, "", VerifiedOnly, SeqNo)
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

            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)
            diagram.AxisY.ConstantLines.Clear()
            If Setup IsNot Nothing Then
                Dim RUCL As New ConstantLine("UCL R")
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
                    Dim GridAlignment As Double = Math.Round(MaxValue / 20, 4)
                    diagram.AxisY.NumericScaleOptions.CustomGridAlignment = GridAlignment
                End If
                Dim EndSideMargin As Single = Math.Round(MaxValue / 10, 3)
                diagram.AxisY.WholeRange.EndSideMargin = EndSideMargin
            End If
            .DataBind()
        End With
    End Sub

    Dim ChartType As String

    Private Sub LoadChartX(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, VerifiedOnly As String, SeqNo As String)
        ChartType = clsXRChartDB.GetChartType(FactoryCode, ItemTypeCode, Line, ItemCheckCode)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartXR(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate, VerifiedOnly, SeqNo)
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
            If ChartType = "1" Or ChartType = "2" Then
                .Titles(0).Text = "Xbar Control Chart"
            Else
                .Titles(0).Text = "Graph Monitoring"
            End If


            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(FactoryCode, ItemTypeCode, Line, ItemCheckCode, ProdDate)
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

                If ChartType = "1" Or ChartType = "2" Then
                    Dim XBarLCL As New ConstantLine("      XBarLCL")
                    XBarLCL.Color = System.Drawing.Color.Orange
                    XBarLCL.LineStyle.Thickness = 1
                    XBarLCL.LineStyle.DashStyle = DashStyle.Dash
                    diagram.AxisY.ConstantLines.Add(XBarLCL)
                    XBarLCL.AxisValue = Setup.XBarLCL

                    Dim XBarUCL As New ConstantLine("      XBarUCL")
                    XBarUCL.Color = System.Drawing.Color.Orange
                    XBarUCL.LineStyle.Thickness = 1
                    XBarUCL.LineStyle.DashStyle = DashStyle.Dash
                    diagram.AxisY.ConstantLines.Add(XBarUCL)
                    XBarUCL.AxisValue = Setup.XBarUCL
                End If

                Dim LSL As New ConstantLine("      LSL")
                LSL.Color = System.Drawing.Color.Red
                LSL.LineStyle.Thickness = 1
                LSL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(LSL)
                LSL.AxisValue = Setup.SpecLSL

                Dim USL As New ConstantLine("      USL")
                USL.Color = System.Drawing.Color.Red
                USL.LineStyle.Thickness = 1
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
                Dim EndSideMargin As Single = Math.Round((MaxValue - MinValue) / 20, 3)

                diagram.AxisY.WholeRange.MinValue = MinValue
                diagram.AxisY.WholeRange.MaxValue = MaxValue
                diagram.AxisY.WholeRange.EndSideMargin = EndSideMargin

                diagram.AxisY.VisualRange.MinValue = MinValue
                diagram.AxisY.VisualRange.MaxValue = MaxValue
                diagram.AxisY.VisualRange.EndSideMargin = EndSideMargin

                Dim diff As Double = MaxValue - MinValue
                If diff > 0 Then
                    Dim gridAlignment As Double = Math.Round(diff / 15, 3)
                    diagram.AxisY.NumericScaleOptions.CustomGridAlignment = gridAlignment
                End If

                CType(.Diagram, XYDiagram).SecondaryAxesY.Clear()
                Dim myAxisY As New SecondaryAxisY("my Y-Axis")
                myAxisY.Visibility = DevExpress.Utils.DefaultBoolean.False
                myAxisY.WholeRange.EndSideMargin = 0
                myAxisY.WholeRange.StartSideMargin = 0
                CType(.Diagram, XYDiagram).SecondaryAxesY.Add(myAxisY)
                CType(.Series("Rule").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                CType(.Series("RuleYellow").View, XYDiagramSeriesViewBase).AxisY = myAxisY
            End If
            .DataBind()
            'If xr.Count > 5 Then
            '    .Width = xr.Count * 20
            'End If
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
        Dim VerifiedOnly As String = Split(Prm, "|")(5)
        Dim SeqNo As String = Split(Prm, "|")(6)

        LoadChartX(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, VerifiedOnly, SeqNo)
    End Sub

    Dim PrevYellow As Integer
    Private Sub gridX_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles gridX.HtmlDataCellPrepared
        Dim LCL As Double
        Dim UCL As Double
        Dim LSL As Double
        Dim USL As Double
        Dim RUCL As Double
        Dim RLCL As Double
        Dim Del As String
        Dim LightYellow As Color = Color.FromArgb(255, 255, 153)

        Dim ColName As String = e.DataColumn.FieldName
        If Not IsDBNull(e.CellValue) AndAlso ColName <> "Seq" AndAlso ColName <> "Sequence" AndAlso ColName <> "Des" AndAlso (e.GetValue("Seq") = "1" Or e.GetValue("Seq") = "3" Or e.GetValue("Seq") = "4" Or e.GetValue("Seq") = "5") Then
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
            If Del = "1" Then
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
        If Not IsDBNull(e.CellValue) AndAlso ColName <> "Seq" AndAlso ColName <> "Sequence" AndAlso ColName <> "Des" And ColName <> "Des" And e.GetValue("Seq") = "6" And ChartType <> "0" Then
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
        If e.KeyValue = "-" Or e.KeyValue = "--" Then
            e.Cell.Text = ""
        End If
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
        Dim ProdDate As String = Split(Prm, "|")(4)
        Dim VerifiedOnly As String = Split(Prm, "|")(5)
        Dim SeqNo As String = Split(Prm, "|")(6)
        LoadChartR(FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, ProdDate, VerifiedOnly, SeqNo)
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

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        DownloadExcel()
    End Sub

    Protected Sub grid_RowValidating(sender As Object, e As ASPxDataValidationEventArgs)
        Dim StatusDelete As Integer = If(e.NewValues("DeleteStatus") IsNot Nothing, CInt(Fix(e.NewValues("DeleteStatus"))), 0)
        If StatusDelete = 1 AndAlso (e.NewValues("Remark") Is Nothing OrElse e.NewValues("Remark") = "") Then
            AddError(e.Errors, grid.Columns("Remark"), "Please input Remark for deletion!")
            Return
        End If
        Dim Value As Object = e.NewValues("Value")
        If Not IsNumeric(Value) Then
            AddError(e.Errors, grid.Columns("Value"), "Please input valid numeric!")
            Return
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