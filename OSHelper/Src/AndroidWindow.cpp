#include "StdAfx.h"
#include "AndroidWindow.h"

static struct android_app* app = 0;
static float _screenDensity = 1.0f;

AndroidWindow::AndroidWindow()
{
	
}

AndroidWindow::~AndroidWindow()
{
	
}

void AndroidWindow::setTitle(String title)
{
	
}

void AndroidWindow::setSize(int width, int height)
{
	
}

int AndroidWindow::getWidth()
{
	return ANativeWindow_getWidth(app->window);
}

int AndroidWindow::getHeight()
{
	return ANativeWindow_getHeight(app->window);
}

void* AndroidWindow::getHandle()
{
	//On android we return the pointer to our app instead of the window, this allows more flexibility in
	//external libs.
	return (void*)app;
}

void AndroidWindow::show()
{
	
}

void AndroidWindow::toggleFullscreen()
{
	
}

void AndroidWindow::close()
{
	
}

void AndroidWindow::setMaximized(bool maximized)
{
	
}

bool AndroidWindow::getMaximized()
{
	return true;
}

void AndroidWindow::setCursor(CursorType cursor)
{
	
}

float AndroidWindow::getWindowScaling()
{
	return _screenDensity;
}

void AndroidWindow_setApp(struct android_app* setApp)
{
	app = setApp;
}

//PInvoke
extern "C" _AnomalousExport NativeOSWindow* NativeOSWindow_create(NativeOSWindow* parent, String caption, int x, int y, int width, int height, bool floatOnParent)
{
	return new AndroidWindow();
}

//This function is used to set some attributes that would otherwise require a bunch of jni calls, we can get them really easily
//on the c# side, however.
extern "C" _AnomalousExport void AndroidOSWindow_EasyAttributeSetup(float screenDensity)
{
	_screenDensity = screenDensity;
}