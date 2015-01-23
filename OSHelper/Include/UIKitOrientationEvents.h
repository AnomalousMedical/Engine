//
//  UIKitOrientationEvents.h
//  OSHelper
//
//  Created by Andrew Piper on 1/23/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitOrientationEvents_h
#define OSHelper_UIKitOrientationEvents_h

#import <UIKit/UIKit.h>

class UIKitWindow;

@interface UIKitOrientationEvents : NSObject
{
@private
    UIKitWindow* win;
}

-(id) init:(UIKitWindow*) window;

@end

#endif
