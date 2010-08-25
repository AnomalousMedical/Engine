#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseWheelTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int relWheel);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseWheelTranslator(MyGUI::Widget* widget, EventMouseWheelTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseWheelTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseWheel = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseWheel = NULL;
	}
};

extern "C" _AnomalousExport EventMouseWheelTranslator* EventMouseWheelTranslator_Create(MyGUI::Widget* widget, EventMouseWheelTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseWheelTranslator(widget, nativeEventCallback);
}