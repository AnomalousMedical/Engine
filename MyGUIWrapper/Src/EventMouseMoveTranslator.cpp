#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseMoveTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, int left, int top)
	{
		nativeEvent(sender, left, top PASS_HANDLE_ARG);
	}
#endif

public:
	EventMouseMoveTranslator(MyGUI::Widget* widget, EventMouseMoveTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseMoveTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMouseMove = MyGUI::newDelegate(this, &EventMouseMoveTranslator::fireEvent);
#else
		widget->eventMouseMove = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMouseMove = NULL;
	}
};

extern "C" _AnomalousExport EventMouseMoveTranslator* EventMouseMoveTranslator_Create(MyGUI::Widget* widget, EventMouseMoveTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseMoveTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}