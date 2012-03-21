#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventEditTextChangeTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::EditBox* sender);

private:
	MyGUI::EditBox* widget;
	NativeEventDelegate nativeEvent;

public:
	EventEditTextChangeTranslator(MyGUI::EditBox* widget, EventEditTextChangeTranslator::NativeEventDelegate nativeEventCallback)
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

extern "C" _AnomalousExport EventEditTextChangeTranslator* EventEditTextChangeTranslator_Create(MyGUI::EditBox* widget, EventEditTextChangeTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventEditTextChangeTranslator(widget, nativeEventCallback);
}