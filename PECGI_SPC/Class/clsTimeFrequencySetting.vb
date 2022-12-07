Imports System.Data.SqlClient

Public Class clsTimeFrequencySetting
    Public Property FrequencyCode As String
    Public Property FrequencyDesc As String
    Public Property Nomor As String
    Public Property Shift As String
    Public Property StartTime As String
    Public Property EndTime As String
    Public Property VerifTime As String
    Public Property User As String
End Class

Public Class clsTimeFrequencySettingDB
    Public Shared Function FillCombo(Type As String) As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_MS_FrequencySetting_FillCombo"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("Type", Type)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetList(Freq As String) As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_MS_FrequencySetting_Sel"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("Frequency", Freq)
            cmd.CommandType = CommandType.StoredProcedure

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function InsertUpdate(cls As clsTimeFrequencySetting, Type As String) As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_MS_FrequencySetting_InsUpd"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Frequency", cls.FrequencyCode)
                cmd.Parameters.AddWithValue("Sequence", CInt(cls.Nomor))
                cmd.Parameters.AddWithValue("Shift", cls.Shift)
                cmd.Parameters.AddWithValue("Start", cls.StartTime)
                cmd.Parameters.AddWithValue("End", cls.EndTime)
                cmd.Parameters.AddWithValue("Verif", cls.VerifTime)
                cmd.Parameters.AddWithValue("User", cls.User)
                cmd.Parameters.AddWithValue("Type", Type)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function Delete(cls As clsTimeFrequencySetting) As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_MS_FrequencySetting_Del"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Frequency", cls.FrequencyCode)
                cmd.Parameters.AddWithValue("Sequence", cls.Nomor)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function Check(cls As clsTimeFrequencySetting, Optional ByRef pErr As String = "") As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_MS_FrequencySetting_Check"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Frequency", cls.FrequencyCode)
                cmd.Parameters.AddWithValue("Shift", cls.Shift)
                cmd.Parameters.AddWithValue("Start", cls.StartTime)
                cmd.Parameters.AddWithValue("End", cls.EndTime)
                cmd.Parameters.AddWithValue("@SequenceNo", CInt(cls.Nomor))

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            pErr = ex.Message
        End Try
    End Function
End Class
