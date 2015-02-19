//
//  UIKitView.m
//  OSHelper
//
//  Created by Andrew Piper on 2/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#include "StdAfx.h"
#import <Foundation/Foundation.h>
#include "UIKitView.h"
#include "UIKitWindow.h"

@implementation UIKitView

- (NSString *)description
{
    return [NSString stringWithFormat:@"UIKitView frame dimensions x: %.0f y: %.0f w: %.0f h: %.0f",
            [self frame].origin.x,
            [self frame].origin.y,
            [self frame].size.width,
            [self frame].size.height];
}

+ (Class)layerClass
{
    return [CAEAGLLayer class];
}

- (void)layoutSubviews
{
    win->fireSized();
}

-(void) setWindow:(UIKitWindow*) window
{
    win = window;
}

@end
