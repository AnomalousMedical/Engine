//
//  UIKitWindow.h
//  OSHelper
//
//  Created by Andrew Piper on 1/19/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#ifndef OSHelper_UIKitWindow_h
#define OSHelper_UIKitWindow_h

#include "Stdafx.h"

#import <UIKit/UIKit.h>
#include "NativeOSWindow.h"

class UIKitWindow : public NativeOSWindow
{
public:
    UIKitWindow(UIKitWindow* parent, String title, int x, int y, int width, int height, bool floatOnParent);
    
    virtual ~UIKitWindow();
    
    virtual void setTitle(String title);
    
    virtual void setSize(int width, int height);
    
    virtual int getWidth();
    
    virtual int getHeight();
    
    virtual void* getHandle();
    
    virtual void show();
    
    virtual void close();
    
    virtual void setMaximized(bool maximized);
    
    virtual bool getMaximized();
    
    virtual void setCursor(CursorType cursor);
    
    virtual void setupMultitouch(MultiTouch* multiTouch);
    
    virtual float getWindowScaling();
    
    virtual void toggleFullscreen();
};

void UIKitWindow_setUIWindow(UIWindow *window);

#endif
