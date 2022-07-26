﻿Public Class clsUserSetup
    Public Property AppID As String
    Public Property UserID As String
    Public Property FullName As String
    Public Property UserType As String
    Public Property Password As String
    Public Property ConfirmPassword As String
    Public Property LockStatus As String
    Public Property FailedLogin As Integer
    Public Property AdminStatus As String
    Public Property Description As String
    Public Property SecurityQuestion As String
    Public Property SecurityAnswer As String
    Public Property PasswordHint As String
    Public Property LineLeaderStatus As String
    Public Property LineForemanStatus As String
    Public Property ProdSectionHeadStatus As String
    Public Property QELeaderStatus As String
    Public Property QESectionHeadStatus As String
    Public Property UpdateDate As String
    Public Property UpdateUser As String
    Public Property CreateDate As String
    Public Property CreateUser As String
    Public ReadOnly Property Privileges As String
        Get
            Return "Privileges"
        End Get
    End Property
    Public ReadOnly Property LinePrivileges As String
        Get
            Return "Lines"
        End Get
    End Property
End Class
