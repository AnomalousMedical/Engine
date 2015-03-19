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
    
    virtual void setupMultitouch(MultiTouch* multiTouch)
	{
		//This does nothing since we have to do our multitouch in another dll to remain compatable with xp.
		//See MultTouch.cpp to see how it works.
	}
private:
	
};