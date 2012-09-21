#include "Stdafx.h"

extern "C" _AnomalousExport bool SubMesh_UseSharedVertices(Ogre::SubMesh* subMesh)
{
	return subMesh->useSharedVertices;
}

extern "C" _AnomalousExport Ogre::VertexData* SubMesh_VertexData(Ogre::SubMesh* subMesh)
{
	return subMesh->vertexData;
}

extern "C" _AnomalousExport Ogre::IndexData* SubMesh_IndexData(Ogre::SubMesh* subMesh)
{
	return subMesh->indexData;
}

extern "C" _AnomalousExport const char* SubMesh_getMaterialName(Ogre::SubMesh* subMesh)
{
	return subMesh->getMaterialName().c_str();
}

extern "C" _AnomalousExport void SubMesh_setMaterialName(Ogre::SubMesh* subMesh, const char* name)
{
	subMesh->setMaterialName(name);
}