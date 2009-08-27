#include "StdAfx.h"
#include "..\include\OgreEngineArchiveFactory.h"
#include "OgreEngineArchive.h"

namespace OgreWrapper
{

OgreEngineArchiveFactory::OgreEngineArchiveFactory(void)
{
}

OgreEngineArchiveFactory::~OgreEngineArchiveFactory(void)
{
}

const Ogre::String& OgreEngineArchiveFactory::getType(void) const
{
	static Ogre::String name = "EngineArchive";
    return name;
}

Ogre::Archive* OgreEngineArchiveFactory::createInstance(const Ogre::String& name) 
{
    return OGRE_NEW OgreEngineArchive(name, "EngineArchive");
}

void OgreEngineArchiveFactory::destroyInstance(Ogre::Archive* arch) 
{ 
	OGRE_DELETE arch; 
}

}