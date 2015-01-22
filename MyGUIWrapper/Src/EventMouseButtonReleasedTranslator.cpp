#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseButtonReleasedTranslator : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton id HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton id)
	{
		nativeEvent(sender, left, top, id PASS_HANDLE_ARG);
	}
#endif

public:
	EventMouseButtonReleasedTranslator(MyGUI::Widget* widget, EventMouseButtonReleasedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseButtonReleasedTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMouseButtonReleased = MyGUI::newDelegate(this, &EventMouseButtonReleasedTranslator::fireEvent);
#else
		widget->eventMouseButtonReleased = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonReleased = NULL;
	}
};

extern "C" _AnomalousExport EventMouseButtonReleasedTranslator* EventMouseButtonReleasedTranslator_Create(MyGUI::Widget* widget, EventMouseButtonReleasedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseButtonReleasedTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}