#pragma once

#include <Rocket/Core/SystemInterface.h>
#include <Ogre.h>

/**
	A sample system interface for Rocket into Ogre3D.

	@author Peter Curry
 */

class AnomalousSystemInterface : public Rocket::Core::SystemInterface
{
	public:
		AnomalousSystemInterface();
		virtual ~AnomalousSystemInterface();

		/// Gets the number of seconds elapsed since the start of the application.
		virtual float GetElapsedTime();

		/// Logs the specified message.
		virtual bool LogMessage(Rocket::Core::Log::Type type, const Rocket::Core::String& message);
		
	private:
		Ogre::Timer timer;
};