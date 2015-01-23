#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventWindowChangedCoordTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Window* sender HANDLE_ARG);

private:
	MyGUI::Window* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

public:
	EventWindowChangedCoordTranslator(MyGUI::Window* widget, EventWindowChangedCoordTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

#if FULL_AOT_COMPILE
	void fireEvent(MyGUI::Window* sender)
	{
		nativeEvent(sender PASS_HANDLE_ARG);
	}
#endif

	virtual ~EventWindowChangedCoordTranslator()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventWindowChangeCoord = MyGUI::newDelegate(this, &EventWindowChangedCoordTranslator::fireEvent);
#else
		widget->eventWindowChangeCoord = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventWindowChangeCoord = NULL;
	}
};

extern "C" _AnomalousExport EventWindowChangedCoordTranslator* EventWindowChangedCoordTranslator_Create(MyGUI::Window* widget, EventWindowChangedCoordTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventWindowChangedCoordTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}