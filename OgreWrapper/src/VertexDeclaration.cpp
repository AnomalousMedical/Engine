#include "StdAfx.h"
#include "..\include\VertexDeclaration.h"
#include "VertexElement.h"
#include "Ogre.h"

namespace OgreWrapper{

VertexDeclaration::VertexDeclaration(Ogre::VertexDeclaration* vertexDeclaration)
:vertexDeclaration(vertexDeclaration)
{
}

VertexDeclaration::~VertexDeclaration(void)
{
	vertexDeclaration = 0;
}

Ogre::VertexDeclaration* VertexDeclaration::getOgreVertexDeclaration()
{
	return vertexDeclaration;
}

size_t VertexDeclaration::getElementCount()
{
	return vertexDeclaration->getElementCount();
}

VertexElementEnum^ VertexDeclaration::getElements()
{
	/*const Ogre::VertexDeclaration::VertexElementList& elementList = vertexDeclaration->getElements();
	VertexElementList^ vertexElementList = gcnew VertexElementList();
	for(Ogre::VertexDeclaration::VertexElementList::const_iterator iter = elementList.begin(); iter != elementList.end(); ++iter)
	{
		vertexElementList->Add(getObject(*iter));
	}
	return vertexElementList;*/
	throw gcnew System::NotImplementedException();
}

VertexElement^ VertexDeclaration::getElement(unsigned short index)
{
	return vertexElements.getObject(vertexDeclaration->getElement(index));
}

void VertexDeclaration::sort()
{
	return vertexDeclaration->sort();
}

void VertexDeclaration::closeGapsInSource()
{
	return vertexDeclaration->closeGapsInSource();
}

VertexDeclaration^ VertexDeclaration::getAutoOrganizedDeclaration(bool skeletalAnimation, bool vertexAnimation)
{
	throw gcnew System::NotImplementedException();
}

unsigned short VertexDeclaration::getMaxSource()
{
	return vertexDeclaration->getMaxSource();
}

VertexElement^ VertexDeclaration::addElement(unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic)
{
	return vertexElements.getObject(&vertexDeclaration->addElement(source, offset, static_cast<Ogre::VertexElementType>(theType), static_cast<Ogre::VertexElementSemantic>(semantic)));
}

VertexElement^ VertexDeclaration::addElement(unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic, unsigned short index)
{
	return vertexElements.getObject(&vertexDeclaration->addElement(source, offset, static_cast<Ogre::VertexElementType>(theType), static_cast<Ogre::VertexElementSemantic>(semantic), index));
}

VertexElement^ VertexDeclaration::insertElement(unsigned short atPosition, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic)
{
	return vertexElements.getObject(&vertexDeclaration->insertElement(atPosition, source, offset, static_cast<Ogre::VertexElementType>(theType), static_cast<Ogre::VertexElementSemantic>(semantic)));
}

VertexElement^ VertexDeclaration::insertElement(unsigned short atPosition, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic, unsigned short index)
{
	return vertexElements.getObject(&vertexDeclaration->insertElement(atPosition, source, offset, static_cast<Ogre::VertexElementType>(theType), static_cast<Ogre::VertexElementSemantic>(semantic), index));
}

void VertexDeclaration::removeElement(unsigned short elemIndex)
{
	vertexElements.destroyObject(vertexDeclaration->getElement(elemIndex));
	return vertexDeclaration->removeElement(elemIndex);
}

void VertexDeclaration::removeElement(VertexElementSemantic semantic)
{
	vertexElements.destroyObject(vertexDeclaration->findElementBySemantic(static_cast<Ogre::VertexElementSemantic>(semantic)));
	return vertexDeclaration->removeElement(static_cast<Ogre::VertexElementSemantic>(semantic));
}

void VertexDeclaration::removeElement(VertexElementSemantic semantic, unsigned short index)
{
	vertexElements.destroyObject(vertexDeclaration->findElementBySemantic(static_cast<Ogre::VertexElementSemantic>(semantic), index));
	return vertexDeclaration->removeElement(static_cast<Ogre::VertexElementSemantic>(semantic), index);
}

void VertexDeclaration::removeAllElements()
{
	vertexElements.clearObjects();
	return vertexDeclaration->removeAllElements();
}

void VertexDeclaration::modifyElement(unsigned short elemIndex, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic)
{
	return vertexDeclaration->modifyElement(elemIndex, source, offset, static_cast<Ogre::VertexElementType>(theType), static_cast<Ogre::VertexElementSemantic>(semantic));
}

void VertexDeclaration::modifyElement(unsigned short elemIndex, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic, unsigned short index)
{
	return vertexDeclaration->modifyElement(elemIndex, source, offset, static_cast<Ogre::VertexElementType>(theType), static_cast<Ogre::VertexElementSemantic>(semantic), index);
}

VertexElement^ VertexDeclaration::findElementBySemantic(VertexElementSemantic sem)
{
	return vertexElements.getObject(vertexDeclaration->findElementBySemantic(static_cast<Ogre::VertexElementSemantic>(sem)));
}

VertexElement^ VertexDeclaration::findElementBySemantic(VertexElementSemantic sem, unsigned short index)
{
	return vertexElements.getObject(vertexDeclaration->findElementBySemantic(static_cast<Ogre::VertexElementSemantic>(sem), index));
}

VertexElementEnum^ VertexDeclaration::findElementsBySource(unsigned short source)
{
	//return vertexDeclaration->findElementsBySource(source);
	throw gcnew System::NotImplementedException();
}

size_t VertexDeclaration::getVertexSize(unsigned short source)
{
	return vertexDeclaration->getVertexSize(source);
}

VertexDeclaration^ VertexDeclaration::clone()
{
	throw gcnew System::NotImplementedException();
}

}