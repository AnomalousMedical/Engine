#pragma once

#include "SoftBodyProviderDefinition.h"

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;
using namespace Engine::Attributes;

namespace BulletPlugin
{

public ref class EllipsoidSoftBodyProviderDefinition : public SoftBodyProviderDefinition
{
internal:
	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew EllipsoidSoftBodyProviderDefinition(name);
	}

protected:
	virtual SoftBodyProvider^ createProductImpl(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene) override;

public:
	EllipsoidSoftBodyProviderDefinition(String^ name);

	virtual ~EllipsoidSoftBodyProviderDefinition(void);

	//Saving
protected:
	EllipsoidSoftBodyProviderDefinition(LoadInfo^ info);
public:
	virtual void getInfo(SaveInfo^ info) override;
};

}