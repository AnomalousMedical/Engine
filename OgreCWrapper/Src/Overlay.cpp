#include "Stdafx.h"
#include "OgreOverlay.h"

extern "C" _AnomalousExport Ogre::OverlayContainer* Overlay_getChild(Ogre::Overlay* overlay, String name)
{
	return overlay->getChild(name);
}

extern "C" _AnomalousExport String Overlay_getName(Ogre::Overlay* overlay)
{
	return overlay->getName().c_str();
}

extern "C" _AnomalousExport void Overlay_setZOrder(Ogre::Overlay* overlay, ushort zOrder)
{
	overlay->setZOrder(zOrder);
}

extern "C" _AnomalousExport ushort Overlay_getZOrder(Ogre::Overlay* overlay)
{
	return overlay->getZOrder();
}

extern "C" _AnomalousExport bool Overlay_isVisible(Ogre::Overlay* overlay)
{
	return overlay->isVisible();
}

extern "C" _AnomalousExport bool Overlay_isInitialized(Ogre::Overlay* overlay)
{
	return overlay->isInitialised();
}

extern "C" _AnomalousExport void Overlay_show(Ogre::Overlay* overlay)
{
	overlay->show();
}

extern "C" _AnomalousExport void Overlay_hide(Ogre::Overlay* overlay)
{
	overlay->hide();
}

extern "C" _AnomalousExport void Overlay_add2d(Ogre::Overlay* overlay, Ogre::OverlayContainer* cont)
{
	overlay->add2D(cont);
}

extern "C" _AnomalousExport void Overlay_remove2d(Ogre::Overlay* overlay, Ogre::OverlayContainer* cont)
{
	overlay->remove2D(cont);
}

extern "C" _AnomalousExport void Overlay_add3d(Ogre::Overlay* overlay, Ogre::SceneNode* node)
{
	overlay->add3D(node);
}

extern "C" _AnomalousExport void Overlay_remove3d(Ogre::Overlay* overlay, Ogre::SceneNode* node)
{
	overlay->remove3D(node);
}

extern "C" _AnomalousExport void Overlay_clear(Ogre::Overlay* overlay)
{
	overlay->clear();
}

extern "C" _AnomalousExport void Overlay_setScroll(Ogre::Overlay* overlay, float x, float y)
{
	overlay->setScroll(x, y);
}

extern "C" _AnomalousExport float Overlay_getScrollX(Ogre::Overlay* overlay)
{
	return overlay->getScrollX();
}

extern "C" _AnomalousExport float Overlay_getScrollY(Ogre::Overlay* overlay)
{
	return overlay->getScrollY();
}

extern "C" _AnomalousExport void Overlay_scroll(Ogre::Overlay* overlay, float xOff, float yOff)
{
	overlay->scroll(xOff, yOff);
}

extern "C" _AnomalousExport void Overlay_setRotate(Ogre::Overlay* overlay, float radAngle)
{
	overlay->setRotate(Ogre::Radian(radAngle));
}

extern "C" _AnomalousExport float Overlay_getRotate(Ogre::Overlay* overlay)
{
	return overlay->getRotate().valueRadians();
}

extern "C" _AnomalousExport void Overlay_rotate(Ogre::Overlay* overlay, float radAngle)
{
	overlay->rotate(Ogre::Radian(radAngle));
}

extern "C" _AnomalousExport void Overlay_setScale(Ogre::Overlay* overlay, float x, float y)
{
	overlay->setScale(x, y);
}

extern "C" _AnomalousExport float Overlay_getScaleX(Ogre::Overlay* overlay)
{
	return overlay->getScaleX();
}

extern "C" _AnomalousExport float Overlay_getScaleY(Ogre::Overlay* overlay)
{
	return overlay->getScaleY();
}