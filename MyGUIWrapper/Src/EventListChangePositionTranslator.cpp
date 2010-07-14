#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventListChangePositionTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MultiList* sender, size_t index);

private:
	MyGUI::MultiList* widget;
	NativeEventDelegate nativeEvent;

public:
	EventListChangePositionTranslator(MyGUI::MultiList* widget, EventListChangePositionTranslator::NativeEventDelegate nativeEventCallback)
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

extern "C" _AnomalousExport EventListChangePositionTranslator* EventListChangePositionTranslator_Create(MyGUI::MultiList* widget, EventListChangePositionTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventListChangePositionTranslator(widget, nativeEventCallback);
}