Imports System.Data.SqlClient
Public Class clsSiteMasterDB
    Public Shared Function GetMenu(AdminStatus As String, UserID As String) As DataTable
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String = "SP_SPC_UserMenu"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("AdminStatus", AdminStatus)
                cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
                dt = ds.Tables(0)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function GetSubMenu(AdminStatus As String, UserID As String, MenuID As String, MenuIndex As String, GroupID As String) As DataTable
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String = "SP_SPC_SubUserMenu"
                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("AdminStatus", AdminStatus)
                cmd.Parameters.AddWithValue("UserID", UserID)
                cmd.Parameters.AddWithValue("MenuID", MenuID)
                cmd.Parameters.AddWithValue("MenuIndex", MenuIndex)
                cmd.Parameters.AddWithValue("GroupID", GroupID)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
                dt = ds.Tables(0)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function GetDelayInput(User As String) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetDelayInput"

                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", User)
                    .AddWithValue("FactoryCode", "ALL")
                    .AddWithValue("TypeReport", "1")
                End With
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function GetDelayVerify(User As String) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetDelayVerification"

                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", User)
                    .AddWithValue("FactoryCode", "ALL")
                    .AddWithValue("TypeReport", "1")
                    .AddWithValue("TypeForm", "1")
                End With
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

    Public Shared Function GetNGInput(User As String) As DataTable
        Try
            Using cn As New SqlConnection(Sconn.Stringkoneksi)
                cn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetNGInput"

                Dim cmd As New SqlCommand(sql, cn)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .AddWithValue("User", User)
                    .AddWithValue("FactoryCode", "ALL")
                    .AddWithValue("TypeReport", "1")
                End With
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
