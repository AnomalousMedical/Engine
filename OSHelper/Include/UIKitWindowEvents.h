//
//  UIKitWindowEvents.h
//  OSHelper
//
//  Created by Andrew Piper on 1/23/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitWindowEvents_h
#define OSHelper_UIKitWindowEvents_h

#include "UIKitOrientationEvents.h"

class UIKitWindow;
class MultiTouch;

@interface UIKitWindowEvents : UIWindow<UIKeyInput>
{
@private
    UIKitWindow* win;
    MultiTouch* touch;
    bool allowFirstResponder;
    bool hasText;
    UIKitOrientationEvents* orientationEvents;
}

-(void) setWindow:(UIKitWindow*) window;

-(void) setMultitouch:(MultiTouch*) multiTouch;

@end

#endif
