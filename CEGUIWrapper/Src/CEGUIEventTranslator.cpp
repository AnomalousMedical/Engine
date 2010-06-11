#include "StdAfx.h"
#include "..\Include\CEGUIEventTranslator.h"

CEGUIEventTranslator::CEGUIEventTranslator(String eventName)
:eventName(eventName)
{
}

CEGUIEventTranslator::~CEGUIEventTranslator(void)
{
	unbindEvent();
}

void CEGUIEventTranslator::bindEvent(CEGUI::Window* window)
{
	connection = window->subscribeEvent(eventName, CEGUI::Event::Subscriber(&CEGUIEventTranslator::handleEvent, this));
}

void CEGUIEventTranslator::unbindEvent()
{
	if(connection.isValid())
	{
		connection->disconnect();
		connection = CEGUI::Event::Connection(0);
	}
}

extern "C" _AnomalousExport void CEGUIEventTranslator_delete(CEGUIEventTranslator* nativeEventTranslator)
{
	delete nativeEventTranslator;
}

extern "C" _AnomalousExport void CEGUIEventTranslator_bindEvent(CEGUIEventTranslator* nativeEventTranslator, CEGUI::Window* window)
{
	nativeEventTranslator->bindEvent(window);
}

extern "C" _AnomalousExport void CEGUIEventTranslator_unbindEvent(CEGUIEventTranslator* nativeEventTranslator)
{
	nativeEventTranslator->unbindEvent();
}