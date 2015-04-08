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

	virtual void setOnscreenKeyboardMode(OnscreenKeyboardMode mode);

	virtual OnscreenKeyboardMode getOnscreenKeyboardMode();
    
    virtual void setupMultitouch(MultiTouch* multiTouch)
	{
		this->multiTouch = multiTouch;
	}

	int32_t handleInputEvent(struct android_app* app, AInputEvent* event);

private:
	MultiTouch* multiTouch;
	OnscreenKeyboardMode keyboardMode;

	//android_app_handle_input variables
	int32_t eventType;

	int deviceId; //We can use this to identify if we are a mouse or the touchscreen or something else
	int eventPointerIndex; //The index into the event of the pointer
	int eventAction; //Event full info, contains action and pointer index
	int action; //The action that was performed
	int pointerCount; //The pointer count for this event
	TouchInfo touchInfo; //Pooled touch info struct;

	int32_t keyCode;
	int32_t keyFlags;
	int32_t keyAction;
};

void AndroidWindow_setApp(struct android_app* setApp);