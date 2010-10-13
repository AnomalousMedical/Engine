#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeySetFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::Widget* old);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventKeySetFocusTranslator(MyGUI::Widget* widget, EventKeySetFocusTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventKeySetFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventKeySetFocus = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventKeySetFocus = NULL;
	}
};

extern "C" _AnomalousExport EventKeySetFocusTranslator* EventKeySetFocusTranslator_Create(MyGUI::Widget* widget, EventKeySetFocusTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventKeySetFocusTranslator(widget, nativeEventCallback);
}