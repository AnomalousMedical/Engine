#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport Quaternion Node_getOrientation(Ogre::Node* ogreNode)
{
	return ogreNode->getOrientation();
}

extern "C" _AnomalousExport void Node_setOrientation(Ogre::Node* ogreNode, Quaternion q)
{
	ogreNode->setOrientation(q.toOgre());
}

extern "C" _AnomalousExport void Node_setOrientationRaw(Ogre::Node* ogreNode, float x, float y, float z, float w)
{
	ogreNode->setOrientation(w, x, y, z);
}

extern "C" _AnomalousExport void Node_resetOrientation(Ogre::Node* ogreNode)
{
	ogreNode->resetOrientation();
}

extern "C" _AnomalousExport void Node_setPosition(Ogre::Node* ogreNode, Vector3 pos)
{
	ogreNode->setPosition(pos.toOgre());
}

extern "C" _AnomalousExport Vector3 Node_getPosition(Ogre::Node* ogreNode)
{
	return ogreNode->getPosition();
}

extern "C" _AnomalousExport void Node_setScale(Ogre::Node* ogreNode, Vector3 scale)
{
	ogreNode->setScale(scale.toOgre());
}

extern "C" _AnomalousExport Vector3 Node_getScale(Ogre::Node* ogreNode)
{
	return ogreNode->getScale();
}

extern "C" _AnomalousExport void Node_setInheritOrientation(Ogre::Node* ogreNode, bool inherit)
{
	ogreNode->setInheritOrientation(inherit);
}

extern "C" _AnomalousExport bool Node_getInheritOrientation(Ogre::Node* ogreNode)
{
	return ogreNode->getInheritOrientation();
}

extern "C" _AnomalousExport void Node_setInheritScale(Ogre::Node* ogreNode, bool inherit)
{
	ogreNode->setInheritScale(inherit);
}

extern "C" _AnomalousExport bool Node_getInheritScale(Ogre::Node* ogreNode)
{
	return ogreNode->getInheritScale();
}

extern "C" _AnomalousExport void Node_scale(Ogre::Node* ogreNode, Vector3 scale)
{
	ogreNode->scale(scale.toOgre());
}

extern "C" _AnomalousExport void Node_scaleRaw(Ogre::Node* ogreNode, float x, float y, float z)
{
	ogreNode->scale(x, y, z);
}

extern "C" _AnomalousExport void Node_translate(Ogre::Node* ogreNode, Vector3 d, Ogre::Node::TransformSpace relativeTo)
{
	ogreNode->translate(d.toOgre(), relativeTo);
}

extern "C" _AnomalousExport void Node_translateRaw(Ogre::Node* ogreNode, float x, float y, float z, Ogre::Node::TransformSpace relativeTo)
{
	ogreNode->translate(x, y, z, relativeTo);
}

extern "C" _AnomalousExport void Node_roll(Ogre::Node* ogreNode, float angle, Ogre::Node::TransformSpace relativeTo)
{
	ogreNode->roll(Ogre::Radian(angle), relativeTo);
}

extern "C" _AnomalousExport void Node_pitch(Ogre::Node* ogreNode, float angle, Ogre::Node::TransformSpace relativeTo)
{
	ogreNode->pitch(Ogre::Radian(angle), relativeTo);
}

extern "C" _AnomalousExport void Node_yaw(Ogre::Node* ogreNode, float angle, Ogre::Node::TransformSpace relativeTo)
{
	ogreNode->yaw(Ogre::Radian(angle), relativeTo);
}

extern "C" _AnomalousExport void Node_rotateAxis(Ogre::Node* ogreNode, Vector3 axis, float angle, Ogre::Node::TransformSpace relativeTo)
{
	ogreNode->rotate(axis.toOgre(), Ogre::Radian(angle), relativeTo);
}

extern "C" _AnomalousExport void Node_rotate(Ogre::Node* ogreNode, Quaternion q, Ogre::Node::TransformSpace relativeTo)
{
	ogreNode->rotate(q.toOgre(), relativeTo);
}

extern "C" _AnomalousExport Vector3 Node_getDerivedPosition(Ogre::Node* ogreNode)
{
	return ogreNode->_getDerivedPosition();
}

extern "C" _AnomalousExport Vector3 Node_getDerivedScale(Ogre::Node* ogreNode)
{
	return ogreNode->_getDerivedScale();
}

extern "C" _AnomalousExport Quaternion Node_getDerivedOrientation(Ogre::Node* ogreNode)
{
	return ogreNode->_getDerivedOrientation();
}

#pragma warning(pop)