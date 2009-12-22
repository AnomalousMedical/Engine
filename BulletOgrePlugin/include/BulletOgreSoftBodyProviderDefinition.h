#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;
using namespace Engine::Attributes;
using namespace BulletPlugin;

namespace BulletOgrePlugin
{

public ref class BulletOgreSoftBodyProviderDefinition : public SoftBodyProviderDefinition
{
private:
	String^ meshName;
	unsigned char renderQueue;

internal:
	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew BulletOgreSoftBodyProviderDefinition(name);
	}

protected:
	virtual SoftBodyProvider^ createProductImpl(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene) override;

public:
	BulletOgreSoftBodyProviderDefinition(String^ name);

	virtual ~BulletOgreSoftBodyProviderDefinition(void);

	[Editable]
	property String^ MeshName
	{
		String^ get()
		{
			return meshName;
		}

		void set(String^ value)
		{
			meshName = value;
		}
	}

	[Editable]
	property unsigned char RenderQueue
	{
		unsigned char get()
		{
			return renderQueue;
		}

		void set(unsigned char value)
		{
			renderQueue = value;
		}
	}

	//Saving
protected:
	BulletOgreSoftBodyProviderDefinition(LoadInfo^ info);
public:
	virtual void getInfo(SaveInfo^ info) override;
};

}