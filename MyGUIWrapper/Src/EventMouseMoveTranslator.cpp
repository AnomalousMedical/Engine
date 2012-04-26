#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseMoveTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseMoveTranslator(MyGUI::Widget* widget, EventMouseMoveTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseMoveTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseMove = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseMove = NULL;
	}
};

extern "C" _AnomalousExport EventMouseMoveTranslator* EventMouseMoveTranslator_Create(MyGUI::Widget* widget, EventMouseMoveTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseMoveTranslator(widget, nativeEventCallback);
}