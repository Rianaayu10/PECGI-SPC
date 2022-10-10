Imports System.Windows.Forms
Imports System.Text.Encoding

Public Class frmLogin

#Region "Init & load"
    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblVersion.Text = "Version. " & Me.ProductVersion
        lblcopyright.Text = ChrW(169) & " 2022 - Developed by PT. TOS Information Systems Indonesia"
    End Sub

#End Region
#Region "Event"
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        frmSPCAlertDashboard.Show()
        Me.Hide()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
#End Region

#Region "sub & Function"

#End Region

End Class
