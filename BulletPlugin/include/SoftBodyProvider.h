#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;
using namespace Engine::Attributes;

class btSoftBody;

namespace BulletPlugin
{

ref class BulletScene;
ref class SoftBodyProviderDefinition;

public ref class SoftBodyProvider abstract : public SimElement
{
protected:
	virtual btSoftBody* createSoftBodyImpl(BulletScene^ scene) = 0;

	virtual void destroySoftBodyImpl(BulletScene^ scene) = 0;

internal:
	btSoftBody* createSoftBody(BulletScene^ scene);

	void destroySoftBody(BulletScene^ scene);

public:
	SoftBodyProvider(SoftBodyProviderDefinition^ description);

	virtual ~SoftBodyProvider(void);

	virtual void updateOtherSubsystems() = 0;
};

}