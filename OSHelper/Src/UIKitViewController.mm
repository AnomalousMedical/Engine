//
//  ViewController.m
//  SuperTinyIOSApp
//
//  Created by Andrew Piper on 1/16/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#include "StdAfx.h"
#import "UIKitViewController.h"
#include "UIKitWindow.h"

@interface UIKitViewController ()

@end

@implementation UIKitViewController

- (void) loadView {
    uiKitView = [[UIKitView alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
    self.view = uiKitView;
    self.view.opaque = YES;
    self.view.contentScaleFactor = [[UIScreen mainScreen] scale];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(keyboardWasShown:)
                                                 name:UIKeyboardDidShowNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(keyboardWillBeHidden:)
                                                 name:UIKeyboardWillHideNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(keyboardDidChangeFrame:)
                                                 name:UIKeyboardDidChangeFrameNotification
                                               object:nil];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (BOOL)prefersStatusBarHidden {
    return YES;
}

-(void) setWindow:(UIKitWindow*) window
{
    win = window;
    [uiKitView setWindow:window];
}

-(void) firePendingResize
{
    return [uiKitView firePendingResize];
}

-(void) fireCreateInternalResources:(InternalResourceType) resourceType
{
    win->fireCreateInternalResources(resourceType);
}

-(void) fireDestroyInternalResources:(InternalResourceType) resourceType
{
    win->fireDestroyInternalResources(resourceType);
}

-(void) cancelResize
{
    [uiKitView cancelResize];
}

-(void) setApplicationActive:(bool) active
{
    [uiKitView setApplicationActive:active];
}

-(UIKitView*) getUIKitView
{
    return uiKitView;
}

-(bool) safeToRender
{
    return [uiKitView safeToRender];
}

-(void) viewDidAppear:(BOOL)animated
{
    [uiKitView setViewVisible:true];
    [super viewDidAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
    [uiKitView setViewVisible:false];
    [super viewWillDisappear:animated];
}

- (void)keyboardWasShown:(NSNotification*)aNotification
{
    NSDictionary* info = [aNotification userInfo];
    CGRect kbRect = [[info objectForKey:UIKeyboardFrameEndUserInfoKey] CGRectValue];
    win->onscreenKeyboardVisible(kbRect);
}

-(void)keyboardWillBeHidden:(NSNotification*)aNotification
{
    win->onscreenKeyboardHiding();
}

-(void)keyboardDidChangeFrame:(NSNotification*)aNotification
{
    NSDictionary* info = [aNotification userInfo];
    CGRect kbRect = [[info objectForKey:UIKeyboardFrameEndUserInfoKey] CGRectValue];
    win->onscreenKeyboardFrameChanged(kbRect);
}

@end
