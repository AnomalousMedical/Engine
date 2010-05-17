#include "Stdafx.h"
#include "..\Include\RigidBodyConstructionInfo.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Ignore 4190 returning c++ classes from extern "c"

extern "C" _declspec(dllexport) btRigidBody* btRigidBody_Create(RigidBodyConstructionInfo* constructionInfo, btMotionState* motionState, btCollisionShape* collisionShape)
{
	btRigidBody::btRigidBodyConstructionInfo info(0, 0, 0);
	constructionInfo->toBullet(info, motionState, collisionShape);
	return new btRigidBody(info);
}

extern "C" _declspec(dllexport) void btRigidBody_Delete(btRigidBody* instance)
{
	delete instance;
}

extern "C" _declspec(dllexport) void btRigidBody_setDamping(btRigidBody* instance, float linearDamping, float angularDamping)
{
	instance->setDamping(linearDamping, angularDamping);
}

extern "C" _declspec(dllexport) float btRigidBody_getLinearDamping(btRigidBody* instance)
{
	return instance->getLinearDamping();
}

extern "C" _declspec(dllexport) float btRigidBody_getAngularDamping(btRigidBody* instance)
{
	return instance->getAngularDamping();
}

extern "C" _declspec(dllexport) float btRigidBody_getLinearSleepingThreshold(btRigidBody* instance)
{
	return instance->getLinearSleepingThreshold();
}

extern "C" _declspec(dllexport) float btRigidBody_getAngularSleepingThreshold(btRigidBody* instance)
{
	return instance->getAngularSleepingThreshold();
}

extern "C" _declspec(dllexport) void btRigidBody_setMassProps(btRigidBody* instance, float mass)
{
	btVector3 inertia = instance->getInvInertiaDiagLocal();
	inertia.setX(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.x(): btScalar(0.0));
	inertia.setY(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.y(): btScalar(0.0));
	inertia.setZ(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.z(): btScalar(0.0));
	instance->setMassProps(mass, inertia);
}

extern "C" _declspec(dllexport) void btRigidBody_setMassPropsInertia(btRigidBody* instance, float mass, Vector3* inertia)
{
	instance->setMassProps(mass, inertia->toBullet());
}

extern "C" _declspec(dllexport) float btRigidBody_getInvMass(btRigidBody* instance)
{
	return instance->getInvMass();
}

extern "C" _declspec(dllexport) void btRigidBody_applyCentralForce(btRigidBody* instance, Vector3* force)
{
	return instance->applyCentralForce(force->toBullet());
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getTotalForce(btRigidBody* instance)
{
	return Vector3(instance->getTotalForce());
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getTotalTorque(btRigidBody* instance)
{
	return Vector3(instance->getTotalTorque());
}

extern "C" _declspec(dllexport) void btRigidBody_setSleepingThresholds(btRigidBody* instance, float linear, float angular)
{
	instance->setSleepingThresholds(linear, angular);
}

extern "C" _declspec(dllexport) void btRigidBody_applyTorque(btRigidBody* instance, Vector3* torque)
{
	instance->applyTorque(torque->toBullet());
}

extern "C" _declspec(dllexport) void btRigidBody_applyForce(btRigidBody* instance, Vector3* force, Vector3* rel_pos)
{
	instance->applyForce(force->toBullet(), rel_pos->toBullet());
}

extern "C" _declspec(dllexport) void btRigidBody_applyCentralImpulse(btRigidBody* instance, Vector3* impulse)
{
	instance->applyCentralImpulse(impulse->toBullet());
}

extern "C" _declspec(dllexport) void btRigidBody_applyTorqueImpulse(btRigidBody* instance, Vector3* torque)
{
	instance->applyTorqueImpulse(torque->toBullet());
}

extern "C" _declspec(dllexport) void btRigidBody_clearForces(btRigidBody* instance)
{
	instance->clearForces();
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getCenterOfMassPosition(btRigidBody* instance)
{
	return Vector3(instance->getCenterOfMassPosition());
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getLinearVelocity(btRigidBody* instance)
{
	return Vector3(instance->getLinearVelocity());
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getAngularVelocity(btRigidBody* instance)
{
	return Vector3(instance->getAngularVelocity());
}

extern "C" _declspec(dllexport) void btRigidBody_setLinearVelocity(btRigidBody* instance, Vector3* lin_vel)
{
	instance->setLinearVelocity(lin_vel->toBullet());
}

extern "C" _declspec(dllexport) void btRigidBody_setAngularVelocity(btRigidBody* instance, Vector3* ang_vel)
{
	instance->setAngularVelocity(ang_vel->toBullet());
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getVelocityInLocalPoint(btRigidBody* instance, Vector3* rel_pos)
{
	return Vector3(instance->getVelocityInLocalPoint(rel_pos->toBullet()));
}

extern "C" _declspec(dllexport) void btRigidBody_translate(btRigidBody* instance, Vector3* v)
{
	instance->translate(v->toBullet());
}

extern "C" _declspec(dllexport) void btRigidBody_getAabb(btRigidBody* instance, Vector3* aabbMin, Vector3* aabbMax)
{
	instance->getAabb(aabbMin->toBullet(), aabbMax->toBullet());
}

extern "C" _declspec(dllexport) float btRigidBody_computeImpulseDenominator(btRigidBody* instance, Vector3* pos, Vector3* normal)
{
	return instance->computeImpulseDenominator(pos->toBullet(), normal->toBullet());
}

extern "C" _declspec(dllexport) float btRigidBody_computeAngularImpulseDenominator(btRigidBody* instance, Vector3* axis)
{
	return instance->computeAngularImpulseDenominator(axis->toBullet());
}

extern "C" _declspec(dllexport) bool btRigidBody_wantsSleeping(btRigidBody* instance)
{
	return instance->wantsSleeping();
}

extern "C" _declspec(dllexport) bool btRigidBody_isInWorld(btRigidBody* instance)
{
	return instance->isInWorld();
}

extern "C" _declspec(dllexport) void btRigidBody_setAnisotropicFriction(btRigidBody* instance, Vector3* anisotropicFriction)
{
	return instance->setAnisotropicFriction(anisotropicFriction->toBullet());
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getAnisotropicFriction(btRigidBody* instance)
{
	return Vector3(instance->getAnisotropicFriction());
}

extern "C" _declspec(dllexport) bool btRigidBody_hasAnisotropicFriction(btRigidBody* instance)
{
	return instance->hasAnisotropicFriction();
}

extern "C" _declspec(dllexport) bool btRigidBody_isStaticObject(btRigidBody* instance)
{
	return instance->isStaticObject();
}

extern "C" _declspec(dllexport) bool btRigidBody_isKinematicObject(btRigidBody* instance)
{
	return instance->isKinematicObject();
}

extern "C" _declspec(dllexport) bool btRigidBody_isStaticOrKinematicObject(btRigidBody* instance)
{
	return instance->isStaticOrKinematicObject();
}

extern "C" _declspec(dllexport) int btRigidBody_getActivationState(btRigidBody* instance)
{
	return instance->getActivationState();
}

extern "C" _declspec(dllexport) void btRigidBody_setActivationState(btRigidBody* instance, int state)
{
	instance->setActivationState(state);
}

extern "C" _declspec(dllexport) void btRigidBody_setDeactivationTime(btRigidBody* instance, float time)
{
	instance->setDeactivationTime(time);
}

extern "C" _declspec(dllexport) float btRigidBody_getDeactivationTime(btRigidBody* instance)
{
	return instance->getDeactivationTime();
}

extern "C" _declspec(dllexport) void btRigidBody_forceActivationState(btRigidBody* instance, int state)
{
	instance->forceActivationState(state);
}

extern "C" _declspec(dllexport) void btRigidBody_activate(btRigidBody* instance, bool forceActivation)
{
	instance->activate();
}

extern "C" _declspec(dllexport) bool btRigidBody_isActive(btRigidBody* instance)
{
	return instance->isActive();
}

extern "C" _declspec(dllexport) void btRigidBody_setRestitution(btRigidBody* instance, float restitution)
{
	instance->setRestitution(restitution);
}

extern "C" _declspec(dllexport) float btRigidBody_getRestitution(btRigidBody* instance)
{
	return instance->getRestitution();
}

extern "C" _declspec(dllexport) void btRigidBody_setFriction(btRigidBody* instance, float friction)
{
	instance->setFriction(friction);
}

extern "C" _declspec(dllexport) float btRigidBody_getFriction(btRigidBody* instance)
{
	return instance->getFriction();
}

extern "C" _declspec(dllexport) void btRigidBody_setHitFraction(btRigidBody* instance, float fraction)
{
	instance->setHitFraction(fraction);
}

extern "C" _declspec(dllexport) float btRigidBody_getHitFraction(btRigidBody* instance)
{
	return instance->getHitFraction();
}

extern "C" _declspec(dllexport) int btRigidBody_getCollisionFlags(btRigidBody* instance)
{
	return instance->getCollisionFlags();
}

extern "C" _declspec(dllexport) void btRigidBody_setCollisionFlags(btRigidBody* instance, int flags)
{
	instance->setCollisionFlags(flags);
}

extern "C" _declspec(dllexport) void btRigidBody_setLocalScaling(btRigidBody* instance, Vector3* scaling)
{
	instance->getCollisionShape()->setLocalScaling(scaling->toBullet());
}

extern "C" _declspec(dllexport) Vector3 btRigidBody_getLocalScaling(btRigidBody* instance)
{
	return Vector3(instance->getCollisionShape()->getLocalScaling());
}

extern "C" _declspec(dllexport) void btRigidBody_setWorldTranslation(btRigidBody* rigidBody, Vector3* trans)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getOrigin().setX(trans->x);
	transform.getOrigin().setY(trans->y);
	transform.getOrigin().setZ(trans->z);
}

extern "C" _declspec(dllexport) void btRigidBody_setWorldRotation(btRigidBody* rigidBody, Quaternion* rot)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getBasis().setRotation(btQuaternion(rot->x, rot->y, rot->z, rot->w));
}

extern "C" _declspec(dllexport) void btRigidBody_setWorldTransform(btRigidBody* rigidBody, Vector3* trans, Quaternion* rot)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getOrigin().setX(trans->x);
	transform.getOrigin().setY(trans->y);
	transform.getOrigin().setZ(trans->z);
	transform.getBasis().setRotation(btQuaternion(rot->x, rot->y, rot->z, rot->w));
}

#pragma warning(pop)