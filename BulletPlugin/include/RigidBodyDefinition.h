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

public ref class RigidBodyDefinition : public BulletElementDefinition
{
private:
	EditInterface^ editInterface;
	btRigidBody::btRigidBodyConstructionInfo* constructionInfo;

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

//Saving

protected:
	RigidBodyDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}