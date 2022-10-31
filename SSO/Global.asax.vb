Imports System.Reflection
Imports System.Web.SessionState
Imports Microsoft.VisualBasic.Logging

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

    'Public Overrides Sub Init()
    '    Dim appName As String = ConfigurationManager.AppSettings("ApplicationName")
    '    If Not String.IsNullOrEmpty(appName) Then
    '        Dim runtimeInfo As System.Reflection.FieldInfo = GetType(HttpRuntime).GetField("_theRuntime", System.Reflection.BindingFlags.[Static] Or System.Reflection.BindingFlags.NonPublic)
    '        Dim theRuntime As HttpRuntime = DirectCast(runtimeInfo.GetValue(Nothing), HttpRuntime)
    '        Dim appNameInfo As System.Reflection.FieldInfo = GetType(HttpRuntime).GetField("_appDomainAppId", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
    '        appNameInfo.SetValue(theRuntime, appName)
    '    End If
    'End Sub

End Class