#include "StdAfx.h"
#include "AndroidWindow.h"

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
	return 0;
}

int AndroidWindow::getHeight()
{
	return 0;
}

void* AndroidWindow::getHandle()
{
	return 0;
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

//PInvoke
extern "C" _AnomalousExport NativeOSWindow* NativeOSWindow_create(NativeOSWindow* parent, String caption, int x, int y, int width, int height, bool floatOnParent)
{
	return new AndroidWindow();
}