Public Class Sconn
    Public Shared Function F1Connection() As String
        Return ConfigurationManager.ConnectionStrings("F1Conn").ConnectionString
    End Function
    Public Shared Function F2Connection() As String
        Return ConfigurationManager.ConnectionStrings("F2Conn").ConnectionString
    End Function
    Public Shared Function F3Connection() As String
        Return ConfigurationManager.ConnectionStrings("F3Conn").ConnectionString
    End Function
    Public Shared Function F4Connection() As String
        Return ConfigurationManager.ConnectionStrings("F4Conn").ConnectionString
    End Function
    Public Shared Function SSOConnection() As String
        Return ConfigurationManager.ConnectionStrings("SSOConn").ConnectionString
    End Function
End Class
