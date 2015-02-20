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

extern "C" _AnomalousExport void ResourceManager_destroyAllTexturesForResource(MyGUI::ResourceManager* resourceManager, String name)
{
	MyGUI::IResourcePtr resource = resourceManager->getByName(name, false);
	MyGUI::ResourceImageSetPtr imageResource = resource ? resource->castType<MyGUI::ResourceImageSet>(false) : nullptr;
	if (imageResource != nullptr)
	{
		MyGUI::EnumeratorGroupImage iter_group = imageResource->getEnumerator();
		while (iter_group.next())
		{
			MyGUI::RenderManager::getInstance().destroyTexture(iter_group.current().texture);
		}
	}
}