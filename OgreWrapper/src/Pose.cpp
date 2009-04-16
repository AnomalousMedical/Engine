#include "stdafx.h"
#include "Pose.h"
#include "OgrePose.h"
#include "MarshalUtils.h"
#include "MathUtils.h"

namespace OgreWrapper
{

Pose::Pose(Ogre::Pose* pose)
:pose(pose)
{

}

Ogre::Pose* Pose::getOgrePose()
{
	return pose;
}

System::String^ Pose::getName()
{
	return MarshalUtils::convertString(pose->getName());
}

unsigned short Pose::getTarget()
{
	return pose->getTarget();
}

void Pose::addVertex(size_t index, EngineMath::Vector3 offset)
{
	return pose->addVertex(index, MathUtils::copyVector3(offset));
}

void Pose::addVertex(size_t index, EngineMath::Vector3% offset)
{
	return pose->addVertex(index, MathUtils::copyVector3(offset));
}

void Pose::removeVertex(size_t index)
{
	return pose->removeVertex(index);
}

void Pose::clearVertexOffsets()
{
	return pose->clearVertexOffsets();
}

EngineMath::Vector3 Pose::getOffset(size_t index)
{
	Ogre::Pose::VertexOffsetMap map = pose->getVertexOffsets();
	return MathUtils::copyVector3(map[index]);
}

}