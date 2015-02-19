//
//  UIKitView.h
//  OSHelper
//
//  Created by Andrew Piper on 2/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitView_h
#define OSHelper_UIKitView_h

#import <UIKit/UIKit.h>

class UIKitWindow;

@interface UIKitView : UIView
{
@private
    UIKitWindow *win;
}

-(void) setWindow:(UIKitWindow*) window;

@end

#endif
