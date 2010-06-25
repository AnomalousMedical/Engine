#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventComboAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::ComboBox* sender, size_t index);

private:
	MyGUI::ComboBox* widget;
	NativeEventDelegate nativeEvent;

public:
	EventComboAcceptTranslator(MyGUI::ComboBox* widget, EventComboAcceptTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventComboAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventComboAccept = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventComboAccept = NULL;
	}
};

extern "C" _AnomalousExport EventComboAcceptTranslator* EventComboAcceptTranslator_Create(MyGUI::ComboBox* widget, EventComboAcceptTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventComboAcceptTranslator(widget, nativeEventCallback);
}