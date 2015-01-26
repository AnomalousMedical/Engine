#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeyButtonReleasedTranslator : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::KeyCode key HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
		void fireEvent(MyGUI::Widget* sender, MyGUI::KeyCode key)
	{
		nativeEvent(sender, key PASS_HANDLE_ARG);
	}
#endif

public:
	EventKeyButtonReleasedTranslator(MyGUI::Widget* widget, EventKeyButtonReleasedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventKeyButtonReleasedTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventKeyButtonReleased = MyGUI::newDelegate(this, &EventKeyButtonReleasedTranslator::fireEvent);
#else
		widget->eventKeyButtonReleased = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventKeyButtonReleased = NULL;
	}
};

extern "C" _AnomalousExport EventKeyButtonReleasedTranslator* EventKeyButtonReleasedTranslator_Create(MyGUI::Widget* widget, EventKeyButtonReleasedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventKeyButtonReleasedTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}