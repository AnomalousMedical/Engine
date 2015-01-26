#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventComboChangePositionTranslator : public MyGUIEventTranslator
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
	EventComboChangePositionTranslator(MyGUI::ComboBox* widget, EventComboChangePositionTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventComboChangePositionTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventComboChangePosition = MyGUI::newDelegate(this, &EventComboChangePositionTranslator::fireEvent);
#else
		widget->eventComboChangePosition = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventComboChangePosition = NULL;
	}
};

extern "C" _AnomalousExport EventComboChangePositionTranslator* EventComboChangePositionTranslator_Create(MyGUI::ComboBox* widget, EventComboChangePositionTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventComboChangePositionTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}