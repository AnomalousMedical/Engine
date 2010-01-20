#pragma once

#include "OgreMesh.h"
#include "AutoPtr.h"
#include "Enums.h"
#include "Resource.h"
#include "VertexElement.h"
#include "AnimationTrack.h"
#include "SubMeshCollection.h"
#include "AnimationCollection.h"
#include "PoseCollection.h"
#include "MeshPtr.h"

namespace OgreWrapper{

using namespace System::Runtime::InteropServices;

ref class SubMesh;
ref class AxisAlignedBox;
ref class SkeletonPtr;
value class VertexBoneAssignment;
ref class AnimationStateSet;
ref class Animation;
ref class Pose;
ref class SubMesh;
ref class VertexData;

[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class Mesh : public Resource
{
private:
	AutoPtr<Ogre::MeshPtr> meshAutoPtr;
	Ogre::Mesh* mesh;
	SubMeshCollection subMeshes;
	AnimationCollection animations;
	PoseCollection poses;

	VertexData^ sharedVertexData;

internal:
	Mesh(const Ogre::MeshPtr& mesh);

	Ogre::Mesh* getOgreMesh()
	{
		return mesh;
	}

	Ogre::MeshPtr getOgreMeshPtr()
	{
		return *(meshAutoPtr.Get());
	}

public:
	virtual ~Mesh(void);

	SubMesh^ createSubMesh();

	SubMesh^ createSubMesh(System::String^ name);

	void nameSubMesh(System::String^ name, unsigned short index);

	unsigned short _getSubMeshIndex(System::String^ name);

	unsigned short getNumSubMeshes();

	SubMesh^ getSubMesh(unsigned short index);

	SubMesh^ getSubMesh(System::String^ name);

	MeshPtr^ clone(System::String^ newName);

	MeshPtr^ clone(System::String^ newName, System::String^ newGroup);

	AxisAlignedBox^ getBounds();

	float getBoundingSphereRadius();

	void _setBounds(AxisAlignedBox^ bounds);

	void _setBounds(AxisAlignedBox^ bounds, bool pad);

	void _setBoundingSphereRadius(float radius);

	void setSkeletonName(System::String^ skeletonName);

	bool hasSkeleton();

	bool hasVertexAnimation();

	SkeletonPtr^ getSkeleton();

	System::String^ getSkeletonName();

	void _initAnimationState(AnimationStateSet^ animSet);

	void _refreshAnimationState(AnimationStateSet^ animSet);

	void clearBoneAssignments();

	unsigned short getNumLodLevels();

	void createManualLodLevel(float fromDepth, System::String^ meshName);

	void updateManualLodLevel(unsigned short index, System::String^ meshName);

	unsigned short getLodIndex(float depth);

	bool isLodManual();

	void removeLodLevels();

	void setVertexBufferPolicy(HardwareBuffer::Usage usage);

	void setVertexBufferPolicy(HardwareBuffer::Usage usage, bool shadowBuffer);

	void setIndexBufferPolicy(HardwareBuffer::Usage usage);

	void setIndexBufferPolicy(HardwareBuffer::Usage usage, bool shadowBuffer);

	HardwareBuffer::Usage getVertexBufferUsage();

	HardwareBuffer::Usage getIndexBufferUsage();

	bool isVertexBufferShadowed();

	bool isIndexBufferShadowed();

	void buildTangentVectors();

	void buildTangentVectors(VertexElementSemantic targetSemantic);

	void buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet);

	void buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index);

	void buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index, bool splitMirrored);

	void buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index, bool splitMirrored, bool splitRotated);

	void buildTangentVectors(VertexElementSemantic targetSemantic, unsigned short sourceTexCoordSet, unsigned short index, bool splitMirrored, bool splitRotated, bool storeParityInW);

	bool suggestTangentVectorBuildParams(VertexElementSemantic targetSemantic, [Out] unsigned short %outSourceCoordSet, [Out] unsigned short % outIndex);

	void buildEdgeList();

	void freeEdgeList();

	void prepareForShadowVolume();

	bool isPreparedForShadowVolumes();

	bool isEdgeListBuilt();

	unsigned int getSubMeshIndex(System::String^ name);

	void setAutoBuildEdgeLists(bool autobuild);

	bool getAutoBuildEdgeLists();

	VertexAnimationType getSharedVertexDataAnimationType();

	Animation^ createAnimation(System::String^ name, float length);

	Animation^ getAnimation(System::String^ name);

	bool hasAnimation(System::String^ name);

	void removeAnimation(System::String^ name);

	unsigned short getNumAnimations();

	Animation^ getAnimation(unsigned short index);

	void removeAllAnimations();

	void updateMaterialForAllSubMeshes();

	Pose^ createPose(unsigned short target);

	Pose^ createPose(unsigned short target, System::String^ name);

	size_t getPoseCount();

	Pose^ getPose(unsigned short index);

	Pose^ getPose(System::String^ name);

	void removePose(unsigned short index);

	void removePose(System::String^ name);

	void removeAllPoses();

	property VertexData^ SharedVertexData 
	{
		VertexData^ get();
	}
};

}