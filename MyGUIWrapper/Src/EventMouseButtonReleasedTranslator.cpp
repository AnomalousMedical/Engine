#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseButtonReleasedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton id);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseButtonReleasedTranslator(MyGUI::Widget* widget, EventMouseButtonReleasedTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseButtonReleasedTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseButtonReleased = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonReleased = NULL;
	}
};

extern "C" _AnomalousExport EventMouseButtonReleasedTranslator* EventMouseButtonReleasedTranslator_Create(MyGUI::Widget* widget, EventMouseButtonReleasedTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseButtonReleasedTranslator(widget, nativeEventCallback);
}