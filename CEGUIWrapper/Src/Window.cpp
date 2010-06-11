#include "Stdafx.h"

extern "C" _AnomalousExport CEGUI::Window* Window_getChildRecursive(CEGUI::Window* window, String name)
{
	return window->getChildRecursive(name);
}

extern "C" _AnomalousExport String Window_getType(CEGUI::Window* window)
{
	return window->getType().c_str();
}