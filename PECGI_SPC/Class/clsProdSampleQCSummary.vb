Imports System.Data.SqlClient

Public Class clsProdSampleQCSummary
    Public Property FactoryCode As String
    Public Property ItemTypeCode As String
    Public Property ItemCheckCode As String
    Public Property MachineCode As String
    Public Property Frequency As String
    Public Property Sequence As String
    Public Property Period As String
    Public Property UserID As String
End Class

Public Class clsProdSampleQCSummaryDB
    Public Shared Function FillCombo(type As String, Optional param As String = "", Optional param2 As String = "", Optional param3 As String = "", Optional param4 As String = "", Optional param5 As String = "") As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_ProdQualitySummary_FillCombo"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("Type", type)
            If param <> "" Then cmd.Parameters.AddWithValue("Param", param)
            If param2 <> "" Then cmd.Parameters.AddWithValue("Param2", param2)
            If param3 <> "" Then cmd.Parameters.AddWithValue("Param3", param3)
            If param4 <> "" Then cmd.Parameters.AddWithValue("Param4", param4)
            If param4 <> "" Then cmd.Parameters.AddWithValue("Param5", param5)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetList(cls As clsProdSampleQCSummary) As DataSet
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_ProdQualitySummary_Sel"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("Factory", cls.FactoryCode)
            cmd.Parameters.AddWithValue("ItemType", cls.ItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", cls.MachineCode)
            cmd.Parameters.AddWithValue("Frequency", cls.Frequency)
            cmd.Parameters.AddWithValue("Sequence", cls.Sequence)
            cmd.Parameters.AddWithValue("Period", cls.Period)
            cmd.Parameters.AddWithValue("UserID", cls.UserID)

            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet

            da.Fill(ds)
            Return ds
        End Using
    End Function

    Public Shared Function GetListDetail(cls As clsProdSampleQCSummary) As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_ProdQualitySummary_Sel_Detail"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("Factory", cls.FactoryCode)
            cmd.Parameters.AddWithValue("ItemType", cls.ItemTypeCode)
            cmd.Parameters.AddWithValue("ItemCheck", cls.ItemCheckCode)
            cmd.Parameters.AddWithValue("LineCode", cls.MachineCode)
            cmd.Parameters.AddWithValue("Frequency", cls.Frequency)
            cmd.Parameters.AddWithValue("Sequence", cls.Sequence)
            cmd.Parameters.AddWithValue("Period", cls.Period)
            cmd.Parameters.AddWithValue("UserID", cls.UserID)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        End Using
    End Function
End Class