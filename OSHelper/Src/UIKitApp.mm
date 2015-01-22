//
//  UIKitApp.m
//  OSHelper
//
//  Created by Andrew Piper on 1/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#import <Foundation/Foundation.h>
#include "StdAfx.h"
#include "UIKitApp.h"
#include "UIKitAppDelegate.h"

UIKitApp::UIKitApp()
{

}

UIKitApp::~UIKitApp()
{

}

void UIKitApp::run()
{
    UIKitAppDelegate_setPrimaryUIKitApp(this);
    @autoreleasepool {
        UIApplicationMain(0, nil, nil, NSStringFromClass([UIKitAppDelegate class]));
    }
}

void UIKitApp::exit()
{
    //Is there a way to exit on iOS?
}

//PInvoke

extern "C" _AnomalousExport UIKitApp* App_create()
{
    return new UIKitApp();
}