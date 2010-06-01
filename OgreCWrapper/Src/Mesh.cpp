#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::VertexData* Mesh_getSharedVertexData(Ogre::Mesh* mesh)
{
	return mesh->sharedVertexData;
}

extern "C" __declspec(dllexport) Ogre::SubMesh* Mesh_createSubMesh(Ogre::Mesh* mesh)
{
	return mesh->createSubMesh();
}

extern "C" __declspec(dllexport) Ogre::SubMesh* Mesh_createSubMeshName(Ogre::Mesh* mesh, String name)
{
	return mesh->createSubMesh(name);
}

extern "C" __declspec(dllexport) void Mesh_nameSubMesh(Ogre::Mesh* mesh, String name, ushort index)
{
	mesh->nameSubMesh(name, index);
}

extern "C" __declspec(dllexport) ushort Mesh__getSubMeshIndex(Ogre::Mesh* mesh, String name)
{
	return mesh->_getSubMeshIndex(name);
}

extern "C" __declspec(dllexport) ushort Mesh_getNumSubMeshes(Ogre::Mesh* mesh)
{
	return mesh->getNumSubMeshes();
}

extern "C" __declspec(dllexport) Ogre::SubMesh* Mesh_getSubMesh(Ogre::Mesh* mesh, ushort index)
{
	return mesh->getSubMesh(index);
}

extern "C" __declspec(dllexport) Ogre::SubMesh* Mesh_getSubMeshName(Ogre::Mesh* mesh, String name)
{
	return mesh->getSubMesh(name);
}

extern "C" __declspec(dllexport) Ogre::Mesh* Mesh_clone(Ogre::Mesh* mesh, String newName, ProcessWrapperObjectDelegate processWrapper)
{
	Ogre::MeshPtr& ptr = mesh->clone(newName);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" __declspec(dllexport) Ogre::Mesh* Mesh_cloneChangeGroup(Ogre::Mesh* mesh, String newName, String newGroup, ProcessWrapperObjectDelegate processWrapper)
{
	Ogre::MeshPtr& ptr = mesh->clone(newName, newGroup);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" __declspec(dllexport) float Mesh_getBoundingSphereRadius(Ogre::Mesh* mesh)
{
	return mesh->getBoundingSphereRadius();
}

extern "C" __declspec(dllexport) void Mesh__setBoundingSphereRadius(Ogre::Mesh* mesh, float radius)
{
	mesh->_setBoundingSphereRadius(radius);
}

extern "C" __declspec(dllexport) void Mesh_setSkeletonName(Ogre::Mesh* mesh, String skeletonName)
{
	mesh->setSkeletonName(skeletonName);
}

extern "C" __declspec(dllexport) bool Mesh_hasSkeleton(Ogre::Mesh* mesh)
{
	return mesh->hasSkeleton();
}

extern "C" __declspec(dllexport) bool Mesh_hasVertexAnimation(Ogre::Mesh* mesh)
{
	return mesh->hasVertexAnimation();
}

extern "C" __declspec(dllexport) Ogre::Skeleton* Mesh_getSkeleton(Ogre::Mesh* mesh, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::SkeletonPtr& skeletonPtr = mesh->getSkeleton();
	processWrapper(skeletonPtr.getPointer(), &skeletonPtr);
	return skeletonPtr.getPointer();
}

extern "C" __declspec(dllexport) String Mesh_getSkeletonName(Ogre::Mesh* mesh)
{
	return mesh->getSkeletonName().c_str();
}

extern "C" __declspec(dllexport) void Mesh__initAnimationState(Ogre::Mesh* mesh, Ogre::AnimationStateSet* animSet)
{
	mesh->_initAnimationState(animSet);
}

extern "C" __declspec(dllexport) void Mesh__refreshAnimationState(Ogre::Mesh* mesh, Ogre::AnimationStateSet* animSet)
{
	mesh->_refreshAnimationState(animSet);
}

extern "C" __declspec(dllexport) void Mesh_clearBoneAssignments(Ogre::Mesh* mesh)
{
	mesh->clearBoneAssignments();
}

extern "C" __declspec(dllexport) ushort Mesh_getNumLodLevels(Ogre::Mesh* mesh)
{
	return mesh->getNumLodLevels();
}

extern "C" __declspec(dllexport) void Mesh_createManualLodLevel(Ogre::Mesh* mesh, float fromDepth, String meshName)
{
	mesh->createManualLodLevel(fromDepth, meshName);
}

extern "C" __declspec(dllexport) void Mesh_updateManualLodLevel(Ogre::Mesh* mesh, ushort index, String meshName)
{
	mesh->updateManualLodLevel(index, meshName);
}

extern "C" __declspec(dllexport) ushort Mesh_getLodIndex(Ogre::Mesh* mesh, float depth)
{
	return mesh->getLodIndex(depth);
}

extern "C" __declspec(dllexport) bool Mesh_isLodManual(Ogre::Mesh* mesh)
{
	return mesh->isLodManual();
}

extern "C" __declspec(dllexport) void Mesh_removeLodLevels(Ogre::Mesh* mesh)
{
	mesh->removeLodLevels();
}

extern "C" __declspec(dllexport) void Mesh_setVertexBufferPolicy(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage)
{
	mesh->setVertexBufferPolicy(usage);
}

extern "C" __declspec(dllexport) void Mesh_setVertexBufferPolicyShadow(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage, bool shadowBuffer)
{
	mesh->setVertexBufferPolicy(usage, shadowBuffer);
}

extern "C" __declspec(dllexport) void Mesh_setIndexBufferPolicy(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage)
{
	mesh->setIndexBufferPolicy(usage);
}

extern "C" __declspec(dllexport) void Mesh_setIndexBufferPolicyShadow(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage, bool shadowBuffer)
{
	mesh->setIndexBufferPolicy(usage, shadowBuffer);
}

extern "C" __declspec(dllexport) Ogre::HardwareBuffer::Usage Mesh_getVertexBufferUsage(Ogre::Mesh* mesh)
{
	return mesh->getVertexBufferUsage();
}

extern "C" __declspec(dllexport) Ogre::HardwareBuffer::Usage Mesh_getIndexBufferUsage(Ogre::Mesh* mesh)
{
	return mesh->getIndexBufferUsage();
}

extern "C" __declspec(dllexport) bool Mesh_isVertexBufferShadowed(Ogre::Mesh* mesh)
{
	return mesh->isVertexBufferShadowed();
}

extern "C" __declspec(dllexport) bool Mesh_isIndexBufferShadowed(Ogre::Mesh* mesh)
{
	return mesh->isIndexBufferShadowed();
}

extern "C" __declspec(dllexport) void Mesh_buildTangentVectors(Ogre::Mesh* mesh)
{
	mesh->buildTangentVectors();
}

extern "C" __declspec(dllexport) void Mesh_buildTangentVectors1(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic)
{
	mesh->buildTangentVectors(targetSemantic);
}

extern "C" __declspec(dllexport) void Mesh_buildTangentVectors2(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet);
}

extern "C" __declspec(dllexport) void Mesh_buildTangentVectors3(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index);
}

extern "C" __declspec(dllexport) void Mesh_buildTangentVectors4(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored);
}

extern "C" __declspec(dllexport) void Mesh_buildTangentVectors5(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated);
}

extern "C" __declspec(dllexport) void Mesh_buildTangentVectors6(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated, bool storeParityInW)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated, storeParityInW);
}

extern "C" __declspec(dllexport) bool Mesh_suggestTangentVectorBuildParams(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort* outSourceCoordSet, ushort* outIndex)
{
	return mesh->suggestTangentVectorBuildParams(targetSemantic, *outSourceCoordSet, *outIndex);
}

extern "C" __declspec(dllexport) void Mesh_buildEdgeList(Ogre::Mesh* mesh)
{
	mesh->buildEdgeList();
}

extern "C" __declspec(dllexport) void Mesh_freeEdgeList(Ogre::Mesh* mesh)
{
	mesh->freeEdgeList();
}

extern "C" __declspec(dllexport) void Mesh_prepareForShadowVolume(Ogre::Mesh* mesh)
{
	mesh->prepareForShadowVolume();
}

extern "C" __declspec(dllexport) bool Mesh_isPreparedForShadowVolumes(Ogre::Mesh* mesh)
{
	return mesh->isPreparedForShadowVolumes();
}

extern "C" __declspec(dllexport) bool Mesh_isEdgeListBuilt(Ogre::Mesh* mesh)
{
	return mesh->isEdgeListBuilt();
}

extern "C" __declspec(dllexport) uint Mesh_getSubMeshIndex(Ogre::Mesh* mesh, String name)
{
	return mesh->_getSubMeshIndex(name);
}

extern "C" __declspec(dllexport) void Mesh_setAutoBuildEdgeLists(Ogre::Mesh* mesh, bool autobuild)
{
	mesh->setAutoBuildEdgeLists(autobuild);
}

extern "C" __declspec(dllexport) bool Mesh_getAutoBuildEdgeLists(Ogre::Mesh* mesh)
{
	return mesh->getAutoBuildEdgeLists();
}

extern "C" __declspec(dllexport) Ogre::VertexAnimationType Mesh_getSharedVertexDataAnimationType(Ogre::Mesh* mesh)
{
	return mesh->getSharedVertexDataAnimationType();
}

extern "C" __declspec(dllexport) Ogre::Animation* Mesh_createAnimation(Ogre::Mesh* mesh, String name, float length)
{
	return mesh->createAnimation(name, length);
}

extern "C" __declspec(dllexport) Ogre::Animation* Mesh_getAnimation(Ogre::Mesh* mesh, String name)
{
	return mesh->getAnimation(name);
}

extern "C" __declspec(dllexport) bool Mesh_hasAnimation(Ogre::Mesh* mesh, String name)
{
	return mesh->hasAnimation(name);
}

extern "C" __declspec(dllexport) void Mesh_removeAnimation(Ogre::Mesh* mesh, String name)
{
	mesh->removeAnimation(name);
}

extern "C" __declspec(dllexport) ushort Mesh_getNumAnimations(Ogre::Mesh* mesh)
{
	return mesh->getNumAnimations();
}

extern "C" __declspec(dllexport) Ogre::Animation* Mesh_getAnimationIndex(Ogre::Mesh* mesh, ushort index)
{
	return mesh->getAnimation(index);
}

extern "C" __declspec(dllexport) void Mesh_removeAllAnimations(Ogre::Mesh* mesh)
{
	mesh->removeAllAnimations();
}

extern "C" __declspec(dllexport) void Mesh_updateMaterialForAllSubMeshes(Ogre::Mesh* mesh)
{
	mesh->updateMaterialForAllSubMeshes();
}

extern "C" __declspec(dllexport) Ogre::Pose* Mesh_createPose(Ogre::Mesh* mesh, ushort target)
{
	return mesh->createPose(target);
}

extern "C" __declspec(dllexport) Ogre::Pose* Mesh_createPoseName(Ogre::Mesh* mesh, ushort target, String name)
{
	return mesh->createPose(target, name);
}

extern "C" __declspec(dllexport) int Mesh_getPoseCount(Ogre::Mesh* mesh)
{
	return mesh->getPoseCount();
}

extern "C" __declspec(dllexport) Ogre::Pose* Mesh_getPose(Ogre::Mesh* mesh, ushort index)
{
	return mesh->getPose(index);
}

extern "C" __declspec(dllexport) Ogre::Pose* Mesh_getPoseName(Ogre::Mesh* mesh, String name)
{
	return mesh->getPose(name);
}

extern "C" __declspec(dllexport) void Mesh_removePose(Ogre::Mesh* mesh, ushort index)
{
	mesh->removePose(index);
}

extern "C" __declspec(dllexport) void Mesh_removePoseName(Ogre::Mesh* mesh, String name)
{
	mesh->removePose(name);
}

extern "C" __declspec(dllexport) void Mesh_removeAllPoses(Ogre::Mesh* mesh)
{
	mesh->removeAllPoses();
}