Imports System.Data.SqlClient

Public Class clsSPCResultDetail
    Public Property SPCResultID As Integer
    Public Property SequenceNo As Integer
    Public Property Value1 As Double
    Public Property Value2 As Double
    Public Property Value As Double
    Public Property Remark As String
    Public Property DeleteStatus As String
    Public Property RegisterUser As String

End Class

Public Class clsSPCResultDetailDB
    Public Shared Function Insert(Detail As clsSPCResultDetail)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPCResultDetail_Ins"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("SPCResultID", Detail.SPCResultID)
            cmd.Parameters.AddWithValue("SequenceNo", Detail.SequenceNo)
            If Detail.Value1 <> 0 Then
                cmd.Parameters.AddWithValue("Value1", Detail.Value1)
            End If
            If Detail.Value2 <> 0 Then
                cmd.Parameters.AddWithValue("Value2", Detail.Value2)
            End If
            cmd.Parameters.AddWithValue("Value", Detail.Value)
            cmd.Parameters.AddWithValue("Remark", Detail.Remark)
            cmd.Parameters.AddWithValue("DeleteStatus", Detail.DeleteStatus)
            cmd.Parameters.AddWithValue("RegisterUser", Detail.RegisterUser)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function GetTable(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer, VerifiedOnly As Integer) As DataTable
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPCResultDetail"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            cmd.Parameters.AddWithValue("ShiftCode", Shift)
            cmd.Parameters.AddWithValue("SequenceNo", Sequence)
            cmd.Parameters.AddWithValue("VerifiedOnly", VerifiedOnly)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetSeqNo(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPCResultDetail_GetSeqNo"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            cmd.Parameters.AddWithValue("ShiftCode", Shift)
            cmd.Parameters.AddWithValue("SequenceNo", Sequence)

            Return cmd.ExecuteScalar
        End Using
    End Function

    Public Shared Function GetTableXR(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, VerifiedOnly As Integer) As DataTable
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_XRTable"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            cmd.Parameters.AddWithValue("VerifiedOnly", VerifiedOnly)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetLastR(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, SequenceNo As Integer, VerifiedOnly As Integer) As DataTable
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_spc_GetLastR"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)
            cmd.Parameters.AddWithValue("VerifiedOnly", VerifiedOnly)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetSampleByPeriod(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String, VerifiedOnly As Integer, Optional ShowVerifier As Boolean = False, Optional CompleteOnly As Boolean = False) As DataSet
        Dim ConStr As String = Sconn.Stringkoneksi
        Using Cn As New SqlConnection(ConStr)
            Cn.Open()
            Dim q As String = "sp_SPC_SampleControl"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            If ProdDate <> "" Then
                cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            End If
            cmd.Parameters.AddWithValue("ProdDate2", CDate(ProdDate2))
            cmd.Parameters.AddWithValue("VerifiedOnly", VerifiedOnly)
            If ShowVerifier Then
                cmd.Parameters.AddWithValue("ShowVerifier", 1)
            End If
            If CompleteOnly Then
                cmd.Parameters.AddWithValue("CompleteStatus", 1)
            End If
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds
        End Using
    End Function
End Class