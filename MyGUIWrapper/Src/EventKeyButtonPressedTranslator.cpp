#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventKeyButtonPressedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::KeyCode key, MyGUI::Char _char);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventKeyButtonPressedTranslator(MyGUI::Widget* widget, EventKeyButtonPressedTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventKeyButtonPressedTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventKeyButtonPressed = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventKeyButtonPressed = NULL;
	}
};

extern "C" _AnomalousExport EventKeyButtonPressedTranslator* EventKeyButtonPressedTranslator_Create(MyGUI::Widget* widget, EventKeyButtonPressedTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventKeyButtonPressedTranslator(widget, nativeEventCallback);
}