//Header
#pragma once

namespace PhysXWrapper
{

ref class PhysScene;
ref class PhysSceneCollection : public WrapperCollection<PhysScene^>
{
protected:
	virtual PhysScene^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~PhysSceneCollection() {}

	PhysScene^ getObject(NxScene* nativeObject);

	void destroyObject(NxScene* nativeObject);
};

}