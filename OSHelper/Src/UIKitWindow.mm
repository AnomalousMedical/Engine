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
ViewController *viewController = NULL;
UIView *view;

//What we think of as a handle on ios is really 3 things a UIWindow, UIViewController and UIView
//As a result we will pass these items in an array that comprises our handle. The first item is the
//UIWindow, the second is the UIViewController and the third is the UIView.
unsigned long handleArray[3];

void UIKitWindow_setUIWindow(UIKitWindowEvents *win, ViewController *vc)
{    
    window = win;
    viewController = vc;
    view = viewController.view;
    
    handleArray[0] = (unsigned long)(__bridge void*)window;
    handleArray[1] = (unsigned long)(__bridge void*)viewController;
    handleArray[2] = (unsigned long)(__bridge void*)viewController.view;
}

UIKitWindow::UIKitWindow(UIKitWindow* parent, String title, int x, int y, int width, int height, bool floatOnParent)
:keyboardVisible(false)
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

void UIKitWindow::setOnscreenKeyboardVisible(bool visible)
{
    [window setOnscreenKeyboardVisible:visible];
}

bool UIKitWindow::isOnscreenKeyboardVisible()
{
    return keyboardVisible;
}

void UIKitWindow::onscreenKeyboardVisible(CGRect kbRect)
{
    if(!keyboardVisible)
    {
        keyboardVisible = true;
        CGRect frame = [window frame];
        frame.size.height -= kbRect.size.height;
        view.frame = frame;
    }
}

void UIKitWindow::onscreenKeyboardFrameChanged(CGRect kbRect)
{
    if(keyboardVisible)
    {
        CGRect frame = [window frame];
        frame.size.height -= kbRect.size.height;
        view.frame = frame;
    }
}

void UIKitWindow::onscreenKeyboardHiding()
{
    if(keyboardVisible)
    {
        keyboardVisible = false;
        CGRect frame = [window frame];
        view.frame = frame;
    }
}

//PInvoke
extern "C" _AnomalousExport NativeOSWindow* NativeOSWindow_create(NativeOSWindow* parent, String caption, int x, int y, int width, int height, bool floatOnParent)
{
    return new UIKitWindow(static_cast<UIKitWindow*>(parent), caption, x, y, width, height, floatOnParent);
}