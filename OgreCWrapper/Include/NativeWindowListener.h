#pragma once

typedef void (*WindowEventDelegate)(Ogre::RenderWindow* renderWindow HANDLE_ARG);
typedef bool(*WindowClosingDelegate)(Ogre::RenderWindow* renderWindow HANDLE_ARG);

class NativeWindowListener : public Ogre::WindowEventListener
{
private:
	WindowEventDelegate windowMovedCallback;
	WindowEventDelegate windowResizedCallback;
	WindowClosingDelegate windowClosingCallback;
	WindowEventDelegate windowClosedCallback;
	WindowEventDelegate windowFocusChangeCallback;
	HANDLE_INSTANCE

public:
	NativeWindowListener(WindowEventDelegate windowMovedCallback, WindowEventDelegate windowResizedCallback, WindowClosingDelegate windowClosingCallback, WindowEventDelegate windowClosedCallback, WindowEventDelegate windowFocusChangeCallback HANDLE_ARG);

	virtual ~NativeWindowListener(void);

	virtual void windowMoved (Ogre::RenderWindow *rw);

	virtual void windowResized (Ogre::RenderWindow *rw);
	
	virtual bool windowClosing (Ogre::RenderWindow *rw);
	
	virtual void windowClosed (Ogre::RenderWindow *rw);
	
	virtual void windowFocusChange (Ogre::RenderWindow *rw);
};
