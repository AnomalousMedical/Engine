#pragma once

#include "OgreArchiveFactory.h"

NativeFunc_String_StrongIntPtr(CreateInstanceDelegate, Ogre::Archive*)
NativeAction_StrongIntPtr(DestroyInstanceDelegate, Ogre::Archive*)

class OgreManagedArchiveFactory : public Ogre::ArchiveFactory
{
private:
	CreateInstanceDelegate createInstanceCallback;
	DestroyInstanceDelegate destroyInstanceCallback;
	std::string archType;
	HANDLE_INSTANCE

public:
	OgreManagedArchiveFactory(String archType, CreateInstanceDelegate createInstanceCallback, DestroyInstanceDelegate destroyInstanceCallback HANDLE_ARG);

	virtual ~OgreManagedArchiveFactory(void);

	const Ogre::String& getType(void) const
	{
		return archType;
	}

	Ogre::Archive* createInstance(const Ogre::String& name, bool readOnly)
	{
		return createInstanceCallback(name.c_str() PASS_HANDLE_ARG);
	}

	void destroyInstance(Ogre::Archive* arch)
	{
		destroyInstanceCallback(arch PASS_HANDLE_ARG);
	}
};
