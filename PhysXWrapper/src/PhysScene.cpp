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
actors(gcnew ActorDictionary()),
nativeRaycastReport(new NativeRaycastReport()),
nativeContactReport(new NativeContactReport()),
joints(gcnew JointDictionary()),
actorGroupPairBuffer(new NxActorGroupPair()),
actorGroupPairPosition(0),
sceneExportWrapper(new NxSceneExportWrapper(scene)),
softBodies(gcnew SoftBodyDictionary())
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
		PhysActor^ actor = gcnew PhysActor(nxActor, desc->Name, desc->ShapeReference);
		actors[actor->Name] = actor;

		if(onActorAdded != nullptr)
		{
			onActorAdded->Invoke(actor);
		}

		return actor;
	}
	return nullptr;
}

void PhysScene::releaseActor(PhysActor^ actor)
{
	if( actors.ContainsKey(actor->Name) )
	{
		if(onActorRemoved != nullptr)
		{
			onActorRemoved->Invoke(actor);
		}

		actors.Remove(actor->Name);
		scene->releaseActor(*actor->actor);

		delete actor;
	}
	else
	{
		Logging::Log::Default->sendMessage("The actor " + actor->Name + " is not part of the scene.  Not released.", Logging::LogLevel::Error, "Physics");
	}
}

PhysActor^ PhysScene::getActor(Engine::Identifier^ name)
{
	if(actors.ContainsKey(name))
	{
		return actors[name];
	}
	return nullptr;
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
	if(joints.ContainsKey(jointDesc->Name))
	{
		Logging::Log::Default->sendMessage("Attempted to create a duplicate joint named " + jointDesc->Name + " aborting creation.", Logging::LogLevel::Error, "Physics");
		return nullptr;
	}
	else{
		NxJoint* nxJoint = scene->createJoint(*jointDesc->jointDesc);
		if(nxJoint)
		{
			PhysJoint^ physJoint;
			switch(nxJoint->getType())
			{
				case NX_JOINT_PRISMATIC:
					physJoint = gcnew PhysPrismaticJoint(jointDesc->Name, (NxPrismaticJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_REVOLUTE:
					physJoint = gcnew PhysRevoluteJoint(jointDesc->Name, (NxRevoluteJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_CYLINDRICAL:
					physJoint = gcnew PhysCylindricalJoint(jointDesc->Name, (NxCylindricalJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_SPHERICAL:
					physJoint = gcnew PhysSphericalJoint(jointDesc->Name, (NxSphericalJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_POINT_ON_LINE:
					physJoint = gcnew PhysPointOnLineJoint(jointDesc->Name, (NxPointOnLineJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_POINT_IN_PLANE:
					physJoint = gcnew PhysPointInPlaneJoint(jointDesc->Name, (NxPointInPlaneJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_DISTANCE:
					physJoint = gcnew PhysDistanceJoint(jointDesc->Name, (NxDistanceJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_PULLEY:
					physJoint = gcnew PhysPulleyJoint(jointDesc->Name, (NxPulleyJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_FIXED:
					physJoint = gcnew PhysFixedJoint(jointDesc->Name, (NxFixedJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_D6:
					physJoint = gcnew PhysD6Joint(jointDesc->Name, (NxD6Joint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
			}
			joints.Add(physJoint->getName(), physJoint);

			if(onJointAdded != nullptr)
			{
				onJointAdded->Invoke(physJoint);
			}

			return physJoint;
		}
		return nullptr;
	}
}

void PhysScene::releaseJoint(PhysJoint^ joint)
{
	if(joints.ContainsKey(joint->getName()))
	{
		if(onJointRemoved != nullptr)
		{
			onJointRemoved->Invoke(joint);
		}

		scene->releaseJoint(*joint->joint);
		joints.Remove(joint->getName());
		delete joint;
	}
	else
	{
		Logging::Log::Default->sendMessage("Attempted to delete " + joint->getName() + " which is not part of this scene.  No changes made.", Logging::LogLevel::Error, "Physics");
	}
}

PhysJoint^ PhysScene::getJoint(Engine::Identifier^ name)
{
	if(joints.ContainsKey(name))
	{
		return joints[name];
	}
	Logging::Log::Default->sendMessage("Could not find joint named " + name + " in scene " + this->name + ".", Logging::LogLevel::Error, "Physics");
	return nullptr;
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
		PhysSoftBody^ softBody = gcnew PhysSoftBody(softBodyDesc->Name, nxSoftBody);
		softBodies[softBody->Name] = softBody;

		if(onSoftBodyAdded != nullptr)
		{
			onSoftBodyAdded->Invoke(softBody);
		}

		return softBody;
	}
	return nullptr;
}

void PhysScene::releaseSoftBody(PhysSoftBody^ softBody)
{
	if(softBodies.ContainsKey(softBody->Name))
	{
		if(onSoftBodyRemoved != nullptr)
		{
			onSoftBodyRemoved->Invoke(softBody);
		}

		softBodies.Remove(softBody->Name);
		scene->releaseSoftBody(*softBody->softBody);

		delete softBody;
	}
	else
	{
		Logging::Log::Default->sendMessage("The SoftBody " + softBody->Name + " is not part of the scene.  Not released.", Logging::LogLevel::Error, "Physics");
	}
}

PhysSoftBody^ PhysScene::getSoftBody(Engine::Identifier^ name)
{
	if(softBodies.ContainsKey(name))
	{
		return softBodies[name];
	}
	return nullptr;
}

}

}