#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::Bone* Bone_createChild(Ogre::Bone* bone, ushort handle, Vector3 translation, Quaternion rotate)
{
	return bone->createChild(handle, translation.toOgre(), rotate.toOgre());
}

extern "C" __declspec(dllexport) ushort Bone_getHandle(Ogre::Bone* bone)
{
	return bone->getHandle();
}

extern "C" __declspec(dllexport) void Bone_setBindingPose(Ogre::Bone* bone)
{
	bone->setBindingPose();
}

extern "C" __declspec(dllexport) void Bone_reset(Ogre::Bone* bone)
{
	bone->reset();
}

extern "C" __declspec(dllexport) void Bone_setManuallyControlled(Ogre::Bone* bone, bool manuallyControlled)
{
	bone->setManuallyControlled(manuallyControlled);
}

extern "C" __declspec(dllexport) bool Bone_isManuallyControlled(Ogre::Bone* bone)
{
	return bone->isManuallyControlled();
}

extern "C" __declspec(dllexport) void Bone_needUpdate(Ogre::Bone* bone, bool forceParentUpdate)
{
	bone->needUpdate(forceParentUpdate);
}

extern "C" __declspec(dllexport) String Bone_getName(Ogre::Bone* bone)
{
	return bone->getName().c_str();
}