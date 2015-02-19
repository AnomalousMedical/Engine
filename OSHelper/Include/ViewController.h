//
//  ViewController.h
//  SuperTinyIOSApp
//
//  Created by Andrew Piper on 1/16/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#pragma once

#import <UIKit/UIKit.h>

class UIKitWindow;

@interface ViewController : UIViewController
{
@private
    UIKitWindow* win;
}

-(void) setWindow:(UIKitWindow*) window;

@end

