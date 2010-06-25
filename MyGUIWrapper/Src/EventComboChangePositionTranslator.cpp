#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventComboChangePositionTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::ComboBox* sender, size_t index);

private:
	MyGUI::ComboBox* widget;
	NativeEventDelegate nativeEvent;

public:
	EventComboChangePositionTranslator(MyGUI::ComboBox* widget, EventComboChangePositionTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventComboChangePositionTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventComboChangePosition = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventComboChangePosition = NULL;
	}
};

extern "C" _AnomalousExport EventComboChangePositionTranslator* EventComboChangePositionTranslator_Create(MyGUI::ComboBox* widget, EventComboChangePositionTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventComboChangePositionTranslator(widget, nativeEventCallback);
}