#include "StdAfx.h"
#include "..\include\RigidBody.h"
#include "MotionState.h"
#include "RigidBodyDefinition.h"
#include "BulletScene.h"

namespace BulletPlugin
{

#pragma unmanaged

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

#pragma managed

RigidBody::RigidBody(RigidBodyDefinition^ description, BulletScene^ scene)
:SimElement(description->Name, description->Subscription),
scene(scene)
{
	motionState = new MotionState(this);
	description->ConstructionInfo->m_motionState = motionState;
	rigidBody = new btRigidBody(*description->ConstructionInfo);
	setLinearVelocity(rigidBody, &description->LinearVelocity.x);
	setAngularVelocity(rigidBody, &description->AngularVelocity.x);
	rigidBody->forceActivationState(static_cast<int>(description->CurrentActivationState));
	setAnisotropicFriction(rigidBody, &description->AnisotropicFriction.x);
	rigidBody->setDeactivationTime(description->DeactivationTime);
	rigidBody->setCollisionFlags(static_cast<int>(description->Flags));
	rigidBody->setHitFraction(description->HitFraction);
}

RigidBody::~RigidBody(void)
{
	if(rigidBody != 0)
	{
		if(Owner->Enabled)
		{
			scene->DynamicsWorld->removeRigidBody(rigidBody);
		}
		delete rigidBody;
		rigidBody = 0;
		delete motionState;
		motionState = 0;
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
		scene->DynamicsWorld->addRigidBody(rigidBody);
	}
	else
	{
		scene->DynamicsWorld->removeRigidBody(rigidBody);
	}
}

SimElementDefinition^ RigidBody::saveToDefinition()
{
	RigidBodyDefinition^ definition = gcnew RigidBodyDefinition(Name);
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
	return definition;
}

void RigidBody::setWorldTransform(Vector3 translation, Quaternion rotation)
{
	BulletPlugin::setWorldTransform(rigidBody, &translation.x, &rotation.x);
	motionState->setStartingTransform(&translation.x, &rotation.x);
}

}