Imports System.Windows.Forms
Imports System.Text.Encoding
Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.Win32

Public Class frmLoginSettings


#Region "Init & load"

    Dim NewEnryption As New clsDESEncryption("TOS")
    Dim lb_Load As Boolean = True
    Private Enum Messages
        Errors = 0
        Success = 1
    End Enum

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblMsg.Text = ""
        lblVersion.Text = "Version. " & Me.ProductVersion
        lblcopyright.Text = ChrW(169) & " 2022 - Developed by PT. TOS Information Systems Indonesia"

        Dim ls_path As String

        ls_path = My.Application.Info.DirectoryPath & "\config.xml"
        If My.Computer.FileSystem.FileExists(ls_path) = False Then
            Dim fs1 As FileStream = New FileStream(ls_path, FileMode.Create, FileAccess.Write)
            Dim s1 As StreamWriter = New StreamWriter(fs1)

            s1.Close()
            fs1.Close()
            MsgBox("Config file is not found!", MsgBoxStyle.Information, "Info")
            Exit Sub
        End If
        Call up_AppSettingLoad(ls_path)
    End Sub

#End Region
#Region "Event"
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If MsgBox("Are you sure want to save this setting?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Information, "SPC Settings Login") = MsgBoxResult.Yes Then
            Call up_ApplySetting()
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        If MsgBox("Are you sure want to close this application?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Information, "SPC Settings Login") = MsgBoxResult.Yes Then
            Close()
        End If
    End Sub

    Private Sub btnTestConnect_Click(sender As Object, e As EventArgs) Handles btnTestConnect.Click
        Call up_TestConnection()
    End Sub
#End Region

#Region "sub & Function"
    Private Sub Message(ByVal txt As String, ByVal Param As Integer)
        If Param = Messages.Errors Then
            lblMsg.Text = txt
            lblMsg.ForeColor = Color.Red
        ElseIf Param = Messages.Success Then
            lblMsg.Text = txt
            lblMsg.ForeColor = Color.Blue
        Else
            lblMsg.Text = ""
            lblMsg.ForeColor = Color.Black
        End If
    End Sub

    Private Sub up_ApplySetting()
        Dim docXML = New XDocument()

        Dim Settings = New XElement("Settings")

        ''01. Login Data
        Dim login = New XElement("Login")
        login.Add(New XElement("UserID", NewEnryption.EncryptData(txtUserID.Text)))
        login.Add(New XElement("Password", NewEnryption.EncryptData(txtPassword.Text)))
        login.Add(New XElement("LinkSPC", NewEnryption.EncryptData(txtLink.Text)))
        login.Add(New XElement("StartUp", NewEnryption.EncryptData(chkStartUp.Checked)))
        Settings.Add(login)

        ''02. Database
        Dim SPCDB = New XElement("SPCDB")
        SPCDB.Add(New XElement("ServerName", NewEnryption.EncryptData(txtDBServer.Text)))
        SPCDB.Add(New XElement("Database", NewEnryption.EncryptData(txtDBDatabase.Text)))
        SPCDB.Add(New XElement("UserID", NewEnryption.EncryptData(txtDBUserID.Text)))
        SPCDB.Add(New XElement("Password", NewEnryption.EncryptData(txtDBPassword.Text)))
        Settings.Add(SPCDB)

        docXML.Add(Settings)
        docXML.Save("config.xml")


        If chkStartUp.Checked = True Then
            Dim reg As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            reg.SetValue("SPC Alert Notification", Application.ExecutablePath.ToString())
        ElseIf chkStartUp.Checked = False Then
            Dim reg As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            If Not IsNothing(reg.GetValue("SPC Alert Notification")) Then
                reg.DeleteValue("SPC Alert Notification", False)
            End If
        End If

        MsgBox("Change Apply Setting Succeeded.", MsgBoxStyle.Information, "SPC Settings Login")
    End Sub

    Private Sub up_AppSettingLoad(ByVal ls_path As String)
        Try
            If (IO.File.Exists(ls_path)) Then
                If Trim(IO.File.ReadAllText(ls_path).Length) = 0 Then Exit Sub
                Dim Settings = XDocument.Load(ls_path)
                Dim login = Settings.Descendants("Login").FirstOrDefault()
                If Not IsNothing(login) Then
                    If Not IsNothing(login.Element("UserID")) Then txtUserID.Text = NewEnryption.DecryptData(login.Element("UserID").Value)
                    If Not IsNothing(login.Element("Password")) Then txtPassword.Text = NewEnryption.DecryptData(login.Element("Password").Value)
                    If Not IsNothing(login.Element("LinkSPC")) Then txtLink.Text = NewEnryption.DecryptData(login.Element("LinkSPC").Value)
                    If Not IsNothing(login.Element("StartUp")) Then chkStartUp.Checked = NewEnryption.DecryptData(login.Element("StartUp").Value)
                End If
                Dim SPCDB = Settings.Descendants("SPCDB").FirstOrDefault()
                If Not IsNothing(SPCDB) Then
                    If Not IsNothing(SPCDB.Element("ServerName")) Then txtDBServer.Text = NewEnryption.DecryptData(SPCDB.Element("ServerName").Value)
                    If Not IsNothing(SPCDB.Element("Database")) Then txtDBDatabase.Text = NewEnryption.DecryptData(SPCDB.Element("Database").Value)
                    If Not IsNothing(SPCDB.Element("UserID")) Then txtDBUserID.Text = NewEnryption.DecryptData(SPCDB.Element("UserID").Value)
                    If Not IsNothing(SPCDB.Element("Password")) Then txtDBPassword.Text = NewEnryption.DecryptData(SPCDB.Element("Password").Value)
                End If
            Else
                MessageBox.Show("Config File is not found!")
            End If
            lb_Load = False
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Exclamation, "Warning")
        End Try
    End Sub

    Private Sub up_TestConnection()
        Dim ls_con As String
        Dim con As New SqlConnection

        Try
            ls_con = "Data Source=" & txtDBServer.Text & ";Initial Catalog=" & txtDBDatabase.Text & ";User ID=" & txtDBUserID.Text & ";pwd=" & txtDBPassword.Text & ""
            con.ConnectionString = ls_con
            con.Open()

            If con.State = ConnectionState.Open Then
                MsgBox("Test Connection SPC Succeeded !", MsgBoxStyle.Information, "Info")
            End If

            con.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

#End Region

End Class
