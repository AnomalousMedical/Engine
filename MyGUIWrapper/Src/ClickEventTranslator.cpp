#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class ClickEventTranslator : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* sender HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

	void fireEvent(MyGUI::Widget* sender)
	{
		nativeEvent(sender PASS_HANDLE_ARG);
	}

public:
	ClickEventTranslator(MyGUI::Widget* widget, ClickEventTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ClickEventTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMouseButtonClick = MyGUI::newDelegate(this, &ClickEventTranslator::fireEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventMouseButtonClick = NULL;
	}
};

extern "C" _AnomalousExport ClickEventTranslator* ClickEventTranslator_Create(MyGUI::Widget* widget, ClickEventTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new ClickEventTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}