#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMenuCtrlCloseTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MenuControl* sender);

private:
	MyGUI::MenuControl* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMenuCtrlCloseTranslator(MyGUI::MenuControl* widget, EventMenuCtrlCloseTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMenuCtrlCloseTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMenuCtrlClose = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMenuCtrlClose = NULL;
	}
};

extern "C" _AnomalousExport EventMenuCtrlCloseTranslator* EventMenuCtrlCloseTranslator_Create(MyGUI::MenuControl* widget, EventMenuCtrlCloseTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMenuCtrlCloseTranslator(widget, nativeEventCallback);
}
