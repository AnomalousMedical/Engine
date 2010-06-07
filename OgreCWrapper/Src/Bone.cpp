#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::Bone* Bone_createChild(Ogre::Bone* bone, ushort handle, Vector3 translation, Quaternion rotate)
{
	return bone->createChild(handle, translation.toOgre(), rotate.toOgre());
}

extern "C" _AnomalousExport ushort Bone_getHandle(Ogre::Bone* bone)
{
	return bone->getHandle();
}

extern "C" _AnomalousExport void Bone_setBindingPose(Ogre::Bone* bone)
{
	bone->setBindingPose();
}

extern "C" _AnomalousExport void Bone_reset(Ogre::Bone* bone)
{
	bone->reset();
}

extern "C" _AnomalousExport void Bone_setManuallyControlled(Ogre::Bone* bone, bool manuallyControlled)
{
	bone->setManuallyControlled(manuallyControlled);
}

extern "C" _AnomalousExport bool Bone_isManuallyControlled(Ogre::Bone* bone)
{
	return bone->isManuallyControlled();
}

extern "C" _AnomalousExport void Bone_needUpdate(Ogre::Bone* bone, bool forceParentUpdate)
{
	bone->needUpdate(forceParentUpdate);
}

extern "C" _AnomalousExport String Bone_getName(Ogre::Bone* bone)
{
	return bone->getName().c_str();
}