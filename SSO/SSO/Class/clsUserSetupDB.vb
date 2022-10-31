Imports System.Data.SqlClient

Public Class clsUserSetupDB

    Public Shared Function UpdatePassword(pUserID As String, pNewPassword As String) As Integer
        Dim des As New clsDESEncryption("TOS")
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "Update dbo.spc_UserSetup set Password = @Password, UpdateDate = GetDate(), UpdateUser = @UserID " & vbCrLf &
                "where UserID = @UserID"
            Dim cmd As New SqlCommand(q, Cn)
            pNewPassword = des.EncryptData(pNewPassword)
            cmd.Parameters.AddWithValue("UserID", pUserID)
            cmd.Parameters.AddWithValue("Password", pNewPassword)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function AddFailedLogin(pUser As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Update dbo.spc_UserSetup set FailedLogin = isnull(FailedLogin, 0) + 1 where UserID = @UserID and isnull(AdminStatus, 0) = 0 "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", pUser)
            Dim i As Integer = cmd.ExecuteNonQuery
            q = "Update dbo.spc_UserSetup set LockStatus = 1 where UserID = @UserID and isnull(AdminStatus, 0) = 0 and isnull(FailedLogin, 0) >= 12 "
            cmd.CommandText = q
            cmd.ExecuteNonQuery()
            Return i
        End Using
    End Function

    Public Shared Function ResetFailedLogin(pUser As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Update dbo.spc_UserSetup set FailedLogin = 0 where UserID = @UserID "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", pUser)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function GetList() As List(Of clsUserSetup)
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " SELECT * FROM dbo.spc_UserSetup " & vbCrLf
            Dim cmd As New SqlCommand(sql, cn)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of clsUserSetup)
            For i = 0 To dt.Rows.Count - 1
                Dim User As New clsUserSetup With {
                    .AppID = dt.Rows(i)("AppID"),
                    .UserID = Trim(dt.Rows(i)("UserID")),
                    .FullName = Trim(dt.Rows(i)("FullName")),
                    .Password = clsDESEncryption.DecryptData(dt.Rows(i)("Password")),
                    .AdminStatus = If(dt.Rows(i)("AdminStatus") = "1", "Yes", "No"),
                    .Description = Trim(dt.Rows(i)("Description") & ""),
                    .LockStatus = dt.Rows(i)("LockStatus"),
                    .FactoryCode = dt.Rows(i)("FactoryCode") & "",
                    .JobPosition = dt.Rows(i)("JobPosition") & "",
                    .EmployeeID = dt.Rows(i)("EmployeeID") & "",
                    .Email = dt.Rows(i)("Email") & "",
                    .LastUpdate = Date.Parse(dt.Rows(i)("UpdateDate").ToString()).ToString("yyyy MMM dd HH:mm:ss"),
                    .LastUser = dt.Rows(i)("UpdateUser") & ""
                }
                Users.Add(User)
            Next
            Return Users
        End Using
    End Function

    Public Shared Function GetData(UserID As String) As clsUserSetup
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String
                Dim clsDESEncryption As New clsDESEncryption("TOS")
                sql = " SELECT * FROM dbo.spc_UserSetup where UserID = @UserID " & vbCrLf
                Dim cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Dim Users As New List(Of clsUserSetup)
                If dt.Rows.Count > 0 Then
                    Dim i As Integer = 0
                    Dim User As New clsUserSetup With {
                            .AppID = dt.Rows(i)("AppID"),
                            .UserID = Trim(dt.Rows(i)("UserID")),
                            .FullName = Trim(dt.Rows(i)("FullName")),
                            .Password = clsDESEncryption.DecryptData(dt.Rows(i)("Password")),
                            .Description = Trim(dt.Rows(i)("Description") & ""),
                            .LockStatus = Val(dt.Rows(i)("LockStatus") & ""),
                            .FactoryCode = dt.Rows(i)("FactoryCode") & "",
                            .FailedLogin = Val(dt.Rows(i)("FailedLogin") & ""),
                            .AdminStatus = Val(dt.Rows(i)("AdminStatus") & "")
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

    Public Shared Function GetEmployee(EmployeeID As String) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String
                Dim clsDESEncryption As New clsDESEncryption("TOS")
                sql = " SELECT * FROM dbo.spc_UserSetup where EmployeeID = @EmployeeID" & vbCrLf
                Dim cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("EmployeeID", EmployeeID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetUserID() As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SELECT UserID, FullName FROM dbo.spc_UserSetup"
                Dim cmd As New SqlCommand(sql, conn)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function
End Class
