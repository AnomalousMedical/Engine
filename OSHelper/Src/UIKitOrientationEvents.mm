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

- (void)keyboardWasShown:(NSNotification*)aNotification
{
    NSDictionary* info = [aNotification userInfo];
    CGRect kbRect = [[info objectForKey:UIKeyboardFrameEndUserInfoKey] CGRectValue];
    win->onscreenKeyboardVisible(kbRect);
}

-(void)keyboardWillBeHidden:(NSNotification*)aNotification
{
    win->onscreenKeyboardHiding();
}

-(void)keyboardDidChangeFrame:(NSNotification*)aNotification
{
    NSDictionary* info = [aNotification userInfo];
    CGRect kbRect = [[info objectForKey:UIKeyboardFrameEndUserInfoKey] CGRectValue];
    win->onscreenKeyboardFrameChanged(kbRect);
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
        
        [[NSNotificationCenter defaultCenter] addObserver:self
         selector:@selector(keyboardWasShown:)
         name:UIKeyboardWillShowNotification
         object:nil];

        [[NSNotificationCenter defaultCenter] addObserver:self
         selector:@selector(keyboardWillBeHidden:)
         name:UIKeyboardWillHideNotification
         object:nil];
        
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(keyboardDidChangeFrame:)
                                                     name:UIKeyboardDidChangeFrameNotification
                                                   object:nil];
    }
    
    return self;
}

@end