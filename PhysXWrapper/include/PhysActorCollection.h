//Header
#pragma once

namespace Physics{

ref class PhysActor;
ref class PhysActorCollection : public WrapperCollection<PhysActor^>
{
protected:
	virtual PhysActor^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~PhysActorCollection() {}

	PhysActor^ getObject(NxActor* nativeObject);

	void destroyObject(NxActor* nativeObject);
};

}