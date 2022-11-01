Imports System.Data.SqlClient

Public Class clsUserSetupDB
    Public Shared Function GetUserF1(UserID As String) As clsUserSetup
        Try
            Using cn As New SqlConnection(Sconn.F1Connection)
                Dim sql As String
                Dim clsDESEncryption As New clsDESEncryption("TOS")
                sql = " SELECT * FROM dbo.spc_UserSetup where UserID = @UserID AND AppID='SPC'" & vbCrLf
                Dim cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Dim Users As New List(Of clsUserSetup)
                If dt.Rows.Count > 0 Then
                    Dim i As Integer = 0
                    Dim User As New clsUserSetup With {
                            .UserID = Trim(dt.Rows(i)("UserID")),
                            .Password = clsDESEncryption.DecryptData(dt.Rows(i)("Password")),
                            .LockStatus = Val(dt.Rows(i)("LockStatus") & "")
                        }
                    Return User
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function GetUserF2(UserID As String) As clsUserSetup
        Try
            Using cn As New SqlConnection(Sconn.F2Connection)
                Dim sql As String
                Dim clsDESEncryption As New clsDESEncryption("TOS")
                sql = " SELECT * FROM dbo.spc_UserSetup where UserID = @UserID AND AppID='SPC'" & vbCrLf
                Dim cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Dim Users As New List(Of clsUserSetup)
                If dt.Rows.Count > 0 Then
                    Dim i As Integer = 0
                    Dim User As New clsUserSetup With {
                            .UserID = Trim(dt.Rows(i)("UserID")),
                            .Password = clsDESEncryption.DecryptData(dt.Rows(i)("Password")),
                            .LockStatus = Val(dt.Rows(i)("LockStatus") & "")
                        }
                    Return User
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function GetUserF3(UserID As String) As clsUserSetup
        Try
            Using cn As New SqlConnection(Sconn.F3Connection)
                Dim sql As String
                Dim clsDESEncryption As New clsDESEncryption("TOS")
                sql = " SELECT * FROM dbo.spc_UserSetup where UserID = @UserID AND AppID='SPC'" & vbCrLf
                Dim cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Dim Users As New List(Of clsUserSetup)
                If dt.Rows.Count > 0 Then
                    Dim i As Integer = 0
                    Dim User As New clsUserSetup With {
                           .UserID = Trim(dt.Rows(i)("UserID")),
                           .Password = clsDESEncryption.DecryptData(dt.Rows(i)("Password")),
                           .LockStatus = Val(dt.Rows(i)("LockStatus") & "")
                        }
                    Return User
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function GetUserF4(UserID As String) As clsUserSetup
        Try
            Using cn As New SqlConnection(Sconn.F4Connection)
                Dim sql As String
                Dim clsDESEncryption As New clsDESEncryption("TOS")
                sql = " SELECT * FROM dbo.spc_UserSetup where UserID = @UserID AND AppID='SPC'" & vbCrLf
                Dim cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Dim Users As New List(Of clsUserSetup)
                If dt.Rows.Count > 0 Then
                    Dim i As Integer = 0
                    Dim User As New clsUserSetup With {
                           .UserID = Trim(dt.Rows(i)("UserID")),
                           .Password = clsDESEncryption.DecryptData(dt.Rows(i)("Password")),
                           .LockStatus = Val(dt.Rows(i)("LockStatus") & "")
                        }
                    Return User
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
