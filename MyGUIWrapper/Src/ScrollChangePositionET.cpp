#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class ScrollChangePositionET : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::VScroll* sender, size_t position);

private:
	MyGUI::VScroll* widget;
	NativeEventDelegate nativeEvent;

public:
	ScrollChangePositionET(MyGUI::VScroll* widget, ScrollChangePositionET::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~ScrollChangePositionET()
	{

	}

	virtual void bindEvent()
	{
		widget->eventScrollChangePosition = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventScrollChangePosition = NULL;
	}
};

extern "C" _AnomalousExport ScrollChangePositionET* ScrollChangePositionET_Create(MyGUI::VScroll* widget, ScrollChangePositionET::NativeEventDelegate nativeEventCallback)
{
	return new ScrollChangePositionET(widget, nativeEventCallback);
}