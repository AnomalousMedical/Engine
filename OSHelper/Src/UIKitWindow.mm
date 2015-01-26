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

void UIKitWindow_setUIWindow(UIKitWindowEvents *win)
{    
    window = win;
}

UIKitWindow::UIKitWindow(UIKitWindow* parent, String title, int x, int y, int width, int height, bool floatOnParent)
{
    [window setWindow:this];
}

UIKitWindow::~UIKitWindow()
{
    
}

void UIKitWindow::setTitle(String title)
{
    logger.sendMessage("with keyboard return", LogLevel::ImportantInfo);
}

void UIKitWindow::setSize(int width, int height)
{
    
}

int UIKitWindow::getWidth()
{
    CGRect frame = [window frame];
    return (int)frame.size.width * getWindowScaling();
}

int UIKitWindow::getHeight()
{
    CGRect frame = [window frame];
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

//PInvoke
extern "C" _AnomalousExport NativeOSWindow* NativeOSWindow_create(NativeOSWindow* parent, String caption, int x, int y, int width, int height, bool floatOnParent)
{
    return new UIKitWindow(static_cast<UIKitWindow*>(parent), caption, x, y, width, height, floatOnParent);
}