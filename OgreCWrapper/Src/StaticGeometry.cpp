#include "Stdafx.h"

extern "C" _AnomalousExport void StaticGeometry_addEntity(Ogre::StaticGeometry* staticGeometry, Ogre::Entity* ent, Vector3 position, Quaternion orientation, Vector3 scale)
{
	staticGeometry->addEntity(ent, position.toOgre(), orientation.toOgre(), scale.toOgre());
}

extern "C" _AnomalousExport void StaticGeometry_addSceneNode(Ogre::StaticGeometry* staticGeometry, Ogre::SceneNode* node)
{
	staticGeometry->addSceneNode(node);
}

extern "C" _AnomalousExport void StaticGeometry_build(Ogre::StaticGeometry* staticGeometry)
{
	staticGeometry->build();
}

extern "C" _AnomalousExport void StaticGeometry_destroy(Ogre::StaticGeometry* staticGeometry)
{
	staticGeometry->destroy();
}

extern "C" _AnomalousExport void StaticGeometry_dump(Ogre::StaticGeometry* staticGeometry, String filename)
{
	staticGeometry->dump(filename);
}

extern "C" _AnomalousExport Vector3 StaticGeometry_getOrigin(Ogre::StaticGeometry* staticGeometry)
{
	return staticGeometry->getOrigin();
}

extern "C" _AnomalousExport Vector3 StaticGeometry_getRegionDimensions(Ogre::StaticGeometry* staticGeometry)
{
	return staticGeometry->getRegionDimensions();
}

extern "C" _AnomalousExport bool StaticGeometry_isVisible(Ogre::StaticGeometry* staticGeometry)
{
	return staticGeometry->isVisible();
}

extern "C" _AnomalousExport void StaticGeometry_reset(Ogre::StaticGeometry* staticGeometry)
{
	staticGeometry->reset();
}

extern "C" _AnomalousExport void StaticGeometry_setOrigin(Ogre::StaticGeometry* staticGeometry, Vector3 origin)
{
	staticGeometry->setOrigin(origin.toOgre());
}

extern "C" _AnomalousExport void StaticGeometry_setRegionDimensions(Ogre::StaticGeometry* staticGeometry, Vector3 size)
{
	staticGeometry->setRegionDimensions(size.toOgre());
}

extern "C" _AnomalousExport void StaticGeometry_setVisible(Ogre::StaticGeometry* staticGeometry, bool visible)
{
	staticGeometry->setVisible(visible);
}