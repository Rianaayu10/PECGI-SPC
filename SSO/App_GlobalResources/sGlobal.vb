Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Math
Imports System.Text

Public Module sGlobal
#Region "DECLARATION"
    Public menuName As String = ""
    Public indexMenu As Integer
    Public idMenu As String = ""

    Public Enum MsgTypeEnum
        Info = 0 ' biru
        Success = 1 'hijau
        Warning = 2 ' kuning
        ErrorMsg = 3 ' merah
    End Enum

#End Region

#Region "LOGIN"
    'Public Sub spScr_UserLogin(ByVal _pShortLess As pShortless)
    '    Try
    '        Using _sConn As New SqlConnection(Sconn.Stringkoneksi)
    '            Dim param As SqlCommand = New SqlCommand("spSc_Usersetup", _sConn)

    '            With param
    '                .CommandTimeout = 0
    '                .CommandType = CommandType.StoredProcedure
    '                .Parameters.AddWithValue("@User_ID_Login", _pShortLess.pDesc1)
    '                .Parameters.AddWithValue("@Password", "")
    '            End With

    '            _sConn.Open()
    '            param.ExecuteNonQuery()
    '            _sConn.Close()
    '        End Using
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Public Function spScr_UserValidation(ByVal pDesc1 As String, _
    '                                            ByVal pDesc2 As String) As pShortless
    '    Dim _pShortLess As pShortless = Nothing

    '    Try
    '        Using _sConn As New SqlConnection(Sconn.Stringkoneksi)
    '            Dim param As SqlCommand = New SqlCommand("spScr_UserValidation", _sConn)
    '            With param
    '                .CommandTimeout = 0
    '                .CommandType = CommandType.StoredProcedure
    '                .Parameters.AddWithValue("@UserID", pDesc1)
    '                .Parameters.AddWithValue("@Password", pDesc2)
    '            End With
    '            _sConn.Open()
    '            Using myReader As SqlDataReader = param.ExecuteReader(CommandBehavior.CloseConnection)
    '                If myReader.Read() Then
    '                    _pShortLess = New pShortless(myReader.GetValue(myReader.GetOrdinal("UserID")))
    '                    If myReader.IsDBNull(myReader.GetOrdinal("Name")) = False Then _
    '                            _pShortLess.pDesc2 = myReader.GetValue(myReader.GetOrdinal("Name"))
    '                    If myReader.IsDBNull(myReader.GetOrdinal("StatusAdminCls")) = False Then _
    '                           _pShortLess.pDesc3 = myReader.GetValue(myReader.GetOrdinal("StatusAdminCls"))
    '                End If
    '                myReader.Close()
    '            End Using
    '            _sConn.Close()
    '        End Using
    '    Catch ex As Exception
    '        'MsgBox(Err.Description)
    '    End Try
    '    Return _pShortLess
    'End Function

    Public Function Auth_UserInsert(ByVal pUserID As String, ByVal pMenuID As String) As Boolean
        Dim retVal As Boolean = False

        Using sqlConn As New SqlConnection(Sconn.Stringkoneksi)
            sqlConn.Open()

            Dim ls_SQL As String = "SELECT AllowInsert FROM dbo.spc_UserPrivilege WHERE AppID = 'P01' AND UserID = '" & Trim(pUserID) & "' AND MenuID = '" & Trim(pMenuID) & "'"
            Dim sqlCmd As New SqlCommand(ls_SQL, sqlConn)
            Dim sqlRdr As SqlDataReader = sqlCmd.ExecuteReader()

            If sqlRdr.Read() Then
                If sqlRdr("AllowInsert") = "1" Then
                    retVal = True
                ElseIf sqlRdr("AllowInsert") = "0" Then
                    retVal = False
                End If
            Else
                retVal = False
            End If

            sqlConn.Close()
        End Using

        Return retVal
    End Function

    Public Function Auth_UserUpdate(ByVal pUserID As String, ByVal pMenuID As String) As Boolean
        Dim retVal As Boolean = False
        Using sqlConn As New SqlConnection(Sconn.Stringkoneksi)
            sqlConn.Open()

            If pMenuID = "Z020" Then
                Dim q As String = "select AdminStatus from dbo.spc_UserSetup A" & vbCrLf &
                              "Inner Join dbo.spc_UserPrivilege B ON A.AppID = B.AppID AND A.UserID = B.UserID" & vbCrLf &
                              "where A.UserID = '" & pUserID & "' and A.AdminStatus = 1"
                Dim cmd As New SqlCommand(q, sqlConn)
                Dim rd As SqlDataReader = cmd.ExecuteReader()
                If rd.Read() Then
                    retVal = True
                    rd.Close()
                    Return retVal
                End If
                rd.Close()
            End If


            Dim ls_SQL As String = "SELECT AllowUpdate FROM dbo.spc_UserPrivilege WHERE AppID = 'SPC' AND UserID = '" & Trim(pUserID) & "' AND MenuID = '" & Trim(pMenuID) & "'"
            Dim sqlCmd As New SqlCommand(ls_SQL, sqlConn)
            Dim sqlRdr As SqlDataReader = sqlCmd.ExecuteReader()

            If sqlRdr.Read() Then
                If sqlRdr("AllowUpdate") = "1" Then
                    retVal = True
                ElseIf sqlRdr("AllowUpdate") = "0" Then
                    retVal = False
                End If
            Else
                retVal = False
            End If
            sqlConn.Close()
        End Using
        Return retVal
    End Function

    Public Function Auth_UserAccess(ByVal pUserID As String, ByVal pMenuID As String) As Boolean
        Dim retVal As Boolean = False
        Using sqlConn As New SqlConnection(Sconn.Stringkoneksi)
            sqlConn.Open()

            If pMenuID = "Z020" Then
                Dim q As String = "select AdminStatus from dbo.spc_UserSetup A" & vbCrLf &
                              "Inner Join dbo.spc_UserPrivilege B ON A.AppID = B.AppID AND A.UserID = B.UserID" & vbCrLf &
                              "where A.UserID = '" & pUserID & "' and A.AdminStatus = 1"
                Dim cmd As New SqlCommand(q, sqlConn)
                Dim rd As SqlDataReader = cmd.ExecuteReader()
                If rd.Read() Then
                    retVal = True
                    rd.Close()
                    Return retVal
                End If
                rd.Close()
            End If


            Dim ls_SQL As String = "SELECT AllowAccess FROM dbo.spc_UserPrivilege WHERE AppID = 'SPC' AND UserID = '" & Trim(pUserID) & "' AND MenuID = '" & Trim(pMenuID) & "'"
            Dim sqlCmd As New SqlCommand(ls_SQL, sqlConn)
            Dim sqlRdr As SqlDataReader = sqlCmd.ExecuteReader()

            If sqlRdr.Read() Then
                If sqlRdr("AllowAccess") = "1" Then
                    retVal = True
                ElseIf sqlRdr("AllowAccess") = "0" Then
                    retVal = False
                End If
            Else
                retVal = False
            End If
            sqlConn.Close()
        End Using
        Return retVal
    End Function

    Public Function Auth_UserDelete(ByVal pUserID As String, ByVal pMenuID As String) As Boolean
        Dim retVal As Boolean = False

        Using sqlConn As New SqlConnection(Sconn.Stringkoneksi)
            sqlConn.Open()

            Dim ls_SQL As String = "SELECT AllowDelete FROM dbo.spc_UserPrivilege WHERE AppID = 'SPC' AND UserID = '" & Trim(pUserID) & "' AND MenuID = '" & Trim(pMenuID) & "'"
            Dim sqlCmd As New SqlCommand(ls_SQL, sqlConn)
            Dim sqlRdr As SqlDataReader = sqlCmd.ExecuteReader()

            If sqlRdr.Read() Then
                If sqlRdr("AllowDelete") = "1" Then
                    retVal = True
                ElseIf sqlRdr("AllowDelete") = "0" Then
                    retVal = False
                End If
            Else
                retVal = False
            End If

            sqlConn.Close()
        End Using

        Return retVal
    End Function

#End Region

#Region "MENU"
    Public Function spScr_MenuUser(ByVal username As String) As DataSet
        Dim _DataSet As DataSet = New DataSet
        Dim myDataAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim sqlstring As String
        Dim sql As SqlCommand
        Try

            Using _sConn As New SqlConnection(Sconn.Stringkoneksi)

                sqlstring = " SELECT MenuIndex,GroupIndex,GroupID,MenuName,MenuDesc" & vbCrLf &
                            " FROM dbo.spc_UserMenu UM  " & vbCrLf &
                            " INNER JOIN dbo.spc_UserPrivilege UP ON UP.MenuID = UM.MenuID  " & vbCrLf &
                            " WHERE UP.AllowView = 1 and UserID ='" & username & "'" & vbCrLf &
                            " order by SeqNo "
                sql = New SqlCommand(sqlstring, _sConn)
                myDataAdapter.SelectCommand = sql
                myDataAdapter.Fill(_DataSet)
                Message = Err.Description
                _sConn.Close()
                Return _DataSet
            End Using
        Catch ex As Exception
            Throw

        End Try
    End Function

    Public Sub getMenu(ByVal MenuID As String)
        Dim constr As New SqlClient.SqlConnection(Sconn.Stringkoneksi)
        Dim ds As New Data.DataSet
        Dim sqlstring As String

        If MenuID = "00" Then
            menuName = ""
            indexMenu = 0
            idMenu = ""
        Else
            sqlstring = " SELECT * FROM dbo.spc_UserMenu WHERE MenuID ='" & MenuID & "' "
            ds = DataAccess.uf_GetDataSet(sqlstring, constr)

            With ds.Tables(0)
                If .Rows.Count > 0 Then
                    menuName = Trim(.Rows(0)("MenuDesc").ToString)
                    indexMenu = .Rows(0)("GroupIndex")
                    idMenu = .Rows(0)("MenuID")
                End If
            End With
        End If
        'End If
    End Sub

    ''' <summary>Check if the value is already used in another table</summary>
    ''' <param name="pTableList">Table name, can be more than one. Separate with comma</param>
    ''' <param name="pFieldList">Field name, can be more than one. Separate with comma</param>
    ''' <param name="pValueList">The number of values must match the number of fields</param>
    ''' <param name="pCriteria">Additional criteria</param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function uf_Used(ByVal pTableList As String, ByVal pFieldList As String, ByVal pValueList As String, Optional ByVal pCriteria As String = "") As String
        Dim q As String
        Dim pTableArray() As String = Split(pTableList, ",")
        Dim pTable As String
        Dim nTable As Integer
        Dim iTable As Integer
        Dim TableName As String = ""
        Dim pFieldArray() As String = Split(pFieldList, ",")
        Dim pField As String
        Dim nField As Integer
        Dim iField As Integer
        Dim pValueArray() As String = Split(pValueList, ",")
        Dim pValue As String
        Dim nValue As Integer

        nTable = UBound(pTableArray)
        nField = UBound(pFieldArray)
        nValue = UBound(pValueArray)
        For iTable = 0 To nTable
            pTable = pTableArray(iTable)
            q = "Select * from " & pTable & " where "
            For iField = 0 To nField
                pField = pFieldArray(iField)
                If iField <= nValue Then
                    pValue = pValueArray(iField)
                Else
                    pValue = "Null"
                End If
                q = q & pField & " = '" & Replace(pValue, "'", "''") & "' "
                If iField < nField Then
                    q = q & "and "
                End If
            Next
            If pCriteria <> "" Then
                q = q & pCriteria
            End If

            Using con As New SqlConnection(Sconn.Stringkoneksi)
                con.Open()
                Dim da As New SqlDataAdapter(q, con)
                Dim dt As New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    TableName = TableName + pTable + ", "
                End If
            End Using
        Next
        If TableName.Length > 2 Then
            TableName = TableName.Substring(0, Len(TableName) - 2)
        End If
        Return TableName
    End Function

    Public Function GetData(ByVal query As String) As DataTable
        Dim dt As New DataTable()
        Using constr As New SqlConnection(Sconn.Stringkoneksi)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = constr
                    sda.SelectCommand = cmd
                    sda.Fill(dt)
                End Using
            End Using
            Return dt
        End Using
    End Function

#End Region

#Region "MESSAGE"
    Public Sub lMessage(ByVal label As Label, ByVal Message As String, Optional ByVal type As String = "Info")
        label.Text = Message
        Select Case type
            Case "Info"
                label.ForeColor = Drawing.Color.White
                label.BackColor = Drawing.Color.Blue
            Case "Warning"
                label.ForeColor = Drawing.Color.White
                label.BackColor = Drawing.Color.Red
            Case "Error"
                label.ForeColor = Drawing.Color.White
                label.BackColor = Drawing.Color.Red
            Case "N\A"
                label.ForeColor = Drawing.Color.White
                label.BackColor = Drawing.Color.White
        End Select
        label.Visible = True
    End Sub
#End Region

End Module
