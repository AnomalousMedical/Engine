#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::ResourceManager* ResourceManager_getInstance()
{
	return MyGUI::ResourceManager::getInstancePtr();
}

extern "C" _AnomalousExport bool ResourceManager_load(MyGUI::ResourceManager* resourceManager, String file)
{
	return resourceManager->load(file);
}

extern "C" _AnomalousExport bool ResourceManager_removeByName(MyGUI::ResourceManager* resourceManager, String name)
{
	return resourceManager->removeByName(name);
}

extern "C" _AnomalousExport void ResourceManager_clear(MyGUI::ResourceManager* resourceManager)
{
	resourceManager->clear();
}

extern "C" _AnomalousExport size_t ResourceManager_getCount(MyGUI::ResourceManager* resourceManager)
{
	return resourceManager->getCount();
}