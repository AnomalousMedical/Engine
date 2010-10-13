#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventRootKeyChangeFocusTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, bool focus);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventRootKeyChangeFocusTranslator(MyGUI::Widget* widget, EventRootKeyChangeFocusTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventRootKeyChangeFocusTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventRootKeyChangeFocus = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventRootKeyChangeFocus = NULL;
	}
};

extern "C" _AnomalousExport EventRootKeyChangeFocusTranslator* EventRootKeyChangeFocusTranslator_Create(MyGUI::Widget* widget, EventRootKeyChangeFocusTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventRootKeyChangeFocusTranslator(widget, nativeEventCallback);
}