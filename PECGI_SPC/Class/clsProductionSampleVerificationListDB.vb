Imports System.Data.SqlClient
Public Class clsProductionSampleVerificationListDB
    Public Shared Function FillCombo(ByVal Status As String, data As clsProductionSampleVerificationList) As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_ProdSampleVerificationList_FillCombo"
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

    Public Shared Function LoadGrid(UserID As String, data As clsProductionSampleVerificationList) As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_ProdSampleVerificationList_Grid"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ProdDateFrom", If(data.ProdDateFrom, ""))
                cmd.Parameters.AddWithValue("ProdDateTo", If(data.ProdDateTo, ""))
                cmd.Parameters.AddWithValue("MKVerification", If(data.MKVerification, ""))
                cmd.Parameters.AddWithValue("QCVerification", If(data.QCVerification, ""))
                cmd.Parameters.AddWithValue("User", If(UserID, ""))

                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error! " & ex.Message)
        End Try
    End Function
End Class
