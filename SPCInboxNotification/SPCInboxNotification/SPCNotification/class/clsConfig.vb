Imports System.Data.SqlClient
Imports System.Xml

'Edit by Pebri
Imports System.IO

Public Class clsConfig

#Region "Initial"
    Private builder As SqlConnectionStringBuilder

    'Used for Alert System Database
    Private ls_path As String
    'Edit by Pebri

    Private cDESEncryption As New clsDESEncryption("TOS")


    Private m_useridlogin As String
    Public Property UserIDLogin As String
        Get
            Return m_useridlogin
        End Get
        Set(ByVal value As String)
            m_useridlogin = value
        End Set
    End Property
    Private m_Passwordlogin As String
    Public Property PasswordLogin As String
        Get
            Return m_Passwordlogin
        End Get
        Set(ByVal value As String)
            m_Passwordlogin = value
        End Set
    End Property

    Private m_Server As String
    Public Property Server As String
        Get
            Return m_Server
        End Get
        Set(ByVal value As String)
            m_Server = value
        End Set
    End Property
    Private m_Database As String
    Public Property Database As String
        Get
            Return m_Database
        End Get
        Set(ByVal value As String)
            m_Database = value
        End Set
    End Property
    Private m_User As String
    Public Property User As String
        Get
            Return m_User
        End Get
        Set(ByVal value As String)
            m_User = value
        End Set
    End Property
    Private m_Password As String
    Public Property Password As String
        Get
            Return m_Password
        End Get
        Set(ByVal value As String)
            m_Password = value
        End Set
    End Property
    Private m_ConnectionString As String
    Public ReadOnly Property ConnectionString As String
        Get
            Return m_ConnectionString
        End Get
    End Property

#End Region

#Region "Method"
    Public Function AddSlash(ByVal Path As String) As String
        Dim Result As String = Path
        If Path.EndsWith("\") = False Then
            Result = Result + "\"
        End If
        Return Result
    End Function
#End Region

#Region "Controller"
    ''' <summary>
    ''' Open config file and store the value in local variables.
    ''' </summary>
    ''' <param name="pConfigFile"></param>
    ''' <remarks></remarks>
    Public Sub New(Optional ByVal pConfigFile As String = "config.xml")
        'Dim Ret As String = Space(1500)

        ls_path = AddSlash(My.Application.Info.DirectoryPath) & pConfigFile
        Dim m_WinMode = "mixed"

        'strConPath = AddSlash(My.Application.Info.DirectoryPath) & "MySql_ConnectionString.txt"
        'Using reader As StreamReader = New StreamReader(strConPath)
        '    ' Read one line from file
        '    m_MySqlConnection = reader.ReadToEnd
        'End Using

        If Not My.Computer.FileSystem.FileExists(ls_path) Then
            Throw New Exception("Config file is not found!")
        End If

        'Check XML file is empty or not
        If Trim(IO.File.ReadAllText(ls_path).Length) = 0 Then Exit Sub

        'Load XML file
        Dim document = XDocument.Load(ls_path)

        ''''''''''''''''''''''''''''''''''01. Load Setting SPCDB to Screen''''''''''''''''''''''''''''''''''
        Dim ASDB = document.Descendants("SPCDB").FirstOrDefault()
        If Not IsNothing(ASDB) Then
            If Not IsNothing(ASDB.Element("ServerName")) Then m_Server = cDESEncryption.DecryptData(ASDB.Element("ServerName").Value)
            If Not IsNothing(ASDB.Element("Database")) Then m_Database = cDESEncryption.DecryptData(ASDB.Element("Database").Value)
            If Not IsNothing(ASDB.Element("UserID")) Then m_User = cDESEncryption.DecryptData(ASDB.Element("UserID").Value)
            If Not IsNothing(ASDB.Element("Password")) Then m_Password = cDESEncryption.DecryptData(ASDB.Element("Password").Value)
            If m_Server = "" Or m_Database = "" Then
                Throw New Exception("Application setting is empty")
            End If

            builder = New SqlConnectionStringBuilder
            builder.DataSource = m_Server
            builder.InitialCatalog = m_Database
            builder.UserID = m_User
            builder.Password = m_Password
            builder.IntegratedSecurity = m_WinMode = "win"

            m_ConnectionString = builder.ConnectionString
        End If
        ''''''''''''''''''''''''''''''''''END''''''''''''''''''''''''''''''''''
    End Sub
#End Region

End Class
