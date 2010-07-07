#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::Texture* TextureManager_createManual(String name, String group, Ogre::TextureType texType, uint width, uint height, uint depth, int num_mips, Ogre::PixelFormat format, Ogre::TextureUsage usage, bool hwGammaCorrection, uint fsaa, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::TexturePtr& texturePtr = Ogre::TextureManager::getSingleton().createManual(name, group, texType, width, height, depth, num_mips, format, usage, 0, hwGammaCorrection, fsaa);
	processWrapper(texturePtr.getPointer(), &texturePtr);
	return texturePtr.getPointer();
}

extern "C" _AnomalousExport Ogre::Texture* TextureManager_getByName1(String name, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::TexturePtr& texturePtr = Ogre::TextureManager::getSingleton().getByName(name);
	processWrapper(texturePtr.getPointer(), &texturePtr);
	return texturePtr.getPointer();
}

extern "C" _AnomalousExport Ogre::Texture* TextureManager_getByName2(String name, String group, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::TexturePtr& texturePtr = Ogre::TextureManager::getSingleton().getByName(name, group);
	processWrapper(texturePtr.getPointer(), &texturePtr);
	return texturePtr.getPointer();
}

//TexturePtr
extern "C" _AnomalousExport Ogre::TexturePtr* TexturePtr_createHeapPtr(Ogre::TexturePtr* stackSharedPtr)
{
	return new Ogre::TexturePtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void TexturePtr_Delete(Ogre::TexturePtr* heapSharedPtr)
{
	delete heapSharedPtr;
}