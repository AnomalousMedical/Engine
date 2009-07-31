#pragma once

using namespace Engine::ObjectManagement;

namespace BulletPlugin
{

ref class BulletElementDefinition;
ref class BulletScene;

ref class BulletFactoryEntry
{
private:
	SimObjectBase^ instance;
	BulletElementDefinition^ element;

public:
	BulletFactoryEntry(SimObjectBase^ instance, BulletElementDefinition^ element);

	/// <summary>
    /// Build the product normally.
    /// </summary>
    /// <param name="scene">The scene to add the product to.</param>
    void createProduct(BulletScene^ scene);

    /// <summary>
    /// Build the static version of the product.
    /// </summary>
    /// <param name="scene">The scene to add the product to.</param>
    void createStaticProduct(BulletScene^ scene);
};

}