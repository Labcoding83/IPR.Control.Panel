# IPR.Control.Panel
Hardware monitoring and some control features that runs on Windows and Linux.

## Features
* Monitor all hardware in Windows and Linux. 
* Controls Intel CPU voltage offsets over/undervolting (between -150mv and +150mv)
* Controls some Dell laptop fans
* Controls can have fixed values or (minimal) custom curve values  

## Linux support
* Install dependencies
	> sudo apt install msr-tools libgdiplus
* To start the application, cd into folder then
	> sudo dotnet IPR.Control.Panel.dll
* Fan control for Dell laptop requires
	> sudo apt install i8kutils3
	> sudo nano /etc/modprobe.d/dell-smm-hwmon.conf
	* 	Add/update the dell-smm-hwmon options then reboot 
	> options dell-smm-hwmon ignore_dmi=1 restricted=0 force=1
	

## Libraries used:
* https://github.com/LibreHardwareMonitor/LibreHardwareMonitor
* https://github.com/424778940z/bzh-windrv-dell-smm-io