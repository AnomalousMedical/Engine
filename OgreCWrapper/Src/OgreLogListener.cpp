#include "StdAfx.h"
#include "../Include/OgreLogListener.h"

OgreLogListener::OgreLogListener(MessageLoggedDelegate messageLoggedCallback HANDLE_ARG)
:messageLoggedCallback(messageLoggedCallback)
ASSIGN_HANDLE_INITIALIZER
{
	Ogre::LogManager::getSingleton().getDefaultLog()->setDebugOutputEnabled(false);
}

OgreLogListener::~OgreLogListener(void)
{
	
}

void OgreLogListener::messageLogged(const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName, bool &skipThisMessage)
{
	messageLoggedCallback(message.c_str(), lml PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport OgreLogListener* OgreLogListener_Create(MessageLoggedDelegate messageLoggedCallback HANDLE_ARG)
{
	return new OgreLogListener(messageLoggedCallback PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void OgreLogListener_Delete(OgreLogListener* logListener)
{
	delete logListener;
}

extern "C" _AnomalousExport void OgreLogListener_subscribe(OgreLogListener* logListener)
{
	Ogre::LogManager::getSingleton().getDefaultLog()->addListener(logListener);
}