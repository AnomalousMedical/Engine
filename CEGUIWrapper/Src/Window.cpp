#include "Stdafx.h"

extern "C" _AnomalousExport CEGUI::Window* Window_getChildRecursive(CEGUI::Window* window, String name)
{
	return window->getChildRecursive(name);
}