#pragma once

typedef void (*MessageLoggedDelegate)(String message, Ogre::LogMessageLevel lml);

class OgreLogListener : public Ogre::LogListener
{
private:
	MessageLoggedDelegate messageLoggedCallback;

public:
	OgreLogListener(MessageLoggedDelegate messageLoggedCallback);

	virtual ~OgreLogListener(void);

	virtual void messageLogged(const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName, bool &skipThisMessage);
};
