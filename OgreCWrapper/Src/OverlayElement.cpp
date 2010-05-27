#include "Stdafx.h"

extern "C" __declspec(dllexport) void OverlayElement_initialize(Ogre::OverlayElement* overlayElement)
{
	overlayElement->initialise();
}

extern "C" __declspec(dllexport) String OverlayElement_getName(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getName().c_str();
}

extern "C" __declspec(dllexport) void OverlayElement_show(Ogre::OverlayElement* overlayElement)
{
	overlayElement->show();
}

extern "C" __declspec(dllexport) void OverlayElement_hide(Ogre::OverlayElement* overlayElement)
{
	overlayElement->hide();
}

extern "C" __declspec(dllexport) bool OverlayElement_isVisible(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isVisible();
}

extern "C" __declspec(dllexport) bool OverlayElement_isEnabled(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isEnabled();
}

extern "C" __declspec(dllexport) void OverlayElement_setEnabled(Ogre::OverlayElement* overlayElement, bool b)
{
	overlayElement->setEnabled(b);
}

extern "C" __declspec(dllexport) void OverlayElement_setDimensions(Ogre::OverlayElement* overlayElement, float width, float height)
{
	overlayElement->setDimensions(width, height);
}

extern "C" __declspec(dllexport) void OverlayElement_setPosition(Ogre::OverlayElement* overlayElement, float left, float top)
{
	overlayElement->setPosition(left, top);
}

extern "C" __declspec(dllexport) void OverlayElement_setWidth(Ogre::OverlayElement* overlayElement, float width)
{
	overlayElement->setWidth(width);
}

extern "C" __declspec(dllexport) float OverlayElement_getWidth(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getWidth();
}

extern "C" __declspec(dllexport) void OverlayElement_setHeight(Ogre::OverlayElement* overlayElement, float height)
{
	overlayElement->setHeight(height);
}

extern "C" __declspec(dllexport) float OverlayElement_getHeight(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getHeight();
}

extern "C" __declspec(dllexport) void OverlayElement_setLeft(Ogre::OverlayElement* overlayElement, float left)
{
	overlayElement->setLeft(left);
}

extern "C" __declspec(dllexport) float OverlayElement_getLeft(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getLeft();
}

extern "C" __declspec(dllexport) void OverlayElement_setTop(Ogre::OverlayElement* overlayElement, float top)
{
	overlayElement->setTop(top);
}

extern "C" __declspec(dllexport) float OverlayElement_getTop(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getTop();
}

extern "C" __declspec(dllexport) String OverlayElement_getMaterialName(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getMaterialName().c_str();
}

extern "C" __declspec(dllexport) void OverlayElement_setMaterialName(Ogre::OverlayElement* overlayElement, String matName)
{
	overlayElement->setMaterialName(matName);
}

//Will return a new heap allocated Ogre::MaterialPtr that must be deleted
extern "C" __declspec(dllexport) Ogre::MaterialPtr* OverlayElement_getMaterial(Ogre::OverlayElement* overlayElement)
{
	return new Ogre::MaterialPtr(overlayElement->getMaterial());
}

extern "C" __declspec(dllexport) float OverlayElement_getDerivedLeft(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->_getDerivedLeft();
}

extern "C" __declspec(dllexport) float OverlayElement_getDerivedTop(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->_getDerivedTop();
}

extern "C" __declspec(dllexport) String OverlayElement_getTypeName(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getTypeName().c_str();
}

extern "C" __declspec(dllexport) void OverlayElement_setCaption(Ogre::OverlayElement* overlayElement, String displayString)
{
	overlayElement->setCaption(displayString);
}

extern "C" __declspec(dllexport) const Ogre::UTFString::code_point* OverlayElement_getCaption(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getCaption().c_str();
}

extern "C" __declspec(dllexport) void OverlayElement_setColor(Ogre::OverlayElement* overlayElement, Color color)
{
	overlayElement->setColour(color.toOgre());
}

extern "C" __declspec(dllexport) Color OverlayElement_getColor(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getColour();
}

extern "C" __declspec(dllexport) void OverlayElement_setMetricsMode(Ogre::OverlayElement* overlayElement, Ogre::GuiMetricsMode mode)
{
	overlayElement->setMetricsMode(mode);
}

extern "C" __declspec(dllexport) Ogre::GuiMetricsMode OverlayElement_getMetricsMode(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getMetricsMode();
}

extern "C" __declspec(dllexport) void OverlayElement_setHorizontalAlignment(Ogre::OverlayElement* overlayElement, Ogre::GuiHorizontalAlignment gha)
{
	overlayElement->setHorizontalAlignment(gha);
}

extern "C" __declspec(dllexport) Ogre::GuiHorizontalAlignment OverlayElement_getHorizontalAlignment(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getHorizontalAlignment();
}

extern "C" __declspec(dllexport) void OverlayElement_setVerticalAlignment(Ogre::OverlayElement* overlayElement, Ogre::GuiVerticalAlignment gva)
{
	overlayElement->setVerticalAlignment(gva);
}

extern "C" __declspec(dllexport) Ogre::GuiVerticalAlignment OverlayElement_getVerticalAlignment(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getVerticalAlignment();
}

extern "C" __declspec(dllexport) bool OverlayElement_contains(Ogre::OverlayElement* overlayElement, float x, float y)
{
	return overlayElement->contains(x, y);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayElement_findElementAt(Ogre::OverlayElement* overlayElement, float x, float y)
{
	return overlayElement->findElementAt(x, y);
}

extern "C" __declspec(dllexport) bool OverlayElement_isContainer(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isContainer();
}

extern "C" __declspec(dllexport) bool OverlayElement_isKeyEnabled(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isKeyEnabled();
}

extern "C" __declspec(dllexport) bool OverlayElement_isCloneable(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->isCloneable();
}

extern "C" __declspec(dllexport) void OverlayElement_setCloneable(Ogre::OverlayElement* overlayElement, bool c)
{
	overlayElement->setCloneable(c);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayElement_getParent(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getParent();
}

extern "C" __declspec(dllexport) ushort OverlayElement_getZOrder(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getZOrder();
}

extern "C" __declspec(dllexport) void OverlayElement_copyFromTemplate(Ogre::OverlayElement* overlayElement, Ogre::OverlayElement* templateOverlay)
{
	overlayElement->copyFromTemplate(templateOverlay);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayElement_clone(Ogre::OverlayElement* overlayElement, String instanceName)
{
	return overlayElement->clone(instanceName);
}

extern "C" __declspec(dllexport) const Ogre::OverlayElement* OverlayElement_getSourceTemplate(Ogre::OverlayElement* overlayElement)
{
	return overlayElement->getSourceTemplate();
}