#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventEditSelectAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::EditBox* sender HANDLE_ARG);

private:
	MyGUI::EditBox* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::EditBox* sender)
	{
		nativeEvent(sender PASS_HANDLE_ARG);
	}
#endif

public:
	EventEditSelectAcceptTranslator(MyGUI::EditBox* widget, EventEditSelectAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventEditSelectAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventEditSelectAccept = MyGUI::newDelegate(this, &EventEditSelectAcceptTranslator::fireEvent);
#else
		widget->eventEditSelectAccept = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventEditSelectAccept = NULL;
	}
};

extern "C" _AnomalousExport EventEditSelectAcceptTranslator* EventEditSelectAcceptTranslator_Create(MyGUI::EditBox* widget, EventEditSelectAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventEditSelectAcceptTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}