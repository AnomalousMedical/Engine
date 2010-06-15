#include "Stdafx.h"

extern "C" _AnomalousExport CEGUI::ImagesetManager* ImagesetManager_getSingletonPtr()
{
	return CEGUI::ImagesetManager::getSingletonPtr();
}

extern "C" _AnomalousExport CEGUI::Imageset* ImagesetManager_create(CEGUI::ImagesetManager* imagesetManager, String xmlFileName)
{
	return &imagesetManager->create(xmlFileName);
}

extern "C" _AnomalousExport CEGUI::Imageset* ImagesetManager_create2(CEGUI::ImagesetManager* imagesetManager, String xmlFileName, String resourceGroup)
{
	return &imagesetManager->create(xmlFileName, resourceGroup);
}

extern "C" _AnomalousExport void ImagesetManager_destroy(CEGUI::ImagesetManager* imagesetManager, String objectName)
{
	imagesetManager->destroy(objectName);
}

extern "C" _AnomalousExport void ImagesetManager_destroy2(CEGUI::ImagesetManager* imagesetManager, CEGUI::Imageset* imageSet)
{
	imagesetManager->destroy(*imageSet);
}

extern "C" _AnomalousExport void ImagesetManager_destroyAll(CEGUI::ImagesetManager* imagesetManager)
{
	imagesetManager->destroyAll();
}

extern "C" _AnomalousExport CEGUI::Imageset* ImagesetManager_get(CEGUI::ImagesetManager* imagesetManager, String objectName)
{
	return &imagesetManager->get(objectName);
}

extern "C" _AnomalousExport bool ImagesetManager_isDefined(CEGUI::ImagesetManager* imagesetManager, String objectName)
{
	return imagesetManager->isDefined(objectName);
}

extern "C" _AnomalousExport void ImagesetManager_createAll(CEGUI::ImagesetManager* imagesetManager, String pattern, String resourceGroup)
{
	imagesetManager->createAll(pattern, resourceGroup);
}