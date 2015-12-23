#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::Texture* TextureManager_createManual(String name, String group, Ogre::TextureType texType, uint width, uint height, uint depth, int num_mips, Ogre::PixelFormat format, Ogre::TextureUsage usage, Ogre::ManualResourceLoader* loader, bool hwGammaCorrection, uint fsaa, String fsaaHint, ProcessWrapperObjectDelegate processWrapper)
{
	try
	{
		const Ogre::TexturePtr& texturePtr = Ogre::TextureManager::getSingleton().createManual(name, group, texType, width, height, depth, num_mips, format, usage, loader, hwGammaCorrection, fsaa, fsaaHint);
		processWrapper(texturePtr.getPointer(), &texturePtr);
		return texturePtr.getPointer();
	}
	catch(Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
	return NULL;
}

extern "C" _AnomalousExport Ogre::Texture* TextureManager_loadImage(String name, String group, Ogre::Image* img, Ogre::TextureType texType, int numMipmaps, float gamma, bool isAlpha, Ogre::PixelFormat desiredFormat, bool hwGamma, ProcessWrapperObjectDelegate processWrapper)
{
	try
	{
		const Ogre::TexturePtr& texturePtr = Ogre::TextureManager::getSingleton().loadImage(name, group, *img, texType, numMipmaps, gamma, isAlpha, desiredFormat, hwGamma);
		processWrapper(texturePtr.getPointer(), &texturePtr);
		return texturePtr.getPointer();
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
	return NULL;
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

extern "C" _AnomalousExport void TextureManager_removeName(String name)
{
	Ogre::TextureManager::getSingleton().remove(name);
}

extern "C" _AnomalousExport void TextureManager_removeResource(Ogre::TexturePtr* heapSharedPtr)
{
	Ogre::ResourcePtr texturePtr = *heapSharedPtr;
	Ogre::TextureManager::getSingleton().remove(texturePtr);
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

extern "C" _AnomalousExport size_t TextureManager_getMemoryUsage()
{
	return Ogre::TextureManager::getSingleton().getMemoryUsage();
}