#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventComboAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::ComboBox* sender, size_t index HANDLE_ARG);

private:
	MyGUI::ComboBox* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
		void fireEvent(MyGUI::ComboBox* sender, size_t index)
	{
		nativeEvent(sender, index PASS_HANDLE_ARG);
	}
#endif

public:
	EventComboAcceptTranslator(MyGUI::ComboBox* widget, EventComboAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventComboAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventComboAccept = MyGUI::newDelegate(this, &EventComboAcceptTranslator::fireEvent);
#else
		widget->eventComboAccept = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventComboAccept = NULL;
	}
};

extern "C" _AnomalousExport EventComboAcceptTranslator* EventComboAcceptTranslator_Create(MyGUI::ComboBox* widget, EventComboAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventComboAcceptTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}