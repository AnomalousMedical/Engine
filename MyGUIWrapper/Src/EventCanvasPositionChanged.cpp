#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventCanvasPositionChangedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int x, int y);

private:
	MyGUI::ScrollView* widget;
	NativeEventDelegate nativeEvent;

public:
	EventCanvasPositionChangedTranslator(MyGUI::ScrollView* widget, EventCanvasPositionChangedTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventCanvasPositionChangedTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventCanvasPositionChanged = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventCanvasPositionChanged = NULL;
	}
};

extern "C" _AnomalousExport EventCanvasPositionChangedTranslator* EventCanvasPositionChangedTranslator_Create(MyGUI::ScrollView* widget, EventCanvasPositionChangedTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventCanvasPositionChangedTranslator(widget, nativeEventCallback);
}