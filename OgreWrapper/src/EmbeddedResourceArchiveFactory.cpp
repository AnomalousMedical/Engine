#include "StdAfx.h"
#include "..\include\EmbeddedResourceArchiveFactory.h"
#include "EmbeddedResourceArchive.h"

namespace OgreWrapper
{

EmbeddedResourceArchiveFactory::EmbeddedResourceArchiveFactory(void)
{
}

EmbeddedResourceArchiveFactory::~EmbeddedResourceArchiveFactory(void)
{
}

const Ogre::String& EmbeddedResourceArchiveFactory::getType(void) const
{
	static Ogre::String name = "EmbeddedResource";
    return name;
}

Ogre::Archive* EmbeddedResourceArchiveFactory::createInstance(const Ogre::String& name) 
{
    return OGRE_NEW EmbeddedResourceArchive(name, "EmbeddedResource");
}

void EmbeddedResourceArchiveFactory::destroyInstance(Ogre::Archive* arch) 
{ 
	OGRE_DELETE arch; 
}

}