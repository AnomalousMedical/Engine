#include "StdAfx.h"
#include "..\include\RigidBody.h"
#include "MotionState.h"
#include "RigidBodyDefinition.h"
#include "BulletScene.h"
#include "TypedConstraintElement.h"

namespace BulletPlugin
{

#pragma unmanaged

float computeImpulseDenominator(btRigidBody* rigidBody, float* pos, float* normal)
{
	return rigidBody->computeImpulseDenominator(btVector3(pos[0], pos[1], pos[2]), btVector3(normal[0], normal[1], normal[2]));
}

void getAabb(btRigidBody* rigidBody, float* aabbMinOut, float* aabbMaxOut)
{
	btVector3 btMin, btMax;
	rigidBody->getAabb(btMin, btMax);
	aabbMinOut[0] = btMin.x();
	aabbMinOut[1] = btMin.y();
	aabbMinOut[2] = btMin.z();
	aabbMaxOut[0] = btMax.x();
	aabbMaxOut[1] = btMax.y();
	aabbMaxOut[2] = btMax.z();

}

void translate(btRigidBody* rigidBody, float* vector)
{
	rigidBody->translate(btVector3(vector[0], vector[1], vector[2]));
}

void getVelocityInLocalPoint(btRigidBody* rigidBody, float* vector, float* outVector)
{
	btVector3 btVec3 = rigidBody->getVelocityInLocalPoint(btVector3(vector[0], vector[1], vector[2]));
	outVector[0] = btVec3.x();
	outVector[1] = btVec3.y();
	outVector[2] = btVec3.z();
}

void applyTorqueImpulse(btRigidBody* rigidBody, float* torque)
{
	rigidBody->applyTorqueImpulse(btVector3(torque[0], torque[1], torque[2]));
}

void applyCentralImpulse(btRigidBody* rigidBody, float* impulse)
{
	rigidBody->applyCentralImpulse(btVector3(impulse[0], impulse[1], impulse[2]));
}

void applyForce(btRigidBody* rigidBody, float* force, float* rel_pos)
{
	rigidBody->applyForce(btVector3(force[0], force[1], force[2]), btVector3(rel_pos[0], rel_pos[1], rel_pos[2]));
}

void applyTorque(btRigidBody* rigidBody, float* vector)
{
	rigidBody->applyTorque(btVector3(vector[0], vector[1], vector[2]));
}

void applyCentralForce(btRigidBody* rigidBody, float* vector)
{
	rigidBody->applyCentralForce(btVector3(vector[0], vector[1], vector[2]));
}

float computeAngularImpulseDenominator(btRigidBody* rigidBody, float* vector)
{
	return rigidBody->computeAngularImpulseDenominator(btVector3(vector[0], vector[1], vector[2]));
}

void setMassProps(btRigidBody* body, float mass)
{
	btVector3 inertia = body->getInvInertiaDiagLocal();
	inertia.setX(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.x(): btScalar(0.0));
	inertia.setY(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.y(): btScalar(0.0));
	inertia.setZ(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.z(): btScalar(0.0));
	body->setMassProps(mass, inertia);
}

void setMassProps(btRigidBody* body, float mass, float* vector)
{
	body->setMassProps(mass, btVector3(vector[0], vector[1], vector[2]));
}

void setWorldTranslation(btRigidBody* rigidBody, float* trans)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getOrigin().setX(trans[0]);
	transform.getOrigin().setY(trans[1]);
	transform.getOrigin().setZ(trans[2]);
}

void setWorldRotation(btRigidBody* rigidBody, float* rot)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getBasis().setRotation(btQuaternion(rot[0], rot[1], rot[2], rot[3]));
}

void setWorldTransform(btRigidBody* rigidBody, float* trans, float* rot)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getOrigin().setX(trans[0]);
	transform.getOrigin().setY(trans[1]);
	transform.getOrigin().setZ(trans[2]);
	transform.getBasis().setRotation(btQuaternion(rot[0], rot[1], rot[2], rot[3]));
}

void setLinearVelocity(btRigidBody* rigidBody, float* linearVelocity)
{
	rigidBody->setLinearVelocity(btVector3(linearVelocity[0], linearVelocity[1], linearVelocity[2]));
}

void setAngularVelocity(btRigidBody* rigidBody, float* angularVelocity)
{
	rigidBody->setAngularVelocity(btVector3(angularVelocity[0], angularVelocity[1], angularVelocity[2]));
}

void setAnisotropicFriction(btRigidBody* rigidBody, float* vector)
{
	rigidBody->setAnisotropicFriction(btVector3(vector[0], vector[1], vector[2]));
}

void setLocalScaling(btRigidBody* rigidBody, float* vector)
{
	rigidBody->getCollisionShape()->setLocalScaling(btVector3(vector[0], vector[1], vector[2]));
}

void getLocalScaling(btRigidBody* rigidBody, float* vector)
{
	const btVector3& scaling = rigidBody->getCollisionShape()->getLocalScaling();
	vector[0] = scaling.x();
	vector[1] = scaling.y();
	vector[2] = scaling.z();
}

#pragma managed

RigidBody::RigidBody(RigidBodyDefinition^ description, BulletScene^ scene, Vector3 initialTrans, Quaternion initialRot)
:SimElement(description->Name, description->Subscription),
scene(scene),
shapeName(description->ShapeName),
maxContactDistance(description->MaxContactDistance),
collisionFilterMask(description->CollisionFilterMask),
collisionFilterGroup(description->CollisionFilterGroup)
{
	motionState = new MotionState(this, &initialTrans.x, &initialRot.x);
	description->ConstructionInfo->m_motionState = motionState;
	rigidBody = new btRigidBody(*description->ConstructionInfo);
	BulletPlugin::setLinearVelocity(rigidBody, &description->LinearVelocity.x);
	BulletPlugin::setAngularVelocity(rigidBody, &description->AngularVelocity.x);
	rigidBody->forceActivationState(static_cast<int>(description->CurrentActivationState));
	BulletPlugin::setAnisotropicFriction(rigidBody, &description->AnisotropicFriction.x);
	rigidBody->setDeactivationTime(description->DeactivationTime);
	rigidBody->setCollisionFlags(static_cast<int>(description->Flags));
	rigidBody->setHitFraction(description->HitFraction);
}

RigidBody::~RigidBody(void)
{
	if(rigidBody != 0)
	{
		System::Collections::Generic::List<TypedConstraintElement^> constraints(rigidBody->getNumConstraintRefs());
		//Gather up all constraints
		for(int i = 0; i < rigidBody->getNumConstraintRefs(); ++i)
		{
			btTypedConstraint* typedConstraint = rigidBody->getConstraintRef(i);
			ConstraintGCRoot* root = static_cast<ConstraintGCRoot*>(typedConstraint->getUserData());
			constraints.Add(*root);
		}
		//Set all constraints to inactive
		for each(TypedConstraintElement^ constraint in constraints)
		{
			constraint->setInactive();
		}

		delete motionState;
		motionState = 0;

		if(Owner->Enabled)
		{
			scene->DynamicsWorld->removeRigidBody(rigidBody);
		}

		delete rigidBody;
		rigidBody = 0;
	}
}

void RigidBody::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{
	Vector3 localTrans = translation;
	Quaternion localRot = rotation;
	BulletPlugin::setWorldTransform(rigidBody, &localTrans.x, &localRot.x);
}

void RigidBody::updateTranslationImpl(Vector3% translation)
{
	Vector3 localTrans = translation;
	setWorldTranslation(rigidBody, &localTrans.x);
}

void RigidBody::updateRotationImpl(Quaternion% rotation)
{
	Quaternion localRot = rotation;
	setWorldRotation(rigidBody, &localRot.x);
}

void RigidBody::updateScaleImpl(Vector3% scale)
{

}

void RigidBody::setEnabled(bool enabled)
{
	if(enabled)
	{
		scene->DynamicsWorld->addRigidBody(rigidBody, collisionFilterGroup, collisionFilterMask);
	}
	else
	{
		scene->DynamicsWorld->removeRigidBody(rigidBody);
	}
}

void RigidBody::fillOutDefinition(RigidBodyDefinition^ definition)
{
	float mass = rigidBody->getInvMass();
	if(mass > 0.0f)
	{
		definition->Mass = 1.0f / mass;
	}
	else
	{
		definition->Mass = 0.0f;
	}
	definition->AngularDamping = rigidBody->getAngularDamping();
	definition->AngularSleepingThreshold = rigidBody->getAngularSleepingThreshold();
	definition->Friction = rigidBody->getFriction();
	definition->LinearDamping = rigidBody->getLinearDamping();
	definition->LinearSleepingThreshold = rigidBody->getLinearSleepingThreshold();
	definition->Restitution = rigidBody->getRestitution();
	const btVector3& linearVelocity = rigidBody->getLinearVelocity();
	definition->LinearVelocity = Vector3(linearVelocity.x(), linearVelocity.y(), linearVelocity.z());
	const btVector3& angularVelocity = rigidBody->getAngularVelocity();
	definition->AngularVelocity = Vector3(angularVelocity.x(), angularVelocity.y(), angularVelocity.z());
	definition->CurrentActivationState = static_cast<ActivationState>(rigidBody->getActivationState());
	const btVector3& anisotropicFriction = rigidBody->getAnisotropicFriction();
	definition->AnisotropicFriction = Vector3(anisotropicFriction.x(), anisotropicFriction.y(), anisotropicFriction.z());
	definition->DeactivationTime = rigidBody->getDeactivationTime();
	definition->Flags = static_cast<CollisionFlags>(rigidBody->getCollisionFlags());
	definition->HitFraction = rigidBody->getHitFraction();
	definition->ShapeName = shapeName;
	definition->MaxContactDistance = maxContactDistance;
	definition->CollisionFilterGroup = collisionFilterGroup;
	definition->CollisionFilterMask = collisionFilterMask;
}

SimElementDefinition^ RigidBody::saveToDefinition()
{
	RigidBodyDefinition^ definition = gcnew RigidBodyDefinition(Name);
	fillOutDefinition(definition);
	return definition;
}

void RigidBody::setDamping(float linearDamping, float angularDamping)
{
	rigidBody->setDamping(linearDamping, angularDamping);
}

float RigidBody::getLinearDamping()
{
	return rigidBody->getLinearDamping();
}

float RigidBody::getAngularDamping()
{
	return rigidBody->getAngularDamping();
}

float RigidBody::getLinearSleepingThreshold()
{
	return rigidBody->getLinearSleepingThreshold();
}

float RigidBody::getAngularSleepingThreshold()
{
	return rigidBody->getAngularSleepingThreshold();
}

void RigidBody::setMassProps(float mass)
{
	BulletPlugin::setMassProps(rigidBody, mass);
}

void RigidBody::setMassProps(float mass, Vector3 inertia)
{
	BulletPlugin::setMassProps(rigidBody, mass, &inertia.x);
}

float RigidBody::getInvMass()
{
	return rigidBody->getInvMass();
}

void RigidBody::applyCentralForce(Vector3 force)
{
	BulletPlugin::applyCentralForce(rigidBody, &force.x);
}

Vector3 RigidBody::getTotalForce()
{
	const btVector3& totalForce = rigidBody->getTotalForce();
	return Vector3(totalForce.x(), totalForce.y(), totalForce.z());
}

Vector3 RigidBody::getTotalTorque()
{
	const btVector3& torque = rigidBody->getTotalTorque();
	return Vector3(torque.x(), torque.y(), torque.z());
}

void RigidBody::setSleepingThresholds(float linear, float angular)
{
	return rigidBody->setSleepingThresholds(linear, angular);
}

void RigidBody::applyTorque(Vector3 torque)
{
	BulletPlugin::applyTorque(rigidBody, &torque.x);
}

void RigidBody::applyForce(Vector3 force, Vector3 rel_pos)
{
	BulletPlugin::applyForce(rigidBody, &force.x, &rel_pos.x);
}

void RigidBody::applyCentralImpulse(Vector3 impulse)
{
	BulletPlugin::applyCentralImpulse(rigidBody, &impulse.x);
}

void RigidBody::applyTorqueImpulse(Vector3 torque)
{
	BulletPlugin::applyTorqueImpulse(rigidBody, &torque.x);
}

void RigidBody::clearForces()
{
	return rigidBody->clearForces();
}

Vector3 RigidBody::getCenterOfMassPosition()
{
	const btVector3& centerOfMass = rigidBody->getCenterOfMassPosition();
	return Vector3(centerOfMass.x(), centerOfMass.y(), centerOfMass.z());
}

Vector3 RigidBody::getLinearVelocity()
{
	const btVector3& velocity = rigidBody->getLinearVelocity();
	return Vector3(velocity.x(), velocity.y(), velocity.z());
}

Vector3 RigidBody::getAngularVelocity()
{
	const btVector3& velocity = rigidBody->getAngularVelocity();
	return Vector3(velocity.x(), velocity.y(), velocity.z());
}

void RigidBody::setLinearVelocity(Vector3 lin_vel)
{
	BulletPlugin::setLinearVelocity(rigidBody, &lin_vel.x);
}

void RigidBody::setAngularVelocity(Vector3 ang_vel)
{
	BulletPlugin::setAngularVelocity(rigidBody, &ang_vel.x);
}

Vector3 RigidBody::getVelocityInLocalPoint(Vector3 rel_pos)
{
	Vector3 ret;
	BulletPlugin::getVelocityInLocalPoint(rigidBody, &rel_pos.x, &ret.x);
	return ret;
}

void RigidBody::translate(Vector3 v)
{
	BulletPlugin::translate(rigidBody, &v.x);
}

void RigidBody::getAabb(Vector3% aabbMin, Vector3% aabbMax)
{
	pin_ptr<Vector3> pinMin = &aabbMin;
	pin_ptr<Vector3> pinMax = &aabbMax;
	BulletPlugin::getAabb(rigidBody, &pinMin->x, &pinMax->x);
}

float RigidBody::computeImpulseDenominator(Vector3 pos, Vector3 normal)
{
	return BulletPlugin::computeImpulseDenominator(rigidBody, &pos.x, &normal.x);
}

float RigidBody::computeAngularImpulseDenominator(Vector3 axis)
{
	return BulletPlugin::computeAngularImpulseDenominator(rigidBody, &axis.x);
}

bool RigidBody::wantsSleeping()
{
	return rigidBody->wantsSleeping();
}

bool RigidBody::isInWorld()
{
	return rigidBody->isInWorld();
}

void RigidBody::setAnisotropicFriction(Vector3 anisotropicFriction)
{
	BulletPlugin::setAnisotropicFriction(rigidBody, &anisotropicFriction.x);
}

Vector3 RigidBody::getAnisotropicFriction()
{
	const btVector3& friction = rigidBody->getAnisotropicFriction();
	return Vector3(friction.x(), friction.y(), friction.z());
}

bool RigidBody::hasAnisotropicFriction()
{
	return rigidBody->hasAnisotropicFriction();
}

bool RigidBody::isStaticObject()
{
	return rigidBody->isStaticObject();
}

bool RigidBody::isKinematicObject()
{
	return rigidBody->isKinematicObject();
}

bool RigidBody::isStaticOrKinematicObject()
{
	return rigidBody->isStaticOrKinematicObject();
}

ActivationState RigidBody::getActivationState()
{
	return static_cast<ActivationState>(rigidBody->getActivationState());
}

void RigidBody::setActivationState(ActivationState state)
{
	return rigidBody->setActivationState(static_cast<int>(state));
}

void RigidBody::setDeactivationTime(float time)
{
	return rigidBody->setDeactivationTime(time);
}

float RigidBody::getDeactivationTime()
{
	return rigidBody->getDeactivationTime();
}

void RigidBody::forceActivationState(ActivationState state)
{
	return rigidBody->forceActivationState(static_cast<int>(state));
}

void RigidBody::activate(bool forceActivation)
{
	return rigidBody->activate(forceActivation);
}

bool RigidBody::isActive()
{
	return rigidBody->isActive();
}

void RigidBody::setRestitution(float restitution)
{
	return rigidBody->setRestitution(restitution);
}

float RigidBody::getRestitution()
{
	return rigidBody->getRestitution();
}

void RigidBody::setFriction(float friction)
{
	return rigidBody->setFriction(friction);
}

float RigidBody::getFriction()
{
	return rigidBody->getFriction();
}

void RigidBody::setHitFraction(float fraction)
{
	return rigidBody->setHitFraction(fraction);
}

float RigidBody::getHitFraction()
{
	return rigidBody->getHitFraction();
}

CollisionFlags RigidBody::getCollisionFlags()
{
	return static_cast<CollisionFlags>(rigidBody->getCollisionFlags());
}

void RigidBody::setCollisionFlags(CollisionFlags flags)
{
	return rigidBody->setCollisionFlags(static_cast<int>(flags));
}

void RigidBody::raiseCollisionFlag(CollisionFlags flag)
{	
	rigidBody->setCollisionFlags(rigidBody->getCollisionFlags() | static_cast<int>(flag));
}

void RigidBody::clearCollisionFlag(CollisionFlags flag)
{
	int collisionFlags = rigidBody->getCollisionFlags();
	int clear = ~static_cast<int>(flag);
	rigidBody->setCollisionFlags(collisionFlags & clear);
}

void RigidBody::setLocalScaling(Vector3 scaling)
{
	BulletPlugin::setLocalScaling(rigidBody, &scaling.x);
}

Vector3 RigidBody::getLocalScaling()
{
	Vector3 ret;
	BulletPlugin::getLocalScaling(rigidBody, &ret.x);
	return ret;
}

}