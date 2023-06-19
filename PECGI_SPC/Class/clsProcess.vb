Imports System.Data.SqlClient

Public Class clsProcess
    Public Property FactoryCode As String
    Public Property ProcessCode As String
    Public Property ProcessName As String
    Public Property LineGroup As String
    Public Property ProcessGroup As String
End Class

Public Class clsProcessDB
    Public Shared Function GetList(UserID As String, FactoryCode As String, ProcessGroup As String, LineGroup As String, Optional ShowAll As Boolean = False) As List(Of clsProcess)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim cmd As New SqlCommand("sp_SPC_FillCombo", Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("ComboType", 2)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ProcessGroup", ProcessGroup)
            cmd.Parameters.AddWithValue("LineGroup", LineGroup)
            If ShowAll Then
                cmd.Parameters.AddWithValue("All", 1)
            End If
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim ProcessList As New List(Of clsProcess)
            Do While rd.Read
                Dim Process As New clsProcess
                Process.FactoryCode = rd("FactoryCode")
                Process.ProcessCode = rd("ProcessCode")
                Process.ProcessName = rd("ProcessName")
                Process.LineGroup = rd("LineGroup")
                Process.ProcessGroup = rd("ProcessGroup")
                ProcessList.Add(Process)
            Loop
            rd.Close()
            Return ProcessList
        End Using
    End Function
    Public Shared Function GetListForItemCheckByType(UserID As String, FactoryCode As String, ProcessGroup As String, LineGroup As String, Optional ShowAll As Boolean = False) As List(Of clsProcess)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim cmd As New SqlCommand("sp_SPC_FillCombo", Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("ComboType", 4)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ProcessGroup", ProcessGroup)
            cmd.Parameters.AddWithValue("LineGroup", LineGroup)
            If ShowAll Then
                cmd.Parameters.AddWithValue("All", 1)
            End If
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim ProcessList As New List(Of clsProcess)
            Do While rd.Read
                Dim Process As New clsProcess
                Process.FactoryCode = rd("FactoryCode")
                Process.ProcessCode = rd("ProcessCode")
                Process.ProcessName = rd("ProcessName")
                Process.LineGroup = rd("LineGroup")
                Process.ProcessGroup = rd("ProcessGroup")
                ProcessList.Add(Process)
            Loop
            rd.Close()
            Return ProcessList
        End Using
    End Function
End Class