#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMenuCtrlAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MenuCtrl* sender, MyGUI::MenuItem* item);

private:
	MyGUI::MenuCtrl* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMenuCtrlAcceptTranslator(MyGUI::MenuCtrl* widget, EventMenuCtrlAcceptTranslator::NativeEventDelegate nativeEventCallback)
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

extern "C" _AnomalousExport EventMenuCtrlAcceptTranslator* EventMenuCtrlAcceptTranslator_Create(MyGUI::MenuCtrl* widget, EventMenuCtrlAcceptTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMenuCtrlAcceptTranslator(widget, nativeEventCallback);
}