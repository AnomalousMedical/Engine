#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseLostFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::Widget* newFocus);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseLostFocusTranslator(MyGUI::Widget* widget, EventMouseLostFocusTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseLostFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseLostFocus = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseLostFocus = NULL;
	}
};

extern "C" _AnomalousExport EventMouseLostFocusTranslator* EventMouseLostFocusTranslator_Create(MyGUI::Widget* widget, EventMouseLostFocusTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseLostFocusTranslator(widget, nativeEventCallback);
}