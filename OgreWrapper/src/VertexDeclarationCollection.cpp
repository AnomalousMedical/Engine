//Source
#include "stdafx.h"
#include "VertexDeclarationCollection.h"
#include "VertexDeclaration.h"

namespace Engine{

namespace Rendering{

VertexDeclaration^ VertexDeclarationCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew VertexDeclaration(static_cast<Ogre::VertexDeclaration*>(nativeObject));
}

VertexDeclaration^ VertexDeclarationCollection::getObject(Ogre::VertexDeclaration* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void VertexDeclarationCollection::destroyObject(Ogre::VertexDeclaration* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}

}