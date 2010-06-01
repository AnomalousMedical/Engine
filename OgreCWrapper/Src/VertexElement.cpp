#include "Stdafx.h"

extern "C" __declspec(dllexport) ushort VertexElement_getSource(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getSource();
}

extern "C" __declspec(dllexport) size_t VertexElement_getOffset(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getOffset();
}

extern "C" __declspec(dllexport) Ogre::VertexElementType VertexElement_getType(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getType();
}

extern "C" __declspec(dllexport) Ogre::VertexElementSemantic VertexElement_getSemantic(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getSemantic();
}

extern "C" __declspec(dllexport) ushort VertexElement_getIndex(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getIndex();
}

extern "C" __declspec(dllexport) size_t VertexElement_getSize(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getSize();
}

extern "C" __declspec(dllexport) void VertexElement_baseVertexPointerToElementVoid(Ogre::VertexElement* vertexElement, void* basePtr, void** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" __declspec(dllexport) void VertexElement_baseVertexPointerToElementFloat(Ogre::VertexElement* vertexElement, void* basePtr, float** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" __declspec(dllexport) void VertexElement_baseVertexPointerToElementUInt(Ogre::VertexElement* vertexElement, void* basePtr, uint** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" __declspec(dllexport) void VertexElement_baseVertexPointerToElementByte(Ogre::VertexElement* vertexElement, void* basePtr, byte** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" __declspec(dllexport) void VertexElement_baseVertexPointerToElementUShort(Ogre::VertexElement* vertexElement, void* basePtr, ushort** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" __declspec(dllexport) size_t VertexElement_getTypeSize(Ogre::VertexElementType eType)
{
	return Ogre::VertexElement::getTypeSize(eType);
}

extern "C" __declspec(dllexport) size_t VertexElement_getTypeCount(Ogre::VertexElementType eType)
{
	return Ogre::VertexElement::getTypeCount(eType);
}

extern "C" __declspec(dllexport) Ogre::VertexElementType VertexElement_multiplyTypeCount(Ogre::VertexElementType baseType, ushort count)
{
	return Ogre::VertexElement::multiplyTypeCount(baseType, count);
}

extern "C" __declspec(dllexport) Ogre::VertexElementType VertexElement_getBaseType(Ogre::VertexElementType multiType)
{
	return Ogre::VertexElement::getBaseType(multiType);
}

extern "C" __declspec(dllexport) void VertexElement_convertColorValue(Ogre::VertexElementType srcType, Ogre::VertexElementType dstType, uint* ptr)
{
	Ogre::VertexElement::convertColourValue(srcType, dstType, ptr);
}

extern "C" __declspec(dllexport) uint VertexElement_convertColorValue2(Color src, Ogre::VertexElementType dst)
{
	return Ogre::VertexElement::convertColourValue(src.toOgre(), dst);
}

extern "C" __declspec(dllexport) Ogre::VertexElementType VertexElement_getBestColorVertexElementType()
{
	return Ogre::VertexElement::getBestColourVertexElementType();
}