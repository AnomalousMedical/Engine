#!/bin/sh

#Discover some stuff about where we are.
START_PATH=$(pwd)
THIS_FOLDER=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

cd "$THIS_FOLDER"

#Build with XCode
xcodebuild -configuration Release -sdk iphoneos ONLY_ACTIVE_ARCH=NO ARCHS="arm64" SHARED_PRECOMPS_DIR="$BUILD_FOLDER/SharedTmpPCH_device"

#Finish up
cd "$START_PATH"