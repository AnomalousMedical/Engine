//Header
#pragma once

namespace PhysXWrapper{

ref class PhysShape;
ref class PhysShapeCollection : public WrapperCollection<PhysShape^>
{
protected:
	virtual PhysShape^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~PhysShapeCollection() {}

	PhysShape^ getObject(NxShape* nativeObject);

	void destroyObject(NxShape* nativeObject);
};

}