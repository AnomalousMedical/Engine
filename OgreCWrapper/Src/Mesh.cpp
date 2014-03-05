#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::VertexData* Mesh_getSharedVertexData(Ogre::Mesh* mesh)
{
	return mesh->sharedVertexData;
}

extern "C" _AnomalousExport Ogre::SubMesh* Mesh_createSubMesh(Ogre::Mesh* mesh)
{
	return mesh->createSubMesh();
}

extern "C" _AnomalousExport Ogre::SubMesh* Mesh_createSubMeshName(Ogre::Mesh* mesh, String name)
{
	return mesh->createSubMesh(name);
}

extern "C" _AnomalousExport void Mesh_nameSubMesh(Ogre::Mesh* mesh, String name, ushort index)
{
	mesh->nameSubMesh(name, index);
}

extern "C" _AnomalousExport ushort Mesh__getSubMeshIndex(Ogre::Mesh* mesh, String name)
{
	return mesh->_getSubMeshIndex(name);
}

extern "C" _AnomalousExport ushort Mesh_getNumSubMeshes(Ogre::Mesh* mesh)
{
	return mesh->getNumSubMeshes();
}

extern "C" _AnomalousExport Ogre::SubMesh* Mesh_getSubMesh(Ogre::Mesh* mesh, ushort index)
{
	return mesh->getSubMesh(index);
}

extern "C" _AnomalousExport Ogre::SubMesh* Mesh_getSubMeshName(Ogre::Mesh* mesh, String name)
{
	return mesh->getSubMesh(name);
}

extern "C" _AnomalousExport Ogre::Mesh* Mesh_clone(Ogre::Mesh* mesh, String newName, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::MeshPtr& ptr = mesh->clone(newName);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::Mesh* Mesh_cloneChangeGroup(Ogre::Mesh* mesh, String newName, String newGroup, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::MeshPtr& ptr = mesh->clone(newName, newGroup);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport float Mesh_getBoundingSphereRadius(Ogre::Mesh* mesh)
{
	return mesh->getBoundingSphereRadius();
}

extern "C" _AnomalousExport void Mesh__setBoundingSphereRadius(Ogre::Mesh* mesh, float radius)
{
	mesh->_setBoundingSphereRadius(radius);
}

extern "C" _AnomalousExport void Mesh_setSkeletonName(Ogre::Mesh* mesh, String skeletonName)
{
	mesh->setSkeletonName(skeletonName);
}

extern "C" _AnomalousExport bool Mesh_hasSkeleton(Ogre::Mesh* mesh)
{
	return mesh->hasSkeleton();
}

extern "C" _AnomalousExport bool Mesh_hasVertexAnimation(Ogre::Mesh* mesh)
{
	return mesh->hasVertexAnimation();
}

extern "C" _AnomalousExport Ogre::Skeleton* Mesh_getSkeleton(Ogre::Mesh* mesh, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::SkeletonPtr& skeletonPtr = mesh->getSkeleton();
	processWrapper(skeletonPtr.getPointer(), &skeletonPtr);
	return skeletonPtr.getPointer();
}

extern "C" _AnomalousExport String Mesh_getSkeletonName(Ogre::Mesh* mesh)
{
	return mesh->getSkeletonName().c_str();
}

extern "C" _AnomalousExport void Mesh__initAnimationState(Ogre::Mesh* mesh, Ogre::AnimationStateSet* animSet)
{
	mesh->_initAnimationState(animSet);
}

extern "C" _AnomalousExport void Mesh__refreshAnimationState(Ogre::Mesh* mesh, Ogre::AnimationStateSet* animSet)
{
	mesh->_refreshAnimationState(animSet);
}

extern "C" _AnomalousExport void Mesh_clearBoneAssignments(Ogre::Mesh* mesh)
{
	mesh->clearBoneAssignments();
}

extern "C" _AnomalousExport ushort Mesh_getNumLodLevels(Ogre::Mesh* mesh)
{
	return mesh->getNumLodLevels();
}

extern "C" _AnomalousExport void Mesh_updateManualLodLevel(Ogre::Mesh* mesh, ushort index, String meshName)
{
	mesh->updateManualLodLevel(index, meshName);
}

extern "C" _AnomalousExport ushort Mesh_getLodIndex(Ogre::Mesh* mesh, float depth)
{
	return mesh->getLodIndex(depth);
}

extern "C" _AnomalousExport void Mesh_removeLodLevels(Ogre::Mesh* mesh)
{
	mesh->removeLodLevels();
}

extern "C" _AnomalousExport void Mesh_setVertexBufferPolicy(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage)
{
	mesh->setVertexBufferPolicy(usage);
}

extern "C" _AnomalousExport void Mesh_setVertexBufferPolicyShadow(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage, bool shadowBuffer)
{
	mesh->setVertexBufferPolicy(usage, shadowBuffer);
}

extern "C" _AnomalousExport void Mesh_setIndexBufferPolicy(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage)
{
	mesh->setIndexBufferPolicy(usage);
}

extern "C" _AnomalousExport void Mesh_setIndexBufferPolicyShadow(Ogre::Mesh* mesh, Ogre::HardwareBuffer::Usage usage, bool shadowBuffer)
{
	mesh->setIndexBufferPolicy(usage, shadowBuffer);
}

extern "C" _AnomalousExport Ogre::HardwareBuffer::Usage Mesh_getVertexBufferUsage(Ogre::Mesh* mesh)
{
	return mesh->getVertexBufferUsage();
}

extern "C" _AnomalousExport Ogre::HardwareBuffer::Usage Mesh_getIndexBufferUsage(Ogre::Mesh* mesh)
{
	return mesh->getIndexBufferUsage();
}

extern "C" _AnomalousExport bool Mesh_isVertexBufferShadowed(Ogre::Mesh* mesh)
{
	return mesh->isVertexBufferShadowed();
}

extern "C" _AnomalousExport bool Mesh_isIndexBufferShadowed(Ogre::Mesh* mesh)
{
	return mesh->isIndexBufferShadowed();
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors(Ogre::Mesh* mesh)
{
	mesh->buildTangentVectors();
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors1(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic)
{
	mesh->buildTangentVectors(targetSemantic);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors2(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors3(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors4(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors5(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated);
}

extern "C" _AnomalousExport void Mesh_buildTangentVectors6(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated, bool storeParityInW)
{
	mesh->buildTangentVectors(targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated, storeParityInW);
}

extern "C" _AnomalousExport bool Mesh_suggestTangentVectorBuildParams(Ogre::Mesh* mesh, Ogre::VertexElementSemantic targetSemantic, ushort* outSourceCoordSet, ushort* outIndex)
{
	return mesh->suggestTangentVectorBuildParams(targetSemantic, *outSourceCoordSet, *outIndex);
}

extern "C" _AnomalousExport void Mesh_buildEdgeList(Ogre::Mesh* mesh)
{
	mesh->buildEdgeList();
}

extern "C" _AnomalousExport void Mesh_freeEdgeList(Ogre::Mesh* mesh)
{
	mesh->freeEdgeList();
}

extern "C" _AnomalousExport void Mesh_prepareForShadowVolume(Ogre::Mesh* mesh)
{
	mesh->prepareForShadowVolume();
}

extern "C" _AnomalousExport bool Mesh_isPreparedForShadowVolumes(Ogre::Mesh* mesh)
{
	return mesh->isPreparedForShadowVolumes();
}

extern "C" _AnomalousExport bool Mesh_isEdgeListBuilt(Ogre::Mesh* mesh)
{
	return mesh->isEdgeListBuilt();
}

extern "C" _AnomalousExport uint Mesh_getSubMeshIndex(Ogre::Mesh* mesh, String name)
{
	return mesh->_getSubMeshIndex(name);
}

extern "C" _AnomalousExport void Mesh_setAutoBuildEdgeLists(Ogre::Mesh* mesh, bool autobuild)
{
	mesh->setAutoBuildEdgeLists(autobuild);
}

extern "C" _AnomalousExport bool Mesh_getAutoBuildEdgeLists(Ogre::Mesh* mesh)
{
	return mesh->getAutoBuildEdgeLists();
}

extern "C" _AnomalousExport Ogre::VertexAnimationType Mesh_getSharedVertexDataAnimationType(Ogre::Mesh* mesh)
{
	return mesh->getSharedVertexDataAnimationType();
}

extern "C" _AnomalousExport Ogre::Animation* Mesh_createAnimation(Ogre::Mesh* mesh, String name, float length)
{
	return mesh->createAnimation(name, length);
}

extern "C" _AnomalousExport Ogre::Animation* Mesh_getAnimation(Ogre::Mesh* mesh, String name)
{
	return mesh->getAnimation(name);
}

extern "C" _AnomalousExport bool Mesh_hasAnimation(Ogre::Mesh* mesh, String name)
{
	return mesh->hasAnimation(name);
}

extern "C" _AnomalousExport void Mesh_removeAnimation(Ogre::Mesh* mesh, String name)
{
	mesh->removeAnimation(name);
}

extern "C" _AnomalousExport ushort Mesh_getNumAnimations(Ogre::Mesh* mesh)
{
	return mesh->getNumAnimations();
}

extern "C" _AnomalousExport Ogre::Animation* Mesh_getAnimationIndex(Ogre::Mesh* mesh, ushort index)
{
	return mesh->getAnimation(index);
}

extern "C" _AnomalousExport void Mesh_removeAllAnimations(Ogre::Mesh* mesh)
{
	mesh->removeAllAnimations();
}

extern "C" _AnomalousExport void Mesh_updateMaterialForAllSubMeshes(Ogre::Mesh* mesh)
{
	mesh->updateMaterialForAllSubMeshes();
}

extern "C" _AnomalousExport Ogre::Pose* Mesh_createPose(Ogre::Mesh* mesh, ushort target)
{
	return mesh->createPose(target);
}

extern "C" _AnomalousExport Ogre::Pose* Mesh_createPoseName(Ogre::Mesh* mesh, ushort target, String name)
{
	return mesh->createPose(target, name);
}

extern "C" _AnomalousExport int Mesh_getPoseCount(Ogre::Mesh* mesh)
{
	return mesh->getPoseCount();
}

extern "C" _AnomalousExport Ogre::Pose* Mesh_getPose(Ogre::Mesh* mesh, ushort index)
{
	return mesh->getPose(index);
}

extern "C" _AnomalousExport Ogre::Pose* Mesh_getPoseName(Ogre::Mesh* mesh, String name)
{
	return mesh->getPose(name);
}

extern "C" _AnomalousExport void Mesh_removePose(Ogre::Mesh* mesh, ushort index)
{
	mesh->removePose(index);
}

extern "C" _AnomalousExport void Mesh_removePoseName(Ogre::Mesh* mesh, String name)
{
	mesh->removePose(name);
}

extern "C" _AnomalousExport void Mesh_removeAllPoses(Ogre::Mesh* mesh)
{
	mesh->removeAllPoses();
}

extern "C" _AnomalousExport void Mesh__updateCompiledBoneAssignments(Ogre::Mesh* mesh)
{
	mesh->_updateCompiledBoneAssignments();
}