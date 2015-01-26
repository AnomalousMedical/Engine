#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseButtonDoubleClickTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender)
	{
		nativeEvent(sender PASS_HANDLE_ARG);
	}
#endif

public:
	EventMouseButtonDoubleClickTranslator(MyGUI::Widget* widget, EventMouseButtonDoubleClickTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseButtonDoubleClickTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMouseButtonDoubleClick = MyGUI::newDelegate(this, &EventMouseButtonDoubleClickTranslator::fireEvent);
#else
		widget->eventMouseButtonDoubleClick = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonDoubleClick = NULL;
	}
};

extern "C" _AnomalousExport EventMouseButtonDoubleClickTranslator* EventMouseButtonDoubleClickTranslator_Create(MyGUI::Widget* widget, EventMouseButtonDoubleClickTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseButtonDoubleClickTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}