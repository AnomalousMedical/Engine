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
   /* NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    
    app = [CocoaIdleApplication sharedApplication];
    [app setApp:this];
    
    appDelegate = [[CocoaIdleApplicationDelegate alloc] initWithApp:app andCocoaApp:this];
    [app setDelegate:appDelegate];
    
    id menubar = [[NSMenu new] autorelease];
    id appMenuItem = [[NSMenuItem new] autorelease];
    [menubar addItem:appMenuItem];
    [app setMainMenu:menubar];
    id appMenu = [[NSMenu new] autorelease];
    id appName = @"Anomalous Medical";//[[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleName"];
    id quitTitle = [@"Quit " stringByAppendingString:appName];
    id quitMenuItem = [[[NSMenuItem alloc] initWithTitle:quitTitle action:@selector(terminate:) keyEquivalent:@"q"] autorelease];
    [appMenu addItem:quitMenuItem];
    [appMenuItem setSubmenu:appMenu];
    
    [pool drain];*/
}

UIKitApp::~UIKitApp()
{
    /*if(app)
    {
        [app release];
        app = nil;
    }
    if(appDelegate)
    {
        [appDelegate release];
        appDelegate = nil;
    }*/
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
    //[app doStopApplication];
}

//PInvoke

extern "C" _AnomalousExport UIKitApp* App_create()
{
    return new UIKitApp();
}