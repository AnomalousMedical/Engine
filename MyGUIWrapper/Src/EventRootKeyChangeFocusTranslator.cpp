#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventRootKeyChangeFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, bool focus HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

public:
	EventRootKeyChangeFocusTranslator(MyGUI::Widget* widget, EventRootKeyChangeFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

#if FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, bool focus)
	{
		nativeEvent(sender, focus PASS_HANDLE_ARG);
	}
#endif

	virtual ~EventRootKeyChangeFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
#if FULL_AOT_COMPILE
		widget->eventRootKeyChangeFocus = MyGUI::newDelegate(this, &EventRootKeyChangeFocusTranslator::fireEvent);
#else
		widget->eventRootKeyChangeFocus = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventRootKeyChangeFocus = NULL;
	}
};

extern "C" _AnomalousExport EventRootKeyChangeFocusTranslator* EventRootKeyChangeFocusTranslator_Create(MyGUI::Widget* widget, EventRootKeyChangeFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventRootKeyChangeFocusTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}