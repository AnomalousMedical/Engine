#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport void Window_setVisibleSmooth(MyGUI::Window* window, bool value)
{
	window->setVisibleSmooth(value);
}

extern "C" _AnomalousExport void Window_destroySmooth(MyGUI::Window* window)
{
	window->destroySmooth();
}

extern "C" _AnomalousExport void Window_setAutoAlpha(MyGUI::Window* window, bool value)
{
	window->setAutoAlpha(value);
}

extern "C" _AnomalousExport bool Window_getAutoAlpha(MyGUI::Window* window)
{
	return window->getAutoAlpha();
}

extern "C" _AnomalousExport MyGUI::Widget* Window_getCaptionWidget(MyGUI::Window* window)
{
	return window->getCaptionWidget();
}

extern "C" _AnomalousExport void Window_setMinSize(MyGUI::Window* window, IntSize2 value)
{
	window->setMinSize(value.toIntSize());
}

extern "C" _AnomalousExport ThreeIntHack Window_getMinSize(MyGUI::Window* window)
{
	return window->getMinSize();
}

extern "C" _AnomalousExport void Window_setMaxSize(MyGUI::Window* window, IntSize2 value)
{
	window->setMaxSize(value.toIntSize());
}

extern "C" _AnomalousExport ThreeIntHack Window_getMaxSize(MyGUI::Window* window)
{
	return window->getMaxSize();
}

extern "C" _AnomalousExport bool Window_getSnap(MyGUI::Window* window)
{
	return window->getSnap();
}

extern "C" _AnomalousExport void Window_setSnap(MyGUI::Window* window, bool value)
{
	window->setSnap(value);
}

extern "C" _AnomalousExport void Window_setActionWidgetsEnabled(MyGUI::Window* window, bool value)
{
	window->setActionWidgetsEnabled(value);
}

#pragma warning(pop)