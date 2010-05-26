#include "Stdafx.h"

extern "C" __declspec(dllexport) void ManualObject_clear(Ogre::ManualObject* manualObject)
{
	manualObject->clear();
}

extern "C" __declspec(dllexport) void ManualObject_estimateVertexCount(Ogre::ManualObject* manualObject, uint count)
{
	manualObject->estimateVertexCount(count);
}

extern "C" __declspec(dllexport) void ManualObject_estimateIndexCount(Ogre::ManualObject* manualObject, uint count)
{
	manualObject->estimateIndexCount(count);
}

extern "C" __declspec(dllexport) void ManualObject_begin(Ogre::ManualObject* manualObject, const char* materialName, Ogre::RenderOperation::OperationType opType)
{
	manualObject->begin(materialName, opType);
}

extern "C" __declspec(dllexport) void ManualObject_setDynamic(Ogre::ManualObject* manualObject, bool dyn)
{
	manualObject->setDynamic(dyn);
}

extern "C" __declspec(dllexport) bool ManualObject_getDynamic(Ogre::ManualObject* manualObject)
{
	return manualObject->getDynamic();
}

extern "C" __declspec(dllexport) void ManualObject_beginUpdate(Ogre::ManualObject* manualObject, uint sectionIndex)
{
	manualObject->beginUpdate(sectionIndex);
}

extern "C" __declspec(dllexport) void ManualObject_positionRef(Ogre::ManualObject* manualObject, Vector3* pos)
{
	manualObject->position(pos->toOgre());
}

extern "C" __declspec(dllexport) void ManualObject_position(Ogre::ManualObject* manualObject, Vector3 pos)
{
	manualObject->position(pos.toOgre());
}

extern "C" __declspec(dllexport) void ManualObject_positionRaw(Ogre::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->position(x, y, z);
}

extern "C" __declspec(dllexport) void ManualObject_normalRef(Ogre::ManualObject* manualObject, Vector3* normal)
{
	manualObject->normal(normal->toOgre());
}

extern "C" __declspec(dllexport) void ManualObject_normal(Ogre::ManualObject* manualObject, Vector3 normal)
{
	manualObject->normal(normal.toOgre());
}

extern "C" __declspec(dllexport) void ManualObject_normalRaw(Ogre::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->normal(x, y, z);
}

extern "C" __declspec(dllexport) void ManualObject_textureCoordU(Ogre::ManualObject* manualObject, float u)
{
	manualObject->textureCoord(u);
}

extern "C" __declspec(dllexport) void ManualObject_textureCoordUV(Ogre::ManualObject* manualObject, float u, float v)
{
	manualObject->textureCoord(u, v);
}

extern "C" __declspec(dllexport) void ManualObject_textureCoordUVW(Ogre::ManualObject* manualObject, float u, float v, float w)
{
	manualObject->textureCoord(u, v, w);
}

extern "C" __declspec(dllexport) void ManualObject_textureCoordRaw(Ogre::ManualObject* manualObject, float x, float y, float z, float w)
{
	manualObject->textureCoord(x, y, z, w);
}

extern "C" __declspec(dllexport) void ManualObject_textureCoord(Ogre::ManualObject* manualObject, Vector3* uvw)
{
	manualObject->textureCoord(uvw->toOgre());
}

extern "C" __declspec(dllexport) void ManualObject_color(Ogre::ManualObject* manualObject, float r, float g, float b, float a)
{
	manualObject->colour(r, g, b, a);
}

extern "C" __declspec(dllexport) void ManualObject_index(Ogre::ManualObject* manualObject, uint idx)
{
	manualObject->index(idx);
}

extern "C" __declspec(dllexport) void ManualObject_triangle(Ogre::ManualObject* manualObject, uint i1, uint i2, uint i3)
{
	manualObject->triangle(i1, i2, i3);
}

extern "C" __declspec(dllexport) void ManualObject_quad(Ogre::ManualObject* manualObject, uint i1, uint i2, uint i3, uint i4)
{
	manualObject->quad(i1, i2, i3, i4);
}

extern "C" __declspec(dllexport) Ogre::ManualObject::ManualObjectSection* ManualObject_end(Ogre::ManualObject* manualObject)
{
	return manualObject->end();
}

extern "C" __declspec(dllexport) void ManualObject_setMaterialName(Ogre::ManualObject* manualObject, uint subindex, const char* name)
{
	manualObject->setMaterialName(subindex, name);
}

extern "C" __declspec(dllexport) void ManualObject_setUseIdentityProjection(Ogre::ManualObject* manualObject, bool useIdentityProjection)
{
	manualObject->setUseIdentityProjection(useIdentityProjection);
}

extern "C" __declspec(dllexport) bool ManualObject_getUseIdentityProjection(Ogre::ManualObject* manualObject)
{
	return manualObject->getUseIdentityProjection();
}

extern "C" __declspec(dllexport) void ManualObject_setUseIdentityView(Ogre::ManualObject* manualObject, bool useIdentityView)
{
	manualObject->setUseIdentityView(useIdentityView);
}

extern "C" __declspec(dllexport) bool ManualObject_getUseIdentityView(Ogre::ManualObject* manualObject)
{
	return manualObject->getUseIdentityView();
}

extern "C" __declspec(dllexport) Ogre::ManualObject::ManualObjectSection* ManualObject_getSection(Ogre::ManualObject* manualObject, uint index)
{
	return manualObject->getSection(index);
}

extern "C" __declspec(dllexport) uint ManualObject_getNumSections(Ogre::ManualObject* manualObject)
{
	return manualObject->getNumSections();
}

extern "C" __declspec(dllexport) void ManualObject_setKeepDeclarationOrder(Ogre::ManualObject* manualObject, bool keepOrder)
{
	manualObject->setKeepDeclarationOrder(keepOrder);
}

extern "C" __declspec(dllexport) bool ManualObject_getKeepDeclarationOrder(Ogre::ManualObject* manualObject)
{
	return manualObject->getKeepDeclarationOrder();
}

extern "C" __declspec(dllexport) float ManualObject_getBoundingRadius(Ogre::ManualObject* manualObject)
{
	return manualObject->getBoundingRadius();
}