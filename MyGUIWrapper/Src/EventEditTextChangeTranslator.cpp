#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventEditTextChangeTranslator : public MyGUIEventTranslator
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
	EventEditTextChangeTranslator(MyGUI::EditBox* widget, EventEditTextChangeTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventEditTextChangeTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventEditTextChange = MyGUI::newDelegate(this, &EventEditTextChangeTranslator::fireEvent);
#else
		widget->eventEditTextChange = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventEditTextChange = NULL;
	}
};

extern "C" _AnomalousExport EventEditTextChangeTranslator* EventEditTextChangeTranslator_Create(MyGUI::EditBox* widget, EventEditTextChangeTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventEditTextChangeTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}