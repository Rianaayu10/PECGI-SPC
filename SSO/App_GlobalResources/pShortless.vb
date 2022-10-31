Public Class pShortless
#Region "Variabel Private"

    Private _pDesc1 As String = String.Empty
    Private _pDesc2 As String = String.Empty
    Private _pDesc3 As String = String.Empty
    Private _pDesc4 As String = String.Empty
    Private _pDesc5 As String = String.Empty
#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pDesc1 As String)
        _pDesc1 = pDesc1
    End Sub

#End Region

#Region "Properti Public"

    Public Property pDesc1() As String
        Get
            Return _pDesc1
        End Get
        Set(ByVal value As String)
            _pDesc1 = value
        End Set
    End Property

    Public Property pDesc2() As String
        Get
            Return _pDesc2
        End Get
        Set(ByVal value As String)
            _pDesc2 = value
        End Set
    End Property

    Public Property pDesc3() As String
        Get
            Return _pDesc3
        End Get
        Set(ByVal value As String)
            _pDesc3 = value
        End Set
    End Property

    Public Property pDesc4() As String
        Get
            Return _pDesc4
        End Get
        Set(ByVal value As String)
            _pDesc4 = value
        End Set
    End Property

    Public Property pDesc5() As String
        Get
            Return _pDesc5
        End Get
        Set(ByVal value As String)
            _pDesc5 = value
        End Set
    End Property

#End Region
End Class
