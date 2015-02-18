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
UIViewController *contentViewController = NULL;

void UIKitWindow_setUIWindow(UIKitWindowEvents *win)
{    
    window = win;
}

void UIKitWindow_setContentViewController(UIViewController *cvc)
{
    contentViewController = cvc;
}

UIKitWindow::UIKitWindow(UIKitWindow* parent, String title, int x, int y, int width, int height, bool floatOnParent)
:keyboardVisible(false)
{
    [window setWindow:this];
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
    CGRect frame;
    if(contentViewController != NULL)
    {
        frame = [contentViewController.view frame];
    }
    else
    {
        frame = [window frame];
    }
    return (int)frame.size.width * getWindowScaling();
}

int UIKitWindow::getHeight()
{
    CGRect frame;
    if(contentViewController != NULL)
    {
        frame = [contentViewController.view frame];
    }
    else
    {
        frame = [window frame];
    }
    return (int)frame.size.height * getWindowScaling();
}

void* UIKitWindow::getHandle()
{
    return (__bridge void*)window;
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

void UIKitWindow::onscreenKeyboardVisible(CGRect kbRect)
{
    logger << "Onscreen keyboard visible " << kbRect.size.width << ", " << kbRect.size.height << debug;
    keyboardVisible = true;
    if(contentViewController != NULL)
    {
        CGRect frame = [window frame];
        frame.size.height -= kbRect.size.height;
        contentViewController.view.frame = frame;
        
        logger << "new frame size " << contentViewController.view.frame.origin.x << " " << contentViewController.view.frame.origin.y << " " << contentViewController.view.frame.size.width << ", " << contentViewController.view.frame.size.height << debug;
        
        fireSized();
    }
}

void UIKitWindow::onscreenKeyboardFrameChanged(CGRect kbRect)
{
    logger << "Onscreen keyboard frame changed " << kbRect.size.width << ", " << kbRect.size.height << debug;
    if(keyboardVisible && contentViewController != NULL)
    {
        CGRect frame = [window frame];
        frame.size.height -= kbRect.size.height;
        contentViewController.view.frame = frame;
        
        logger << "new frame size " << contentViewController.view.frame.origin.x << " " << contentViewController.view.frame.origin.y << " " << contentViewController.view.frame.size.width << ", " << contentViewController.view.frame.size.height << debug;
        
        fireSized();
    }
}

void UIKitWindow::onscreenKeyboardHiding()
{
    logger.sendMessage("Onscreen keyboard hiding", LogLevel::Debug);
    keyboardVisible = false;
    if(contentViewController != NULL)
    {
        CGRect frame = [window frame];
        contentViewController.view.frame = frame;
        
        fireSized();
    }
}

//PInvoke
extern "C" _AnomalousExport NativeOSWindow* NativeOSWindow_create(NativeOSWindow* parent, String caption, int x, int y, int width, int height, bool floatOnParent)
{
    return new UIKitWindow(static_cast<UIKitWindow*>(parent), caption, x, y, width, height, floatOnParent);
}