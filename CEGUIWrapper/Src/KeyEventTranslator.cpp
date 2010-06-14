#include "StdAfx.h"
#include "..\Include\KeyEventTranslator.h"

KeyEventTranslator::KeyEventTranslator(String eventName, KeyEventDelegate eventDelegate)
:CEGUIEventTranslator(eventName),
eventDelegate(eventDelegate)
{
}

KeyEventTranslator::~KeyEventTranslator(void)
{
}

bool KeyEventTranslator::handleEvent(const CEGUI::EventArgs& event)
{
	const CEGUI::KeyEventArgs& eventSub = static_cast<const CEGUI::KeyEventArgs&>(event);
	return eventDelegate(eventSub.window, eventSub.codepoint, eventSub.scancode, eventSub.sysKeys);
}

extern "C" _AnomalousExport KeyEventTranslator* KeyEventTranslator_create(String eventName, KeyEventDelegate eventDelegate)
{
	return new KeyEventTranslator(eventName, eventDelegate);
}
