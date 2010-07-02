#!/bin/sh
xcodebuild -project Engine/Engine.xcodeproj -alltargets
xcodebuild -project MyGUI/MyGUI.xcodeproj -alltargets
xcodebuild -project Zip/Zip.xcodeproj -alltargets

mkdir Release
cp Engine/build/Release/libBulletWrapper.dylib Release/libBulletWrapper.dylib
cp Engine/build/Release/libOgreCWrapper.dylib Release/libOgreCWrapper.dylib
cp Engine/build/Release/libPCPlatform.dylib Release/libPCPlatform.dylib

cp MyGUI/build/Release/libMyGUIWrapper.dylib Release/libMyGUIWrapper.dylib

cp Zip/build/Release/libZip.dylib Release/libZip.dylib