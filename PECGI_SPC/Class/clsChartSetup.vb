Imports System.Data.SqlClient

Public Class clsChartSetup
    Public Property FactoryCode As String
    Public Property ItemTypeCode As String
    Public Property LineCode As String
    Public Property ItemCheckCode As String
    Public Property StartDate As Date
    Public Property EndDate As Date
    Public Property SpecUSL As Double
    Public Property SpecLSL As Double
    Public Property XBarCL As Double
    Public Property XBarUCL As Double
    Public Property XBarLCL As Double
    Public Property CPCL As Double
    Public Property CPUCL As Double
    Public Property CPLCL As Double

    Public Property RCL As Double
    Public Property RLCL As Double
    Public Property RUCL As Double
    Public Property Min As Double
    Public Property Max As Double
    Public Property R As Double
    Public Property Avg As Double
    Public Property NG As String
End Class

Public Class clsChartSetupDB
    Public Shared Function GetData(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, ProdDate As Date) As clsChartSetup
        Using Cn As New SqlConnection(Sconn.Stringkoneksi)
            Cn.Open()
            Dim q As String = "sp_SPC_ChartSetup"
            Dim cmd As New SqlCommand(q, Cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
            cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
            cmd.Parameters.AddWithValue("LineCode", LineCode)
            cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
            cmd.Parameters.AddWithValue("ProdDate", ProdDate)
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            Dim dt As DataTable = ds.Tables(0)
            If dt.Rows.Count = 0 Then
                Return Nothing
            Else
                Dim Setup As New clsChartSetup
                With dt.Rows(0)
                    Setup.FactoryCode = FactoryCode
                    Setup.ItemTypeCode = ItemTypeCode
                    Setup.LineCode = LineCode
                    Setup.ItemCheckCode = ItemCheckCode
                    Setup.StartDate = .Item("StartDate")
                    Setup.EndDate = .Item("EndDate")
                    Setup.SpecUSL = .Item("SpecUSL")
                    Setup.SpecLSL = .Item("SpecLSL")
                    Setup.XBarCL = 0
                    Setup.XBarUCL = .Item("XBarUCL")
                    Setup.XBarLCL = .Item("XBarLCL")
                    Setup.CPCL = .Item("CPCL")
                    Setup.CPUCL = .Item("CPUCL")
                    Setup.CPLCL = .Item("CPLCL")
                    Setup.RCL = .Item("RCL")
                    Setup.RLCL = .Item("RLCL")
                    Setup.RUCL = .Item("RUCL")
                End With
                Return Setup
            End If
        End Using
    End Function
End Class