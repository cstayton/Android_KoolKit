#!/system/bin/sh
#

# Check for G900A, apply board specific settings for build.prop
# Install Device specific apks and modules.
PROP=/sdcard/tmp/build_prop.txt

	while read line; do
		echo $line >> /system/build.prop
	done < $PROP

