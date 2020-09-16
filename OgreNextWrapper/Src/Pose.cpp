#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport String Pose_getName(Ogre::v1::Pose* pose)
{
	return pose->getName().c_str();
}

extern "C" _AnomalousExport ushort Pose_getTarget(Ogre::v1::Pose* pose)
{
	return pose->getTarget();
}

extern "C" _AnomalousExport void Pose_addVertex(Ogre::v1::Pose* pose, size_t index, Vector3 offset)
{
	pose->addVertex(index, offset.toOgre());
}

extern "C" _AnomalousExport void Pose_removeVertex(Ogre::v1::Pose* pose, size_t index)
{
	pose->removeVertex(index);
}

extern "C" _AnomalousExport void Pose_clearVertices(Ogre::v1::Pose* pose)
{
	pose->clearVertices();
}

extern "C" _AnomalousExport Vector3 Pose_getOffset(Ogre::v1::Pose* pose, size_t index)
{
	Ogre::v1::Pose::VertexOffsetMap map = pose->getVertexOffsets();
	return map[index];
}

#pragma warning(pop)