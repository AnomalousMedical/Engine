#include "StdAfx.h"
#include "AndroidWindow.h"

static struct android_app* app;

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
	return (void*)app->window;
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
	return 1.0f;
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