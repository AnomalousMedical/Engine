#include "StdAfx.h"
#include "..\include\Node.h"
#include "MathUtils.h"

namespace OgreWrapper
{

Node::Node(Ogre::Node* ogreNode)
:ogreNode(ogreNode)
{

}

Ogre::Node* Node::getOgreNode()
{
	return ogreNode;
}

EngineMath::Quaternion Node::getOrientation()
{
	return MathUtils::copyQuaternion(ogreNode->getOrientation());
}

void Node::setOrientation(EngineMath::Quaternion q)
{
	ogreNode->setOrientation(MathUtils::copyQuaternion(q));
}

void Node::setOrientation(float x, float y, float z, float w)
{
	ogreNode->setOrientation(w, x, y, z);
}

void Node::resetOrientation()
{
	ogreNode->resetOrientation();
}

void Node::setPosition(EngineMath::Vector3 pos)
{
	ogreNode->setPosition(MathUtils::copyVector3(pos));
}

EngineMath::Vector3 Node::getPosition()
{
	return MathUtils::copyVector3(ogreNode->getPosition());
}

void Node::setScale(EngineMath::Vector3 scale)
{
	ogreNode->setScale(MathUtils::copyVector3(scale));
}

EngineMath::Vector3 Node::getScale()
{
	return MathUtils::copyVector3(ogreNode->getScale());
}

void Node::setInheritOrientation(bool inherit)
{
	ogreNode->setInheritOrientation(inherit);
}

bool Node::getInheritOrientation()
{
	return ogreNode->getInheritOrientation();
}

void Node::setInheritScale(bool inherit)
{
	ogreNode->setInheritScale(inherit);
}

bool Node::getInheritScale()
{
	return ogreNode->getInheritScale();
}

void Node::scale(EngineMath::Vector3 scale)
{
	ogreNode->scale(MathUtils::copyVector3(scale));
}

void Node::scale(float x, float y, float z)
{
	ogreNode->scale(x, y, z);
}

void Node::translate(EngineMath::Vector3 d, TransformSpace relativeTo)
{
	ogreNode->translate(MathUtils::copyVector3(d), (Ogre::Node::TransformSpace)relativeTo);
}

void Node::translate(float x, float y, float z, TransformSpace relativeTo)
{
	ogreNode->translate(x, y, z, (Ogre::Node::TransformSpace)relativeTo);
}

void Node::roll(float angle, TransformSpace relativeTo)
{
	ogreNode->roll(Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void Node::pitch(float angle, TransformSpace relativeTo)
{
	ogreNode->pitch(Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void Node::yaw(float angle, TransformSpace relativeTo)
{
	ogreNode->yaw(Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void Node::rotate(EngineMath::Vector3 axis, float angle, TransformSpace relativeTo)
{
	ogreNode->rotate(MathUtils::copyVector3(axis), Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void Node::rotate(EngineMath::Quaternion q, TransformSpace relativeTo)
{
	ogreNode->rotate(MathUtils::copyQuaternion(q), (Ogre::Node::TransformSpace)relativeTo);
}

EngineMath::Vector3 Node::getDerivedPosition()
{
	return MathUtils::copyVector3(ogreNode->_getDerivedPosition());
}

EngineMath::Vector3 Node::getDerivedScale()
{
	return MathUtils::copyVector3(ogreNode->_getDerivedScale());
}

EngineMath::Quaternion Node::getDerivedOrientation()
{
	return MathUtils::copyQuaternion(ogreNode->_getDerivedOrientation());
}

}