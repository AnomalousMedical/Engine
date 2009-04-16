//Header
#pragma once

namespace OgreWrapper{

ref class VertexDeclaration;
ref class VertexDeclarationCollection : public WrapperCollection<VertexDeclaration^>
{
protected:
	virtual VertexDeclaration^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~VertexDeclarationCollection() {}

	VertexDeclaration^ getObject(Ogre::VertexDeclaration* nativeObject);

	void destroyObject(Ogre::VertexDeclaration* nativeObject);
};

}