#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::OverlayManager* OverlayManager_getSingletonPtr()
{
	return Ogre::OverlayManager::getSingletonPtr();
}

extern "C" __declspec(dllexport) Ogre::Overlay* OverlayManager_create(Ogre::OverlayManager* overlayManager, String name)
{
	return overlayManager->create(name);
}

extern "C" __declspec(dllexport) Ogre::Overlay* OverlayManager_getByName(Ogre::OverlayManager* overlayManager, String name)
{
	return overlayManager->getByName(name);
}

extern "C" __declspec(dllexport) void OverlayManager_destroyName(Ogre::OverlayManager* overlayManager, String name)
{
	overlayManager->destroy(name);
}

extern "C" __declspec(dllexport) void OverlayManager_destroy(Ogre::OverlayManager* overlayManager, Ogre::Overlay* overlay)
{
	overlayManager->destroy(overlay);
}

extern "C" __declspec(dllexport) void OverlayManager_destroyAll(Ogre::OverlayManager* overlayManager)
{
	overlayManager->destroyAll();
}

extern "C" __declspec(dllexport) bool OverlayManager_hasViewportChanged(Ogre::OverlayManager* overlayManager)
{
	return overlayManager->hasViewportChanged();
}

extern "C" __declspec(dllexport) int OverlayManager_getViewportHeight(Ogre::OverlayManager* overlayManager)
{
	return overlayManager->getViewportHeight();
}

extern "C" __declspec(dllexport) int OverlayManager_getViewportWidth(Ogre::OverlayManager* overlayManager)
{
	return overlayManager->getViewportWidth();
}

extern "C" __declspec(dllexport) float OverlayManager_getViewportAspectRatio(Ogre::OverlayManager* overlayManager)
{
	return overlayManager->getViewportAspectRatio();
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayManager_createOverlayElementTypeInstance(Ogre::OverlayManager* overlayManager, String typeName, String instanceName)
{
	return overlayManager->createOverlayElement(typeName, instanceName);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayManager_createOverlayElement(Ogre::OverlayManager* overlayManager, String typeName, String instanceName, bool isTemplate)
{
	return overlayManager->createOverlayElement(typeName, instanceName, isTemplate);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayManager_getOverlayElementName(Ogre::OverlayManager* overlayManager, String name)
{
	return overlayManager->getOverlayElement(name);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayManager_getOverlayElement(Ogre::OverlayManager* overlayManager, String name, bool isTemplate)
{
	return overlayManager->getOverlayElement(name, isTemplate);
}

extern "C" __declspec(dllexport) void OverlayManager_destroyOverlayElementName(Ogre::OverlayManager* overlayManager, String name)
{
	overlayManager->destroyOverlayElement(name);
}

extern "C" __declspec(dllexport) void OverlayManager_destroyOverlayElementNameTemplate(Ogre::OverlayManager* overlayManager, String name, bool isTemplate)
{
	overlayManager->destroyOverlayElement(name, isTemplate);
}

extern "C" __declspec(dllexport) void OverlayManager_destroyOverlayElement(Ogre::OverlayManager* overlayManager, Ogre::OverlayElement* element)
{
	overlayManager->destroyOverlayElement(element);
}

extern "C" __declspec(dllexport) void OverlayManager_destroyOverlayElementTemplate(Ogre::OverlayManager* overlayManager, Ogre::OverlayElement* element, bool isTemplate)
{
	overlayManager->destroyOverlayElement(element, isTemplate);
}

extern "C" __declspec(dllexport) void OverlayManager_destroyAllOverlayElements(Ogre::OverlayManager* overlayManager)
{
	overlayManager->destroyAllOverlayElements();
}

extern "C" __declspec(dllexport) void OverlayManager_destroyAllOverlayElementsTemplate(Ogre::OverlayManager* overlayManager, bool isTemplate)
{
	overlayManager->destroyAllOverlayElements(isTemplate);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayManager_createOverlayElementFromTemplate1(Ogre::OverlayManager* overlayManager, String templateName, String typeName, String instanceName)
{
	return overlayManager->createOverlayElementFromTemplate(templateName, typeName, instanceName);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayManager_createOverlayElementFromTemplate2(Ogre::OverlayManager* overlayManager, String templateName, String typeName, String instanceName, bool isTemplate)
{
	return overlayManager->createOverlayElementFromTemplate(templateName, typeName, instanceName, isTemplate);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayManager_cloneOverlayElementFromTemplate(Ogre::OverlayManager* overlayManager, String templateName, String instanceName)
{
	return overlayManager->cloneOverlayElementFromTemplate(templateName, instanceName);
}