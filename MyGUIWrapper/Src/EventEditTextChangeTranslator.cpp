#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventEditTextChangeTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Edit* sender);

private:
	MyGUI::Edit* widget;
	NativeEventDelegate nativeEvent;

public:
	EventEditTextChangeTranslator(MyGUI::Edit* widget, EventEditTextChangeTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventEditTextChangeTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventEditTextChange = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventEditTextChange = NULL;
	}
};

extern "C" _AnomalousExport EventEditTextChangeTranslator* EventEditTextChangeTranslator_Create(MyGUI::Edit* widget, EventEditTextChangeTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventEditTextChangeTranslator(widget, nativeEventCallback);
}