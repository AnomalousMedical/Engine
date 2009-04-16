//Source
#include "stdafx.h"
#include "VertexBufferBindingCollection.h"
#include "VertexBufferBinding.h"

namespace Rendering{

VertexBufferBinding^ VertexBufferBindingCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew VertexBufferBinding(static_cast<Ogre::VertexBufferBinding*>(nativeObject));
}

VertexBufferBinding^ VertexBufferBindingCollection::getObject(Ogre::VertexBufferBinding* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void VertexBufferBindingCollection::destroyObject(Ogre::VertexBufferBinding* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}