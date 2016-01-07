//
//  ViewController.h
//  SuperTinyIOSApp
//
//  Created by Andrew Piper on 1/16/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#pragma once

#import <UIKit/UIKit.h>
#include "UIKitView.h"
#include "NativeOSWindow.h"

class UIKitWindow;

@interface UIKitViewController : UIViewController
{
@private
    UIKitWindow* win;
    UIKitView* uiKitView;
}

-(void) setWindow:(UIKitWindow*) window;

-(void) firePendingResize;

-(void) fireCreateInternalResources:(InternalResourceType) resourceType;

-(void) fireDestroyInternalResources:(InternalResourceType) resourceType;

-(void) cancelResize;

-(void) setApplicationActive:(bool) active;

-(bool) safeToRender;

-(UIKitView*) getUIKitView;

@end

