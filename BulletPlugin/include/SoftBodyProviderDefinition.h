#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;
using namespace Engine::Attributes;

namespace BulletPlugin
{

ref class BulletScene;
ref class SoftBodyProvider;

/// <summary>
/// This class provides the soft body to the SoftBodyDefinition. It can be
/// subclassed to provide a soft body based off whatever is required.
/// </summary>
public ref class SoftBodyProviderDefinition abstract : public SimElementDefinition
{
private:
	EditInterface^ editInterface;

	static MemberScanner^ memberScanner = gcnew MemberScanner();

	static SoftBodyProviderDefinition()
    {
        memberScanner->ProcessFields = false;
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }
protected:
	virtual SoftBodyProvider^ createProductImpl(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene) = 0;

public:
	SoftBodyProviderDefinition(String^ elementName);

	virtual ~SoftBodyProviderDefinition(void);

	virtual void registerScene(SimSubScene^ subscene, SimObjectBase^ instance) override;

	virtual EditInterface^ getEditInterface() override;

	virtual void createProduct(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene);

	virtual void createStaticProduct(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene);

//Saving

protected:
	SoftBodyProviderDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;
};

}