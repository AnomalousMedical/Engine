#include "StdAfx.h"
#include "..\include\VertexElement.h"
#include "MathUtils.h"

namespace OgreWrapper{

VertexElement::VertexElement(const Ogre::VertexElement* vertexElement)
:vertexElement(vertexElement)
{
}

VertexElement::~VertexElement(void)
{
	vertexElement = 0;
}

unsigned short VertexElement::getSource()
{
	return vertexElement->getSource();
}

size_t VertexElement::getOffset()
{
	return vertexElement->getOffset();
}

VertexElementType VertexElement::getType()
{
	return (VertexElementType)vertexElement->getType();
}

VertexElementSemantic VertexElement::getSemantic()
{
	return (VertexElementSemantic)vertexElement->getSemantic();
}

unsigned short VertexElement::getIndex()
{
	return vertexElement->getIndex();
}

size_t VertexElement::getSize()
{
	return vertexElement->getSize();
}

void VertexElement::baseVertexPointerToElement(void* base, void** elem)
{
	return vertexElement->baseVertexPointerToElement(base, elem);
}

void VertexElement::baseVertexPointerToElement(void* base, float** elem)
{
	return vertexElement->baseVertexPointerToElement(base, elem);
}

void VertexElement::baseVertexPointerToElement(void* base, unsigned int** elem)
{
	return vertexElement->baseVertexPointerToElement(base, elem);
}

void VertexElement::baseVertexPointerToElement(void* base, unsigned char** elem)
{
	return vertexElement->baseVertexPointerToElement(base, elem);
}

void VertexElement::baseVertexPointerToElement(void* base, unsigned short** elem)
{
	return vertexElement->baseVertexPointerToElement(base, elem);
}

size_t VertexElement::getTypeSize(VertexElementType eType)
{
	return Ogre::VertexElement::getTypeSize((Ogre::VertexElementType)eType);
}

size_t VertexElement::getTypeCount(VertexElementType eType)
{
	return Ogre::VertexElement::getTypeCount((Ogre::VertexElementType)eType);
}

VertexElementType VertexElement::multiplyTypeCount(VertexElementType baseType, unsigned short count)
{
	return (VertexElementType)Ogre::VertexElement::multiplyTypeCount((Ogre::VertexElementType)baseType, count);
}

VertexElementType VertexElement::getBaseType(VertexElementType multiType)
{
	return (VertexElementType)Ogre::VertexElement::getBaseType((Ogre::VertexElementType)multiType);
}

void VertexElement::convertColorValue(VertexElementType srcType, VertexElementType dstType, unsigned int* ptr)
{
	return Ogre::VertexElement::convertColourValue((Ogre::VertexElementType)srcType, (Ogre::VertexElementType)dstType, ptr);
}

unsigned int VertexElement::convertColorValue(Engine::Color src, VertexElementType dst)
{
	return Ogre::VertexElement::convertColourValue(MathUtils::copyColor(src), (Ogre::VertexElementType)dst);
}

VertexElementType VertexElement::getBestColorVertexElementType()
{
	return (VertexElementType)Ogre::VertexElement::getBestColourVertexElementType();
}

}