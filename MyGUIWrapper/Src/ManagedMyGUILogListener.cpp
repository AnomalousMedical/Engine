#include "StdAfx.h"
#include <string>

#pragma once

typedef void(*MessageLoggedDelegate)(String section, MyGUI::LogLevel lml, String message HANDLE_ARG);

class ManagedMyGUILogListener : public MyGUI::ILogListener
{
private:
	MessageLoggedDelegate messageLoggedCallback;
	MyGUI::LogSource logSource;
	HANDLE_INSTANCE

public:
	ManagedMyGUILogListener(MessageLoggedDelegate messageLoggedCallback HANDLE_ARG)
	:messageLoggedCallback(messageLoggedCallback)
	ASSIGN_HANDLE_INITIALIZER
	{
		MyGUI::LogManager* logManager = MyGUI::LogManager::getInstancePtr();
		logManager->setSTDOutputEnabled(false);
		logSource.addLogListener(this);
		logManager->addLogSource(&logSource);
		/*MyGUI::LogManager::setThirdPartyLogListener(this);
		MyGUI::LogManager::getInstancePtr()->setSTDOutputEnabled(false);*/
	}

	virtual ~ManagedMyGUILogListener(void)
	{
		//This could be a problem, need way to remove log
		//MyGUI::LogManager::setThirdPartyLogListener(0);
	}

	virtual void log(const std::string& _section, MyGUI::LogLevel _level, const struct tm* _time, const std::string& _message, const char* _file, int _line)
	{
		messageLoggedCallback(_section.c_str(), _level, _message.c_str() PASS_HANDLE_ARG);
	}
};

extern "C" _AnomalousExport ManagedMyGUILogListener* ManagedMyGUILogListener_Create(MessageLoggedDelegate messageLoggedCallback HANDLE_ARG)
{
	return new ManagedMyGUILogListener(messageLoggedCallback PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedMyGUILogListener_Delete(ManagedMyGUILogListener* logListener)
{
	delete logListener;
}