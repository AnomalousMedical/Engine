#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeyLostFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::Widget* newFocus);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventKeyLostFocusTranslator(MyGUI::Widget* widget, EventKeyLostFocusTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventKeyLostFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventKeyLostFocus = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventKeyLostFocus = NULL;
	}
};

extern "C" _AnomalousExport EventKeyLostFocusTranslator* EventKeyLostFocusTranslator_Create(MyGUI::Widget* widget, EventKeyLostFocusTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventKeyLostFocusTranslator(widget, nativeEventCallback);
}