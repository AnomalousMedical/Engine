//
//  UIKitWindowEvents.h
//  OSHelper
//
//  Created by Andrew Piper on 1/23/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitWindowEvents_h
#define OSHelper_UIKitWindowEvents_h

#include <map>
#include <stack>

class UIKitWindow;
class MultiTouch;

@interface UIKitWindowEvents : UIWindow<UIKeyInput, UITextInputTraits, UITextFieldDelegate>
{
@private
    UIKitWindow* win;
    MultiTouch* touch;
    bool allowFirstResponder;
    bool hasText;
    std::map<uintptr_t, int> touchIdMap;
    std::stack<int> availableIds;
    int nextNewId;
    
    UITextField* dummyTextField;
}

-(void) setWindow:(UIKitWindow*) window;

-(void) setMultitouch:(MultiTouch*) multiTouch;

-(void) setOnscreenKeyboardVisible:(bool) visible;

@end

#endif
