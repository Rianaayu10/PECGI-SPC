﻿Imports System.Data.SqlClient

Public Class clsIOT
    Public Shared Function GetDailyProd(FactoryCode As String, LineCode As String, ItemTypeCode As String, LotNo As String, ProdDate As String) As DataTable
        Using Cn As New SqlConnection(Sconn.IOTConnectionString)
            Cn.Open()
            Dim q As String = "SELECT FactoryCode = A.Factory_Code, C.ProcessGroup, C.LineGroup, ProcessCode = A.process_Code, " & vbCrLf &
                "LineCode = A.Line_Code, ScheduleDate = CAST(A.Schedule_Date AS Date), InstructionNo = A.Instruction_No," & vbCrLf &
                "Shift = A.Shift, ItemCode = A.Item_code, LotNo = A.Lot_No" & vbCrLf &
                "FROM Daily_Production A" & vbCrLf &
                "INNER JOIN MS_ItemType B ON A.Item_code = B.Description " & vbCrLf &
                "INNER JOIN MS_Process C ON A.Factory_code = C.FactoryCode And A.process_Code = C.ProcessCode " & vbCrLf &
                "WHERE TRIM(Factory_code) = @FactoryCode And TRIM(Line_Code) = @LineCode And TRIM(ItemTypeCode) = @ItemTypeCode And COALESCE(TRIM(Lot_No),'') = @LotNo AND CAST(Schedule_Date AS DATE) = @ProdDate"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("LotNo", LotNo)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetURL(UserID As String) As String
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "SP_SPC_GetURL"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("UserID", UserID)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt.Rows(0)("URL").ToString
        End Using
    End Function

    Public Shared Function GetEmployeeID(UserID As String) As String
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Select * from dbo.SPC_UserSetup WHERE UserID = @UserID"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", UserID)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt.Rows(0)("EmployeeID").ToString
        End Using
    End Function


    Public Shared Function AllowSkill(EmployeeID As String, FactoryCode As String, LineCode As String, ItemTypeCode As String) As Boolean
        Using Cn As New SqlConnection(Sconn.IOTConnectionString)
            Cn.Open()
            Dim q As String = "Select E.EmployeeID, S.SequenceNo, S.SkillCode " & vbCrLf &
                "from MS_MachineSkillSetting S inner join MS_EmployeeSkill E on S.SkillCode = E.SkillCode " & vbCrLf &
                "inner join MS_ItemDetail D on S.FactoryCode = D.FactoryCode and S.LineCode = D.LineCode " & vbCrLf &
                "inner join Ms_Item I on D.Item_Code = I.Item_Code " & vbCrLf &
                "inner join MS_ItemType T on I.Item_Type = T.ItemTypeCode " & vbCrLf &
                "where S.FactoryCode = @FactoryCode and S.LineCode = @LineCode and I.Item_Type = @ItemTypeCode " & vbCrLf &
                "and E.EmployeeID = @EmployeeID and E.SkillCode = @SkillCode" & vbCrLf &
                "and CAST(GETDATE() AS DATE) BETWEEN CAST(StartDate AS DATE) AND CAST(EndDate AS DATE)"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("EmployeeID", EmployeeID)
            cmd.Parameters.AddWithValue("SkillCode", "SPC001")
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt.Rows.Count > 0
        End Using
    End Function
End Class
