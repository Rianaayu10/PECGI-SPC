Imports System.Data.SqlClient

Public Class clsSPCNotification

    Public Shared Function GetData(constr As String, UserID As String, Type As String) As DataTable
        Try
            Dim dt As New DataTable
            Dim sql As String
            Using con As New SqlConnection(constr)
                con.Open()
                sql = "sp_SPCNotification_GET"
                Dim cmd As New SqlCommand(sql, con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@UserID", UserID)
                cmd.Parameters.AddWithValue("@Type", Type)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
