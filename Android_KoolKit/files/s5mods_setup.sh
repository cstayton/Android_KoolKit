#!/system/bin/sh

#Enabler script for S5 Tab4 Tweaks
#Installs busybox if missing
#Creates sysinit and init.d structure
#copies needed files in place for
#proper execution of modifications.
#Option to de-bloat included
#Option to enable custom bootanimation included

BBOX_DIR=busybox
BIN_DIR=xbin
BLOAT=bloat/nobloat.txt
SYSBIN_DIR=bin
M_DIR=media
XBIN_DIR=/system/xbin
SCRIPTS=scripts
SYSCTL=sysctl

error_msg(){
echo "You're all set, you do not need this mod..."
sleep 1
echo ""
echo "If you are reapplying, please delete these files if present:"
echo "/system/build.prop.backup"
echo "/system/bin/sysinit"
echo "/system/etc/install-recovery.sh"
echo "/system/etc/install-recovery-2.sh"
echo "And run again..."
sleep 2
echo "Aborting..."
mount -o remount,ro -t auto /system
echo ""
echo "cstayton @ XDA"
exit 1
}

error_msg1(){
echo "Uh Oh there was a problem applying the modifications..."
sleep 1
echo "Is your device Rooted?"
echo "Did you run this script per instructions in my thread?"
echo "Did you reboot your device twice before checking results?"
echo ""
sleep 2
echo "Aborting..."
mount -o remount,ro -t auto /system
echo ""
echo "cstayton @ XDA"
exit 1
}

clear
echo "|=======================================|"
echo "| These modifications are intended for  |"
echo "| devices running stock ROMs only. If   |"
echo "| you are running a CM nightly or other |"
echo "| custom AOSP ROM these mods are not    |"
echo "| are not required.                     |"
echo "|=======================================|"
echo -n "Are you on a stock ROM? (y/n)"
read stock_rom

if [ "$stock_rom" != "n" ]
	then
	sleep 1
	echo ""
	echo "You have chosen to install these modifications "
	echo "on a stock ROM, we will now proceed."
  else
  echo ""
	echo "You have chosen not to proceed since you are "
	echo "not running a stock ROM."
	sleep 2
	exit 0
fi

echo "Mounting system as rewritable..."
mount -o remount,rw -t auto /system
echo "Switching to working folder..."
echo ""
cd /sdcard/s5mods
sleep 1

if [ -f /data/local/init.log ]; then
	echo "|==================================================|"
	echo "| These modifications have already been completed  |"
	echo "| on your device, If you are reapplying, please    |"
	echo "| delete these files if present:                   |"
	echo "| /system/build.prop.backup                        |"
	echo "| /system/bin/sysinit                              |"
	echo "| /system/etc/install-recovery.sh                  |"
	echo "| /system/etc/install-recovery-2.sh                |"
	echo "| /data/local/init.log                             |"
	echo "| /data/local/S5_Memory_Recovery.log               |"
	echo "| And run again...                                 |"
	echo "|==================================================|"
	sleep 1
	echo -n "Press enter to continue..."
	read enterKey
	sleep 1
else
	echo "Enable init.d & install init.d scripts"
	echo "Check for needed files and install/copy if missing"
	echo ""
	sleep 1

	id=`id`;
	id=`echo ${id#*=}`;
	id=`echo ${id%%\(*}`;
	id=`echo ${id%% *}`
	if [ "$id" != "0" ] && [ "$id" != "root" ]; then
		echo "Script NOT running as root!"
		echo "Superuser access not granted!"
		echo "Please type 'su' first before running this script..."
		sleep 2
		exit 1
	else
		echo "Super User access detected :P"
		echo "Now running as root"
		echo ""
		sleep 1
	fi

	if [ ! "`which busybox`" ]; then
		echo "busybox NOT INSTALLED!"
		sleep 1
		echo "Now installing busybox."
		cp -f $BBOX_DIR/busybox $XBIN_DIR/busybox
		chmod 0755 $XBIN_DIR/busybox
		chown 0.0 $XBIN_DIR/busybox
		$XBIN_DIR/busybox --install -s $XBIN_DIR
	else
		echo "busybox already installed."
		sleep 1
	fi

	if [ ! "`which fstrim`" ]; then
		echo "fstrim NOT INSTALLED!"
		sleep 1
		echo "Now installing fstrim."
		cp -f $BIN_DIR/fstrim $XBIN_DIR/fstrim
		chmod 0755 $XBIN_DIR/fstrim
		chown 0.0 $XBIN_DIR/fstrim
	else
		echo "fstrim already installed."
		sleep 1
	fi

	if [ ! "`which sqlite3`" ]; then
		echo "sqlite3 NOT INSTALLED!"
		sleep 1
		echo "Now installing sqlite3."
		cp -f $BIN_DIR/sqlite3 $XBIN_DIR/sqlite3
		chmod 0755 $XBIN_DIR/sqlite3
		chown 0.0 $XBIN_DIR/sqlite3
	else
		echo "sqlite3 already installed."
		sleep 1
	fi

	if [ ! "`which tune2fs`" ]; then
		echo "tune2fs NOT INSTALLED!"
		sleep 1
		echo "Now installing tune2fs."
		cp -f $BIN_DIR/tune2fs $XBIN_DIR/tune2fs
		chmod 0755 $XBIN_DIR/tune2fs
		chown 0.0 $XBIN_DIR/tune2fs
	else
		echo "tune2fs already installed."
		sleep 1
	fi

	bbb=0

	if [ ! "`which grep`" ]; then
		bbb=1
		echo "grep applet NOT FOUND!"
		sleep 1
	else
		echo "Awesome! grep found! :D"
		sleep 1
	fi

	if [ ! "`which run-parts`" ]; then
		bbb=1
		echo "run-parts applet NOT FOUND!"
		sleep 1
	else
		echo "Good! run-parts found! :)"
		echo ""
		sleep 1
	fi

	if [ $bbb -eq 1 ] ; then
		echo ""
		echo "Required applets are NOT FOUND!"
		echo ""
		sleep 1
		error_msg1
	fi

	###All required files found or installed/copied in place###

	echo "Great! Let's proceed..."
	echo ""
	sleep 1
	echo -n "Press enter to continue..."
	read enterKey
	sleep 1

	echo "Now lets add some tweaks and modifications to build.prop"
	echo "If the tweaks already exist I'll just skip this part."
	sleep 1
	if [ -f /system/build.prop ]; then
		if [ -z "`cat /system/build.prop | grep "# Start S5 Modifications"`" ]; then
				cp -f /system/build.prop /system/build.prop.backup
				echo  "Correcting a few build.prop errors first..."
				sed -i 's/ro.build.product.*=.*/# ro.build.product=/g' /system/build.prop
				sed -i 's/ro.ril.hsxpa.*=.*/# ro.ril.hsxpa=/g' /system/build.prop
				sed -i 's/ro.kernel.android.checkjni.*=.*/# ro.kernel.android.checkjni=/g' /system/build.prop	
				sed -i 's/ro.kernel.checkjni.*=.*/# ro.kernel.checkjni/g' /system/build.prop				
				sleep 2
				echo  "Applying build.prop modifications"
				echo  "# Start S5 Modifications" >> /system/build.prop
				echo  "ro.config.hw_menu_unlockscreen=false" >> /system/build.prop
				echo  "persist.sys.use_dithering=0" >> /system/build.prop
				echo  "persist.sys.purgeable_assets=1" >> /system/build.prop
				echo  "dalvik.vm.dexopt-flags=m=y" >> /system/build.prop
				echo  "ro.mot.eri.losalert.delay=1000" >> /system/build.prop
				echo  "ro.kernel.android.checkjni=0" >> /system/build.prop
				echo  "ro.kernel.checkjni=0" >> /system/build.prop
				echo  "ro.media.dec.jpeg.memcap=12000000" >> /system/build.prop
				echo  "ro.media.enc.hprof.vid.bps=12000000" >> /system/build.prop
				echo  "debug.performance.tuning=1" >> /system/build.prop
				echo  "video.accelerate.hw=1" >> /system/build.prop
				echo  "pm.sleep_mode=1" >> /system/build.prop
				echo  "ro.ril.disable.power.collapse=0" >> /system/build.prop
				echo  "windowsmgr.max_events_per_sec=150" >> /system/build.prop
				echo  "dalvik.vm.heapgrowthlimit=96m" >> /system/build.prop
				echo  "wifi.supplicant_scan_interval=120" >> /system/build.prop
				echo  "ro.ril.hsdpa.category=14" >> /system/build.prop
				echo  "ro.ril.hsupa.category=6" >> /system/build.prop
				echo  "ro.ril.hsxpa=4" >> /system/build.prop
				echo  "# End S5 Modifications" >> /system/build.prop
				chmod 0644 /system/build.prop
			else
				echo "S5 Modifications already exist, nothing to do."
			fi
		fi
	sleep 1

	echo ""
	echo "|=============================================================|"
	echo "| Checking for the presence of sysinit in /system/bin...      |"
	echo "| I'll create it for you if it doesn't exist.                 |"
	echo "| If it does exist, I'll make sure it has everything we need. |"
	echo "|=============================================================|"
	echo ""
	sleep 1
	if [ -e /system/bin/sysinit ]; then
		echo "sysinit found..."
		if [ -z "`cat /system/bin/sysinit | grep "init.d"`" ]; then
			echo "Adding lines to sysinit..."
			echo "" >> /system/bin/sysinit
			echo "# init.d support" >> /system/bin/sysinit
			echo "" >> /system/bin/sysinit
			echo "export PATH=/sbin:/system/sbin:/system/bin:/system/xbin" >> /system/bin/sysinit
			echo "if [ -w /system ] ; then" >> /system/bin/sysinit
			echo "	echo "'/system is already Read/Write'" $(date) > /data/local/init.log" >> /system/bin/sysinit
			echo "	$L "'/system is already Read/Write'" $(date)" >> /system/bin/sysinit
			echo "else" >> /system/bin/sysinit
			echo "	echo "'Applying Read/Write access to /system'" $(date) > /data/local/init.log" >> /system/bin/sysinit
			echo "	$L "'Applying Read/Write access to /system'" $(date)" >> /system/bin/sysinit
			echo "	mount -o remount,rw -t auto /system" >> /system/bin/sysinit
			echo "fi" >> /system/bin/sysinit
			echo "run-parts /system/etc/init.d" >> /system/bin/sysinit
			echo "" >> /system/bin/sysinit
		else
			echo ""
			echo "Your sysinit should already be running the scripts in init.d folder at boot..."
			error_msg
		fi
	else
		echo "sysinit not found, creating file..."
		echo "#!/system/bin/sh" > /system/bin/sysinit
		echo "# init.d support" >> /system/bin/sysinit
		echo "" >> /system/bin/sysinit
		echo "export PATH=/sbin:/system/sbin:/system/bin:/system/xbin" >> /system/bin/sysinit
		echo ""
		echo "if [ -w /system ] ; then" >> /system/bin/sysinit
		echo "	echo "'/system is already Read/Write'" $(date) > /data/local/init.log" >> /system/bin/sysinit
		echo "	$L "'/system is already Read/Write'" $(date)" >> /system/bin/sysinit
		echo "else" >> /system/bin/sysinit
		echo "	echo "'Applying Read/Write access to /system'" $(date) > /data/local/init.log" >> /system/bin/sysinit
		echo "	$L "'Applying Read/Write access to /system'" $(date)" >> /system/bin/sysinit
		echo "	mount -o remount,rw -t auto /system" >> /system/bin/sysinit
		echo "fi" >> /system/bin/sysinit
		echo "run-parts /system/etc/init.d" >> /system/bin/sysinit
		echo "" >> /system/bin/sysinit
	fi
	sleep 1

	echo "Setting correct permissions and ownership for sysinit..."
	chmod 0755 /system/bin/sysinit
	chown 0.2000 /system/bin/sysinit

	sleep 1
	echo ""
	echo "Checking for the presence of install-recovery.sh..."
	sleep 1
	if [ -f /system/etc/install-recovery.sh ] && [ -z "`cat /system/etc/install-recovery.sh | grep "daemon"`" ]; then
		if [ ! -z "`cat /system/etc/install-recovery.sh | grep "init.d"`" ];then
			echo "Your install-recovery.sh seems to be already modified for init.d..."
			error_msg
		fi
		echo "install-recovery.sh found, renaming it as install-recovery-2.sh..."
		mv /system/etc/install-recovery.sh /system/etc/install-recovery-2.sh
		echo "Recreating install-recovery.sh..."
		echo "#!/system/bin/sh" > /system/etc/install-recovery.sh
		echo "# init.d support" >> /system/etc/install-recovery.sh
		echo "" >> /system/etc/install-recovery.sh
		echo "/system/bin/sysinit" >> /system/etc/install-recovery.sh
		echo "" >> /system/etc/install-recovery.sh
		echo "# excecuting extra commands" >> /system/etc/install-recovery.sh
		echo "/system/etc/install-recovery-2.sh" >> /system/etc/install-recovery.sh
		echo "" >> /system/etc/install-recovery.sh
	elif [ -f /system/etc/install-recovery.sh ] && [ ! -z "`cat /system/etc/install-recovery.sh | grep "daemon"`" ]; then
		if [ -f /system/etc/install-recovery-2.sh ] && [ ! -z "`cat /system/etc/install-recovery-2.sh | grep "init.d"`" ];then
			echo "Your install-recovery-2.sh seems to be already modified for init.d..."
			error_msg
		fi
		echo "install-recovery.sh is used for superuser, using install-recovery-2.sh instead..."
		if [ -f /system/etc/install-recovery-2.sh ]; then
			echo "" >> /system/etc/install-recovery-2.sh
			echo "# init.d support" >> /system/etc/install-recovery-2.sh
			echo "/system/bin/sysinit" >> /system/etc/install-recovery-2.sh
			echo "" >> /system/etc/install-recovery-2.sh
		else
			echo "#!/system/bin/sh" > /system/etc/install-recovery-2.sh
			echo "# init.d support" >> /system/etc/install-recovery-2.sh
			echo "" >> /system/etc/install-recovery-2.sh
			echo "/system/bin/sysinit" >> /system/etc/install-recovery-2.sh
			echo "" >> /system/etc/install-recovery-2.sh
		fi
		if [ -z "`cat /system/etc/install-recovery.sh | grep "install-recovery-2.sh"`" ]; then
			echo "" >> /system/etc/install-recovery.sh
			echo "# extra commands" >> /system/etc/install-recovery.sh
			echo "/system/etc/install-recovery-2.sh" >> /system/etc/install-recovery.sh
			echo "" >> /system/etc/install-recovery.sh
		fi
	else
		echo "install-recovery.sh not found, creating it..."
		echo "#!/system/bin/sh" > /system/etc/install-recovery.sh
		echo "# init.d support" >> /system/etc/install-recovery.sh
		echo "" >> /system/etc/install-recovery.sh
		echo "/system/bin/sysinit" >> /system/etc/install-recovery.sh
		echo "" >> /system/etc/install-recovery.sh
	fi

	sleep 1
	echo "Setting the correct permissions and ownership for "
	echo "install-recovery.sh Also for install-recovery-2.sh "
	echo "if it exists..."
	chmod 0755 /system/etc/install-recovery.sh
	chown 0.0 /system/etc/install-recovery.sh
	if [ -f /system/etc/install-recovery-2.sh ]; then
		chmod 0755 /system/etc/install-recovery-2.sh
		chown 0.0 /system/etc/install-recovery-2.sh
	fi
	sleep 1

	echo ""
	echo "Checking for the presence of the init.d folder..."
	sleep 1
	if [ -d /system/etc/init.d ]; then
		echo "init.d folder found..."
	else
		echo "init.d folder not found, creating the folder..."
		mkdir /system/etc/init.d
	fi
	sleep 1

	echo ""
	echo "Copying S5_Mods and init.d scripts..."
	cp -f $SYSCTL/sysctl.conf /system/etc/sysctl.conf
	cp -f $SCRIPTS/01sysctl /system/etc/init.d/01sysctl
	cp -f $SCRIPTS/02misctweaks /system/etc/init.d/02misctweaks
	cp -f $SCRIPTS/03memory_clean /system/etc/init.d/03memory_clean
	sleep 1

	echo "Setting correct permissions and ownership for "
	echo "init.d folder and scipts..."
	chmod 0755 /system/etc/init.d
	chmod 0777 /system/etc/init.d/01sysctl
	chmod 0777 /system/etc/init.d/02misctweaks
	chmod 0777 /system/etc/init.d/03memory_clean
	chown 0.0 /system/etc/init.d
	chown 0.0 /system/etc/init.d/01sysctl
	chown 0.0 /system/etc/init.d/02misctweaks
	chown 0.0 /system/etc/init.d/03memory_clean
	sleep 1
	echo ""
	echo "Mounting system as read-only..."
	mount -o remount,ro -t auto /system
	sleep 1
	echo ""
	echo "Done!!!"
	sleep 1
	echo "Please reboot at least twice before checking /data..."
	sleep 1
	echo "If init.d is working, you will see an init.log and "
	echo "S5_Memory_Recovery.log in /data/local/..."
	sleep 3
	echo ""
	echo ""
fi

clear
echo ""
echo "Now for some fun stuff"
echo ""
echo -n "Do you want to enable custom bootanimations? (y/n)"
read do_bootani
if [ $do_bootani != "n" ]
  then
	mv /system/bin/bootanimation /system/bin/bootanimation.bak
	cp -f $SYSBIN_DIR/bootanimation /system/bin/bootanimation
	cp -f $M_DIR/bootanimation.zip /system/media/bootanimation.zip
	chmod 0755 /system/bin/bootanimation
	chown 0.0 /system/bin/bootanimation
	chmod 0644 /system/media/bootanimation.zip
	sleep 1
	echo ""
	echo "Custom bootanimation enabled,"
	echo "Default google bootanimation installed."
	echo "To install your bootanimations just "
	echo "copy bootanimation.zip to /system/media folder "
	echo "and reboot your device."
	sleep 1
  else
  echo ""
	echo "No custom bootanimations OK, Gotcha, "
	echo "your really missing out but ok..."
	echo ""
	sleep 2
fi

echo "|==================================================|"
echo "| Removing bloatware can free up much needed       |"
echo "| system resources and can help with battery       |"
echo "| life and overall performance. Keep in mind       |"
echo "| that the included nobloat.txt file handles       |"
echo "| minimal removal only, and does not uninstall     |"
echo "| updates to any of the apps listed. You will      |"
echo "| need to modify this file to fit your specific    |"
echo "| needs and manually uninstall Google Play updates |"
echo "| for the apps in order for the bloatware removal  |"
echo "| to be complete.                                  |"
echo "|==================================================|"
echo -n "Do you want to remove all bloatware from your device? (y/n)"
read do_nobloat
if [ $do_nobloat != "n" ]
  then
	while read line; do
		case $line in \#*) continue ;; esac
			rm -fR $line
	done < $BLOAT
else
	echo ""
	echo "OK, not removing any bloatware from your device,"
	echo "remember you need to reboot your device if you "
	echo "applied any of the modifications."
fi

echo ""
echo "Enjoy!!! =)"
echo "cstayton @ XDA 2014"
exit 0
