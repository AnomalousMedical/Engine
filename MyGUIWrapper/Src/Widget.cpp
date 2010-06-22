#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

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

extern "C" _AnomalousExport void Widget_setVisible(MyGUI::Widget* widget, bool value)
{
	widget->setVisible(value);
}

extern "C" _AnomalousExport bool Widget_isVisible(MyGUI::Widget* widget)
{
	return widget->isVisible();
}

extern "C" _AnomalousExport void Widget_setAlign(MyGUI::Widget* widget, MyGUI::Align::Enum value)
{
	widget->setAlign(value);
}

extern "C" _AnomalousExport MyGUI::Align::Enum Widget_getAlign(MyGUI::Widget* widget)
{
	return getAlignEnumVal(widget->getAlign());
}

extern "C" _AnomalousExport void Widget_setCaption(MyGUI::Widget* widget, UStringIn value)
{
	widget->setCaption(value);
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* Widget_getCaption(MyGUI::Widget* widget)
{
	return widget->getCaption().c_str();
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

extern "C" _AnomalousExport bool Widget_isInheritsAlpha(MyGUI::Widget* widget)
{
	return widget->isInheritsAlpha();
}

extern "C" _AnomalousExport bool Widget_setState(MyGUI::Widget* widget, String value)
{
	return widget->setState(value);
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

extern "C" _AnomalousExport bool Widget_isNeedKeyFocus(MyGUI::Widget* widget)
{
	return widget->isNeedKeyFocus();
}

extern "C" _AnomalousExport void Widget_setNeedMouseFocus(MyGUI::Widget* widget, bool value)
{
	widget->setNeedMouseFocus(value);
}

extern "C" _AnomalousExport bool Widget_isNeedMouseFocus(MyGUI::Widget* widget)
{
	return widget->isNeedMouseFocus();
}

extern "C" _AnomalousExport void Widget_setInheritsPick(MyGUI::Widget* widget, bool value)
{
	widget->setInheritsPick(value);
}

extern "C" _AnomalousExport bool Widget_isInheritsPick(MyGUI::Widget* widget)
{
	return widget->isInheritsPick();
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

extern "C" _AnomalousExport bool Widget_isEnabled(MyGUI::Widget* widget)
{
	return widget->isEnabled();
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
	return widget->getLayerName().c_str();
}

extern "C" _AnomalousExport MyGUI::IntCoord Widget_getClientCoord(MyGUI::Widget* widget)
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

extern "C" _AnomalousExport void Widget_setEnableToolTip(MyGUI::Widget* widget, bool value)
{
	widget->setEnableToolTip(value);
}

extern "C" _AnomalousExport bool Widget_getEnableToolTip(MyGUI::Widget* widget)
{
	return widget->getEnableToolTip();
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

extern "C" _AnomalousExport MyGUI::WidgetStyle Widget_getWidgetStyle(MyGUI::Widget* widget)
{
	return widget->getWidgetStyle();
}

extern "C" _AnomalousExport void Widget_setProperty(MyGUI::Widget* widget, String key, String value)
{
	widget->setProperty(key, value);
}

extern "C" _AnomalousExport void Widget_setCaptionWithNewLine(MyGUI::Widget* widget, String value)
{
	widget->setCaptionWithNewLine(value);
}

#pragma warning(pop)