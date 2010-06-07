#include "Stdafx.h"

//MaterialManager

extern "C" _AnomalousExport Ogre::Material* MaterialManager_getByName(String name, ProcessWrapperObjectDelegate processWrapperCallback)
{
	const Ogre::MaterialPtr& matPtr = Ogre::MaterialManager::getSingleton().getByName(name);
	processWrapperCallback(matPtr.getPointer(), &matPtr);
	return matPtr.getPointer();
}

extern "C" _AnomalousExport bool MaterialManager_resourceExists(String name)
{
	return Ogre::MaterialManager::getSingleton().resourceExists(name);
}

extern "C" _AnomalousExport String MaterialManager_getActiveScheme()
{
	return Ogre::MaterialManager::getSingleton().getActiveScheme().c_str();
}

extern "C" _AnomalousExport void MaterialManager_setActiveScheme(String name)
{
	Ogre::MaterialManager::getSingleton().setActiveScheme(name);
}

//MaterialPtr
extern "C" _AnomalousExport Ogre::MaterialPtr* MaterialPtr_createHeapPtr(Ogre::MaterialPtr* stackSharedPtr)
{
	return new Ogre::MaterialPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void MaterialPtr_Delete(Ogre::MaterialPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}