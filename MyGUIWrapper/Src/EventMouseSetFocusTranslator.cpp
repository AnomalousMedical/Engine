#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseSetFocusTranslator : public MyGUIEventTranslator
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
	EventMouseSetFocusTranslator(MyGUI::Widget* widget, EventMouseSetFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseSetFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMouseSetFocus = MyGUI::newDelegate(this, &EventMouseSetFocusTranslator::fireEvent);
#else
		widget->eventMouseSetFocus = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMouseSetFocus = NULL;
	}
};

extern "C" _AnomalousExport EventMouseSetFocusTranslator* EventMouseSetFocusTranslator_Create(MyGUI::Widget* widget, EventMouseSetFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseSetFocusTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}