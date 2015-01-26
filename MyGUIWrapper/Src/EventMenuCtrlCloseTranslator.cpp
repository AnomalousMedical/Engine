#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventMenuCtrlCloseTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::MenuControl* sender HANDLE_ARG);

private:
	MyGUI::MenuControl* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::MenuControl* sender)
	{
		nativeEvent(sender PASS_HANDLE_ARG);
	}
#endif

public:
	EventMenuCtrlCloseTranslator(MyGUI::MenuControl* widget, EventMenuCtrlCloseTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventMenuCtrlCloseTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventMenuCtrlClose = MyGUI::newDelegate(this, &EventMenuCtrlCloseTranslator::fireEvent);
#else
		widget->eventMenuCtrlClose = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventMenuCtrlClose = NULL;
	}
};

extern "C" _AnomalousExport EventMenuCtrlCloseTranslator* EventMenuCtrlCloseTranslator_Create(MyGUI::MenuControl* widget, EventMenuCtrlCloseTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMenuCtrlCloseTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}
