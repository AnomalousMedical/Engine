#include "Stdafx.h"

class ManagedManualResourceLoader : public Ogre::ManualResourceLoader
{
private:
	NativeAction prepareResourceCb;
	NativeAction loadResourceCb;
	HANDLE_INSTANCE

public:
	ManagedManualResourceLoader(NativeAction prepareResource, NativeAction loadResource)
		:prepareResourceCb(prepareResource),
		loadResourceCb(loadResource)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedManualResourceLoader()
	{

	}

	virtual void prepareResource(Ogre::Resource* resource)
	{
		prepareResourceCb();
	}

	virtual void loadResource(Ogre::Resource* resource)
	{
		loadResourceCb();
	}
};

extern "C" _AnomalousExport ManagedManualResourceLoader* ManagedManualResourceLoader_Create(NativeAction prepareResource, NativeAction loadResource HANDLE_ARG)
{
	return new ManagedManualResourceLoader(prepareResource, loadResource PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedManualResourceLoader_Delete(ManagedManualResourceLoader* nativeRenderQueueListener)
{
	delete nativeRenderQueueListener;
}