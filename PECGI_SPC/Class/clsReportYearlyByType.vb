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

    Public Shared Function ChartLineDetail(data As clsReportYearlyByType) As List(Of clsReportYearlyByType_ChartLineDetail)
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_spc_FTA_ReportYearlyByType_ChartLineDetail"
                Dim cmd As New SqlCommand(sql, conn)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("User", If(data.UserID, ""))
                cmd.Parameters.AddWithValue("LineGroup", If(data.LineGroup, ""))
                cmd.Parameters.AddWithValue("ProcessCode", If(data.ProcessCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("Periode", If(data.Periode, ""))
                cmd.Parameters.AddWithValue("QtyFTA", If(data.QtyFTA, ""))
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    Dim ChartLine As New List(Of clsReportYearlyByType_ChartLineDetail)()
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ChartLine.Add(
                        New clsReportYearlyByType_ChartLineDetail() With {
                        .LABELNAME = dt.Rows(i)("LABELNAME"),
                        .DATAVAL = dt.Rows(i)("QtyFTA_Detail"),
                        .DATAVAL2 = dt.Rows(i)("Percentage"),
                        .AXISVAL = Trim(dt.Rows(i)("No")),
                        .AXISVAL2 = Trim(dt.Rows(i)("NoofLine")),
                        .AXISNAME = Trim(dt.Rows(i)("ItemCheckName"))
                        })
                    Next

                    Return ChartLine
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function


End Class

Public Class clsReportYearlyByType_ChartLineDetail
    Public Property LABELNAME As String
    Public Property DATAVAL As String
    Public Property DATAVAL2 As String
    Public Property AXISVAL As String
    Public Property AXISVAL2 As String
    Public Property AXISNAME As String
End Class

Public Class clsReportYearlyByType_ChartLineSetting
    Public Property data As String
    Public Property xaxisTicks As String
    Public Property Min As String
    Public Property Max As String
    Public Property Median As String
    Public Property Length As String
End Class