#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventWindowChangedCoordTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Window* sender);

private:
	MyGUI::Window* widget;
	NativeEventDelegate nativeEvent;

public:
	EventWindowChangedCoordTranslator(MyGUI::Window* widget, EventWindowChangedCoordTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventWindowChangedCoordTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventWindowChangeCoord = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->eventWindowChangeCoord = NULL;
	}
};

extern "C" _AnomalousExport EventWindowChangedCoordTranslator* EventWindowChangedCoordTranslator_Create(MyGUI::Window* widget, EventWindowChangedCoordTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventWindowChangedCoordTranslator(widget, nativeEventCallback);
}