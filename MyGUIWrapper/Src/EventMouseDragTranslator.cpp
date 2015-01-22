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

	void fireEvent(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton _id)
	{
		nativeEvent(sender, left, top, _id PASS_HANDLE_ARG);
	}

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
		widget->eventMouseDrag = MyGUI::newDelegate(this, &EventMouseDragTranslator::fireEvent);
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