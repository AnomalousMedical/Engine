#pragma once

#include "BulletElementDefinition.h"
#include "AutoPtr.h"

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;

namespace BulletPlugin
{

[Engine::Attributes::NativeSubsystemType]
public ref class RigidBodyDefinition : public BulletElementDefinition
{
private:
	Vector3 linearVelocity;
	Vector3 angularVelocity;
	ActivationState activationState;
	EditInterface^ editInterface;
	btRigidBody::btRigidBodyConstructionInfo* constructionInfo;
	Vector3 anisotropicFriction;
	float deactivationTime;
	CollisionFlags flags;
	float hitFraction;
	String^ shapeName;
	float maxContactDistance;
	short collisionFilterMask;
	short collisionFilterGroup;

	static MemberScanner^ memberScanner = gcnew MemberScanner();

internal:
	/// <summary>
    /// Create a new element normally as a part of scene and add it to instance.
    /// </summary>
    /// <param name="instance">The SimObject to add the element to.</param>
    /// <param name="scene">The PhysSceneManager to create the product with.</param>
    virtual void createProduct(SimObjectBase^ instance, BulletScene^ scene) override;

    /// <summary>
    /// Create a new element staticly as a part of scene and add it to instance.
    /// </summary>
    /// <param name="instance">The SimObject to add the element to.</param>
    /// <param name="scene">The PhysSceneManager to create the product with.</param>
    virtual void createStaticProduct(SimObjectBase^ instance, BulletScene^ scene) override;

	property btRigidBody::btRigidBodyConstructionInfo* ConstructionInfo
	{
		btRigidBody::btRigidBodyConstructionInfo* get()
		{
			return constructionInfo;
		}
	}

	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew RigidBodyDefinition(name);
	}

public:
	static RigidBodyDefinition()
    {
        memberScanner->ProcessFields = false;
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }

	RigidBodyDefinition(String^ name);

	virtual void registerScene(SimSubScene^ subscene, SimObjectBase^ instance) override;

	virtual EditInterface^ getEditInterface() override;

	[Editable]
	property short CollisionFilterMask
	{
		short get()
		{
			return collisionFilterMask;
		}
		void set(short value)
		{
			collisionFilterMask = value;
		}
	}

	[Editable]
	property short CollisionFilterGroup
	{
		short get()
		{
			return collisionFilterGroup;
		}
		void set(short value)
		{
			collisionFilterGroup = value;
		}
	}

	[Editable]
	property float HitFraction
	{
		float get()
		{
			return hitFraction;
		}
		void set(float value)
		{
			hitFraction = value;
		}
	}

	[Editable]
	property CollisionFlags Flags
	{
		CollisionFlags get()
		{
			return flags;
		}
		void set(CollisionFlags value)
		{
			flags = value;
		}
	}

	[Editable]
	property float DeactivationTime
	{
		float get()
		{
			return deactivationTime;
		}
		void set(float value)
		{
			deactivationTime = value;
		}
	}

	[Editable]
	property Vector3 AnisotropicFriction
	{
		Vector3 get()
		{
			return anisotropicFriction;
		}
		void set(Vector3 value)
		{
			anisotropicFriction = value;
		}
	}

	[Editable]
	property ActivationState CurrentActivationState
	{
		ActivationState get()
		{
			return activationState;
		}
		void set(ActivationState value)
		{
			activationState = value;
		}
	}

	[Editable]
	property Vector3 AngularVelocity
	{
		Vector3 get()
		{
			return angularVelocity;
		}
		void set(Vector3 value)
		{
			angularVelocity = value;
		}
	}

	[Editable]
	property Vector3 LinearVelocity
	{
		Vector3 get()
		{
			return linearVelocity;
		}
		void set(Vector3 value)
		{
			linearVelocity = value;
		}
	}

	[Editable]
	property float AngularSleepingThreshold
	{
		float get()
		{
			return constructionInfo->m_angularSleepingThreshold;
		}
		void set(float value)
		{
			constructionInfo->m_angularSleepingThreshold = value;
		}
	}

	[Editable]
	property float LinearSleepingThreshold
	{
		float get()
		{
			return constructionInfo->m_linearSleepingThreshold;
		}
		void set(float value)
		{
			constructionInfo->m_linearSleepingThreshold = value;
		}
	}

	[Editable]
	property float Restitution
	{
		float get()
		{
			return constructionInfo->m_restitution;
		}
		void set(float value)
		{
			constructionInfo->m_restitution = value;
		}
	}

	[Editable]
	property float Friction
	{
		float get()
		{
			return constructionInfo->m_friction;
		}
		void set(float value)
		{
			constructionInfo->m_friction = value;
		}
	}

	[Editable]
	property float AngularDamping
	{
		float get()
		{
			return constructionInfo->m_angularDamping;
		}
		void set(float value)
		{
			constructionInfo->m_angularDamping = value;
		}
	}

	[Editable]
	property float LinearDamping
	{
		float get()
		{
			return constructionInfo->m_linearDamping;
		}
		void set(float value)
		{
			constructionInfo->m_linearDamping = value;
		}
	}

	[Editable]
	property float MaxContactDistance
	{
		float get()
		{
			return maxContactDistance;
		}
		void set(float value)
		{
			maxContactDistance = value;
		}
	}

	[Editable]
	property float Mass
	{
		float get()
		{
			return constructionInfo->m_mass;
		}
		void set(float value)
		{
			constructionInfo->m_mass = value;
		}
	}

	[Editable]
	property String^ ShapeName
	{
		String^ get()
		{
			return shapeName;
		}
		void set(String^ value)
		{
			shapeName = value;
		}
	}

//Saving

protected:
	RigidBodyDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}

/*
[Editable]
	property float AdditionalAngularDampingFactor
	{
		float get()
		{
			return constructionInfo->m_additionalAngularDampingFactor;
		}
		void set(float value)
		{
			constructionInfo->m_additionalAngularDampingFactor = value;
		}
	}

	[Editable]
	property float AdditionalAngularDampingThresholdSqr
	{
		float get()
		{
			return constructionInfo->m_additionalAngularDampingThresholdSqr;
		}
		void set(float value)
		{
			constructionInfo->m_additionalAngularDampingThresholdSqr = value;
		}
	}

	[Editable]
	property float AdditionalLinearDampingThresholdSqr
	{
		float get()
		{
			return constructionInfo->m_additionalLinearDampingThresholdSqr;
		}
		void set(float value)
		{
			constructionInfo->m_additionalLinearDampingThresholdSqr = value;
		}
	}

	[Editable]
	property float AdditionalDampingFactor
	{
		float get()
		{
			return constructionInfo->m_additionalDampingFactor;
		}
		void set(float value)
		{
			constructionInfo->m_additionalDampingFactor = value;
		}
	}

	[Editable]
	property bool AdditionalDamping
	{
		bool get()
		{
			return constructionInfo->m_additionalDamping;
		}
		void set(bool value)
		{
			constructionInfo->m_additionalDamping = value;
		}
	}
*/