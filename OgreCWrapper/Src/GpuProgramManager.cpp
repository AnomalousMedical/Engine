#include "Stdafx.h"

extern "C" _AnomalousExport void GpuProgramManager_setSaveMicrocodesToCache(const bool val)
{
	Ogre::GpuProgramManager::getSingleton().setSaveMicrocodesToCache(val);
}

extern "C" _AnomalousExport bool GpuProgramManager_getSaveMicrocodesToCache()
{
	return Ogre::GpuProgramManager::getSingleton().getSaveMicrocodesToCache();
}

extern "C" _AnomalousExport void GpuProgramManager_saveMicrocodeCache(Ogre::DataStream* dataStream)
{
	Ogre::GpuProgramManager::getSingleton().saveMicrocodeCache(Ogre::DataStreamPtr(dataStream));
}

extern "C" _AnomalousExport void GpuProgramManager_loadMicrocodeCache(Ogre::DataStream* dataStream)
{
	Ogre::GpuProgramManager::getSingleton().loadMicrocodeCache(Ogre::DataStreamPtr(dataStream));
}

extern "C" _AnomalousExport Ogre::GpuProgramParametersSharedPtr* GpuProgramParameters_createHeapPtr(Ogre::GpuProgramParametersSharedPtr* stackSharedPtr)
{
	return new Ogre::GpuProgramParametersSharedPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void GpuProgramParameters_Delete(Ogre::GpuProgramParametersSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

extern "C" _AnomalousExport bool GpuProgramManager_isCacheDirty()
{
	return Ogre::GpuProgramManager::getSingleton().isCacheDirty();
}

extern "C" _AnomalousExport Ogre::GpuSharedParameters* GpuProgramManager_createSharedParameters(String name, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::GpuSharedParametersPtr& ptr = Ogre::GpuProgramManager::getSingleton().createSharedParameters(name);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::GpuSharedParameters* GpuProgramManager_getSharedParameters(String name, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::GpuSharedParametersPtr& ptr = Ogre::GpuProgramManager::getSingleton().getSharedParameters(name);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::GpuSharedParametersPtr* GpuSharedParametersPtr_createHeapPtr(Ogre::GpuSharedParametersPtr* stackSharedPtr)
{
	return new Ogre::GpuSharedParametersPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void GpuSharedParametersPtr_Delete(Ogre::GpuSharedParametersPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}