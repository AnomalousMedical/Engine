#pragma once

using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Saving;

namespace BulletPlugin
{

ref class BulletScene;

public ref class BulletElementDefinition abstract : public SimElementDefinition
{
internal:
	/// <summary>
    /// Create a new element normally as a part of scene and add it to instance.
    /// </summary>
    /// <param name="instance">The SimObject to add the element to.</param>
    /// <param name="scene">The PhysSceneManager to create the product with.</param>
    virtual void createProduct(SimObjectBase^ instance, BulletScene^ scene) = 0;

    /// <summary>
    /// Create a new element staticly as a part of scene and add it to instance.
    /// </summary>
    /// <param name="instance">The SimObject to add the element to.</param>
    /// <param name="scene">The PhysSceneManager to create the product with.</param>
    virtual void createStaticProduct(SimObjectBase^ instance, BulletScene^ scene) = 0;

public:
	BulletElementDefinition(String^ name);

//Saving

protected:
	BulletElementDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}