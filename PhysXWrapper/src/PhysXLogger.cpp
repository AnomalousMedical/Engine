#include "StdAfx.h"
#include "PhysXLogger.h"

namespace Engine
{

namespace Physics
{

PhysXLogger::PhysXLogger(void)
{
}

PhysXLogger::~PhysXLogger(void)
{
}

void PhysXLogger::reportError( NxErrorCode code, const char* message, const char* file, int line ){
	if( code < NXE_DB_INFO )
	{
		System::String^ logMessage = "Error({0}): {1} on line {2} of file {3}";
		array<System::Object^>^ info = {(int)code, gcnew System::String(message), line, gcnew System::String(file)};
		logMessage = logMessage->Format(logMessage, info);
		Logging::Log::Default->sendMessage( logMessage, Logging::LogLevel::Error, "PhysX" );
	}
}

NxAssertResponse PhysXLogger::reportAssertViolation( const char* message, const char* file, int line ){
	System::String^ logMessage = "Assert Violation: {0} on line {1} of file {2}";
	array<System::Object^>^ info = {gcnew System::String(message), line, gcnew System::String(file)};
	logMessage = logMessage->Format(logMessage, info);
	Logging::Log::Default->sendMessage( logMessage, Logging::LogLevel::Error, "PhysX" );
	
	assert(0);
	return NX_AR_CONTINUE;
}

void PhysXLogger::print( const char* message ){
	Logging::Log::Default->sendMessage( gcnew System::String(message), Logging::LogLevel::Info, "PhysX" );
}

}

}