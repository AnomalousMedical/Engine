#include "Stdafx.h"

extern "C" __declspec(dllexport) size_t VertexDeclaration_getElementCount(Ogre::VertexDeclaration* vertexDeclaration)
{
	return vertexDeclaration->getElementCount();
}

extern "C" __declspec(dllexport) const Ogre::VertexElement* VertexDeclaration_getElement(Ogre::VertexDeclaration* vertexDeclaration, ushort index)
{
	return vertexDeclaration->getElement(index);
}

extern "C" __declspec(dllexport) void VertexDeclaration_sort(Ogre::VertexDeclaration* vertexDeclaration)
{
	vertexDeclaration->sort();
}

extern "C" __declspec(dllexport) void VertexDeclaration_closeGapsInSource(Ogre::VertexDeclaration* vertexDeclaration)
{
	vertexDeclaration->closeGapsInSource();
}

extern "C" __declspec(dllexport) ushort VertexDeclaration_getMaxSource(Ogre::VertexDeclaration* vertexDeclaration)
{
	return vertexDeclaration->getMaxSource();
}

extern "C" __declspec(dllexport) const Ogre::VertexElement* VertexDeclaration_addElement(Ogre::VertexDeclaration* vertexDeclaration, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic)
{
	return &vertexDeclaration->addElement(source, offset, theType, semantic);
}

extern "C" __declspec(dllexport) const Ogre::VertexElement* VertexDeclaration_addElement2(Ogre::VertexDeclaration* vertexDeclaration, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic, ushort index)
{
	return &vertexDeclaration->addElement(source, offset, theType, semantic, index);
}

extern "C" __declspec(dllexport) const Ogre::VertexElement* VertexDeclaration_insertElement(Ogre::VertexDeclaration* vertexDeclaration, ushort atPosition, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic)
{
	return &vertexDeclaration->insertElement(atPosition, source, offset, theType, semantic);
}

extern "C" __declspec(dllexport) const Ogre::VertexElement* VertexDeclaration_insertElement2(Ogre::VertexDeclaration* vertexDeclaration, ushort atPosition, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic, ushort index)
{
	return &vertexDeclaration->insertElement(atPosition, source, offset, theType, semantic, index);
}

extern "C" __declspec(dllexport) void VertexDeclaration_removeElement(Ogre::VertexDeclaration* vertexDeclaration, ushort elemIndex)
{
	vertexDeclaration->removeElement(elemIndex);
}

extern "C" __declspec(dllexport) void VertexDeclaration_removeElement2(Ogre::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic semantic)
{
	vertexDeclaration->removeElement(semantic);
}

extern "C" __declspec(dllexport) void VertexDeclaration_removeElement3(Ogre::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic semantic, ushort index)
{
	vertexDeclaration->removeElement(semantic, index);
}

extern "C" __declspec(dllexport) void VertexDeclaration_removeAllElements(Ogre::VertexDeclaration* vertexDeclaration)
{
	vertexDeclaration->removeAllElements();
}

extern "C" __declspec(dllexport) void VertexDeclaration_modifyElement(Ogre::VertexDeclaration* vertexDeclaration, ushort elemIndex, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic)
{
	vertexDeclaration->modifyElement(elemIndex, source, offset, theType, semantic);
}

extern "C" __declspec(dllexport) void VertexDeclaration_modifyElement2(Ogre::VertexDeclaration* vertexDeclaration, ushort elemIndex, ushort source, size_t offset, Ogre::VertexElementType theType, Ogre::VertexElementSemantic semantic, ushort index)
{
	vertexDeclaration->modifyElement(elemIndex, source, offset, theType, semantic, index);
}

extern "C" __declspec(dllexport) const Ogre::VertexElement* VertexDeclaration_findElementBySemantic(Ogre::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic sem)
{
	return vertexDeclaration->findElementBySemantic(sem);
}

extern "C" __declspec(dllexport) const Ogre::VertexElement* VertexDeclaration_findElementBySemantic2(Ogre::VertexDeclaration* vertexDeclaration, Ogre::VertexElementSemantic sem, ushort index)
{
	return vertexDeclaration->findElementBySemantic(sem, index);
}

extern "C" __declspec(dllexport) size_t VertexDeclaration_getVertexSize(Ogre::VertexDeclaration* vertexDeclaration, ushort source)
{
	return vertexDeclaration->getVertexSize(source);
}