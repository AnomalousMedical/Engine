//Header
#pragma once

namespace Physics{

ref class PhysJoint;
ref class PhysActor;
ref class PhysScene;
ref class PhysJointCollection : public WrapperCollection<PhysJoint^>
{
protected:
	virtual PhysJoint^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~PhysJointCollection() {}

	PhysJoint^ getObject(NxJoint* nativeObject, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	void destroyObject(NxJoint* nativeObject);
};

}