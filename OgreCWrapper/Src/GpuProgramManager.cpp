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