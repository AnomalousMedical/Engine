#include "StdAfx.h"
#include "..\include\OverlayManager.h"
#include "MarshalUtils.h"
#include "Overlay.h"
#include "OverlayElement.h"

namespace Engine{

namespace Rendering{

OverlayManager::OverlayManager()
:overlayManager(Ogre::OverlayManager::getSingletonPtr())
{
}

OverlayElement^ OverlayManager::getObject(const Ogre::OverlayElement* overlayElement)
{
	return overlayElements.getObject((Ogre::OverlayElement*)overlayElement);
}

Overlay^ OverlayManager::create(System::String^ name)
{
	try
	{
		return overlays.getObject(overlayManager->create(MarshalUtils::convertString(name)));
	}
	catch(Ogre::Exception e)
	{
		Logging::Log::Default->sendMessage("Attempted to create duplicate overlay named {0} original overlay with that name returned instead.", Logging::LogLevel::Warning, "Renderer", name);
	}
	return nullptr;
}

Overlay^ OverlayManager::getByName(System::String^ name)
{
	try
	{
		return overlays.getObject(overlayManager->getByName(MarshalUtils::convertString(name)));
	}
	catch(Ogre::Exception e)
	{
		Logging::Log::Default->sendMessage("Attempted to get an overlay named {0} that does not exist.", Logging::LogLevel::Warning, "Renderer", name);
	}
	return nullptr;
}

void OverlayManager::destroy(System::String^ name)
{
	try
	{
		Ogre::String ogreString = MarshalUtils::convertString(name);
		overlays.destroyObject(overlayManager->getByName(ogreString));
		overlayManager->destroy(ogreString);
	}
	catch(Ogre::Exception e)
	{
		Logging::Log::Default->sendMessage("Attempted to destroy an overlay named {0} that does not exist.", Logging::LogLevel::Warning, "Renderer", name);
	}
}

void OverlayManager::destroy(Overlay^ overlay)
{
	Ogre::Overlay* ogreOverlay = overlay->getOverlay();
	overlays.destroyObject(ogreOverlay);
	overlayManager->destroy(ogreOverlay);
}

void OverlayManager::destroyAll()
{
	overlays.clearObjects();
	Ogre::OverlayManager::getSingleton().destroyAll();
}

bool OverlayManager::hasViewportChanged()
{
	return Ogre::OverlayManager::getSingleton().hasViewportChanged();
}

int OverlayManager::getViewportHeight()
{
	return Ogre::OverlayManager::getSingleton()	.getViewportHeight();
}

int OverlayManager::getViewportWidth()
{
	return Ogre::OverlayManager::getSingleton().getViewportWidth();
}

float OverlayManager::getViewportAspectRatio()
{
	return Ogre::OverlayManager::getSingleton().getViewportAspectRatio();
}

OverlayElement^ OverlayManager::createOverlayElement(System::String^ typeName, System::String^ instanceName)
{
	return getObject(overlayManager->createOverlayElement(MarshalUtils::convertString(typeName), MarshalUtils::convertString(instanceName)));
}

OverlayElement^ OverlayManager::createOverlayElement(System::String^ typeName, System::String^ instanceName, bool isTemplate)
{
	return getObject(overlayManager->createOverlayElement(MarshalUtils::convertString(typeName), MarshalUtils::convertString(instanceName)));
}

OverlayElement^ OverlayManager::getOverlayElement(System::String^ name)
{
	return getObject(overlayManager->getOverlayElement(MarshalUtils::convertString(name)));
}

OverlayElement^ OverlayManager::getOverlayElement(System::String^ name, bool isTemplate)
{
	return getObject(overlayManager->getOverlayElement(MarshalUtils::convertString(name), isTemplate));
}

void OverlayManager::destroyOverlayElement(System::String^ name)
{
	overlayElements.destroyObject(overlayManager->getOverlayElement(MarshalUtils::convertString(name)));
	return overlayManager->destroyOverlayElement(MarshalUtils::convertString(name));
}

void OverlayManager::destroyOverlayElement(System::String^ name, bool isTemplate)
{
	overlayElements.destroyObject(overlayManager->getOverlayElement(MarshalUtils::convertString(name), isTemplate));
	return overlayManager->destroyOverlayElement(MarshalUtils::convertString(name), isTemplate);
}

void OverlayManager::destroyOverlayElement(OverlayElement^ element)
{
	Ogre::OverlayElement* ogreElement = element->getOverlayElement();
	overlayElements.destroyObject(ogreElement);
	return overlayManager->destroyOverlayElement(ogreElement);
}

void OverlayManager::destroyOverlayElement(OverlayElement^ element, bool isTemplate)
{
	Ogre::OverlayElement* ogreElement = element->getOverlayElement();
	overlayElements.destroyObject(ogreElement);
	return overlayManager->destroyOverlayElement(ogreElement, isTemplate);
}

void OverlayManager::destroyAllOverlayElements()
{
	overlayElements.clearObjects();
	return overlayManager->destroyAllOverlayElements();
}

void OverlayManager::destroyAllOverlayElements(bool isTemplate)
{
	overlayElements.clearObjects();
	return overlayManager->destroyAllOverlayElements(isTemplate);
}

OverlayElement^ OverlayManager::createOverlayElementFromTemplate(System::String^ templateName, System::String^ typeName, System::String^ instanceName)
{
	return getObject(overlayManager->createOverlayElementFromTemplate(MarshalUtils::convertString(templateName), MarshalUtils::convertString(typeName), MarshalUtils::convertString(instanceName)));
}

OverlayElement^ OverlayManager::createOverlayElementFromTemplate(System::String^ templateName, System::String^ typeName, System::String^ instanceName, bool isTemplate)
{
	return getObject(overlayManager->createOverlayElementFromTemplate(MarshalUtils::convertString(templateName), MarshalUtils::convertString(typeName), MarshalUtils::convertString(instanceName), isTemplate));
}

OverlayElement^ OverlayManager::cloneOverlayElementFromTemplate(System::String^ templateName, System::String^ instanceName)
{
	return getObject(overlayManager->cloneOverlayElementFromTemplate(MarshalUtils::convertString(templateName), MarshalUtils::convertString(instanceName)));
}

}

}