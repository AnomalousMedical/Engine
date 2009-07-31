#pragma once

#include "BulletElementDefinition.h"

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;

namespace BulletPlugin
{

public ref class RigidBodyDefinition : public BulletElementDefinition
{
private:
	EditInterface^ editInterface;

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

public:
	static RigidBodyDefinition()
    {
        memberScanner->ProcessFields = false;
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }

	RigidBodyDefinition(String^ name);

	virtual void registerScene(SimSubScene^ subscene, SimObjectBase^ instance) override;

	virtual EditInterface^ getEditInterface() override;
};

}