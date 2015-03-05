//
//  ListenerTextField.m
//  OSHelper
//
//  Created by Andrew Piper on 3/5/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#include "Stdafx.h"
#include "ListenerTextField.h"
#include "UIKitWindow.h"

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
    win->fireKeyDown(KeyboardButtonCode::KC_UP, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_UP);
}

- (void) downArrow: (UIKeyCommand *) keyCommand
{
    win->fireKeyDown(KeyboardButtonCode::KC_DOWN, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_DOWN);
}

- (void) leftArrow: (UIKeyCommand *) keyCommand
{
    win->fireKeyDown(KeyboardButtonCode::KC_LEFT, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_LEFT);
}

- (void) rightArrow: (UIKeyCommand *) keyCommand
{
    win->fireKeyDown(KeyboardButtonCode::KC_RIGHT, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_RIGHT);
}

- (void) escapeKey: (UIKeyCommand *) keyCommand
{
    win->fireKeyDown(KeyboardButtonCode::KC_ESCAPE, 0);
    win->fireKeyUp(KeyboardButtonCode::KC_ESCAPE);
}

- (void) tabKey: (UIKeyCommand *) keyCommand
{
    win->fireKeyDown(KeyboardButtonCode::KC_TAB, '\t');
    win->fireKeyUp(KeyboardButtonCode::KC_TAB);
}

-(void) setWindow:(UIKitWindow*) window
{
    win = window;
}

@end