#include "Stdafx.h"
#include "../Include/RigidBodyConstructionInfo.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Ignore 4190 returning c++ classes from extern "c"

extern "C" _AnomalousExport btRigidBody* btRigidBody_Create(RigidBodyConstructionInfo* constructionInfo, btMotionState* motionState, btCollisionShape* collisionShape)
{
	btRigidBody::btRigidBodyConstructionInfo info(0, 0, 0);
	constructionInfo->toBullet(info, motionState, collisionShape);
	return new btRigidBody(info);
}

extern "C" _AnomalousExport void btRigidBody_Delete(btRigidBody* instance)
{
	delete instance;
}

extern "C" _AnomalousExport void btRigidBody_setDamping(btRigidBody* instance, float linearDamping, float angularDamping)
{
	instance->setDamping(linearDamping, angularDamping);
}

extern "C" _AnomalousExport float btRigidBody_getLinearDamping(btRigidBody* instance)
{
	return instance->getLinearDamping();
}

extern "C" _AnomalousExport float btRigidBody_getAngularDamping(btRigidBody* instance)
{
	return instance->getAngularDamping();
}

extern "C" _AnomalousExport float btRigidBody_getLinearSleepingThreshold(btRigidBody* instance)
{
	return instance->getLinearSleepingThreshold();
}

extern "C" _AnomalousExport float btRigidBody_getAngularSleepingThreshold(btRigidBody* instance)
{
	return instance->getAngularSleepingThreshold();
}

extern "C" _AnomalousExport void btRigidBody_setMassProps(btRigidBody* instance, float mass)
{
	btVector3 inertia = instance->getInvInertiaDiagLocal();
	inertia.setX(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.x(): btScalar(0.0));
	inertia.setY(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.y(): btScalar(0.0));
	inertia.setZ(inertia.x() != btScalar(0.0) ? btScalar(1.0) / inertia.z(): btScalar(0.0));
	instance->setMassProps(mass, inertia);
}

extern "C" _AnomalousExport void btRigidBody_setMassPropsInertia(btRigidBody* instance, float mass, Vector3* inertia)
{
	instance->setMassProps(mass, inertia->toBullet());
}

extern "C" _AnomalousExport float btRigidBody_getInvMass(btRigidBody* instance)
{
	return instance->getInvMass();
}

extern "C" _AnomalousExport void btRigidBody_applyCentralForce(btRigidBody* instance, Vector3* force)
{
	return instance->applyCentralForce(force->toBullet());
}

extern "C" _AnomalousExport Vector3 btRigidBody_getTotalForce(btRigidBody* instance)
{
	return Vector3(instance->getTotalForce());
}

extern "C" _AnomalousExport Vector3 btRigidBody_getTotalTorque(btRigidBody* instance)
{
	return Vector3(instance->getTotalTorque());
}

extern "C" _AnomalousExport void btRigidBody_setSleepingThresholds(btRigidBody* instance, float linear, float angular)
{
	instance->setSleepingThresholds(linear, angular);
}

extern "C" _AnomalousExport void btRigidBody_applyTorque(btRigidBody* instance, Vector3* torque)
{
	instance->applyTorque(torque->toBullet());
}

extern "C" _AnomalousExport void btRigidBody_applyForce(btRigidBody* instance, Vector3* force, Vector3* rel_pos)
{
	instance->applyForce(force->toBullet(), rel_pos->toBullet());
}

extern "C" _AnomalousExport void btRigidBody_applyCentralImpulse(btRigidBody* instance, Vector3* impulse)
{
	instance->applyCentralImpulse(impulse->toBullet());
}

extern "C" _AnomalousExport void btRigidBody_applyTorqueImpulse(btRigidBody* instance, Vector3* torque)
{
	instance->applyTorqueImpulse(torque->toBullet());
}

extern "C" _AnomalousExport void btRigidBody_clearForces(btRigidBody* instance)
{
	instance->clearForces();
}

extern "C" _AnomalousExport Vector3 btRigidBody_getCenterOfMassPosition(btRigidBody* instance)
{
	return Vector3(instance->getCenterOfMassPosition());
}

extern "C" _AnomalousExport Vector3 btRigidBody_getLinearVelocity(btRigidBody* instance)
{
	return Vector3(instance->getLinearVelocity());
}

extern "C" _AnomalousExport Vector3 btRigidBody_getAngularVelocity(btRigidBody* instance)
{
	return Vector3(instance->getAngularVelocity());
}

extern "C" _AnomalousExport void btRigidBody_setLinearVelocity(btRigidBody* instance, Vector3* lin_vel)
{
	instance->setLinearVelocity(lin_vel->toBullet());
}

extern "C" _AnomalousExport void btRigidBody_setAngularVelocity(btRigidBody* instance, Vector3* ang_vel)
{
	instance->setAngularVelocity(ang_vel->toBullet());
}

extern "C" _AnomalousExport Vector3 btRigidBody_getVelocityInLocalPoint(btRigidBody* instance, Vector3* rel_pos)
{
	return Vector3(instance->getVelocityInLocalPoint(rel_pos->toBullet()));
}

extern "C" _AnomalousExport void btRigidBody_translate(btRigidBody* instance, Vector3* v)
{
	instance->translate(v->toBullet());
}

extern "C" _AnomalousExport void btRigidBody_getAabb(btRigidBody* instance, Vector3* aabbMin, Vector3* aabbMax)
{
	//instance->getAabb(aabbMin->toBullet(), aabbMax->toBullet());
}

extern "C" _AnomalousExport float btRigidBody_computeImpulseDenominator(btRigidBody* instance, Vector3* pos, Vector3* normal)
{
	return instance->computeImpulseDenominator(pos->toBullet(), normal->toBullet());
}

extern "C" _AnomalousExport float btRigidBody_computeAngularImpulseDenominator(btRigidBody* instance, Vector3* axis)
{
	return instance->computeAngularImpulseDenominator(axis->toBullet());
}

extern "C" _AnomalousExport bool btRigidBody_wantsSleeping(btRigidBody* instance)
{
	return instance->wantsSleeping();
}

extern "C" _AnomalousExport bool btRigidBody_isInWorld(btRigidBody* instance)
{
	return instance->isInWorld();
}

extern "C" _AnomalousExport void btRigidBody_setAnisotropicFriction(btRigidBody* instance, Vector3* anisotropicFriction)
{
	return instance->setAnisotropicFriction(anisotropicFriction->toBullet());
}

extern "C" _AnomalousExport Vector3 btRigidBody_getAnisotropicFriction(btRigidBody* instance)
{
	return Vector3(instance->getAnisotropicFriction());
}

extern "C" _AnomalousExport bool btRigidBody_hasAnisotropicFriction(btRigidBody* instance)
{
	return instance->hasAnisotropicFriction();
}

extern "C" _AnomalousExport bool btRigidBody_isStaticObject(btRigidBody* instance)
{
	return instance->isStaticObject();
}

extern "C" _AnomalousExport bool btRigidBody_isKinematicObject(btRigidBody* instance)
{
	return instance->isKinematicObject();
}

extern "C" _AnomalousExport bool btRigidBody_isStaticOrKinematicObject(btRigidBody* instance)
{
	return instance->isStaticOrKinematicObject();
}

extern "C" _AnomalousExport int btRigidBody_getActivationState(btRigidBody* instance)
{
	return instance->getActivationState();
}

extern "C" _AnomalousExport void btRigidBody_setActivationState(btRigidBody* instance, int state)
{
	instance->setActivationState(state);
}

extern "C" _AnomalousExport void btRigidBody_setDeactivationTime(btRigidBody* instance, float time)
{
	instance->setDeactivationTime(time);
}

extern "C" _AnomalousExport float btRigidBody_getDeactivationTime(btRigidBody* instance)
{
	return instance->getDeactivationTime();
}

extern "C" _AnomalousExport void btRigidBody_forceActivationState(btRigidBody* instance, int state)
{
	instance->forceActivationState(state);
}

extern "C" _AnomalousExport void btRigidBody_activate(btRigidBody* instance, bool forceActivation)
{
	instance->activate();
}

extern "C" _AnomalousExport bool btRigidBody_isActive(btRigidBody* instance)
{
	return instance->isActive();
}

extern "C" _AnomalousExport void btRigidBody_setRestitution(btRigidBody* instance, float restitution)
{
	instance->setRestitution(restitution);
}

extern "C" _AnomalousExport float btRigidBody_getRestitution(btRigidBody* instance)
{
	return instance->getRestitution();
}

extern "C" _AnomalousExport void btRigidBody_setFriction(btRigidBody* instance, float friction)
{
	instance->setFriction(friction);
}

extern "C" _AnomalousExport float btRigidBody_getFriction(btRigidBody* instance)
{
	return instance->getFriction();
}

extern "C" _AnomalousExport void btRigidBody_setHitFraction(btRigidBody* instance, float fraction)
{
	instance->setHitFraction(fraction);
}

extern "C" _AnomalousExport float btRigidBody_getHitFraction(btRigidBody* instance)
{
	return instance->getHitFraction();
}

extern "C" _AnomalousExport int btRigidBody_getCollisionFlags(btRigidBody* instance)
{
	return instance->getCollisionFlags();
}

extern "C" _AnomalousExport void btRigidBody_setCollisionFlags(btRigidBody* instance, int flags)
{
	instance->setCollisionFlags(flags);
}

extern "C" _AnomalousExport void btRigidBody_setLocalScaling(btRigidBody* instance, Vector3* scaling)
{
	instance->getCollisionShape()->setLocalScaling(scaling->toBullet());
}

extern "C" _AnomalousExport Vector3 btRigidBody_getLocalScaling(btRigidBody* instance)
{
	return Vector3(instance->getCollisionShape()->getLocalScaling());
}

extern "C" _AnomalousExport void btRigidBody_setWorldTranslation(btRigidBody* rigidBody, Vector3* trans)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getOrigin().setX(trans->x);
	transform.getOrigin().setY(trans->y);
	transform.getOrigin().setZ(trans->z);
}

extern "C" _AnomalousExport void btRigidBody_setWorldRotation(btRigidBody* rigidBody, Quaternion* rot)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getBasis().setRotation(btQuaternion(rot->x, rot->y, rot->z, rot->w));
}

extern "C" _AnomalousExport void btRigidBody_setWorldTransform(btRigidBody* rigidBody, Vector3* trans, Quaternion* rot)
{
	btTransform& transform = rigidBody->getWorldTransform();
	transform.getOrigin().setX(trans->x);
	transform.getOrigin().setY(trans->y);
	transform.getOrigin().setZ(trans->z);
	transform.getBasis().setRotation(btQuaternion(rot->x, rot->y, rot->z, rot->w));
}

extern "C" _AnomalousExport int btRigidBody_getNumConstraintRefs(btRigidBody* rigidBody)
{
	return rigidBody->getNumConstraintRefs();
}

extern "C" _AnomalousExport btTypedConstraint* btRigidBody_getConstraintRef(btRigidBody* rigidBody, int num)
{
	return rigidBody->getConstraintRef(num);
}

#pragma warning(pop)