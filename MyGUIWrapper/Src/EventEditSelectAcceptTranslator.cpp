#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventEditSelectAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::EditBox* sender);

private:
	MyGUI::EditBox* widget;
	NativeEventDelegate nativeEvent;

public:
	EventEditSelectAcceptTranslator(MyGUI::EditBox* widget, EventEditSelectAcceptTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventEditSelectAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventEditSelectAccept = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventEditSelectAccept = NULL;
	}
};

extern "C" _AnomalousExport EventEditSelectAcceptTranslator* EventEditSelectAcceptTranslator_Create(MyGUI::EditBox* widget, EventEditSelectAcceptTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventEditSelectAcceptTranslator(widget, nativeEventCallback);
}