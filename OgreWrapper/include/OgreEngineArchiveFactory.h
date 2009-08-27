#pragma once

#include "OgreArchiveFactory.h"

namespace OgreWrapper
{

class OgreEngineArchiveFactory : public Ogre::ArchiveFactory
{
public:
	OgreEngineArchiveFactory(void);

	virtual ~OgreEngineArchiveFactory(void);

	const Ogre::String& getType(void) const;

	Ogre::Archive* createInstance(const Ogre::String& name);
    
    void destroyInstance(Ogre::Archive* arch);
};

}