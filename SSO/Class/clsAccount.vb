Imports System.Data.SqlClient
Public Class clsAccount
    Public Shared Function GetURL(UserID As String, Password As String) As DataTable
        Dim pErr = ""
        Try
            Using Cn As New SqlConnection(Sconn.SSOConnection)
                Cn.Open()
                Dim SQL As String = ""
                SQL = "SELECT URL, FactoryCode" & vbCrLf &
                      "FROM SPC_IOTConSetting WHERE AppID = 'SPC'"
                Dim Cmd As New SqlCommand(SQL, Cn)
                Cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(Cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Dim clsDESEncryption As New clsDESEncryption("TOS")
                Dim dtRes As DataTable = New DataTable()
                dtRes.Columns.Add("FactoryName")
                dtRes.Columns.Add("URL")
                dtRes.Columns.Add("BKClr")

                For i = 0 To dt.Rows.Count() - 1
                    Dim FactoryCode = dt.Rows(i)("FactoryCode")
                    Dim URL = dt.Rows(i)("URL")
                    Dim FactoryName = ""
                    Dim BKClr = ""
                    Dim Link = clsDESEncryption.EncryptData("SPCSSO" & "|" & UserID & "|" & Password & "|")

                    If FactoryCode = "F001" Then
                        FactoryName = "FACTORY 1"
                        BKClr = "#FF0000"
                    ElseIf FactoryCode = "F002" Then
                        FactoryName = "FACTORY 2"
                        BKClr = "#0B02FF"
                    ElseIf FactoryCode = "F003" Then
                        FactoryName = "FACTORY 3"
                        BKClr = "#377D22"
                    ElseIf FactoryCode = "F004" Then
                        FactoryName = "FACTORY 4"
                        BKClr = "#FF6C00"
                    End If

                    Dim dr As DataRow = dtRes.NewRow()
                    dr(0) = FactoryName
                    dr(1) = URL & "?link=" & Link
                    dr(2) = BKClr
                    dtRes.Rows.Add(dr)
                Next

                Return dtRes
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

End Class
