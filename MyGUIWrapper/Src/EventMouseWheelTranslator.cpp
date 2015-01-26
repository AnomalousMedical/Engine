#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseWheelTranslator : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender, int relWheel HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, int relWheel)
	{
		nativeEvent(sender, relWheel PASS_HANDLE_ARG);
	}
#endif

public:
	EventMouseWheelTranslator(MyGUI::Widget* widget, EventMouseWheelTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseWheelTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMouseWheel = MyGUI::newDelegate(this, &EventMouseWheelTranslator::fireEvent);
#else
		widget->eventMouseWheel = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMouseWheel = NULL;
	}
};

extern "C" _AnomalousExport EventMouseWheelTranslator* EventMouseWheelTranslator_Create(MyGUI::Widget* widget, EventMouseWheelTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseWheelTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}