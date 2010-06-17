#include "Stdafx.h"

extern "C" _AnomalousExport size_t Widget_getChildCount(MyGUI::Widget* widget)
{
	return widget->getChildCount();
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_getChildAt(MyGUI::Widget* widget, size_t index)
{
	return widget->getChildAt(index);
}