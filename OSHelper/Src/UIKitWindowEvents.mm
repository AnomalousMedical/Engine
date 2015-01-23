//
//  UIKitWindowEvents.m
//  OSHelper
//
//  Created by Andrew Piper on 1/23/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#include "StdAfx.h"
#include "UIKitWindowEvents.h"
#include "UIKitWindow.h"
#include "MultiTouch.h"

@implementation UIKitWindowEvents

- (id) initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self)
    {
        allowFirstResponder = false;
        hasText = false;
    }
    return self;
}

-(void) setWindow:(UIKitWindow*) window
{
    win = window;
    orientationEvents = [[UIKitOrientationEvents alloc] init:window];
}

-(void) setMultitouch:(MultiTouch*) multiTouch
{
    touch = multiTouch;
}

- (void)sendEvent:(UIEvent *)event {
    
    if (event.type == UIEventTypeTouches)
    {
        TouchInfo touchInfo;
        for(UITouch * t in [event allTouches])
        {
            CGPoint loc = [t locationInView:self];
            touchInfo.pixelX = loc.x;
            touchInfo.pixelY = loc.y;
            switch (t.phase)
            {
                case UITouchPhaseBegan:
                    logger << "Simple Touch Begin " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << (uintptr_t)t << NativeLog::DebugFlush();
                    break;
                case UITouchPhaseEnded:
                    logger << "Simple Touch End " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << (uintptr_t)t << NativeLog::DebugFlush();
                    break;
                case UITouchPhaseMoved:
                    logger << "Simple Touch Moved " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << (uintptr_t)t << NativeLog::DebugFlush();
                    break;
                case UITouchPhaseCancelled:
                    logger << "Touches canceled " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << (uintptr_t)t << NativeLog::DebugFlush();
                    break;
                default:
                    break;
            }
        }
    }
    
    [super sendEvent:event];
}

- (void)insertText:(NSString *)text
{
    logger.sendMessage("Text entered", LogLevel::ImportantInfo);
    // Do something with the typed character
}
- (void)deleteBackward
{
    logger.sendMessage("delete backward", LogLevel::ImportantInfo);
    // Handle the delete key
}
- (BOOL)hasText
{
    // Return whether there's any text present
    return hasText;
}
- (BOOL)canBecomeFirstResponder
{
    return allowFirstResponder;
}

@end