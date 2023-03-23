Imports System.Data.SqlClient
Public Class clsFTADashboardMonitoring
    Public Property Station1 As String
    Public Property Station2 As String
    Public Property Station3 As String
    Public Property Station4 As String
    Public Property Station5 As String
    Public Property Station6 As String
    Public Property Station7 As String
    Public Property Station8 As String
    Public Property Station9 As String
    Public Property Station10 As String
    Public Property Station11 As String
    Public Property Station12 As String
    Public Property Station13 As String
    Public Property Station14 As String
    Public Property Station15 As String
    Public Property Station16 As String
    Public Property DelayInput As String
    Public Property DelayNG As String

    Public Shared Function LoadData(UserID As String) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                Dim sql As String = "sp_spc_FTA_DashboardMonitoring"
                Dim Cmd As New SqlCommand(sql, cn)
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.AddWithValue("UserID", UserID)
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
