Imports System.Data.SqlClient

Public Class clsItemCheck
    Public Property ItemCheckCode As String
    Public Property ItemCheck As String
    Public Property Measure2Cls As String
End Class

Public Class clsItemCheckDB
    Public Shared Function GetData(ItemCheckCode As String) As clsItemCheck
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "Select * from spc_ItemCheckMaster where ItemCheckCode = @ItemCheckCode"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count = 0 Then
                Return Nothing
            Else
                Dim Item As New clsItemCheck
                Item.ItemCheckCode = dt.Rows(0)("ItemCheckCode") & ""
                Item.ItemCheck = dt.Rows(0)("ItemCheck") & ""
                Item.Measure2Cls = dt.Rows(0)("Measure2Cls") & ""
                Return Item
            End If
        End Using
    End Function

    Public Shared Function GetList(Optional FactoryCode As String = "", Optional ItemTypeCode As String = "", Optional LineCode As String = "", Optional ShowAll As Boolean = False) As List(Of clsItemCheck)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = ""
            If ShowAll Then
                q = "select 'ALL' ItemCheckCode, 'ALL' ItemCheck, 0 seq union "
            End If
            q = q & "select I.ItemCheckCode, I.ItemCheckCode + ' - ' + I.ItemCheck ItemCheck, 1 seq " & vbCrLf &
                "from spc_ItemCheckMaster I inner join spc_ItemCheckByType T on I.ItemCheckCode = T.ItemCheckCode "
            q = q & "where T.ItemCheckCode is not Null "
            If FactoryCode <> "" Then
                q = q & "and T.FactoryCode = @FactoryCode " & vbCrLf
            End If
            If ItemTypeCode <> "" Then
                q = q & "and ItemTypeCode = @ItemTypeCode " & vbCrLf
            End If
            If LineCode <> "" Then
                q = q & "and LineCode = @LineCode "
            End If
            q = q & "and T.ActiveStatus = '1' "
            q = q & "order by seq, ItemCheckCode "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            Dim rd As SqlDataReader = cmd.ExecuteReader
            Dim ItemCheckList As New List(Of clsItemCheck)
            Do While rd.Read
                Dim ItemCheck As New clsItemCheck
                ItemCheck.ItemCheckCode = rd("ItemCheckCode") & ""
                ItemCheck.ItemCheck = rd("ItemCheck") & ""
                ItemCheckList.Add(ItemCheck)
            Loop
            rd.Close()
            Return ItemCheckList
        End Using
    End Function
End Class
