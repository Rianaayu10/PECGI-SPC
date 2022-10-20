Imports System.Data.SqlClient

Public Class clsSchedulerLog
    Public Property EmailDate As String
    Public Property Seq As Integer
    Public Property EmailTime As String
End Class

Public Class clsSchedulerLogDB
    Public Shared Function GetList(pEmailDate As String) As List(Of clsSchedulerLog)
        Dim cf As New clsConfig
        Using Cn As New SqlConnection(cf.m_ConnectionString)
            Cn.Open()
            Dim q As String = "select * from SchedulerLog where convert(char(8), EmailDate, 112) = '" & pEmailDate & "' "
            Dim cmd As New SqlCommand(q, Cn)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim ScList As New List(Of clsSchedulerLog)
            For i = 0 To dt.Rows.Count - 1
                Dim Sc As New clsSchedulerLog
                Sc.EmailDate = dt.Rows(i)("EmailDate")
                Sc.Seq = dt.Rows(i)("Seq")
                Sc.EmailTime = dt.Rows(i)("EmailTime")
                ScList.Add(Sc)
            Next
            Return ScList
        End Using
    End Function

    Public Shared Function GetData(pEmailDate As String, Seq As Integer) As clsSchedulerLog
        Dim cf As New clsConfig
        Using Cn As New SqlConnection(cf.m_ConnectionString)
            Cn.Open()
            Dim q As String = "select * from SchedulerLog where convert(char(8), EmailDate, 112) = '" & pEmailDate & "' and Seq = " & Seq
            Dim cmd As New SqlCommand(q, Cn)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count = 0 Then
                Return Nothing
            End If
            Dim Sc As New clsSchedulerLog
            Sc.EmailDate = dt.Rows(0)("EmailDate")
            Sc.Seq = dt.Rows(0)("Seq")
            Sc.EmailTime = dt.Rows(0)("EmailTime")
            Return Sc
        End Using
    End Function

    Public Shared Function Insert(Sc As clsSchedulerLog) As Integer
        Dim cf As New clsConfig
        Using Cn As New SqlConnection(cf.m_ConnectionString)
            Cn.Open()
            Dim q As String = "if not exists (select Seq from SchedulerLog where EmailDate = @EmailDate and Seq = @Seq) " & vbCrLf & _
                "Insert into SchedulerLog (EmailDate, Seq, EmailTime) " & vbCrLf & _
                "values (@EmailDate, @Seq, @EmailTime)"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("EmailDate", Sc.EmailDate)
            cmd.Parameters.AddWithValue("Seq", Sc.Seq)
            cmd.Parameters.AddWithValue("EmailTime", Sc.EmailTime)
            Dim i As Integer = cmd.ExecuteNonQuery
            Return i
        End Using
    End Function


End Class
