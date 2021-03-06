//
//  UIKitWindowEvents.h
//  OSHelper
//
//  Created by Andrew Piper on 1/23/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitWindowEvents_h
#define OSHelper_UIKitWindowEvents_h

#include "ListenerTextField.h"
#include <map>
#include <stack>
#include "NativeOSWindow.h"

class UIKitWindow;
class MultiTouch;

@interface UIKitWindowEvents : UIWindow<UITextFieldDelegate>
{
@private
    UIKitWindow* win;
    MultiTouch* touch;
    std::map<uintptr_t, int> touchIdMap;
    std::stack<int> availableIds;
    int nextNewId;
    
    ListenerTextField* dummyTextField;
}

-(void) setWindow:(UIKitWindow*) window;

-(void) setMultitouch:(MultiTouch*) multiTouch;

-(void) setOnscreenKeyboardMode:(OnscreenKeyboardMode) mode;

@end

#endif
