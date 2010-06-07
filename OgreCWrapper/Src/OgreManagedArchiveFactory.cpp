#include "StdAfx.h"
#include "../Include/OgreManagedArchiveFactory.h"

OgreManagedArchiveFactory::OgreManagedArchiveFactory(String archType, CreateInstanceDelegate createInstanceCallback, DestroyInstanceDelegate destroyInstanceCallback)
:createInstanceCallback(createInstanceCallback),
destroyInstanceCallback(destroyInstanceCallback),
archType(archType)
{
}

OgreManagedArchiveFactory::~OgreManagedArchiveFactory(void)
{
}

extern "C" _AnomalousExport OgreManagedArchiveFactory* OgreManagedArchiveFactory_Create(String archType, CreateInstanceDelegate createInstanceCallback, DestroyInstanceDelegate destroyInstanceCallback)
{
	return new OgreManagedArchiveFactory(archType, createInstanceCallback, destroyInstanceCallback);
}

extern "C" _AnomalousExport void OgreManagedArchiveFactory_Delete(OgreManagedArchiveFactory* archive)
{
	delete archive;
}