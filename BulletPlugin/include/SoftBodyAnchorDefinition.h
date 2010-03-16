#pragma once

#include "BulletElementDefinition.h"
#include "AutoPtr.h"

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;
using namespace Engine::Attributes;

namespace BulletPlugin
{

ref class SoftBodyAnchorDefinition : public BulletElementDefinition
{
private:
	static MemberScanner^ memberScanner = gcnew MemberScanner();
	EditInterface^ editInterface;

	String^ rigidBodySimObject;
	String^ rigidBodyElement;
	String^ softBodySimObject;
	String^ softBodyElement;
	bool disableCollisionBetweenNodes;

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

	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew SoftBodyAnchorDefinition(name);
	}

public:
	static SoftBodyAnchorDefinition()
    {
        memberScanner->ProcessFields = false;
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }

	SoftBodyAnchorDefinition(System::String^ name);

	virtual ~SoftBodyAnchorDefinition(void);

	virtual void registerScene(SimSubScene^ subscene, SimObjectBase^ instance) override;

	virtual EditInterface^ getEditInterface() override;

	[Editable]
	property bool DisableCollisionBetweenNodes
	{
		bool get()
		{
			return disableCollisionBetweenNodes;
		}
		void set(bool value)
		{
			disableCollisionBetweenNodes = value;
		}
	}

	[Editable]
	property String^ SoftBodyElement
	{
		String^ get()
		{
			return softBodyElement;
		}
		void set(String^ value)
		{
			softBodyElement = value;
		}
	}

	[Editable]
	property String^ SoftBodySimObject
	{
		String^ get()
		{
			return softBodySimObject;
		}
		void set(String^ value)
		{
			softBodySimObject = value;
		}
	}

	[Editable]
	property String^ RigidBodyElement
	{
		String^ get()
		{
			return rigidBodyElement;
		}
		void set(String^ value)
		{
			rigidBodyElement = value;
		}
	}

	[Editable]
	property String^ RigidBodySimObject
	{
		String^ get()
		{
			return rigidBodySimObject;
		}
		void set(String^ value)
		{
			rigidBodySimObject = value;
		}
	}

//Saving
protected:
	SoftBodyAnchorDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}