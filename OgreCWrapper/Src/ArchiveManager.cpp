#include "StdAfx.h"

extern "C" _AnomalousExport void ArchiveManager_unload(String filename)
{
	Ogre::ArchiveManager::getSingletonPtr()->unload(filename);
}