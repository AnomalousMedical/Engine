#include "Stdafx.h"

extern "C" _AnomalousExport ushort VertexElement_getSource(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getSource();
}

extern "C" _AnomalousExport size_t VertexElement_getOffset(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getOffset();
}

extern "C" _AnomalousExport Ogre::VertexElementType VertexElement_getType(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getType();
}

extern "C" _AnomalousExport Ogre::VertexElementSemantic VertexElement_getSemantic(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getSemantic();
}

extern "C" _AnomalousExport ushort VertexElement_getIndex(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getIndex();
}

extern "C" _AnomalousExport size_t VertexElement_getSize(Ogre::VertexElement* vertexElement)
{
	return vertexElement->getSize();
}

extern "C" _AnomalousExport void VertexElement_baseVertexPointerToElementVoid(Ogre::VertexElement* vertexElement, void* basePtr, void** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" _AnomalousExport void VertexElement_baseVertexPointerToElementFloat(Ogre::VertexElement* vertexElement, void* basePtr, float** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" _AnomalousExport void VertexElement_baseVertexPointerToElementUInt(Ogre::VertexElement* vertexElement, void* basePtr, uint** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" _AnomalousExport void VertexElement_baseVertexPointerToElementByte(Ogre::VertexElement* vertexElement, void* basePtr, byte** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" _AnomalousExport void VertexElement_baseVertexPointerToElementUShort(Ogre::VertexElement* vertexElement, void* basePtr, ushort** elem)
{
	vertexElement->baseVertexPointerToElement(basePtr, elem);
}

extern "C" _AnomalousExport size_t VertexElement_getTypeSize(Ogre::VertexElementType eType)
{
	return Ogre::VertexElement::getTypeSize(eType);
}

extern "C" _AnomalousExport size_t VertexElement_getTypeCount(Ogre::VertexElementType eType)
{
	return Ogre::VertexElement::getTypeCount(eType);
}

extern "C" _AnomalousExport Ogre::VertexElementType VertexElement_multiplyTypeCount(Ogre::VertexElementType baseType, ushort count)
{
	return Ogre::VertexElement::multiplyTypeCount(baseType, count);
}

extern "C" _AnomalousExport Ogre::VertexElementType VertexElement_getBaseType(Ogre::VertexElementType multiType)
{
	return Ogre::VertexElement::getBaseType(multiType);
}

extern "C" _AnomalousExport void VertexElement_convertColorValue(Ogre::VertexElementType srcType, Ogre::VertexElementType dstType, uint* ptr)
{
	Ogre::VertexElement::convertColourValue(srcType, dstType, ptr);
}

extern "C" _AnomalousExport uint VertexElement_convertColorValue2(Color src, Ogre::VertexElementType dst)
{
	return Ogre::VertexElement::convertColourValue(src.toOgre(), dst);
}

extern "C" _AnomalousExport Ogre::VertexElementType VertexElement_getBestColorVertexElementType()
{
	return Ogre::VertexElement::getBestColourVertexElementType();
}