#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventScrollGesture : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int absx, int absy, int deltax, int deltay HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
		void fireEvent(MyGUI::Widget* sender, int absx, int absy, int deltax, int deltay)
	{
		nativeEvent(sender, absx, absy, deltax, deltay PASS_HANDLE_ARG);
	}
#endif

public:
	EventScrollGesture(MyGUI::Widget* widget, EventScrollGesture::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventScrollGesture()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventScrollGesture = MyGUI::newDelegate(this, &EventScrollGesture::fireEvent);
#else
		widget->eventScrollGesture = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventScrollGesture = NULL;
	}
};

extern "C" _AnomalousExport EventScrollGesture* EventScrollGesture_Create(MyGUI::Widget* widget, EventScrollGesture::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventScrollGesture(widget, nativeEventCallback PASS_HANDLE_ARG);
}