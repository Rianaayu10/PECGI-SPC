<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNotification
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lifeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.labelTitle = New System.Windows.Forms.Label()
        Me.labelBod = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'labelTitle
        '
        Me.labelTitle.BackColor = System.Drawing.Color.Gray
        Me.labelTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.labelTitle.Font = New System.Drawing.Font("Calibri", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.labelTitle.Location = New System.Drawing.Point(0, 0)
        Me.labelTitle.Name = "labelTitle"
        Me.labelTitle.Size = New System.Drawing.Size(366, 21)
        Me.labelTitle.TabIndex = 0
        Me.labelTitle.Text = "title goes here"
        '
        'labelBod
        '
        Me.labelBod.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.labelBod.BackColor = System.Drawing.Color.Transparent
        Me.labelBod.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.labelBod.Location = New System.Drawing.Point(90, 36)
        Me.labelBod.Name = "labelBod"
        Me.labelBod.Size = New System.Drawing.Size(268, 77)
        Me.labelBod.TabIndex = 1
        Me.labelBod.Text = "Body goes here and here and here and here and here"
        '
        'frmNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(366, 122)
        Me.ControlBox = False
        Me.Controls.Add(Me.labelBod)
        Me.Controls.Add(Me.labelTitle)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmNotification"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "frmNotification"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lifeTimer As Timer
    Friend WithEvents labelTitle As Label
    Friend WithEvents labelBod As Label
End Class
