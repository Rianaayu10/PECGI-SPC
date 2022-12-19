Imports System.Data.SqlClient

Public Class ClsSPCItemCheckMasterDB
    Public Shared Function Insert(pItemCheckMaster As ClsSPCItemCheckMaster) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "INSERT INTO spc_ItemCheckMaster (" & vbCrLf &
                "   ItemCheckCode,ItemCheck,UnitMeasurement ,Description,ActiveStatus ," &
                "   RegisterUser,RegisterDate,UpdateUser,UpdateDate" & vbCrLf &
                ") VALUES ( " & vbCrLf &
                "   @ItemCheckCode, @ItemCheck, @UnitMeasurement, @Description, @ActiveStatus, @CreateUser, GETDATE(), @UpdateUser, GETDATE())"
            Dim cmd As New SqlCommand(q, Cn)
            Dim des As New clsDESEncryption("TOS")
            With cmd.Parameters
                .AddWithValue("ItemCheckCode", pItemCheckMaster.ItemCheckCode)
                .AddWithValue("ItemCheck", pItemCheckMaster.ItemCheck)
                .AddWithValue("UnitMeasurement", pItemCheckMaster.UnitMeasurement)
                .AddWithValue("Description", pItemCheckMaster.Description)
                .AddWithValue("ActiveStatus", Val(pItemCheckMaster.ActiveStatus & ""))
                .AddWithValue("UpdateUser", pItemCheckMaster.UpdateUser)
                .AddWithValue("CreateUser", pItemCheckMaster.CreateUser)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
    Public Shared Function Delete(pItemCheckCode As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Delete from spc_ItemCheckMaster where ItemCheckCode = @ItemCheckCode"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("ItemCheckCode", pItemCheckCode)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function Update(pItemCheckMaster As ClsSPCItemCheckMaster) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "UPDATE spc_ItemCheckMaster SET ItemCheck=@ItemCheck, UnitMeasurement=@UnitMeasurement," &
                "Description=@Description, " &
                "ActiveStatus = @ActiveStatus, " &
                "UpdateUser = @UpdateUser, " &
                "UpdateDate = GETDATE() " &
                "WHERE ItemCheckCode = @ItemCheckCode "
            Dim des As New clsDESEncryption("TOS")
            Dim cmd As New SqlCommand(q, Cn)
            With cmd.Parameters
                .AddWithValue("ItemCheck", pItemCheckMaster.ItemCheck)
                .AddWithValue("UnitMeasurement", pItemCheckMaster.UnitMeasurement)
                .AddWithValue("Description", pItemCheckMaster.Description)
                .AddWithValue("ActiveStatus", Val(pItemCheckMaster.ActiveStatus & ""))
                .AddWithValue("UpdateUser", pItemCheckMaster.UpdateUser)
                .AddWithValue("ItemCheckCode", pItemCheckMaster.ItemCheckCode)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function GetList() As List(Of ClsSPCItemCheckMaster)
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " SELECT ItemCheckCode, ItemCheck, UnitMeasurement, Description, ActiveStatus, RegisterUser, FORMAT(RegisterDate, 'dd MMM yy hh:mm:ss') RegisterDate, UpdateUser, FORMAT(UpdateDate, 'dd MMM yy hh:mm:ss') UpdateDate FROM spc_ItemCheckMaster " & vbCrLf
            Dim cmd As New SqlCommand(sql, cn)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of ClsSPCItemCheckMaster)
            For i = 0 To dt.Rows.Count - 1
                Dim pItemCheckMaster As New ClsSPCItemCheckMaster With {
                    .ItemCheckCode = dt.Rows(i)("ItemCheckCode"),
                    .ItemCheck = Trim(dt.Rows(i)("ItemCheck")),
                    .UnitMeasurement = Trim(dt.Rows(i)("UnitMeasurement")),
                    .Description = Trim(dt.Rows(i)("Description") & ""),
                    .ActiveStatus = Trim(dt.Rows(i)("ActiveStatus")),
                    .UpdateUser = Trim(dt.Rows(i)("UpdateUser")),
                    .UpdateDate = Trim(dt.Rows(i)("UpdateDate"))
                }
                Users.Add(pItemCheckMaster)
            Next
            Return Users
        End Using
    End Function

    Public Shared Function GetData(ItemCheckCode As String) As ClsSPCItemCheckMaster
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " SELECT ItemCheckCode, ItemCheck, UnitMeasurement, Description, ActiveStatus, RegisterUser, FORMAT(RegisterDate, 'dd MMM yy hh:mm:ss') RegisterDate, UpdateUser, FORMAT(UpdateDate, 'dd MMM yy hh:mm:ss') UpdateDate FROM spc_ItemCheckMaster where ItemCheckCode = @ItemCheckCode " & vbCrLf
            Dim cmd As New SqlCommand(sql, cn)
            Dim da As New SqlDataAdapter(cmd)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of ClsSPCItemCheckMaster)
            If dt.Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim User As New ClsSPCItemCheckMaster With {
                    .ItemCheckCode = dt.Rows(i)("ItemCheckCode"),
                    .ItemCheck = Trim(dt.Rows(i)("ItemCheck")),
                    .UnitMeasurement = Trim(dt.Rows(i)("UnitMeasurement")),
                    .Description = Trim(dt.Rows(i)("Description") & ""),
                    .ActiveStatus = Trim(dt.Rows(i)("ActiveStatus")),
                    .UpdateUser = Trim(dt.Rows(i)("UpdateUser")),
                    .UpdateDate = Trim(dt.Rows(i)("UpdateDate"))
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
                Dim sql As String = "  SELECT GroupID, USM.MenuID,   " & vbCrLf &
                  "  MenuDesc, " & vbCrLf &
                  "  ISNULL(AllowAccess,'0') AS AllowAccess,  " & vbCrLf &
                  "  ISNULL(AllowUpdate,'0') AS AllowUpdate  " & vbCrLf &
                  "  FROM UserMenu USM " & vbCrLf &
                  "  LEFT JOIN (SELECT * FROM UserPrivilege WHERE UserID='" & pUserID & "' ) UP   " & vbCrLf &
                  "  ON USM.AppID = UP.AppID AND USM.MenuID=UP.MenuID    " & vbCrLf &
                  "  WHERE USM.AppID='QCS' and USM.MenuID <> 'Z010' " & vbCrLf &
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
    Public Shared Function ValidationDelete(ItemCheckCode As String) As ClsSPCItemCheckMaster
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " SELECT * FROM dbo.spc_ItemCheckByType where ItemCheckCode = @ItemCheckCode " & vbCrLf
            Dim cmd As New SqlCommand(sql, cn)
            Dim da As New SqlDataAdapter(cmd)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of ClsSPCItemCheckMaster)
            If dt.Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim User As New ClsSPCItemCheckMaster With {
                    .ItemCheckCode = dt.Rows(i)("ItemCheckCode")
                    }
                Return User
            Else
                Return Nothing
            End If
        End Using
    End Function
End Class
