#include "Stdafx.h"

extern "C" _AnomalousExport bool SubMesh_UseSharedVertices(Ogre::v1::SubMesh* subMesh)
{
	return subMesh->useSharedVertices;
}

extern "C" _AnomalousExport Ogre::v1::VertexData* SubMesh_VertexData(Ogre::v1::SubMesh* subMesh)
{
	return subMesh->vertexData;
}

extern "C" _AnomalousExport Ogre::v1::IndexData* SubMesh_IndexData(Ogre::v1::SubMesh* subMesh)
{
	return subMesh->indexData;
}

extern "C" _AnomalousExport const char* SubMesh_getMaterialName(Ogre::v1::SubMesh* subMesh)
{
	return subMesh->getMaterialName().c_str();
}

extern "C" _AnomalousExport void SubMesh_setMaterialName(Ogre::v1::SubMesh* subMesh, const char* name)
{
	subMesh->setMaterialName(name);
}