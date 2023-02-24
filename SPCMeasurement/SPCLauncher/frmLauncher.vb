Public Class frmLauncher
    Dim strCommand As String = Command()
    Dim strConfig As String = ""
    Dim SourcePath As String = ""
    Dim DestPath As String = ""

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblErr.Text = ""
        lblCommand.Text = strCommand
        Dim IniPath As String = Application.StartupPath & "\app.ini"
        strConfig = My.Computer.FileSystem.ReadAllText(IniPath)
        Dim strArray() As String = strConfig.Split(vbCrLf)
        If strArray.Count > 0 Then
            SourcePath = strArray(0).Trim
        End If
        If strArray.Count > 1 Then
            DestPath = strArray(1).Trim
        End If
    End Sub

    Private Sub frmLauncher_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim s As String = "SPC,admin,F001,PG003,ASS01,P002,015,TPMSBR011,IC021,SH001,1,24-Jan-2023"
        Try
            FileSystem.FileCopy(SourcePath, DestPath)
            Process.Start(DestPath, strCommand)
            Me.Close()
            End
        Catch ex As Exception
            If ex.Message.ToString.ToLower.Contains("the process cannot access the file") Then
                Dim processList() As Process
                processList = Process.GetProcessesByName("SPCMeasurement")
                For Each proc As Process In processList
                    proc.Kill()
                Next
                Try
                    Process.Start(DestPath, strCommand)
                    Me.Close()
                    End
                Catch ex2 As Exception
                    If ex2.Message.ToString.ToLower.Contains("the process cannot access the file") Then
                        lblErr.Text = "The application is still running." & vbCrLf &
                            "Please close " & DestPath
                    Else
                        lblErr.Text = ex2.Message
                    End If
                End Try
            Else
                lblErr.Text = ex.Message
            End If
        End Try
    End Sub

    Private Sub lblCommand_Click(sender As Object, e As EventArgs) Handles lblCommand.Click

    End Sub

    Private Sub lblErr_Click(sender As Object, e As EventArgs) Handles lblErr.Click

    End Sub
End Class
