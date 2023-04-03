Imports System.Data.SqlClient

Public Class ClsFTAMasterDB
    Public Shared Function Insert(pFTAMaster As ClsFTAMaster) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "sp_SPC_FTAMaster"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            Dim des As New clsDESEncryption("TOS")
            With cmd.Parameters
                .AddWithValue("FactoryCode", pFTAMaster.FactoryCode)
                .AddWithValue("ItemTypeCode", pFTAMaster.ItemTypeCode)
                .AddWithValue("ItemCheckCode", pFTAMaster.ItemCheck)
                .AddWithValue("FTAID", pFTAMaster.FTAID)
                .AddWithValue("Factor1", pFTAMaster.Factor1)
                .AddWithValue("Factor2", pFTAMaster.Factor2)
                .AddWithValue("Factor3", pFTAMaster.Factor3)
                .AddWithValue("Factor4", pFTAMaster.Factor4)
                .AddWithValue("CounterMeasure", pFTAMaster.CounterMeasure)
                .AddWithValue("CheckItem", pFTAMaster.CheckItem)
                .AddWithValue("CheckOrder", pFTAMaster.CheckOrder)
                .AddWithValue("Remark", pFTAMaster.Remark)
                .AddWithValue("ActiveStatus", Val(pFTAMaster.ActiveStatus & ""))
                .AddWithValue("RegisterUser", pFTAMaster.CreateUser)
                .AddWithValue("TypeProcess", 2)

            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function Delete(pFactoryCode As String, pItemTypeCode As String, pItemCheck As String, pFTAID As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_FTAMaster"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", pFactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", pItemTypeCode)
            cmd.Parameters.AddWithValue("ItemCheckCode", pItemCheck)
            cmd.Parameters.AddWithValue("FTAID", pFTAID)
            cmd.Parameters.AddWithValue("TypeProcess", 8)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function Update(pFTAMaster As ClsFTAMaster) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            'q = "UPDATE spc_ItemCheckByType SET FrequencyCode = @FrequencyCode, RegistrationNo = @RegistrationNo, SampleSize = @SampleSize, Remark = @Remark, " &
            '    " Evaluation = @Evaluation, CharacteristicStatus = @CharacteristicItem, ProcessTableLineCode = @ProcessTableLineCode, ActiveStatus = @ActiveStatus, UpdateUser = @UpdateUser, UpdateDate = GETDATE() " &
            '    " WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheck "

            q = "sp_SPC_FTAMaster"
            Dim des As New clsDESEncryption("TOS")
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            With cmd.Parameters
                .AddWithValue("FactoryCode", pFTAMaster.FactoryCode)
                .AddWithValue("ItemTypeCode", pFTAMaster.ItemTypeCode)
                .AddWithValue("ItemCheckCode", pFTAMaster.ItemCheck)
                .AddWithValue("FTAID", pFTAMaster.FTAID)
                .AddWithValue("Factor1", pFTAMaster.Factor1)
                .AddWithValue("Factor2", pFTAMaster.Factor2)
                .AddWithValue("Factor3", pFTAMaster.Factor3)
                .AddWithValue("Factor4", pFTAMaster.Factor4)
                .AddWithValue("CounterMeasure", pFTAMaster.CounterMeasure)
                .AddWithValue("CheckItem", pFTAMaster.CheckItem)
                .AddWithValue("CheckOrder", pFTAMaster.CheckOrder)
                .AddWithValue("Remark", pFTAMaster.Remark)
                .AddWithValue("ActiveStatus", Val(pFTAMaster.ActiveStatus & ""))
                .AddWithValue("UpdateUser", pFTAMaster.UpdateUser)
                .AddWithValue("TypeProcess", 9)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
    Public Shared Function GetList(FactoryCode As String, ItemCheckCode As String, ItemTypeCode As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_FTAMaster_GetList"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("FactoryCode", FactoryCode)
                    .AddWithValue("ItemCheckCode", ItemCheckCode)
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
    Public Shared Function ValidateData(pFTAMaster As ClsFTAMaster) As ClsFTAMaster
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")

            sql = "sp_SPC_FTAMaster"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            Dim da As New SqlDataAdapter(cmd)
            cmd.Parameters.AddWithValue("FactoryCode", pFTAMaster.FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", pFTAMaster.ItemTypeCode)
            cmd.Parameters.AddWithValue("ItemCheckCode", pFTAMaster.ItemCheck)
            cmd.Parameters.AddWithValue("FTAID", pFTAMaster.FTAID)
            cmd.Parameters.AddWithValue("TypeProcess", 1)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of ClsFTAMaster)
            If dt.Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim FTAMaster As New ClsFTAMaster With {
                    .FTAID = dt.Rows(i)("FTAID")
                    }
                Return FTAMaster
            Else
                Return Nothing
            End If
        End Using
    End Function

    Public Shared Function GetMachineProccess(UserID As String, FactoryCode As String, Machine As String, Optional ByRef pErr As String = "") As List(Of ClsFTAMaster)
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                'Dim q As String = "select distinct Number = 2, L.FactoryCode, L.ProcessCode, L.LineCode, L.LineCode + ' - ' + L.LineName as LineName from MS_Line L " & vbCrLf
                'If FactoryCode <> "" Then
                '    q = q & "where L.FactoryCode = @FactoryCode AND L.ProcessCode = @Machine "
                'End If
                'q = q & "order by Number ASC, LineCode"
                Dim Sql As String = "sp_SPC_ItemCheckByTypeMaster"
                Dim cmd As New SqlCommand(Sql, Cn)
                cmd.CommandType = CommandType.StoredProcedure
                Dim da As New SqlDataAdapter(cmd)
                'cmd.Parameters.AddWithValue("UserID", UserID)
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("Machine", Machine)
                cmd.Parameters.AddWithValue("TypeProcess", 2)
                'cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                Dim rd As SqlDataReader = cmd.ExecuteReader
                Dim FactoryList As New List(Of ClsFTAMaster)
                Do While rd.Read
                    Dim Factory As New ClsFTAMaster
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
    Public Shared Function UploadIK_UpdIns(pUploadIK As ClsFTAMaster) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "sp_SPC_FTAMaster"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            Dim des As New clsDESEncryption("TOS")
            With cmd.Parameters
                .AddWithValue("FactoryCode", pUploadIK.FactoryCode)
                .AddWithValue("ItemTypeCode", pUploadIK.ItemTypeCode)
                .AddWithValue("ItemCheckCode", pUploadIK.ItemCheck)
                .AddWithValue("FTAID", pUploadIK.FTAID)
                .AddWithValue("IK", pUploadIK.Image)
                .AddWithValue("UpdateUser", pUploadIK.UpdateUser)
                .AddWithValue("TypeProcess", 3)

            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
    Public Shared Function GetListActionByFTAID(FTAID As String, Optional ByRef pErr As String = "") As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_FTAMaster"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("FTAID", FTAID)
                    .AddWithValue("TypeProcess", 4)
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
    Public Shared Function UpdateFTAAction(pFTAID As String, pActionName As String, pActionID As String, pRemark As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "sp_SPC_FTAMaster"
            Dim des As New clsDESEncryption("TOS")
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            With cmd.Parameters
                .AddWithValue("ActionName", pActionName)
                .AddWithValue("ActionID", pActionID)
                .AddWithValue("Remark", pRemark)
                .AddWithValue("FTAID", pFTAID)
                .AddWithValue("TypeProcess", 5)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
    Public Shared Function DeleteAction(pFTAID As String, pActionID As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_FTAMaster"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            With cmd.Parameters
                .AddWithValue("ActionID", pActionID)
                .AddWithValue("FTAID", pFTAID)
                .AddWithValue("TypeProcess", 6)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
    Public Shared Function InsertAction(pFTAID As String, pActionName As String, pActionID As String, pRemark As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            q = "sp_SPC_FTAMaster"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            Dim des As New clsDESEncryption("TOS")
            With cmd.Parameters
                .AddWithValue("ActionName", pActionName)
                .AddWithValue("ActionID", pActionID)
                .AddWithValue("Remark", pRemark)
                .AddWithValue("FTAID", pFTAID)
                .AddWithValue("TypeProcess", 7)
            End With
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
    Public Shared Function GetListForExcel(FactoryCode As String, ItemCheckCode As String, ItemTypeCode As String, Optional ByRef pErr As String = "") As DataSet
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_FTAMaster_GetList"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("FactoryCode", FactoryCode)
                    .AddWithValue("ItemCheckCode", ItemCheckCode)
                    .AddWithValue("ItemTypeCode", ItemTypeCode)
                End With
                Dim da As New SqlDataAdapter(cmd)
                Dim ds As New DataSet
                da.Fill(ds)

                Return ds
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
                sql = "sp_SPC_ItemCheckByTypeMaster"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("TypeProcess", 3)
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
    Public Shared Function ValidateDataFTAIDinC010(pFTAID As String) As ClsFTAMaster
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")

            sql = "sp_SPC_FTAMaster"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            Dim da As New SqlDataAdapter(cmd)
            cmd.Parameters.AddWithValue("FTAID", pFTAID)
            cmd.Parameters.AddWithValue("TypeProcess", 10)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of ClsFTAMaster)
            If dt.Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim FTAMaster As New ClsFTAMaster With {
                    .FTAID = dt.Rows(i)("FTAID")
                    }
                Return FTAMaster
            Else
                Return Nothing
            End If
        End Using
    End Function
    Public Shared Function ValidationDelete(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String) As ClsFTAMaster
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            Dim sql As String
            Dim clsDESEncryption As New clsDESEncryption("TOS")
            'sql = " select top 1 * from spc_Result where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheckCode " & vbCrLf
            sql = "sp_SPC_ItemCheckByTypeMaster"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            Dim da As New SqlDataAdapter(cmd)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("TypeProcess", 4)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim Users As New List(Of ClsFTAMaster)
            If dt.Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim User As New ClsFTAMaster With {
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
            sql = "sp_SPC_ItemCheckByTypeMaster"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("TypeProcess", 5)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        End Using
    End Function
End Class
