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

@implementation UIKitWindowEvents

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    logger.sendMessage("Touches Begin", LogLevel::ImportantInfo);
}

- (void)touchesMoved:(NSSet *)touches withEvent:(UIEvent *)event
{
    logger.sendMessage("Touches moved", LogLevel::ImportantInfo);
}

- (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event
{
    logger.sendMessage("Touches ended", LogLevel::ImportantInfo);
}

- (void)touchesCancelled:(NSSet *)touches withEvent:(UIEvent *)event
{
    logger.sendMessage("Touches cancelled", LogLevel::ImportantInfo);
}

@end