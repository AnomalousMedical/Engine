#include "Stdafx.h"
#include "OgreBorderPanelOverlayElement.h"

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setBorderSize(Ogre::BorderPanelOverlayElement* borderPanel, float size)
{
	borderPanel->setBorderSize(size);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setBorderSizeSidesTB(Ogre::BorderPanelOverlayElement* borderPanel, float sides, float topAndBottom)
{
	borderPanel->setBorderSize(sides, topAndBottom);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setBorderSizeExact(Ogre::BorderPanelOverlayElement* borderPanel, float left, float right, float top, float bottom)
{
	borderPanel->setBorderSize(left, right, top, bottom);
}

extern "C" __declspec(dllexport) float BorderPanelOverlayElement_getLeftBorderSize(Ogre::BorderPanelOverlayElement* borderPanel)
{
	return borderPanel->getLeftBorderSize();
}

extern "C" __declspec(dllexport) float BorderPanelOverlayElement_getRightBorderSize(Ogre::BorderPanelOverlayElement* borderPanel)
{
	return borderPanel->getRightBorderSize();
}

extern "C" __declspec(dllexport) float BorderPanelOverlayElement_getTopBorderSize(Ogre::BorderPanelOverlayElement* borderPanel)
{
	return borderPanel->getTopBorderSize();
}

extern "C" __declspec(dllexport) float BorderPanelOverlayElement_getBottomBorderSize(Ogre::BorderPanelOverlayElement* borderPanel)
{
	return borderPanel->getBottomBorderSize();
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setLeftBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setLeftBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setRightBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setRightBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setTopBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setTopBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setBottomBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setBottomBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setTopLeftBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setTopLeftBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setTopRightBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setTopRightBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setBottomLeftBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setBottomLeftBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setBottomRightBorderUV(Ogre::BorderPanelOverlayElement* borderPanel, float u1, float v1, float u2, float v2)
{
	borderPanel->setBottomRightBorderUV(u1, v1, u2, v2);
}

extern "C" __declspec(dllexport) void BorderPanelOverlayElement_setBorderMaterialName(Ogre::BorderPanelOverlayElement* borderPanel, String name)
{
	borderPanel->setBorderMaterialName(name);
}

extern "C" __declspec(dllexport) String BorderPanelOverlayElement_getBorderMaterialName(Ogre::BorderPanelOverlayElement* borderPanel)
{
	return borderPanel->getBorderMaterialName().c_str();
}