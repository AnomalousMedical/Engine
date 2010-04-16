#pragma once

#include "RigidBodyDefinition.h"

namespace BulletPlugin
{

public ref class ReshapeableRigidBodyDefinition : public RigidBodyDefinition
{
internal:
	/// <summary>
    /// Create a new element normally as a part of scene and add it to instance.
    /// </summary>
    /// <param name="instance">The SimObject to add the element to.</param>
    /// <param name="scene">The PhysSceneManager to create the product with.</param>
    virtual void createProduct(SimObjectBase^ instance, BulletScene^ scene) override;

	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew ReshapeableRigidBodyDefinition(name);
	}

public:
	ReshapeableRigidBodyDefinition(String^ name);

	virtual ~ReshapeableRigidBodyDefinition(void);

	virtual EditInterface^ getEditInterface() override;

	//Saving

protected:
	ReshapeableRigidBodyDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}