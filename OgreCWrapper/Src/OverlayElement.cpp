#include "Stdafx.h"
#include "OgreOverlayElement.h"

extern "C" _AnomalousExport void OverlayElement_initialize(Ogre::OverlayElement* overlayElement)
{
	overlayElement->initialise();
}

extern "C" _AnomalousExport String OverlayElement_getName(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getName().c_str();
}

extern "C" _AnomalousExport void OverlayElement_show(Ogre::OverlayElement* overlayElement)
{
	overlayElement->show();
}

extern "C" _AnomalousExport void OverlayElement_hide(Ogre::OverlayElement* overlayElement)
{
	overlayElement->hide();
}

extern "C" _AnomalousExport bool OverlayElement_isVisible(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isVisible();
}

extern "C" _AnomalousExport bool OverlayElement_isEnabled(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isEnabled();
}

extern "C" _AnomalousExport void OverlayElement_setEnabled(Ogre::OverlayElement* overlayElement, bool b)
{
	overlayElement->setEnabled(b);
}

extern "C" _AnomalousExport void OverlayElement_setDimensions(Ogre::OverlayElement* overlayElement, float width, float height)
{
	overlayElement->setDimensions(width, height);
}

extern "C" _AnomalousExport void OverlayElement_setPosition(Ogre::OverlayElement* overlayElement, float left, float top)
{
	overlayElement->setPosition(left, top);
}

extern "C" _AnomalousExport void OverlayElement_setWidth(Ogre::OverlayElement* overlayElement, float width)
{
	overlayElement->setWidth(width);
}

extern "C" _AnomalousExport float OverlayElement_getWidth(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getWidth();
}

extern "C" _AnomalousExport void OverlayElement_setHeight(Ogre::OverlayElement* overlayElement, float height)
{
	overlayElement->setHeight(height);
}

extern "C" _AnomalousExport float OverlayElement_getHeight(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getHeight();
}

extern "C" _AnomalousExport void OverlayElement_setLeft(Ogre::OverlayElement* overlayElement, float left)
{
	overlayElement->setLeft(left);
}

extern "C" _AnomalousExport float OverlayElement_getLeft(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getLeft();
}

extern "C" _AnomalousExport void OverlayElement_setTop(Ogre::OverlayElement* overlayElement, float top)
{
	overlayElement->setTop(top);
}

extern "C" _AnomalousExport float OverlayElement_getTop(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getTop();
}

extern "C" _AnomalousExport String OverlayElement_getMaterialName(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getMaterialName().c_str();
}

extern "C" _AnomalousExport void OverlayElement_setMaterialName(Ogre::OverlayElement* overlayElement, String matName)
{
	overlayElement->setMaterialName(matName);
}

extern "C" _AnomalousExport Ogre::Material* OverlayElement_getMaterial(Ogre::OverlayElement* overlayElement, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::MaterialPtr& materialPtr = overlayElement->getMaterial();
	processWrapper(materialPtr.getPointer(), &materialPtr);
	return materialPtr.getPointer();
}

extern "C" _AnomalousExport float OverlayElement_getDerivedLeft(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->_getDerivedLeft();
}

extern "C" _AnomalousExport float OverlayElement_getDerivedTop(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->_getDerivedTop();
}

extern "C" _AnomalousExport String OverlayElement_getTypeName(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getTypeName().c_str();
}

extern "C" _AnomalousExport void OverlayElement_setCaption(Ogre::OverlayElement* overlayElement, Ogre::UTFString::code_point* displayString)
{
	overlayElement->setCaption(displayString);
}

extern "C" _AnomalousExport const Ogre::UTFString::code_point* OverlayElement_getCaption(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getCaption().c_str();
}

extern "C" _AnomalousExport void OverlayElement_setColor(Ogre::OverlayElement* overlayElement, Color color)
{
	overlayElement->setColour(color.toOgre());
}

extern "C" _AnomalousExport Color OverlayElement_getColor(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getColour();
}

extern "C" _AnomalousExport void OverlayElement_setMetricsMode(Ogre::OverlayElement* overlayElement, Ogre::GuiMetricsMode mode)
{
	overlayElement->setMetricsMode(mode);
}

extern "C" _AnomalousExport Ogre::GuiMetricsMode OverlayElement_getMetricsMode(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getMetricsMode();
}

extern "C" _AnomalousExport void OverlayElement_setHorizontalAlignment(Ogre::OverlayElement* overlayElement, Ogre::GuiHorizontalAlignment gha)
{
	overlayElement->setHorizontalAlignment(gha);
}

extern "C" _AnomalousExport Ogre::GuiHorizontalAlignment OverlayElement_getHorizontalAlignment(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getHorizontalAlignment();
}

extern "C" _AnomalousExport void OverlayElement_setVerticalAlignment(Ogre::OverlayElement* overlayElement, Ogre::GuiVerticalAlignment gva)
{
	overlayElement->setVerticalAlignment(gva);
}

extern "C" _AnomalousExport Ogre::GuiVerticalAlignment OverlayElement_getVerticalAlignment(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getVerticalAlignment();
}

extern "C" _AnomalousExport bool OverlayElement_contains(Ogre::OverlayElement* overlayElement, float x, float y)
{
	return overlayElement->contains(x, y);
}

extern "C" _AnomalousExport Ogre::OverlayElement* OverlayElement_findElementAt(Ogre::OverlayElement* overlayElement, float x, float y)
{
	return overlayElement->findElementAt(x, y);
}

extern "C" _AnomalousExport bool OverlayElement_isContainer(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isContainer();
}

extern "C" _AnomalousExport bool OverlayElement_isKeyEnabled(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isKeyEnabled();
}

extern "C" _AnomalousExport bool OverlayElement_isCloneable(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isCloneable();
}

extern "C" _AnomalousExport void OverlayElement_setCloneable(Ogre::OverlayElement* overlayElement, bool c)
{
	overlayElement->setCloneable(c);
}

extern "C" _AnomalousExport Ogre::OverlayContainer* OverlayElement_getParent(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getParent();
}

extern "C" _AnomalousExport ushort OverlayElement_getZOrder(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getZOrder();
}

extern "C" _AnomalousExport void OverlayElement_copyFromTemplate(Ogre::OverlayElement* overlayElement, Ogre::OverlayElement* templateOverlay)
{
	overlayElement->copyFromTemplate(templateOverlay);
}

extern "C" _AnomalousExport Ogre::OverlayElement* OverlayElement_clone(Ogre::OverlayElement* overlayElement, String instanceName)
{
	return overlayElement->clone(instanceName);
}

extern "C" _AnomalousExport const Ogre::OverlayElement* OverlayElement_getSourceTemplate(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getSourceTemplate();
}