Imports System.Data.SqlClient

Public Class clsReportYearlyByType
    Public Property FactoryCode As String
    Public Property ProcessGroup As String
    Public Property LineGroup As String
    Public Property LineGroupName As String
    Public Property ProcessCode As String
    Public Property LineCode As String
    Public Property LineName As String
    Public Property ItemType As String
    Public Property UserID As String
    Public Property ProdDateFrom As String
    Public Property ProdDateTo As String
    Public Property Periode As String
    Public Property QtyFTA As String
    Public Property QtyFTA_Detail As String
    Public Property QtyFTA_DetailPercentage As String
    Public Property val As Object

    Public Property V1 As String
    Public Property V2 As String
    Public Property V3 As String
    Public Property V4 As String
    Public Property V5 As String
    Public Property V6 As String
    Public Property V7 As String
    Public Property V8 As String
    Public Property V9 As String
    Public Property V10 As String
    Public Property V11 As String
    Public Property V12 As String


    Public Shared Function FillCombo(ComboType As String, data As clsFillCombo) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String = "sp_spc_FTA_ReportYearlyByType_FillCombo"
                Dim Cmd As New SqlCommand(sql, cn)
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.AddWithValue("ComboType", If(ComboType, ""))
                Cmd.Parameters.AddWithValue("User", If(data.UserID, ""))
                Cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                Cmd.Parameters.AddWithValue("ProcessGroup", If(data.ProcessGroup, ""))
                Cmd.Parameters.AddWithValue("LineGroup", If(data.LineGroup, ""))
                Cmd.Parameters.AddWithValue("ProcessCode", If(data.ProcessCode, ""))
                Cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                Cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType, ""))
                Dim da As New SqlDataAdapter(Cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function LoadData(data As clsReportYearlyByType) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String = "sp_spc_FTA_ReportYearlyByType_LoadData"
                Dim Cmd As New SqlCommand(sql, cn)
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.AddWithValue("User", If(data.UserID, ""))
                Cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                Cmd.Parameters.AddWithValue("ProcessGroup", If(data.ProcessGroup, ""))
                Cmd.Parameters.AddWithValue("LineGroup", If(data.LineGroup, ""))
                Cmd.Parameters.AddWithValue("ProcessCode", If(data.ProcessCode, ""))
                Cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                Cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType, ""))
                Cmd.Parameters.AddWithValue("ProdDate_From", If(data.ProdDateFrom, ""))
                Cmd.Parameters.AddWithValue("ProdDate_To", If(data.ProdDateTo, ""))
                Dim da As New SqlDataAdapter(Cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function LoadDetail(data As clsReportYearlyByType) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String = "sp_spc_FTA_ReportYearlyByType_LoadDetail"
                Dim Cmd As New SqlCommand(sql, cn)
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.AddWithValue("User", If(data.UserID, ""))
                Cmd.Parameters.AddWithValue("LineGroup", If(data.LineGroup, ""))
                Cmd.Parameters.AddWithValue("ProcessCode", If(data.ProcessCode, ""))
                Cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                Cmd.Parameters.AddWithValue("Periode", If(data.Periode, ""))
                Cmd.Parameters.AddWithValue("QtyFTA", If(data.QtyFTA, ""))
                Dim da As New SqlDataAdapter(Cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function ChartLineGroup(data As clsReportYearlyByType) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String = "sp_spc_FTA_ReportYearlyByType_ChartLineGroup"
                Dim Cmd As New SqlCommand(sql, cn)
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.AddWithValue("User", If(data.UserID, ""))
                Cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                Cmd.Parameters.AddWithValue("ProcessGroup", If(data.ProcessGroup, ""))
                Cmd.Parameters.AddWithValue("LineGroup", If(data.LineGroup, ""))
                Cmd.Parameters.AddWithValue("ProcessCode", If(data.ProcessCode, ""))
                Cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                Cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType, ""))
                Cmd.Parameters.AddWithValue("ProdDate_From", If(data.ProdDateFrom, ""))
                Cmd.Parameters.AddWithValue("ProdDate_To", If(data.ProdDateTo, ""))
                Dim da As New SqlDataAdapter(Cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function


End Class
