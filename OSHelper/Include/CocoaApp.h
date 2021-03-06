//
//  CocoaApp.h
//  TestOpenGL
//
//  Created by AndrewPiper on 8/30/12.
//  Copyright (c) 2012 AndrewPiper. All rights reserved.
//

#ifndef __TestOpenGL__CocoaApp__
#define __TestOpenGL__CocoaApp__

#include <Cocoa/Cocoa.h>
#include "App.h"
#import "CocoaIdleApplication.h"
#import "CocoaIdleApplicationDelegate.h"

class CocoaApp : public App
{
public:
    CocoaApp();
    
    virtual ~CocoaApp();
    
    virtual void run();
    
    virtual void exit();
    
private:
    CocoaIdleApplication *app;
    CocoaIdleApplicationDelegate *appDelegate;
};

//Determine if the app is running normally or has set the crashed flag,
//this is global for the entire application
bool isRunningNormally();

#endif /* defined(__TestOpenGL__CocoaApp__) */
