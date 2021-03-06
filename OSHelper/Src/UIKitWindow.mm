//
//  UIKitWindow.m
//  OSHelper
//
//  Created by Andrew Piper on 1/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#import <Foundation/Foundation.h>
#include "Stdafx.h"
#include "UIKitWindow.h"

UIKitWindowEvents *window;
UIKitViewController *viewController = NULL;
UIKitView *view;

//What we think of as a handle on ios is really 3 things a UIWindow, UIViewController and UIView
//As a result we will pass these items in an array that comprises our handle. The first item is the
//UIWindow, the second is the UIViewController and the third is the UIView.
unsigned long handleArray[3];

void UIKitWindow_setUIWindow(UIKitWindowEvents *win, UIKitViewController *vc)
{    
    window = win;
    viewController = vc;
    view = [viewController getUIKitView];
    
    handleArray[0] = (unsigned long)(__bridge void*)window;
    handleArray[1] = (unsigned long)(__bridge void*)viewController;
    handleArray[2] = (unsigned long)(__bridge void*)viewController.view;
}

UIKitWindow::UIKitWindow(UIKitWindow* parent, String title, int x, int y, int width, int height, bool floatOnParent)
:keyboardMode(OnscreenKeyboardMode::Hidden)
{
    [window setWindow:this];
    [viewController setWindow:this];
}

UIKitWindow::~UIKitWindow()
{
    
}

void UIKitWindow::setTitle(String title)
{
    
}

void UIKitWindow::setSize(int width, int height)
{
    
}

int UIKitWindow::getWidth()
{
    CGRect frame = [view frame];
    return (int)frame.size.width * getWindowScaling();
}

int UIKitWindow::getHeight()
{
    CGRect frame = [view frame];
    return (int)frame.size.height * getWindowScaling();
}

void* UIKitWindow::getHandle()
{
    return handleArray;
}

void UIKitWindow::show()
{
    
}

void UIKitWindow::close()
{
    
}

void UIKitWindow::setMaximized(bool maximized)
{
    
}

bool UIKitWindow::getMaximized()
{
    return true;
}

void UIKitWindow::setCursor(CursorType cursor)
{
    
}

float UIKitWindow::getWindowScaling()
{
    return [[UIScreen mainScreen] scale];
}

void UIKitWindow::setupMultitouch(MultiTouch *multiTouch)
{
    [window setMultitouch:multiTouch];
}

void UIKitWindow::toggleFullscreen()
{
    
}

void UIKitWindow::setOnscreenKeyboardMode(OnscreenKeyboardMode mode)
{
    [window setOnscreenKeyboardMode:mode];
    keyboardMode = mode;
}

OnscreenKeyboardMode UIKitWindow::getOnscreenKeyboardMode()
{
    return keyboardMode;
}

void UIKitWindow::onscreenKeyboardVisible(CGRect kbRect)
{
    if(keyboardMode == OnscreenKeyboardMode::Hidden)
    {
        CGRect frame = [window frame];
        frame.size.height -= kbRect.size.height;
        [view setFrameForNextUpdate:frame];
    }
}

void UIKitWindow::onscreenKeyboardFrameChanged(CGRect kbRect)
{
    if(keyboardMode != OnscreenKeyboardMode::Hidden)
    {
        CGRect frame = [window frame];
        frame.size.height -= kbRect.size.height;
        [view setFrameForNextUpdate:frame];
    }
}

void UIKitWindow::onscreenKeyboardHiding()
{
    if(keyboardMode != OnscreenKeyboardMode::Hidden)
    {
        keyboardMode = OnscreenKeyboardMode::Hidden;
        CGRect frame = [window frame];
        [view setFrameForNextUpdate:frame];
    }
}

//PInvoke
extern "C" _AnomalousExport NativeOSWindow* NativeOSWindow_create(NativeOSWindow* parent, String caption, int x, int y, int width, int height, bool floatOnParent)
{
    return new UIKitWindow(static_cast<UIKitWindow*>(parent), caption, x, y, width, height, floatOnParent);
}