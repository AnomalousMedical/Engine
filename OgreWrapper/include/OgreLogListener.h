/// <file>OgreLogListener.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "OgreLog.h"
#include "AutoPtr.h"

namespace OgreWrapper
{

class OgreLogListener;

/// <summary>
/// This class links the output of the ogre logger to the internal log. Simply
/// create an instance of this class and the logs will be hooked up until it is
/// disposed.
/// </summary>
public ref class OgreLogConnection
{
private:
	AutoPtr<OgreLogListener> ogreLogListener;

public:
	/// <summary>
	/// Constructor. Creating an instance of this class automaticaly links it to
    /// the ogre log.
	/// </summary>
	OgreLogConnection();

	/// <summary>
	/// Destructor. Call to disconnect from the ogre log. This object cannot be
    /// reused after that.
	/// </summary>
	virtual ~OgreLogConnection();
};

/// <summary>
/// This class extends the Ogre LogListener and forwards the messages to our internal
/// log.
/// </summary>
class OgreLogListener : public Ogre::LogListener
{
private:

public:
	/// <summary>
	/// Constructor
	/// </summary>
	OgreLogListener(void);

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~OgreLogListener(void);

	/// <summary>
	/// Dispatch a message to the listener
	/// </summary>
	virtual void messageLogged( const Ogre::String& message, Ogre::LogMessageLevel lml, bool maskDebug, const Ogre::String &logName );
};

}