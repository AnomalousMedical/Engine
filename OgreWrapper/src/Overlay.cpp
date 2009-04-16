#include "StdAfx.h"
#include "..\include\Overlay.h"
#include "MarshalUtils.h"
#include "SceneNode.h"
#include "OgreOverlay.h"
#include "OverlayContainer.h"
#include "OverlayManager.h"

namespace OgreWrapper{

Overlay::Overlay(Ogre::Overlay* overlay)
:overlay(overlay)
{
}

Overlay::~Overlay(void)
{
	overlay = 0;
}

Ogre::Overlay* Overlay::getOverlay()
{
	return overlay;
}

OverlayContainer^ Overlay::getChild(System::String^ name)
{
	return static_cast<OverlayContainer^>(OverlayManager::getInstance()->getObject(overlay->getChild(MarshalUtils::convertString(name))));
}

System::String^ Overlay::getName()
{
	return MarshalUtils::convertString(overlay->getName());
}

void Overlay::setZOrder(unsigned short zOrder)
{
	overlay->setZOrder(zOrder);
}

unsigned short Overlay::getZOrder()
{
	return overlay->getZOrder();
}

bool Overlay::isVisible()
{
	return overlay->isVisible();
}

bool Overlay::isInitialized()
{
	return overlay->isInitialised();
}

void Overlay::show()
{
	overlay->show();
}

void Overlay::hide()
{
	overlay->hide();
}

void Overlay::add2d(OverlayContainer^ cont)
{
	overlay->add2D(cont->getOverlayContainer());
}

void Overlay::remove2d(OverlayContainer^ cont)
{
	overlay->remove2D(cont->getOverlayContainer());
}

void Overlay::add3d(SceneNode^ node)
{
	overlay->add3D(node->getSceneNode());
}

void Overlay::remove3d(SceneNode^ node)
{
	overlay->remove3D(node->getSceneNode());
}

void Overlay::clear()
{
	overlay->clear();
}

void Overlay::setScroll(float x, float y)
{
	overlay->setScroll(x, y);
}

float Overlay::getScrollX()
{
	return overlay->getScrollX();
}

float Overlay::getScrollY()
{
	return overlay->getScrollY();
}

void Overlay::scroll(float xOff, float yOff)
{
	overlay->scroll(xOff, yOff);
}

void Overlay::setRotate(float radAngle)
{
	overlay->setRotate(Ogre::Radian(radAngle));
}

float Overlay::getRotate()
{
	return overlay->getRotate().valueRadians();
}

void Overlay::rotate(float radAngle)
{
	overlay->rotate(Ogre::Radian(radAngle));
}

void Overlay::setScale(float x, float y)
{
	overlay->setScale(x, y);
}

float Overlay::getScaleX()
{
	return overlay->getScaleX();
}

float Overlay::getScaleY()
{
	return overlay->getScaleY();
}

}