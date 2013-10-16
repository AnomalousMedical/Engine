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

		virtual void JoinPath(Rocket::Core::String& translated_path, const Rocket::Core::String& document_path, const Rocket::Core::String& path);

		void AddRootPath(const Rocket::Core::String& rootPath);

		void RemoveRootPath(const Rocket::Core::String& rootPath);
		
	private:
		GetElapsedTimeDelegate etDelegate;
		LogMessageDelegate logDelegate;
		std::list<Rocket::Core::String> rootedPaths;
};