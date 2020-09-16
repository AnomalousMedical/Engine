#include "Stdafx.h"

extern "C" _AnomalousExport size_t VertexDeclaration_getElementCount(Ogre::v1::VertexDeclaration* vertexDeclaration)
{
	return vertexDeclaration->getElementCount();
}

extern "C" _AnomalousExport const Ogre::v1::VertexElement* VertexDeclaration_getElement(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort index)
{
	return vertexDeclaration->getElement(index);
}

extern "C" _AnomalousExport void VertexDeclaration_sort(Ogre::v1::VertexDeclaration* vertexDeclaration)
{
	vertexDeclaration->sort();
}

extern "C" _AnomalousExport void VertexDeclaration_closeGapsInSource(Ogre::v1::VertexDeclaration* vertexDeclaration)
{
	vertexDeclaration->closeGapsInSource();
}

extern "C" _AnomalousExport ushort VertexDeclaration_getMaxSource(Ogre::v1::VertexDeclaration* vertexDeclaration)
{
	return vertexDeclaration->getMaxSource();
}

extern "C" _AnomalousExport const Ogre::v1::VertexElement* VertexDeclaration_addElement(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic)
{
	return &vertexDeclaration->addElement(source, offset, theType, semantic);
}

extern "C" _AnomalousExport const Ogre::v1::VertexElement* VertexDeclaration_addElement2(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic, ushort index)
{
	return &vertexDeclaration->addElement(source, offset, theType, semantic, index);
}

extern "C" _AnomalousExport const Ogre::v1::VertexElement* VertexDeclaration_insertElement(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort atPosition, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic)
{
	return &vertexDeclaration->insertElement(atPosition, source, offset, theType, semantic);
}

extern "C" _AnomalousExport const Ogre::v1::VertexElement* VertexDeclaration_insertElement2(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort atPosition, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic, ushort index)
{
	return &vertexDeclaration->insertElement(atPosition, source, offset, theType, semantic, index);
}

extern "C" _AnomalousExport void VertexDeclaration_removeElement(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort elemIndex)
{
	vertexDeclaration->removeElement(elemIndex);
}

extern "C" _AnomalousExport void VertexDeclaration_removeElement2(Ogre::v1::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic semantic)
{
	vertexDeclaration->removeElement(semantic);
}

extern "C" _AnomalousExport void VertexDeclaration_removeElement3(Ogre::v1::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic semantic, ushort index)
{
	vertexDeclaration->removeElement(semantic, index);
}

extern "C" _AnomalousExport void VertexDeclaration_removeAllElements(Ogre::v1::VertexDeclaration* vertexDeclaration)
{
	vertexDeclaration->removeAllElements();
}

extern "C" _AnomalousExport void VertexDeclaration_modifyElement(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort elemIndex, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic)
{
	vertexDeclaration->modifyElement(elemIndex, source, offset, theType, semantic);
}

extern "C" _AnomalousExport void VertexDeclaration_modifyElement2(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort elemIndex, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic, ushort index)
{
	vertexDeclaration->modifyElement(elemIndex, source, offset, theType, semantic, index);
}

extern "C" _AnomalousExport const Ogre::v1::VertexElement* VertexDeclaration_findElementBySemantic(Ogre::v1::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic sem)
{
	return vertexDeclaration->findElementBySemantic(sem);
}

extern "C" _AnomalousExport const Ogre::v1::VertexElement* VertexDeclaration_findElementBySemantic2(Ogre::v1::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic sem, ushort index)
{
	return vertexDeclaration->findElementBySemantic(sem, index);
}

extern "C" _AnomalousExport size_t VertexDeclaration_getVertexSize(Ogre::v1::VertexDeclaration* vertexDeclaration, ushort source)
{
	return vertexDeclaration->getVertexSize(source);
}