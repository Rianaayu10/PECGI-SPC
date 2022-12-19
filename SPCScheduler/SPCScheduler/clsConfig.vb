Imports System.Data.SqlClient
Imports System.Xml

Public Class clsConfig
    Private builder As SqlConnectionStringBuilder
    Public m_Server As String
    Public m_Database As String
    Public m_User As String
    Public m_Password As String
    Public m_CommandTimeout As Integer
    Public m_DatabaseTimeout As Integer
    Public m_WinMode As String
    Public m_PathBOMToSAP As String
    Public m_PathBOMFromSAP As String
    Public m_PathNIKFromSAP As String
    Public m_ConnectionString As String
    Public m_SLtoEZRFolder As String
    Public m_SLtoEZRBackUpFolder As String
    Public m_EZRtoSLFolder As String
    Public m_EZRStockTransferFolder As String
    Public m_EZRtoSLBackUpFolder As String
    Public m_IntervalValue As Integer
    Private ls_path As String
    Dim NewEnryption As New clsDESEncryption("TOS")

    Public Function AddSlash(ByVal Path As String) As String
        Dim Result As String = Path
        If Path.EndsWith("\") = False Then
            Result = Result + "\"
        End If
        Return Result
    End Function

    Public Sub New(Optional ByVal pConfigFile As String = "config.xml")
        ls_path = AddSlash(My.Application.Info.DirectoryPath) & pConfigFile

        If Not My.Computer.FileSystem.FileExists(ls_path) Then
            Throw New Exception("Config file is not found")
        End If

        'Check XML file is empty or not
        If Trim(IO.File.ReadAllText(ls_path).Length) = 0 Then Exit Sub

        'Load XML file
        Dim document = XDocument.Load(ls_path)


        Dim APPDB = document.Descendants("EZDB").FirstOrDefault()
        If Not IsNothing(APPDB) Then
            If Not IsNothing(APPDB.Element("ServerName")) Then m_Server = NewEnryption.DecryptData(APPDB.Element("ServerName").Value)
            If Not IsNothing(APPDB.Element("Database")) Then m_Database = NewEnryption.DecryptData(APPDB.Element("Database").Value)
            If Not IsNothing(APPDB.Element("UserID")) Then m_User = NewEnryption.DecryptData(APPDB.Element("UserID").Value)
            If Not IsNothing(APPDB.Element("Password")) Then m_Password = NewEnryption.DecryptData(APPDB.Element("Password").Value)
            If Not IsNothing(APPDB.Element("WinMode")) Then m_WinMode = NewEnryption.DecryptData(APPDB.Element("WinMode").Value)
            If Not IsNothing(APPDB.Element("CommandTimeOut")) Then m_CommandTimeout = IIf(IsNumeric(NewEnryption.DecryptData(APPDB.Element("CommandTimeOut").Value)) = True, NewEnryption.DecryptData(APPDB.Element("CommandTimeOut").Value), 0)
            If Not IsNothing(APPDB.Element("DatabaseTimeOut")) Then m_DatabaseTimeout = IIf(IsNumeric(NewEnryption.DecryptData(APPDB.Element("DatabaseTimeOut").Value)) = True, NewEnryption.DecryptData(APPDB.Element("DatabaseTimeOut").Value), 0)

            If Not IsNothing(APPDB.Element("PathBOMToSAP")) Then m_PathBOMToSAP = NewEnryption.DecryptData(APPDB.Element("PathBOMToSAP").Value)
            If Not IsNothing(APPDB.Element("PathBOMFromSAP")) Then m_PathBOMFromSAP = NewEnryption.DecryptData(APPDB.Element("PathBOMFromSAP").Value)
            If Not IsNothing(APPDB.Element("PathNIKFromSAP")) Then m_PathNIKFromSAP = NewEnryption.DecryptData(APPDB.Element("PathNIKFromSAP").Value)


            Dim SchedulerDB = document.Descendants("Scheduler").FirstOrDefault()
            If Not IsNothing(SchedulerDB) Then
                If Not IsNothing(SchedulerDB.Element("SLtoEZRFolder")) Then m_SLtoEZRFolder = NewEnryption.DecryptData(SchedulerDB.Element("SLtoEZRFolder").Value)
                If Not IsNothing(SchedulerDB.Element("SLtoEZRBackUpFolder")) Then m_SLtoEZRBackUpFolder = NewEnryption.DecryptData(SchedulerDB.Element("SLtoEZRBackUpFolder").Value)
                If Not IsNothing(SchedulerDB.Element("EZRtoSLFolder")) Then m_EZRtoSLFolder = NewEnryption.DecryptData(SchedulerDB.Element("EZRtoSLFolder").Value)
                If Not IsNothing(SchedulerDB.Element("EZRStockTransferFolder")) Then m_EZRStockTransferFolder = NewEnryption.DecryptData(SchedulerDB.Element("EZRStockTransferFolder").Value)
                If Not IsNothing(SchedulerDB.Element("EZRtoSLBackUpFolder")) Then m_EZRtoSLBackUpFolder = NewEnryption.DecryptData(SchedulerDB.Element("EZRtoSLBackUpFolder").Value)

                If Not IsNothing(SchedulerDB.Element("Interval")) Then m_IntervalValue = NewEnryption.DecryptData(SchedulerDB.Element("Interval").Value)
            End If

            If m_Server = "" Or m_Database = "" Then
                Throw New Exception("Application setting is empty")
            End If

            builder = New SqlConnectionStringBuilder
            builder.DataSource = m_Server
            builder.InitialCatalog = m_Database
            builder.UserID = m_User
            builder.Password = m_Password
            builder.IntegratedSecurity = m_WinMode = ""

            m_ConnectionString = builder.ConnectionString
        End If

    End Sub
End Class
