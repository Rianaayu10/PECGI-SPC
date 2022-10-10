Imports System.Data.SqlClient

Public Class clsXRChart
    Public Property Seq As String
    Public Property Description As String
    Public Property Value As Double?
    Public Property AvgValue As Double?
    Public Property MinValue As Double
    Public Property MaxValue As Double
    Public Property RValue As Double
    Public Property LCL As Double
    Public Property UCL As Double
    Public Property USL As Double
    Public Property LSL As Double
    Public Property RuleValue As Double?
    Public Property RuleYellow As Double?
    Public Property RuleColor As String
End Class

Public Class clsHistogram
    Public Property Range As String
    Public Property Value As Double
    Public Property MaxValue As Double
End Class

Public Class clsXRChartDB
    Public Shared Function GetHistogram(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String) As List(Of clsHistogram)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim cmd As New SqlCommand("sp_SPC_Histogram", Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            cmd.Parameters.AddWithValue("ProdDate2", ProdDate2)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim HtList As New List(Of clsHistogram)
            For i = 0 To dt.Rows.Count - 1
                Dim ht As New clsHistogram
                ht.Range = dt.Rows(i)("ValueRange") & ""
                ht.Value = dt.Rows(i)("ValueCount")
                ht.MaxValue = dt.Rows(i)("MaxValue")
                HtList.Add(ht)
            Next
            Return HtList
        End Using
    End Function

    Public Shared Function GetChartXR(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate2 As String, Optional VerifiedOnly As Integer = 0) As List(Of clsXRChart)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_SampleControlChart"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate2", ProdDate2)
            cmd.Parameters.AddWithValue("VerifiedOnly", VerifiedOnly)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim XRList As New List(Of clsXRChart)
            For i = 0 To dt.Rows.Count - 1
                Dim xr As New clsXRChart
                With dt.Rows(i)
                    xr.Seq = .Item("Seq")
                    xr.Description = .Item("Description")
                    Dim value As Double
                    If Not IsDBNull(.Item("Value")) Then
                        value = .Item("Value")
                        xr.Value = value
                    End If
                    If Not IsDBNull(.Item("AvgValue")) Then
                        value = .Item("AvgValue")
                        xr.AvgValue = value
                    End If
                    If Not IsDBNull(.Item("RValue")) Then
                        xr.RValue = .Item("RValue")
                    End If
                    If Not IsDBNull(.Item("RuleValue")) Then
                        value = .Item("RuleValue")
                        xr.RuleValue = value
                    End If
                    If Not IsDBNull(.Item("RuleYellow")) Then
                        value = .Item("RuleYellow")
                        xr.RuleYellow = value
                    End If
                    If Not IsDBNull(.Item("XbarUCL")) Then
                        value = .Item("XbarUCL")
                        xr.UCL = value
                    End If
                    If Not IsDBNull(.Item("XbarLCL")) Then
                        value = .Item("XbarLCL")
                        xr.LCL = value
                    End If
                    If Not IsDBNull(.Item("SpecLSL")) Then
                        value = .Item("SpecLSL")
                        xr.LSL = value
                    End If
                    If Not IsDBNull(.Item("SpecUSL")) Then
                        value = .Item("SpecUSL")
                        xr.USL = value
                    End If
                    If Not IsDBNull(.Item("MaxValue")) Then
                        value = .Item("MaxValue")
                        xr.MaxValue = value
                    End If
                    If Not IsDBNull(.Item("MinValue")) Then
                        value = .Item("MinValue")
                        xr.MinValue = value
                    End If
                End With
                XRList.Add(xr)
            Next
            Return XRList
        End Using
    End Function

    Public Shared Function GetChartXRMonthly(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String, ProdDate2 As String) As List(Of clsXRChart)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_SampleControlChart"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            cmd.Parameters.AddWithValue("ProdDate2", ProdDate2)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim XRList As New List(Of clsXRChart)
            For i = 0 To dt.Rows.Count - 1
                Dim xr As New clsXRChart
                With dt.Rows(i)
                    xr.Seq = .Item("Seq")
                    xr.Description = .Item("Description")
                    Dim value As Double
                    If Not IsDBNull(.Item("Value")) Then
                        value = .Item("Value")
                        xr.Value = value
                    End If
                    If Not IsDBNull(.Item("AvgValue")) Then
                        value = .Item("AvgValue")
                        xr.AvgValue = value
                    End If
                    If Not IsDBNull(.Item("RValue")) Then
                        xr.RValue = .Item("RValue")
                    End If
                    If Not IsDBNull(.Item("RuleValue")) Then
                        value = .Item("RuleValue")
                        xr.RuleValue = value
                    End If
                    If Not IsDBNull(.Item("RuleYellow")) Then
                        value = .Item("RuleYellow")
                        xr.RuleYellow = value
                    End If
                    If Not IsDBNull(.Item("MaxValue")) Then
                        value = .Item("MaxValue")
                        xr.MaxValue = value
                    End If
                    If Not IsDBNull(.Item("MinValue")) Then
                        value = .Item("MinValue")
                        xr.MinValue = value
                    End If
                End With
                XRList.Add(xr)
            Next
            Return XRList
        End Using
    End Function

    Public Shared Function GetChartType(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String) As String
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "select top 1 isnull(CharacteristicStatus, '0') from spc_ItemCheckByType where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @Line and ItemCheckCode = @ItemCheckCode "
            Dim cmd As New SqlCommand(q, Cn)
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            Dim ChartType As String = cmd.ExecuteScalar
            Return ChartType
        End Using
    End Function



    Public Shared Function GetChartR(FactoryCode As String, ItemTypeCode As String, Line As String, ItemCheckCode As String, ProdDate As String) As List(Of clsXRChart)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_RChart"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("Line", Line)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim XRList As New List(Of clsXRChart)
            For i = 0 To dt.Rows.Count - 1
                Dim xr As New clsXRChart
                With dt.Rows(i)
                    xr.Seq = .Item("TimeNo")
                    xr.Description = 1
                    Dim value As Double = .Item("AvgValue")
                    xr.AvgValue = value
                    value = .Item("RValue")
                    xr.RValue = .Item("RValue")
                    value = .Item("MaxValue")
                    xr.MaxValue = value
                End With
                XRList.Add(xr)
            Next
            Return XRList
        End Using
    End Function

    Public Shared Function GetChartX() As List(Of clsXRChart)
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim xrList As New List(Of clsXRChart)
            Dim xr As clsXRChart
            xr = New clsXRChart
            xr.Seq = "1"
            xr.Description = "Avg"
            xr.Value = 2.5
            xr.AvgValue = 3
            xrList.Add(xr)

            xr = New clsXRChart
            xr.Seq = "2"
            xr.Description = "Avg"
            xr.AvgValue = 2.6
            xrList.Add(xr)

            xr = New clsXRChart
            xr.Seq = "3"
            xr.Description = "Avg"
            xr.Value = 2.7
            xrList.Add(xr)

            xr = New clsXRChart
            xr.Seq = "3"
            xr.Description = "Avg"
            xr.Value = 2.56
            xrList.Add(xr)

            xr = New clsXRChart
            xr.Seq = "1"
            xr.Description = "Max"
            xr.AvgValue = 2.61
            xrList.Add(xr)

            xr = New clsXRChart
            xr.Seq = "2"
            xr.Description = "Max"
            xr.AvgValue = 2.52
            xrList.Add(xr)

            Return xrList
        End Using
    End Function
End Class
