Imports System.Data.SqlClient

Public Class clsTroubleHistoryByLineComboBox
    Public Property CODE As String
    Public Property CODENAME As String
    Public Property FactoryCode As String
    Public Property LineCode As String
    Public Property ItemType_Code As String
    Public Property UserID As String
End Class

Public Class clsTroubleHistoryByLineChartSetting
    Public Property data As String
    Public Property xaxisTicks As String
    Public Property Min As String
    Public Property Max As String
    Public Property Median As String
    Public Property Length As String
End Class

Public Class clsTroubleHistoryByLineChartData
    Public Property LABELNAME As String
    Public Property DATAVAL As String
    Public Property DATAVAL2 As String
    Public Property AXISVAL As String
    Public Property AXISVAL2 As String
    Public Property AXISNAME As String
End Class

Public Class clsTroubleHistoryByLineTableData
    Public Property No As String
    Public Property Machine As String
    Public Property AbnormalFreq As String
    Public Property LineCode As String
End Class

Public Class clsTroubleHistoryByLineDB

    Public Shared Function FillCombo(ByVal Status As String, data As clsTroubleHistoryByLineComboBox) As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_TroubleHistoryByLine_FillCombo"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Status", Status)
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("UserID", If(data.UserID, ""))
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function
    Public Shared Function GetChartMachineProcess(ByVal Factory As String, ByVal Type As String, ByVal DateFrom As String, ByVal DateTo As String, ByVal UserID As String) As List(Of clsTroubleHistoryByLineChartData)
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_TroubleHistoryByLine_MachineProcess_GetListPercentage"
                Dim cmd As New SqlCommand(sql, conn)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", Factory)
                cmd.Parameters.AddWithValue("ItemType", Type)
                cmd.Parameters.AddWithValue("DateFrom", DateFrom)
                cmd.Parameters.AddWithValue("DateTo", DateTo)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    Dim ChartMachineProcess As New List(Of clsTroubleHistoryByLineChartData)()
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ChartMachineProcess.Add(
                            New clsTroubleHistoryByLineChartData() With {
                            .LABELNAME = dt.Rows(i)("LABELNAME"),
                            .DATAVAL = dt.Rows(i)("AbnormalFreq"),
                            .DATAVAL2 = dt.Rows(i)("Percentage"),
                            .AXISVAL = Trim(dt.Rows(i)("No")),
                            .AXISVAL2 = Trim(dt.Rows(i)("NoofLine")),
                            .AXISNAME = Trim(dt.Rows(i)("Machine"))
                            })
                    Next

                    Return ChartMachineProcess
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function

    Public Shared Function GetTableMachineProcess(ByVal Factory As String, ByVal Type As String, ByVal DateFrom As String, ByVal DateTo As String, ByVal UserID As String) As List(Of clsTroubleHistoryByLineTableData)
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_TroubleHistoryByLine_MachineProcess_GetList_Table"
                Dim cmd As New SqlCommand(sql, conn)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", Factory)
                cmd.Parameters.AddWithValue("ItemType", Type)
                cmd.Parameters.AddWithValue("DateFrom", DateFrom)
                cmd.Parameters.AddWithValue("DateTo", DateTo)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    Dim ChartMachineProcess As New List(Of clsTroubleHistoryByLineTableData)()
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ChartMachineProcess.Add(
                            New clsTroubleHistoryByLineTableData() With {
                            .No = dt.Rows(i)("No"),
                            .Machine = dt.Rows(i)("Machine"),
                            .AbnormalFreq = dt.Rows(i)("AbnormalFreq"),
                            .LineCode = Trim(dt.Rows(i)("LineCode"))
                            })
                    Next

                    'If ChartMachineProcess.Count < 8 Then
                    '    For j As Integer = ChartMachineProcess.Count - 1 To 6
                    '        ChartMachineProcess.Add(
                    '            New clsTroubleHistoryByLineTableData() With {
                    '            .No = "",
                    '            .Machine = "",
                    '            .AbnormalFreq = " ",
                    '            .LineCode = ""
                    '            })
                    '    Next
                    'End If

                    Return ChartMachineProcess
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function


    Public Shared Function GetChartItemCheck(ByVal Factory As String, ByVal Type As String, ByVal Line As String, ByVal DateFrom As String, ByVal DateTo As String, ByVal UserID As String) As List(Of clsTroubleHistoryByLineChartData)
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_TroubleHistoryByLine_ItemCheck_GetListPercentage"
                Dim cmd As New SqlCommand(sql, conn)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", Factory)
                cmd.Parameters.AddWithValue("ItemType", Type)
                cmd.Parameters.AddWithValue("Line", Line)
                cmd.Parameters.AddWithValue("DateFrom", DateFrom)
                cmd.Parameters.AddWithValue("DateTo", DateTo)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    Dim ChartMachineProcess As New List(Of clsTroubleHistoryByLineChartData)()
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ChartMachineProcess.Add(
                            New clsTroubleHistoryByLineChartData() With {
                            .LABELNAME = dt.Rows(i)("LABELNAME"),
                            .DATAVAL = dt.Rows(i)("AbnormalFreq"),
                            .DATAVAL2 = dt.Rows(i)("Percentage"),
                            .AXISVAL = Trim(dt.Rows(i)("No")),
                            .AXISVAL2 = Trim(dt.Rows(i)("NoofLine")),
                            .AXISNAME = Trim(dt.Rows(i)("Machine"))
                            })
                    Next

                    Return ChartMachineProcess
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function

    Public Shared Function GetTableItemCheck(ByVal Factory As String, ByVal Type As String, ByVal Line As String, ByVal DateFrom As String, ByVal DateTo As String, ByVal UserID As String) As List(Of clsTroubleHistoryByLineTableData)
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_TroubleHistoryByLine_ItemCheck_GetList_Table"
                Dim cmd As New SqlCommand(sql, conn)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Factory", Factory)
                cmd.Parameters.AddWithValue("ItemType", Type)
                cmd.Parameters.AddWithValue("Line", Line)
                cmd.Parameters.AddWithValue("DateFrom", DateFrom)
                cmd.Parameters.AddWithValue("DateTo", DateTo)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    Dim ChartMachineProcess As New List(Of clsTroubleHistoryByLineTableData)()
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ChartMachineProcess.Add(
                            New clsTroubleHistoryByLineTableData() With {
                            .No = dt.Rows(i)("No"),
                            .Machine = dt.Rows(i)("Machine"),
                            .AbnormalFreq = dt.Rows(i)("AbnormalFreq"),
                            .LineCode = Trim(dt.Rows(i)("LineCode"))
                            })
                    Next

                    'If ChartMachineProcess.Count < 8 Then
                    '    For j As Integer = ChartMachineProcess.Count - 1 To 6
                    '        ChartMachineProcess.Add(
                    '            New clsTroubleHistoryByLineTableData() With {
                    '            .No = "1",
                    '            .Machine = "-",
                    '            .AbnormalFreq = "2",
                    '            .LineCode = "0"
                    '            })
                    '    Next
                    'End If

                    Return ChartMachineProcess
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function
End Class