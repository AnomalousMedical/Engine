#pragma once

#include "BulletElementDefinition.h"

using namespace System;
using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace Engine::Editing;
using namespace Engine::Reflection;

namespace BulletPlugin
{

ref class RigidBody;
ref class TypedConstraintElement;

ref class TypedConstraintDefinition abstract : public BulletElementDefinition
{
private:
	EditInterface^ editInterface;
	String^ rigidBodyASimObject;
	String^ rigidBodyAElement;
	String^ rigidBodyBSimObject;
	String^ rigidBodyBElement;

	static MemberScanner^ memberScanner = gcnew MemberScanner();

protected:
	virtual TypedConstraintElement^ createConstraint(RigidBody^ rbA, RigidBody^ rbB, SimObjectBase^ instance, BulletScene^ scene) = 0;

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

public:
	static TypedConstraintDefinition()
    {
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }

	TypedConstraintDefinition(String^ name);

	virtual void registerScene(SimSubScene^ subscene, SimObjectBase^ instance) override;

	virtual EditInterface^ getEditInterface() override;

	virtual property String^ JointType
	{
		virtual String^ get() = 0;
	}

	[Editable]
	property String^ RigidBodyBElement
	{
		String^ get()
		{
			return rigidBodyBElement;
		}
		void set(String^ value)
		{
			rigidBodyBElement = value;
		}
	}

	[Editable]
	property String^ RigidBodyBSimObject
	{
		String^ get()
		{
			return rigidBodyBSimObject;
		}
		void set(String^ value)
		{
			rigidBodyBSimObject = value;
		}
	}
	
	[Editable]
	property String^ RigidBodyAElement
	{
		String^ get()
		{
			return rigidBodyAElement;
		}
		void set(String^ value)
		{
			rigidBodyAElement = value;
		}
	}

	[Editable]
	property String^ RigidBodyASimObject
	{
		String^ get()
		{
			return rigidBodyASimObject;
		}
		void set(String^ value)
		{
			rigidBodyASimObject = value;
		}
	}

//Saving

protected:
	TypedConstraintDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}