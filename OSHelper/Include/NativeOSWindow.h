#pragma once

enum CursorType
{
    Arrow = 0,
    Beam = 1,
    SizeLeft = 2,
    SizeRight = 3,
    SizeHorz = 4,
    SizeVert = 5,
    Hand = 6,
    Link = 7,
};

enum class OnscreenKeyboardMode
{
	Hidden = 0,
	Normal = 1,
	Secure = 2,
};

enum InternalResourceType
{
	RT_None = 0,
	RT_Graphics = 1,
	RT_Sound = 2,
	RT_All = RT_Graphics | RT_Sound
};

#include "KeyboardButtonCode.h"
#include "MouseButtonCode.h"

class MultiTouch;
class NativeKeyboard;
class NativeMouse;

typedef void(*ActivateCB)(bool arg0 HANDLE_ARG);
typedef void(*ModifyResourcesCB)(InternalResourceType resourceType HANDLE_ARG);

class NativeOSWindow
{
public:        
    typedef void (*MouseButtonDownDelegate)(MouseButtonCode id);
	typedef void (*MouseButtonUpDelegate)(MouseButtonCode id);
	typedef void (*MouseMoveDelegate)(int absX, int absY);
	typedef void (*MouseWheelDelegate)(int relZ);
    
	NativeOSWindow();
    
	virtual ~NativeOSWindow(void);
    
    virtual void setTitle(String title) = 0;
    
    virtual void setSize(int width, int height) = 0;
    
    virtual int getWidth() = 0;
    
    virtual int getHeight() = 0;
    
    virtual void* getHandle() = 0;
    
    virtual void show() = 0;
    
    virtual void close() = 0;
    
    virtual void setMaximized(bool maximized) = 0;
    
    virtual bool getMaximized() = 0;
    
    virtual void setCursor(CursorType cursor) = 0;
    
    virtual void setupMultitouch(MultiTouch* multiTouch) = 0;

	virtual float getWindowScaling() = 0;

	virtual void toggleFullscreen() = 0;

	virtual void toggleBorderless() { } //Does nothing by default

	virtual void setOnscreenKeyboardMode(OnscreenKeyboardMode mode)
	{
		//Does nothing by default.
	}
    
	virtual OnscreenKeyboardMode getOnscreenKeyboardMode()
    {
		return OnscreenKeyboardMode::Hidden;
    }

	void setCallbacks(NativeAction deleteCB, NativeAction sizedCB, NativeAction closingCB, NativeAction closedCB, ActivateCB activateCB, ModifyResourcesCB createInternalResourcesCB, ModifyResourcesCB destroyInternalResourcesCB HANDLE_ARG);
    
	void setExclusiveFullscreen(bool exclusiveFullscreen)
	{
		this->exclusiveFullscreen = exclusiveFullscreen;
	}

	bool getExclusiveFullscreen()
	{
		return exclusiveFullscreen;
	}

    void fireSized()
	{
		sizedCB(PASS_HANDLE);
	}

	void fireClosing()
	{
		closingCB(PASS_HANDLE);
	}
    
	void fireClosed()
	{
		closedCB(PASS_HANDLE);
	}
    
	void fireActivate(bool active)
	{
		activateCB(active PASS_HANDLE_ARG);
	}

	void fireCreateInternalResources(InternalResourceType resourceType)
	{
		createInternalResourcesCB(resourceType PASS_HANDLE_ARG);
	}

	void fireDestroyInternalResources(InternalResourceType resourceType)
	{
		destroyInternalResourcesCB(resourceType PASS_HANDLE_ARG);
	}
    
	void setKeyboard(NativeKeyboard* keyboard);
    
	void fireKeyDown(KeyboardButtonCode keyCode, uint character);
    
	void fireKeyUp(KeyboardButtonCode keyCode);
    
	void setMouse(NativeMouse* mouse);
    
	void fireMouseButtonDown(MouseButtonCode id);
    
	void fireMouseButtonUp(MouseButtonCode id);
    
	void fireMouseMove(int absX, int absY);
    
	void fireMouseWheel(int relZ);

protected:
	bool exclusiveFullscreen;
    
private:
	NativeAction deleteCB;
	NativeAction sizedCB;
	NativeAction closingCB;
	NativeAction closedCB;
	ActivateCB activateCB;
	ModifyResourcesCB createInternalResourcesCB;
	ModifyResourcesCB destroyInternalResourcesCB;
	HANDLE_INSTANCE

	NativeKeyboard* keyboard;
	NativeMouse* mouse;
};