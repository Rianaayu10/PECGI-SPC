﻿Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Data.SqlClient

Public Class clsAlertDashboard
    Public Property FactoryCode As String
    Public Property ItemTypeCode As String
    Public Property LineCode As String
    Public Property ItemCheckCode As String
    Public Property ProdDate As String
    Public Property ShiftCode As String
    Public Property SequenceNo As String
    Public Property BodyEmail As String
    Public Property Subject As String
    Public Property ToEmail As String
    Public Property CC As String
    Public Property NotificationCategory As String
    Public Property LSL As String
    Public Property USL As String
    Public Property LCL As String
    Public Property UCL As String
    Public Property MinValue As String
    Public Property MaxValue As String
    Public Property Average As String
    Public Property Status As String
    Public Property ScheduleStart As String
    Public Property ScheduleEnd As String
    Public Property VerifTime As String
    Public Property DelayTime As String
    Public Property LastUser As String
End Class

Public Class clsAlertDashboardDB

#Region "Declaration"
    Dim st As New clsConfig
    Public ConStr As String = st.m_ConnectionString
#End Region

    Public Shared Function GetListNGResult(FactoryCode As String, ConStr As String, Optional ByRef pErr As String = "") As List(Of clsAlertDashboard)
        Try
            Using conn As New SqlConnection(ConStr)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetNGInput"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("User", "")
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("TypeReport", 2)

                Dim rd As SqlDataReader = cmd.ExecuteReader
                Dim AlertList As New List(Of clsAlertDashboard)
                Do While rd.Read
                    Dim Alert As New clsAlertDashboard
                    Alert.FactoryCode = rd("FactoryCode")
                    Alert.ItemTypeCode = rd("ItemTypeName")
                    Alert.LineCode = rd("LineCode")
                    Alert.ItemCheckCode = rd("ItemCheckCode")
                    Alert.ProdDate = rd("Date")
                    Alert.ShiftCode = rd("ShiftCode")
                    Alert.SequenceNo = rd("SequenceNo")
                    Alert.USL = rd("USL")
                    Alert.LSL = rd("LSL")
                    Alert.UCL = rd("UCL")
                    Alert.LCL = rd("LCL")
                    Alert.MinValue = rd("MinValue")
                    Alert.MaxValue = rd("MaxValue")
                    Alert.Average = rd("Average")
                    AlertList.Add(Alert)
                Loop
                rd.Close()
                Return AlertList
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function

    Public Shared Function GetListDelayInput(FactoryCode As String, ConStr As String, Optional ByRef pErr As String = "") As List(Of clsAlertDashboard)
        Try
            Using conn As New SqlConnection(ConStr)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetDelayInput"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("User", "")
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("TypeReport", 2)

                Dim rd As SqlDataReader = cmd.ExecuteReader
                Dim AlertList As New List(Of clsAlertDashboard)
                Do While rd.Read
                    Dim Alert As New clsAlertDashboard
                    Alert.FactoryCode = rd("FactoryCode")
                    Alert.ItemTypeCode = rd("ItemTypeName")
                    Alert.LineCode = rd("LineCode")
                    Alert.ItemCheckCode = rd("ItemCheckCode")
                    Alert.ShiftCode = rd("ShiftCode")
                    Alert.SequenceNo = rd("SequenceNo")
                    Alert.ScheduleStart = rd("StartTime")
                    Alert.ScheduleEnd = rd("EndTime")
                    Alert.DelayTime = rd("Delay")
                    Alert.ProdDate = rd("Date")
                    AlertList.Add(Alert)
                Loop
                rd.Close()
                Return AlertList
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function

    Public Shared Function GetListDelayVerification(FactoryCode As String, ConStr As String, Optional ByRef pErr As String = "") As List(Of clsAlertDashboard)
        Try
            Using conn As New SqlConnection(ConStr)
                conn.Open()
                Dim sql As String = ""
                sql = "sp_SPC_GetDelayVerification"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("User", "")
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("TypeReport", 2)

                Dim rd As SqlDataReader = cmd.ExecuteReader
                Dim AlertList As New List(Of clsAlertDashboard)
                Do While rd.Read
                    Dim Alert As New clsAlertDashboard
                    Alert.FactoryCode = rd("FactoryCode")
                    Alert.ItemTypeCode = rd("ItemTypeName")
                    Alert.LineCode = rd("LineCode")
                    Alert.ItemCheckCode = rd("ItemCheckCode")
                    Alert.ProdDate = rd("Date")
                    Alert.ShiftCode = rd("ShiftCode")
                    Alert.SequenceNo = rd("SequenceNo")
                    Alert.USL = rd("USL")
                    Alert.LSL = rd("LSL")
                    Alert.UCL = rd("UCL")
                    Alert.LCL = rd("LCL")
                    Alert.MinValue = rd("MinValue")
                    Alert.MaxValue = rd("MaxValue")
                    Alert.Average = rd("Average")
                    Alert.Status = rd("Status")
                    Alert.VerifTime = rd("VerifTime")
                    Alert.DelayTime = rd("DelayVerif")
                    AlertList.Add(Alert)
                Loop
                rd.Close()
                Return AlertList
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try
    End Function

    Public Shared Function SendEmail(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, LinkDate As String, ShiftCode As String, SequenceNo As String, NotificationCategory As String,
                                     LSL As String, USL As String, LCL As String, UCL As String, MinValue As String, MaxValue As String, Average As String, Status As String,
                                     ScheduleStart As String, ScheduleEnd As String, VerifTime As String, DelayTime As String, ConStr As String, Optional ByRef pErr As String = "") As Integer
        Try
            Using Cn As New SqlConnection(ConStr)
                Cn.Open()
                Dim q As String
                q = "SP_SPC_SendEmailAlert"
                Dim cmd As New SqlCommand(q, Cn)
                'Dim des As New clsDESEncryption("TOS")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                cmd.Parameters.AddWithValue("LineCode", LineCode)
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
                cmd.Parameters.AddWithValue("ProdDate", LinkDate)
                cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
                cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)
                cmd.Parameters.AddWithValue("LSL", LSL)
                cmd.Parameters.AddWithValue("USL", USL)
                cmd.Parameters.AddWithValue("LCL", LCL)
                cmd.Parameters.AddWithValue("UCL", UCL)
                cmd.Parameters.AddWithValue("MinValue", MinValue)
                cmd.Parameters.AddWithValue("MaxValue", MaxValue)
                cmd.Parameters.AddWithValue("Average", Average)
                cmd.Parameters.AddWithValue("Status", Status)
                cmd.Parameters.AddWithValue("ScheduleStart", ScheduleStart)
                cmd.Parameters.AddWithValue("ScheduleEnd", ScheduleEnd)
                cmd.Parameters.AddWithValue("VerifTime", VerifTime)
                cmd.Parameters.AddWithValue("DelayTime", DelayTime)
                cmd.Parameters.AddWithValue("NotificationCategory", NotificationCategory)
                cmd.Parameters.AddWithValue("LastUser", "spc")
                Dim i As Integer = cmd.ExecuteNonQuery
                Return 1
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try

    End Function

    Public Shared Function CheckDataSendEmailAlert(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, LinkDate As String, ShiftCode As String, SequenceNo As String, ConStr As String, Optional ByRef pErr As String = "") As DataTable

        Try
            Using Cn As New SqlConnection(ConStr)
                Cn.Open()
                Dim q As String
                q = "SP_SPC_CheckDataSendEmailAlert"
                Dim cmd As New SqlCommand(q, Cn)
                'Dim des As New clsDESEncryption("TOS")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                cmd.Parameters.AddWithValue("LineCode", LineCode)
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
                cmd.Parameters.AddWithValue("ProdDate", LinkDate)
                cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
                cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)

                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Return dt
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try

    End Function

    Public Shared Function SendNotification(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, LinkDate As String, ShiftCode As String, SequenceNo As String, NotificationCategory As String, ConStr As String, Optional ByRef pErr As String = "") As Integer
        Try
            Using Cn As New SqlConnection(ConStr)
                Cn.Open()
                Dim q As String
                q = "sp_SPC_NotificationLog_Ins"
                Dim cmd As New SqlCommand(q, Cn)
                'Dim des As New clsDESEncryption("TOS")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                cmd.Parameters.AddWithValue("LineCode", LineCode)
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
                cmd.Parameters.AddWithValue("ProdDate", LinkDate)
                cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
                cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)
                cmd.Parameters.AddWithValue("NotificationCategory", NotificationCategory)
                cmd.Parameters.AddWithValue("LastUser", "spc")
                Dim i As Integer = cmd.ExecuteNonQuery
                Return i
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try

    End Function

    Public Shared Function CheckDataSendNotificationAlert(FactoryCode As String, ItemTypeCode As String, LineCode As String, ItemCheckCode As String, LinkDate As String, ShiftCode As String, SequenceNo As String, NotificationCategory As String, ConStr As String, Optional ByRef pErr As String = "") As DataTable

        Try
            Using Cn As New SqlConnection(ConStr)
                Cn.Open()
                Dim q As String
                q = "SP_SPC_CheckDataSendNotification"
                Dim cmd As New SqlCommand(q, Cn)
                'Dim des As New clsDESEncryption("TOS")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode)
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode)
                cmd.Parameters.AddWithValue("LineCode", LineCode)
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode)
                cmd.Parameters.AddWithValue("ProdDate", LinkDate)
                cmd.Parameters.AddWithValue("ShiftCode", ShiftCode)
                cmd.Parameters.AddWithValue("SequenceNo", SequenceNo)
                cmd.Parameters.AddWithValue("NotificationCategory", NotificationCategory)

                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Return dt
            End Using
        Catch ex As Exception
            pErr = ex.Message
            Return Nothing
        End Try

    End Function
    Public Shared Function FillComboFactoryGrid(ConStr As String, Optional User As String = "") As DataTable
        Using cn As New SqlConnection(ConStr)
            cn.Open()
            Dim sql As String
            sql = "SELECT distinct FactoryCode, FactoryName FROM MS_Factory"
            Dim cmd As New SqlCommand(sql, cn)
            cmd.CommandType = CommandType.Text

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        End Using
    End Function
End Class

