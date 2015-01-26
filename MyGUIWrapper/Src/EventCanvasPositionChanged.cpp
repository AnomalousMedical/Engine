#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventCanvasPositionChangedTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, int x, int y HANDLE_ARG);

private:
	MyGUI::ScrollView* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
	void fireEvent(MyGUI::Widget* sender, int x, int y)
	{
		nativeEvent(sender, x, y PASS_HANDLE_ARG);
	}
#endif

public:
	EventCanvasPositionChangedTranslator(MyGUI::ScrollView* widget, EventCanvasPositionChangedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventCanvasPositionChangedTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventCanvasPositionChanged = MyGUI::newDelegate(this, &EventCanvasPositionChangedTranslator::fireEvent);
#else
		widget->eventCanvasPositionChanged = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventCanvasPositionChanged = NULL;
	}
};

extern "C" _AnomalousExport EventCanvasPositionChangedTranslator* EventCanvasPositionChangedTranslator_Create(MyGUI::ScrollView* widget, EventCanvasPositionChangedTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventCanvasPositionChangedTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}