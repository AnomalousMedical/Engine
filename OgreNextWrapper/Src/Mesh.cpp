#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::VertexData* Mesh_getSharedVertexData(Ogre::v1::Mesh* mesh)
{
	return mesh->sharedVertexData;
}

extern "C" _AnomalousExport Ogre::v1::SubMesh* Mesh_createSubMesh(Ogre::v1::Mesh* mesh)
{
	return mesh->createSubMesh();
}

extern "C" _AnomalousExport Ogre::v1::SubMesh* Mesh_createSubMeshName(Ogre::v1::Mesh* mesh, String name)
{
	return mesh->createSubMesh(name);
}

extern "C" _AnomalousExport void Mesh_nameSubMesh(Ogre::v1::Mesh* mesh, String name, ushort index)
{
	mesh->nameSubMesh(name, index);
}

extern "C" _AnomalousExport ushort Mesh__getSubMeshIndex(Ogre::v1::Mesh* mesh, String name)
{
	return mesh->_getSubMeshIndex(name);
}

extern "C" _AnomalousExport ushort Mesh_getNumSubMeshes(Ogre::v1::Mesh* mesh)
{
	return mesh->getNumSubMeshes();
}

extern "C" _AnomalousExport Ogre::v1::SubMesh* Mesh_getSubMesh(Ogre::v1::Mesh* mesh, ushort index)
{
	return mesh->getSubMesh(index);
}

extern "C" _AnomalousExport Ogre::v1::SubMesh* Mesh_getSubMeshName(Ogre::v1::Mesh* mesh, String name)
{
	return mesh->getSubMesh(name);
}

extern "C" _AnomalousExport Ogre::v1::Mesh* Mesh_clone(Ogre::v1::Mesh* mesh, String newName, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::v1::MeshPtr& ptr = mesh->clone(newName);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::v1::Mesh* Mesh_cloneChangeGroup(Ogre::v1::Mesh* mesh, String newName, String newGroup, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::v1::MeshPtr& ptr = mesh->clone(newName, newGroup);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport float Mesh_getBoundingSphereRadius(Ogre::v1::Mesh* mesh)
{
	return mesh->getBoundingSphereRadius();
}

extern "C" _AnomalousExport void Mesh__setBoundingSphereRadius(Ogre::v1::Mesh* mesh, float radius)
{
	mesh->_setBoundingSphereRadius(radius);
}

extern "C" _AnomalousExport void Mesh_setSkeletonName(Ogre::v1::Mesh* mesh, String skeletonName)
{
	mesh->setSkeletonName(skeletonName);
}

extern "C" _AnomalousExport bool Mesh_hasSkeleton(Ogre::v1::Mesh* mesh)
{
	return mesh->hasSkeleton();
}

extern "C" _AnomalousExport bool Mesh_hasVertexAnimation(Ogre::v1::Mesh* mesh)
{
	return mesh->hasVertexAnimation();
}

extern "C" _AnomalousExport Ogre::v1::Skeleton* Mesh_getSkeleton(Ogre::v1::Mesh* mesh, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::v1::SkeletonPtr& skeletonPtr = mesh->getSkeleton();
	processWrapper(skeletonPtr.getPointer(), &skeletonPtr);
	return skeletonPtr.getPointer();
}

extern "C" _AnomalousExport String Mesh_getSkeletonName(Ogre::v1::Mesh* mesh)
{
	return mesh->getSkeletonName().c_str();
}

extern "C" _AnomalousExport void Mesh__initAnimationState(Ogre::v1::Mesh* mesh, Ogre::v1::AnimationStateSet* animSet)
{
	mesh->_initAnimationState(animSet);
}

extern "C" _AnomalousExport void Mesh__refreshAnimationState(Ogre::v1::Mesh* mesh, Ogre::v1::AnimationStateSet* animSet)
{
	mesh->_refreshAnimationState(animSet);
}

extern "C" _AnomalousExport void Mesh_clearBoneAssignments(Ogre::v1::Mesh* mesh)
{
	mesh->clearBoneAssignments();
}

extern "C" _AnomalousExport ushort Mesh_getNumLodLevels(Ogre::v1::Mesh* mesh)
{
	return mesh->getNumLodLevels();
}

extern "C" _AnomalousExport void Mesh_updateManualLodLevel(Ogre::v1::Mesh* mesh, ushort index, String meshName)
{
	mesh->updateManualLodLevel(index, meshName);
}

extern "C" _AnomalousExport ushort Mesh_getLodIndex(Ogre::v1::Mesh* mesh, float depth)
{
	return mesh->getLodIndex(depth);
}

extern "C" _AnomalousExport void Mesh_removeLodLevels(Ogre::v1::Mesh* mesh)
{
	mesh->removeLodLevels();
}

extern "C" _AnomalousExport void Mesh_setVertexBufferPolicy(Ogre::v1::Mesh* mesh, Ogre::v1::HardwareBuffer::Usage usage)
{
	mesh->setVertexBufferPolicy(usage);
}

extern "C" _AnomalousExport void Mesh_setVertexBufferPolicyShadow(Ogre::v1::Mesh* mesh, Ogre::v1::HardwareBuffer::Usage usage, bool shadowBuffer)
{
	mesh->setVertexBufferPolicy(usage, shadowBuffer);
}

extern "C" _AnomalousExport void Mesh_setIndexBufferPolicy(Ogre::v1::Mesh* mesh, Ogre::v1::HardwareBuffer::Usage usage)
{
	mesh->setIndexBufferPolicy(usage);
}

extern "C" _AnomalousExport void Mesh_setIndexBufferPolicyShadow(Ogre::v1::Mesh* mesh, Ogre::v1::HardwareBuffer::Usage usage, bool shadowBuffer)
{
	mesh->setIndexBufferPolicy(usage, shadowBuffer);
}

extern "C" _AnomalousExport Ogre::v1::HardwareBuffer::Usage Mesh_getVertexBufferUsage(Ogre::v1::Mesh* mesh)
{
	return mesh->getVertexBufferUsage();
}

extern "C" _AnomalousExport Ogre::v1::HardwareBuffer::Usage Mesh_getIndexBufferUsage(Ogre::v1::Mesh* mesh)
{
	return mesh->getIndexBufferUsage();
}

extern "C" _AnomalousExport bool Mesh_isVertexBufferShadowed(Ogre::v1::Mesh* mesh)
{
	return mesh->isVertexBufferShadowed();
}

extern "C" _AnomalousExport bool Mesh_isIndexBufferShadowed(Ogre::v1::Mesh* mesh)
{
	return mesh->isIndexBufferShadowed();
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors(Ogre::v1::Mesh* mesh)
{
	mesh->buildTangentVectors();
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors1(Ogre::v1::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic)
{
	mesh->buildTangentVectors(targetSemantic);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors2(Ogre::v1::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors3(Ogre::v1::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors4(Ogre::v1::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors5(Ogre::v1::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors6(Ogre::v1::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated, bool storeParityInW)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated, storeParityInW);
}

extern "C" _AnomalousExport bool Mesh_suggestTangentVectorBuildParams(Ogre::v1::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort* outSourceCoordSet, ushort* outIndex)
{
	return mesh->suggestTangentVectorBuildParams(targetSemantic, *outSourceCoordSet, *outIndex);
}

extern "C" _AnomalousExport void Mesh_buildEdgeList(Ogre::v1::Mesh* mesh)
{
	mesh->buildEdgeList();
}

extern "C" _AnomalousExport void Mesh_freeEdgeList(Ogre::v1::Mesh* mesh)
{
	mesh->freeEdgeList();
}

extern "C" _AnomalousExport void Mesh_prepareForShadowVolume(Ogre::v1::Mesh* mesh)
{
	mesh->prepareForShadowVolume();
}

extern "C" _AnomalousExport bool Mesh_isPreparedForShadowVolumes(Ogre::v1::Mesh* mesh)
{
	return mesh->isPreparedForShadowVolumes();
}

extern "C" _AnomalousExport bool Mesh_isEdgeListBuilt(Ogre::v1::Mesh* mesh)
{
	return mesh->isEdgeListBuilt();
}

extern "C" _AnomalousExport uint Mesh_getSubMeshIndex(Ogre::v1::Mesh* mesh, String name)
{
	return mesh->_getSubMeshIndex(name);
}

extern "C" _AnomalousExport void Mesh_setAutoBuildEdgeLists(Ogre::v1::Mesh* mesh, bool autobuild)
{
	mesh->setAutoBuildEdgeLists(autobuild);
}

extern "C" _AnomalousExport bool Mesh_getAutoBuildEdgeLists(Ogre::v1::Mesh* mesh)
{
	return mesh->getAutoBuildEdgeLists();
}

extern "C" _AnomalousExport Ogre::v1::VertexAnimationType Mesh_getSharedVertexDataAnimationType(Ogre::v1::Mesh* mesh)
{
	return mesh->getSharedVertexDataAnimationType();
}

extern "C" _AnomalousExport Ogre::v1::Animation* Mesh_createAnimation(Ogre::v1::Mesh* mesh, String name, float length)
{
	return mesh->createAnimation(name, length);
}

extern "C" _AnomalousExport Ogre::v1::Animation* Mesh_getAnimation(Ogre::v1::Mesh* mesh, String name)
{
	return mesh->getAnimation(name);
}

extern "C" _AnomalousExport bool Mesh_hasAnimation(Ogre::v1::Mesh* mesh, String name)
{
	return mesh->hasAnimation(name);
}

extern "C" _AnomalousExport void Mesh_removeAnimation(Ogre::v1::Mesh* mesh, String name)
{
	mesh->removeAnimation(name);
}

extern "C" _AnomalousExport ushort Mesh_getNumAnimations(Ogre::v1::Mesh* mesh)
{
	return mesh->getNumAnimations();
}

extern "C" _AnomalousExport Ogre::v1::Animation* Mesh_getAnimationIndex(Ogre::v1::Mesh* mesh, ushort index)
{
	return mesh->getAnimation(index);
}

extern "C" _AnomalousExport void Mesh_removeAllAnimations(Ogre::v1::Mesh* mesh)
{
	mesh->removeAllAnimations();
}

extern "C" _AnomalousExport void Mesh_updateMaterialForAllSubMeshes(Ogre::v1::Mesh* mesh)
{
	mesh->updateMaterialForAllSubMeshes();
}

extern "C" _AnomalousExport Ogre::v1::Pose* Mesh_createPose(Ogre::v1::Mesh* mesh, ushort target)
{
	return mesh->createPose(target);
}

extern "C" _AnomalousExport Ogre::v1::Pose* Mesh_createPoseName(Ogre::v1::Mesh* mesh, ushort target, String name)
{
	return mesh->createPose(target, name);
}

extern "C" _AnomalousExport int Mesh_getPoseCount(Ogre::v1::Mesh* mesh)
{
	return mesh->getPoseCount();
}

extern "C" _AnomalousExport Ogre::v1::Pose* Mesh_getPose(Ogre::v1::Mesh* mesh, ushort index)
{
	return mesh->getPose(index);
}

extern "C" _AnomalousExport Ogre::v1::Pose* Mesh_getPoseName(Ogre::v1::Mesh* mesh, String name)
{
	return mesh->getPose(name);
}

extern "C" _AnomalousExport void Mesh_removePose(Ogre::v1::Mesh* mesh, ushort index)
{
	mesh->removePose(index);
}

extern "C" _AnomalousExport void Mesh_removePoseName(Ogre::v1::Mesh* mesh, String name)
{
	mesh->removePose(name);
}

extern "C" _AnomalousExport void Mesh_removeAllPoses(Ogre::v1::Mesh* mesh)
{
	mesh->removeAllPoses();
}

extern "C" _AnomalousExport void Mesh__updateCompiledBoneAssignments(Ogre::v1::Mesh* mesh)
{
	mesh->_updateCompiledBoneAssignments();
}