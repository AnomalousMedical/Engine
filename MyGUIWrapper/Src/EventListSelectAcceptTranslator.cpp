#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventListSelectAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MultiListBox* sender, size_t index HANDLE_ARG);

private:
	MyGUI::MultiListBox* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::MultiListBox* sender, size_t index)
	{
		nativeEvent(sender, index PASS_HANDLE_ARG);
	}
#endif

public:
	EventListSelectAcceptTranslator(MyGUI::MultiListBox* widget, EventListSelectAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventListSelectAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventListSelectAccept = MyGUI::newDelegate(this, &EventListSelectAcceptTranslator::fireEvent);
#else
		widget->eventListSelectAccept = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventListSelectAccept = NULL;
	}
};

extern "C" _AnomalousExport EventListSelectAcceptTranslator* EventListSelectAcceptTranslator_Create(MyGUI::MultiListBox* widget, EventListSelectAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventListSelectAcceptTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}