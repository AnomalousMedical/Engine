#!/bin/sh

#Discover some stuff about where we are.
START_PATH=$(pwd)
THIS_FOLDER=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )


#Build with XCode
xcodebuild -configuration Release -sdk iphonesimulator8.1 ONLY_ACTIVE_ARCH=NO ARCHS="i386 x86_64" SHARED_PRECOMPS_DIR="$BUILD_FOLDER/SharedTmpPCH_sim"
xcodebuild -configuration Release -sdk iphoneos8.1 ONLY_ACTIVE_ARCH=NO ARCHS="armv7 armv7s arm64" SHARED_PRECOMPS_DIR="$BUILD_FOLDER/SharedTmpPCH_device"

mkdir build
mkdir build/Release

lipo -create build/Release-iphoneos/libMyGUIWrapper.a build/Release-iphonesimulator/libMyGUIWrapper.a -output build/Release/libMyGUIWrapper.a

#Finish up
cd "$START_PATH"