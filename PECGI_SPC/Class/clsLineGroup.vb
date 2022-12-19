Imports System.Data.SqlClient
Public Class clsLineGroup
    Public Property FactoryCode As String
    Public Property ProcessGroup As String
    Public Property LineGroup As String
    Public Property LineGroupName As String

End Class

Public Class clsLineGroupDB
    Public Shared Function GetList(UserID As String, FactoryCode As String, ProcessGroup As String) As List(Of clsLineGroup)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim cmd As New SqlCommand("sp_SPC_FillCombo", Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("ComboType", 1)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ProcessGroup", ProcessGroup)
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim LineGroupList As New List(Of clsLineGroup)
            Do While rd.Read
                Dim LineGroup As New clsLineGroup
                LineGroup.FactoryCode = rd("FactoryCode")
                LineGroup.ProcessGroup = rd("ProcessGroup")
                LineGroup.LineGroup = rd("LineGroup")
                LineGroup.LineGroupName = rd("ProcessGroupName")
                LineGroupList.Add(LineGroup)
            Loop
            rd.Close()
            Return LineGroupList
        End Using
    End Function
End Class