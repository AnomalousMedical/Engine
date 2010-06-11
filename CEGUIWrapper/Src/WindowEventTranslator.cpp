#include "StdAfx.h"
#include "..\Include\WindowEventTranslator.h"

WindowEventTranslator::WindowEventTranslator(String eventName, WindowEventDelegate eventDelegate)
:CEGUIEventTranslator(eventName),
eventDelegate(eventDelegate)
{
}

WindowEventTranslator::~WindowEventTranslator(void)
{
}

bool WindowEventTranslator::handleEvent(const CEGUI::EventArgs& event)
{
	const CEGUI::WindowEventArgs& eventSub = static_cast<const CEGUI::WindowEventArgs&>(event);
	return eventDelegate(eventSub.window);
}

extern "C" _AnomalousExport WindowEventTranslator* WindowEventTranslator_create(String eventName, WindowEventDelegate eventDelegate)
{
	return new WindowEventTranslator(eventName, eventDelegate);
}