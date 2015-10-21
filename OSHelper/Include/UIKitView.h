//
//  UIKitView.h
//  OSHelper
//
//  Created by Andrew Piper on 2/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitView_h
#define OSHelper_UIKitView_h

#import <UIKit/UIKit.h>

class UIKitWindow;

@interface UIKitView : UIView
{
@private
    UIKitWindow *win;
    bool pendingResize;
    CGRect newFrame;
    bool viewVisible;
    bool applicationActive;
}

-(void) setWindow:(UIKitWindow*) window;

-(void) firePendingResize;

-(void) cancelResize;

-(void) setViewVisible:(bool) visible;

-(void) setApplicationActive:(bool) active;

-(void) setFrameForNextUpdate:(CGRect) frame;

-(bool) safeToRender;

@end

#endif
