#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventListSelectAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MultiListBox* sender, size_t index);

private:
	MyGUI::MultiListBox* widget;
	NativeEventDelegate nativeEvent;

public:
	EventListSelectAcceptTranslator(MyGUI::MultiListBox* widget, EventListSelectAcceptTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventListSelectAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventListSelectAccept = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventListSelectAccept = NULL;
	}
};

extern "C" _AnomalousExport EventListSelectAcceptTranslator* EventListSelectAcceptTranslator_Create(MyGUI::MultiListBox* widget, EventListSelectAcceptTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventListSelectAcceptTranslator(widget, nativeEventCallback);
}