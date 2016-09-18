#include "Stdafx.h"

extern "C" _AnomalousExport void ManualObject_clear(Ogre::ManualObject* manualObject)
{
	manualObject->clear();
}

extern "C" _AnomalousExport void ManualObject_estimateVertexCount(Ogre::ManualObject* manualObject, uint count)
{
	manualObject->estimateVertexCount(count);
}

extern "C" _AnomalousExport void ManualObject_estimateIndexCount(Ogre::ManualObject* manualObject, uint count)
{
	manualObject->estimateIndexCount(count);
}

extern "C" _AnomalousExport void ManualObject_begin(Ogre::ManualObject* manualObject, const char* materialName, Ogre::RenderOperation::OperationType opType)
{
	manualObject->begin(materialName, opType);
}

extern "C" _AnomalousExport void ManualObject_setDynamic(Ogre::ManualObject* manualObject, bool dyn)
{
	manualObject->setDynamic(dyn);
}

extern "C" _AnomalousExport bool ManualObject_getDynamic(Ogre::ManualObject* manualObject)
{
	return manualObject->getDynamic();
}

extern "C" _AnomalousExport void ManualObject_beginUpdate(Ogre::ManualObject* manualObject, uint sectionIndex)
{
	manualObject->beginUpdate(sectionIndex);
}

extern "C" _AnomalousExport void ManualObject_positionRef(Ogre::ManualObject* manualObject, Vector3* pos)
{
	manualObject->position(pos->toOgre());
}

extern "C" _AnomalousExport void ManualObject_position(Ogre::ManualObject* manualObject, Vector3 pos)
{
	manualObject->position(pos.toOgre());
}

extern "C" _AnomalousExport void ManualObject_positionRaw(Ogre::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->position(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_normalRef(Ogre::ManualObject* manualObject, Vector3* normal)
{
	manualObject->normal(normal->toOgre());
}

extern "C" _AnomalousExport void ManualObject_normal(Ogre::ManualObject* manualObject, Vector3 normal)
{
	manualObject->normal(normal.toOgre());
}

extern "C" _AnomalousExport void ManualObject_normalRaw(Ogre::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->normal(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_binormal(Ogre::ManualObject* manualObject, Vector3 binormal)
{
	manualObject->binormal(binormal.toOgre());
}

extern "C" _AnomalousExport void ManualObject_binormalRaw(Ogre::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->binormal(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_tangent(Ogre::ManualObject* manualObject, Vector3 tangent)
{
	manualObject->tangent(tangent.toOgre());
}

extern "C" _AnomalousExport void ManualObject_tangentRaw(Ogre::ManualObject* manualObject, float x, float y, float z)
{
	manualObject->tangent(x, y, z);
}

extern "C" _AnomalousExport void ManualObject_textureCoordU(Ogre::ManualObject* manualObject, float u)
{
	manualObject->textureCoord(u);
}

extern "C" _AnomalousExport void ManualObject_textureCoordUV(Ogre::ManualObject* manualObject, float u, float v)
{
	manualObject->textureCoord(u, v);
}

extern "C" _AnomalousExport void ManualObject_textureCoordUVW(Ogre::ManualObject* manualObject, float u, float v, float w)
{
	manualObject->textureCoord(u, v, w);
}

extern "C" _AnomalousExport void ManualObject_textureCoordRaw(Ogre::ManualObject* manualObject, float x, float y, float z, float w)
{
	manualObject->textureCoord(x, y, z, w);
}

extern "C" _AnomalousExport void ManualObject_textureCoord(Ogre::ManualObject* manualObject, Vector3* uvw)
{
	manualObject->textureCoord(uvw->toOgre());
}

extern "C" _AnomalousExport void ManualObject_color(Ogre::ManualObject* manualObject, float r, float g, float b, float a)
{
	manualObject->colour(r, g, b, a);
}

extern "C" _AnomalousExport void ManualObject_index(Ogre::ManualObject* manualObject, uint idx)
{
	manualObject->index(idx);
}

extern "C" _AnomalousExport void ManualObject_triangle(Ogre::ManualObject* manualObject, uint i1, uint i2, uint i3)
{
	manualObject->triangle(i1, i2, i3);
}

extern "C" _AnomalousExport void ManualObject_quad(Ogre::ManualObject* manualObject, uint i1, uint i2, uint i3, uint i4)
{
	manualObject->quad(i1, i2, i3, i4);
}

extern "C" _AnomalousExport Ogre::ManualObject::ManualObjectSection* ManualObject_end(Ogre::ManualObject* manualObject)
{
	return manualObject->end();
}

extern "C" _AnomalousExport void ManualObject_setMaterialName(Ogre::ManualObject* manualObject, uint subindex, const char* name)
{
	manualObject->setMaterialName(subindex, name);
}

extern "C" _AnomalousExport void ManualObject_setUseIdentityProjection(Ogre::ManualObject* manualObject, bool useIdentityProjection)
{
	manualObject->setUseIdentityProjection(useIdentityProjection);
}

extern "C" _AnomalousExport bool ManualObject_getUseIdentityProjection(Ogre::ManualObject* manualObject)
{
	return manualObject->getUseIdentityProjection();
}

extern "C" _AnomalousExport void ManualObject_setUseIdentityView(Ogre::ManualObject* manualObject, bool useIdentityView)
{
	manualObject->setUseIdentityView(useIdentityView);
}

extern "C" _AnomalousExport bool ManualObject_getUseIdentityView(Ogre::ManualObject* manualObject)
{
	return manualObject->getUseIdentityView();
}

extern "C" _AnomalousExport Ogre::ManualObject::ManualObjectSection* ManualObject_getSection(Ogre::ManualObject* manualObject, uint index)
{
	return manualObject->getSection(index);
}

extern "C" _AnomalousExport uint ManualObject_getNumSections(Ogre::ManualObject* manualObject)
{
	return manualObject->getNumSections();
}

extern "C" _AnomalousExport void ManualObject_setKeepDeclarationOrder(Ogre::ManualObject* manualObject, bool keepOrder)
{
	manualObject->setKeepDeclarationOrder(keepOrder);
}

extern "C" _AnomalousExport bool ManualObject_getKeepDeclarationOrder(Ogre::ManualObject* manualObject)
{
	return manualObject->getKeepDeclarationOrder();
}

extern "C" _AnomalousExport float ManualObject_getBoundingRadius(Ogre::ManualObject* manualObject)
{
	return manualObject->getBoundingRadius();
}