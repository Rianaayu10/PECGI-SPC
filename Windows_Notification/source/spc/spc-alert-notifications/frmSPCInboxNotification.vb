Imports System.Windows.Forms
Imports System.Transactions
Imports System.Drawing

Public Class frmSPCInboxNotification

#Region "Declare"
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
        pScheduleStart = 6
        pScheduleEnd = 7
        pDelayMinutes = 8
        Count = 9
    End Enum

    Private dtNG As DataTable
    Private dtDelayInput As DataTable
    Private dtDelayVerification As DataTable
    Private factory As String

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
        GridHeader()
        GridLoad()
        setFactory()
    End Sub
#End Region

#Region "Control-Event"

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        setDateTime()
    End Sub

#End Region

#Region "Sub and Function"

    Private Sub setFactory()
        cboFactory.Text = factory
        cboFactory.Enabled = False
    End Sub

    Private Sub setDateTime()
        lblDate.Text = Date.Now.ToString("dd MMM yyyy")
        lblTime.Text = Date.Now.ToString("HH:mm:ss")
    End Sub

    Private Sub GridHeader()
        NGHeader()
        DelayHeader()
        VerificationHeader()
    End Sub

    Private Sub NGHeader()
        With gridNGResult
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
            .GetCellRange(0, NGResult.pType, 0, NGResult.pQC).StyleNew.BackColor = Color.LightGray

            .AllowEditing = False
            .Styles.Normal.WordWrap = True
            .ExtendLastCol = False
            For j As Integer = NGResult.pType To NGResult.Count - 1
                .AutoSizeCol(j)
            Next
        End With
    End Sub
    Private Sub DelayHeader()
        With gridDelayInput
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

            .GetCellRange(0, DelayInput.pType, 0, DelayInput.pDelayMinutes).StyleNew.BackColor = Color.LightGray

            .AllowEditing = False
            .Styles.Normal.WordWrap = True
            .ExtendLastCol = False
            For j As Integer = DelayInput.pType To DelayInput.Count - 1
                .AutoSizeCol(j)
            Next
        End With
    End Sub
    Private Sub VerificationHeader()
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
            .Item(0, DelayVerification.pScheduleStart) = "Schedule Start"
            .Item(0, DelayVerification.pScheduleEnd) = "Schedule End"
            .Item(0, DelayVerification.pDelayMinutes) = "Delay (Minutes)"

            '.Cols(grdHeader.datetime).Width = 150
            '.Cols(grdHeader.datetime).Width = 450

            '.AutoSizeCols()

            .Cols(DelayVerification.pType).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(DelayVerification.pMachineProcess).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(DelayVerification.pItemCheck).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(DelayVerification.pDate).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(DelayVerification.pShift).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(DelayVerification.pSeq).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(DelayVerification.pScheduleStart).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(DelayVerification.pScheduleEnd).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(DelayVerification.pDelayMinutes).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

            .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

            .GetCellRange(0, DelayVerification.pType, 0, DelayVerification.pDelayMinutes).StyleNew.BackColor = Color.LightGray

            .AllowEditing = False
            .Styles.Normal.WordWrap = True
            .ExtendLastCol = False
            For j As Integer = DelayVerification.pType To DelayVerification.Count - 1
                .AutoSizeCol(j)
            Next
        End With
    End Sub

    Private Sub GridLoad()
        NGLoad()
        DelayInputLoad()
        DelayVerificationLoad()
    End Sub

    Private Sub NGLoad()
        With gridNGResult
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
        End With
    End Sub
    Private Sub DelayInputLoad()
        With gridDelayInput
            '.DataSource = dtDelayInput
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
                .Item(i + 1, DelayInput.pDelayMinutes) = dtDelayInput.Rows(i)("Delay").ToString()

                If CInt(.Item(i + 1, DelayInput.pDelayMinutes)) > 60 Then
                    .GetCellRange(i + 1, DelayInput.pDelayMinutes, i + 1, DelayInput.pDelayMinutes).StyleNew.BackColor = Color.Red
                End If
                If CInt(.Item(i + 1, DelayInput.pDelayMinutes)) < 60 Then
                    .GetCellRange(i + 1, DelayInput.pDelayMinutes, i + 1, DelayInput.pDelayMinutes).StyleNew.BackColor = Color.Yellow
                End If

                .AutoSizeCols()
            Next
        End With
    End Sub
    Private Sub DelayVerificationLoad()
        With gridDelayVerification
            For i = 0 To dtDelayVerification.Rows.Count - 1
                .AddItem("")
                .Item(0, DelayVerification.pType) = "Type"
                .Item(0, DelayVerification.pMachineProcess) = "Machine Process"
                .Item(0, DelayVerification.pItemCheck) = "Item Check"
                .Item(0, DelayVerification.pDate) = "Date"
                .Item(0, DelayVerification.pShift) = "Shift"
                .Item(0, DelayVerification.pSeq) = "Seq"
                .Item(0, DelayVerification.pScheduleStart) = "Schedule Start"
                .Item(0, DelayVerification.pScheduleEnd) = "Schedule End"
                .Item(0, DelayVerification.pDelayMinutes) = "Delay (Minutes)"
                .AutoSizeCols()
            Next
        End With
    End Sub

#End Region

End Class