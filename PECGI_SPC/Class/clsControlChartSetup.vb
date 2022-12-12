Imports System.Data.SqlClient

Public Class clsControlChartSetup
    Public Property Factory As String
    Public Property Machine As String
    Public Property Type As String
    Public Property Period As String
    Public Property ItemType As String
    Public Property ItemCheck As String
    Public Property StartTime As String : Public Property StartTimeOld As String
    Public Property EndTime As String : Public Property EndTimeOld As String
    Public Property SpecUSL As String : Public Property SpecUSLOld As String
    Public Property SpecLSL As String : Public Property SpecLSLOld As String
    'Public Property XBarCL As String : Public Property XBarCLOld As String
    Public Property XBarLCL As String : Public Property XBarLCLOld As String
    Public Property XBarUCL As String : Public Property XBarUCLOld As String
    Public Property CPCL As String : Public Property CPCLOld As String
    Public Property CPLCL As String : Public Property CPLCLOld As String
    Public Property CPUCL As String : Public Property CPUCLOld As String
    Public Property RCL As String : Public Property RCLOld As String
    Public Property RLCL As String : Public Property RLCLOld As String
    Public Property RUCL As String : Public Property RUCLOld As String
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

    Public Shared Sub InsertUpdate(cls As clsControlChartSetup, Type As String)
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
                'cmd.Parameters.AddWithValue("XBarCL", CDbl(cls.XBarCL))
                cmd.Parameters.AddWithValue("XBarUCL", CDbl(cls.XBarUCL))
                cmd.Parameters.AddWithValue("XBarLCL", CDbl(cls.XBarLCL))
                cmd.Parameters.AddWithValue("CPCL", CDbl(cls.CPCL))
                cmd.Parameters.AddWithValue("CPUCL", CDbl(cls.CPUCL))
                cmd.Parameters.AddWithValue("CPLCL", CDbl(cls.CPLCL))
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
    End Sub

    Public Shared Sub Email(cls As clsControlChartSetup, Type As String)
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_ChartSetup_Email"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", cls.Factory)
                cmd.Parameters.AddWithValue("ItemType", cls.ItemType)
                cmd.Parameters.AddWithValue("Line", cls.Machine)
                cmd.Parameters.AddWithValue("ItemCheck", cls.ItemCheck)
                cmd.Parameters.AddWithValue("Start", cls.StartTime) : cmd.Parameters.AddWithValue("StartOld", cls.StartTimeOld)
                cmd.Parameters.AddWithValue("End", cls.EndTime) : cmd.Parameters.AddWithValue("EndOld", cls.EndTimeOld)
                cmd.Parameters.AddWithValue("SpecUSL", CDbl(cls.SpecUSL)) : cmd.Parameters.AddWithValue("SpecUSLOld", CDbl(cls.SpecUSLOld))
                cmd.Parameters.AddWithValue("SpecLSL", CDbl(cls.SpecLSL)) : cmd.Parameters.AddWithValue("SpecLSLOld", CDbl(cls.SpecLSLOld))
                'cmd.Parameters.AddWithValue("XBarCL", CDbl(cls.XBarCL)) : cmd.Parameters.AddWithValue("XBarCLOld", CDbl(cls.XBarCLOld))
                cmd.Parameters.AddWithValue("XBarUCL", CDbl(cls.XBarUCL)) : cmd.Parameters.AddWithValue("XBarUCLOld", CDbl(cls.XBarUCLOld))
                cmd.Parameters.AddWithValue("XBarLCL", CDbl(cls.XBarLCL)) : cmd.Parameters.AddWithValue("XBarLCLOld", CDbl(cls.XBarLCLOld))
                cmd.Parameters.AddWithValue("CPCL", CDbl(cls.CPCL)) : cmd.Parameters.AddWithValue("CPCLOld", CDbl(cls.CPCLOld))
                cmd.Parameters.AddWithValue("CPUCL", CDbl(cls.CPUCL)) : cmd.Parameters.AddWithValue("CPUCLOld", CDbl(cls.CPUCLOld))
                cmd.Parameters.AddWithValue("CPLCL", CDbl(cls.CPLCL)) : cmd.Parameters.AddWithValue("CPLCLOld", CDbl(cls.CPLCLOld))
                cmd.Parameters.AddWithValue("RCL", CDbl(cls.RCL)) : cmd.Parameters.AddWithValue("RCLOld", CDbl(cls.RCLOld))
                cmd.Parameters.AddWithValue("RLCL", CDbl(cls.RLCL)) : cmd.Parameters.AddWithValue("RLCLOld", CDbl(cls.RLCLOld))
                cmd.Parameters.AddWithValue("RUCL", CDbl(cls.RUCL)) : cmd.Parameters.AddWithValue("RUCLOld", CDbl(cls.RUCLOld))
                cmd.Parameters.AddWithValue("User", cls.User)
                cmd.Parameters.AddWithValue("To", clsSPCAlertDashboardDB.GetUserLine(cls.Factory, cls.Machine, "1"))
                cmd.Parameters.AddWithValue("Type", Type)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Sub Check(cls As clsControlChartSetup, Optional ByRef pErr As String = "")
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
    End Sub

    Public Shared Sub Delete(cls As clsControlChartSetup)
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
    End Sub
End Class