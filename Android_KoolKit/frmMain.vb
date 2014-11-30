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
    Dim adb As String = "adb_tools\adb.exe"
    Dim shell As String = "shell" & " su -c"
    Private Sub btn_Click(sender As Object, e As EventArgs) Handles btnBloat.Click, btnInitD.Click, btnBootani.Click, btnTweaks.Click, btnReboot.Click, btnExit.Click
        Dim btn As Button = sender

        Try
            Select Case btn.Name
                Case "btnBloat"
                    Dim sr As New StreamReader("bloat\nobloat.txt")
                    ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
                    While sr.Peek() <> -1
                        Dim srLine As String = sr.ReadLine()
                        If srLine.Substring(0, 1) = "/" Then
                            ProcStart(adb, shell & " rm -fR " & srLine)
                        End If
                    End While
                    sr.Close()
                    ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
                Case "btnInitD"
                    If noinit <> "" Then
                        MsgBox("Your system already has init.d support enabled, you don't need to run this again")
                    Else
                        ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
                        ProcStart(adb, "push" & " bin\sysinit /sdcard/sysinit")
                        ProcStart(adb, shell & " cp -f /sdcard/sysinit /system/bin/sysinit")
                        ProcStart(adb, shell & " mkdir /system/etc/init.d")
                        ProcStart(adb, shell & " " & Chr(34) & " echo '/system/xbin/daemonsu --auto-daemon &' >> /system/etc/install-recovery-2.sh" & Chr(34))
                        ProcStart(adb, shell & " " & Chr(34) & " echo '/system/bin/sysinit' >> /system/etc/install-recovery-2.sh" & Chr(34))
                        ProcStart(adb, shell & " chmod -R 0755 /system/etc/init.d")
                        ProcStart(adb, shell & " chmod 0755 /system/bin/sysinit")
                        ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
                    End If
                Case "btnBootani" 'Not available for all models
                    ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
                    ProcStart(adb, shell & " cp -f /system/bin/bootanimation /system/bin/bootanimation.bak")
                    ProcStart(adb, "push" & " bin\bootanimation /sdcard/bootanimation")
                    ProcStart(adb, "push" & " media\bootanimation.zip /sdcard/bootanimation.zip")
                    ProcStart(adb, shell & " cp -f /sdcard/bootanimation /system/bin/bootanimation")
                    ProcStart(adb, shell & " cp -f /sdcard/bootanimation.zip /system/media/bootanimation.zip")
                    ProcStart(adb, shell & " cp -f /system/media/audio/ui/PowerOn.ogg /system/media/audio/ui/PowerOn.ogg.bak")
                    ProcStart(adb, shell & " chmod 0644 /system/media/bootanimation.zip")
                    ProcStart(adb, shell & " chmod 0755 /system/bin/bootanimation")
                    ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
                Case "btnTweaks"
                    If notweaks <> "" Then
                        If MessageBox.Show("Performance enhancements have already been applied, do you want to update them now?", "Close", _
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
                                       = Windows.Forms.DialogResult.Yes Then
                            ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
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
                               editBloatToolStripMenuItem.Click, editBuildPropToolstripMenuItem.Click, editSysctlToolStripMenuItem.Click

        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim iFileDialog As New OpenFileDialog()
        iFileDialog.InitialDirectory = FileSystem.CurDir
        iFileDialog.RestoreDirectory = True
        Dim fName As String = Nothing
        Dim cName As String = Nothing

        Try
            Select Case mnu.Name.ToString
                Case "editBloatToolStripMenuItem"
                    ProcStart("adb_tools\notepad++.exe", "bloat\nobloat.txt", 1)
                Case "editBuildPropToolstripMenuItem"
                    ProcStart("adb_tools\notepad++.exe", "tweaks\build.prop", 1)
                Case "editSysctlToolStripMenuItem"
                    ProcStart("adb_tools\notepad++.exe", "sysctl\sysctl.conf", 1)
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
            My.Computer.FileSystem.DeleteFile("install-recovery-2.sh")
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
        ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
        ProcStart(adb, "push" & " xbin /sdcard/xbin")
        ProcStart(adb, "push" & " scripts /sdcard/init.d")
        ProcStart(adb, "push" & " sysctl\sysctl.conf /sdcard/sysctl.conf")
        ProcStart(adb, "push" & " tweaks\customer.xml /sdcard/customer.xml")
        ProcStart(adb, "push" & " etc\init.qcom.post_boot.sh /sdcard/init.qcom.post_boot.sh")
        ProcStart(adb, shell & " cp -R /sdcard/xbin /system")
        ProcStart(adb, shell & " cp -f /sdcard/sysctl.conf /system/etc/sysctl.conf")
        ProcStart(adb, shell & " cp -f /sdcard/customer.xml /system/csc/customer.xml")
        ProcStart(adb, shell & " cp -R /sdcard/init.d /system/etc")
        ProcStart(adb, shell & " cp -R /sdcard/init.qcom.post_boot.sh /system/etc/init.qcom.post_boot.sh")
        ProcStart(adb, shell & " chmod 0644 /system/etc/sysctl.conf")
        ProcStart(adb, shell & " chmod 0644 /system/csc/customer.xml")
        ProcStart(adb, shell & " chmod 0755 /system/etc/init.qcom.post_boot.sh")
        ProcStart(adb, shell & " chmod 0755 /system/xbin/sqlite3")
        ProcStart(adb, shell & " chmod 0755 /system/xbin/tune2fs")
        ProcStart(adb, shell & " chmod 0755 /system/xbin/fstrim")
        ProcStart(adb, shell & " chmod -R 0755 /system/etc/init.d")
        ProcStart(adb, shell & " cp -f /system/build.prop /system/build.prop.backup")
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/ro.build.product.*=.*/# ro.build.product=/g' /system/build.prop" & Chr(34))
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/ro.kernel.android.checkjni.*=.*/# ro.kernel.android.checkjni=/g' /system/build.prop" & Chr(34))
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/ro.kernel.checkjni.*=.*/# ro.kernel.checkjni/g' /system/build.prop" & Chr(34))
        ProcStart(adb, shell & " " & Chr(34) & " sed -i 's/dalvik.vm.heapsize=36m/# dalvik.vm.heapsize=36m/g' /system/build.prop" & Chr(34))

        Dim st As New StreamReader("tweaks\build_prop.txt")
        While st.Peek() <> -1
            Dim stLine As String = st.ReadLine()
            ProcStart(adb, shell & " " & Chr(34) & " echo " _
                          & Chr(39) & stLine & Chr(39) & " >> /system/build.prop" & Chr(34))
        End While
        st.Close()

        ProcStart(adb, shell & " chmod 0644 /system/build.prop")
        ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
        Return Nothing
    End Function

    Public Function installApk(ByVal iType As String, ByVal iFile As String, ByVal cFile As String)
        ProcStart(adb, shell & " mount -o remount,rw -t auto /system")
        If iType = "System" Then
            ProcStart(adb, "push" & " " & Chr(34) & iFile & Chr(34) & " /sdcard/" & cFile)
            ProcStart(adb, shell & " cp -f /sdcard/" & cFile & " /system/priv-app/" & cFile)
            ProcStart(adb, shell & " chmod -R 0644 /system/priv-app/" & cFile)
        ElseIf iType = "User" Then
            ProcStart(adb, "install" & " " & Chr(34) & iFile & Chr(34))
        ElseIf iType = "Busybox" Then
            ProcStart(adb, "push" & " busybox/" & cFile & " /sdcard/" & cFile)
            ProcStart(adb, shell & " cp -f /sdcard/" & cFile & " /system/xbin/" & cFile)
            ProcStart(adb, shell & " chmod 0755 /system/xbin/" & cFile)
            ProcStart(adb, shell & " chown 0.0 /system/xbin/" & cFile)
            ProcStart(adb, shell & " /system/xbin/" & cFile & " --install -s /system/xbin")
        End If
        ProcStart(adb, shell & " mount -o remount,ro -t auto /system")
        Return Nothing
    End Function

End Class
