#pragma once

#include "NativeOSWindow.h"

class AndroidWindow : public NativeOSWindow
{
public:
	AndroidWindow();
    
	virtual ~AndroidWindow();
    
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

	virtual float getWindowScaling();

	virtual void toggleFullscreen();

	virtual void setOnscreenKeyboardVisible(bool visible);

	virtual bool isOnscreenKeyboardVisible();
    
    virtual void setupMultitouch(MultiTouch* multiTouch)
	{
		
	}
private:
};

void AndroidWindow_setApp(struct android_app* setApp);