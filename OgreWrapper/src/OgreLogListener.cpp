/// <file>OgreLogListener.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\OgreLogListener.h"

namespace Engine
{

namespace Rendering
{

using namespace Logging;

OgreLogConnection::OgreLogConnection()
:ogreLogListener(new OgreLogListener())
{
	Ogre::Log* log = Ogre::LogManager::getSingleton().getDefaultLog();
	log->addListener(ogreLogListener.Get());
	log->setDebugOutputEnabled(false);
}

OgreLogConnection::~OgreLogConnection()
{
	Ogre::Log* log = Ogre::LogManager::getSingleton().getDefaultLog();
	log->removeListener(ogreLogListener.Get());
}

OgreLogListener::OgreLogListener(void)
{
}

OgreLogListener::~OgreLogListener(void)
{
}

void OgreLogListener::messageLogged( const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName )
{
	LogLevel level;
	switch( lml )
	{
	case Ogre::LML_CRITICAL:
		level = LogLevel::Error;
		break;
	case Ogre::LML_NORMAL:
		level = LogLevel::ImportantInfo;
		break;
	case Ogre::LML_TRIVIAL:
		level = LogLevel::Info;
		break;
	}
	Log::Default->sendMessage(gcnew System::String(message.c_str()), level, "Ogre");
}

}

}