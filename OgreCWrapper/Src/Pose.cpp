#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" __declspec(dllexport) String Pose_getName(Ogre::Pose* pose)
{
	return pose->getName().c_str();
}

extern "C" __declspec(dllexport) ushort Pose_getTarget(Ogre::Pose* pose)
{
	return pose->getTarget();
}

extern "C" __declspec(dllexport) void Pose_addVertex(Ogre::Pose* pose, size_t index, Vector3 offset)
{
	pose->addVertex(index, offset.toOgre());
}

extern "C" __declspec(dllexport) void Pose_removeVertex(Ogre::Pose* pose, size_t index)
{
	pose->removeVertex(index);
}

extern "C" __declspec(dllexport) void Pose_clearVertexOffsets(Ogre::Pose* pose)
{
	pose->clearVertexOffsets();
}

extern "C" __declspec(dllexport) Vector3 Pose_getOffset(Ogre::Pose* pose, size_t index)
{
	Ogre::Pose::VertexOffsetMap map = pose->getVertexOffsets();
	return map[index];
}

#pragma warning(pop)