#include "Stdafx.h"

extern "C" _AnomalousExport String Window_getType(CEGUI::Window* window)
{
	return window->getType().c_str();
}

extern "C" _AnomalousExport size_t Window_getChildCount(CEGUI::Window* window)
{
	return window->getChildCount();
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildIndex(CEGUI::Window* window, uint index)
{
	return window->getChildByIndex(index);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildId(CEGUI::Window* window, uint id)
{
	return window->getChild(id);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildRecursive(CEGUI::Window* window, String name)
{
	return window->getChildRecursive(name);
}