#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventRootMouseChangeFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, bool focus);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventRootMouseChangeFocusTranslator(MyGUI::Widget* widget, EventRootMouseChangeFocusTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventRootMouseChangeFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventRootMouseChangeFocus = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventRootMouseChangeFocus = NULL;
	}
};

extern "C" _AnomalousExport EventRootMouseChangeFocusTranslator* EventRootMouseChangeFocusTranslator_Create(MyGUI::Widget* widget, EventRootMouseChangeFocusTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventRootMouseChangeFocusTranslator(widget, nativeEventCallback);
}