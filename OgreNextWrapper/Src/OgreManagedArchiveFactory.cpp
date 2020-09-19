#include "StdAfx.h"
#include "../Include/OgreManagedArchiveFactory.h"

OgreManagedArchiveFactory::OgreManagedArchiveFactory(String archType, CreateInstanceDelegate createInstanceCallback, DestroyInstanceDelegate destroyInstanceCallback HANDLE_ARG)
:createInstanceCallback(createInstanceCallback),
destroyInstanceCallback(destroyInstanceCallback),
archType(archType)
ASSIGN_HANDLE_INITIALIZER
{
}

OgreManagedArchiveFactory::~OgreManagedArchiveFactory(void)
{
}

extern "C" _AnomalousExport OgreManagedArchiveFactory* OgreManagedArchiveFactory_Create(String archType, CreateInstanceDelegate createInstanceCallback, DestroyInstanceDelegate destroyInstanceCallback HANDLE_ARG)
{
	return new OgreManagedArchiveFactory(archType, createInstanceCallback, destroyInstanceCallback PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void OgreManagedArchiveFactory_Delete(OgreManagedArchiveFactory* archive)
{
	delete archive;
}