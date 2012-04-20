#pragma once

#include <Rocket/Core/SystemInterface.h>

typedef float (*GetElapsedTimeDelegate)();
typedef void (*LogMessageDelegate)(Rocket::Core::Log::Type type, String message);

class ManagedSystemInterface : public Rocket::Core::SystemInterface
{
	public:
		ManagedSystemInterface(GetElapsedTimeDelegate etDelegate, LogMessageDelegate logDelegate);

		virtual ~ManagedSystemInterface();

		virtual float GetElapsedTime();

		virtual bool LogMessage(Rocket::Core::Log::Type type, const Rocket::Core::String& message);
		
	private:
		GetElapsedTimeDelegate etDelegate;
		LogMessageDelegate logDelegate;
};