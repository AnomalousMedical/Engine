//Header
#pragma once

namespace PhysXWrapper{

ref class PhysSoftBody;
ref class PhysSoftBodyCollection : public WrapperCollection<PhysSoftBody^>
{
protected:
	virtual PhysSoftBody^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~PhysSoftBodyCollection() {}

	PhysSoftBody^ getObject(NxSoftBody* nativeObject);

	void destroyObject(NxSoftBody* nativeObject);
};

}