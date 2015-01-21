#pragma once

typedef void(*MessageLoggedDelegate)(String message, Ogre::LogMessageLevel lml HANDLE_ARG);

class OgreLogListener : public Ogre::LogListener
{
private:
	MessageLoggedDelegate messageLoggedCallback;
	HANDLE_INSTANCE

public:
	OgreLogListener(MessageLoggedDelegate messageLoggedCallback HANDLE_ARG);

	virtual ~OgreLogListener(void);

	virtual void messageLogged(const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName, bool &skipThisMessage);
};
