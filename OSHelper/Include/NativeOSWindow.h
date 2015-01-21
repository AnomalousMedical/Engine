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

#include "KeyboardButtonCode.h"
#include "MouseButtonCode.h"

class MultiTouch;

typedef void(*ActivateCB)(bool arg0 HANDLE_ARG);

class NativeOSWindow
{
public:    
    typedef void (*KeyDownDelegate)(KeyboardButtonCode keyCode, uint character);
	typedef void (*KeyUpDelegate)(KeyboardButtonCode keyCode);
    
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

	void setCallbacks(NativeAction deleteCB, NativeAction sizedCB, NativeAction closingCB, NativeAction closedCB, ActivateCB activateCB HANDLE_ARG);
    
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
    
    void setKeyDownCallback(KeyDownDelegate keyDown)
    {
        keyDownCB = keyDown;
    }
    
    void fireKeyDown(KeyboardButtonCode keyCode, uint character)
    {
        if(keyDownCB != 0)
        {
            keyDownCB(keyCode, character);
        }
    }
    
    void setKeyUpCallback(KeyUpDelegate keyUp)
    {
        keyUpCB = keyUp;
    }
    
	void fireKeyUp(KeyboardButtonCode keyCode)
    {
        if(keyUpCB != 0)
        {
            keyUpCB(keyCode);
        }
    }
    
    void setMouseButtonDownCallback(MouseButtonDownDelegate mouseButtonDown)
    {
        this->mouseButtonDownCB = mouseButtonDown;
    }
    
    void fireMouseButtonDown(MouseButtonCode id)
    {
        if(mouseButtonDownCB != 0)
        {
            mouseButtonDownCB(id);
        }
    }
    
    void setMouseButtonUpCallback(MouseButtonUpDelegate mouseButtonUp)
    {
        this->mouseButtonUpCB = mouseButtonUp;
    }
    
	void fireMouseButtonUp(MouseButtonCode id)
    {
        if(mouseButtonUpCB != 0)
        {
            mouseButtonUpCB(id);
        }
    }
    
    void setMouseMoveCallback(MouseMoveDelegate mouseMove)
    {
        this->mouseMoveCB = mouseMove;
    }
    
	void fireMouseMove(int absX, int absY)
    {
        if(mouseMoveCB != 0)
        {
            mouseMoveCB(absX, absY);
        }
    }
    
    void setMouseWheelCallback(MouseWheelDelegate mouseWheel)
    {
        this->mouseWheelCB = mouseWheel;
    }
    
	void fireMouseWheel(int relZ)
    {
        if(mouseWheelCB != 0)
        {
            mouseWheelCB(relZ);
        }
    }

protected:
	bool exclusiveFullscreen;
    
private:
	NativeAction deleteCB;
	NativeAction sizedCB;
	NativeAction closingCB;
	NativeAction closedCB;
	ActivateCB activateCB;
	HANDLE_INSTANCE
    
    KeyDownDelegate keyDownCB;
	KeyUpDelegate keyUpCB;
    
    MouseButtonDownDelegate mouseButtonDownCB;
	MouseButtonUpDelegate mouseButtonUpCB;
	MouseMoveDelegate mouseMoveCB;
	MouseWheelDelegate mouseWheelCB;
};