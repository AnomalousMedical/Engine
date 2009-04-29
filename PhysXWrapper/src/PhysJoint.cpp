#include "StdAfx.h"
#include "PhysJoint.h"
#include "NxPhysics.h"
#include "MarshalUtils.h"
#include "MathUtil.h"
#include "NxJointDesc.h"

namespace PhysXWrapper
{

PhysJoint::PhysJoint(NxJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:joint(joint),
actor0(actor0),
actor1(actor1),
scene(scene)
{

}

PhysJoint::~PhysJoint()
{
	joint = 0;
}

PhysActor^ PhysJoint::getActor0()
{
	return actor0;
}

PhysActor^ PhysJoint::getActor1()
{
	return actor1;
}

void PhysJoint::setGlobalAnchor(Engine::Vector3 vec)
{
	joint->setGlobalAnchor(NxVec3(vec.x, vec.y, vec.z));
}

void PhysJoint::setGlobalAxis(Engine::Vector3 vec)
{
	joint->setGlobalAxis(NxVec3(vec.x, vec.y, vec.z));
}

Engine::Vector3 PhysJoint::getGlobalAnchor()
{
	NxVec3 v = joint->getGlobalAnchor();
	return Engine::Vector3(v.x, v.y, v.z);
}

Engine::Vector3 PhysJoint::getGlobalAxis()
{
	NxVec3 v = joint->getGlobalAxis();
	return Engine::Vector3(v.x, v.y, v.z);
}

JointState PhysJoint::getState()
{
	return (JointState)joint->getState();
}

void PhysJoint::setBreakable(float maxForce, float maxTorque)
{
	joint->setBreakable(maxForce, maxTorque);
}

void PhysJoint::getBreakable(float% maxForce, float% maxTorque)
{
	NxReal mf, mt;
	joint->getBreakable(mf, mt);
	maxForce = mf;
	maxTorque = mt;
}

void PhysJoint::setSolverExtrapolationFactor(float factor)
{
	joint->setSolverExtrapolationFactor(factor);
}

float PhysJoint::getSolverExtrapolationFactor()
{
	return joint->getSolverExtrapolationFactor();
}

void PhysJoint::setUseAccelerationSpring(bool b)
{
	joint->setUseAccelerationSpring(b);
}

bool PhysJoint::getUseAccelerationSpring()
{
	return joint->getUseAccelerationSpring();
}

JointType PhysJoint::getType()
{
	return (JointType)joint->getType();
}

PhysScene^ PhysJoint::getScene()
{
	return scene;
}

//---------------
//   Limits
//---------------
void PhysJoint::setLimitPoint(Engine::Vector3% point, bool pointIsOnActor1)
{
	joint->setLimitPoint(NxVec3(point.x, point.y, point.z), pointIsOnActor1);
}

bool PhysJoint::getLimitPoint(Engine::Vector3% worldLimitPoint)
{
	NxVec3 v;
	bool result = joint->getLimitPoint(v);
	MathUtil::copyVector3(v, worldLimitPoint);
	return result;
}

bool PhysJoint::addLimitPlane(Engine::Vector3% normal, Engine::Vector3% pointInPlane, float restitution)
{
	return joint->addLimitPlane(NxVec3(normal.x, normal.y, normal.z), NxVec3(pointInPlane.x, pointInPlane.y, pointInPlane.z), restitution);
}

void PhysJoint::purgeLimitPlanes()
{
	joint->purgeLimitPlanes();
}

void PhysJoint::resetLimitPlaneIterator()
{
	joint->resetLimitPlaneIterator();
}

bool PhysJoint::hasMoreLimitPlanes()
{
	return joint->hasMoreLimitPlanes();
}

bool PhysJoint::getNextLimitPlane(Engine::Vector3% planeNormal, float% planeD, float% restitution)
{
	float pd, rest;
	NxVec3 v;
	bool result = joint->getNextLimitPlane(v, pd, &rest);
	MathUtil::copyVector3(v, planeNormal);
	planeD = pd;
	restitution = rest;
	return result;
}

void PhysJoint::setLocalAnchor0(Engine::Vector3 anchor)
{
	NxJointDesc& desc = getDesc();
	MathUtil::copyVector3(anchor, desc.localAnchor[0]);
	this->reloadFromDesc(desc);
}

void PhysJoint::setLocalAnchor1(Engine::Vector3 anchor)
{
	NxJointDesc& desc = getDesc();
	MathUtil::copyVector3(anchor, desc.localAnchor[1]);
	this->reloadFromDesc(desc);
}

}