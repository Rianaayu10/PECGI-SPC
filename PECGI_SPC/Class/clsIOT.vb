﻿Imports System.Data.SqlClient

Public Class clsIOT
    Public Shared Function AllowSkill(UserID As String, FactoryCode As String, LineCode As String, ItemTypeCode As String) As Boolean
        Using Cn As New SqlConnection(Sconn.IOTConnectionString)
            Cn.Open()
            Dim q As String = "Select E.EmployeeID, S.SequenceNo, S.SkillCode " & vbCrLf &
                "from MS_MachineSkillSetting S inner join MS_EmployeeSkill E on S.SkillCode = E.SkillCode " & vbCrLf &
                "inner join MS_ItemDetail D on S.FactoryCode = D.FactoryCode and S.LineCode = D.LineCode " & vbCrLf &
                "inner join Ms_Item I on D.Item_Code = I.Item_Code " & vbCrLf &
                "inner join MS_ItemType T on I.Item_Type = T.ItemTypeCode " & vbCrLf &
                "where S.FactoryCode = @FactoryCode and S.LineCode = @LineCode and I.Item_Type = @ItemTypeCode " & vbCrLf &
                "and E.EmployeeID = @EmployeeID "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("EmployeeID", UserID)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt.Rows.Count > 0
        End Using
    End Function
End Class