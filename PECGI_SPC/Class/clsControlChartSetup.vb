Imports System.Data.SqlClient

Public Class clsControlChartSetup
    Public Property Factory As String
    Public Property Machine As String
    Public Property Type As String
    Public Property Period As String
    Public Property ItemType As String
    Public Property ItemCheck As String
    Public Property StartTime As String
    Public Property StartTimeOld As String
    Public Property EndTime As String
    Public Property EndTimeOld As String
    Public Property SpecUSL As String
    Public Property SpecLSL As String
    Public Property XBarCL As String
    Public Property XBarLCL As String
    Public Property XBarUCL As String
    Public Property RCL As String
    Public Property RLCL As String
    Public Property RUCL As String
    Public Property User As String
End Class

Public Class clsControlChartSetupDB
    Public Shared Function FillCombo(type As String, Optional param As String = "", Optional param2 As String = "", Optional param3 As String = "") As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_ChartSetup_FillCombo"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("Type", type)
            If param <> "" Then cmd.Parameters.AddWithValue("Param", param)
            If param2 <> "" Then cmd.Parameters.AddWithValue("Param2", param2)
            If param3 <> "" Then cmd.Parameters.AddWithValue("Param3", param3)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetList(cls As clsControlChartSetup) As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_ChartSetup_Sel"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("Factory", cls.Factory)
            cmd.Parameters.AddWithValue("Machine", cls.Machine)
            cmd.Parameters.AddWithValue("Type", cls.Type)
            cmd.Parameters.AddWithValue("Period", cls.Period)
            cmd.CommandType = CommandType.StoredProcedure

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function InsertUpdate(cls As clsControlChartSetup, Type As String) As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_ChartSetup_InsUpd"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", cls.Factory)
                cmd.Parameters.AddWithValue("ItemType", cls.ItemType)
                cmd.Parameters.AddWithValue("Line", cls.Machine)
                cmd.Parameters.AddWithValue("ItemCheck", cls.ItemCheck)
                cmd.Parameters.AddWithValue("Start", cls.StartTime)
                cmd.Parameters.AddWithValue("StartOld", cls.StartTimeOld)
                cmd.Parameters.AddWithValue("End", cls.EndTime)
                cmd.Parameters.AddWithValue("EndOld", cls.EndTimeOld)
                cmd.Parameters.AddWithValue("SpecUSL", CDbl(cls.SpecUSL))
                cmd.Parameters.AddWithValue("SpecLSL", CDbl(cls.SpecLSL))
                cmd.Parameters.AddWithValue("XBarCL", CDbl(cls.XBarCL))
                cmd.Parameters.AddWithValue("XBarUCL", CDbl(cls.XBarUCL))
                cmd.Parameters.AddWithValue("XBarLCL", CDbl(cls.XBarLCL))
                cmd.Parameters.AddWithValue("RCL", CDbl(cls.RCL))
                cmd.Parameters.AddWithValue("RLCL", CDbl(cls.RLCL))
                cmd.Parameters.AddWithValue("RUCL", CDbl(cls.RUCL))
                cmd.Parameters.AddWithValue("User", cls.User)
                cmd.Parameters.AddWithValue("Type", Type)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function Check(cls As clsControlChartSetup, Optional ByRef pErr As String = "") As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_ChartSetup_Check"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", cls.Factory)
                cmd.Parameters.AddWithValue("ItemType", cls.ItemType)
                cmd.Parameters.AddWithValue("Line", cls.Machine)
                cmd.Parameters.AddWithValue("ItemCheck", cls.ItemCheck)
                cmd.Parameters.AddWithValue("Start", cls.StartTime)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            pErr = ex.Message
        End Try
    End Function

    Public Shared Function Delete(cls As clsControlChartSetup) As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_ChartSetup_Del"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", cls.Factory)
                cmd.Parameters.AddWithValue("ItemType", cls.ItemType)
                cmd.Parameters.AddWithValue("Line", cls.Machine)
                cmd.Parameters.AddWithValue("ItemCheck", cls.ItemCheck)
                cmd.Parameters.AddWithValue("Start", cls.StartTime)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class