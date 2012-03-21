#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMenuCtrlAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MenuControl* sender, MyGUI::MenuItem* item);

private:
	MyGUI::MenuControl* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMenuCtrlAcceptTranslator(MyGUI::MenuControl* widget, EventMenuCtrlAcceptTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMenuCtrlAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMenuCtrlAccept = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMenuCtrlAccept = NULL;
	}
};

extern "C" _AnomalousExport EventMenuCtrlAcceptTranslator* EventMenuCtrlAcceptTranslator_Create(MyGUI::MenuControl* widget, EventMenuCtrlAcceptTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMenuCtrlAcceptTranslator(widget, nativeEventCallback);
}