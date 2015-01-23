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

@implementation UIKitWindowEvents

-(id) init
{
    if(self)
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

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    logger <<  "Touches Begin " << touch << NativeLog::DebugFlush();
}

- (void)touchesMoved:(NSSet *)touches withEvent:(UIEvent *)event
{
    logger.sendMessage("Touches moved", LogLevel::ImportantInfo);
}

- (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self resignFirstResponder];
    allowFirstResponder = true;
    [self becomeFirstResponder];
    logger.sendMessage("Touches ended", LogLevel::ImportantInfo);
}

- (void)touchesCancelled:(NSSet *)touches withEvent:(UIEvent *)event
{
    logger.sendMessage("Touches cancelled", LogLevel::ImportantInfo);
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