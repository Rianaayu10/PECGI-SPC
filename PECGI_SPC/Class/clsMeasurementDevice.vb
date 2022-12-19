Imports System.Data.SqlClient

Public Class clsMeasurementDevice
    Public Property FactoryCode As String
    Public Property RegNo As String
    Public Property Description As String
    Public Property ToolName As String
    Public Property ToolFunction As String
    Public Property BaudRate As String
    Public Property DataBit As String
    Public Property Parity As String
    Public Property StopBit As String
    Public Property Stable As String
    Public Property Passive As String
    Public Property GetResult As String
    Public Property Active As String
    Public Property User As String
End Class

Public Class clsMeasurementDeviceDB
    Public Shared Function FillCombo(Type As String) As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_MSDevice_FillCombo"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("Type", Type)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function GetList(Factory As String) As DataTable
        Using cn As New SqlConnection(Sconn.Stringkoneksi)
            cn.Open()
            Dim sql As String
            sql = "sp_SPC_MSDevice_Sel"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("Factory", Factory)
            cmd.CommandType = CommandType.StoredProcedure

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        End Using
    End Function

    Public Shared Function InsertUpdate(cls As clsMeasurementDevice, Type As String) As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_MSDevice_InsUpd"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", cls.FactoryCode)
                cmd.Parameters.AddWithValue("RegistrationNo", cls.RegNo)
                cmd.Parameters.AddWithValue("Description", cls.Description)
                cmd.Parameters.AddWithValue("ToolName", cls.ToolName)
                cmd.Parameters.AddWithValue("ToolFunction", cls.ToolFunction)
                cmd.Parameters.AddWithValue("BaudRate", CInt(cls.BaudRate))
                cmd.Parameters.AddWithValue("DataBits", CInt(cls.DataBit))
                cmd.Parameters.AddWithValue("Parity", cls.Parity)
                cmd.Parameters.AddWithValue("StopBits", CInt(cls.StopBit))
                cmd.Parameters.AddWithValue("StableCondition", CInt(cls.Stable))
                cmd.Parameters.AddWithValue("PassiveActiveCls", cls.Passive)
                cmd.Parameters.AddWithValue("GetResultData", CInt(cls.GetResult))
                cmd.Parameters.AddWithValue("ActiveStatus", cls.Active)
                cmd.Parameters.AddWithValue("User", cls.User)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function Delete(cls As clsMeasurementDevice) As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_MSDevice_Del"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", cls.FactoryCode)
                cmd.Parameters.AddWithValue("RegistrationNo", cls.RegNo)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function Check(cls As clsMeasurementDevice, Optional ByRef pErr As String = "") As Boolean
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String
                sql = "sp_SPC_MSDevice_Check"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", cls.FactoryCode)
                cmd.Parameters.AddWithValue("RegistrationNo", cls.RegNo)

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            pErr = ex.Message
        End Try
    End Function
End Class
