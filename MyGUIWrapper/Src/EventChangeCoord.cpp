#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeCoord : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventChangeCoord(MyGUI::Widget* widget, EventChangeCoord::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventChangeCoord()
	{

	}

	virtual void bindEvent()
	{
		widget->eventChangeCoord = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventChangeCoord = NULL;
	}
};

extern "C" _AnomalousExport EventChangeCoord* EventChangeCoord_Create(MyGUI::Widget* widget, EventChangeCoord::NativeEventDelegate nativeEventCallback)
{
	return new EventChangeCoord(widget, nativeEventCallback);
}