#include "StdAfx.h"
#include "..\include\Mesh.h"
#include "OgreMesh.h"
#include "MarshalUtils.h"
#include "AxisAlignedBox.h"
#include "AnimationStateSet.h"
#include "SkeletonManager.h"
#include "Pose.h"
#include "Animation.h"
#include "SubMesh.h"
#include "VertexData.h"
#include "MeshManager.h"

namespace Engine{

namespace Rendering{

Mesh::Mesh(const Ogre::MeshPtr& mesh)
:RenderResource(mesh.get()),
meshAutoPtr(new Ogre::MeshPtr(mesh)),
mesh(mesh.get()),
sharedVertexData(nullptr)
{
	if(mesh->sharedVertexData != NULL)
	{
		sharedVertexData = gcnew VertexData(mesh->sharedVertexData);
	}
}

Mesh::~Mesh(void)
{
	if(mesh != 0)
	{
		if(sharedVertexData != nullptr)
		{
			delete sharedVertexData;
			sharedVertexData = nullptr;
		}
		
		mesh = 0;
	}
}

SubMesh^ Mesh::createSubMesh()
{
	return subMeshes.getObject(mesh->createSubMesh());
}

SubMesh^ Mesh::createSubMesh(System::String^ name)
{
	return subMeshes.getObject(mesh->createSubMesh(MarshalUtils::convertString(name)));
}

void Mesh::nameSubMesh(System::String^ name, unsigned short index)
{
	return mesh->nameSubMesh(MarshalUtils::convertString(name), index);
}

unsigned short Mesh::_getSubMeshIndex(System::String^ name)
{
	return mesh->_getSubMeshIndex(MarshalUtils::convertString(name));
}

unsigned short Mesh::getNumSubMeshes()
{
	return mesh->getNumSubMeshes();
}

SubMesh^ Mesh::getSubMesh(unsigned short index)
{
	return subMeshes.getObject(mesh->getSubMesh(index));
}

SubMesh^ Mesh::getSubMesh(System::String^ name)
{
	return subMeshes.getObject(mesh->getSubMesh(MarshalUtils::convertString(name)));
}

MeshPtr^ Mesh::clone(System::String^ newName)
{
	return MeshManager::getInstance()->getObject(mesh->clone(MarshalUtils::convertString(newName)));
}

MeshPtr^ Mesh::clone(System::String^ newName, System::String^ newGroup)
{
	return MeshManager::getInstance()->getObject(mesh->clone(MarshalUtils::convertString(newName), MarshalUtils::convertString(newGroup)));
}

AxisAlignedBox^ Mesh::getBounds()
{
	return gcnew AxisAlignedBox(&mesh->getBounds());
}

float Mesh::getBoundingSphereRadius()
{
	return mesh->getBoundingSphereRadius();
}

void Mesh::_setBounds(AxisAlignedBox^ bounds)
{
	return mesh->_setBounds(*bounds->getOgreBox());
}

void Mesh::_setBounds(AxisAlignedBox^ bounds, bool pad)
{
	return mesh->_setBounds(*bounds->getOgreBox(), pad);
}

void Mesh::_setBoundingSphereRadius(float radius)
{
	return mesh->_setBoundingSphereRadius(radius);
}

void Mesh::setSkeletonName(System::String^ skeletonName)
{
	return mesh->setSkeletonName(MarshalUtils::convertString(skeletonName));
}

bool Mesh::hasSkeleton()
{
	return mesh->hasSkeleton();
}

bool Mesh::hasVertexAnimation()
{
	return mesh->hasVertexAnimation();
}

SkeletonPtr^ Mesh::getSkeleton()
{
	return SkeletonManager::getInstance()->getObject(mesh->getSkeleton());
}

System::String^ Mesh::getSkeletonName()
{
	return MarshalUtils::convertString(mesh->getSkeletonName());
}

void Mesh::_initAnimationState(AnimationStateSet^ animSet)
{
	return mesh->_initAnimationState(animSet->getOgreAnimationStateSet());
}

void Mesh::_refreshAnimationState(AnimationStateSet^ animSet)
{
	return mesh->_refreshAnimationState(animSet->getOgreAnimationStateSet());
}

void Mesh::clearBoneAssignments()
{
	return mesh->clearBoneAssignments();
}

unsigned short Mesh::getNumLodLevels()
{
	return mesh->getNumLodLevels();
}

void Mesh::createManualLodLevel(float fromDepth, System::String^ meshName)
{
	return mesh->createManualLodLevel(fromDepth, MarshalUtils::convertString(meshName));
}

void Mesh::updateManualLodLevel(unsigned short index, System::String^ meshName)
{
	return mesh->updateManualLodLevel(index, MarshalUtils::convertString(meshName));
}

unsigned short Mesh::getLodIndex(float depth)
{
	return mesh->getLodIndex(depth);
}

unsigned short Mesh::getLodIndexSquaredDepth(float squaredDepth)
{
	return mesh->getLodIndexSquaredDepth(squaredDepth);
}

bool Mesh::isLodManual()
{
	return mesh->isLodManual();
}

void Mesh::removeLodLevels()
{
	return mesh->removeLodLevels();
}

void Mesh::setVertexBufferPolicy(HardwareBuffer::Usage usage)
{
	return mesh->setVertexBufferPolicy((Ogre::HardwareBuffer::Usage)usage);
}

void Mesh::setVertexBufferPolicy(HardwareBuffer::Usage usage, bool shadowBuffer)
{
	return mesh->setVertexBufferPolicy((Ogre::HardwareBuffer::Usage)usage, shadowBuffer);
}

void Mesh::setIndexBufferPolicy(HardwareBuffer::Usage usage)
{
	return mesh->setIndexBufferPolicy((Ogre::HardwareBuffer::Usage)usage);
}

void Mesh::setIndexBufferPolicy(HardwareBuffer::Usage usage, bool shadowBuffer)
{
	return mesh->setIndexBufferPolicy((Ogre::HardwareBuffer::Usage)usage, shadowBuffer);
}

HardwareBuffer::Usage Mesh::getVertexBufferUsage()
{
	return (HardwareBuffer::Usage)mesh->getVertexBufferUsage();
}

HardwareBuffer::Usage Mesh::getIndexBufferUsage()
{
	return (HardwareBuffer::Usage)mesh->getIndexBufferUsage();
}

bool Mesh::isVertexBufferShadowed()
{
	return mesh->isVertexBufferShadowed();
}

bool Mesh::isIndexBufferShadowed()
{
	return mesh->isIndexBufferShadowed();
}

void Mesh::buildTangentVectors()
{
	return mesh->buildTangentVectors();
}

void Mesh::buildTangentVectors(VertexElementSemantic targetSemantic)
{
	return mesh->buildTangentVectors((Ogre::VertexElementSemantic)targetSemantic);
}

void Mesh::buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet)
{
	return mesh->buildTangentVectors((Ogre::VertexElementSemantic)targetSemantic, sourceTexCoordSet);
}

void Mesh::buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index)
{
	return mesh->buildTangentVectors((Ogre::VertexElementSemantic)targetSemantic, sourceTexCoordSet, index);
}

void Mesh::buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index, bool splitMirrored)
{
	return mesh->buildTangentVectors((Ogre::VertexElementSemantic)targetSemantic, sourceTexCoordSet, index, splitMirrored);
}

void Mesh::buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index, bool splitMirrored, bool splitRotated)
{
	return mesh->buildTangentVectors((Ogre::VertexElementSemantic)targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated);
}

void Mesh::buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index, bool splitMirrored, bool splitRotated, bool storeParityInW)
{
	return mesh->buildTangentVectors((Ogre::VertexElementSemantic)targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated, storeParityInW);
}

bool Mesh::suggestTangentVectorBuildParams(VertexElementSemantic targetSemantic, [Out] unsigned short %outSourceCoordSet, [Out] unsigned short % outIndex)
{
	unsigned short cSet;
	unsigned short index;
	return mesh->suggestTangentVectorBuildParams((Ogre::VertexElementSemantic)targetSemantic, cSet, index);
	outSourceCoordSet = cSet;
	outIndex = index;
}

void Mesh::buildEdgeList()
{
	return mesh->buildEdgeList();
}

void Mesh::freeEdgeList()
{
	return mesh->freeEdgeList();
}

void Mesh::prepareForShadowVolume()
{
	return mesh->prepareForShadowVolume();
}

bool Mesh::isPreparedForShadowVolumes()
{
	return mesh->isPreparedForShadowVolumes();
}

bool Mesh::isEdgeListBuilt()
{
	return mesh->isEdgeListBuilt();
}

unsigned int Mesh::getSubMeshIndex(System::String^ name)
{
	Ogre::Mesh::SubMeshNameMap names = mesh->getSubMeshNameMap();
	return names[MarshalUtils::convertString(name)];
}

void Mesh::setAutoBuildEdgeLists(bool autobuild)
{
	return mesh->setAutoBuildEdgeLists(autobuild);
}

bool Mesh::getAutoBuildEdgeLists()
{
	return mesh->getAutoBuildEdgeLists();
}

VertexAnimationType Mesh::getSharedVertexDataAnimationType()
{
	return (VertexAnimationType)mesh->getSharedVertexDataAnimationType();
}

Animation^ Mesh::createAnimation(System::String^ name, float length)
{
	return animations.getObject(mesh->createAnimation(MarshalUtils::convertString(name), length));
}

Animation^ Mesh::getAnimation(System::String^ name)
{
	return animations.getObject(mesh->getAnimation(MarshalUtils::convertString(name)));
}

bool Mesh::hasAnimation(System::String^ name)
{
	return mesh->hasAnimation(MarshalUtils::convertString(name));
}

void Mesh::removeAnimation(System::String^ name)
{
	Ogre::String ogreString = MarshalUtils::convertString(name);
	animations.destroyObject(mesh->getAnimation(ogreString));
	mesh->removeAnimation(ogreString);
}

unsigned short Mesh::getNumAnimations()
{
	return mesh->getNumAnimations();
}

Animation^ Mesh::getAnimation(unsigned short index)
{
	return animations.getObject(mesh->getAnimation(index));
}

void Mesh::removeAllAnimations()
{
	animations.clearObjects();
	mesh->removeAllAnimations();
}

void Mesh::updateMaterialForAllSubMeshes()
{
	return mesh->updateMaterialForAllSubMeshes();
}

Pose^ Mesh::createPose(unsigned short target)
{
	return createPose(target, System::String::Empty);
}

Pose^ Mesh::createPose(unsigned short target, System::String^ name)
{
	return poses.getObject(mesh->createPose(target, MarshalUtils::convertString(name)));
}

size_t Mesh::getPoseCount()
{
	return mesh->getPoseCount();
}

Pose^ Mesh::getPose(unsigned short index)
{
	return poses.getObject(mesh->getPose(index));
}

Pose^ Mesh::getPose(System::String^ name)
{
	return poses.getObject(mesh->getPose(MarshalUtils::convertString(name)));
}

void Mesh::removePose(unsigned short index)
{
	poses.destroyObject(mesh->getPose(index));
	mesh->removePose(index);
}

void Mesh::removePose(System::String^ name)
{
	Ogre::String ogreString = MarshalUtils::convertString(name);
	poses.destroyObject(mesh->getPose(ogreString));
	mesh->removePose(ogreString);
}

void Mesh::removeAllPoses()
{
	poses.clearObjects();
	mesh->removeAllPoses();
}

VertexData^ Mesh::SharedVertexData::get() 
{
	return sharedVertexData;
}

}

}