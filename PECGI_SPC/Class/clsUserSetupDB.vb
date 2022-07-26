﻿Imports System.Data.SqlClient

Public Class clsUserSetupDB
    Public Shared Function Insert(pUser As clsUserSetup) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "INSERT INTO UserSetup (" & vbCrLf & _
                "   [AppID],[UserID],[FullName],[Password],[Description],[AdminStatus],[LockStatus]," & _
                "   LineLeaderStatus, LineForemanStatus, ProdSectionHeadStatus, " & vbCrLf & _
                "   QELeaderStatus, QESectionHeadStatus, " & vbCrLf & _
                "   CreateDate, CreateUser " & vbCrLf & _
                ") VALUES ( " & vbCrLf & _
                "   @AppID, @UserID, @FullName, @Password, @Description, @AdminStatus, @LockStatus, " & vbCrLf & _
                "   @LineLeaderStatus, @LineForemanStatus, @ProdSectionHeadStatus, " & vbCrLf & _
                "   @QELeaderStatus, @QESectionHeadStatus, " & vbCrLf & _
                "   GETDATE(), @CreateUser )"
            Dim cmd As New SqlCommand(q, Cn)
            Dim des As New clsDESEncryption("TOS")
            With cmd.Parameters
                .AddWithValue("AppID", "QCS")
                .AddWithValue("UserID", pUser.UserID)
                .AddWithValue("FullName", pUser.FullName)
                .AddWithValue("AdminStatus", Val(pUser.AdminStatus & ""))
                Dim pwd As String = des.EncryptData(pUser.Password)
                .AddWithValue("Password", pwd)
                .AddWithValue("Description", pUser.Description)
                .AddWithValue("LineLeaderStatus", Val(pUser.LineLeaderStatus & ""))
                .AddWithValue("LineForemanStatus", Val(pUser.LineForemanStatus & ""))
                .AddWithValue("ProdSectionHeadStatus", Val(pUser.ProdSectionHeadStatus & ""))
                .AddWithValue("QELeaderStatus", Val(pUser.QELeaderStatus & ""))
                .AddWithValue("QESectionHeadStatus", Val(pUser.QESectionHeadStatus & ""))
                .AddWithValue("LockStatus", Val(pUser.LockStatus & ""))
                .AddWithValue("CreateUser", pUser.CreateUser)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function Delete(pUserID As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Delete from UserSetup where UserID = @UserID" & vbCrLf & _
                "Delete from UserPrivilege where UserID = @UserID " & vbCrLf & _
                "Delete from UserLine where UserID = @UserID "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", pUserID)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function UpdatePassword(pUserID As String, pNewPassword As String) As Integer
        Dim des As New clsDESEncryption("TOS")
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "Update UserSetup set Password = @Password, UpdateDate = GetDate(), UpdateUser = @UserID " & vbCrLf & _
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
            Dim q As String = "Update UserSetup set FailedLogin = isnull(FailedLogin, 0) + 1 where UserID = @UserID and isnull(AdminStatus, 0) = 0 "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", pUser)
            Dim i As Integer = cmd.ExecuteNonQuery
            q = "Update UserSetup set LockStatus = 1 where UserID = @UserID and isnull(AdminStatus, 0) = 0 and isnull(FailedLogin, 0) >= 12 "
            cmd.CommandText = q
            cmd.ExecuteNonQuery()
            Return i
        End Using
    End Function

    Public Shared Function ResetFailedLogin(pUser As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Update UserSetup set FailedLogin = 0 where UserID = @UserID "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", pUser)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function Update(pUser As clsUserSetup) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "UPDATE UserSetup SET FullName=@FullName, Password=@Password," &
                "Description=@Description, " & _
                "AdminStatus = @AdminStatus, " & _
                "LineLeaderStatus = @LineLeaderStatus, " & _
                "LineForemanStatus = @LineForemanStatus, " & _
                "ProdSectionHeadStatus = @ProdSectionHeadStatus, " & _
                "QELeaderStatus = @QELeaderStatus, " & _
                "QESectionHeadStatus = @QESectionHeadStatus, " & _
                "LockStatus=@LockStatus, " & vbCrLf & _
                "FailedLogin = 0, UpdateDate = GETDATE(), UpdateUser = @UpdateUser " &
                "WHERE UserID = @UserID and AppID = @AppID "
            Dim des As New clsDESEncryption("TOS")
            Dim cmd As New SqlCommand(q, Cn)
            With cmd.Parameters
                .AddWithValue("AppID", "QCS")
                .AddWithValue("UserID", pUser.UserID)
                .AddWithValue("FullName", pUser.FullName)
                .AddWithValue("AdminStatus", pUser.AdminStatus)
                Dim pwd As String = des.EncryptData(pUser.Password)
                .AddWithValue("Password", pwd)
                .AddWithValue("Description", pUser.Description)
                .AddWithValue("LineLeaderStatus", Val(pUser.LineLeaderStatus & ""))
                .AddWithValue("LineForemanStatus", Val(pUser.LineForemanStatus & ""))
                .AddWithValue("ProdSectionHeadStatus", Val(pUser.ProdSectionHeadStatus & ""))
                .AddWithValue("QELeaderStatus", Val(pUser.QELeaderStatus & ""))
                .AddWithValue("QESectionHeadStatus", Val(pUser.QESectionHeadStatus & ""))
                .AddWithValue("LockStatus", pUser.LockStatus)
                .AddWithValue("UpdateUser", pUser.UpdateUser)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function GetList() As List(Of clsUserSetup)
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " SELECT * FROM dbo.UserSetup " & vbCrLf
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
                    .Description = Trim(dt.Rows(i)("Description") & ""),
                    .LineLeaderStatus = Val(dt.Rows(i)("LineLeaderStatus") & ""),
                    .LineForemanStatus = Val(dt.Rows(i)("LineForemanStatus") & ""),
                    .ProdSectionHeadStatus = Val(dt.Rows(i)("ProdSectionHeadStatus") & ""),
                    .QELeaderStatus = Val(dt.Rows(i)("QELeaderStatus") & ""),
                    .QESectionHeadStatus = Val(dt.Rows(i)("QESectionHeadStatus") & ""),
                    .LockStatus = dt.Rows(i)("LockStatus"),
                    .AdminStatus = dt.Rows(i)("AdminStatus") & ""
                }
                Users.Add(User)
            Next
            Return Users
        End Using
    End Function

    Public Shared Function GetData(UserID As String) As clsUserSetup
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " SELECT * FROM dbo.UserSetup where UserID = @UserID " & vbCrLf
            Dim cmd As New SqlCommand(sql, cn)
            Dim da As New SqlDataAdapter(cmd)
            cmd.Parameters.AddWithValue("UserID", UserID)
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
                        .LineLeaderStatus = Val(dt.Rows(i)("LineLeaderStatus") & ""),
                        .LineForemanStatus = Val(dt.Rows(i)("LineForemanStatus") & ""),
                        .ProdSectionHeadStatus = Val(dt.Rows(i)("ProdSectionHeadStatus") & ""),
                        .QELeaderStatus = Val(dt.Rows(i)("QELeaderStatus") & ""),
                        .QESectionHeadStatus = Val(dt.Rows(i)("QESectionHeadStatus") & ""),
                        .LockStatus = Val(dt.Rows(i)("LockStatus") & ""),
                        .FailedLogin = Val(dt.Rows(i)("FailedLogin") & ""),
                        .AdminStatus = Val(dt.Rows(i)("AdminStatus") & "")
                    }
                Return User
            Else
                Return Nothing
            End If
        End Using
    End Function

    Public Shared Function GetListMenu(ByVal pUserID As String, Optional ByRef pErr As String = "") As List(Of Cls_ss_UserMenu)
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String = "  SELECT GroupID, USM.MenuID,   " & vbCrLf & _
                  "  MenuDesc, " & vbCrLf & _
                  "  ISNULL(AllowAccess,'0') AS AllowAccess,  " & vbCrLf & _
                  "  ISNULL(AllowUpdate,'0') AS AllowUpdate  " & vbCrLf & _
                  "  FROM UserMenu USM " & vbCrLf & _
                  "  LEFT JOIN (SELECT * FROM UserPrivilege WHERE UserID='" & pUserID & "' ) UP   " & vbCrLf & _
                  "  ON USM.AppID = UP.AppID AND USM.MenuID=UP.MenuID    " & vbCrLf & _
                  "  WHERE USM.AppID='QCS' and USM.MenuID <> 'Z010' " & vbCrLf & _
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
