#pragma once

#include "OgreArchiveFactory.h"

namespace OgreWrapper
{

class EmbeddedResourceArchiveFactory : public Ogre::ArchiveFactory
{
public:
	EmbeddedResourceArchiveFactory(void);

	virtual ~EmbeddedResourceArchiveFactory(void);

	const Ogre::String& getType(void) const;

	Ogre::Archive* createInstance(const Ogre::String& name);
    
    void destroyInstance(Ogre::Archive* arch);
};

}