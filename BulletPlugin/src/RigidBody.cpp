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

#pragma managed

RigidBody::RigidBody(RigidBodyDefinition^ description, BulletScene^ scene)
:SimElement(description->Name, description->Subscription),
scene(scene)
{
	motionState = new MotionState(this);
	description->ConstructionInfo->m_motionState = motionState;
	rigidBody = new btRigidBody(*description->ConstructionInfo);
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
	definition->Mass = 1.0f / rigidBody->getInvMass();
	return definition;
}

void RigidBody::setWorldTransform(Vector3 translation, Quaternion rotation)
{
	BulletPlugin::setWorldTransform(rigidBody, &translation.x, &rotation.x);
}

}