//
//  UIKitApp.h
//  OSHelper
//
//  Created by Andrew Piper on 1/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitApp_h
#define OSHelper_UIKitApp_h

#include "App.h"

class UIKitApp : public App
{
public:
    UIKitApp();
    
    virtual ~UIKitApp();
    
    virtual void run();
    
    virtual void exit();
};

#endif
