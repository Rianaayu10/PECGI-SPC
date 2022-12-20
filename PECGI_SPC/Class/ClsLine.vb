Imports System.Data.SqlClient

Public Class ClsLine
    Public Property FactoryCode As String
    Public Property ProcessCode As String
    Public Property LineCode As String
    Public Property ProcessGroup As String
    Public Property LineGroup As String
    Public Property VisualProcessCode As String
    Public Property LineName As String
    Public Property AllowShow As Integer
    Public Property AllowUpdate As Integer
    Public Property AllowVerify As Integer
End Class


Public Class ClsLineDB
    Public Shared Function GetData(FactoryCode As String, LineCode As String) As ClsLine
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select L.FactoryCode, L.ProcessCode, L.LineCode, P.ProcessGroup, G.LineGroup, L.LineName " & vbCrLf &
                "from MS_Line L left join MS_Process P on L.FactoryCode = P.FactoryCode and L.ProcessCode = P.ProcessCode " & vbCrLf &
                "left join MS_LineGroup G on P.FactoryCode = G.FactoryCode and P.ProcessGroup = G.ProcessGroup and P.LineGroup = G.LineGroup " & vbCrLf &
                "where L.FactoryCode = @FactoryCode And L.LineCode = @LineCode "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count = 0 Then
                Return Nothing
            Else
                With dt.Rows(0)
                    Dim Line As New ClsLine
                    Line.LineCode = .Item("LineCode")
                    Line.FactoryCode = .Item("FactoryCode")
                    Line.ProcessCode = .Item("ProcessCode") & ""
                    Line.ProcessGroup = .Item("ProcessGroup") & ""
                    Line.LineGroup = .Item("LineGroup") & ""
                    Line.LineName = .Item("LineName")
                    Return Line
                End With
            End If
        End Using
    End Function
    Public Shared Function GetList(UserID As String, FactoryCode As String, ItemTypeCode As String) As List(Of ClsLine)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select distinct L.FactoryCode, L.ProcessCode, L.LineCode, L.LineCode + ' - ' + L.LineName as LineName, " & vbCrLf &
                "isnull(P.AllowShow, 0) AllowShow, " & vbCrLf &
                "isnull(P.AllowUpdate, 0) AllowUpdate, " & vbCrLf &
                "isnull(P.AllowVerify, 0) AllowVerify " & vbCrLf
            q = q &
                "from MS_Line L inner join spc_ItemCheckByType I " & vbCrLf &
                "on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode " & vbCrLf &
                "inner join spc_UserLine P on L.LineCode = P.LineCode " & vbCrLf &
                "where P.UserID = @UserID and P.AllowShow = 1 " & vbCrLf &
                "and I.ActiveStatus = 1 " & vbCrLf
            If FactoryCode <> "" Then
                q = q & "and L.FactoryCode = @FactoryCode "
            End If
            If ItemTypeCode <> "" Then
                q = q & "and I.ItemTypeCode = @ItemTypeCode "
            End If
            q = q & "order by LineCode"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim FactoryList As New List(Of ClsLine)
            Do While rd.Read
                Dim Factory As New ClsLine
                Factory.FactoryCode = rd("FactoryCode")
                Factory.ProcessCode = rd("ProcessCode")
                Factory.LineCode = rd("LineCode")
                Factory.LineName = rd("LineName")
                Factory.AllowShow = rd("AllowShow")
                Factory.AllowUpdate = rd("AllowUpdate")
                Factory.AllowVerify = rd("AllowVerify")
                FactoryList.Add(Factory)
            Loop
            rd.Close()
            Return FactoryList
        End Using
    End Function

    Public Shared Function GetListByProcess(UserID As String, FactoryCode As String, ProcessCode As String) As List(Of ClsLine)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select distinct L.FactoryCode, L.ProcessCode, L.LineCode, L.LineCode + ' - ' + L.LineName as LineName, " & vbCrLf &
                "isnull(P.AllowShow, 0) AllowShow, " & vbCrLf &
                "isnull(P.AllowUpdate, 0) AllowUpdate, " & vbCrLf &
                "isnull(P.AllowVerify, 0) AllowVerify " & vbCrLf
            q = q &
                "from MS_Line L inner join spc_ItemCheckByType I " & vbCrLf &
                "on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode " & vbCrLf &
                "inner join spc_UserLine P on L.LineCode = P.LineCode " & vbCrLf &
                "where P.UserID = @UserID and P.AllowShow = 1 " & vbCrLf &
                "and I.ActiveStatus = 1 " & vbCrLf
            If FactoryCode <> "" Then
                q = q & "and L.FactoryCode = @FactoryCode "
            End If
            If ProcessCode <> "" Then
                q = q & "and L.ProcessCode = @ProcessCode "
            End If
            q = q & "order by LineCode"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ProcessCode", ProcessCode)
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim FactoryList As New List(Of ClsLine)
            Do While rd.Read
                Dim Factory As New ClsLine
                Factory.FactoryCode = rd("FactoryCode")
                Factory.ProcessCode = rd("ProcessCode")
                Factory.LineCode = rd("LineCode")
                Factory.LineName = rd("LineName")
                Factory.AllowShow = rd("AllowShow")
                Factory.AllowUpdate = rd("AllowUpdate")
                Factory.AllowVerify = rd("AllowVerify")
                FactoryList.Add(Factory)
            Loop
            rd.Close()
            Return FactoryList
        End Using
    End Function
End Class