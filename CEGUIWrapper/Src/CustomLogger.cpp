#include "StdAfx.h"
#include "..\Include\CustomLogger.h"

CustomLogger::CustomLogger(LogEventDelegate logEventCallback)
:logEventCallback(logEventCallback)
{
}

CustomLogger::~CustomLogger(void)
{
}

void CustomLogger::logEvent(const CEGUI::String& message, CEGUI::LoggingLevel level)
{
	if(level <= d_level)
	{
		logEventCallback(message.c_str(), level);
	}
}

void CustomLogger::setLogFilename(const CEGUI::String& filename, bool append)
{

}

extern "C" _AnomalousExport CustomLogger* CustomLogger_create(LogEventDelegate logEventCallback)
{
	return new CustomLogger(logEventCallback);
}

extern "C" _AnomalousExport void CustomLogger_delete(CustomLogger* log)
{
	delete log;
}

extern "C" _AnomalousExport void CustomLogger_setLoggingLevel(CustomLogger* log, CEGUI::LoggingLevel level)
{
	//CEGUI::Logger::getSingleton().setLoggingLevel(level);
	log->setLoggingLevel(level);
}