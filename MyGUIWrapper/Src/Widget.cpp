#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

//UserData
extern "C" _AnomalousExport void Widget_setUserString(MyGUI::Widget* widget, String key, String value)
{
	widget->setUserString(key, value);
}

extern "C" _AnomalousExport String Widget_getUserString(MyGUI::Widget* widget, String key)
{
	return widget->getUserString(key).c_str();
}

extern "C" _AnomalousExport bool Widget_clearUserString(MyGUI::Widget* widget, String key)
{
	return widget->clearUserString(key);
}

extern "C" _AnomalousExport bool Widget_isUserString(MyGUI::Widget* widget, String key)
{
	return widget->isUserString(key);
}

extern "C" _AnomalousExport void Widget_clearUserStrings(MyGUI::Widget* widget)
{
	widget->clearUserStrings();
}

//Clipped Rectangle
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

extern "C" _AnomalousExport void Widget_setLeft(MyGUI::Widget* widget, int left)
{
	widget->setPosition(left, widget->getTop());
}

extern "C" _AnomalousExport int Widget_getRight(MyGUI::Widget* widget)
{
	return widget->getRight();
}

extern "C" _AnomalousExport void Widget_setRight(MyGUI::Widget* widget, int right)
{
	widget->setSize(right - widget->getLeft(), widget->getHeight());
}

extern "C" _AnomalousExport int Widget_getTop(MyGUI::Widget* widget)
{
	return widget->getTop();
}

extern "C" _AnomalousExport void Widget_setTop(MyGUI::Widget* widget, int top)
{
	widget->setPosition(widget->getLeft(), top);
}

extern "C" _AnomalousExport int Widget_getBottom(MyGUI::Widget* widget)
{
	return widget->getBottom();
}

extern "C" _AnomalousExport void Widget_setBottom(MyGUI::Widget* widget, int bottom)
{
	widget->setSize(widget->getWidth(), bottom - widget->getTop());
}

extern "C" _AnomalousExport int Widget_getWidth(MyGUI::Widget* widget)
{
	return widget->getWidth();
}

extern "C" _AnomalousExport void Widget_setWidth(MyGUI::Widget* widget, int width)
{
	widget->setSize(width, widget->getHeight());
}

extern "C" _AnomalousExport int Widget_getHeight(MyGUI::Widget* widget)
{
	return widget->getHeight();
}

extern "C" _AnomalousExport void Widget_setHeight(MyGUI::Widget* widget, int height)
{
	widget->setSize(widget->getWidth(), height);
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

extern "C" _AnomalousExport void Widget_setCoordIntCoord(MyGUI::Widget* widget, IntCoord& coord)
{
	widget->setCoord(coord.toIntCoord());
}

extern "C" _AnomalousExport IntCoord Widget_getCoord(MyGUI::Widget* widget)
{
	return widget->getCoord();
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

extern "C" _AnomalousExport void Widget_setVisible(MyGUI::Widget* widget, bool value)
{
	widget->setVisible(value);
}

extern "C" _AnomalousExport bool Widget_getVisible(MyGUI::Widget* widget)
{
	return widget->getVisible();
}

extern "C" _AnomalousExport void Widget_setAlign(MyGUI::Widget* widget, MyGUI::Align::Enum value)
{
	widget->setAlign(value);
}

extern "C" _AnomalousExport MyGUI::Align::Enum Widget_getAlign(MyGUI::Widget* widget)
{
	return getAlignEnumVal(widget->getAlign());
}

extern "C" _AnomalousExport void Widget_setAlpha(MyGUI::Widget* widget, float value)
{
	widget->setAlpha(value);
}

extern "C" _AnomalousExport float Widget_getAlpha(MyGUI::Widget* widget)
{
	return widget->getAlpha();
}

extern "C" _AnomalousExport void Widget_setInheritsAlpha(MyGUI::Widget* widget, bool value)
{
	widget->setInheritsAlpha(value);
}

extern "C" _AnomalousExport bool Widget_getInheritsAlpha(MyGUI::Widget* widget)
{
	return widget->getInheritsAlpha();
}

extern "C" _AnomalousExport bool Widget__setWidgetState(MyGUI::Widget* widget, String value)
{
	return widget->_setWidgetState(value);
}

extern "C" _AnomalousExport void Widget_setColour(MyGUI::Widget* widget, Color value)
{
	widget->setColour(value.toMyGUI());
}

extern "C" _AnomalousExport bool Widget_isRootWidget(MyGUI::Widget* widget)
{
	return widget->isRootWidget();
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_getParent(MyGUI::Widget* widget)
{
	return widget->getParent();
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_findWidget(MyGUI::Widget* widget, String name)
{
	return widget->findWidget(name);
}

extern "C" _AnomalousExport void Widget_setNeedKeyFocus(MyGUI::Widget* widget, bool value)
{
	widget->setNeedKeyFocus(value);
}

extern "C" _AnomalousExport bool Widget_getNeedKeyFocus(MyGUI::Widget* widget)
{
	return widget->getNeedKeyFocus();
}

extern "C" _AnomalousExport void Widget_setNeedMouseFocus(MyGUI::Widget* widget, bool value)
{
	widget->setNeedMouseFocus(value);
}

extern "C" _AnomalousExport bool Widget_getForwardMouseWheelToParent(MyGUI::Widget* widget)
{
	return widget->getForwardMouseWheelToParent();
}

extern "C" _AnomalousExport void Widget_setForwardMouseWheelToParent(MyGUI::Widget* widget, bool value)
{
	widget->setForwardMouseWheelToParent(value);
}

extern "C" _AnomalousExport bool Widget_getNeedMouseFocus(MyGUI::Widget* widget)
{
	return widget->getNeedMouseFocus();
}

extern "C" _AnomalousExport void Widget_setInheritsPick(MyGUI::Widget* widget, bool value)
{
	widget->setInheritsPick(value);
}

extern "C" _AnomalousExport bool Widget_getInheritsPick(MyGUI::Widget* widget)
{
	return widget->getInheritsPick();
}

extern "C" _AnomalousExport void Widget_setMaskPick(MyGUI::Widget* widget, String filename)
{
	widget->setMaskPick(filename);
}

extern "C" _AnomalousExport void Widget_setEnabled(MyGUI::Widget* widget, bool value)
{
	widget->setEnabled(value);
}

extern "C" _AnomalousExport void Widget_setEnabledSilent(MyGUI::Widget* widget, bool value)
{
	widget->setEnabledSilent(value);
}

extern "C" _AnomalousExport bool Widget_getEnabled(MyGUI::Widget* widget)
{
	return widget->getEnabled();
}

extern "C" _AnomalousExport void Widget_setPointer(MyGUI::Widget* widget, String value)
{
	widget->setPointer(value);
}

extern "C" _AnomalousExport String Widget_getPointer(MyGUI::Widget* widget)
{
	return widget->getPointer().c_str();
}

extern "C" _AnomalousExport String Widget_getLayerName(MyGUI::Widget* widget)
{
	return widget->getLayer()->getName().c_str();
}

extern "C" _AnomalousExport IntCoord Widget_getClientCoord(MyGUI::Widget* widget)
{
	return widget->getClientCoord();
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_getClientWidget(MyGUI::Widget* widget)
{
	return widget->getClientWidget();
}

extern "C" _AnomalousExport void Widget_setNeedToolTip(MyGUI::Widget* widget, bool value)
{
	widget->setNeedToolTip(value);
}

extern "C" _AnomalousExport bool Widget_getNeedToolTip(MyGUI::Widget* widget)
{
	return widget->getNeedToolTip();
}

extern "C" _AnomalousExport void Widget_detachFromWidget(MyGUI::Widget* widget)
{
	widget->detachFromWidget();
}

extern "C" _AnomalousExport void Widget_detachFromWidget2(MyGUI::Widget* widget, String layer)
{
	widget->detachFromWidget(layer);
}

extern "C" _AnomalousExport void Widget_attachToWidget(MyGUI::Widget* widget, MyGUI::Widget* parent)
{
	widget->attachToWidget(parent);
}

extern "C" _AnomalousExport void Widget_attachToWidget2(MyGUI::Widget* widget, MyGUI::Widget* parent, MyGUI::WidgetStyle style)
{
	widget->attachToWidget(parent, style);
}

extern "C" _AnomalousExport void Widget_attachToWidget3(MyGUI::Widget* widget, MyGUI::Widget* parent, MyGUI::WidgetStyle style, String layer)
{
	widget->attachToWidget(parent, style, layer);
}

extern "C" _AnomalousExport void Widget_changeWidgetSkin(MyGUI::Widget* widget, String skinname)
{
	widget->changeWidgetSkin(skinname);
}

extern "C" _AnomalousExport void Widget_setWidgetStyle(MyGUI::Widget* widget, MyGUI::WidgetStyle style)
{
	widget->setWidgetStyle(style);
}

extern "C" _AnomalousExport void Widget_setWidgetStyle2(MyGUI::Widget* widget, MyGUI::WidgetStyle style, String layer)
{
	widget->setWidgetStyle(style, layer);
}

extern "C" _AnomalousExport MyGUI::WidgetStyle::Enum Widget_getWidgetStyle(MyGUI::Widget* widget)
{
	MyGUI::WidgetStyle style = widget->getWidgetStyle();
	if(style == MyGUI::WidgetStyle::Child)
	{
		return MyGUI::WidgetStyle::Child;
	}
	else if(style == MyGUI::WidgetStyle::Popup)
	{
		return MyGUI::WidgetStyle::Popup;
	}
	else if(style == MyGUI::WidgetStyle::Overlapped)
	{
		return MyGUI::WidgetStyle::Overlapped;
	}
	return MyGUI::WidgetStyle::MAX;
}

extern "C" _AnomalousExport void Widget_setProperty(MyGUI::Widget* widget, String key, String value)
{
	widget->setProperty(key, value);
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_createWidgetT(MyGUI::Widget* widget, String type, String skin, int left, int top, int width, int height, MyGUI::Align::Enum align, String name)
{
	return widget->createWidgetT(type, skin, left, top, width, height, align, name);
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_createWidgetRealT(MyGUI::Widget* widget, String type, String skin, int left, int top, int width, int height, MyGUI::Align::Enum align, String name)
{
	return widget->createWidgetRealT(type, skin, left, top, width, height, align, name);
}

extern "C" _AnomalousExport MyGUI::Widget* Widget_findWidgetChildSkin(MyGUI::Widget* widget, String name)
{
	return widget->findWidgetChildSkin(name);
}

extern "C" _AnomalousExport String Widget_getName(MyGUI::Widget* widget)
{
	return widget->getName().c_str();
}

extern "C" _AnomalousExport MyGUI::ISubWidgetText* Widget_getSubWidgetText(MyGUI::Widget* widget)
{
	return widget->getSubWidgetText();
}

extern "C" _AnomalousExport void Widget_setDestructorCallback(MyGUI::Widget* widget, MyGUI::WidgetDestructorCallback widgetDestructorCallback)
{
	widget->_setDestructorCallback(widgetDestructorCallback);
}

extern "C" _AnomalousExport ThreeIntHack Widget_getSize(MyGUI::Widget* widget)
{
	return ThreeIntHack(widget->getWidth(), widget->getHeight());
}

#pragma warning(pop)