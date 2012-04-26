#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMouseDragTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int left, int top, MyGUI::MouseButton _id);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMouseDragTranslator(MyGUI::Widget* widget, EventMouseDragTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMouseDragTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseDrag = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseDrag = NULL;
	}
};

extern "C" _AnomalousExport EventMouseDragTranslator* EventMouseDragTranslator_Create(MyGUI::Widget* widget, EventMouseDragTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMouseDragTranslator(widget, nativeEventCallback);
}