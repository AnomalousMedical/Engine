#include "StdAfx.h"
#include <string>

#pragma once

typedef void (*MessageLoggedDelegate)(String section, MyGUI::LogLevel lml, String message);

class ManagedMyGUILogListener : public MyGUI::ILogListener
{
private:
	MessageLoggedDelegate messageLoggedCallback;

public:
	ManagedMyGUILogListener(MessageLoggedDelegate messageLoggedCallback)
	:messageLoggedCallback(messageLoggedCallback)
	{
		MyGUI::LogManager::setThirdPartyLogListener(this);
		MyGUI::LogManager::setSTDOutputEnabled(false);
	}

	virtual ~ManagedMyGUILogListener(void)
	{
		MyGUI::LogManager::setThirdPartyLogListener(0);
	}

	virtual void log(const std::string& _section, MyGUI::LogLevel _level, const struct tm* _time, const std::string& _message, const char* _file, int _line)
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