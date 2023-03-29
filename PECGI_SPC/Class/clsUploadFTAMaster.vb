Imports System.Data.SqlClient
Public Class ClsUploadFTAMaster
    Public Property FactoryCode As String
    Public Property TypeHeader As String
    Public Property TypeDetail As String
    Public Property ItemCheckDetail As String
    Public Property FTAIDDetail As String
    Public Property Factor1Detail As String
    Public Property Factor2Detail As String
    Public Property Factor3Detail As String
    Public Property Factor4Detail As String
    Public Property CounterMeasureDetail As String
    Public Property CheckItemDetail As String
    Public Property CheckOrderDetail As String
    Public Property RemarkDetail As String
    Public Property ActiveStatusDetail As String
    Public Property ActionIDDetail As String
    Public Property ActionNameDetail As String
End Class
Public Class ClsUploadFTAMasterDB
    Public Shared Function InsertData(dtbFileUpload As DataTable, pType As String, pUserUpdate As String) As Boolean
        Try
            Using con As New SqlConnection(Sconn.Stringkoneksi)
                con.Open()
                Dim cmd As SqlCommand = con.CreateCommand
                Dim trans As SqlTransaction = con.BeginTransaction
                Dim strCmd As String = Nothing
                cmd.Transaction = trans
                Try
                    For Each Rows As DataRow In dtbFileUpload.Rows

                        If pType = 1 Then

                            strCmd = "INSERT INTO [dbo].[spc_MS_FTA] VALUES " & vbCrLf &
                                     "('" & Rows("FactoryCode") & "', '" & Rows("ItemTypeCode") & "', '" & Rows("ItemCheckCode") & "', " & vbCrLf &
                                     "'" & Rows("FTAID") & "', '" & Rows("Factor1") & "', '" & Rows("Factor2") & "', '" & Rows("Factor3") & "' ," & vbCrLf &
                                     "'" & Rows("Factor4") & "', '" & Rows("CounterMeasure") & "', '" & Rows("CheckItem") & "', '" & Rows("CheckOrder") & "', '' , '" & Rows("Remark") & "'," & vbCrLf &
                                     "'" & Rows("ActiveStatus") & "', '" & pUserUpdate & "', GETDATE(), '', '' )" & vbCrLf

                        ElseIf pType = 2 Then

                            strCmd = "INSERT INTO [dbo].[spc_MS_FTAAction] VALUES " & vbCrLf &
                                     "('" & Rows("FTAID") & "', '" & Rows("ActionID") & "', '" & Rows("ActionName") & "', '" & Rows("Remark") & "' )" & vbCrLf
                        End If

                        cmd.CommandText = strCmd
                        cmd.ExecuteNonQuery()
                    Next
                    trans.Commit()
                    Return True
                Catch ex As Exception
                    trans.Rollback()
                    Return False
                End Try
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function GetItemType(ItemTypeCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_UploadFTAMaster"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("ItemTypeCode", ItemTypeCode)
                    .AddWithValue("Type", 1)
                End With
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Return dt
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function
    Public Shared Function GetItemCheck(ItemCheckCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_UploadFTAMaster"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("ItemCheckCode", ItemCheckCode)
                    .AddWithValue("Type", 2)
                End With
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Return dt
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function
    Public Shared Function GetFTAID(FTAID As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_UploadFTAMaster"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("FTAID", FTAID)
                    .AddWithValue("Type", 3)
                End With
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Return dt
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function
    Public Shared Function GetActionID(FTAID As String, ActionID As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_UploadFTAMaster"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("FTAID", FTAID)
                    .AddWithValue("ActionID", ActionID)
                    .AddWithValue("Type", 4)
                End With
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Return dt
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function
End Class
