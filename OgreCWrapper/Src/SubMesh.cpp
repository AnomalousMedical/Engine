#include "Stdafx.h"

extern "C" __declspec(dllexport) bool SubMesh_UseSharedVertices(Ogre::SubMesh* subMesh)
{
	return subMesh->useSharedVertices;
}

extern "C" __declspec(dllexport) Ogre::VertexData* SubMesh_VertexData(Ogre::SubMesh* subMesh)
{
	return subMesh->vertexData;
}

extern "C" __declspec(dllexport) Ogre::IndexData* SubMesh_IndexData(Ogre::SubMesh* subMesh)
{
	return subMesh->indexData;
}