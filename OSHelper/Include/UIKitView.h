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
}

-(void) setWindow:(UIKitWindow*) window;

-(void) firePendingResize;

-(void) setFrameForNextUpdate:(CGRect) frame;

@end

#endif
