#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventListChangePositionTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MultiListBox* sender, size_t index);

private:
	MyGUI::MultiListBox* widget;
	NativeEventDelegate nativeEvent;

public:
	EventListChangePositionTranslator(MyGUI::MultiListBox* widget, EventListChangePositionTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventListChangePositionTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventListChangePosition = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventListChangePosition = NULL;
	}
};

extern "C" _AnomalousExport EventListChangePositionTranslator* EventListChangePositionTranslator_Create(MyGUI::MultiListBox* widget, EventListChangePositionTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventListChangePositionTranslator(widget, nativeEventCallback);
}