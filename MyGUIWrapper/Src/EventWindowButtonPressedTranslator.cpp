#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventWindowButtonPressedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Window* sender, String name);

private:
	MyGUI::Window* widget;
	NativeEventDelegate nativeEvent;

public:
	EventWindowButtonPressedTranslator(MyGUI::Window* widget, EventWindowButtonPressedTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
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
		nativeEvent(sender, name.c_str());
	}
};

extern "C" _AnomalousExport EventWindowButtonPressedTranslator* EventWindowButtonPressedTranslator_Create(MyGUI::Window* widget, EventWindowButtonPressedTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventWindowButtonPressedTranslator(widget, nativeEventCallback);
}