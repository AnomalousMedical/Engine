#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;

namespace BulletPlugin
{

public ref class RigidBodyDefinition : SimElementDefinition
{
private:
	EditInterface^ editInterface;

	static MemberScanner^ memberScanner = gcnew MemberScanner();   

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