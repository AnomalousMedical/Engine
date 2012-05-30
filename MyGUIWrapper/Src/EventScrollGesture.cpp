#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventScrollGesture : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int absx, int absy, int deltax, int deltay);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventScrollGesture(MyGUI::Widget* widget, EventScrollGesture::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventScrollGesture()
	{

	}

	virtual void bindEvent()
	{
		widget->eventScrollGesture = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventScrollGesture = NULL;
	}
};

extern "C" _AnomalousExport EventScrollGesture* EventScrollGesture_Create(MyGUI::Widget* widget, EventScrollGesture::NativeEventDelegate nativeEventCallback)
{
	return new EventScrollGesture(widget, nativeEventCallback);
}