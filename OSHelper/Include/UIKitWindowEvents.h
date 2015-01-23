//
//  UIKitWindowEvents.h
//  OSHelper
//
//  Created by Andrew Piper on 1/23/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitWindowEvents_h
#define OSHelper_UIKitWindowEvents_h

class UIKitWindow;

@interface UIKitWindowEvents : UIWindow<UIKeyInput>
{
@private
    UIKitWindow* win;
    bool allowFirstResponder;
    bool hasText;
}

-(id) init;

-(void) setWindow:(UIKitWindow*) window;

@end

#endif
