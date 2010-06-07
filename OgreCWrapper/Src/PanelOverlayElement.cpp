#include "Stdafx.h"
#include "OgrePanelOverlayElement.h"

extern "C" _AnomalousExport void PanelOverlayElement_setTiling(Ogre::PanelOverlayElement* panelOverlayElement, float x, float y)
{
	panelOverlayElement->setTiling(x, y);
}

extern "C" _AnomalousExport void PanelOverlayElement_setTilingLayer(Ogre::PanelOverlayElement* panelOverlayElement, float x, float y, ushort layer)
{
	panelOverlayElement->setTiling(x, y, layer);
}

extern "C" _AnomalousExport float PanelOverlayElement_getTileX(Ogre::PanelOverlayElement* panelOverlayElement)
{
	return panelOverlayElement->getTileX();
}

extern "C" _AnomalousExport float PanelOverlayElement_getTileXLayer(Ogre::PanelOverlayElement* panelOverlayElement, ushort layer)
{
	return panelOverlayElement->getTileX(layer);
}

extern "C" _AnomalousExport float PanelOverlayElement_getTileY(Ogre::PanelOverlayElement* panelOverlayElement)
{
	return panelOverlayElement->getTileY();
}

extern "C" _AnomalousExport float PanelOverlayElement_getTileYLayer(Ogre::PanelOverlayElement* panelOverlayElement, ushort layer)
{
	return panelOverlayElement->getTileY(layer);
}

extern "C" _AnomalousExport void PanelOverlayElement_setUV(Ogre::PanelOverlayElement* panelOverlayElement, float u1, float v1, float u2, float v2)
{
	panelOverlayElement->setUV(u1, v1, u2, v2);
}

extern "C" _AnomalousExport void PanelOverlayElement_getUV(Ogre::PanelOverlayElement* panelOverlayElement, float* u1, float* v1, float* u2, float* v2)
{
	panelOverlayElement->getUV(*u1, *v1, *u2, *v2);
}

extern "C" _AnomalousExport void PanelOverlayElement_setTransparent(Ogre::PanelOverlayElement* panelOverlayElement, bool isTransparent)
{
	panelOverlayElement->setTransparent(isTransparent);
}

extern "C" _AnomalousExport bool PanelOverlayElement_isTransparent(Ogre::PanelOverlayElement* panelOverlayElement)
{
	return panelOverlayElement->isTransparent();
}