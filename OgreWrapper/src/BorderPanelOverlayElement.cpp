#include "StdAfx.h"
#include "..\include\BorderPanelOverlayElement.h"
#include "MarshalUtils.h"

#pragma warning(push)
#pragma warning(disable : 4251) //Disable dll interface warning
#include "OgreBorderPanelOverlayElement.h"
#pragma warning(pop)

namespace OgreWrapper
{

BorderPanelOverlayElement::BorderPanelOverlayElement(Ogre::BorderPanelOverlayElement* borderPanel)
:PanelOverlayElement(borderPanel),
borderPanel( borderPanel )
{

}

BorderPanelOverlayElement::~BorderPanelOverlayElement()
{
	borderPanel = 0;
}

Ogre::BorderPanelOverlayElement* BorderPanelOverlayElement::getBorderPanelOverlayElement()
{
	return borderPanel;
}

void BorderPanelOverlayElement::setBorderSize(float size)
{
	return borderPanel->setBorderSize(size);
}

void BorderPanelOverlayElement::setBorderSize(float sides, float topAndBottom)
{
	return borderPanel->setBorderSize(sides, topAndBottom);
}

void BorderPanelOverlayElement::setBorderSize(float left, float right, float top, float bottom)
{
	return borderPanel->setBorderSize(left, right, top, bottom);
}

float BorderPanelOverlayElement::getLeftBorderSize()
{
	return borderPanel->getLeftBorderSize();
}

float BorderPanelOverlayElement::getRightBorderSize()
{
	return borderPanel->getRightBorderSize();
}

float BorderPanelOverlayElement::getTopBorderSize()
{
	return borderPanel->getTopBorderSize();
}

float BorderPanelOverlayElement::getBottomBorderSize()
{
	return borderPanel->getBottomBorderSize();
}

void BorderPanelOverlayElement::setLeftBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setLeftBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setRightBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setRightBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setTopBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setTopBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setBottomBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setBottomBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setTopLeftBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setTopLeftBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setTopRightBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setTopRightBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setBottomLeftBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setBottomLeftBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setBottomRightBorderUV(float u1, float v1, float u2, float v2)
{
	return borderPanel->setBottomRightBorderUV(u1, v1, u2, v2);
}

void BorderPanelOverlayElement::setBorderMaterialName(System::String^ name)
{
	return borderPanel->setBorderMaterialName(MarshalUtils::convertString(name));
}

System::String^ BorderPanelOverlayElement::getBorderMaterialName()
{
	return MarshalUtils::convertString(borderPanel->getBorderMaterialName());
}

}