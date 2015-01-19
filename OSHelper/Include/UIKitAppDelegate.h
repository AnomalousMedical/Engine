//
//  UIKitAppDelegate.h
//  OSHelper
//
//  Created by Andrew Piper on 1/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitAppDelegate_h
#define OSHelper_UIKitAppDelegate_h

#import <UIKit/UIKit.h>
#include "UIKitApp.h"

@interface UIKitAppDelegate : UIResponder <UIApplicationDelegate>

@property (strong, nonatomic) UIWindow *window;

@end

void UIKitAppDelegate_setPrimaryUIKitApp(UIKitApp* app);

#endif
