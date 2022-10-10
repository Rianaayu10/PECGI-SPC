' =====COPYRIGHT=====
' Code originally retrieved from http://www.vbforums.com/showthread.php?t=547778 - no license information supplied
' =====COPYRIGHT=====
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports spc_alert_notifications.frmNotification

Namespace ToastNotifications
    Partial Public Class frmNotification
        Inherits Form
        Private Shared ReadOnly OpenNotifications As List(Of frmNotification) = New List(Of frmNotification)()
        Private _allowFocus As Boolean
        Private ReadOnly _animator As FormAnimator
        Private _currentForegroundWindow As IntPtr
        Private dtNG As DataTable
        Private dtDelayInput As DataTable
        Private dtDelayVerification As DataTable

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <paramname></param>
        ''' <paramname></param>
        ''' <paramname></param>
        ''' <paramname></param>
        ''' <paramname></param>

        Public Sub New(ByVal title As String, ByVal body As String, ByVal duration As Integer, ByVal animation As FormAnimator.AnimationMethod, ByVal direction As FormAnimator.AnimationDirection, ByVal type As String, ByVal link As String, ByVal pdtNG As DataTable, ByVal pdtDelayInput As DataTable, ByVal pdtDelayVerification As DataTable)
            InitializeComponent()
            dtNG = pdtNG
            dtDelayInput = pdtDelayInput
            dtDelayVerification = pdtDelayVerification

            If duration < 0 Then
                duration = Integer.MaxValue
            Else
                duration = duration * 1000
            End If

            lifeTimer.Interval = duration
            labelTitle.Text = title
            labelBody.Text = body
            lblLink.Text = link
            'If type = "NG" Then
            '    pbIcon.Image = My.Resources.NG
            'ElseIf type = "Delay" Then
            '    pbIcon.Image = My.Resources.Delay
            'End If

            _animator = New FormAnimator(Me, animation, direction, 500)

            Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, Width - 5, Height - 5, 20, 20))
        End Sub

#Region "Methods"

        ''' <summary>
        ''' Displays the form
        ''' </summary>
        ''' <remarks>
        ''' Required to allow the form to determine the current foreground window before being displayed
        ''' </remarks>
        Public Overloads Sub Show()
            ' Determine the current foreground window so it can be reactivated each time this form tries to get the focus
            _currentForegroundWindow = NativeMethods.GetForegroundWindow()

            MyBase.Show()
        End Sub

#End Region

#Region "Event Handlers"
        Private Sub frmNotification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ' Display the form just above the system tray.
            Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Width, Screen.PrimaryScreen.WorkingArea.Height - Height)

            ' Move each open form upwards to make room for this one
            For Each openForm In OpenNotifications
                openForm.Top -= Height
            Next

            OpenNotifications.Add(Me)
            lifeTimer.Start()
        End Sub

        Private Sub Notification_Activated(ByVal sender As Object, ByVal e As EventArgs)
            ' Prevent the form taking focus when it is initially shown
            If Not _allowFocus Then
                ' Activate the window that previously had focus
                NativeMethods.SetForegroundWindow(_currentForegroundWindow)
            End If
        End Sub

        Private Sub Notification_Shown(ByVal sender As Object, ByVal e As EventArgs)
            ' Once the animation has completed the form can receive focus
            _allowFocus = True

            ' Close the form by sliding down.
            _animator.Duration = 0
            _animator.Direction = FormAnimator.AnimationDirection.Down
        End Sub

        Private Sub Notification_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
            ' Move down any open forms above this one
            For Each openForm In OpenNotifications
                If Object.ReferenceEquals(openForm, Me) Then
                    ' Remaining forms are below this one
                    Exit For
                End If
                openForm.Top += Height
            Next

            OpenNotifications.Remove(Me)
        End Sub

        Private Sub labelRO_Click(ByVal sender As Object, ByVal e As EventArgs) Handles labelBody.Click
            Try
                'Dim URL = lblLink.Text
                'Dim NewProcess = New ProcessStartInfo(URL)
                'NewProcess.UseShellExecute = True
                'Process.Start(NewProcess)
                'Close()
                Dim frm As New frmInboxNotifications(dtNG, dtDelayInput, dtDelayVerification)
                frm.Show()
                Close()
            Catch ex As Exception
                MsgBox("Something error with link spc.", MsgBoxStyle.OkOnly, "Error!")
            End Try
        End Sub

        Private Sub pbClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbClose.Click
            Close()
        End Sub

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.lifeTimer = New System.Windows.Forms.Timer(Me.components)
            Me.labelTitle = New System.Windows.Forms.Label()
            Me.labelBody = New System.Windows.Forms.Label()
            Me.pbIcon = New System.Windows.Forms.PictureBox()
            Me.pbClose = New System.Windows.Forms.PictureBox()
            Me.lblLink = New System.Windows.Forms.Label()
            CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.pbClose, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'labelTitle
            '
            Me.labelTitle.BackColor = System.Drawing.Color.DarkGray
            Me.labelTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.labelTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.labelTitle.Location = New System.Drawing.Point(0, 0)
            Me.labelTitle.Name = "labelTitle"
            Me.labelTitle.Size = New System.Drawing.Size(366, 21)
            Me.labelTitle.TabIndex = 0
            Me.labelTitle.Text = "title goes here"
            '
            'labelBody
            '
            Me.labelBody.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.labelBody.BackColor = System.Drawing.Color.Transparent
            Me.labelBody.Cursor = System.Windows.Forms.Cursors.Hand
            Me.labelBody.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.labelBody.Location = New System.Drawing.Point(11, 28)
            Me.labelBody.Name = "labelBody"
            Me.labelBody.Size = New System.Drawing.Size(342, 84)
            Me.labelBody.TabIndex = 1
            Me.labelBody.Text = "Body goes here and here and here and here and here"
            '
            'pbIcon
            '
            Me.pbIcon.Location = New System.Drawing.Point(10, 140)
            Me.pbIcon.Name = "pbIcon"
            Me.pbIcon.Size = New System.Drawing.Size(62, 63)
            Me.pbIcon.TabIndex = 2
            Me.pbIcon.TabStop = False
            '
            'pbClose
            '
            Me.pbClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pbClose.Image = Global.spc_alert_notifications.My.Resources.Resources.close_icon
            Me.pbClose.Location = New System.Drawing.Point(336, 0)
            Me.pbClose.Name = "pbClose"
            Me.pbClose.Size = New System.Drawing.Size(19, 21)
            Me.pbClose.TabIndex = 3
            Me.pbClose.TabStop = False
            '
            'lblLink
            '
            Me.lblLink.AutoSize = True
            Me.lblLink.Location = New System.Drawing.Point(5, 89)
            Me.lblLink.Name = "lblLink"
            Me.lblLink.Size = New System.Drawing.Size(0, 13)
            Me.lblLink.TabIndex = 4
            Me.lblLink.Visible = False
            '
            'frmNotification
            '
            Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
            Me.BackColor = System.Drawing.Color.Gainsboro
            Me.ClientSize = New System.Drawing.Size(366, 121)
            Me.ControlBox = False
            Me.Controls.Add(Me.lblLink)
            Me.Controls.Add(Me.pbClose)
            Me.Controls.Add(Me.pbIcon)
            Me.Controls.Add(Me.labelBody)
            Me.Controls.Add(Me.labelTitle)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "frmNotification"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Notification"
            Me.TopMost = True
            CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.pbClose, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private WithEvents lifeTimer As Timer
        Private components As System.ComponentModel.IContainer
        Private WithEvents labelTitle As Label
        Private WithEvents labelBody As Label
        Private WithEvents pbIcon As PictureBox
        Private WithEvents lblLink As Label
        Private WithEvents pbClose As PictureBox


#End Region
    End Class
End Namespace