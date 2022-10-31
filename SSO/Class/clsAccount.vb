Imports System.Data.SqlClient
Public Class clsAccount
    Public Shared Function GetURL(UserID As String) As DataTable
        Dim pErr = ""
        Try
            Using Cn As New SqlConnection(Sconn.Stringkoneksi)
                Cn.Open()
                Dim SQL As String = ""
                SQL = "SELECT FactoryName, URL, A.FactoryCode" & vbCrLf &
                      "FROM SPC_IOTConSetting A" & vbCrLf &
                      "INNER JOIN MS_Factory B ON A.FactoryCode = B.FactoryCode where AppID = 'SPC'"
                Dim Cmd As New SqlCommand(SQL, Cn)
                Cmd.Parameters.AddWithValue("UserID", UserID)
                Dim da As New SqlDataAdapter(Cmd)
                Dim dt As New DataTable
                da.Fill(dt)

                Dim clsDESEncryption As New clsDESEncryption("TOS")
                Dim UserEncript = clsDESEncryption.EncryptData(UserID)
                Dim dtRes As DataTable = New DataTable()
                dtRes.Columns.Add("FactoryName")
                dtRes.Columns.Add("URL")

                For i = 0 To dt.Rows.Count() - 1
                    Dim FactoryCode = clsDESEncryption.EncryptData(dt.Rows(i)("FactoryCode"))
                    Dim FactoryName = dt.Rows(i)("FactoryName")
                    Dim URL = dt.Rows(i)("URL")
                    Dim dr As DataRow = dtRes.NewRow()
                    dr(0) = FactoryName
                    dr(1) = URL & "?UserID=" & UserEncript & "&FactoryCode=" & FactoryCode
                    dtRes.Rows.Add(dr)
                Next

                Return dtRes
            End Using
        Catch ex As Exception
            Throw New Exception("Query Error !" & ex.Message)
        End Try
    End Function

End Class
