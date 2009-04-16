//Header
#pragma once

namespace Rendering{

ref class VertexElement;
ref class VertexElementCollection : public WrapperCollection<VertexElement^>
{
protected:
	virtual VertexElement^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~VertexElementCollection() {}

	VertexElement^ getObject(const Ogre::VertexElement* nativeObject);

	void destroyObject(const Ogre::VertexElement* nativeObject);
};

}