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