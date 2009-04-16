#include "StdAfx.h"
#include "..\include\PanelOverlayElement.h"

#pragma warning(push)
#pragma warning(disable : 4251) //Disable dll interface warning
#include "OgrePanelOverlayElement.h"
#pragma warning(pop)

namespace Rendering
{

PanelOverlayElement::PanelOverlayElement(Ogre::PanelOverlayElement* panelOverlay)
:OverlayContainer(panelOverlay), 
panelOverlay( panelOverlay )
{

}

PanelOverlayElement::~PanelOverlayElement()
{
	panelOverlay = 0;
}

Ogre::PanelOverlayElement* PanelOverlayElement::getPanelOverlayElement()
{
	return panelOverlay;
}

void PanelOverlayElement::setTiling(float x, float y)
{
	return panelOverlay->setTiling(x, y);
}

void PanelOverlayElement::setTiling(float x, float y, unsigned short layer)
{
	return panelOverlay->setTiling(x, y, layer);
}

float PanelOverlayElement::getTileX()
{
	return panelOverlay->getTileX();
}

float PanelOverlayElement::getTileX(unsigned short layer)
{
	return panelOverlay->getTileX(layer);
}

float PanelOverlayElement::getTileY()
{
	return panelOverlay->getTileY();
}

float PanelOverlayElement::getTileY(unsigned short layer)
{
	return panelOverlay->getTileY(layer);
}

void PanelOverlayElement::setUV(float u1, float v1, float u2, float v2)
{
	return panelOverlay->setUV(u1, v1, u2, v2);
}

void PanelOverlayElement::getUV(float% u1, float% v1, float% u2, float% v2)
{
	return panelOverlay->setUV(u1, v1, u2, v2);
}

void PanelOverlayElement::setTransparent(bool isTransparent)
{
	return panelOverlay->setTransparent(isTransparent);
}

bool PanelOverlayElement::isTransparent()
{
	return panelOverlay->isTransparent();
}

}