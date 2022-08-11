﻿Imports System.Data.SqlClient

Public Class clsUserSetupDB
    Public Shared Function Insert(pUser As clsUserSetup) As String
        Dim Msg = ""
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim sqL As String
                sqL = "INSERT INTO dbo.spc_UserSetup (" & vbCrLf &
                        "   [AppID],[UserID],[FullName],[Password],[Description],[AdminStatus]," & vbCrLf &
                        "   [FactoryCode],[JobPosition],[Email],[LockStatus]," & vbCrLf &
                        "   [RegisterDate],[RegisterUser],[UpdateDate],[UpdateUser]" & vbCrLf &
                        ") VALUES ( " & vbCrLf &
                        "   @AppID, @UserID, @FullName, @Password, @Description, @AdminStatus," & vbCrLf &
                        "   @FactoryCode, @JobPosition, @Email, @LockStatus, " & vbCrLf &
                        "   GETDATE(), @RegisterUser, GETDATE(), @RegisterUser )"
                Dim cmd As New SqlCommand(sqL, Cn)
                Dim des As New clsDESEncryption("TOS")
                With cmd.Parameters
                    .AddWithValue("AppID", "SPC")
                    .AddWithValue("UserID", pUser.UserID)
                    .AddWithValue("FullName", pUser.FullName)
                    .AddWithValue("AdminStatus", Val(pUser.AdminStatus & ""))
                    Dim pwd As String = des.EncryptData(pUser.Password)
                    .AddWithValue("Password", pwd)
                    .AddWithValue("Description", pUser.Description)
                    .AddWithValue("FactoryCode", pUser.FactoryCode)
                    .AddWithValue("JobPosition", pUser.JobPosition)
                    .AddWithValue("Email", pUser.Email)
                    .AddWithValue("LockStatus", Val(pUser.LockStatus & ""))
                    .AddWithValue("RegisterUser", pUser.CreateUser)
                End With
                cmd.ExecuteNonQuery()
                Return Msg
            End Using
        Catch ex As Exception
            Msg = ex.Message
            Throw ex
            Return Msg
        End Try
    End Function

    Public Shared Function Update(pUser As clsUserSetup) As String
        Dim Msg = ""
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim q As String
                q = "UPDATE dbo.spc_UserSetup SET FullName=@FullName, Password=@Password," &
                    "Description=@Description, " &
                    "AdminStatus = @AdminStatus, " &
                    "FactoryCode = @FactoryCode, " &
                    "JobPosition = @JobPosition, " &
                    "Email = @Email, " &
                    "LockStatus = @LockStatus, " & vbCrLf &
                    "FailedLogin = 0, UpdateDate = GETDATE(), UpdateUser = @UpdateUser " &
                    "WHERE UserID = @UserID and AppID = @AppID "
                Dim des As New clsDESEncryption("TOS")
                Dim cmd As New SqlCommand(q, Cn)
                With cmd.Parameters
                    .AddWithValue("AppID", "SPC")
                    .AddWithValue("UserID", pUser.UserID)
                    .AddWithValue("FullName", pUser.FullName)
                    .AddWithValue("AdminStatus", pUser.AdminStatus)
                    Dim pwd As String = des.EncryptData(pUser.Password)
                    .AddWithValue("Password", pwd)
                    .AddWithValue("Description", pUser.Description)
                    .AddWithValue("FactoryCode", pUser.FactoryCode)
                    .AddWithValue("JobPosition", pUser.JobPosition)
                    .AddWithValue("Email", pUser.Email)
                    .AddWithValue("LockStatus", pUser.LockStatus)
                    .AddWithValue("UpdateUser", pUser.UpdateUser)
                End With
                cmd.ExecuteNonQuery()
                Return Msg
            End Using
        Catch ex As Exception
            Msg = ex.Message
            Throw ex
            Return Msg
        End Try
    End Function

    Public Shared Function Delete(pUserID As String) As String
        Dim Msg = ""
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim q As String = "Delete from dbo.spc_UserPrivilege where AppID = 'SPC' AND UserID = @UserID" & vbCrLf &
                                  "Delete from dbo.spc_UserLine where AppID = 'SPC' AND UserID = @UserID" & vbCrLf &
                                  "Delete from dbo.spc_UserSetup where AppID = 'SPC' AND UserID = @UserID"
                Dim cmd As New SqlCommand(q, Cn)
                cmd.Parameters.AddWithValue("UserID", pUserID)
                cmd.ExecuteNonQuery()
                Return Msg
            End Using
        Catch ex As Exception
            Msg = ex.Message
            Throw ex
            Return Msg
        End Try
    End Function

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

    Public Shared Function GetListMenu(ByVal pUserID As String, Optional ByRef pErr As String = "") As List(Of Cls_ss_UserMenu)
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String = "  SELECT GroupID, USM.MenuID,   " & vbCrLf &
                  "  MenuDesc, " & vbCrLf &
                  "  ISNULL(AllowAccess,'0') AS AllowAccess,  " & vbCrLf &
                  "  ISNULL(AllowUpdate,'0') AS AllowUpdate,  " & vbCrLf &
                  "  ISNULL(AllowDelete,'0') AS AllowDelete  " & vbCrLf &
                  "  FROM dbo.spc_UserMenu USM " & vbCrLf &
                  "  LEFT JOIN (SELECT * FROM dbo.spc_UserPrivilege WHERE UserID='" & pUserID & "' ) UP   " & vbCrLf &
                  "  ON USM.AppID = UP.AppID AND USM.MenuID=UP.MenuID" & vbCrLf &
                  "  WHERE USM.AppID='SPC'" & vbCrLf &
                  "  ORDER BY USM.MenuID  "
                Dim Cmd As New SqlCommand(sql, cn)
                Dim da As New SqlDataAdapter(Cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Dim Menus As New List(Of Cls_ss_UserMenu)
                For i = 0 To dt.Rows.Count - 1
                    Dim Menu As New Cls_ss_UserMenu
                    Menu.GroupID = dt.Rows(i)("GroupID")
                    Menu.MenuID = dt.Rows(i)("MenuID")
                    Menu.MenuDesc = dt.Rows(i)("MenuDesc")
                    Menu.AllowAccess = dt.Rows(i)("AllowAccess")
                    Menu.AllowUpdate = dt.Rows(i)("AllowUpdate")
                    Menu.AllowDelete = dt.Rows(i)("AllowDelete")
                    Menus.Add(Menu)
                Next
                Return Menus
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function
End Class