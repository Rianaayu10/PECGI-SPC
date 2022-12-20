Imports Tulpep.NotificationWindow
Imports System.Net
Imports System.IO
Imports C1.Win.C1List
Imports Microsoft.Win32

Public Class frmSPCAlertNotifications

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

    Private ConnectionString As String

    Dim config As clsConfig
    Dim NewEnryption As New clsDESEncryption("TOS")
    Dim ls_path As String = AddSlash(My.Application.Info.DirectoryPath) & "config.xml"

    Dim ProcessRunning As Boolean = False
    Dim NGInputRowsCount As Integer = 0
    Dim DelayInputRowsCount As Integer = 0
    Dim DelayVerificationRowsCount As Integer = 0
    'Dim processRunning As Booleann = False

    Dim NGInputLastUpdate As String
    Dim DelayInputLastUpdate As String
    Dim DelayVerificationLastUpdate As String

    Dim currentNGLastUpdate As String
    Dim currentDelayInput As String
    Dim currentDelayVerification As String
    Dim firstLoad As Integer = 0
    Dim firstballonLoad As Integer = 0

    Private Sub frmSPCAlertNotifications_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Timer1.Start()
    End Sub
#End Region

#Region "Event"
    Public Function AddSlash(ByVal Path As String) As String
        Dim Result As String = Path
        If Path.EndsWith("\") = False Then
            Result = Result + "\"
        End If
        Return Result
    End Function

    Private Sub InboxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NotificationToolStripMenuItem.Click
        'Dim frm As New frmSPCInboxNotification(dtNG, dtDelayInput, dtDelayVerification, factory)
        'frm.Show()
        Dim Path As String = AddSlash(My.Application.Info.DirectoryPath) & "SPCNotification.exe"
        For Each p As Process In Process.GetProcessesByName("SPCNotification")
            p.Refresh()
            p.Kill()
            p.Close()
        Next
        Path = AddSlash(My.Application.Info.DirectoryPath) & "SPCNotification.exe"
        Process.Start(Path)
        'ContextMenuStrip1.Visible = False
    End Sub

    Private Sub SettingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingToolStripMenuItem.Click
        frmLoginSettings.Show()
        'ContextMenuStrip1.Visible = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
        'ContextMenuStrip1.Hide()
    End Sub

    Private Sub frmSPCAlertNotifications_Move(sender As Object, e As EventArgs) Handles MyBase.Move
        Me.Hide()
        'ContextMenuStrip1.Visible = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If ProcessRunning = False Then
            config = New clsConfig
            up_AppSettingLoad(ls_path)
            ConnectionString = config.ConnectionString

            up_GetData()

            'ProcessRunning = False
        End If
    End Sub

    Private Sub NotifyIcon1_Click(sender As Object, ByVal e As EventArgs)
        Try
            'Timer1.Enabled = False
            'Timer1.Stop()
            If firstballonLoad > 0 Then
                'If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim Path As String = AddSlash(My.Application.Info.DirectoryPath) & "SPCNotification.exe"
                For Each p As Process In Process.GetProcessesByName("SPCNotification")
                    p.Refresh()
                    p.Kill()
                    p.Close()
                Next
                Path = AddSlash(My.Application.Info.DirectoryPath) & "SPCNotification.exe"
                Process.Start(Path)
            End If
            firstballonLoad = 1
            'Else

            'End If
            'Dim frm As New frmSPCInboxNotification(dtNG, dtDelayInput, dtDelayVerification, factory) 'frmInboxNotifications(dtNG, dtDelayInput, dtDelayVerification)
            'frm.Show()
            'Close()
        Catch ex As Exception
            MsgBox("Something error.", MsgBoxStyle.OkOnly, "Error!")
        End Try
    End Sub

    Private Sub NotifyShowing_Click(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Try
            'Timer1.Enabled = False
            'Timer1.Stop()
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim Path As String = AddSlash(My.Application.Info.DirectoryPath) & "SPCNotification.exe"
                For Each p As Process In Process.GetProcessesByName("SPCNotification")
                    p.Refresh()
                    p.Kill()
                    p.Close()
                Next
                Path = AddSlash(My.Application.Info.DirectoryPath) & "SPCNotification.exe"
                Process.Start(Path)
                'Else
                '    ContextMenuStrip1.Show(MousePosition)
            End If
            
            'Dim frm As New frmSPCInboxNotification(dtNG, dtDelayInput, dtDelayVerification, factory) 'frmInboxNotifications(dtNG, dtDelayInput, dtDelayVerification)
            'frm.Show()
            'Close()
        Catch ex As Exception
            MsgBox("Something error.", MsgBoxStyle.OkOnly, "Error!")
        End Try

    End Sub
#End Region

#Region "LoadDB & Notification"
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
            'If StartUp = True Then
            '    Dim reg As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            '    reg.SetValue("SPC Alert Notification", Application.ExecutablePath.ToString())
            'End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Exclamation, "Warning")
        End Try
    End Sub

    Public Sub GetNotify(ByRef pNGInputLastUpdate As String, ByRef pDelayInputLastUpdate As String, ByRef pDelayVerificationLastUpdate As String, pConstr As String, pUserLogin As String,
                         ByRef pcurrentNGLastUpdate As String, ByRef pcurrentDelayInput As String, ByRef pcurrentDelayVerification As String)
        ProcessRunning = False
        firstLoad = 1
        'NGInputRowsCount = 0
        'DelayInputRowsCount = 0
        'DelayVerificationRowsCount = 0
        ConnectionString = pConstr
        UserIDLogin = pUserLogin

        NGInputLastUpdate = pNGInputLastUpdate
        DelayInputLastUpdate = pDelayInputLastUpdate
        DelayVerificationLastUpdate = pDelayVerificationLastUpdate

        up_GetData()
        pNGInputLastUpdate = NGInputLastUpdate
        pDelayInputLastUpdate = DelayInputLastUpdate
        pDelayVerificationLastUpdate = DelayVerificationLastUpdate
        pcurrentNGLastUpdate = currentNGLastUpdate
        pcurrentDelayInput = currentDelayInput
        pcurrentDelayVerification = currentDelayVerification
    End Sub

    Private Sub up_GetData()
        'NG = NG Result
        'DV = Delay Verification
        'DI = Delay Input

        If ProcessRunning = False Then
            Try
                If firstLoad = 0 Then
                    dtNG = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "NG")
                    dtDelayInput = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "DelayInput")
                    dtDelayVerification = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "DelayVerification")
                    factory = clsSPCNotification.GetData(ConnectionString, UserIDLogin, "Factory").Rows(0)("Factory")

                    NGInputRowsCount = dtNG.Rows.Count
                    DelayInputRowsCount = dtDelayInput.Rows.Count
                    DelayVerificationRowsCount = dtDelayVerification.Rows.Count

                    Dim header, body As String
                    'Dim strDate As String = dtNG.Rows(i)("Date")
                    'header = "ALERT Notification"
                    body = " there is new NG Result " & vbCrLf &
                           " there is new Delay Input " & vbCrLf &
                           " there is new Delay Verification " ': " & DelayVerificationRowsCount
                    ' : " & NGInputRowsCount & " " & vbCrLf &
                    ' : " & DelayInputRowsCount & " " & vbCrLf &
                    'link = pLink '+ dtNG.Rows(i)("Link")
                    'ShowNotification(header, body, "NG", link)

                    NotifyIcon1.ShowBalloonTip(500, "There are new notifications:", body, ToolTipIcon.Info)
                    'AddHandler NotifyIcon1.Click, AddressOf Me.NotifyShowing_Click
                    AddHandler NotifyIcon1.MouseDown, AddressOf Me.NotifyShowing_Click
                    AddHandler NotifyIcon1.BalloonTipClicked, AddressOf Me.NotifyIcon1_Click

                    If NGInputRowsCount > 0 Then
                        NGInputLastUpdate = Format(CDate(dtNG.Rows(dtNG.Rows.Count - 1)("UpdateDate")), "yyyy-MM-dd HH:mm:ss")
                    End If
                    If DelayInputRowsCount > 0 Then
                        DelayInputLastUpdate = dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("UpdateTime") 'Format(CDate(dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("UpdateTime")), "yyyy-MM-dd HH:mm:ss")
                    End If
                    If DelayVerificationRowsCount > 0 Then
                        DelayVerificationLastUpdate = Format(CDate(dtDelayVerification.Rows(dtDelayVerification.Rows.Count - 1)("UpdateDate")), "yyyy-MM-dd HH:mm:ss")
                    End If

                    firstLoad = 1
                    If firstLoad = 0 Then
                        firstLoad += 1
                    End If

                Else
                    dtNG = clsSPCNotification.CheckNotificationLog(ConnectionString, UserIDLogin, "NG")
                    dtDelayInput = clsSPCNotification.CheckNotificationLog(ConnectionString, UserIDLogin, "DI")
                    dtDelayVerification = clsSPCNotification.CheckNotificationLog(ConnectionString, UserIDLogin, "DV")


                    If dtNG.Rows.Count > 0 Then
                        NGInputRowsCount = dtNG.Rows.Count
                        DelayInputRowsCount = 0
                        DelayVerificationRowsCount = 0
                        currentNGLastUpdate = Format(dtNG.Rows(dtNG.Rows.Count - 1)("LastUpdate"), "yyyy-MM-dd HH:mm:ss")
                    Else
                        NGInputRowsCount = 0
                        DelayInputRowsCount = 0
                        DelayVerificationRowsCount = 0
                        currentNGLastUpdate = NGInputLastUpdate
                    End If
                    If dtDelayInput.Rows.Count > 0 Then
                        NGInputRowsCount = NGInputRowsCount
                        DelayInputRowsCount = dtDelayInput.Rows.Count
                        DelayVerificationRowsCount = 0
                        currentDelayInput = Format(CDate(dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("LastUpdate")), "yyyy-MM-dd HH:mm:ss") 'dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("UpdateTime") 'Format(CDate(dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("LastUpdate")), "yyyy-MM-dd HH:mm:ss")
                    Else
                        NGInputRowsCount = NGInputRowsCount
                        DelayInputRowsCount = 0
                        DelayVerificationRowsCount = 0
                        currentDelayInput = DelayInputLastUpdate
                    End If
                    If dtDelayVerification.Rows.Count > 0 Then
                        NGInputRowsCount = NGInputRowsCount
                        DelayInputRowsCount = DelayInputRowsCount
                        DelayVerificationRowsCount = dtDelayVerification.Rows.Count
                        currentDelayVerification = Format(dtDelayVerification.Rows(dtDelayVerification.Rows.Count - 1)("LastUpdate"), "yyyy-MM-dd HH:mm:ss")
                    Else
                        NGInputRowsCount = NGInputRowsCount
                        DelayInputRowsCount = DelayInputRowsCount
                        DelayVerificationRowsCount = 0
                        currentDelayVerification = DelayVerificationLastUpdate
                    End If

                    Try
                        If Not IsNothing(NGInputLastUpdate) OrElse NGInputLastUpdate <> "" Then
                            If currentNGLastUpdate >= NGInputLastUpdate Then
                                Dim drRemoved As DataRow() = dtNG.Select("LastUpdate <= '" & CDate(NGInputLastUpdate).ToString("yyyy-MM-dd HH:mm:ss") & "'")
                                For i = 0 To drRemoved.Length - 1
                                    dtNG.Rows.Remove(drRemoved(i))
                                Next
                            Else
                                Dim drRemoved As DataRow() = dtNG.Select("LastUpdate <= '" & CDate(NGInputLastUpdate).ToString("yyyy-MM-dd HH:mm:ss") & "'")
                                For i = 0 To drRemoved.Length - 1
                                    dtNG.Rows.Remove(drRemoved(i))
                                Next
                            End If

                        End If
                    Catch ex As Exception

                    End Try
                    Try
                        If Not IsNothing(DelayInputLastUpdate) OrElse DelayInputLastUpdate <> "" Then
                            If currentDelayInput >= DelayInputLastUpdate Then
                                Dim drRemoved1 As DataRow() = dtDelayInput.Select("LastUpdate <= '" & CDate(DelayInputLastUpdate).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") & "'") '.ToString("yyyy-MM-dd HH:mm:ss")
                                For i = 0 To drRemoved1.Length - 1
                                    dtDelayInput.Rows.Remove(drRemoved1(i))
                                Next
                            Else
                                Dim drRemoved1 As DataRow() = dtDelayInput.Select("LastUpdate <= '" & CDate(DelayInputLastUpdate).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") & "'") '.ToString("yyyy-MM-dd HH:mm:ss")
                                For i = 0 To drRemoved1.Length - 1
                                    dtDelayInput.Rows.Remove(drRemoved1(i))
                                Next
                            End If

                        End If
                    Catch ex As Exception

                    End Try
                    Try
                        If Not IsNothing(DelayVerificationLastUpdate) OrElse DelayVerificationLastUpdate <> "" Then
                            If currentDelayVerification >= DelayVerificationLastUpdate Then
                                Dim drRemoved2 As DataRow() = dtDelayVerification.Select("LastUpdate <= '" & CDate(DelayVerificationLastUpdate).ToString("yyyy-MM-dd HH:mm:ss") & "'")
                                For i = 0 To drRemoved2.Length - 1
                                    dtDelayVerification.Rows.Remove(drRemoved2(i))
                                Next
                            Else
                                Dim drRemoved2 As DataRow() = dtDelayVerification.Select("LastUpdate <= '" & CDate(DelayVerificationLastUpdate).ToString("yyyy-MM-dd HH:mm:ss") & "'")
                                For i = 0 To drRemoved2.Length - 1
                                    dtDelayVerification.Rows.Remove(drRemoved2(i))
                                Next
                            End If

                        End If
                    Catch ex As Exception

                    End Try


                    NGInputRowsCount = IIf(dtNG.Rows.Count > 0, dtNG.Rows.Count - 1, dtNG.Rows.Count)
                    DelayInputRowsCount = IIf(dtDelayInput.Rows.Count > 0, dtDelayInput.Rows.Count - 1, dtDelayInput.Rows.Count)
                    DelayVerificationRowsCount = IIf(dtDelayVerification.Rows.Count > 0, dtDelayVerification.Rows.Count - 1, dtDelayVerification.Rows.Count)


                    Try
                        If CDate(currentNGLastUpdate) > CDate(NGInputLastUpdate) Then
                            If firstLoad > 1 Then
                                Dim header, body, link As String
                                'Dim strDate As String = dtNG.Rows(i)("Date")
                                'header = "ALERT Notification"
                                body = " there is new NG Result " '& vbCrLf &
                                '" there is new Delay Input " & vbCrLf &
                                '" there is new Delay Verification " ': " & DelayVerificationRowsCount
                                ' : " & NGInputRowsCount & " " & vbCrLf &
                                ' : " & DelayInputRowsCount & " " & vbCrLf &
                                link = pLink '+ dtNG.Rows(i)("Link")
                                'ShowNotification(header, body, "NG", link)

                                NotifyIcon1.ShowBalloonTip(500, "There are new notifications:", body, ToolTipIcon.Info)
                                'AddHandler NotifyIcon1.Click, AddressOf Me.NotifyShowing_Click
                                AddHandler NotifyIcon1.MouseDown, AddressOf Me.NotifyShowing_Click
                                AddHandler NotifyIcon1.BalloonTipClicked, AddressOf Me.NotifyIcon1_Click

                            End If
                            'If NGInputRowsCount > 0 Then
                            '    NGInputLastUpdate = currentNGLastUpdate 'dtNG.Rows(dtNG.Rows.Count - 1)("LastUpdate")
                            'End If
                            'If DelayInputRowsCount > 0 Then
                            '    DelayInputLastUpdate = currentDelayInput 'dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("LastUpdate")
                            'End If
                            'If DelayVerificationRowsCount > 0 Then
                            '    DelayVerificationLastUpdate = currentDelayVerification 'dtDelayVerification.Rows(dtDelayVerification.Rows.Count - 1)("LastUpdate")
                            'End If
                            NGInputLastUpdate = currentNGLastUpdate
                            DelayInputLastUpdate = currentDelayInput
                            DelayVerificationLastUpdate = currentDelayVerification
                            firstLoad = 2
                        End If
                        If CDate(currentDelayInput) > CDate(DelayInputLastUpdate) Then
                            If firstLoad > 1 Then
                                Dim header, body, link As String
                                'Dim strDate As String = dtNG.Rows(i)("Date")
                                'header = "ALERT Notification"
                                body = " there is new Delay Input " '& vbCrLf &
                                '" there is new Delay Verification " ': " & DelayVerificationRowsCount
                                ' : " & NGInputRowsCount & " " & vbCrLf &
                                ' : " & DelayInputRowsCount & " " & vbCrLf &
                                link = pLink '+ dtNG.Rows(i)("Link")
                                'ShowNotification(header, body, "NG", link)

                                NotifyIcon1.ShowBalloonTip(500, "There are new notifications:", body, ToolTipIcon.Info)
                                'AddHandler NotifyIcon1.Click, AddressOf Me.NotifyShowing_Click
                                AddHandler NotifyIcon1.MouseDown, AddressOf Me.NotifyShowing_Click
                                AddHandler NotifyIcon1.BalloonTipClicked, AddressOf Me.NotifyIcon1_Click

                            End If
                            'If NGInputRowsCount > 0 Then
                            '    NGInputLastUpdate = currentNGLastUpdate 'dtNG.Rows(dtNG.Rows.Count - 1)("LastUpdate")
                            'End If
                            'If DelayInputRowsCount > 0 Then
                            '    DelayInputLastUpdate = currentDelayInput 'dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("LastUpdate")
                            'End If
                            'If DelayVerificationRowsCount > 0 Then
                            '    DelayVerificationLastUpdate = currentDelayVerification 'dtDelayVerification.Rows(dtDelayVerification.Rows.Count - 1)("LastUpdate")
                            'End If
                            NGInputLastUpdate = currentNGLastUpdate
                            DelayInputLastUpdate = currentDelayInput
                            DelayVerificationLastUpdate = currentDelayVerification
                            firstLoad = 2
                        End If
                        If CDate(currentDelayVerification) > CDate(DelayVerificationLastUpdate) Then
                            If firstLoad > 1 Then
                                Dim header, body, link As String
                                'Dim strDate As String = dtNG.Rows(i)("Date")
                                'header = "ALERT Notification"
                                body = " there is new Delay Verification " ': " & DelayVerificationRowsCount
                                ' : " & NGInputRowsCount & " " & vbCrLf &
                                ' : " & DelayInputRowsCount & " " & vbCrLf &
                                link = pLink '+ dtNG.Rows(i)("Link")
                                'ShowNotification(header, body, "NG", link)

                                NotifyIcon1.ShowBalloonTip(500, "There are new notifications:", body, ToolTipIcon.Info)
                                'AddHandler NotifyIcon1.Click, AddressOf Me.NotifyShowing_Click
                                AddHandler NotifyIcon1.MouseDown, AddressOf Me.NotifyShowing_Click
                                AddHandler NotifyIcon1.BalloonTipClicked, AddressOf Me.NotifyIcon1_Click

                            End If
                            'If NGInputRowsCount > 0 Then
                            '    NGInputLastUpdate = currentNGLastUpdate 'dtNG.Rows(dtNG.Rows.Count - 1)("LastUpdate")
                            'End If
                            'If DelayInputRowsCount > 0 Then
                            '    DelayInputLastUpdate = currentDelayInput 'dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("LastUpdate")
                            'End If
                            'If DelayVerificationRowsCount > 0 Then
                            '    DelayVerificationLastUpdate = currentDelayVerification 'dtDelayVerification.Rows(dtDelayVerification.Rows.Count - 1)("LastUpdate")
                            'End If
                            NGInputLastUpdate = currentNGLastUpdate
                            DelayInputLastUpdate = currentDelayInput
                            DelayVerificationLastUpdate = currentDelayVerification
                            firstLoad = 2
                        End If
                    Catch ex As Exception
                        'If NGInputRowsCount > 0 Then
                        '    NGInputLastUpdate = currentNGLastUpdate 'dtNG.Rows(dtNG.Rows.Count - 1)("LastUpdate")
                        'End If
                        'If DelayInputRowsCount > 0 Then
                        '    DelayInputLastUpdate = currentDelayInput 'dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("LastUpdate")
                        'End If
                        'If DelayVerificationRowsCount > 0 Then
                        '    DelayVerificationLastUpdate = currentDelayVerification 'dtDelayVerification.Rows(dtDelayVerification.Rows.Count - 1)("LastUpdate")
                        'End If
                        NGInputLastUpdate = currentNGLastUpdate
                        DelayInputLastUpdate = currentDelayInput
                        DelayVerificationLastUpdate = currentDelayVerification
                    End Try

                    'If dtNG.Rows.Count > 0 Then
                    '    NGInputLastUpdate = dtNG.Rows(dtNG.Rows.Count - 1)("LastUpdate")
                    'Else
                    '    NGInputLastUpdate = 0
                    'End If

                    'If dtDelayInput.Rows.Count > 0 Then
                    '    DelayInputLastUpdate = dtDelayInput.Rows(dtDelayInput.Rows.Count - 1)("LastUpdate")
                    'Else
                    '    DelayInputLastUpdate = 0
                    'End If

                    'If dtDelayVerification.Rows.Count > 0 Then
                    '    DelayVerificationLastUpdate = dtDelayVerification.Rows(dtDelayVerification.Rows.Count - 1)("LastUpdate")
                    'Else
                    '    DelayVerificationLastUpdate = 0
                    'End If
                End If

            Catch ex As Exception

            End Try

            ProcessRunning = False
        End If
    End Sub
#End Region

End Class
