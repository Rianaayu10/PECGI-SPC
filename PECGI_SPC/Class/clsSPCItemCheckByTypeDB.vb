Imports System.Data.SqlClient

Public Class ClsSPCItemCheckByTypeDB
    Public Shared Function Insert(pItemCheckByType As ClsSPCItemCheckByType) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "INSERT INTO spc_ItemCheckByType " & vbCrLf &
                " VALUES ( @FactoryCode, @ItemTypeCode, @LineCode, @ItemCheck, @FrequencyCode, @RegistrationNo," &
                " @SampleSize, @Remark, @Evaluation, @CharacteristicItem, @ProcessTableLineCode, @ActiveStatus, @CreateUser, GETDATE(), @CreateUser, GETDATE() ) "
            Dim cmd As New SqlCommand(q, Cn)
            Dim des As New clsDESEncryption("TOS")
            With cmd.Parameters
                .AddWithValue("FactoryCode", pItemCheckByType.FactoryCode)
                .AddWithValue("ItemTypeCode", pItemCheckByType.ItemTypeCode)
                .AddWithValue("LineCode", pItemCheckByType.LineCode)
                .AddWithValue("ItemCheck", pItemCheckByType.ItemCheck)
                .AddWithValue("FrequencyCode", pItemCheckByType.FrequencyCode)
                .AddWithValue("RegistrationNo", pItemCheckByType.RegistrationNo)
                .AddWithValue("SampleSize", pItemCheckByType.SampleSize)
                .AddWithValue("Remark", pItemCheckByType.Remark)
                .AddWithValue("Evaluation", pItemCheckByType.Evaluation)
                .AddWithValue("CharacteristicItem", Val(pItemCheckByType.CharacteristicItem))
                .AddWithValue("ProcessTableLineCode", pItemCheckByType.ProcessTableLineCode)
                .AddWithValue("ActiveStatus", Val(pItemCheckByType.ActiveStatus & ""))
                .AddWithValue("CreateUser", pItemCheckByType.CreateUser)

            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

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

    Public Shared Function Update(pItemCheckByType As ClsSPCItemCheckByType) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "UPDATE spc_ItemCheckByType SET FrequencyCode = @FrequencyCode, RegistrationNo = @RegistrationNo, SampleSize = @SampleSize, Remark = @Remark, " &
                " Evaluation = @Evaluation, CharacteristicStatus = @CharacteristicItem, ProcessTableLineCode = @ProcessTableLineCode, ActiveStatus = @ActiveStatus, UpdateUser = @UpdateUser, UpdateDate = GETDATE() " &
                " WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheck "
            Dim des As New clsDESEncryption("TOS")
            Dim cmd As New SqlCommand(q, Cn)
            With cmd.Parameters
                .AddWithValue("FrequencyCode", pItemCheckByType.FrequencyCode)
                .AddWithValue("RegistrationNo", pItemCheckByType.RegistrationNo)
                .AddWithValue("SampleSize", pItemCheckByType.SampleSize)
                .AddWithValue("Remark", pItemCheckByType.Remark)
                .AddWithValue("Evaluation", pItemCheckByType.Evaluation)
                .AddWithValue("CharacteristicItem", pItemCheckByType.CharacteristicItem)
                .AddWithValue("ProcessTableLineCode", pItemCheckByType.ProcessTableLineCode)
                .AddWithValue("ActiveStatus", pItemCheckByType.ActiveStatus)
                .AddWithValue("UpdateUser", pItemCheckByType.UpdateUser)
                .AddWithValue("FactoryCode", pItemCheckByType.FactoryCode)
                .AddWithValue("ItemTypeCode", pItemCheckByType.ItemTypeCode)
                .AddWithValue("LineCode", pItemCheckByType.LineCode)
                .AddWithValue("ItemCheck", pItemCheckByType.ItemCheck)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
    Public Shared Function GetList(pUser As String, FactoryCode As String, ItemTypeDescription As String, MachineProccess As String, ItemTypeCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_ItemCheckByType_GetList"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", pUser)
                    .AddWithValue("FactoryCode", FactoryCode)
                    .AddWithValue("ItemTypeDescription", ItemTypeDescription)
                    .AddWithValue("MachineProccess", MachineProccess)
                    .AddWithValue("ItemTypeCode", ItemTypeCode)
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
            sql = " select ICT.*, ItemTypeName = IT.Description from spc_ItemCheckByType ICT inner join MS_ItemType IT ON ICT.ItemTypeCode = IT.ItemTypeCode where 
                    ICT.FactoryCode = @FactoryCode and ICT.ItemTypeCode = @ItemTypeCode and ICT.LineCode = @LineCode and ICT.ItemCheckCode = @ItemCheckCode " & vbCrLf
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
                Dim BatteryType As New ClsSPCItemCheckByType With {
                    .FactoryCode = dt.Rows(i)("FactoryCode"),
                    .ItemTypeCode = dt.Rows(i)("ItemTypeCode"),
                    .LineCode = dt.Rows(i)("LineCode"),
                    .ItemCheck = Trim(dt.Rows(i)("ItemCheckCode")),
                    .FrequencyCode = Trim(dt.Rows(i)("FrequencyCode")),
                    .RegistrationNo = Trim(dt.Rows(i)("RegistrationNo")),
                    .SampleSize = Trim(dt.Rows(i)("SampleSize")),
                    .Remark = Trim(dt.Rows(i)("Remark")),
                    .Evaluation = Trim(dt.Rows(i)("Evaluation")),
                    .CharacteristicItem = Trim(dt.Rows(i)("CharacteristicStatus")),
                    .ActiveStatus = Trim(dt.Rows(i)("ActiveStatus")),
                    .UpdateUser = Trim(dt.Rows(i)("UpdateUser")),
                    .UpdateDate = Trim(dt.Rows(i)("UpdateDate")),
                    .ItemTypeName = dt.Rows(i)("ItemTypeName")
                    }
                Return BatteryType
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
    Public Shared Function GetMachineProccess(UserID As String, FactoryCode As String, ItemTypeCode As String, Optional ByRef pErr As String = "") As List(Of ClsSPCItemCheckByType)
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim q As String = "SELECT Number = 1, 'ALL' FactoryCode, 'ALL' ProcessCode, 'ALL' LineCode, 'ALL' LineName UNION" & vbCrLf &
                "select distinct Number = 2, L.FactoryCode, L.ProcessCode, L.LineCode, L.LineCode + ' - ' + L.LineName as LineName from MS_Line L " & vbCrLf
                '"from MS_Line L inner join spc_ItemCheckByType I " & vbCrLf &
                '"on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode " & vbCrLf &
                '" where 1 = 1 " & vbCrLf
                If FactoryCode <> "" Then
                    q = q & "where L.FactoryCode = @FactoryCode "
                End If
                'If ItemTypeCode <> "" Then
                '    q = q & "and I.ItemTypeCode = @ItemTypeCode "
                'End If
                q = q & "order by Number ASC, LineCode"
                Dim cmd As New SqlCommand(q, Cn)
                cmd.Parameters.AddWithValue("UserID", UserID)
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                Dim rd As SqlDataReader = cmd.ExecuteReader
                Dim FactoryList As New List(Of ClsSPCItemCheckByType)
                Do While rd.Read
                    Dim Factory As New ClsSPCItemCheckByType
                    Factory.FactoryCode = rd("FactoryCode")
                    Factory.LineCode = rd("LineCode")
                    Factory.LineName = rd("LineName")
                    FactoryList.Add(Factory)
                Loop
                rd.Close()
                Return FactoryList
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
    Public Shared Function ValidationDelete(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String) As ClsSPCItemCheckByType
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            sql = " select top 1 * from spc_Result where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheckCode " & vbCrLf
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
                    .ItemCheck = dt.Rows(i)("ItemCheckCode")
                    }
                Return User
            Else
                Return Nothing
            End If
        End Using
    End Function
    Public Shared Function GetRegNo(FactoryCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_ItemCheckByBattery_FillCombo"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("Type", "6")
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
    Public Shared Function FillComboFactoryGrid(Type As String, Optional User As String = "") As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_ItemCheckByBattery_FillCombo"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("Type", Type)
            If User <> "" Then cmd.Parameters.AddWithValue("User", User)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        End Using
    End Function
    Public Shared Function GetListItemType() As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "select ItemTypeCode, Description from MS_ItemType order by Description ASC"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.Text

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        End Using
    End Function
End Class
