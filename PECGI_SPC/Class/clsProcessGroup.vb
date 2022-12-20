Imports System.Data.SqlClient

Public Class clsProcessGroup
    Public Property FactoryCode As String
    Public Property ProcessGroup As String
    Public Property ProcessGroupName As String
End Class

Public Class clsProcessGroupDB
    Public Shared Function GetList(UserID As String, FactoryCode As String, Optional ShowAll As Boolean = False) As List(Of clsProcessGroup)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim cmd As New SqlCommand("sp_SPC_FillCombo", Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("ComboType", 0)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            If ShowAll Then
                cmd.Parameters.AddWithValue("All", 1)
            End If
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim ProcessGroupList As New List(Of clsProcessGroup)
            Do While rd.Read
                Dim ProcessGroup As New clsProcessGroup
                ProcessGroup.FactoryCode = rd("FactoryCode")
                ProcessGroup.ProcessGroup = rd("ProcessGroup")
                ProcessGroup.ProcessGroupName = rd("ProcessGroupName")
                ProcessGroupList.Add(ProcessGroup)
            Loop
            rd.Close()
            Return ProcessGroupList
        End Using
    End Function
End Class