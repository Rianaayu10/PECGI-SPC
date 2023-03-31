Imports System.Data.SqlClient

Public Class clsFTAResult
    Public Property No As Integer
    Public Property Description As String
    Public Property Action As String
    Public Property OK As Boolean
    Public Property NG As Boolean
    Public Property NoCheck As Boolean
    Public Property ViewIK As String
    Public Property LastUser As String
    Public Property LastUpdate As String
    Public Property FTAID As String
    Public Property SPCResultID As Integer
    Public Property Remark As String
    Public Property ProdDate As String
    Public Property MKVerificationStatus As String
    Public Property MKVerificationUser As String
    Public Property MKVerificationDate As String
    Public Property QCVerificationStatus As String
    Public Property QCVerificationUser As String
    Public Property QCVerificationDate As String
End Class

Public Class clsFTAResultDB
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

    Public Shared Function GetList(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer) As List(Of clsFTAResult)
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
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim FTAList As New List(Of clsFTAResult)
            Do While rd.Read
                Dim FTA As New clsFTAResult
                FTA.No = rd("No")
                FTA.Description = rd("Description") & ""
                FTA.Action = rd("Action") & ""
                FTA.OK = rd("OK") = 1
                FTA.NG = rd("NG") = 1
                FTA.NoCheck = rd("NoCheck") = 1
                FTA.ViewIK = rd("ViewIK") & ""
                FTA.LastUser = rd("LastUser") & ""
                FTA.LastUpdate = rd("LastUpdate") & ""
                FTA.FTAID = rd("FTAID")
                If Not IsDBNull(rd("SPCResultID")) Then
                    FTA.SPCResultID = rd("SPCResultID")
                End If
                FTA.Remark = rd("Remark") & ""
                FTA.ProdDate = rd("ProdDate") & ""
                FTA.MKVerificationStatus = rd("MKVerificationStatus") & ""
                FTA.MKVerificationDate = rd("MKVerificationDate") & ""
                FTA.MKVerificationUser = rd("MKVerificationUser") & ""
                FTA.QCVerificationStatus = rd("QCVerificationStatus") & ""
                FTA.QCVerificationDate = rd("QCVerificationDate") & ""
                FTA.QCVerificationUser = rd("QCVerificationUser") & ""

                FTAList.Add(FTA)
            Loop
            rd.Close()

            Return FTAList
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

    Public Shared Function GetFTAAction(FTAID As String, Optional UseCheckBox As Boolean = False) As DataTable
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String
            If UseCheckBox Then
                q = "select 0 [Select], * from spc_MS_FTAAction where FTAID = @FTAID order by ActionID "
            Else
                q = "select * from spc_MS_FTAAction where FTAID = @FTAID order by ActionID "
            End If
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FTAID", FTAID)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function Insert(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer, FTAResult As String, Remark As String, UserID As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_spc_FTAResult_Ins"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            cmd.Parameters.AddWithValue("ShiftCode", Shift)
            cmd.Parameters.AddWithValue("SequenceNo", Sequence)
            cmd.Parameters.AddWithValue("FTAResult", FTAResult)
            cmd.Parameters.AddWithValue("Remark", Remark)
            cmd.Parameters.AddWithValue("UserID", UserID)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function Verify(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer, UserID As String, JobPos As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_spc_FTACorrectiveAction_Verify"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            cmd.Parameters.AddWithValue("ShiftCode", Shift)
            cmd.Parameters.AddWithValue("SequenceNo", Sequence)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("JobPos", JobPos)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function

    Public Shared Function GetIK(FTAID As String) As Object
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Select IK from spc_MS_FTA where FTAID = @FTAID"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FTAID", FTAID)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count = 0 Then
                Return Nothing
            Else
                Return dt.Rows(0)("IK")
            End If
        End Using
    End Function

    Public Shared Function GetInquiry(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String, Optional MKVerification As Integer = 0, Optional QCVerification As Integer = 0) As DataTable
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

Public Class clsFTAResultDetail
    Public Property FAResultID As Integer
    Public Property SequenceNo As Integer

    Public Property FTAID As String
    Public Property FTAResult As String
    Public Property Remark As String
End Class

Public Class clsFTAResultDetailDB
    Public Shared Function Insert(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, Shift As String, Sequence As Integer, Remark As String,
                                  FTAID As String, DetailSeqNo As Integer, Action As String, FTAResult As String, UserID As String) As Integer
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_spc_FTAResultDetail_Ins"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", CDate(ProdDate))
            cmd.Parameters.AddWithValue("ShiftCode", Shift)
            cmd.Parameters.AddWithValue("SequenceNo", Sequence)
            cmd.Parameters.AddWithValue("Remark", Remark)
            cmd.Parameters.AddWithValue("FTAID", FTAID)
            cmd.Parameters.AddWithValue("DetailSeqNo", DetailSeqNo)
            cmd.Parameters.AddWithValue("Action", Action)
            cmd.Parameters.AddWithValue("FTAResult", FTAResult)
            cmd.Parameters.AddWithValue("UserID", UserID)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function
End Class