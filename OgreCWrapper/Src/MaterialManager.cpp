#include "Stdafx.h"

//MaterialManager

extern "C" __declspec(dllexport) Ogre::Material* MaterialManager_getByName(String name, ProcessWrapperObjectDelegate processWrapperCallback)
{
	const Ogre::MaterialPtr& matPtr = Ogre::MaterialManager::getSingleton().getByName(name);
	processWrapperCallback(matPtr.getPointer(), &matPtr);
	return matPtr.getPointer();
}

extern "C" __declspec(dllexport) bool MaterialManager_resourceExists(String name)
{
	return Ogre::MaterialManager::getSingleton().resourceExists(name);
}

extern "C" __declspec(dllexport) String MaterialManager_getActiveScheme()
{
	return Ogre::MaterialManager::getSingleton().getActiveScheme().c_str();
}

extern "C" __declspec(dllexport) void MaterialManager_setActiveScheme(String name)
{
	Ogre::MaterialManager::getSingleton().setActiveScheme(name);
}

//MaterialPtr
extern "C" __declspec(dllexport) Ogre::MaterialPtr* MaterialPtr_createHeapPtr(Ogre::MaterialPtr* stackSharedPtr)
{
	return new Ogre::MaterialPtr(*stackSharedPtr);
}

extern "C" __declspec(dllexport) void MaterialPtr_Delete(Ogre::MaterialPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}