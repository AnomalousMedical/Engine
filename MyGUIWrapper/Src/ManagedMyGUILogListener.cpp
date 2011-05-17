#include "StdAfx.h"
#include <string>

#pragma once

typedef void (*MessageLoggedDelegate)(String section, MyGUI::LogManager::LogLevel lml, String message);

class ManagedMyGUILogListener : public MyGUI::LogListener
{
private:
	MessageLoggedDelegate messageLoggedCallback;

public:
	ManagedMyGUILogListener(MessageLoggedDelegate messageLoggedCallback)
	:messageLoggedCallback(messageLoggedCallback)
	{
		MyGUI::LogManager::setThirdPartyLogListener(this);
		/*Ogre::Log* log = Ogre::LogManager::getSingleton().getDefaultLog();
		log->addListener(this);
		log->setDebugOutputEnabled(false);*/
	}

	virtual ~ManagedMyGUILogListener(void)
	{
		MyGUI::LogManager::setThirdPartyLogListener(0);
	}

	virtual void log(const std::string& _section, MyGUI::LogManager::LogLevel _level, const std::string& _message)
	{
		messageLoggedCallback(_section.c_str(), _level, _message.c_str());
	}
};

extern "C" _AnomalousExport ManagedMyGUILogListener* ManagedMyGUILogListener_Create(MessageLoggedDelegate messageLoggedCallback)
{
	return new ManagedMyGUILogListener(messageLoggedCallback);
}

extern "C" _AnomalousExport void ManagedMyGUILogListener_Delete(ManagedMyGUILogListener* logListener)
{
	delete logListener;
}