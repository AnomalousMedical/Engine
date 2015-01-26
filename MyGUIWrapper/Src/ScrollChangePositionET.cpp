#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class ScrollChangePositionET : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::ScrollBar* sender, size_t position HANDLE_ARG);

private:
	MyGUI::ScrollBar* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

#ifdef FULL_AOT_COMPILE
		void fireEvent(MyGUI::ScrollBar* sender, size_t position)
	{
		nativeEvent(sender, position PASS_HANDLE_ARG);
	}
#endif

public:
	ScrollChangePositionET(MyGUI::ScrollBar* widget, ScrollChangePositionET::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ScrollChangePositionET()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->eventScrollChangePosition = MyGUI::newDelegate(this, &ScrollChangePositionET::fireEvent);
#else
		widget->eventScrollChangePosition = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->eventScrollChangePosition = NULL;
	}
};

extern "C" _AnomalousExport ScrollChangePositionET* ScrollChangePositionET_Create(MyGUI::ScrollBar* widget, ScrollChangePositionET::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new ScrollChangePositionET(widget, nativeEventCallback PASS_HANDLE_ARG);
}