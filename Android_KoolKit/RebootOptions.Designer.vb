<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RebootOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RebootOptions))
        Me.btnDevice = New System.Windows.Forms.Button()
        Me.btnBL = New System.Windows.Forms.Button()
        Me.btnRecovery = New System.Windows.Forms.Button()
        Me.lblDevice = New System.Windows.Forms.Label()
        Me.lblRecovery = New System.Windows.Forms.Label()
        Me.lblBL = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnDevice
        '
        Me.btnDevice.BackColor = System.Drawing.Color.Transparent
        Me.btnDevice.BackgroundImage = Global.Android_KoolKit.My.Resources.Resources._47
        Me.btnDevice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnDevice.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDevice.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnDevice.ForeColor = System.Drawing.Color.Transparent
        Me.btnDevice.Location = New System.Drawing.Point(12, 12)
        Me.btnDevice.Name = "btnDevice"
        Me.btnDevice.Size = New System.Drawing.Size(64, 64)
        Me.btnDevice.TabIndex = 0
        Me.btnDevice.UseVisualStyleBackColor = False
        '
        'btnBL
        '
        Me.btnBL.BackColor = System.Drawing.Color.Transparent
        Me.btnBL.BackgroundImage = Global.Android_KoolKit.My.Resources.Resources._5_64x64
        Me.btnBL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnBL.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBL.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnBL.ForeColor = System.Drawing.Color.Transparent
        Me.btnBL.Location = New System.Drawing.Point(12, 152)
        Me.btnBL.Name = "btnBL"
        Me.btnBL.Size = New System.Drawing.Size(64, 64)
        Me.btnBL.TabIndex = 1
        Me.btnBL.UseVisualStyleBackColor = False
        '
        'btnRecovery
        '
        Me.btnRecovery.BackColor = System.Drawing.Color.Transparent
        Me.btnRecovery.BackgroundImage = Global.Android_KoolKit.My.Resources.Resources._30_64x64
        Me.btnRecovery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRecovery.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRecovery.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnRecovery.ForeColor = System.Drawing.Color.Transparent
        Me.btnRecovery.Location = New System.Drawing.Point(13, 82)
        Me.btnRecovery.Name = "btnRecovery"
        Me.btnRecovery.Size = New System.Drawing.Size(64, 64)
        Me.btnRecovery.TabIndex = 2
        Me.btnRecovery.UseVisualStyleBackColor = False
        '
        'lblDevice
        '
        Me.lblDevice.AutoSize = True
        Me.lblDevice.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDevice.ForeColor = System.Drawing.Color.Maroon
        Me.lblDevice.Location = New System.Drawing.Point(82, 30)
        Me.lblDevice.Name = "lblDevice"
        Me.lblDevice.Size = New System.Drawing.Size(134, 24)
        Me.lblDevice.TabIndex = 3
        Me.lblDevice.Text = "Reboot Device"
        '
        'lblRecovery
        '
        Me.lblRecovery.AutoSize = True
        Me.lblRecovery.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecovery.ForeColor = System.Drawing.Color.Maroon
        Me.lblRecovery.Location = New System.Drawing.Point(83, 100)
        Me.lblRecovery.Name = "lblRecovery"
        Me.lblRecovery.Size = New System.Drawing.Size(156, 24)
        Me.lblRecovery.TabIndex = 4
        Me.lblRecovery.Text = "Reboot Recovery"
        '
        'lblBL
        '
        Me.lblBL.AutoSize = True
        Me.lblBL.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBL.ForeColor = System.Drawing.Color.Maroon
        Me.lblBL.Location = New System.Drawing.Point(83, 170)
        Me.lblBL.Name = "lblBL"
        Me.lblBL.Size = New System.Drawing.Size(167, 24)
        Me.lblBL.TabIndex = 5
        Me.lblBL.Text = "Reboot Bootloader"
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Transparent
        Me.btnExit.BackgroundImage = Global.Android_KoolKit.My.Resources.Resources._stop
        Me.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnExit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnExit.ForeColor = System.Drawing.Color.Transparent
        Me.btnExit.Location = New System.Drawing.Point(247, 197)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(32, 32)
        Me.btnExit.TabIndex = 6
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'RebootOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(288, 239)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblBL)
        Me.Controls.Add(Me.lblRecovery)
        Me.Controls.Add(Me.lblDevice)
        Me.Controls.Add(Me.btnRecovery)
        Me.Controls.Add(Me.btnBL)
        Me.Controls.Add(Me.btnDevice)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RebootOptions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reboot Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnDevice As System.Windows.Forms.Button
    Friend WithEvents btnBL As System.Windows.Forms.Button
    Friend WithEvents btnRecovery As System.Windows.Forms.Button
    Friend WithEvents lblDevice As System.Windows.Forms.Label
    Friend WithEvents lblRecovery As System.Windows.Forms.Label
    Friend WithEvents lblBL As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
End Class
