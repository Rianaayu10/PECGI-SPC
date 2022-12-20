Imports System.Data.SqlClient
Public Class clsProdSampleVerificationDB
    Public Shared Function FillCombo(ByVal Status As String, data As clsProdSampleVerification) As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_ProdSampleVerification_FillCombo"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Status", Status)
                cmd.Parameters.AddWithValue("User", If(data.User, ""))
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("ProcessGroup", If(data.ProcessGroup, ""))
                cmd.Parameters.AddWithValue("LineGroup", If(data.LineGroup, ""))
                cmd.Parameters.AddWithValue("ProcessCode", If(data.ProcessCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ShiftCode", If(data.ShiftCode, ""))
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function


    Public Shared Function GridLoad(ActionGrid As String, data As clsProdSampleVerification) As DataSet
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_ProdSampleVerification_Grid"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ShiftCode", If(data.ShiftCode, ""))
                cmd.Parameters.AddWithValue("Seq", If(data.Seq, ""))
                cmd.Parameters.AddWithValue("ProdDate", If(data.ProdDate, ""))
                cmd.Parameters.AddWithValue("ActionGrid", If(ActionGrid, ""))
                cmd.Parameters.AddWithValue("Shiftcode_Grid", If(data.Shiftcode_Grid, ""))
                cmd.Parameters.AddWithValue("ProdDate_Grid", If(data.ProdDate_Grid, ""))
                cmd.Parameters.AddWithValue("ShowVerify", If(data.ShowVerify, ""))
                cmd.Parameters.AddWithValue("User", If(data.User, ""))
                Dim da As New SqlDataAdapter(cmd)
                Dim ds As New DataSet
                da.Fill(ds)
                Return ds
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function Validation(Action As String, data As clsProdSampleVerification) As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_ProdSampleVerification_Val"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ShiftCode", If(data.ShiftCode, ""))
                cmd.Parameters.AddWithValue("Seq", If(data.Seq, ""))
                cmd.Parameters.AddWithValue("ProdDate", If(data.ProdDate, ""))
                cmd.Parameters.AddWithValue("Action", If(Action, ""))
                cmd.Parameters.AddWithValue("User", If(data.User, ""))
                Dim da As New SqlDataAdapter(cmd)
                Dim ds As New DataSet
                Dim dt As New DataTable
                da.Fill(ds)
                dt = ds.Tables(0)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function IOTconnection(ActionSts As String, data As clsProdSampleVerification) As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_ProdSampleVerification_IOTConn"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ShiftCode", If(data.ShiftCode, ""))
                cmd.Parameters.AddWithValue("Seq", If(data.Seq, ""))
                cmd.Parameters.AddWithValue("ProdDate", If(data.ProdDate, ""))
                cmd.Parameters.AddWithValue("User", If(data.User, ""))
                cmd.Parameters.AddWithValue("ActionSts", If(ActionSts, ""))
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function Activity_Insert(ActionStatus As String, data As clsProdSampleVerification) As String
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim Sql = "SP_SPC_ProdSampleVerification_Ins"
                Dim cmd As New SqlCommand(Sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("ActivityID", If(data.ActivityID, ""))
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ProdDate", If(data.ProdDate, ""))
                cmd.Parameters.AddWithValue("ShiftCode", If(data.ShiftCode, ""))
                cmd.Parameters.AddWithValue("Time", If(data.Time, ""))
                cmd.Parameters.AddWithValue("PIC", If(data.PIC, ""))
                cmd.Parameters.AddWithValue("Action", If(data.Action, ""))
                cmd.Parameters.AddWithValue("Result", If(data.Result, ""))
                cmd.Parameters.AddWithValue("Remark", If(data.Remark, ""))
                cmd.Parameters.AddWithValue("User", If(data.User, ""))
                cmd.Parameters.AddWithValue("ActionSts", If(ActionStatus, ""))

                Dim da As New SqlDataAdapter(cmd)
                Dim ds As New DataSet
                da.Fill(ds)
                Return ds.Tables(0).Rows(0)("Response")

            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function Verify(data As clsProdSampleVerification) As String
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim Sql = "SP_SPC_ProdSampleVerification_Verify"
                Dim cmd As New SqlCommand(Sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ProdDate", If(data.ProdDate, ""))
                cmd.Parameters.AddWithValue("ShiftCode", If(data.ShiftCode, ""))
                cmd.Parameters.AddWithValue("Seq", If(data.Seq, ""))
                cmd.Parameters.AddWithValue("User", If(data.User, ""))

                Dim da As New SqlDataAdapter(cmd)
                Dim ds As New DataSet
                da.Fill(ds)
                Return ds.Tables(0).Rows(0)("Result")

            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function GetFilterCombo(ByVal Status As String, data As clsProdSampleVerification) As DataTable
        Try
            Using conn As New SqlConnection(Sconn.Stringkoneksi)
                conn.Open()
                Dim sql As String = ""
                sql = "SP_SPC_ProdSampleVerification_FillCombo"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("Status", Status)
                cmd.Parameters.AddWithValue("User", If(data.User, ""))
                cmd.Parameters.AddWithValue("FactoryCode", If(data.FactoryCode, ""))
                cmd.Parameters.AddWithValue("ProcessGroup", If(data.ProcessGroup, ""))
                cmd.Parameters.AddWithValue("LineGroup", If(data.LineGroup, ""))
                cmd.Parameters.AddWithValue("ProcessCode", If(data.ProcessCode, ""))
                cmd.Parameters.AddWithValue("LineCode", If(data.LineCode, ""))
                cmd.Parameters.AddWithValue("ItemTypeCode", If(data.ItemType_Code, ""))
                cmd.Parameters.AddWithValue("ItemCheckCode", If(data.ItemCheck_Code, ""))
                cmd.Parameters.AddWithValue("ShiftCode", If(data.ShiftCode, ""))
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

End Class
