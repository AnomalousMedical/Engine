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
        nextNewId = 0;

        dummyTextField = [[ListenerTextField alloc] initWithFrame:CGRectMake(0, 0, 1, 1)];
        [self addSubview:dummyTextField];
        dummyTextField.delegate = self;
        dummyTextField.keyboardType = UIKeyboardTypeASCIICapable;
        dummyTextField.autocorrectionType = UITextAutocorrectionTypeNo;
        dummyTextField.autocapitalizationType = UITextAutocapitalizationTypeNone;
        dummyTextField.text = @" ";
    }
    return self;
}

- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string
{
    NSString *resultString = [textField.text stringByReplacingCharactersInRange:range withString:string];
    BOOL isPressedBackspaceAfterSingleSpaceSymbol = [string isEqualToString:@""] && [resultString isEqualToString:@""] && range.location == 0 && range.length == 1;
    if (isPressedBackspaceAfterSingleSpaceSymbol)
    {
        win->fireKeyDown(KeyboardButtonCode::KC_BACK, 0);
        win->fireKeyUp(KeyboardButtonCode::KC_BACK);
    }
    else
    {
        unichar firstChar = [string characterAtIndex:0];
        win->fireKeyDown(KeyboardButtonCode::KC_UNASSIGNED, firstChar);
        win->fireKeyUp(KeyboardButtonCode::KC_UNASSIGNED);
    }
    
    return NO;
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField
{
    win->fireKeyDown(KeyboardButtonCode::KC_RETURN, '\n');
    win->fireKeyUp(KeyboardButtonCode::KC_RETURN);
    return NO;
}

-(void) setWindow:(UIKitWindow*) window
{
    win = window;
    [dummyTextField setWindow:window];
}

-(void) setMultitouch:(MultiTouch*) multiTouch
{
    touch = multiTouch;
}

-(void) setOnscreenKeyboardMode:(OnscreenKeyboardMode) mode;
{
    switch (mode)
    {
        case OnscreenKeyboardMode::Hidden:
            [dummyTextField resignFirstResponder];
            break;
        case OnscreenKeyboardMode::Secure:
            [dummyTextField becomeFirstResponder];
            break;
        case OnscreenKeyboardMode::Normal:
        default:
            [dummyTextField becomeFirstResponder];
            break;
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
            touchInfo.normalizedX = (float)touchInfo.pixelX / win->getWidth();
            touchInfo.normalizedY = (float)touchInfo.pixelY / win->getHeight();
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

@end