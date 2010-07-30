#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseButtonPressedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton id);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseButtonPressedTranslator(MyGUI::Widget* widget, EventMouseButtonPressedTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseButtonPressedTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseButtonPressed = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonPressed = NULL;
	}
};

extern "C" _AnomalousExport EventMouseButtonPressedTranslator* EventMouseButtonPressedTranslator_Create(MyGUI::Widget* widget, EventMouseButtonPressedTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseButtonPressedTranslator(widget, nativeEventCallback);
}