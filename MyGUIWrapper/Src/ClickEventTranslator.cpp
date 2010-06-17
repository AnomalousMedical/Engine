#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class ClickEventTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	ClickEventTranslator(MyGUI::Widget* widget, ClickEventTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~ClickEventTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseButtonClick = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonClick = NULL;
	}
};

extern "C" _AnomalousExport ClickEventTranslator* ClickEventTranslator_Create(MyGUI::Widget* widget, ClickEventTranslator::NativeEventDelegate nativeEventCallback)
{
	return new ClickEventTranslator(widget, nativeEventCallback);
}