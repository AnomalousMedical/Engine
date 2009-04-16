//Header
#pragma once

namespace Engine{

namespace Rendering{

ref class VertexBufferBinding;
ref class VertexBufferBindingCollection : public WrapperCollection<VertexBufferBinding^>
{
protected:
	virtual VertexBufferBinding^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~VertexBufferBindingCollection() {}

	VertexBufferBinding^ getObject(Ogre::VertexBufferBinding* nativeObject);

	void destroyObject(Ogre::VertexBufferBinding* nativeObject);
};

}

}