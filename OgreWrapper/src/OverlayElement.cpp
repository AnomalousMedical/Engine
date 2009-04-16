#include "StdAfx.h"
#include "..\include\OverlayElement.h"
#include "MarshalUtils.h"
#include "MathUtils.h"
#include "MaterialManager.h"
#include "OgreOverlayElement.h"
#include "Color.h"
#include "OverlayManager.h"
#include "OverlayContainer.h"

namespace OgreWrapper
{

OverlayElement::OverlayElement(Ogre::OverlayElement* overlayElement)
:overlayElement( overlayElement )
{

}

OverlayElement::~OverlayElement()
{
	overlayElement = 0;
}

Ogre::OverlayElement* OverlayElement::getOverlayElement()
{
	return overlayElement;
}

void OverlayElement::initialize()
{
	return overlayElement->initialise();
}

System::String^ OverlayElement::getName()
{
	return MarshalUtils::convertString(overlayElement->getName());
}

void OverlayElement::show()
{
	return overlayElement->show();
}

void OverlayElement::hide()
{
	return overlayElement->hide();
}

bool OverlayElement::isVisible()
{
	return overlayElement->isVisible();
}

bool OverlayElement::isEnabled()
{
	return overlayElement->isEnabled();
}

void OverlayElement::setEnabled(bool b)
{
	return overlayElement->setEnabled(b);
}

void OverlayElement::setDimensions(float width, float height)
{
	return overlayElement->setDimensions(width, height);
}

void OverlayElement::setPosition(float left, float top)
{
	return overlayElement->setPosition(left, top);
}

void OverlayElement::setWidth(float width)
{
	return overlayElement->setWidth(width);
}

float OverlayElement::getWidth()
{
	return overlayElement->getWidth();
}

void OverlayElement::setHeight(float height)
{
	return overlayElement->setHeight(height);
}

float OverlayElement::getHeight()
{
	return overlayElement->getHeight();
}

void OverlayElement::setLeft(float left)
{
	return overlayElement->setLeft(left);
}

float OverlayElement::getLeft()
{
	return overlayElement->getLeft();
}

void OverlayElement::setTop(float top)
{
	return overlayElement->setTop(top);
}

float OverlayElement::getTop()
{
	return overlayElement->getTop();
}

System::String^ OverlayElement::getMaterialName()
{
	return MarshalUtils::convertString(overlayElement->getMaterialName());
}

void OverlayElement::setMaterialName(System::String^ matName)
{
	return overlayElement->setMaterialName(MarshalUtils::convertString(matName));
}

MaterialPtr^ OverlayElement::getMaterial()
{
	return MaterialManager::getInstance()->getObject(overlayElement->getMaterial());
}

System::String^ OverlayElement::getTypeName()
{
	return MarshalUtils::convertString(overlayElement->getTypeName());
}

void OverlayElement::setCaption(System::String^ displayString)
{
	pin_ptr<System::Char> str = const_cast<interior_ptr<System::Char>>(PtrToStringChars(displayString));
	return overlayElement->setCaption(str);
}

System::String^ OverlayElement::getCaption()
{
	return MarshalUtils::convertString(overlayElement->getCaption());
}

void OverlayElement::setColor(Color color)
{
	return overlayElement->setColour(MathUtils::copyColor(color));
}

void OverlayElement::setColor(Color% color)
{
	return overlayElement->setColour(MathUtils::copyColor(color));
}

Color OverlayElement::getColor()
{
	return MathUtils::copyColor(overlayElement->getColour());
}

void OverlayElement::setMetricsMode(GuiMetricsMode mode)
{
	return overlayElement->setMetricsMode(static_cast<Ogre::GuiMetricsMode>(mode));
}

GuiMetricsMode OverlayElement::getMetricsMode()
{
	return static_cast<GuiMetricsMode>(overlayElement->getMetricsMode());
}

void OverlayElement::setHorizontalAlignment(GuiHorizontalAlignment gha)
{
	return overlayElement->setHorizontalAlignment(static_cast<Ogre::GuiHorizontalAlignment>(gha));
}

GuiHorizontalAlignment OverlayElement::getHorizontalAlignment()
{
	return static_cast<GuiHorizontalAlignment>(overlayElement->getHorizontalAlignment());
}

void OverlayElement::setVerticalAlignment(GuiVerticalAlignment gva)
{
	return overlayElement->setVerticalAlignment(static_cast<Ogre::GuiVerticalAlignment>(gva));
}

GuiVerticalAlignment OverlayElement::getVerticalAlignment()
{
	return static_cast<GuiVerticalAlignment>(overlayElement->getVerticalAlignment());
}

bool OverlayElement::contains(float x, float y)
{
	return overlayElement->contains(x, y);
}

OverlayElement^ OverlayElement::findElementAt(float x, float y)
{
	return OverlayManager::getInstance()->getObject(overlayElement->findElementAt(x, y));
}

bool OverlayElement::isContainer()
{
	return overlayElement->isContainer();
}

bool OverlayElement::isKeyEnabled()
{
	return overlayElement->isKeyEnabled();
}

bool OverlayElement::isCloneable()
{
	return overlayElement->isCloneable();
}

void OverlayElement::setCloneable(bool c)
{
	return overlayElement->setCloneable(c);
}

OverlayContainer^ OverlayElement::getParent()
{
	return static_cast<OverlayContainer^>(OverlayManager::getInstance()->getObject(overlayElement->getParent()));
}

unsigned short OverlayElement::getZOrder()
{
	return overlayElement->getZOrder();
}

void OverlayElement::copyFromTemplate(OverlayElement^ templateOverlay)
{
	return overlayElement->copyFromTemplate(templateOverlay->overlayElement);
}

OverlayElement^ OverlayElement::clone(System::String^ instanceName)
{
	return OverlayManager::getInstance()->getObject(overlayElement->clone(MarshalUtils::convertString(instanceName)));
}

OverlayElement^ OverlayElement::getSourceTemplate()
{
	return OverlayManager::getInstance()->getObject(overlayElement->getSourceTemplate());
}

}