﻿Imports System.Data.SqlClient

Public Class clsSPCAlertDashboardDB

    Public Shared Function Delete(pFactoryCode As String, pItemTypeCode As String, pLineCode As String, pItemCheck As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Delete from spc_ItemCheckByType WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheck"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", pFactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", pItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", pLineCode)
            cmd.Parameters.AddWithValue("ItemCheck", pItemCheck)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function GetList(User As String, FactoryCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetDelayInput"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", User)
                    .AddWithValue("FactoryCode", FactoryCode)
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
    Public Shared Function GetDelayVerificationGrid(User As String, FactoryCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetDelayVerification"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", User)
                    .AddWithValue("FactoryCode", FactoryCode)
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
    Public Shared Function GetNGDataList(User As String, FactoryCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetNGInput"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", User)
                    .AddWithValue("FactoryCode", FactoryCode)
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

    Public Shared Function GetVerifyDataList(User As String, FactoryCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetDelayVerify"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", User)
                    .AddWithValue("FactoryCode", FactoryCode)
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
    Public Shared Function GetData(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String) As ClsSPCItemCheckByType
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " select * from spc_ItemCheckByType where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheckCode " & vbCrLf
            Dim cmd As New SqlCommand(sql, cn)
            Dim da As New SqlDataAdapter(cmd)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of ClsSPCItemCheckByType)
            If dt.Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim User As New ClsSPCItemCheckByType With {
                    .FactoryCode = dt.Rows(i)("FactoryCode"),
                    .ItemTypeCode = dt.Rows(i)("ItemTypeCode"),
                    .LineCode = dt.Rows(i)("MachineProcess"),
                    .ItemCheck = Trim(dt.Rows(i)("ItemCheck")),
                    .FrequencyCode = Trim(dt.Rows(i)("FrequencyName")),
                    .RegistrationNo = Trim(dt.Rows(i)("RegistrationNo")),
                    .SampleSize = Trim(dt.Rows(i)("SampleSize")),
                    .Remark = Trim(dt.Rows(i)("Remark")),
                    .Evaluation = Trim(dt.Rows(i)("Evaluation")),
                    .CharacteristicItem = Trim(dt.Rows(i)("CharacteristicItem")),
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
    Public Shared Function GetMachineProccess(Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                'If Type = "1" Then
                '    sql = "SELECT LineCode, LineName FROM MS_Line"
                'ElseIf Type = "2" Then
                sql = "SELECT '0' LineCode, 'ALL' LineName UNION SELECT LineCode, LineName = LineCode +  ' - ' + LineName FROM MS_Line"
                'End If

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.Text
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
    Public Shared Function GetItemCheckMaster(Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SELECT 'ALL' ItemCheckCode, 'ALL' ItemCheck UNION SELECT  ItemCheckCode, ItemCheck from spc_ItemCheckMaster"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.Text
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
    Public Shared Function GetFrequencyCode(Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SELECT 'ALL' FrequencyCode, 'ALL' FrequencyName UNION SELECT FrequencyCode, FrequencyName from spc_MS_FrequencySetting"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.Text
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
    Public Shared Function GetItemTypeCode(Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SELECT ItemTypeCode, ItemTypeName = Description from MS_ItemType"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.Text
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
    Public Shared Function GetFactoryCode(Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SELECT FactoryCode, FactoryName from MS_Factory"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.Text
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
    Public Shared Function SendEmail(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, LinkDate As String, ShiftCode As String, SequenceNo As String, Optional ByRef pErr As String = "") As Integer
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim q As String
                q = "SP_SPC_SendEmailAlert"
                Dim cmd As New SqlCommand(q, Cn)
                'Dim des As New clsDESEncryption("TOS")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                cmd.Parameters.AddWithValue("LineCode", LineCode)
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
                cmd.Parameters.AddWithValue("ProdDate", LinkDate)
                cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
                cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)
                cmd.Parameters.AddWithValue("LastUser", "spc")
                Dim i As Integer = cmd.ExecuteNonQuery
                Return i
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try

    End Function
    Public Shared Function CheckDataSendNotification(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, LinkDate As String, ShiftCode As String, SequenceNo As String, Optional ByRef pErr As String = "") As DataTable

        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim q As String
                q = "SP_SPC_CheckDataSendEmailAlert"
                Dim cmd As New SqlCommand(q, Cn)
                'Dim des As New clsDESEncryption("TOS")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                cmd.Parameters.AddWithValue("LineCode", LineCode)
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
                cmd.Parameters.AddWithValue("ProdDate", LinkDate)
                cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
                cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)
                cmd.Parameters.AddWithValue("NotificationCategory", "DV")

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
    Public Shared Function SendNotification(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, LinkDate As String, ShiftCode As String, SequenceNo As String, Optional ByRef pErr As String = "") As Integer
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim q As String
                q = "sp_SPC_NotificationLog_Ins"
                Dim cmd As New SqlCommand(q, Cn)
                'Dim des As New clsDESEncryption("TOS")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                cmd.Parameters.AddWithValue("LineCode", LineCode)
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
                cmd.Parameters.AddWithValue("ProdDate", LinkDate)
                cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
                cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)
                cmd.Parameters.AddWithValue("NotificationCategory", "DV")
                cmd.Parameters.AddWithValue("LastUser", "spc")
                Dim i As Integer = cmd.ExecuteNonQuery
                Return i
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try

    End Function
End Class
