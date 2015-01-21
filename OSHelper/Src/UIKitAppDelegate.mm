//
//  UIKitAppDelegate.m
//  OSHelper
//
//  Created by Andrew Piper on 1/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#import <Foundation/Foundation.h>
#include "StdAfx.h"
#include "UIKitAppDelegate.h"
#include "UIKitWindow.h"

@interface UIKitAppDelegate ()

@end

static UIKitApp* primaryUiKitApp;

void UIKitAppDelegate_setPrimaryUIKitApp(UIKitApp* app)
{
    primaryUiKitApp = app;
}

@implementation UIKitAppDelegate


//Better but not quite right yet.
-(void) doFrame:(id)data
{
    primaryUiKitApp->fireIdle();
}

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    
    NSString *storyboardName = @"MainStoryboard";
    UIStoryboard *storyboard = [UIStoryboard storyboardWithName:storyboardName bundle:nil];
    UIViewController *initialViewController = [storyboard instantiateInitialViewController];
    self.window = [[UIWindow alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
    self.window.rootViewController  = initialViewController;
    [self.window makeKeyAndVisible];
    UIKitWindow_setUIWindow(self.window);
    
    primaryUiKitApp->fireInit();
    
    CADisplayLink* mFrameLink = [CADisplayLink displayLinkWithTarget:self selector:@selector(doFrame:)];
    mFrameLink.frameInterval = 1;
    [mFrameLink addToRunLoop:[NSRunLoop mainRunLoop] forMode:NSDefaultRunLoopMode];
    
    return YES;
}

- (void)applicationWillResignActive:(UIApplication *)application {
    // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
    // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
}

- (void)applicationDidEnterBackground:(UIApplication *)application {
    // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
    // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
}

- (void)applicationWillEnterForeground:(UIApplication *)application {
    // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
}

- (void)applicationDidBecomeActive:(UIApplication *)application {
    // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
}

- (void)applicationWillTerminate:(UIApplication *)application {
    // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
    primaryUiKitApp->fireExit();
}

@end

