#pragma once

#include "NativeOSWindow.h"
#include "MultiTouch.h"

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
		this->multiTouch = multiTouch;
	}

	int32_t handleInputEvent(struct android_app* app, AInputEvent* event);

private:
	MultiTouch* multiTouch;

	//android_app_handle_input variables
	int deviceId; //We can use this to identify if we are a mouse or the touchscreen or something else
	int eventPointerIndex; //The index into the event of the pointer
	int eventAction; //Event full info, contains action and pointer index
	int action; //The action that was performed
	int pointerCount; //The pointer count for this event
	TouchInfo touchInfo; //Pooled touch info struct;
};

void AndroidWindow_setApp(struct android_app* setApp);