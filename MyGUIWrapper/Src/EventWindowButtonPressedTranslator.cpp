#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventWindowButtonPressedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Window* sender, String name HANDLE_ARG);

private:
	MyGUI::Window* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

public:
	EventWindowButtonPressedTranslator(MyGUI::Window* widget, EventWindowButtonPressedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventWindowButtonPressedTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventWindowButtonPressed = MyGUI::newDelegate(this, &EventWindowButtonPressedTranslator::eventCallback);
	}

	virtual void unbindEvent()
	{
		widget->eventWindowButtonPressed = NULL;
	}

private:
	void eventCallback(MyGUI::Window* sender, const std::string& name)
	{
		nativeEvent(sender, name.c_str() PASS_HANDLE_ARG);
	}
};

extern "C" _AnomalousExport EventWindowButtonPressedTranslator* EventWindowButtonPressedTranslator_Create(MyGUI::Window* widget, EventWindowButtonPressedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventWindowButtonPressedTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}