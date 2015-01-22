#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseButtonPressedTranslator : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton id HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

	void fireEvent(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton id)
	{
		nativeEvent(sender, left, top, id PASS_HANDLE_ARG);
	}

public:
	EventMouseButtonPressedTranslator(MyGUI::Widget* widget, EventMouseButtonPressedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseButtonPressedTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseButtonPressed = MyGUI::newDelegate(this, &EventMouseButtonPressedTranslator::fireEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonPressed = NULL;
	}
};

extern "C" _AnomalousExport EventMouseButtonPressedTranslator* EventMouseButtonPressedTranslator_Create(MyGUI::Widget* widget, EventMouseButtonPressedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseButtonPressedTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}