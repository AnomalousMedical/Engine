#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::HighLevelGpuProgram* HighLevelGpuProgramManager_createProgram(String name, String group, String language, Ogre::GpuProgramType gptype, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::HighLevelGpuProgramPtr& ptr = Ogre::HighLevelGpuProgramManager::getSingleton().createProgram(name, group, language, gptype);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::HighLevelGpuProgram* HighLevelGpuProgramManager_getByName1(String name, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::HighLevelGpuProgramPtr& ptr = Ogre::HighLevelGpuProgramManager::getSingleton().getByName(name);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::HighLevelGpuProgram* HighLevelGpuProgramManager_getByName2(String name, String group, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::HighLevelGpuProgramPtr& ptr = Ogre::HighLevelGpuProgramManager::getSingleton().getByName(name, group);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::HighLevelGpuProgramPtr* HighLevelGpuProgram_createHeapPtr(Ogre::HighLevelGpuProgramPtr* stackSharedPtr)
{
	return new Ogre::HighLevelGpuProgramPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void HighLevelGpuProgram_Delete(Ogre::HighLevelGpuProgramPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}