#include "StdAfx.h"
#include "NativeLog.h"

namespace SoundWrapper
{

typedef void (*LogMessageDelegate)(const char* message, LogLevel logLevel, const char* subsystem HANDLE_ARG);

class ManagedLogListener : NativeLogListener
{
private:
	LogMessageDelegate logDelegate;
	HANDLE_INSTANCE

public:
	ManagedLogListener(LogMessageDelegate logDelegate HANDLE_ARG)
		:logDelegate(logDelegate)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedLogListener(void)
	{

	}

	virtual void logMessage(const std::string& message, LogLevel logLevel, const std::string& subsystem)
	{
		logDelegate(message.c_str(), logLevel, subsystem.c_str() PASS_HANDLE_ARG);
	}
};

}

//CWrapper
using namespace SoundWrapper;

extern "C" _AnomalousExport ManagedLogListener* ManagedLogListener_create(LogMessageDelegate logDelegate HANDLE_ARG)
{
	return new ManagedLogListener(logDelegate PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedLogListener_destroy(ManagedLogListener* managedLogListener)
{
	delete managedLogListener;
}