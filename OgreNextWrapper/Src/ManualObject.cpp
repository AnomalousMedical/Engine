#include "Stdafx.h"

extern "C" _AnomalousExport void ManualObject_clear(Ogre::v1::ManualObject* manualObject)
{
	manualObject->clear();
}

extern "C" _AnomalousExport void ManualObject_estimateVertexCount(Ogre::v1::ManualObject* manualObject, uint count)
{
	manualObject->estimateVertexCount(count);
}

extern "C" _AnomalousExport void ManualObject_estimateIndexCount(Ogre::v1::ManualObject* manualObject, uint count)
{
	manualObject->estimateIndexCount(count);
}

extern "C" _AnomalousExport void ManualObject_begin(Ogre::v1::ManualObject* manualObject, const char* materialName, Ogre::v1::RenderOperation::OperationType opType) //There is a replacement for this
{
	manualObject->begin(materialName, opType);
}

extern "C" _AnomalousExport void ManualObject_setDynamic(Ogre::v1::ManualObject* manualObject, bool dyn)
{
	manualObject->setDynamic(dyn);
}

extern "C" _AnomalousExport bool ManualObject_getDynamic(Ogre::v1::ManualObject* manualObject)
{
	return manualObject->getDynamic();
}

extern "C" _AnomalousExport void ManualObject_beginUpdate(Ogre::v1::ManualObject* manualObject, uint sectionIndex)
{
	manualObject->beginUpdate(sectionIndex);
}

extern "C" _AnomalousExport void ManualObject_positionRef(Ogre::v1::ManualObject* manualObject, Vector3* pos)
{
	manualObject->position(pos->toOgre());
}

extern "C" _AnomalousExport void ManualObject_position(Ogre::v1::ManualObject* manualObject, Vector3 pos)
{
	manualObject->position(pos.toOgre());
}

extern "C" _AnomalousExport void ManualObject_positionRaw(Ogre::v1::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->position(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_normalRef(Ogre::v1::ManualObject* manualObject, Vector3* normal)
{
	manualObject->normal(normal->toOgre());
}

extern "C" _AnomalousExport void ManualObject_normal(Ogre::v1::ManualObject* manualObject, Vector3 normal)
{
	manualObject->normal(normal.toOgre());
}

extern "C" _AnomalousExport void ManualObject_normalRaw(Ogre::v1::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->normal(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_binormal(Ogre::v1::ManualObject* manualObject, Vector3 binormal)
{
	manualObject->binormal(binormal.toOgre());
}

extern "C" _AnomalousExport void ManualObject_binormalRaw(Ogre::v1::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->binormal(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_tangent(Ogre::v1::ManualObject* manualObject, Vector3 tangent)
{
	manualObject->tangent(tangent.toOgre());
}

extern "C" _AnomalousExport void ManualObject_tangentRaw(Ogre::v1::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->tangent(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_textureCoordU(Ogre::v1::ManualObject* manualObject, float u)
{
	manualObject->textureCoord(u);
}

extern "C" _AnomalousExport void ManualObject_textureCoordUV(Ogre::v1::ManualObject* manualObject, float u, float v)
{
	manualObject->textureCoord(u, v);
}

extern "C" _AnomalousExport void ManualObject_textureCoordUVW(Ogre::v1::ManualObject* manualObject, float u, float v, float w)
{
	manualObject->textureCoord(u, v, w);
}

extern "C" _AnomalousExport void ManualObject_textureCoordRaw(Ogre::v1::ManualObject* manualObject, float x, float y, float z, float w)
{
	manualObject->textureCoord(x, y, z, w);
}

extern "C" _AnomalousExport void ManualObject_textureCoord(Ogre::v1::ManualObject* manualObject, Vector3* uvw)
{
	manualObject->textureCoord(uvw->toOgre());
}

extern "C" _AnomalousExport void ManualObject_color(Ogre::v1::ManualObject* manualObject, float r, float g, float b, float a)
{
	manualObject->colour(r, g, b, a);
}

extern "C" _AnomalousExport void ManualObject_index(Ogre::v1::ManualObject* manualObject, uint idx)
{
	manualObject->index(idx);
}

extern "C" _AnomalousExport void ManualObject_triangle(Ogre::v1::ManualObject* manualObject, uint i1, uint i2, uint i3)
{
	manualObject->triangle(i1, i2, i3);
}

extern "C" _AnomalousExport void ManualObject_quad(Ogre::v1::ManualObject* manualObject, uint i1, uint i2, uint i3, uint i4)
{
	manualObject->quad(i1, i2, i3, i4);
}

extern "C" _AnomalousExport Ogre::v1::ManualObject::ManualObjectSection* ManualObject_end(Ogre::v1::ManualObject* manualObject)
{
	return manualObject->end();
}

extern "C" _AnomalousExport void ManualObject_setMaterialName(Ogre::v1::ManualObject* manualObject, uint subindex, const char* name)
{
	manualObject->setMaterialName(subindex, name);
}

extern "C" _AnomalousExport void ManualObject_setUseIdentityProjection(Ogre::v1::ManualObject* manualObject, bool useIdentityProjection)
{
	manualObject->setUseIdentityProjection(useIdentityProjection);
}

extern "C" _AnomalousExport bool ManualObject_getUseIdentityProjection(Ogre::v1::ManualObject* manualObject)
{
	return manualObject->getUseIdentityProjection();
}

extern "C" _AnomalousExport void ManualObject_setUseIdentityView(Ogre::v1::ManualObject* manualObject, bool useIdentityView)
{
	manualObject->setUseIdentityView(useIdentityView);
}

extern "C" _AnomalousExport bool ManualObject_getUseIdentityView(Ogre::v1::ManualObject* manualObject)
{
	return manualObject->getUseIdentityView();
}

extern "C" _AnomalousExport Ogre::v1::ManualObject::ManualObjectSection* ManualObject_getSection(Ogre::v1::ManualObject* manualObject, uint index)
{
	return manualObject->getSection(index);
}

extern "C" _AnomalousExport uint ManualObject_getNumSections(Ogre::v1::ManualObject* manualObject)
{
	return manualObject->getNumSections();
}

extern "C" _AnomalousExport void ManualObject_setKeepDeclarationOrder(Ogre::v1::ManualObject* manualObject, bool keepOrder)
{
	manualObject->setKeepDeclarationOrder(keepOrder);
}

extern "C" _AnomalousExport bool ManualObject_getKeepDeclarationOrder(Ogre::v1::ManualObject* manualObject)
{
	return manualObject->getKeepDeclarationOrder();
}

extern "C" _AnomalousExport float ManualObject_getBoundingRadius(Ogre::v1::ManualObject* manualObject)
{
	return manualObject->getBoundingRadius();
}