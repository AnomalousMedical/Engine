#include "Stdafx.h"

extern "C" _AnomalousExport int Widget_getAbsoluteLeft(MyGUI::Widget* widget)
{
	return widget->getAbsoluteLeft();
}

extern "C" _AnomalousExport int Widget_getAbsoluteTop(MyGUI::Widget* widget)
{
	return widget->getAbsoluteTop();
}

extern "C" _AnomalousExport int Widget_getLeft(MyGUI::Widget* widget)
{
	return widget->getLeft();
}

extern "C" _AnomalousExport int Widget_getRight(MyGUI::Widget* widget)
{
	return widget->getRight();
}

extern "C" _AnomalousExport int Widget_getTop(MyGUI::Widget* widget)
{
	return widget->getTop();
}

extern "C" _AnomalousExport int Widget_getBottom(MyGUI::Widget* widget)
{
	return widget->getBottom();
}

extern "C" _AnomalousExport int Widget_getWidth(MyGUI::Widget* widget)
{
	return widget->getWidth();
}

extern "C" _AnomalousExport int Widget_getHeight(MyGUI::Widget* widget)
{
	return widget->getHeight();
}

extern "C" _AnomalousExport void Widget_setPosition(MyGUI::Widget* widget, int left, int top)
{
	widget->setPosition(left, top);
}

extern "C" _AnomalousExport void Widget_setSize(MyGUI::Widget* widget, int width, int height)
{
	widget->setSize(width, height);
}

extern "C" _AnomalousExport void Widget_setCoord(MyGUI::Widget* widget, int left, int top, int width, int height)
{
	widget->setCoord(left, top, width, height);
}

extern "C" _AnomalousExport void Widget_setRealPosition(MyGUI::Widget* widget, float left, float top)
{
	widget->setRealPosition(left, top);
}

extern "C" _AnomalousExport void Widget_setRealSize(MyGUI::Widget* widget, float width, float height)
{
	widget->setRealSize(width, height);
}

extern "C" _AnomalousExport void Widget_setRealCoord(MyGUI::Widget* widget, float left, float top, float width, float height)
{
	widget->setRealCoord(left, top, width, height);
}

extern "C" _AnomalousExport size_t Widget_getChildCount(MyGUI::Widget* widget)
{
	return widget->getChildCount();
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_getChildAt(MyGUI::Widget* widget, size_t index)
{
	return widget->getChildAt(index);
}