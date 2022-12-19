Imports System.Data.SqlClient

Public Class clsFactory
    Public Property FactoryCode As String
    Public Property FactoryName As String
End Class

Public Class clsFactoryDB
    Public Shared Function GetList() As List(Of clsFactory)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select * from MS_Factory"
            Dim cmd As New SqlCommand(q, Cn)
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim FactoryList As New List(Of clsFactory)
            Do While rd.Read
                Dim Factory As New clsFactory
                Factory.FactoryCode = rd("FactoryCode") & ""
                Factory.FactoryName = rd("FactoryName") & ""
                FactoryList.Add(Factory)
            Loop
            rd.Close()
            Return FactoryList
        End Using
    End Function
End Class
