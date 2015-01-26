#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventListChangePositionTranslator : public MyGUIEventTranslator
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
	EventListChangePositionTranslator(MyGUI::MultiListBox* widget, EventListChangePositionTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventListChangePositionTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventListChangePosition = MyGUI::newDelegate(this, &EventListChangePositionTranslator::fireEvent);
#else
		widget->eventListChangePosition = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventListChangePosition = NULL;
	}
};

extern "C" _AnomalousExport EventListChangePositionTranslator* EventListChangePositionTranslator_Create(MyGUI::MultiListBox* widget, EventListChangePositionTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventListChangePositionTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}