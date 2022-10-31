Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Math
Imports System.Text


Public Module DataAccess
    Public delimiter As String = "|"
    'default connection:
    'Public ConnectionString As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConString")

    Public Key As String = "PGMEATS"
    Private ConString As String = System.Configuration.ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
    'Public ConString As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
    Dim countzero As Integer
    Public Message As String

#Region "Encrypt"

    Private Function Encrypt(ByRef Msg As String, ByRef Key As String) As String
        Dim ResultSB As New System.Text.StringBuilder
        Dim Counter As Integer
        Dim MSGChar As Char
        Dim MsgInt As Integer
        Dim KeyChar As Char
        Dim KeyInt As Integer
        Dim RsltInt As Integer

        Try

            For Counter = 0 To Msg.Length - 1
                MSGChar = Msg.Substring(Counter, 1)
                MsgInt = Asc(MSGChar)
                KeyChar = GetKey(Key, Counter)
                KeyInt = Asc(KeyChar)
                RsltInt = MsgInt + KeyInt
                ResultSB.Append(Chr(RsltInt))
            Next

            Return ResultSB.ToString

        Catch ex As Exception
            Throw New Exception("Encrypt Exception: " & vbCrLf & ex.ToString & vbCrLf)
        Finally
            ResultSB.Length = 0
        End Try
    End Function

    Public Function Decrypt(ByRef Msg As String, ByRef Key As String) As String
        Dim ResultSB As New System.Text.StringBuilder
        Dim Counter As Integer
        Dim MSGChar As Char
        Dim MsgInt As Integer
        Dim KeyChar As Char
        Dim KeyInt As Integer
        Dim RsltInt As Integer

        Try

            For Counter = 0 To Msg.Length - 1
                MSGChar = Msg.Substring(Counter, 1)
                MsgInt = Asc(MSGChar)
                KeyChar = GetKey(Key, Counter)
                KeyInt = Asc(KeyChar)
                RsltInt = MsgInt - KeyInt
                ResultSB.Append(Chr(RsltInt))
            Next

            Return ResultSB.ToString

        Catch ex As Exception
            Throw New Exception("Decrypt Exception: " & vbCrLf & ex.ToString & vbCrLf)
        Finally
            ResultSB.Length = 0
        End Try
    End Function

    Private Function GetKey(ByRef Key As String, ByRef Index As Integer) As Char
        Dim ModIndex As Integer
        Try
            ModIndex = Index Mod Key.Length
            Return Key.Substring(ModIndex, 1)
        Catch ex As Exception
            Throw New Exception("GetKey Exception: " & vbCrLf & ex.ToString & vbCrLf)
        End Try
    End Function

#End Region

#Region "SQL"

    Public Function SQLExecuteNonQuery(ByRef SQLString As String, Optional ByVal ConnString As String = "", Optional ByVal DBMaxTry As Integer = 5, Optional ByVal SleepInterval As Integer = 1000) As Long
        Dim cn As New SqlConnection
        Dim cmd As New SqlCommand
        Dim recordsAffected As Long = 0
        Dim TryCount As Integer
        Dim Success As Boolean
        Dim CummulatedMessage As String = ""

        Try
            If ConnString = "" Then
                ConnString = ConString
            End If

            '---- Looping untill success or maxtry reached ------
            TryCount = 0
            Success = False
            While Not Success
                Try
                    cn = New SqlConnection(ConnString)
                    cmd = New SqlCommand(SQLString, cn)
                    cn.Open()
                    recordsAffected = cmd.ExecuteNonQuery()
                    Success = True

                    Return recordsAffected

                Catch ex As Threading.ThreadAbortException
                Catch ex As Exception
                    If ex.Message.ToLower.IndexOf("network") <> -1 _
                        Or ex.Message.ToLower.IndexOf("timeout") <> -1 _
                        Or ex.Message.ToLower.IndexOf("access") <> -1 _
                        Or ex.Message.ToLower.IndexOf("deadlocked") <> -1 Then
                        If TryCount >= DBMaxTry Then
                            Throw New Exception("SQLExecuteNonQuery - MaxTry Exceeded." & vbCrLf & "CummulatedMessage = " & CummulatedMessage & vbCrLf & "Trycount = " & TryCount.ToString & _
                                      "SQL: " & SQLString & vbCrLf)
                        End If
                        CummulatedMessage += "Try: " & TryCount.ToString & " - " & ex.Message & vbCrLf
                        TryCount += 1
                        Threading.Thread.Sleep(SleepInterval)
                    Else
                        Throw New Exception("SQLExecuteNonQuery Loop Exception: " & vbCrLf & ex.ToString)
                    End If
                End Try
            End While
            '---- Looping untill success or maxtry reached END------

        Catch ex As Threading.ThreadAbortException
        Catch e As Exception
            Throw New Exception("SQLExecuteNonQuery  Exception: " & vbCrLf & e.ToString & vbCrLf & "SQLString = " & SQLString & vbCrLf)
        Finally
            If Not cn Is Nothing Then cn.Dispose()
            If Not cmd Is Nothing Then cmd.Dispose()
        End Try
        Return recordsAffected
    End Function

    Public Function SQLExecuteQuery(ByRef SQLString As String, Optional ByVal ConnString As String = "", Optional ByVal DBMaxTry As Integer = 5, Optional ByVal SleepInterval As Integer = 1000) As DataSet
        Dim con As New SqlConnection
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim TryCount As Integer
        Dim Success As Boolean
        Dim CummulatedMessage As String = ""

        Try
            If ConnString = "" Then
                ConnString = ConString
            End If

            '---- Looping untill success or maxtry reached ------
            TryCount = 0
            Success = False
            While Not Success
                Try
                    con = New SqlConnection(ConnString)
                    da = New SqlDataAdapter(SQLString, con)
                    con.Open()
                    da.Fill(ds)

                    Success = True
                    Return ds
                Catch ex As Threading.ThreadAbortException
                Catch ex As Exception
                    If ex.Message.ToLower.IndexOf("network") <> -1 _
                        Or ex.Message.ToLower.IndexOf("timeout") <> -1 _
                        Or ex.Message.ToLower.IndexOf("access") <> -1 _
                        Or ex.Message.ToLower.IndexOf("deadlocked") <> -1 Then
                        If TryCount >= DBMaxTry Then
                            Throw New Exception("SQLExecuteQuery - MaxTry Exceeded." & vbCrLf & "CummulatedMessage = " & CummulatedMessage & vbCrLf & "Trycount = " & TryCount.ToString & vbCrLf & _
                                              "SQLString: " & SQLString & vbCrLf & "ConnString: " & vbCrLf & ConnString & vbCrLf)
                        End If
                        CummulatedMessage += "Try: " & TryCount.ToString & " - " & ex.Message & vbCrLf
                        TryCount += 1
                        Threading.Thread.Sleep(SleepInterval)
                    Else
                        Throw New Exception("SQLExecuteQuery Loop Exception: " & vbCrLf & ex.ToString)
                    End If
                End Try
            End While
            '---- Looping untill success or maxtry reached END------
        Catch ex As Threading.ThreadAbortException
        Catch e As Exception
            Throw New Exception("SQLExecuteQuery Exception: " & vbCrLf & e.ToString & vbCrLf & "SQLString = " & SQLString)
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
        Return Nothing
    End Function

    Public Function SQLExecuteScalar(ByRef SQLString As String, Optional ByVal ConnString As String = "", Optional ByVal DBMaxTry As Integer = 5, Optional ByVal SleepInterval As Integer = 1000) As String
        Dim cn As New SqlConnection
        Dim cmd As New SqlCommand
        Dim TryCount As Integer
        Dim Success As Boolean
        Dim CummulatedMessage As String = ""
        Dim Obj As Object
        Dim Result As String

        Try
            If ConnString = "" Then
                ConnString = ConString
            End If

            '---- Looping untill success or maxtry reached ------
            TryCount = 0
            Success = False
            While Not Success
                Try
                    cn = New SqlConnection(ConnString)
                    cmd = New SqlCommand(SQLString, cn)
                    cn.Open()
                    Obj = cmd.ExecuteScalar
                    If IsDBNull(Obj) Then
                        Result = "0"
                    Else
                        If IsNothing(Obj) Then
                            Result = ""
                        Else
                            Result = Obj.ToString
                        End If
                    End If
                    Success = True

                    Return Result
                Catch ex As Threading.ThreadAbortException
                Catch ex As Exception
                    If ex.Message.ToLower.IndexOf("network") <> -1 _
                        Or ex.Message.ToLower.IndexOf("timeout") <> -1 _
                        Or ex.Message.ToLower.IndexOf("access") <> -1 _
                        Or ex.Message.ToLower.IndexOf("deadlocked") <> -1 Then
                        If TryCount >= DBMaxTry Then
                            Throw New Exception("SQLExecuteScalar - MaxTry Exceeded." & vbCrLf & "CummulatedMessage = " & CummulatedMessage & vbCrLf & "Trycount = " & TryCount.ToString & _
                                      "SQL: " & SQLString & vbCrLf)
                        End If
                        CummulatedMessage += "Try: " & TryCount.ToString & " - " & ex.Message & vbCrLf
                        TryCount += 1
                        Threading.Thread.Sleep(SleepInterval)
                    Else
                        Throw New Exception("SQLExecuteScalar Loop Exception: " & vbCrLf & ex.ToString)
                    End If
                End Try
            End While
            '---- Looping untill success or maxtry reached END------

        Catch ex As Threading.ThreadAbortException
        Catch e As Exception
            Throw New Exception("SQLExecuteScalar  Exception: " & vbCrLf & e.ToString & vbCrLf & "SQLString = " & SQLString & vbCrLf)
        Finally
            If Not cn Is Nothing Then cn.Dispose()
            If Not cmd Is Nothing Then cmd.Dispose()
        End Try
        Return Nothing
    End Function

    Public Function SQLExecuteReader(ByRef SQLString As String, ByRef con As SqlConnection, Optional ByVal ConnString As String = "", Optional ByVal DBMaxTry As Integer = 5, Optional ByVal SleepInterval As Integer = 1000) As SqlDataReader
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader
        Dim TryCount As Integer
        Dim Success As Boolean
        Dim CummulatedMessage As String = ""

        Try
            If ConnString = "" Then
                ConnString = ConString
            End If

            '---- Looping untill success or maxtry reached ------
            TryCount = 0
            Success = False
            While Not Success
                Try
                    con = New SqlConnection(ConnString)
                    cmd = New SqlCommand(SQLString, con)
                    con.Open()
                    dr = cmd.ExecuteReader

                    Success = True
                    Return dr
                Catch ex As Threading.ThreadAbortException
                Catch ex As Exception
                    If ex.Message.ToLower.IndexOf("network") <> -1 _
                        Or ex.Message.ToLower.IndexOf("timeout") <> -1 _
                        Or ex.Message.ToLower.IndexOf("access") <> -1 _
                        Or ex.Message.ToLower.IndexOf("deadlocked") <> -1 Then
                        If TryCount >= DBMaxTry Then
                            Throw New Exception("SQLExecuteQuery - MaxTry Exceeded." & vbCrLf & "CummulatedMessage = " & CummulatedMessage & vbCrLf & "Trycount = " & TryCount.ToString & vbCrLf & _
                                              "SQLString: " & SQLString & vbCrLf & "ConnString: " & vbCrLf & ConnString & vbCrLf)
                        End If
                        CummulatedMessage += "Try: " & TryCount.ToString & " - " & ex.Message & vbCrLf
                        TryCount += 1
                        Threading.Thread.Sleep(SleepInterval)
                    Else
                        Throw New Exception("SQLExecuteReader Loop Exception: " & vbCrLf & ex.ToString)
                    End If
                End Try
            End While
            '---- Looping untill success or maxtry reached END------
        Catch ex As Threading.ThreadAbortException
        Catch e As Exception
            Throw New Exception("SQLExecuteReader Exception: " & vbCrLf & e.ToString & vbCrLf & "SQLString = " & SQLString)
        End Try
        Return Nothing
    End Function

    Public Function uf_GetDataSet(ByVal Query As String, Optional ByVal pCon As SqlConnection = Nothing, Optional ByVal pTrans As SqlTransaction = Nothing) As DataSet
        Dim cmd As New SqlCommand(Query)
        If pTrans IsNot Nothing Then
            cmd.Transaction = pTrans
        End If
        If pCon IsNot Nothing Then
            cmd.Connection = pCon
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            da = Nothing
            Return ds
        Else
            Using Cn As New SqlConnection(ConString)
                Cn.Open()
                cmd.Connection = Cn
                Dim da As New SqlDataAdapter(cmd)
                Dim ds As New DataSet
                da.Fill(ds)
                da = Nothing
                Return ds
            End Using
        End If
    End Function

#End Region
    
End Module