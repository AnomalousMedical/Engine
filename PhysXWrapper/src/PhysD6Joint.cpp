#include "StdAfx.h"
#include "..\include\PhysD6Joint.h"
#include "NxPhysics.h"
#include "PhysD6JointDesc.h"
#include "MathUtil.h"
#include "PhysActor.h"

namespace Physics
{

PhysD6Joint::PhysD6Joint(NxD6Joint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysD6Joint::saveToDesc(PhysD6JointDesc^ desc)
{
	typedJoint->saveToDesc(*desc->joint.Get());
	NxActor* actor0, *actor1;
	typedJoint->getActors(&actor0, &actor1);
	if(actor0)
	{
		desc->Actor[0] = *((PhysActorGCRoot*)actor0->userData);
	}
	if(actor1)
	{
		desc->Actor[1] = *((PhysActorGCRoot*)actor1->userData);
	}
}

void PhysD6Joint::loadFromDesc(PhysD6JointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

void PhysD6Joint::setDrivePosition(EngineMath::Vector3 position)
{
	typedJoint->setDrivePosition(NxVec3(position.x, position.y, position.z));
}

void PhysD6Joint::setDriveOrientation(EngineMath::Quaternion orientation)
{
	typedJoint->setDriveOrientation(MathUtil::convertNxQuaternion(orientation));
}

void PhysD6Joint::setDriveLinearVelocity(EngineMath::Vector3 linVel)
{
	typedJoint->setDriveLinearVelocity(NxVec3(linVel.x, linVel.y, linVel.z));
}

void PhysD6Joint::setDriveAngularVelocity(EngineMath::Vector3 angVel)
{
	typedJoint->setDriveAngularVelocity(NxVec3(angVel.x, angVel.y, angVel.z));
}

NxJointDesc& PhysD6Joint::getDesc()
{
	NxD6JointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}