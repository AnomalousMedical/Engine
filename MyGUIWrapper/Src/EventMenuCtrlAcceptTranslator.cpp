#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMenuCtrlAcceptTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MenuControl* sender, MyGUI::MenuItem* item HANDLE_ARG);

private:
	MyGUI::MenuControl* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
		void fireEvent(MyGUI::MenuControl* sender, MyGUI::MenuItem* item)
	{
		nativeEvent(sender, item PASS_HANDLE_ARG);
	}
#endif
public:
	EventMenuCtrlAcceptTranslator(MyGUI::MenuControl* widget, EventMenuCtrlAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMenuCtrlAcceptTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMenuCtrlAccept = MyGUI::newDelegate(this, &EventMenuCtrlAcceptTranslator::fireEvent);
#else
		widget->eventMenuCtrlAccept = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMenuCtrlAccept = NULL;
	}
};

extern "C" _AnomalousExport EventMenuCtrlAcceptTranslator* EventMenuCtrlAcceptTranslator_Create(MyGUI::MenuControl* widget, EventMenuCtrlAcceptTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMenuCtrlAcceptTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}