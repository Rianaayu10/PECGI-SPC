Imports C1.Win
Imports C1.Win.C1FlexGrid

Public Class frmSPCAlertDashboard

#Region "Declare"

    Private Enum GrdDelay
        Action = 0
        Type = 1
        MachineProcess = 2
        ItemCheck = 3
        gDate = 4
        Shift = 5
        Seq = 6
        ScheduleStart = 7
        ScheduleEnd = 8
        Delay = 9
        Alert = 10
        Count = 11
    End Enum
    Private Enum GrdNG
        Action = 0
        Type = 1
        MachineProcess = 2
        ItemCheck = 3
        gDate = 4
        Shift = 5
        Seq = 6
        USL = 7
        LSL = 8
        UCL = 9
        LCL = 10
        MIN = 11
        MAX = 12
        AVE = 13
        gOperator = 14
        MK = 15
        QC = 16
        Alert = 17
        Count = 18
    End Enum

#End Region

#Region "Init & Load"
    Private Sub frmSPCAlertDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        up_GridDelayHeader()
        up_GridNGHeader()
    End Sub

#End Region

#Region "Sub & Function"
    Private Sub up_GridDelayHeader()
        Dim IRow As Integer = 1
        Dim ICol As Integer

        With gridDelayInput
            .Rows.Fixed = 1
            .Rows.Count = 1
            .Cols.Fixed = 0
            '.Cols.Frozen = 3
            .Cols.Count = GrdDelay.Count

            .Item(0, GrdDelay.Action) = "Action"
            .Item(0, GrdDelay.Type) = "Type"
            .Item(0, GrdDelay.MachineProcess) = "Machine Process"
            .Item(0, GrdDelay.ItemCheck) = "Item Check"
            .Item(0, GrdDelay.gDate) = "Date"
            .Item(0, GrdDelay.Shift) = "Shift"
            .Item(0, GrdDelay.Seq) = "Seq"
            .Item(0, GrdDelay.ScheduleStart) = "Schedule Start"
            .Item(0, GrdDelay.ScheduleEnd) = "Schedule End"
            .Item(0, GrdDelay.Delay) = "Delay (Minutes)"
            .Item(0, GrdDelay.Alert) = "Alert"

            .Cols(GrdDelay.Action).Width = 100
            .Cols(GrdDelay.Type).Width = 100
            .Cols(GrdDelay.MachineProcess).Width = 170
            .Cols(GrdDelay.ItemCheck).Width = 170
            .Cols(GrdDelay.gDate).Width = 100
            .Cols(GrdDelay.Shift).Width = 50
            .Cols(GrdDelay.Seq).Width = 50
            .Cols(GrdDelay.ScheduleStart).Width = 60
            .Cols(GrdDelay.ScheduleEnd).Width = 60
            .Cols(GrdDelay.Delay).Width = 60
            .Cols(GrdDelay.Alert).Width = 100

            .Rows(0).Height = 50
            .Styles.Normal.WordWrap = True
            .ExtendLastCol = False
            .AllowEditing = True
            .GetCellRange(.Rows.Count - 1, GrdDelay.Action, .Rows.Count - 1, GrdDelay.Count - 1).StyleNew.BackColor = Color.LightGray

            .AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
            .BackColor = Drawing.Color.LemonChiffon
            .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
            .SelectionMode = SelectionModeEnum.Cell
            .Cols(GrdDelay.Alert).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.LeftCenter
            For ICol = GrdDelay.Type To GrdDelay.ItemCheck
                .Cols(ICol).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.LeftCenter
            Next
            .Cols(GrdDelay.Action).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdDelay.gDate).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdDelay.Shift).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdDelay.Seq).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdDelay.Delay).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter

            .Cols(GrdDelay.ScheduleStart).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.RightCenter
            .Cols(GrdDelay.ScheduleEnd).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.RightCenter
        End With
    End Sub
    Private Sub up_GridNGHeader()
        Dim IRow As Integer = 1
        Dim ICol As Integer

        With gridNGResult
            .Rows.Fixed = 1
            .Rows.Count = 1
            .Cols.Fixed = 0
            '.Cols.Frozen = 3
            .Cols.Count = GrdNG.Count

            .Item(0, GrdNG.Action) = "Action"
            .Item(0, GrdNG.Type) = "Type"
            .Item(0, GrdNG.MachineProcess) = "Machine Process"
            .Item(0, GrdNG.ItemCheck) = "Item Check"
            .Item(0, GrdNG.gDate) = "Date"
            .Item(0, GrdNG.Shift) = "Shift"
            .Item(0, GrdNG.Seq) = "Seq"
            .Item(0, GrdNG.USL) = "USL"
            .Item(0, GrdNG.LSL) = "LSL"
            .Item(0, GrdNG.UCL) = "UCL"
            .Item(0, GrdNG.LCL) = "LCL"
            .Item(0, GrdNG.MIN) = "MIN"
            .Item(0, GrdNG.MAX) = "MAX"
            .Item(0, GrdNG.AVE) = "AVE"
            .Item(0, GrdNG.gOperator) = "Operator"
            .Item(0, GrdNG.MK) = "MK"
            .Item(0, GrdNG.QC) = "QC"
            .Item(0, GrdNG.Alert) = "Alert"

            .Cols(GrdNG.Action).Width = 100
            .Cols(GrdNG.Type).Width = 100
            .Cols(GrdNG.MachineProcess).Width = 170
            .Cols(GrdNG.ItemCheck).Width = 170
            .Cols(GrdNG.gDate).Width = 100
            .Cols(GrdNG.Shift).Width = 50
            .Cols(GrdNG.Seq).Width = 50
            .Cols(GrdNG.USL).Width = 50
            .Cols(GrdNG.LSL).Width = 50
            .Cols(GrdNG.UCL).Width = 50
            .Cols(GrdNG.LCL).Width = 50
            .Cols(GrdNG.MIN).Width = 50
            .Cols(GrdNG.MAX).Width = 50
            .Cols(GrdNG.AVE).Width = 50
            .Cols(GrdNG.gOperator).Width = 60
            .Cols(GrdNG.MK).Width = 50
            .Cols(GrdNG.QC).Width = 50
            .Cols(GrdNG.Alert).Width = 100

            .Rows(0).Height = 50
            .Styles.Normal.WordWrap = True
            .ExtendLastCol = False
            .AllowEditing = True
            .GetCellRange(.Rows.Count - 1, GrdNG.Action, .Rows.Count - 1, GrdNG.Count - 1).StyleNew.BackColor = Color.LightGray

            .AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
            .BackColor = Drawing.Color.LemonChiffon
            .Styles.Fixed.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
            .SelectionMode = SelectionModeEnum.Cell

            .Cols(GrdNG.Alert).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.LeftCenter
            For ICol = GrdNG.Type To GrdNG.ItemCheck
                .Cols(ICol).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.LeftCenter
            Next
            .Cols(GrdNG.Action).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.gDate).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.Shift).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.Seq).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.USL).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.LSL).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.UCL).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.LCL).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.MIN).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.MAX).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(GrdNG.AVE).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter

            .Cols(GrdNG.gOperator).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.RightCenter
            .Cols(GrdNG.MK).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.RightCenter
            .Cols(GrdNG.QC).TextAlign = C1.Win.C1FlexGrid.ImageAlignEnum.RightCenter


        End With

    End Sub
#End Region

#Region "Event"

#End Region

End Class