#include "StdAfx.h"
#include "..\Include\WindowEventTranslator.h"

WindowEventTranslator::WindowEventTranslator(String eventName, BasicEventDelegate basicEvent)
:basicEvent(basicEvent),
eventName(eventName)
{
}

WindowEventTranslator::~WindowEventTranslator(void)
{
	unbindEvent();
}

void WindowEventTranslator::bindEvent(CEGUI::Window* window)
{
	connection = window->subscribeEvent(eventName, CEGUI::Event::Subscriber(&WindowEventTranslator::eventCallback, this));
}

void WindowEventTranslator::unbindEvent()
{
	if(connection.isValid())
	{
		connection->disconnect();
		connection = CEGUI::Event::Connection(0);
	}
}

bool WindowEventTranslator::eventCallback(const CEGUI::EventArgs& event)
{
	return basicEvent();
}

extern "C" _AnomalousExport WindowEventTranslator* WindowEventTranslator_create(String eventName, BasicEventDelegate basicEvent)
{
	return new WindowEventTranslator(eventName, basicEvent);
}

extern "C" _AnomalousExport void WindowEventTranslator_delete(WindowEventTranslator* nativeEventTranslator)
{
	delete nativeEventTranslator;
}

extern "C" _AnomalousExport void WindowEventTranslator_bindEvent(WindowEventTranslator* nativeEventTranslator, CEGUI::Window* window)
{
	nativeEventTranslator->bindEvent(window);
}

extern "C" _AnomalousExport void WindowEventTranslator_unbindEvent(WindowEventTranslator* nativeEventTranslator)
{
	nativeEventTranslator->unbindEvent();
}