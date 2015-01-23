//
//  UIKitOrientationEvents.m
//  OSHelper
//
//  Created by Andrew Piper on 1/23/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#import <Foundation/Foundation.h>
#include "StdAfx.h"
#include "UIKitOrientationEvents.h"
#include "UIKitWindow.h"

@implementation UIKitOrientationEvents

- (void) orientationChanged:(NSNotification *)note
{
    win->fireSized();
}

-(id) init:(UIKitWindow*) window
{
    if (self)
    {
        win = window;
        
        //For Screen Rotation
        [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
        [[NSNotificationCenter defaultCenter]
         addObserver:self selector:@selector(orientationChanged:)
         name:UIDeviceOrientationDidChangeNotification
         object:[UIDevice currentDevice]];
    }
    
    return self;
}

@end