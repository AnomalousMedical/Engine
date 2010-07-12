#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseButtonDoubleClickTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseButtonDoubleClickTranslator(MyGUI::Widget* widget, EventMouseButtonDoubleClickTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseButtonDoubleClickTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseButtonDoubleClick = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonDoubleClick = NULL;
	}
};

extern "C" _AnomalousExport EventMouseButtonDoubleClickTranslator* EventMouseButtonDoubleClickTranslator_Create(MyGUI::Widget* widget, EventMouseButtonDoubleClickTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseButtonDoubleClickTranslator(widget, nativeEventCallback);
}