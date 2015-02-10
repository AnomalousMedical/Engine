#!/bin/sh

#The iOS version has to be built through this script for now.

#Discover some stuff about where we are.
START_PATH=$(pwd)
THIS_FOLDER=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

#Configuration Settings
SOLUTION_NAME=EngineiOS.sln

#Do Build
SOLUTION_PATH=$THIS_FOLDER/$SOLUTION_NAME

#cleaning doesn't work for some reason
#xbuild "$SOLUTION_PATH" /p:Configuration=Release /t:clean
xbuild "$SOLUTION_PATH" /p:Configuration=Release /p:DefineConstants="STATIC_LINK FULL_AOT_COMPILE LITE_EMBEDDED_LIB"

sh $THIS_FOLDER/BulletWrapper/iOS/AutoBuild_iOS.sh
sh $THIS_FOLDER/libRocketWrapper/iOS/AutoBuild_iOS.sh
sh $THIS_FOLDER/MyGUIWrapper/iOS/AutoBuild_iOS.sh
sh $THIS_FOLDER/OgreCWrapper/iOS/AutoBuild_iOS.sh
sh $THIS_FOLDER/OSHelper/iOS/AutoBuild_iOS.sh
sh $THIS_FOLDER/SoundWrapper/iOS/AutoBuild_iOS.sh
sh $THIS_FOLDER/Zip/iOS/AutoBuild_iOS.sh

#Finish up
cd "$START_PATH"