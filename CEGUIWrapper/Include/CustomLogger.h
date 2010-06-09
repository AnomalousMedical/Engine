#pragma once

typedef void (*LogEventDelegate)(String message, CEGUI::LoggingLevel level);

class CustomLogger : public CEGUI::Logger
{
private:
	LogEventDelegate logEventCallback;

public:
	CustomLogger(LogEventDelegate logEventCallback);

	virtual ~CustomLogger(void);

	virtual void logEvent(const CEGUI::String& message, CEGUI::LoggingLevel level = CEGUI::Standard);

    virtual void setLogFilename(const CEGUI::String& filename, bool append = false);
};
