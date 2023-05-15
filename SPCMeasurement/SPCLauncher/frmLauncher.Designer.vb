<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLauncher
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLauncher))
        Me.lblCommand = New System.Windows.Forms.Label()
        Me.lblErr = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblCommand
        '
        Me.lblCommand.Location = New System.Drawing.Point(12, 44)
        Me.lblCommand.Name = "lblCommand"
        Me.lblCommand.Size = New System.Drawing.Size(390, 28)
        Me.lblCommand.TabIndex = 0
        Me.lblCommand.Text = "args"
        '
        'lblErr
        '
        Me.lblErr.ForeColor = System.Drawing.Color.Red
        Me.lblErr.Location = New System.Drawing.Point(12, 9)
        Me.lblErr.Name = "lblErr"
        Me.lblErr.Size = New System.Drawing.Size(390, 35)
        Me.lblErr.TabIndex = 0
        Me.lblErr.Text = "Error"
        '
        'frmLauncher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(414, 81)
        Me.Controls.Add(Me.lblErr)
        Me.Controls.Add(Me.lblCommand)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLauncher"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SPC Launcher"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblCommand As Label
    Friend WithEvents lblErr As Label
End Class
