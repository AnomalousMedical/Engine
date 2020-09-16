#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::OldBone* Bone_createChild(Ogre::v1::OldBone* bone, ushort handle, Vector3 translation, Quaternion rotate)
{
	return bone->createChild(handle, translation.toOgre(), rotate.toOgre());
}

extern "C" _AnomalousExport ushort Bone_getHandle(Ogre::v1::OldBone* bone)
{
	return bone->getHandle();
}

extern "C" _AnomalousExport void Bone_setBindingPose(Ogre::v1::OldBone* bone)
{
	bone->setBindingPose();
}

extern "C" _AnomalousExport void Bone_reset(Ogre::v1::OldBone* bone)
{
	bone->reset();
}

extern "C" _AnomalousExport void Bone_setManuallyControlled(Ogre::v1::OldBone* bone, bool manuallyControlled)
{
	bone->setManuallyControlled(manuallyControlled);
}

extern "C" _AnomalousExport bool Bone_isManuallyControlled(Ogre::v1::OldBone* bone)
{
	return bone->isManuallyControlled();
}

extern "C" _AnomalousExport void Bone_needUpdate(Ogre::v1::OldBone* bone, bool forceParentUpdate)
{
	bone->needUpdate(forceParentUpdate);
}

extern "C" _AnomalousExport String Bone_getName(Ogre::v1::OldBone* bone)
{
	return bone->getName().c_str();
}