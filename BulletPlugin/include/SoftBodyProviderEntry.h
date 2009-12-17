#pragma once

using namespace Engine::ObjectManagement;

namespace BulletPlugin
{

ref class SoftBodyProviderDefinition;
ref class BulletScene;

ref class SoftBodyProviderEntry
{
private:
	SimObjectBase^ instance;
	SoftBodyProviderDefinition^ element;
	SimSubScene^ subScene;

public:
	SoftBodyProviderEntry(SimObjectBase^ instance, SoftBodyProviderDefinition^ element, SimSubScene^ subScene);

	virtual ~SoftBodyProviderEntry(void);

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