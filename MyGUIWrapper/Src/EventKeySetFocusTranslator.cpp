#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeySetFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::Widget* old HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
		void fireEvent(MyGUI::Widget* sender, MyGUI::Widget* old)
	{
		nativeEvent(sender, old PASS_HANDLE_ARG);
	}
#endif

public:
	EventKeySetFocusTranslator(MyGUI::Widget* widget, EventKeySetFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventKeySetFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventKeySetFocus = MyGUI::newDelegate(this, &EventKeySetFocusTranslator::fireEvent);
#else
		widget->eventKeySetFocus = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventKeySetFocus = NULL;
	}
};

extern "C" _AnomalousExport EventKeySetFocusTranslator* EventKeySetFocusTranslator_Create(MyGUI::Widget* widget, EventKeySetFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventKeySetFocusTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}