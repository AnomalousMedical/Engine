#include "StdAfx.h"
#include "..\Include\OgreLogListener.h"

OgreLogListener::OgreLogListener(MessageLoggedDelegate messageLoggedCallback)
:messageLoggedCallback(messageLoggedCallback)
{
	Ogre::Log* log = Ogre::LogManager::getSingleton().getDefaultLog();
	log->addListener(this);
	log->setDebugOutputEnabled(false);
}

OgreLogListener::~OgreLogListener(void)
{
	Ogre::Log* log = Ogre::LogManager::getSingleton().getDefaultLog();
	log->removeListener(this);
}

void OgreLogListener::messageLogged( const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName )
{
	messageLoggedCallback(message.c_str(), lml);
}

extern "C" __declspec(dllexport) OgreLogListener* OgreLogListener_Create(MessageLoggedDelegate messageLoggedCallback)
{
	return new OgreLogListener(messageLoggedCallback);
}

extern "C" __declspec(dllexport) void OgreLogListener_Delete(OgreLogListener* logListener)
{
	delete logListener;
}