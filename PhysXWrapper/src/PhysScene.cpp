#include "StdAfx.h"

#include "NxPhysics.h"

#include "..\include\PhysScene.h"
#include "MathUtil.h"

#include "PhysActorDesc.h"
#include "PhysActor.h"
#include "PhysJoint.h"
#include "PhysJointDesc.h"
#include "PhysPrismaticJoint.h"
#include "PhysRevoluteJoint.h"
#include "PhysCylindricalJoint.h"
#include "PhysSphericalJoint.h"
#include "PhysPointOnLineJoint.h"
#include "PhysPointInPlaneJoint.h"
#include "PhysDistanceJoint.h"
#include "PhysPulleyJoint.h"
#include "PhysFixedJoint.h"
#include "PhysD6Joint.h"
#include "PhysActorGroupPair.h"
#include "PhysMaterial.h"
#include "PhysMaterialDesc.h"
#include "PhysSoftBody.h"
#include "PhysSoftBodyDesc.h"

namespace Engine
{

namespace Physics
{

PhysScene::PhysScene(NxScene* scene, System::String^ name)
:scene(scene), 
name(name), 
pooledVector(new NxVec3()), 
nativeRaycastReport(new NativeRaycastReport()),
nativeContactReport(new NativeContactReport()),
actorGroupPairBuffer(new NxActorGroupPair()),
actorGroupPairPosition(0),
sceneExportWrapper(new NxSceneExportWrapper(scene))
{
	//setup callbacks
	scene->setUserContactReport(nativeContactReport.Get());

	//temp
	NxMaterial* defaultMaterial = scene->getMaterialFromIndex(0);
	defaultMaterial->setRestitution(0.5);
	defaultMaterial->setStaticFriction(0.5);
	defaultMaterial->setDynamicFriction(0.5);
}

PhysScene::~PhysScene()
{
	
}

void PhysScene::setGravity(EngineMath::Vector3 grav)
{
	scene->setGravity(NxVec3(grav.x, grav.y, grav.z));	
}

void PhysScene::getGravity(EngineMath::Vector3% grav)
{
	scene->getGravity(*(pooledVector.Get()));
	MathUtil::copyVector3(*(pooledVector.Get()), grav);
}

PhysActor^ PhysScene::createActor(PhysActorDesc^ desc)
{
	NxActor* nxActor = scene->createActor(*desc->actorDesc.Get());
	if( nxActor )
	{
		return actors.getObject(nxActor);
	}
	return nullptr;
}

void PhysScene::releaseActor(PhysActor^ actor)
{
	NxActor* nxActor = actor->actor;
	actors.destroyObject(nxActor);
	scene->releaseActor(*nxActor);
}

void PhysScene::stepSimulation(double time)
{
	scene->simulate((NxReal)time);
	scene->flushStream();
	scene->fetchResults(NX_RIGID_BODY_FINISHED, true);

	//Loop and update all transforms for objects that have moved
	NxU32 numTransforms = 0;
	NxActiveTransform* activeTransforms = scene->getActiveTransforms( numTransforms );
	if( numTransforms && activeTransforms )
	{
		PhysActorGCRoot* actor;
		for( NxU32 i = 0; i < numTransforms; ++i )
		{
			actor = (PhysActorGCRoot*)activeTransforms[i].userData;
			if( actor )
			{
				(*actor)->fireLocationChanged( activeTransforms[i].actor2World );
			}
		}
	}
}

void PhysScene::setActorGroupPairFlags( int group1, int group2, ContactPairFlag flags )
{
	scene->setActorGroupPairFlags( group1, group2, (NxU32)flags );
}

void PhysScene::startActorGroupPairIter()
{
	actorGroupPairPosition = 0;
}

bool PhysScene::hasNextActorGroupPair()
{
	NxU32 pos = actorGroupPairPosition;
	int count = scene->getActorGroupPairArray(actorGroupPairBuffer.Get(), 1, pos);
	actorGroupPairPosition = pos;
	return count > 0;
}

PhysActorGroupPair PhysScene::getNextActorGroupPair()
{
	return PhysActorGroupPair(actorGroupPairBuffer.Get());
}

int PhysScene::raycastAllShapes( EngineMath::Ray3 ray, 
						RaycastReport^ report, 
						ShapesType shapesType )
{
	return raycastAllShapes(ray, report, shapesType, -1, NX_MAX_F32, RaycastBit::NX_RAYCAST_ALL);
}

int PhysScene::raycastAllShapes( EngineMath::Ray3 ray, 
						RaycastReport^ report, 
						ShapesType shapesType, 
						unsigned int groups )
{
	return raycastAllShapes(ray, report, shapesType, groups, NX_MAX_F32, RaycastBit::NX_RAYCAST_ALL);
}

int PhysScene::raycastAllShapes( EngineMath::Ray3 ray,
						RaycastReport^ report, 
						ShapesType shapesType, 
						unsigned int groups, 
						float maxDistance )
{
	return raycastAllShapes(ray, report, shapesType, groups, maxDistance, RaycastBit::NX_RAYCAST_ALL);
}

int PhysScene::raycastAllShapes( EngineMath::Ray3 ray, 
						RaycastReport^ report, 
						ShapesType shapesType, 
						unsigned int groups, 
						float maxDistance, 
						RaycastBit hintFlags )
{
	NxRay nxRay;
	MathUtil::copyRay(ray, nxRay);
	nativeRaycastReport->setCurrentReport( report );
	return scene->raycastAllShapes( nxRay, *nativeRaycastReport.Get(), (NxShapesType)shapesType, 
									groups, maxDistance, (NxRaycastBit)hintFlags );
}

PhysJoint^ PhysScene::createJoint(PhysJointDesc^ jointDesc)
{
	NxJoint* nxJoint = scene->createJoint(*jointDesc->jointDesc);
	if(nxJoint)
	{
		return joints.getObject(nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
	}
	return nullptr;
}

void PhysScene::releaseJoint(PhysJoint^ joint)
{
	NxJoint* nxJoint = joint->joint;
	joints.destroyObject(nxJoint);
	scene->releaseJoint(*nxJoint);
}

SceneFlags PhysScene::getFlags()
{
	return (SceneFlags)scene->getFlags();
}

//Properties
System::String^ PhysScene::Name::get() 
{ 
	return name; 
}

unsigned int PhysScene::getNbMaterials()
{
	return scene->getNbMaterials();
}

short PhysScene::getHighestMaterialIndex()
{
	return scene->getHighestMaterialIndex();
}

PhysMaterial^ PhysScene::getMaterialFromIndex(short index)
{
	return *((PhysMaterialGcRoot*)scene->getMaterialFromIndex(index)->userData);
}

PhysMaterial^ PhysScene::createMaterial(PhysMaterialDesc^ desc)
{
	NxMaterial* nxMat = scene->createMaterial(*(desc->desc.Get()));
	return gcnew PhysMaterial(nxMat, desc->Name);
}

void PhysScene::releaseMaterial(PhysMaterial^ material)
{
	scene->releaseMaterial(*material->material);
	delete material;
}

PhysSoftBody^ PhysScene::createSoftBody(PhysSoftBodyDesc^ softBodyDesc)
{
	NxSoftBody* nxSoftBody = scene->createSoftBody(*softBodyDesc->desc.Get());
	if(nxSoftBody)
	{
		return softBodies.getObject(nxSoftBody);
	}
	return nullptr;
}

void PhysScene::releaseSoftBody(PhysSoftBody^ softBody)
{
	NxSoftBody* nxSoftBody = softBody->softBody;
	softBodies.destroyObject(nxSoftBody);
	scene->releaseSoftBody(*nxSoftBody);
}

}

}