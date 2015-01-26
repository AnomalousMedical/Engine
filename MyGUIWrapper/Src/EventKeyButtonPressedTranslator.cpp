#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeyButtonPressedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::KeyCode key, MyGUI::Char _char HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, MyGUI::KeyCode key, MyGUI::Char _char)
	{
		nativeEvent(sender, key, _char PASS_HANDLE_ARG);
	}
#endif

public:
	EventKeyButtonPressedTranslator(MyGUI::Widget* widget, EventKeyButtonPressedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventKeyButtonPressedTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventKeyButtonPressed = MyGUI::newDelegate(this, &EventKeyButtonPressedTranslator::fireEvent);
#else
		widget->eventKeyButtonPressed = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventKeyButtonPressed = NULL;
	}
};

extern "C" _AnomalousExport EventKeyButtonPressedTranslator* EventKeyButtonPressedTranslator_Create(MyGUI::Widget* widget, EventKeyButtonPressedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventKeyButtonPressedTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}