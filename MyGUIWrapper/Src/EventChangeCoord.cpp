#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeCoord : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender HANDLE_ARG);

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
	EventChangeCoord(MyGUI::Widget* widget, EventChangeCoord::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventChangeCoord()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventChangeCoord = MyGUI::newDelegate(this, &EventChangeCoord::fireEvent);
#else
		widget->eventChangeCoord = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventChangeCoord = NULL;
	}
};

extern "C" _AnomalousExport EventChangeCoord* EventChangeCoord_Create(MyGUI::Widget* widget, EventChangeCoord::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventChangeCoord(widget, nativeEventCallback PASS_HANDLE_ARG);
}