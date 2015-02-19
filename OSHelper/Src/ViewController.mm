//
//  ViewController.m
//  SuperTinyIOSApp
//
//  Created by Andrew Piper on 1/16/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#include "StdAfx.h"
#import "ViewController.h"
#include "UIKitWindow.h"

@interface GLView : UIView {

}

@end

@implementation GLView

- (NSString *)description
{
    return [NSString stringWithFormat:@"GLView frame dimensions x: %.0f y: %.0f w: %.0f h: %.0f",
            [self frame].origin.x,
            [self frame].origin.y,
            [self frame].size.width,
            [self frame].size.height];
}

+ (Class)layerClass
{
    return [CAEAGLLayer class];
}

- (void)layoutSubviews
{
    
}

@end


@interface ViewController ()

@end

@implementation ViewController

- (void) loadView {    
    self.view = [[GLView alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
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
                                                 name:UIKeyboardDidHideNotification
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

- (void)viewDidLayoutSubviews
{
    win->fireSized();
}

-(void) setWindow:(UIKitWindow*) window
{
    win = window;
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
