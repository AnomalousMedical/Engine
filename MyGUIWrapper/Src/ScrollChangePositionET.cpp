#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class ScrollChangePositionET : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::ScrollBar* sender, size_t position);

private:
	MyGUI::ScrollBar* widget;
	NativeEventDelegate nativeEvent;

public:
	ScrollChangePositionET(MyGUI::ScrollBar* widget, ScrollChangePositionET::NativeEventDelegate nativeEventCallback)
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

extern "C" _AnomalousExport ScrollChangePositionET* ScrollChangePositionET_Create(MyGUI::ScrollBar* widget, ScrollChangePositionET::NativeEventDelegate nativeEventCallback)
{
	return new ScrollChangePositionET(widget, nativeEventCallback);
}