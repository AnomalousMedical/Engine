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

ref class SoftBodyDefinition : public BulletElementDefinition
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

	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew SoftBodyDefinition(name);
	}

public:
	static SoftBodyDefinition()
    {
        memberScanner->ProcessFields = false;
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }

	SoftBodyDefinition(String^ name);

	virtual ~SoftBodyDefinition(void);

	virtual void registerScene(SimSubScene^ subscene, SimObjectBase^ instance) override;

	virtual EditInterface^ getEditInterface() override;


//Saving

protected:
	SoftBodyDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}