// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#pragma unmanaged
#include "btBulletDynamicsCommon.h"
#include "BulletSoftBody\btSoftRigidDynamicsWorld.h"
#include "BulletSoftBody\btSoftBodyRigidBodyCollisionConfiguration.h"
#pragma managed

#include "ConversionUtils.h"

namespace BulletPlugin
{
[Engine::Attributes::SingleEnum]
public enum class ActivationState : int
{
	ActiveTag = ACTIVE_TAG,
	IslandSleeping = ISLAND_SLEEPING,
	WantsDeactivation = WANTS_DEACTIVATION,
	DisableDeactivation = DISABLE_DEACTIVATION,
	DisableSimulation = DISABLE_SIMULATION,
};

[Engine::Attributes::MultiEnum]
public enum class CollisionFlags : int
{
	StaticObject = ::btCollisionObject::CF_STATIC_OBJECT,
	KinematicObject = ::btCollisionObject::CF_KINEMATIC_OBJECT,
	NoContactResponse = ::btCollisionObject::CF_NO_CONTACT_RESPONSE,
	CustomMaterialCallback = ::btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK,
	CharacterCallback = ::btCollisionObject::CF_CHARACTER_OBJECT,
};

}