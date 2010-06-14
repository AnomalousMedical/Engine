#include "StdAfx.h"
#include "..\Include\MouseEventTranslator.h"

MouseEventTranslator::MouseEventTranslator(String eventName, MouseEventDelegate eventDelegate)
:CEGUIEventTranslator(eventName),
eventDelegate(eventDelegate)
{
}

MouseEventTranslator::~MouseEventTranslator(void)
{
}

bool MouseEventTranslator::handleEvent(const CEGUI::EventArgs& event)
{
	const CEGUI::MouseEventArgs& eventSub = static_cast<const CEGUI::MouseEventArgs&>(event);
	return eventDelegate(eventSub.window, eventSub.position, eventSub.moveDelta, eventSub.button, eventSub.sysKeys, eventSub.wheelChange, eventSub.clickCount);
}

extern "C" _AnomalousExport MouseEventTranslator* MouseEventTranslator_create(String eventName, MouseEventDelegate eventDelegate)
{
	return new MouseEventTranslator(eventName, eventDelegate);
}
