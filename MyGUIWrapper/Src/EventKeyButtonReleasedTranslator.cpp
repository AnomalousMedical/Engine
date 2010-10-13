#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeyButtonReleasedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::KeyCode key);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventKeyButtonReleasedTranslator(MyGUI::Widget* widget, EventKeyButtonReleasedTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventKeyButtonReleasedTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventKeyButtonReleased = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventKeyButtonReleased = NULL;
	}
};

extern "C" _AnomalousExport EventKeyButtonReleasedTranslator* EventKeyButtonReleasedTranslator_Create(MyGUI::Widget* widget, EventKeyButtonReleasedTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventKeyButtonReleasedTranslator(widget, nativeEventCallback);
}