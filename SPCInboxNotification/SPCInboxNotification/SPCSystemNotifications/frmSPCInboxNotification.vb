Imports C1.Win
Imports C1.Win.C1FlexGrid
Imports System
Imports System.Threading
Imports System.Windows.Forms
Imports System.Transactions
Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Imports System.Xml

Public Class frmSPCInboxNotification

#Region "Declare"
    'Data Login
    Private UserIDLogin As String
    Private PasswordLogin As String
    Private pLink As String
    Private StartUp As Boolean

    'datatable 
    Private dtNG As DataTable
    Private dtDelayInput As DataTable
    Private dtDelayVerification As DataTable
    Private factory As String

    'Database
    Private Server As String
    Private Database As String
    Private UserName As String
    Private Password As String

    Dim NGInputLastUpdate As String
    Dim DelayInputLastUpdate As String
    Dim DelayVerificationLastUpdate As String

    Dim currentNGLastUpdate As String
    Dim currentDelayInput As String
    Dim currentDelayVerification As String
    Dim firstLoad As Integer = 0

    Private ConnectionString As String

    Dim config As clsConfig
    Dim NewEnryption As New clsDESEncryption("TOS")
    Dim ls_path As String = AddSlash(My.Application.Info.DirectoryPath) & "config.xml"


    Private Enum DelayInput
        pType = 0
        pMachineProcess = 1
        pItemCheck = 2
        pDate = 3
        pShift = 4
        pSeq = 5
        pScheduleStart = 6
        pScheduleEnd = 7
        pDelayMinutes = 8
        Count = 9
    End Enum

    Private Enum NGResult
        pType = 0
        pMachineProcess = 1
        pItemCheck = 2
        pDate = 3
        pShift = 4
        pSeq = 5
        pUSL = 6
        pLSL = 7
        pUCL = 8
        pLCL = 9
        pMin = 10
        pMax = 11
        pAve = 12
        pOperator = 13
        pMK = 14
        pQC = 15
        Count = 16
    End Enum

    Private Enum DelayVerification
        pType = 0
        pMachineProcess = 1
        pItemCheck = 2
        pDate = 3
        pShift = 4
        pSeq = 5
        pUSL = 6
        pLSL = 7
        pUCL = 8
        pLCL = 9
        pMin = 10
        pMax = 11
        pAve = 12
        pOperator = 13
        pMK = 14
        pQC = 15
        pVerifTime = 16
        pDelayVerif = 17
        Count = 18
    End Enum

    Dim thrd As Thread
    Dim accessLock As New Object
    Dim endThread As Boolean = False
    Dim Finish As Boolean
    Dim inThrdProcess As Boolean = False

    Delegate Sub SetLabelCallBack(ByVal txt As String, ByVal labelControl As System.Windows.Forms.Label)
    Delegate Sub SetGridCallBack(ByVal dt As DataTable, ByVal grid As C1FlexGrid, ByVal Type As String)
    Delegate Sub SetComboCallBack(ByVal txt As String, ByVal combo As System.Windows.Forms.ComboBox)
#End Region

#Region "Init"
    Public Sub New(ByVal pdtNG As DataTable, ByVal pdtDelayInput As DataTable, ByVal pdtDelayVerification As DataTable, pFactory As String)
        InitializeComponent()
        dtNG = pdtNG
        dtDelayInput = pdtDelayInput
        dtDelayVerification = pdtDelayVerification
        factory = pFactory
    End Sub

    Private Sub frmInboxNotifications_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setDateTime()
        'GridHeader()
        'GridLoad()
        'setFactory()
    End Sub
#End Region

#Region "Control-Event"

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        up_dispatchThread()
        Timer1.Stop()
    End Sub

    'Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
    '    'up_dispatchThread()
    '    'Timer2.Stop()
    'End Sub

    Private Sub frmSPCInboxNotification_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        StopThread()
        'Do Until thrd.ThreadState = ThreadState.Stopped
        '    setLabel("Please wait until thread safely terminated...", txtMsg, Color.Red)
        '    System.Windows.Forms.Application.DoEvents()
        '    If tgnTHD Is Nothing Then Exit Do
        '    Thread.Sleep(100)
        'Loop
        'tgnFinish = True
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        dtNG = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "NG")
        dtDelayInput = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "DelayInput")
        dtDelayVerification = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "DelayVerification")
        factory = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "Factory").Rows(0)("Factory")

        GridHeader()
        GridLoad()
    End Sub

#End Region

#Region "Sub and Function"

    Public Function AddSlash(ByVal Path As String) As String
        Dim Result As String = Path
        If Path.EndsWith("\") = False Then
            Result = Result + "\"
        End If
        Return Result
    End Function

    Private Sub up_AppSettingLoad(ByVal ls_path As String)
        Try
            If (IO.File.Exists(ls_path)) Then
                If Trim(IO.File.ReadAllText(ls_path).Length) = 0 Then Exit Sub
                Dim Settings = XDocument.Load(ls_path)
                Dim login = Settings.Descendants("Login").FirstOrDefault()
                If Not IsNothing(login) Then
                    If Not IsNothing(login.Element("UserID")) Then UserIDLogin = NewEnryption.DecryptData(login.Element("UserID").Value)
                    If Not IsNothing(login.Element("Password")) Then PasswordLogin = NewEnryption.DecryptData(login.Element("Password").Value)
                    If Not IsNothing(login.Element("LinkSPC")) Then pLink = NewEnryption.DecryptData(login.Element("LinkSPC").Value)
                    If Not IsNothing(login.Element("StartUp")) Then StartUp = NewEnryption.DecryptData(login.Element("StartUp").Value)
                End If
                Dim SPCDB = Settings.Descendants("SPCDB").FirstOrDefault()
                If Not IsNothing(SPCDB) Then
                    If Not IsNothing(SPCDB.Element("ServerName")) Then Server = NewEnryption.DecryptData(SPCDB.Element("ServerName").Value)
                    If Not IsNothing(SPCDB.Element("Database")) Then Database = NewEnryption.DecryptData(SPCDB.Element("Database").Value)
                    If Not IsNothing(SPCDB.Element("UserID")) Then UserName = NewEnryption.DecryptData(SPCDB.Element("UserID").Value)
                    If Not IsNothing(SPCDB.Element("Password")) Then Password = NewEnryption.DecryptData(SPCDB.Element("Password").Value)
                End If
            Else
                MessageBox.Show("Config File is not found!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Exclamation, "Warning")
        End Try
    End Sub

    Private Sub setFactory()
        setCombo(factory, cboFactory)
        'cboFactory.Text = factory
        'cboFactory.Enabled = False
    End Sub

    Private Sub setDateTime()
        setLabel(Date.Now.ToString("dd MMM yyyy"), lblDate)
        setLabel(Date.Now.ToString("HH:mm:ss"), lblTime)

        'lblDate.Text = Date.Now.ToString("dd MMM yyyy")
        'lblTime.Text = Date.Now.ToString("HH:mm:ss")
    End Sub

    Private Sub GridHeader()
        NGHeader()
        DelayHeader()
        VerificationHeader()
    End Sub

    Private Sub NGHeader()
        setNG(dtNG, gridNGResult, "Header")
        'With gridNGResult
        '    .Rows.Fixed = 1
        '    .Rows.Count = 1
        '    .Cols.Fixed = 0
        '    .Cols.Count = NGResult.Count

        '    .Item(0, NGResult.pType) = "Type"
        '    .Item(0, NGResult.pMachineProcess) = "Machine Process"
        '    .Item(0, NGResult.pItemCheck) = "Item Check"
        '    .Item(0, NGResult.pDate) = "Date"
        '    .Item(0, NGResult.pShift) = "Shift"
        '    .Item(0, NGResult.pSeq) = "Seq"
        '    .Item(0, NGResult.pUSL) = "USL"
        '    .Item(0, NGResult.pLSL) = "LSL"
        '    .Item(0, NGResult.pUCL) = "UCL"
        '    .Item(0, NGResult.pLCL) = "LCL"
        '    .Item(0, NGResult.pMin) = "Min"
        '    .Item(0, NGResult.pMax) = "Max"
        '    .Item(0, NGResult.pAve) = "Ave"
        '    .Item(0, NGResult.pOperator) = "Operator"
        '    .Item(0, NGResult.pMK) = "MK"
        '    .Item(0, NGResult.pQC) = "QC"

        '    '.AutoSizeCols()

        '    .Cols(NGResult.pType).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pMachineProcess).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(NGResult.pItemCheck).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(NGResult.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pShift).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pSeq).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pUSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pLSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pUCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pLCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pMin).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pMax).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pAve).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pOperator).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(NGResult.pMK).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(NGResult.pQC).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        '    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .GetCellRange(0, NGResult.pType, 0, NGResult.Count - 1).StyleNew.BackColor = Color.LightGray

        '    .AllowEditing = False
        '    .Styles.Normal.WordWrap = True
        '    .ExtendLastCol = False
        '    For j As Integer = NGResult.pType To NGResult.Count - 1
        '        .AutoSizeCol(j)
        '    Next
        'End With
    End Sub
    Private Sub DelayHeader()
        setDelay(dtNG, gridDelayInput, "Header")
        'With gridDelayInput
        '    .Rows.Fixed = 1
        '    .Rows.Count = 1
        '    .Cols.Fixed = 0
        '    .Cols.Count = DelayInput.Count

        '    .Item(0, DelayInput.pType) = "Type"
        '    .Item(0, DelayInput.pMachineProcess) = "Machine Process"
        '    .Item(0, DelayInput.pItemCheck) = "Item Check"
        '    .Item(0, DelayInput.pDate) = "Date"
        '    .Item(0, DelayInput.pShift) = "Shift"
        '    .Item(0, DelayInput.pSeq) = "Seq"
        '    .Item(0, DelayInput.pScheduleStart) = "Schedule Start"
        '    .Item(0, DelayInput.pScheduleEnd) = "Schedule End"
        '    .Item(0, DelayInput.pDelayMinutes) = "Delay (Minutes)"

        '    '.Cols(grdHeader.datetime).Width = 150
        '    '.Cols(grdHeader.datetime).Width = 450

        '    '.AutoSizeCols()

        '    .Cols(DelayInput.pType).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayInput.pMachineProcess).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(DelayInput.pItemCheck).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(DelayInput.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayInput.pShift).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayInput.pSeq).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayInput.pScheduleStart).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayInput.pScheduleEnd).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayInput.pDelayMinutes).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        '    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        '    .GetCellRange(0, DelayInput.pType, 0, DelayInput.Count - 1).StyleNew.BackColor = Color.LightGray

        '    .AllowEditing = False
        '    .Styles.Normal.WordWrap = True
        '    .ExtendLastCol = False
        '    For j As Integer = DelayInput.pType To DelayInput.Count - 1
        '        .AutoSizeCol(j)
        '    Next
        'End With
    End Sub
    Private Sub VerificationHeader()
        SetVerification(dtNG, gridDelayVerification, "Header")
        'With gridDelayVerification
        '    .Rows.Fixed = 1
        '    .Rows.Count = 1
        '    .Cols.Fixed = 0
        '    .Cols.Count = DelayVerification.Count

        '    .Item(0, DelayVerification.pType) = "Type"
        '    .Item(0, DelayVerification.pMachineProcess) = "Machine Process"
        '    .Item(0, DelayVerification.pItemCheck) = "Item Check"
        '    .Item(0, DelayVerification.pDate) = "Date"
        '    .Item(0, DelayVerification.pShift) = "Shift"
        '    .Item(0, DelayVerification.pSeq) = "Seq"
        '    .Item(0, DelayVerification.pUSL) = "USL"
        '    .Item(0, DelayVerification.pLSL) = "LSL"
        '    .Item(0, DelayVerification.pUCL) = "UCL"
        '    .Item(0, DelayVerification.pLCL) = "LCL"
        '    .Item(0, DelayVerification.pMin) = "Min"
        '    .Item(0, DelayVerification.pMax) = "Max"
        '    .Item(0, DelayVerification.pAve) = "Ave"
        '    .Item(0, DelayVerification.pOperator) = "Operator"
        '    .Item(0, DelayVerification.pMK) = "MK"
        '    .Item(0, DelayVerification.pQC) = "QC"
        '    .Item(0, DelayVerification.pVerifTime) = "Verif Time"
        '    .Item(0, DelayVerification.pDelayVerif) = "Delay Verif"

        '    '.AutoSizeCols()

        '    .Cols(DelayVerification.pType).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pMachineProcess).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(DelayVerification.pItemCheck).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(DelayVerification.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pShift).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pSeq).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pUSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pLSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pUCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pLCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pMin).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pMax).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pAve).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pOperator).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(DelayVerification.pMK).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pQC).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pVerifTime).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .Cols(DelayVerification.pDelayVerif).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        '    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        '    .GetCellRange(0, DelayVerification.pType, 0, DelayVerification.Count - 1).StyleNew.BackColor = Color.LightGray

        '    .AllowEditing = False
        '    .Styles.Normal.WordWrap = True
        '    .ExtendLastCol = False
        '    For j As Integer = DelayVerification.pType To DelayVerification.Count - 1
        '        .AutoSizeCol(j)
        '    Next
        'End With
    End Sub

    Private Sub GridLoad()
        NGLoad()
        DelayInputLoad()
        DelayVerificationLoad()
    End Sub

    Private Sub NGLoad()
        setNG(dtNG, gridNGResult, "Content")
        'With gridNGResult
        '    For i = 0 To dtNG.Rows.Count - 1
        '        .AddItem("")
        '        .Item(i + 1, NGResult.pType) = dtNG.Rows(i)("ItemTypeName").ToString()
        '        .Item(i + 1, NGResult.pMachineProcess) = dtNG.Rows(i)("LineName").ToString()
        '        .Item(i + 1, NGResult.pItemCheck) = dtNG.Rows(i)("ItemCheck").ToString()
        '        .Item(i + 1, NGResult.pDate) = dtNG.Rows(i)("Date").ToString()
        '        .Item(i + 1, NGResult.pShift) = dtNG.Rows(i)("ShiftCode").ToString()
        '        .Item(i + 1, NGResult.pSeq) = dtNG.Rows(i)("SequenceNo").ToString()
        '        .Item(i + 1, NGResult.pUSL) = dtNG.Rows(i)("USL").ToString()
        '        .Item(i + 1, NGResult.pLSL) = dtNG.Rows(i)("LSL").ToString()
        '        .Item(i + 1, NGResult.pUCL) = dtNG.Rows(i)("UCL").ToString()
        '        .Item(i + 1, NGResult.pLCL) = dtNG.Rows(i)("LCL").ToString()
        '        .Item(i + 1, NGResult.pMin) = dtNG.Rows(i)("MinValue").ToString()
        '        .Item(i + 1, NGResult.pMax) = dtNG.Rows(i)("MaxValue").ToString()
        '        .Item(i + 1, NGResult.pAve) = dtNG.Rows(i)("Average").ToString()
        '        .Item(i + 1, NGResult.pOperator) = dtNG.Rows(i)("Operator").ToString()
        '        .Item(i + 1, NGResult.pMK) = dtNG.Rows(i)("MK").ToString()
        '        .Item(i + 1, NGResult.pQC) = dtNG.Rows(i)("QC").ToString()
        '        .AutoSizeCols()
        '    Next
        '    lblTotalNGResult.Text = "Total : " & dtNG.Rows.Count & " Record"
        'End With
    End Sub
    Private Sub DelayInputLoad()
        setDelay(dtNG, gridDelayInput, "Content")
        'With gridDelayInput
        '    '.DataSource = dtDelayInput
        '    For i = 0 To dtDelayInput.Rows.Count - 1
        '        .AddItem("")
        '        .Item(i + 1, DelayInput.pType) = dtDelayInput.Rows(i)("ItemTypeName").ToString()
        '        .Item(i + 1, DelayInput.pMachineProcess) = dtDelayInput.Rows(i)("LineName").ToString()
        '        .Item(i + 1, DelayInput.pItemCheck) = dtDelayInput.Rows(i)("ItemCheck").ToString()
        '        .Item(i + 1, DelayInput.pDate) = dtDelayInput.Rows(i)("Date").ToString()
        '        .Item(i + 1, DelayInput.pShift) = dtDelayInput.Rows(i)("ShiftCode").ToString()
        '        .Item(i + 1, DelayInput.pSeq) = dtDelayInput.Rows(i)("SequenceNo").ToString()
        '        .Item(i + 1, DelayInput.pScheduleStart) = dtDelayInput.Rows(i)("StartTime").ToString()
        '        .Item(i + 1, DelayInput.pScheduleEnd) = dtDelayInput.Rows(i)("EndTime").ToString()

        '        Dim delay = dtDelayInput.Rows(i)("Delay").ToString()
        '        Dim tmSpan = TimeSpan.FromMinutes(dtDelayInput.Rows(i)("Delay").ToString())
        '        Dim Days = tmSpan.Days * 24
        '        Dim Hours = Days + tmSpan.Hours
        '        If Days > 0 Then
        '            .Item(i + 1, DelayInput.pDelayMinutes) = Convert.ToString(tmSpan.Days & " Day " & tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")

        '        Else

        '            If Hours > 0 Then
        '                .Item(i + 1, DelayInput.pDelayMinutes) = Convert.ToString(tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")
        '            Else
        '                .Item(i + 1, DelayInput.pDelayMinutes) = Convert.ToString(tmSpan.Minutes & " Minutes")
        '            End If

        '        End If
        '        '.Item(i + 1, DelayInput.pDelayMinutes) = dtDelayInput.Rows(i)("Delay").ToString()

        '        If CInt(dtDelayInput.Rows(i)("Delay").ToString()) > 60 Then
        '            .GetCellRange(i + 1, DelayInput.pDelayMinutes, i + 1, DelayInput.pDelayMinutes).StyleNew.BackColor = Color.Red
        '        End If
        '        If CInt(dtDelayInput.Rows(i)("Delay").ToString()) < 60 Then
        '            .GetCellRange(i + 1, DelayInput.pDelayMinutes, i + 1, DelayInput.pDelayMinutes).StyleNew.BackColor = Color.Yellow
        '        End If

        '        .AutoSizeCols()
        '    Next
        '    lblTotalDelayInput.Text = "Total : " & dtDelayInput.Rows.Count & " Record"
        'End With
    End Sub
    Private Sub DelayVerificationLoad()
        SetVerification(dtNG, gridDelayVerification, "Content")
        'With gridDelayVerification
        '    For i = 0 To dtDelayVerification.Rows.Count - 1
        '        .AddItem("")
        '        .Item(i + 1, DelayVerification.pType) = dtDelayVerification.Rows(i)("ItemTypeName").ToString()
        '        .Item(i + 1, DelayVerification.pMachineProcess) = dtDelayVerification.Rows(i)("LineName").ToString()
        '        .Item(i + 1, DelayVerification.pItemCheck) = dtDelayVerification.Rows(i)("ItemCheck").ToString()
        '        .Item(i + 1, DelayVerification.pDate) = dtDelayVerification.Rows(i)("Date").ToString()
        '        .Item(i + 1, DelayVerification.pShift) = dtDelayVerification.Rows(i)("ShiftCode").ToString()
        '        .Item(i + 1, DelayVerification.pSeq) = dtDelayVerification.Rows(i)("SequenceNo").ToString()
        '        .Item(i + 1, DelayVerification.pUSL) = dtDelayVerification.Rows(i)("USL").ToString()
        '        .Item(i + 1, DelayVerification.pLSL) = dtDelayVerification.Rows(i)("LSL").ToString()
        '        .Item(i + 1, DelayVerification.pUCL) = dtDelayVerification.Rows(i)("UCL").ToString()
        '        .Item(i + 1, DelayVerification.pLCL) = dtDelayVerification.Rows(i)("LCL").ToString()
        '        .Item(i + 1, DelayVerification.pMin) = dtDelayVerification.Rows(i)("MinValue").ToString()
        '        .Item(i + 1, DelayVerification.pMax) = dtDelayVerification.Rows(i)("MaxValue").ToString()
        '        .Item(i + 1, DelayVerification.pAve) = dtDelayVerification.Rows(i)("Average").ToString()
        '        .Item(i + 1, DelayVerification.pOperator) = dtDelayVerification.Rows(i)("Operator").ToString()
        '        .Item(i + 1, DelayVerification.pMK) = dtDelayVerification.Rows(i)("MK").ToString()
        '        .Item(i + 1, DelayVerification.pQC) = dtDelayVerification.Rows(i)("QC").ToString()
        '        .Item(i + 1, DelayVerification.pVerifTime) = dtDelayVerification.Rows(i)("VerifTime").ToString()

        '        Dim delay = dtDelayVerification.Rows(i)("DelayVerif").ToString()
        '        Dim tmSpan = TimeSpan.FromMinutes(dtDelayVerification.Rows(i)("DelayVerif").ToString())
        '        Dim Days = tmSpan.Days * 24
        '        Dim Hours = Days + tmSpan.Hours
        '        If Days > 0 Then
        '            .Item(i + 1, DelayVerification.pDelayVerif) = Convert.ToString(tmSpan.Days & " Day " & tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")

        '        Else

        '            If Hours > 0 Then
        '                .Item(i + 1, DelayVerification.pDelayVerif) = Convert.ToString(tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")
        '            Else
        '                .Item(i + 1, DelayVerification.pDelayVerif) = Convert.ToString(tmSpan.Minutes & " Minutes")
        '            End If

        '        End If
        '        '.Item(i + 1, DelayVerification.pDelayVerif) = dtDelayVerification.Rows(i)("DelayVerif").ToString()

        '        If CInt(dtDelayVerification.Rows(i)("DelayVerif").ToString()) > 60 Then
        '            .GetCellRange(i + 1, DelayVerification.pDelayVerif, i + 1, DelayVerification.pDelayVerif).StyleNew.BackColor = Color.Red
        '        End If
        '        If CInt(dtDelayVerification.Rows(i)("DelayVerif").ToString()) < 60 Then
        '            .GetCellRange(i + 1, DelayVerification.pDelayVerif, i + 1, DelayVerification.pDelayVerif).StyleNew.BackColor = Color.Yellow
        '        End If
        '        .AutoSizeCols()
        '    Next
        '    lblTotalDelayVerification.Text = "Total : " & dtDelayVerification.Rows.Count & " Record"
        'End With
    End Sub

#End Region

#Region "Thread"

    Private Sub loadDataRecord()
        Try
            config = New clsConfig
            up_AppSettingLoad(ls_path)
            ConnectionString = config.ConnectionString


            If inThrdProcess = False Then
                inThrdProcess = True
                dtNG = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "NG")
                dtDelayInput = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "DelayInput")
                dtDelayVerification = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "DelayVerification")
                factory = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "Factory").Rows(0)("Factory")


                If firstLoad = 0 Then
                    GridHeader()
                    GridLoad()
                    setFactory()
                Else
                    Try
                        If CDate(currentNGLastUpdate) > CDate(NGInputLastUpdate) OrElse CDate(currentDelayInput) > CDate(DelayInputLastUpdate) OrElse CDate(currentDelayVerification) > CDate(DelayVerificationLastUpdate) Then
                            GridHeader()
                            GridLoad()
                            setFactory()
                        End If
                    Catch ex As Exception

                    End Try
                End If
                frmSPCAlertNotifications.GetNotify(NGInputLastUpdate, DelayInputLastUpdate, DelayVerificationLastUpdate, ConnectionString, UserIDLogin,
                                                    currentNGLastUpdate, currentDelayInput, currentDelayVerification)

                firstLoad = 1
            End If
        Catch ex As Exception
            Dim errMsg As String
            errMsg = ex.Message
            GridHeader()
            GridLoad()
            setFactory()
        End Try
        inThrdProcess = False
    End Sub

    Private Sub thrdLoaded()
        Dim errMsg As String = ""
        Finish = False
        endThread = False

        Do Until Finish
            Try
                setDateTime()
                loadDataRecord()

                SyncLock accessLock
                    If endThread Then
                        Finish = True
                    End If
                End SyncLock
            Catch ex As Exception
                Dim pErrMsg As String = ex.Message
            End Try
        Loop

    End Sub

    Public Sub up_dispatchThread()
        thrd = New Thread(AddressOf thrdLoaded)
        thrd.Name = "Load Thread"
        thrd.IsBackground = True
        thrd.Start()
    End Sub

    Private Sub setLabel(ByVal txt As String, ByVal labelControl As System.Windows.Forms.Label)
        If labelControl.InvokeRequired Then
            Dim d As New SetLabelCallBack(AddressOf setLabel)
            Me.Invoke(d, New Object() {txt, labelControl})
        Else
            labelControl.Text = txt
        End If
    End Sub

    Private Sub setNG(ByVal dt As DataTable, ByVal grid As C1FlexGrid, ByVal Type As String)
        If grid.InvokeRequired Then
            Dim d As New SetGridCallBack(AddressOf setNG)
            Me.Invoke(d, New Object() {dt, grid, Type})
        Else
            If Type = "Header" Then
                With grid
                    .Redraw = False
                    .Rows.Fixed = 1
                    .Rows.Count = 1
                    .Cols.Fixed = 0
                    .Cols.Count = NGResult.Count

                    .Item(0, NGResult.pType) = "Type"
                    .Item(0, NGResult.pMachineProcess) = "Machine Process"
                    .Item(0, NGResult.pItemCheck) = "Item Check"
                    .Item(0, NGResult.pDate) = "Date"
                    .Item(0, NGResult.pShift) = "Shift"
                    .Item(0, NGResult.pSeq) = "Seq"
                    .Item(0, NGResult.pUSL) = "USL"
                    .Item(0, NGResult.pLSL) = "LSL"
                    .Item(0, NGResult.pUCL) = "UCL"
                    .Item(0, NGResult.pLCL) = "LCL"
                    .Item(0, NGResult.pMin) = "Min"
                    .Item(0, NGResult.pMax) = "Max"
                    .Item(0, NGResult.pAve) = "Ave"
                    .Item(0, NGResult.pOperator) = "Operator"
                    .Item(0, NGResult.pMK) = "MK"
                    .Item(0, NGResult.pQC) = "QC"

                    '.AutoSizeCols()

                    .Cols(NGResult.pType).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pMachineProcess).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(NGResult.pItemCheck).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(NGResult.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pShift).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pSeq).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pUSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pLSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pUCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pLCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pMin).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pMax).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pAve).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pOperator).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(NGResult.pMK).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(NGResult.pQC).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

                    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    '.GetCellRange(0, NGResult.pType, 0, NGResult.Count - 1).StyleNew.BackColor = Color.LightGray

                    .AllowEditing = False
                    .Styles.Normal.WordWrap = True
                    .ExtendLastCol = False
                    For j As Integer = NGResult.pType To NGResult.Count - 1
                        .AutoSizeCol(j)
                    Next
                    .Redraw = True
                End With
            ElseIf Type = "Content" Then
                With gridNGResult
                    .Redraw = False
                    For i = 0 To dtNG.Rows.Count - 1
                        .AddItem("")
                        .Item(i + 1, NGResult.pType) = dtNG.Rows(i)("ItemTypeName").ToString()
                        .Item(i + 1, NGResult.pMachineProcess) = dtNG.Rows(i)("LineName").ToString()
                        .Item(i + 1, NGResult.pItemCheck) = dtNG.Rows(i)("ItemCheck").ToString()
                        .Item(i + 1, NGResult.pDate) = dtNG.Rows(i)("Date").ToString()
                        .Item(i + 1, NGResult.pShift) = dtNG.Rows(i)("ShiftCode").ToString()
                        .Item(i + 1, NGResult.pSeq) = dtNG.Rows(i)("SequenceNo").ToString()
                        .Item(i + 1, NGResult.pUSL) = dtNG.Rows(i)("USL").ToString()
                        .Item(i + 1, NGResult.pLSL) = dtNG.Rows(i)("LSL").ToString()
                        .Item(i + 1, NGResult.pUCL) = dtNG.Rows(i)("UCL").ToString()
                        .Item(i + 1, NGResult.pLCL) = dtNG.Rows(i)("LCL").ToString()
                        .Item(i + 1, NGResult.pMin) = dtNG.Rows(i)("MinValue").ToString()
                        .Item(i + 1, NGResult.pMax) = dtNG.Rows(i)("MaxValue").ToString()
                        .Item(i + 1, NGResult.pAve) = dtNG.Rows(i)("Average").ToString()
                        .Item(i + 1, NGResult.pOperator) = dtNG.Rows(i)("Operator").ToString()
                        .Item(i + 1, NGResult.pMK) = dtNG.Rows(i)("MK").ToString()
                        .Item(i + 1, NGResult.pQC) = dtNG.Rows(i)("QC").ToString()
                        .AutoSizeCols()
                    Next
                    lblTotalNGResult.Text = "Total : " & dtNG.Rows.Count & " Record"
                    .Redraw = True
                End With
            End If
        End If
    End Sub

    Private Sub setDelay(ByVal dt As DataTable, ByVal grid As C1FlexGrid, ByVal Type As String)
        If grid.InvokeRequired Then
            Dim d As New SetGridCallBack(AddressOf setDelay)
            Me.Invoke(d, New Object() {dt, grid, Type})
        Else
            If Type = "Header" Then
                With gridDelayInput
                    .Redraw = False
                    .Rows.Fixed = 1
                    .Rows.Count = 1
                    .Cols.Fixed = 0
                    .Cols.Count = DelayInput.Count

                    .Item(0, DelayInput.pType) = "Type"
                    .Item(0, DelayInput.pMachineProcess) = "Machine Process"
                    .Item(0, DelayInput.pItemCheck) = "Item Check"
                    .Item(0, DelayInput.pDate) = "Date"
                    .Item(0, DelayInput.pShift) = "Shift"
                    .Item(0, DelayInput.pSeq) = "Seq"
                    .Item(0, DelayInput.pScheduleStart) = "Schedule Start"
                    .Item(0, DelayInput.pScheduleEnd) = "Schedule End"
                    .Item(0, DelayInput.pDelayMinutes) = "Delay (Minutes)"

                    '.Cols(grdHeader.datetime).Width = 150
                    '.Cols(grdHeader.datetime).Width = 450

                    '.AutoSizeCols()

                    .Cols(DelayInput.pType).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayInput.pMachineProcess).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(DelayInput.pItemCheck).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(DelayInput.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayInput.pShift).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayInput.pSeq).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayInput.pScheduleStart).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayInput.pScheduleEnd).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayInput.pDelayMinutes).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

                    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

                    '.GetCellRange(0, DelayInput.pType, 0, DelayInput.Count - 1).StyleNew.BackColor = Color.LightGray

                    .AllowEditing = False
                    .Styles.Normal.WordWrap = True
                    .ExtendLastCol = False
                    For j As Integer = DelayInput.pType To DelayInput.Count - 1
                        .AutoSizeCol(j)
                    Next
                    .Redraw = True
                End With
            ElseIf Type = "Content" Then
                With gridDelayInput
                    '.DataSource = dtDelayInput
                    .Redraw = False
                    For i = 0 To dtDelayInput.Rows.Count - 1
                        .AddItem("")
                        .Item(i + 1, DelayInput.pType) = dtDelayInput.Rows(i)("ItemTypeName").ToString()
                        .Item(i + 1, DelayInput.pMachineProcess) = dtDelayInput.Rows(i)("LineName").ToString()
                        .Item(i + 1, DelayInput.pItemCheck) = dtDelayInput.Rows(i)("ItemCheck").ToString()
                        .Item(i + 1, DelayInput.pDate) = dtDelayInput.Rows(i)("Date").ToString()
                        .Item(i + 1, DelayInput.pShift) = dtDelayInput.Rows(i)("ShiftCode").ToString()
                        .Item(i + 1, DelayInput.pSeq) = dtDelayInput.Rows(i)("SequenceNo").ToString()
                        .Item(i + 1, DelayInput.pScheduleStart) = dtDelayInput.Rows(i)("StartTime").ToString()
                        .Item(i + 1, DelayInput.pScheduleEnd) = dtDelayInput.Rows(i)("EndTime").ToString()

                        Dim delay = dtDelayInput.Rows(i)("Delay").ToString()
                        Dim tmSpan = TimeSpan.FromMinutes(dtDelayInput.Rows(i)("Delay").ToString())
                        Dim Days = tmSpan.Days * 24
                        Dim Hours = Days + tmSpan.Hours
                        If Days > 0 Then
                            .Item(i + 1, DelayInput.pDelayMinutes) = Convert.ToString(tmSpan.Days & " Day " & tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")

                        Else

                            If Hours > 0 Then
                                .Item(i + 1, DelayInput.pDelayMinutes) = Convert.ToString(tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")
                            Else
                                .Item(i + 1, DelayInput.pDelayMinutes) = Convert.ToString(tmSpan.Minutes & " Minutes")
                            End If

                        End If
                        '.Item(i + 1, DelayInput.pDelayMinutes) = dtDelayInput.Rows(i)("Delay").ToString()

                        If CInt(dtDelayInput.Rows(i)("Delay").ToString()) > 60 Then
                            .GetCellRange(i + 1, DelayInput.pDelayMinutes, i + 1, DelayInput.pDelayMinutes).StyleNew.BackColor = Color.Red
                        End If
                        If CInt(dtDelayInput.Rows(i)("Delay").ToString()) < 60 Then
                            .GetCellRange(i + 1, DelayInput.pDelayMinutes, i + 1, DelayInput.pDelayMinutes).StyleNew.BackColor = Color.Yellow
                        End If

                        .AutoSizeCols()
                    Next
                    lblTotalDelayInput.Text = "Total : " & dtDelayInput.Rows.Count & " Record"
                    .Redraw = True
                End With
            End If
        End If
    End Sub

    Private Sub SetVerification(ByVal dt As DataTable, ByVal grid As C1FlexGrid, ByVal Type As String)
        If grid.InvokeRequired Then
            Dim d As New SetGridCallBack(AddressOf SetVerification)
            Me.Invoke(d, New Object() {dt, grid, Type})
        Else
            If Type = "Header" Then
                With gridDelayVerification
                    .Rows.Fixed = 1
                    .Rows.Count = 1
                    .Cols.Fixed = 0
                    .Cols.Count = DelayVerification.Count

                    .Item(0, DelayVerification.pType) = "Type"
                    .Item(0, DelayVerification.pMachineProcess) = "Machine Process"
                    .Item(0, DelayVerification.pItemCheck) = "Item Check"
                    .Item(0, DelayVerification.pDate) = "Date"
                    .Item(0, DelayVerification.pShift) = "Shift"
                    .Item(0, DelayVerification.pSeq) = "Seq"
                    .Item(0, DelayVerification.pUSL) = "USL"
                    .Item(0, DelayVerification.pLSL) = "LSL"
                    .Item(0, DelayVerification.pUCL) = "UCL"
                    .Item(0, DelayVerification.pLCL) = "LCL"
                    .Item(0, DelayVerification.pMin) = "Min"
                    .Item(0, DelayVerification.pMax) = "Max"
                    .Item(0, DelayVerification.pAve) = "Ave"
                    .Item(0, DelayVerification.pOperator) = "Operator"
                    .Item(0, DelayVerification.pMK) = "MK"
                    .Item(0, DelayVerification.pQC) = "QC"
                    .Item(0, DelayVerification.pVerifTime) = "Verif Time"
                    .Item(0, DelayVerification.pDelayVerif) = "Delay Verif"

                    '.AutoSizeCols()

                    .Cols(DelayVerification.pType).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pMachineProcess).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(DelayVerification.pItemCheck).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(DelayVerification.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pShift).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pSeq).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pUSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pLSL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pUCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pLCL).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pMin).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pMax).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pAve).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pOperator).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                    .Cols(DelayVerification.pMK).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pQC).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pVerifTime).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    .Cols(DelayVerification.pDelayVerif).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

                    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                    '.GetCellRange(0, DelayVerification.pType, 0, DelayVerification.Count - 1).StyleNew.BackColor = Color.LightGray

                    .AllowEditing = False
                    .Styles.Normal.WordWrap = True
                    .ExtendLastCol = False
                    For j As Integer = DelayVerification.pType To DelayVerification.Count - 1
                        .AutoSizeCol(j)
                    Next
                End With
            ElseIf Type = "Content" Then
                With gridDelayVerification
                    .Redraw = False
                    For i = 0 To dtDelayVerification.Rows.Count - 1

                        .AddItem("")
                        .Item(i + 1, DelayVerification.pType) = dtDelayVerification.Rows(i)("ItemTypeName").ToString()
                        .Item(i + 1, DelayVerification.pMachineProcess) = dtDelayVerification.Rows(i)("LineName").ToString()
                        .Item(i + 1, DelayVerification.pItemCheck) = dtDelayVerification.Rows(i)("ItemCheck").ToString()
                        .Item(i + 1, DelayVerification.pDate) = dtDelayVerification.Rows(i)("Date").ToString()
                        .Item(i + 1, DelayVerification.pShift) = dtDelayVerification.Rows(i)("ShiftCode").ToString()
                        .Item(i + 1, DelayVerification.pSeq) = dtDelayVerification.Rows(i)("SequenceNo").ToString()
                        .Item(i + 1, DelayVerification.pUSL) = dtDelayVerification.Rows(i)("USL").ToString()
                        .Item(i + 1, DelayVerification.pLSL) = dtDelayVerification.Rows(i)("LSL").ToString()
                        .Item(i + 1, DelayVerification.pUCL) = dtDelayVerification.Rows(i)("UCL").ToString()
                        .Item(i + 1, DelayVerification.pLCL) = dtDelayVerification.Rows(i)("LCL").ToString()
                        .Item(i + 1, DelayVerification.pMin) = dtDelayVerification.Rows(i)("MinValue").ToString()
                        .Item(i + 1, DelayVerification.pMax) = dtDelayVerification.Rows(i)("MaxValue").ToString()
                        .Item(i + 1, DelayVerification.pAve) = dtDelayVerification.Rows(i)("Average").ToString()
                        .Item(i + 1, DelayVerification.pOperator) = dtDelayVerification.Rows(i)("Operator").ToString()
                        .Item(i + 1, DelayVerification.pMK) = dtDelayVerification.Rows(i)("MK").ToString()
                        .Item(i + 1, DelayVerification.pQC) = dtDelayVerification.Rows(i)("QC").ToString()
                        .Item(i + 1, DelayVerification.pVerifTime) = dtDelayVerification.Rows(i)("VerifTime").ToString()

                        Dim delay = dtDelayVerification.Rows(i)("DelayVerif").ToString()
                        Dim tmSpan = TimeSpan.FromMinutes(dtDelayVerification.Rows(i)("DelayVerif").ToString())
                        Dim Days = tmSpan.Days * 24
                        Dim Hours = Days + tmSpan.Hours
                        If Days > 0 Then
                            .Item(i + 1, DelayVerification.pDelayVerif) = Convert.ToString(tmSpan.Days & " Day " & tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")

                        Else

                            If Hours > 0 Then
                                .Item(i + 1, DelayVerification.pDelayVerif) = Convert.ToString(tmSpan.Hours & " Hours " & tmSpan.Minutes & " Minutes")
                            Else
                                .Item(i + 1, DelayVerification.pDelayVerif) = Convert.ToString(tmSpan.Minutes & " Minutes")
                            End If

                        End If
                        '.Item(i + 1, DelayVerification.pDelayVerif) = dtDelayVerification.Rows(i)("DelayVerif").ToString()

                        If CInt(dtDelayVerification.Rows(i)("DelayVerif").ToString()) > 60 Then
                            .GetCellRange(i + 1, DelayVerification.pDelayVerif, i + 1, DelayVerification.pDelayVerif).StyleNew.BackColor = Color.Red
                        End If
                        If CInt(dtDelayVerification.Rows(i)("DelayVerif").ToString()) < 60 Then
                            .GetCellRange(i + 1, DelayVerification.pDelayVerif, i + 1, DelayVerification.pDelayVerif).StyleNew.BackColor = Color.Yellow
                        End If
                        .AutoSizeCols()
                    Next
                    lblTotalDelayVerification.Text = "Total : " & dtDelayVerification.Rows.Count & " Record"
                    .Redraw = True
                End With
            End If
        End If
    End Sub

    Private Sub setCombo(ByVal txt As String, ByVal combo As System.Windows.Forms.ComboBox)
        If combo.InvokeRequired Then
            Dim d As New SetComboCallBack(AddressOf setCombo)
            Me.Invoke(d, New Object() {txt, combo})
        Else
            combo.Text = txt
            combo.Enabled = False
        End If
    End Sub

    Public Sub StopThread()
        SyncLock accessLock
            endThread = True
        End SyncLock
    End Sub

#End Region

End Class