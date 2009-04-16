#include "StdAfx.h"
#include "..\include\RenderNode.h"
#include "MathUtils.h"

namespace Engine
{

namespace Rendering
{

RenderNode::RenderNode(Ogre::Node* ogreNode)
:ogreNode(ogreNode)
{

}

Ogre::Node* RenderNode::getOgreNode()
{
	return ogreNode;
}

EngineMath::Quaternion RenderNode::getOrientation()
{
	return MathUtils::copyQuaternion(ogreNode->getOrientation());
}

void RenderNode::setOrientation(EngineMath::Quaternion q)
{
	ogreNode->setOrientation(MathUtils::copyQuaternion(q));
}

void RenderNode::setOrientation(float x, float y, float z, float w)
{
	ogreNode->setOrientation(w, x, y, z);
}

void RenderNode::resetOrientation()
{
	ogreNode->resetOrientation();
}

void RenderNode::setPosition(EngineMath::Vector3 pos)
{
	ogreNode->setPosition(MathUtils::copyVector3(pos));
}

EngineMath::Vector3 RenderNode::getPosition()
{
	return MathUtils::copyVector3(ogreNode->getPosition());
}

void RenderNode::setScale(EngineMath::Vector3 scale)
{
	ogreNode->setScale(MathUtils::copyVector3(scale));
}

EngineMath::Vector3 RenderNode::getScale()
{
	return MathUtils::copyVector3(ogreNode->getScale());
}

void RenderNode::setInheritOrientation(bool inherit)
{
	ogreNode->setInheritOrientation(inherit);
}

bool RenderNode::getInheritOrientation()
{
	return ogreNode->getInheritOrientation();
}

void RenderNode::setInheritScale(bool inherit)
{
	ogreNode->setInheritScale(inherit);
}

bool RenderNode::getInheritScale()
{
	return ogreNode->getInheritScale();
}

void RenderNode::scale(EngineMath::Vector3 scale)
{
	ogreNode->scale(MathUtils::copyVector3(scale));
}

void RenderNode::scale(float x, float y, float z)
{
	ogreNode->scale(x, y, z);
}

void RenderNode::translate(EngineMath::Vector3 d, TransformSpace relativeTo)
{
	ogreNode->translate(MathUtils::copyVector3(d), (Ogre::Node::TransformSpace)relativeTo);
}

void RenderNode::translate(float x, float y, float z, TransformSpace relativeTo)
{
	ogreNode->translate(x, y, z, (Ogre::Node::TransformSpace)relativeTo);
}

void RenderNode::roll(float angle, TransformSpace relativeTo)
{
	ogreNode->roll(Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void RenderNode::pitch(float angle, TransformSpace relativeTo)
{
	ogreNode->pitch(Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void RenderNode::yaw(float angle, TransformSpace relativeTo)
{
	ogreNode->yaw(Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void RenderNode::rotate(EngineMath::Vector3 axis, float angle, TransformSpace relativeTo)
{
	ogreNode->rotate(MathUtils::copyVector3(axis), Ogre::Radian(angle), (Ogre::Node::TransformSpace)relativeTo);
}

void RenderNode::rotate(EngineMath::Quaternion q, TransformSpace relativeTo)
{
	ogreNode->rotate(MathUtils::copyQuaternion(q), (Ogre::Node::TransformSpace)relativeTo);
}

EngineMath::Vector3 RenderNode::getDerivedPosition()
{
	return MathUtils::copyVector3(ogreNode->_getDerivedPosition());
}

EngineMath::Vector3 RenderNode::getDerivedScale()
{
	return MathUtils::copyVector3(ogreNode->_getDerivedScale());
}

EngineMath::Quaternion RenderNode::getDerivedOrientation()
{
	return MathUtils::copyQuaternion(ogreNode->_getDerivedOrientation());
}

}

}