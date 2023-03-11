Imports System.Data.SqlClient

Public Class clsFTACorrectiveAction

End Class

Public Class clsFTACorrectiveActionDB
    Public Shared Function GetTable(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer) As DataTable
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_spc_FTACorrectiveAction"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            cmd.Parameters.AddWithValue("ShiftCode", Shift)
            cmd.Parameters.AddWithValue("SequenceNo", Sequence)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetFTAMaster(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String) As DataTable
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select FTAID, Factor1, Factor2, Factor3, Factor4, CounterMeasure, CheckItem, 'View' Action, 'View' IK from spc_MS_FTA where ActiveStatus = 1 order by CheckOrder"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function
End Class

Public Class clsFTAInquiryDB
    Public Shared Function GetTable(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String, Optional MKVerification As Integer = 0, Optional QCVerification As Integer = 0) As DataTable
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_FTAInquiry"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            cmd.Parameters.AddWithValue("ProdDate2", CDate(ProdDate2))
            cmd.Parameters.AddWithValue("MKVerification", MKVerification)
            cmd.Parameters.AddWithValue("QCVerification", QCVerification)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function
End Class
