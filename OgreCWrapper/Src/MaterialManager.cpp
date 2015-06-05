#include "Stdafx.h"

typedef Ogre::Technique*(*HandleSchemeNotFoundCb)(unsigned short schemeIndex, const String schemeName, Ogre::Material* originalMaterial, unsigned short lodIndex, const Ogre::Renderable* rend);

class NativeMaterialListener : public Ogre::MaterialManager::Listener
{
private:
	HandleSchemeNotFoundCb schemeNotFoundCb;

public:
	NativeMaterialListener(HandleSchemeNotFoundCb schemeNotFoundCb)
		:schemeNotFoundCb(schemeNotFoundCb)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~NativeMaterialListener() { }

	virtual Ogre::Technique* handleSchemeNotFound(unsigned short schemeIndex, const Ogre::String& schemeName, Ogre::Material* originalMaterial, unsigned short lodIndex, const Ogre::Renderable* rend)
	{
		return schemeNotFoundCb(schemeIndex, schemeName.c_str(), originalMaterial, lodIndex, rend);
	}
};

extern "C" _AnomalousExport NativeMaterialListener* NativeMaterialListener_create(HandleSchemeNotFoundCb schemeNotFoundCb)
{
	NativeMaterialListener* listener = new NativeMaterialListener(schemeNotFoundCb);
	Ogre::MaterialManager::getSingleton().addListener(listener);
	return listener;
}

extern "C" _AnomalousExport void NativeMaterialListener_delete(NativeMaterialListener* listener)
{
	Ogre::MaterialManager::getSingleton().removeListener(listener);
	delete listener;
}

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