Public Class frmInboxNotifications

#Region "Declare"
    Private Enum grdHeader
        datetime = 0
        inbox = 1
        count = 2
    End Enum

    Private dtNG As DataTable
    Private dtDelayInput As DataTable
    Private dtDelayVerification As DataTable

#End Region

#Region "Init"
    Public Sub New(ByVal pdtNG As DataTable, ByVal pdtDelayInput As DataTable, ByVal pdtDelayVerification As DataTable)
        InitializeComponent()
        dtNG = pdtNG
        dtDelayInput = pdtDelayInput
        dtDelayVerification = pdtDelayVerification

    End Sub

    Private Sub frmInboxNotifications_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tabNGInput.Text = "NG INPUT (" & dtNG.Rows.Count & ")"
        tabDelayInput.Text = "DELAY INPUT (" & dtDelayInput.Rows.Count & ")"
        tabDelayVerification.Text = "DELAY VERIFICATION (" & dtDelayVerification.Rows.Count & ")"
        GridHeader()
        GridLoad()
    End Sub
#End Region

#Region "Control-Event"
    Private Sub pctClose_Click(sender As Object, e As EventArgs) Handles pctClose.Click
        Close()
    End Sub
#End Region

#Region "Sub and Function"

    Private Sub GridHeader()
        NGHeader()
        DelayHeader()
        VerificationHeader()
    End Sub

    Private Sub NGHeader()
        'With gridNG
        '    .Rows.Fixed = 1
        '    .Rows.Count = 1
        '    .Cols.Fixed = 0
        '    .Cols.Count = grdHeader.count

        '    .Item(0, grdHeader.datetime) = "Date Time"
        '    .Item(0, grdHeader.inbox) = "Inbox"

        '    .Cols(grdHeader.datetime).Width = 150
        '    '.Cols(grdHeader.datetime).Width = 450

        '    .Cols(grdHeader.datetime).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(grdHeader.inbox).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        '    .AllowEditing = False
        '    .Styles.Normal.WordWrap = True
        'End With
    End Sub
    Private Sub DelayHeader()
        'With gridInput
        '    .Rows.Fixed = 1
        '    .Rows.Count = 1
        '    .Cols.Fixed = 0
        '    .Cols.Count = grdHeader.count

        '    .Item(0, grdHeader.datetime) = "Date Time"
        '    .Item(0, grdHeader.inbox) = "Inbox"

        '    .Cols(grdHeader.datetime).Width = 150
        '    '.Cols(grdHeader.datetime).Width = 450

        '    .Cols(grdHeader.datetime).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(grdHeader.inbox).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        '    .AllowEditing = False
        '    .Styles.Normal.WordWrap = True
        'End With
    End Sub
    Private Sub VerificationHeader()
        'With gridVerification
        '    .Rows.Fixed = 1
        '    .Rows.Count = 1
        '    .Cols.Fixed = 0
        '    .Cols.Count = grdHeader.count

        '    .Item(0, grdHeader.datetime) = "Date Time"
        '    .Item(0, grdHeader.inbox) = "Inbox"

        '    .Cols(grdHeader.datetime).Width = 150
        '    '.Cols(grdHeader.datetime).Width = 450

        '    .Cols(grdHeader.datetime).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Cols(grdHeader.inbox).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        '    .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        '    .AllowEditing = False
        '    .Styles.Normal.WordWrap = True
        'End With
    End Sub

    Private Sub GridLoad()
        NGLoad()
        DelayInputLoad()
        DelayVerificationLoad()
    End Sub

    Private Sub NGLoad()
        With gridNG
            .DataSource = dtNG
            'For i = 0 To dtNG.Rows.Count - 1
            '    .AddItem("")
            'Next
        End With
    End Sub
    Private Sub DelayInputLoad()
        With gridInput
            .DataSource = dtDelayInput
            'For i = 0 To dtDelayInput.Rows.Count - 1
            '    .AddItem("")
            'Next
        End With
    End Sub
    Private Sub DelayVerificationLoad()
        With gridVerification
            .DataSource = dtDelayVerification

            'For i = 0 To dtDelayVerification.Rows.Count - 1
            '    .AddItem("")
            'Next
        End With
    End Sub

#End Region

End Class