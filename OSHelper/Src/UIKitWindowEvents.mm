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
        nextNewId = 0;
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

-(int) findNextId
{
    if(availableIds.empty())
    {
        return nextNewId++;
    }
    else
    {
        int retVal = availableIds.top();
        availableIds.pop();
        return retVal;
    }
}

-(void) returnid:(int)id
{
    availableIds.push(id);
}

-(void) resetIds
{
    touchIdMap.clear();
    nextNewId = 0;
    while(!availableIds.empty())
    {
        availableIds.pop();
    }
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
                    touchInfo.id = [self findNextId];
                    touchIdMap[(uintptr_t)t] = touchInfo.id;
                    //Do call here to c#
                    logger << "Simple Touch Begin " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << touchInfo.id << NativeLog::DebugFlush();
                    break;
                case UITouchPhaseEnded:
                    touchInfo.id = touchIdMap[(uintptr_t)t];
                    //Do call here to c#
                    [self returnid: touchInfo.id];
                    logger << "Simple Touch End " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << touchInfo.id << NativeLog::DebugFlush();
                    break;
                case UITouchPhaseMoved:
                    touchInfo.id = touchIdMap[(uintptr_t)t];
                    //Do call here to c#
                    logger << "Simple Touch Moved " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << touchInfo.id << NativeLog::DebugFlush();
                    break;
                case UITouchPhaseCancelled:
                    //Do call here to c#
                    [self resetIds];
                    logger << "Touches canceled " << touchInfo.pixelX << " " << touchInfo.pixelY << " " << touchInfo.id << NativeLog::DebugFlush();
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