Public Class RebootOptions
    Private Sub btn_Click(sender As Object, e As EventArgs) Handles btnDevice.Click, btnRecovery.Click, btnBL.Click, btnExit.Click
        Dim btn As Button = sender

        Try
            Select Case btn.Name
                Case "btnDevice"
                    frmMain.ProcStart(frmMain.App_Path() & "adb_tools\adb.exe", "reboot")
                    Me.Dispose()
                Case "btnRecovery"
                    frmMain.ProcStart(frmMain.App_Path() & "adb_tools\adb.exe", "reboot recovery")
                    Me.Dispose()
                Case "btnBL"
                    frmMain.ProcStart(frmMain.App_Path() & "adb_tools\adb.exe", "reboot bootloader")
                    Me.Dispose()
                Case "btnExit"
                    Me.Dispose()
            End Select
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub NativeMethods_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Me.Dispose()
    End Sub
End Class