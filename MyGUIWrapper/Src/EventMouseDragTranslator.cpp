#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseDragTranslator : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton _id HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton _id)
	{
		nativeEvent(sender, left, top, _id PASS_HANDLE_ARG);
	}
#endif

public:
	EventMouseDragTranslator(MyGUI::Widget* widget, EventMouseDragTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMouseDragTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMouseDrag = MyGUI::newDelegate(this, &EventMouseDragTranslator::fireEvent);
#else
		widget->eventMouseDrag = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMouseDrag = NULL;
	}
};

extern "C" _AnomalousExport EventMouseDragTranslator* EventMouseDragTranslator_Create(MyGUI::Widget* widget, EventMouseDragTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMouseDragTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}