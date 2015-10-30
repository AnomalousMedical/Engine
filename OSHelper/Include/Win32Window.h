#pragma once

#include "NativeOSWindow.h"

#define WIN32_WINDOW_CLASS L"Win32WindowClass"

class Win32Window : public NativeOSWindow
{
public:
	Win32Window(HWND parent, String title, int x, int y, int width, int height);
    
    virtual ~Win32Window();
    
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

	virtual void toggleBorderless();

	void activateCursor()
	{
		SetCursor(hCursor);
	}

	virtual void setOnscreenKeyboardMode(OnscreenKeyboardMode mode);

	virtual OnscreenKeyboardMode getOnscreenKeyboardMode()
	{
		return keyboardMode;
	}
    
    virtual void setupMultitouch(MultiTouch* multiTouch)
	{
		//This does nothing since we have to do our multitouch in another dll to remain compatable with xp.
		//See MultTouch.cpp to see how it works.
	}

	void manageCapture(MouseButtonCode mouseCode);

	void manageRelease(MouseButtonCode mouseCode);

	//Enable or disable windows size messages, this is not actually handeled by this class just stored, you
	//must manually check getAllowWindowSizeMessages before calling fireSized()
	void setAllowWindowSizeMessages(bool allowWindowSizeMessages)
	{
		this->allowWindowSizeMessages = allowWindowSizeMessages;
	}

	bool getAllowWindowSizeMessages()
	{
		return allowWindowSizeMessages;
	}

	static void createWindowClass(HANDLE hModule);

	static void destroyWindowClass();

	void keyboardClosed();

	void usageModeChanged();
private:
	HWND window;
	static WNDCLASSEX wndclass;
	HWND keyboardHwnd;
	HCURSOR hCursor;
	bool mouseDown[MouseButtonCode::NUM_BUTTONS];
	WINDOWPLACEMENT previousWindowPlacement;
	bool allowWindowSizeMessages;
	OnscreenKeyboardMode keyboardMode;
	bool allowShowKeyboard;
	UsageMode usageMode;

	bool showKeyboard();

	void closeKeyboard();
};