#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseLostFocusTranslator : public MyGUIEventTranslator
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
	EventMouseLostFocusTranslator(MyGUI::Widget* widget, EventMouseLostFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseLostFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMouseLostFocus = MyGUI::newDelegate(this, &EventMouseLostFocusTranslator::fireEvent);
#else
		widget->eventMouseLostFocus = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMouseLostFocus = NULL;
	}
};

extern "C" _AnomalousExport EventMouseLostFocusTranslator* EventMouseLostFocusTranslator_Create(MyGUI::Widget* widget, EventMouseLostFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseLostFocusTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}