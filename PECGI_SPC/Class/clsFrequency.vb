﻿Imports System.Data.SqlClient

Public Class clsFrequency
    Public Property FrequencyCode As String
End Class

Public Class clsShift
    Public Property ShiftCode As String
    Public Property ShiftName As String
End Class

Public Class clsSequenceNo
    Public Property SequenceNo As Integer
    Public Property StartTime As String
End Class

Public Class clsFrequencyDB
    Public Shared Function GetShift(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String) As List(Of clsShift)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select distinct F.ShiftCode, case when F.ShiftCode = 'SH001' then '1' else '2' end ShiftName " & vbCrLf &
                "From spc_ItemCheckByType T inner Join spc_MS_Frequency F on T.FrequencyCode = F.FrequencyCode " & vbCrLf &
                "where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheckCode  " & vbCrLf &
                "and T.ActiveStatus = 1 and F.ActiveStatus = 1  " & vbCrLf
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim ShiftList As New List(Of clsShift)
            Do While rd.Read
                Dim Shift As New clsShift
                Shift.ShiftCode = rd("ShiftCode")
                Shift.ShiftName = rd("ShiftName")
                ShiftList.Add(Shift)
            Loop
            rd.Close()
            Return ShiftList
        End Using
    End Function

    Public Shared Function GetSequence(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, ShiftCode As String, ProdDate As String) As List(Of clsSequenceNo)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select distinct R.SequenceNo, convert(char(5), coalesce(R.RegisterDate, F.StartTime), 114) StartTime " & vbCrLf &
                "From spc_ItemCheckByType T inner Join spc_MS_Frequency F on T.FrequencyCode = F.FrequencyCode " & vbCrLf &
                "left join spc_Result R on R.FactoryCode = T.FactoryCode and R.ItemTypeCode = T.ItemTypeCode and R.LineCode = T.LineCode and R.ItemCheckCode = T.ItemCheckCode and R.ProdDate = @ProdDate " & vbCrLf &
                "where T.FactoryCode = @FactoryCode and T.ItemTypeCode = @ItemTypeCode and T.LineCode = @LineCode and T.ItemCheckCode = @ItemCheckCode and R.ShiftCode = @ShiftCode and R.ProdDate = @ProdDate " & vbCrLf &
                "and T.ActiveStatus = 1 and F.ActiveStatus = 1  " & vbCrLf
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim SequenceList As New List(Of clsSequenceNo)
            Do While rd.Read
                Dim Shift As New clsSequenceNo
                Shift.SequenceNo = rd("SequenceNo")
                Shift.StartTime = rd("StartTime") & ""
                SequenceList.Add(Shift)
            Loop
            rd.Close()
            Return SequenceList
        End Using
    End Function
End Class