//
//  ListenerTextField.h
//  OSHelper
//
//  Created by Andrew Piper on 3/5/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_ListenerTextField_h
#define OSHelper_ListenerTextField_h

class UIKitWindow;

@interface ListenerTextField : UITextField
{
@private
    UIKitWindow* win;
}

-(void) setWindow:(UIKitWindow*) window;

@end

#endif
