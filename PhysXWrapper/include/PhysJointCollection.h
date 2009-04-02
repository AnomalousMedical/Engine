//Header
#pragma once

namespace Engine{

namespace Physics{

ref class PhysJoint;
ref class PhysJointCollection : public WrapperCollection<PhysJoint^>
{
protected:
	virtual PhysJoint^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~PhysJointCollection() {}

	PhysJoint^ getObject(NxJoint* nativeObject);

	void destroyObject(NxJoint* nativeObject);
};

}

}