Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data
Imports System.Data.Odbc
Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop.Excel.XlBordersIndex
Imports Microsoft.Office.Interop.Excel.XlLineStyle
Imports Microsoft.Office.Interop.Excel.XlBorderWeight
Imports Microsoft.Office.Interop.Excel.Constants
Imports System.Text.RegularExpressions
Public Class frmScheduler
#Region "Declaration"
    Dim st As New clsConfig
    Dim NoData As Boolean = False
    Dim LastCalculation As Date
    Dim LastEmail As Date
    Dim VDir As String = "D:\Interface\"
    Dim BackUP As String = st.m_SLtoEZRBackUpFolder
    Dim DestDir As String = st.m_EZRtoSLFolder
    Dim DestStockTransfer As String = st.m_EZRStockTransferFolder
    Dim DestDirBU As String = st.m_EZRtoSLBackUpFolder
    Dim LastMsg As String = ""
    Dim ItemCSV As String = "Item_Master.csv"
    Dim TradeCSV As String = "Trade_Master.csv"
    Dim RcvCSV As String = "PurchOrd.csv"
    Private Const fTgl As String = "yyyyMMdd"
    Private Const fJam As String = "HHmm"
    Dim FileTitle1 As String = ""
    Dim FileTitle2 As String = ""
    Public ConStr As String = st.m_ConnectionString
    Dim pErr As String
    Dim dtExec As DateTime
    Dim iValue As Integer
    Dim iFileCount As Integer
    Dim iFolder As String
    Dim currentFile As String
    Dim filecontents As String
    Dim file As String
    Dim afile As FileIO.TextFieldParser


    Enum MSGINFO
        NORMAL = 0
        ERR = 1
        SUCCESS = 2
        WARNING = 3
    End Enum
#End Region

#Region "Procedure"
    Private Sub ShowLog(ByVal msg As String, Optional ByVal pMSGINFO As MSGINFO = MSGINFO.NORMAL)
        Dim Time As String = Microsoft.VisualBasic.Format(Date.Now, "dd-MMM-yyyy HH:mm:ss")
        Dim lItem As ListViewItem
        If msg = "" Then
            lItem = lvwLog.Items.Add("")
        Else
            lItem = lvwLog.Items.Add(Time)
        End If
        lItem.SubItems.Add(msg)
        If pMSGINFO = MSGINFO.ERR Then
            lItem.SubItems(0).ForeColor = Color.Red
        ElseIf pMSGINFO = MSGINFO.SUCCESS Then
            lItem.SubItems(0).ForeColor = Color.Green
        ElseIf pMSGINFO = MSGINFO.WARNING Then
            lItem.SubItems(0).ForeColor = Color.Orange
        End If
        lItem.Selected = True
        lItem.EnsureVisible()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub EnableTime(ByVal Enable As Boolean)
        dtpProcessTime.Enabled = Enable
    End Sub
    Private Sub LoadStartTime()
        Try
            Dim keyValue As Object
            'keyValue = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\TOS\FA\Scheduler", "StartTime", Nothing)
            'If keyValue IsNot Nothing Then
            '    dtpProcessTime.Value = keyValue
            'Else
            '    dtpProcessTime.Value = Now
            'End If
        Catch ex As Exception
            ShowLog(ex.Message, MSGINFO.ERR)
        End Try
    End Sub
    Private Sub SaveStartTime()
        Try
            Dim Today As String = Microsoft.VisualBasic.Format(Now.Date, "yyyy-MM-dd")
            Dim Value As String = Today & " " & Microsoft.VisualBasic.Format(dtpProcessTime.Value, "HH:mm")
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\TOS\FA\Scheduler", "StartTime", Value)
        Catch ex As Exception
            ShowLog(ex.Message, MSGINFO.ERR)
        End Try
    End Sub
    Private Sub up_Scheduler()
        Dim UserTo As String
        iFolder = st.m_SLtoEZRFolder
        If chkNGResult.Checked = True Then
            Try
                Dim FactoryCode As String = cboFactory.SelectedValue
                Dim Alertlist As List(Of clsAlertDashboard) = clsAlertDashboardDB.GetListNGResult(FactoryCode, ConStr)

                For Each AlertData In Alertlist

                    ' Send Email
                    Dim CheckDataEmail As DataTable = clsAlertDashboardDB.CheckDataSendEmailAlert(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, ConStr)

                    UserTo = clsAlertDashboardDB.GetUserLine(ConStr, FactoryCode, AlertData.LineCode, "1")

                    If CheckDataEmail.Rows.Count <= 0 Then
                        Dim Factory As String = AlertData.FactoryCode + " - " + AlertData.FactoryName
                        Dim CountSendEmail As Integer = clsAlertDashboardDB.SendEmail(Factory, AlertData.ItemTypeCode, AlertData.LineName, AlertData.ItemCheckName, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "1", AlertData.LSL, AlertData.USL, AlertData.LCL, AlertData.UCL, AlertData.MinValue, AlertData.MaxValue, AlertData.Average, "NG", AlertData.ScheduleStart, AlertData.ScheduleEnd, "", AlertData.DelayTime, ConStr, UserTo)
                    Else
                        'Nothing Happens Here, Go Back To Your WorkTable
                    End If

                    ' Send Notification
                    Dim CheckDataNotification As DataTable = clsAlertDashboardDB.CheckDataSendNotificationAlert(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "NG", ConStr)

                    If CheckDataNotification.Rows.Count <= 0 Then
                        clsAlertDashboardDB.SendNotification(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "NG", ConStr)
                    Else
                        'Nothing again here, Go Back 
                    End If
                Next


            Catch ex As Exception
                Throw New Exception(ex.ToString())
            End Try
        End If
        If chkDelayVerification.Checked = True Then
            Try
                Dim pErr As String
                Dim FactoryCode As String = cboFactory.SelectedValue
                Dim Alertlist As List(Of clsAlertDashboard) = clsAlertDashboardDB.GetListDelayVerification(FactoryCode, ConStr, pErr)

                If pErr <> "" Then
                    ShowLog(pErr)
                Else

                    For Each AlertData In Alertlist

                        If AlertData.MK.ToString() = "" AndAlso AlertData.QC.ToString() = "" Then
                            UserTo = clsAlertDashboardDB.GetUserLine(ConStr, FactoryCode, AlertData.LineCode, "3")
                        ElseIf AlertData.MK.ToString() = "" Then
                            UserTo = clsAlertDashboardDB.GetUserLine(ConStr, FactoryCode, AlertData.LineCode, "2")
                        End If

                        ' Send Email
                        Dim CheckDataEmail As DataTable = clsAlertDashboardDB.CheckDataSendEmailAlert(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, ConStr)

                        If CheckDataEmail.Rows.Count <= 0 Then
                            Dim Factory As String = AlertData.FactoryCode + " - " + AlertData.FactoryName
                            Dim CountSendEmail As Integer = clsAlertDashboardDB.SendEmail(Factory, AlertData.ItemTypeCode, AlertData.LineName, AlertData.ItemCheckName, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "3", AlertData.LSL, AlertData.USL, AlertData.LCL, AlertData.UCL, AlertData.MinValue, AlertData.MaxValue, AlertData.Average, AlertData.Status, "", "", AlertData.VerifTime, AlertData.DelayTime, ConStr, UserTo)
                        Else
                            'Nothing Happens Here, Go Back To Your WorkTable
                        End If

                        ' Send Notification
                        Dim CheckDataNotification As DataTable = clsAlertDashboardDB.CheckDataSendNotificationAlert(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "DV", ConStr)

                        If CheckDataNotification.Rows.Count <= 0 Then
                            clsAlertDashboardDB.SendNotification(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "DV", ConStr)
                        Else
                            'Nothing again here, Go Back 
                        End If
                    Next

                End If



            Catch ex As Exception
                Throw New Exception(ex.ToString())
            End Try
        End If
        If chkDelayInput.Checked Then
            Try
                Dim FactoryCode As String = cboFactory.SelectedValue
                Dim Alertlist As List(Of clsAlertDashboard) = clsAlertDashboardDB.GetListDelayInput(FactoryCode, ConStr)


                For Each AlertData In Alertlist

                    ' Send Email
                    Dim CheckDataEmail As DataTable = clsAlertDashboardDB.CheckDataSendEmailAlert(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, ConStr)

                    UserTo = clsAlertDashboardDB.GetUserLine(ConStr, FactoryCode, AlertData.LineCode, "1")

                    If CheckDataEmail.Rows.Count <= 0 Then
                        Dim Factory As String = AlertData.FactoryCode + " - " + AlertData.FactoryName
                        Dim CountSendEmail As Integer = clsAlertDashboardDB.SendEmail(Factory, AlertData.ItemTypeCode, AlertData.LineName, AlertData.ItemCheckName, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "2", "", "", "", "", "", "", "", "", AlertData.ScheduleStart, AlertData.ScheduleEnd, "", AlertData.DelayTime, ConStr, UserTo)
                    Else
                        'Nothing Happens Here, Go Back To Your WorkTable
                    End If

                    ' Send Notification
                    Dim CheckDataNotification As DataTable = clsAlertDashboardDB.CheckDataSendNotificationAlert(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "DI", ConStr)

                    If CheckDataNotification.Rows.Count <= 0 Then
                        clsAlertDashboardDB.SendNotification(AlertData.FactoryCode, AlertData.ItemTypeCode, AlertData.LineCode, AlertData.ItemCheckCode, AlertData.ProdDate, AlertData.ShiftCode, AlertData.SequenceNo, "DI", ConStr)
                    Else
                        'Nothing again here, Go Back 
                    End If
                Next


            Catch ex As Exception
                Throw New Exception(ex.ToString())
            End Try
        End If
    End Sub

#End Region

#Region "Function"

#End Region

#Region "Event"
    Private Sub btnStart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStart.Click
        If chkDelayVerification.Checked = False Then
            If chkNGResult.Checked = False Then
                If chkDelayInput.Checked = False Then

                    MsgBox("Please Select at Least One Option..", MsgBoxStyle.Exclamation, "Cannot Start")
                    Exit Sub

                End If
            End If
        End If

        btnStart.Enabled = False
        btnStop.Enabled = True
        btnClear.Enabled = False
        cboFactory.Enabled = False
        txtFactory.Enabled = False

        dtExec = Microsoft.VisualBasic.Format(Now.AddMinutes(st.m_IntervalValue), "yyyy-MM-dd HH:mm:ss")

        EnableTime(False)
        chkNGResult.Enabled = False
        chkDelayVerification.Enabled = False
        chkNGResult.Enabled = False
        chkDelayInput.Enabled = False

        lblStart.Text = "STARTED"
        lblStart.BackColor = Color.MediumSeaGreen
        ShowLog("Scheduler start")
        SaveStartTime()
        tmrExecute.Start()
    End Sub
    Private Sub btnStop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStop.Click
        btnStart.Enabled = True
        btnStop.Enabled = False
        btnClear.Enabled = True
        txtFactory.Enabled = True
        cboFactory.Enabled = True

        chkNGResult.Enabled = True
        chkDelayVerification.Enabled = True

        chkDelayInput.Enabled = True

        EnableTime(True)
        lblStart.Text = "STOPPED"
        lblStart.BackColor = Color.Gray

        ShowLog("Scheduler stop")

        tmrExecute.Enabled = False
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        lvwLog.Items.Clear()
    End Sub

    Private Sub btnManual_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            up_Scheduler()
            'ProcessScheduler()
        Catch ex As Exception
            ShowLog(ex.Message)
        End Try
    End Sub

    Private Sub tmrExecute_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrExecute.Tick
        Try

            If Microsoft.VisualBasic.Format(dtExec, "yyyy-MM-dd HH:mm") = Microsoft.VisualBasic.Format(Date.Now, "yyyy-MM-dd HH:mm") Then
                up_Scheduler()
                dtExec = Microsoft.VisualBasic.Format(dtExec.AddMinutes(st.m_IntervalValue), "yyyy-MM-dd HH:mm:ss")
            End If

        Catch ex As Exception
            Dim ErrMsg As String = ex.Message
            If LastMsg <> ErrMsg Then
                LastMsg = ErrMsg
                ShowLog(ErrMsg, MSGINFO.ERR)
            End If
        Finally
            tmrExecute.Enabled = True
        End Try
    End Sub

    Private Sub frmScheduler_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblVer.Text = "ver" & System.Windows.Forms.Application.ProductVersion
        LastCalculation = DateAdd(DateInterval.Day, -1, Now.Date)
        LastEmail = DateAdd(DateInterval.Day, -1, Now.Date)
        dtExec = Date.Now
        dtExec = Microsoft.VisualBasic.Format(Now.AddMinutes(st.m_IntervalValue), "yyyy-MM-dd HH:mm:ss")
        dtpProcessTime.Value = Now
        LoadStartTime()
        FillCboFactory()
    End Sub

    Private Sub FillCboFactory()
        cboFactory.DataSource = clsAlertDashboardDB.FillComboFactoryGrid(ConStr)
        cboFactory.ValueMember = "FactoryCode"
        cboFactory.DisplayMember = "FactoryName"
        cboFactory.SelectedIndex = 0
    End Sub

    Private Sub chkPackingList_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

#End Region

End Class
