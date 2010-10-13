#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseSetFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::Widget* old);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseSetFocusTranslator(MyGUI::Widget* widget, EventMouseSetFocusTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseSetFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseSetFocus = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseSetFocus = NULL;
	}
};

extern "C" _AnomalousExport EventMouseSetFocusTranslator* EventMouseSetFocusTranslator_Create(MyGUI::Widget* widget, EventMouseSetFocusTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseSetFocusTranslator(widget, nativeEventCallback);
}