#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventRootMouseChangeFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, bool focus HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, bool focus)
	{
		nativeEvent(sender, focus PASS_HANDLE_ARG);
	}
#endif

public:
	EventRootMouseChangeFocusTranslator(MyGUI::Widget* widget, EventRootMouseChangeFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventRootMouseChangeFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventRootMouseChangeFocus = MyGUI::newDelegate(this, &EventRootMouseChangeFocusTranslator::fireEvent);
#else
		widget->eventRootMouseChangeFocus = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventRootMouseChangeFocus = NULL;
	}
};

extern "C" _AnomalousExport EventRootMouseChangeFocusTranslator* EventRootMouseChangeFocusTranslator_Create(MyGUI::Widget* widget, EventRootMouseChangeFocusTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventRootMouseChangeFocusTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}