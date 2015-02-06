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

@synthesize keyboardType;

@synthesize autocorrectionType;

- (id) initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self)
    {
        allowFirstResponder = false;
        hasText = false;
        nextNewId = 0;
        self.keyboardType = UIKeyboardTypeASCIICapable;
        self.autocorrectionType = UITextAutocorrectionTypeNo;
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

-(void) setOnscreenKeyboardVisible:(bool) visible;
{
    if(visible)
    {
        if(!allowFirstResponder)
        {
            [self resignFirstResponder];
            allowFirstResponder = true;
        }
        [self becomeFirstResponder];
    }
    else
    {
        allowFirstResponder = false;
        [self resignFirstResponder];
    }
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
            float scale = win->getWindowScaling();
            touchInfo.pixelX = (int)(loc.x * scale);
            touchInfo.pixelY = (int)(loc.y * scale);
            switch (t.phase)
            {
                case UITouchPhaseBegan:
                    touchInfo.id = [self findNextId];
                    touchIdMap[(uintptr_t)t] = touchInfo.id;
                    touch->fireTouchStarted(touchInfo);
                    break;
                case UITouchPhaseEnded:
                    touchInfo.id = touchIdMap[(uintptr_t)t];
                    touch->fireTouchEnded(touchInfo);
                    [self returnid: touchInfo.id];
                    break;
                case UITouchPhaseMoved:
                    touchInfo.id = touchIdMap[(uintptr_t)t];
                    touch->fireTouchMoved(touchInfo);
                    break;
                case UITouchPhaseCancelled:
                    touch->fireAllTouchesCanceled();
                    [self resetIds];
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
    //logger.sendMessage("Text entered", LogLevel::ImportantInfo);
    //NSLog(text);
    if([text length] > 0)
    {
        unichar firstChar = [text characterAtIndex:0];
        KeyboardButtonCode keyCode = KC_UNASSIGNED;
        switch(firstChar)
        {
            case '\n':
                keyCode = KeyboardButtonCode::KC_RETURN;
                break;
        }
        win->fireKeyDown(keyCode, firstChar);
        win->fireKeyUp(keyCode);
    }
}
- (void)deleteBackward
{
    //logger.sendMessage("delete backward", LogLevel::ImportantInfo);
    win->fireKeyDown(KeyboardButtonCode::KC_BACK, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_BACK);
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