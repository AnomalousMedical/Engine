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

@implementation ListenerTextField

- (id) initWithFrame: (CGRect) frame
{
    self = [super initWithFrame:frame];
    if (self) {
    }
    return self;
}

- (NSArray *) keyCommands
{
    UIKeyCommand *upArrow = [UIKeyCommand keyCommandWithInput: UIKeyInputUpArrow modifierFlags: 0 action: @selector(upArrow:)];
    UIKeyCommand *downArrow = [UIKeyCommand keyCommandWithInput: UIKeyInputDownArrow modifierFlags: 0 action: @selector(downArrow:)];
    UIKeyCommand *leftArrow = [UIKeyCommand keyCommandWithInput: UIKeyInputLeftArrow modifierFlags: 0 action: @selector(leftArrow:)];
    UIKeyCommand *rightArrow = [UIKeyCommand keyCommandWithInput: UIKeyInputRightArrow modifierFlags: 0 action: @selector(rightArrow:)];
    UIKeyCommand *escape = [UIKeyCommand keyCommandWithInput: UIKeyInputEscape modifierFlags: 0 action: @selector(escapeKey:)];
    UIKeyCommand *tab = [UIKeyCommand keyCommandWithInput: @"\t" modifierFlags: 0 action: @selector(tabKey:)];
    return [[NSArray alloc] initWithObjects: upArrow, downArrow, leftArrow, rightArrow, escape, tab, nil];
}

- (void) upArrow: (UIKeyCommand *) keyCommand
{
    NSLog(@"upArrow");
    win->fireKeyDown(KeyboardButtonCode::KC_UP, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_UP);
}

- (void) downArrow: (UIKeyCommand *) keyCommand
{
    NSLog(@"downArrow");
    win->fireKeyDown(KeyboardButtonCode::KC_DOWN, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_DOWN);
}

- (void) leftArrow: (UIKeyCommand *) keyCommand
{
    NSLog(@"leftArrow");
    win->fireKeyDown(KeyboardButtonCode::KC_LEFT, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_LEFT);
}

- (void) rightArrow: (UIKeyCommand *) keyCommand
{
    NSLog(@"rightArrow");
    win->fireKeyDown(KeyboardButtonCode::KC_RIGHT, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_RIGHT);
}

- (void) escapeKey: (UIKeyCommand *) keyCommand
{
    NSLog(@"escapeKey");
    win->fireKeyDown(KeyboardButtonCode::KC_ESCAPE, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_ESCAPE);
}

- (void) tabKey: (UIKeyCommand *) keyCommand
{
    NSLog(@"tabKey");
    win->fireKeyDown(KeyboardButtonCode::KC_TAB, '\t');
    win->fireKeyUp(KeyboardButtonCode::KC_TAB);
}

-(void) setWindow:(UIKitWindow*) window
{
    win = window;
}

@end

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

        dummyTextField = [[ListenerTextField alloc] initWithFrame:CGRectMake(0, 0, 100, 100)];
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
        NSLog(@"Backspace");
        win->fireKeyDown(KeyboardButtonCode::KC_BACK, 0);
        win->fireKeyUp(KeyboardButtonCode::KC_BACK);
    }
    else
    {
        unichar firstChar = [string characterAtIndex:0];
        win->fireKeyDown(KeyboardButtonCode::KC_UNASSIGNED, firstChar);
        win->fireKeyUp(KeyboardButtonCode::KC_UNASSIGNED);
        NSLog(@"%@", string);
    }
    
    return NO;
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField
{
    win->fireKeyDown(KeyboardButtonCode::KC_RETURN, '\n');
    win->fireKeyUp(KeyboardButtonCode::KC_RETURN);
    NSLog(@"Return");
    return NO;
}

- (void)textViewDidChangeSelection:(UITextView *)textView
{
    NSLog(@"Text selection change");
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

-(void) setOnscreenKeyboardVisible:(bool) visible;
{
    if(visible)
    {
        if(!allowFirstResponder)
        {
            [dummyTextField resignFirstResponder];
            allowFirstResponder = true;
        }
        [dummyTextField becomeFirstResponder];
    }
    else
    {
        allowFirstResponder = false;
        [dummyTextField resignFirstResponder];
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
    win->fireKeyDown(KeyboardButtonCode::KC_BACK, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_BACK);
}

- (BOOL)hasText
{
    return hasText;
}

- (BOOL)canBecomeFirstResponder
{
    return allowFirstResponder;
}

@end