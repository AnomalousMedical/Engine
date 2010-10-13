#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventToolTipTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, MyGUI::ToolTipInfo::ToolTipType type, uint index, int x, int y);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EventToolTipTranslator(MyGUI::Widget* widget, EventToolTipTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventToolTipTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventToolTip = MyGUI::newDelegate(this, &EventToolTipTranslator::callback);
	}

	virtual void unbindEvent()
	{
		widget->eventToolTip = NULL;
	}

	void callback(MyGUI::Widget* sender, const MyGUI::ToolTipInfo& info)
	{
		nativeEvent(sender, info.type, info.index, info.point.left, info.point.top);
	}
};

extern "C" _AnomalousExport EventToolTipTranslator* EventToolTipTranslator_Create(MyGUI::Widget* widget, EventToolTipTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventToolTipTranslator(widget, nativeEventCallback);
}