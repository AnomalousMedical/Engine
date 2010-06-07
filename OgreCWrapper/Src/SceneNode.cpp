#include "Stdafx.h"

extern "C" _AnomalousExport void SceneNode_addChild(Ogre::SceneNode* sceneNode, Ogre::SceneNode* child)
{
	sceneNode->addChild(child);
}

extern "C" _AnomalousExport void SceneNode_removeChild(Ogre::SceneNode* sceneNode, Ogre::SceneNode* child)
{
	sceneNode->removeChild(child);
}

extern "C" _AnomalousExport void SceneNode_attachObject(Ogre::SceneNode* sceneNode, Ogre::MovableObject* obj)
{
	sceneNode->attachObject(obj);
}

extern "C" _AnomalousExport void SceneNode_detachObject(Ogre::SceneNode* sceneNode, Ogre::MovableObject* obj)
{
	sceneNode->detachObject(obj);
}

extern "C" _AnomalousExport void SceneNode_setAutoTracking(Ogre::SceneNode* sceneNode, bool enabled, Ogre::SceneNode* target, Vector3 offset)
{
	sceneNode->setAutoTracking(enabled, target, offset.toOgre());
}

extern "C" _AnomalousExport void SceneNode_setVisible(Ogre::SceneNode* sceneNode, bool visible)
{
	sceneNode->setVisible(visible);
}

extern "C" _AnomalousExport void SceneNode_setVisibleCascade(Ogre::SceneNode* sceneNode, bool visible, bool cascade)
{
	sceneNode->setVisible(visible, cascade);
}

extern "C" _AnomalousExport void SceneNode_lookAt(Ogre::SceneNode* sceneNode, Vector3 targetPoint, Ogre::Node::TransformSpace relativeTo)
{
	sceneNode->lookAt(targetPoint.toOgre(), relativeTo);
}

extern "C" _AnomalousExport void SceneNode_lookAtLocalDirection(Ogre::SceneNode* sceneNode, Vector3 targetPoint, Ogre::Node::TransformSpace relativeTo, Vector3 localDirectionVector)
{
	sceneNode->lookAt(targetPoint.toOgre(), relativeTo, localDirectionVector.toOgre());
}

extern "C" _AnomalousExport void SceneNode_setDebugDisplayEnabled(Ogre::SceneNode* sceneNode, bool enabled, bool cascade)
{
	sceneNode->setDebugDisplayEnabled(enabled, cascade);
}

extern "C" _AnomalousExport void SceneNode_showBoundingBox(Ogre::SceneNode* sceneNode, bool show)
{
	sceneNode->showBoundingBox(show);
}