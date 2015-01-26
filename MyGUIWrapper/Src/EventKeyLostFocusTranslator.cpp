#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeyLostFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::Widget* newFocus HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
		void fireEvent(MyGUI::Widget* sender, MyGUI::Widget* newFocus)
	{
		nativeEvent(sender, newFocus PASS_HANDLE_ARG);
	}
#endif

public:
	EventKeyLostFocusTranslator(MyGUI::Widget* widget, EventKeyLostFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventKeyLostFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventKeyLostFocus = MyGUI::newDelegate(this, &EventKeyLostFocusTranslator::fireEvent);
#else
		widget->eventKeyLostFocus = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventKeyLostFocus = NULL;
	}
};

extern "C" _AnomalousExport EventKeyLostFocusTranslator* EventKeyLostFocusTranslator_Create(MyGUI::Widget* widget, EventKeyLostFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventKeyLostFocusTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}