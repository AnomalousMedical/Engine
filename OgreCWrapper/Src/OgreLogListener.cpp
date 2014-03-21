#include "StdAfx.h"
#include "../Include/OgreLogListener.h"

OgreLogListener::OgreLogListener(MessageLoggedDelegate messageLoggedCallback)
:messageLoggedCallback(messageLoggedCallback)
{
	Ogre::LogManager::getSingleton().getDefaultLog()->setDebugOutputEnabled(false);
}

OgreLogListener::~OgreLogListener(void)
{
	
}

void OgreLogListener::messageLogged(const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName, bool &skipThisMessage)
{
	messageLoggedCallback(message.c_str(), lml);
}

extern "C" _AnomalousExport OgreLogListener* OgreLogListener_Create(MessageLoggedDelegate messageLoggedCallback)
{
	return new OgreLogListener(messageLoggedCallback);
}

extern "C" _AnomalousExport void OgreLogListener_Delete(OgreLogListener* logListener)
{
	delete logListener;
}

extern "C" _AnomalousExport void OgreLogListener_subscribe(OgreLogListener* logListener)
{
	Ogre::LogManager::getSingleton().getDefaultLog()->addListener(logListener);
}