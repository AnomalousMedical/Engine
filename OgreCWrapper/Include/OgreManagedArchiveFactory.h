#pragma once

#include "OgreArchiveFactory.h"

typedef Ogre::Archive* (*CreateInstanceDelegate)(String name);
typedef void (*DestroyInstanceDelegate)(Ogre::Archive* arch);

class OgreManagedArchiveFactory : public Ogre::ArchiveFactory
{
private:
	CreateInstanceDelegate createInstanceCallback;
	DestroyInstanceDelegate destroyInstanceCallback;
	std::string archType;

public:
	OgreManagedArchiveFactory(String archType, CreateInstanceDelegate createInstanceCallback, DestroyInstanceDelegate destroyInstanceCallback);

	virtual ~OgreManagedArchiveFactory(void);

	const Ogre::String& getType(void) const
	{
		return archType;
	}

	Ogre::Archive* createInstance(const Ogre::String& name, bool readOnly)
	{
		return createInstanceCallback(name.c_str());
	}

	void destroyInstance(Ogre::Archive* arch)
	{
		destroyInstanceCallback(arch);
	}
};
