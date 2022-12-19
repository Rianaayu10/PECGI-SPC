Public Class Sconn
    Public Shared Function Stringkoneksi() As String
        Return ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
    End Function

    Public Shared Function IOTConnectionString() As String
        Return ConfigurationManager.ConnectionStrings("IOTSystem").ConnectionString
    End Function
    Public Shared Function LinkedServerPECGI() As String
        Return ConfigurationManager.ConnectionStrings("LinkedServerPECGI").ConnectionString
    End Function
End Class
