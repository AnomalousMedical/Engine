//Source
#include "stdafx.h"
#include "VertexElementCollection.h"
#include "VertexElement.h"

namespace Engine{

namespace Rendering{

VertexElement^ VertexElementCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew VertexElement(static_cast<Ogre::VertexElement*>(nativeObject));
}

VertexElement^ VertexElementCollection::getObject(const Ogre::VertexElement* nativeObject)
{
	return getObjectVoid((void*)nativeObject);
}

void VertexElementCollection::destroyObject(const Ogre::VertexElement* nativeObject)
{
	destroyObjectVoid((void*)nativeObject);
}

}

}