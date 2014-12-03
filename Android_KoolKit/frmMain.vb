Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class frmMain
    Dim notweaks As String = Nothing
    Dim noinit As String = Nothing
    Dim adb As String = "files\adb_tools\adb.exe"
    Dim shell As String = "shell" & " su -c"
    Private Sub btn_Click(sender As Object, e As EventArgs) Handles btnBloat.Click, btnInitD.Click, btnBootani.Click, btnTweaks.Click, btnReboot.Click, btnExit.Click
        Dim btn As Button = sender

        Try
            Select Case btn.Name
                Case "btnBloat"
				if lblModel.Text = "SM-T217A" then 
                    Dim sr As New StreamReader("files\bloat\ATT\nobloat.txt")
				else if lblModel.Text = "SM-T217T" then
					Dim sr As New StreamReader("files\bloat\TMO\nobloat.txt")
				end if
'                    ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
                    While sr.Peek() <> -1
                        Dim srLine As String = sr.ReadLine()
                        If srLine.Substring(0, 1) = "/" Then
                            ProcStart(adb, shell & " rm -fR " & srLine)
                        End If
                    End While
                    sr.Close()
					if lblModel.Text = "SM-T217A" then
						ProcStart(adb, shell & " rm -f /system/app/SecSettings.apk")
						ProcStart(adb, shell & " rm -f /system/app/SecSettings.odex")
						ProcStart(adb, "push" & " files\bloat\ATT\SecSettings.apk /sdcard/SecSettings.apk")
						ProcStart(adb, shell & " cp -f /sdcard/SecSettings.apk /system/app/SecSettings.apk")
						ProcStart(adb, shell & " chmod 0644 /system/app/SecSettings.apk")
					else if lblModel.Text = "SM-T217T" then
						msgbox("Tmobile devices not yet supported")
					end if	
'						ProcStart(adb, shell & " mount -o remount,ro -t auto /system")					
                Case "btnInitD"
                    If noinit <> "" Then
                        MsgBox("Your system already has init.d support enabled, you don't need to run this again")
                    Else
'                        ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
                        ProcStart(adb, "push" & " files\bin\sysinit /sdcard/sysinit")
                        ProcStart(adb, shell & " cp -f /sdcard/sysinit /system/bin/sysinit")
                        ProcStart(adb, shell & " mkdir /system/etc/init.d")
                        ProcStart(adb, "push" & " files\etc\init.qcom.post_boot.sh /sdcard/init.qcom.post_boot.sh")
                        ProcStart(adb, shell & " cp -f /sdcard/init.qcom.post_boot.sh /system/etc/init.qcom.post_boot.sh")
                        ProcStart(adb, shell & " chmod 0755 /system/etc/init.qcom.post_boot.sh")
                        ProcStart(adb, shell & " chmod -R 0755 /system/etc/init.d")
                        ProcStart(adb, shell & " chmod 0755 /system/bin/sysinit")
                    End If
'					ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
                Case "btnBootani" 'Not available for all models
'                    ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
                    ProcStart(adb, shell & " cp -f /system/bin/bootanimation /system/bin/bootanimation.bak")
                    ProcStart(adb, "push" & " files\bin\bootanimation /sdcard/bootanimation")
                    ProcStart(adb, "push" & " files\media\bootanimation.zip /sdcard/bootanimation.zip")
                    ProcStart(adb, shell & " cp -f /sdcard/bootanimation /system/bin/bootanimation")
                    ProcStart(adb, shell & " cp -f /sdcard/bootanimation.zip /system/media/bootanimation.zip")
                    ProcStart(adb, shell & " cp -f /system/media/audio/ui/PowerOn.ogg /system/media/audio/ui/PowerOn.ogg.bak")
                    ProcStart(adb, shell & " chmod 0644 /system/media/bootanimation.zip")
                    ProcStart(adb, shell & " chmod 0755 /system/bin/bootanimation")
'                    ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
                Case "btnTweaks"
                    If notweaks <> "" Then
                        If MessageBox.Show("Performance enhancements have already been applied, do you want to update them now?", "Close", _
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
                                       = Windows.Forms.DialogResult.Yes Then
'                            ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
                            'REMOVE EXISTING TWEAKS START
                            ProcStart(adb, shell & " rm -f /system/etc/sysctl.conf")
                            ProcStart(adb, shell & " rm -Rf /system/etc/init.d")
                            ProcStart(adb, shell & " cp -f /system/build.prop.backup /system/build.prop")
                            ProcStart(adb, shell & " rm -f /system/build.prop.backup")
                            'REMOVE EXISTING TWEAKS END
                            tweaks()
                        ElseIf Windows.Forms.DialogResult.No Then
                            MsgBox("You have chosen not to update/re-apply the system performance enhancements, (no changes made)")
                        End If
                    Else
                        tweaks()

                    End If

                Case "btnReboot"
                    RebootOptions.Show()
                Case "btnExit"
                    Application.Exit()
            End Select
        Catch ex As Exception
            MsgBox("An error occured please close the application and try again.")
        End Try
    End Sub

    Private Sub FileMenuItem_Click(sender As Object, e As EventArgs) Handles FileMenuItem.Click, CloseToolStripMenuItem.Click, _
                               InstallSystemToolStripMenuItem.Click, InstallUserToolStripMenuItem.Click, InstallBusyboxToolStripMenuItem.Click, _
                               editBloatToolStripMenuItem.Click, editBuildPropToolstripMenuItem.Click, editSysctlToolStripMenuItem.Click, _
                               RootYourDeviceToolStripMenuItem.Click

        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim iFileDialog As New OpenFileDialog()
        iFileDialog.InitialDirectory = FileSystem.CurDir
        iFileDialog.RestoreDirectory = True
        Dim fName As String = Nothing
        Dim cName As String = Nothing

        Try
            Select Case mnu.Name.ToString
                Case "editBloatToolStripMenuItem"
				if lblModel.Text = "SM-T217A" then 
                    ProcStart("files\adb_tools\notepad++.exe", "files\bloat\ATT\nobloat.txt", 1)
				else if lblModel.Text = "SM-T217T" then
					ProcStart("files\adb_tools\notepad++.exe", "files\bloat\TMO\nobloat.txt", 1)
				End If
                Case "editBuildPropToolstripMenuItem"
                    ProcStart("files\adb_tools\notepad++.exe", "files\tweaks\build_prop.txt", 1)
                Case "editSysctlToolStripMenuItem"
                    ProcStart("files\adb_tools\notepad++.exe", "files\sysctl\sysctl.conf", 1)
                Case "CloseToolStripMenuItem"
                    Application.Exit()
                Case "InstallSystemToolStripMenuItem"
                    iFileDialog.ShowDialog()
                    fName = iFileDialog.FileName
                    cName = iFileDialog.SafeFileName
                    installApk("System", fName, cName)
                Case "InstallUserToolStripMenuItem"
                    iFileDialog.ShowDialog()
                    fName = iFileDialog.FileName
                    cName = iFileDialog.SafeFileName
                    installApk("User", fName, cName)
                Case "InstallBusyboxToolStripMenuItem"
                    installApk("Busybox", "busybox", "busybox")
                Case "RootYourDeviceToolStripMenuItem"
                    ProcStart("files\Kingo_ROOT\Kingo_Root.exe")
            End Select

        Catch ex As Exception
            MsgBox("An error occured please close the application and try again.")
        End Try

    End Sub

    Private Sub NativeMethods_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If MessageBox.Show("Are you sure to close this application?", "Close", _
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
                       = Windows.Forms.DialogResult.Yes Then
            ProcStart(adb, shell & " rm -f /sdcard/bootanimation")
            ProcStart(adb, shell & " rm -f /sdcard/bootanimation.zip")
            ProcStart(adb, shell & " rm -f /sdcard/sysctl.conf")
            ProcStart(adb, shell & " rm -f /sdcard/init.qcom.post_boot.sh")
            ProcStart(adb, shell & " rm -f /sdcard/customer.xml")
            ProcStart(adb, shell & " rm -r /sdcard/xbin")
            ProcStart(adb, shell & " rm -r /sdcard/sysinit")
            ProcStart(adb, shell & " rm -r /sdcard/init.d")
            ProcStart(adb, shell & " rm -f /sdcard/set_build.sh")
            ProcStart(adb, shell & " rm -f /sdcard/SecSettings.apk")
            ProcStart(adb, shell & " rm -r /sdcard/tmp")
            ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
        Else
            e.Cancel = True
        End If

    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, _
    ByVal e As System.EventArgs) Handles MyBase.Load
        frmReadme.Show()
        Try
            ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
            ProcStart(adb, "pull" & " /system/build.prop")
            ProcStart(adb, "pull" & " /system/etc/init.qcom.post_boot.sh")

            Dim sc As New StreamReader("build.prop")

            While sc.Peek() <> -1
                Dim scLine As String = sc.ReadLine()
                If scLine.Contains("ro.product.model") Then
                    lblModel.Text = scLine.Substring(17, scLine.Length - 17)
                End If
                If scLine.Contains("# Start S5 Modifications") Then
                    notweaks = "applied"
                End If
            End While
            sc.Close()
            Dim si As New StreamReader("init.qcom.post_boot.sh")
            While si.Peek() <> -1
                Dim siLine As String = si.ReadLine()
                If siLine.Contains("/system/bin/sysinit") Then
                    noinit = "applied"
                End If
            End While
            si.Close()
            My.Computer.FileSystem.DeleteFile("build.prop")
            My.Computer.FileSystem.DeleteFile("init.qcom.post_boot.sh")
        Catch ex As Exception
            MsgBox("Couldn't detect your device, Please reconnect and try again.")
            Application.Exit()
        End Try

    End Sub

    Shared Function ProcStart(ByVal cmdline As String, Optional ByVal args As String = Nothing, _
                              Optional ByVal args1 As Integer = Nothing)
        Dim psInfo As New System.Diagnostics.ProcessStartInfo(cmdline, args)
        If args = Nothing Or args1 = 1 Then
            psInfo.WindowStyle = ProcessWindowStyle.Normal
        Else
            psInfo.WindowStyle = ProcessWindowStyle.Hidden
        End If
        System.Diagnostics.Process.Start(psInfo).WaitForExit()
        Return Nothing
    End Function

    Shared Function App_Path() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory()
    End Function

    Public Function tweaks()
'        ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
        ProcStart(adb, "push" & " files\xbin /sdcard/xbin")
        ProcStart(adb, "push" & " files\scripts /sdcard/init.d")
        ProcStart(adb, "push" & " files\sysctl\sysctl.conf /sdcard/sysctl.conf")
        ProcStart(adb, "push" & " files\tweaks\customer.xml /sdcard/customer.xml")
        ProcStart(adb, shell & " cp -R /sdcard/xbin /system")
        ProcStart(adb, shell & " cp -f /sdcard/sysctl.conf /system/etc/sysctl.conf")
        ProcStart(adb, shell & " cp -f /sdcard/customer.xml /system/csc/customer.xml")
        ProcStart(adb, shell & " cp -R /sdcard/init.d /system/etc")
        ProcStart(adb, shell & " chmod 0644 /system/etc/sysctl.conf")
        ProcStart(adb, shell & " chmod 0644 /system/csc/customer.xml")
        ProcStart(adb, shell & " chmod 0755 /system/xbin/sqlite3")
        ProcStart(adb, shell & " chmod 0755 /system/xbin/tune2fs")
        ProcStart(adb, shell & " chmod 0755 /system/xbin/fstrim")
        ProcStart(adb, shell & " chmod -R 0755 /system/etc/init.d")
        ProcStart(adb, shell & " cp -f /system/build.prop /system/build.prop.backup")
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/ro.build.product.*=.*/# ro.build.product=/g' /system/build.prop" & Chr(34))
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/ro.kernel.android.checkjni.*=.*/# ro.kernel.android.checkjni=/g' /system/build.prop" & Chr(34))
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/ro.kernel.checkjni.*=.*/# ro.kernel.checkjni/g' /system/build.prop" & Chr(34))
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/dalvik.vm.heapsize=36m/# dalvik.vm.heapsize=36m/g' /system/build.prop" & Chr(34))

        'build.prop tweaks

        ProcStart(adb, "push" & " files\set_build.sh /sdcard/set_build.sh")
        ProcStart(adb, "push" & " files\tweaks/build_prop.txt /sdcard/tmp/build_prop.txt")
        ProcStart(adb, shell & " cp /sdcard/set_build.sh /system/etc/set_build.sh")
        ProcStart(adb, shell & " chmod 0755 /system/etc/set_build.sh")
        ProcStart(adb, shell & " ./system/etc/set_build.sh")
        ProcStart(adb, shell & " chmod 0644 /system/build.prop")
'        ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
        Return Nothing
    End Function

    Public Function installApk(ByVal iType As String, ByVal iFile As String, ByVal cFile As String)
'        ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
        If iType = "System" Then
            ProcStart(adb, "push" & " " & Chr(34) & iFile & Chr(34) & " /sdcard/" & cFile)
            ProcStart(adb, shell & " cp -f /sdcard/" & cFile & " /system/app/" & cFile)
            ProcStart(adb, shell & " chmod -R 0644 /system/app/" & cFile)
        ElseIf iType = "User" Then
            ProcStart(adb, "install" & " " & Chr(34) & iFile & Chr(34))
        ElseIf iType = "Busybox" Then
            ProcStart(adb, "push" & " files\busybox\" & cFile & " /sdcard/" & cFile)
            ProcStart(adb, shell & " cp -f /sdcard/" & cFile & " /system/xbin/" & cFile)
            ProcStart(adb, shell & " chmod 0755 /system/xbin/" & cFile)
            ProcStart(adb, shell & " chown 0.0 /system/xbin/" & cFile)
            ProcStart(adb, shell & " /system/xbin/" & cFile & " --install -s /system/xbin")
        End If
'        ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
        Return Nothing
    End Function

End Class
