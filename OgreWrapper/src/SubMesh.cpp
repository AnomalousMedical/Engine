#include "StdAfx.h"
#include "..\include\SubMesh.h"
#include "VertexData.h"
#include "IndexData.h"

namespace Engine{

namespace Rendering{

SubMesh::SubMesh(Ogre::SubMesh* subMesh)
:subMesh(subMesh),
vertex(nullptr),
index(nullptr)
{
	if(subMesh->vertexData != NULL)
	{
		vertex = gcnew VertexData(subMesh->vertexData);
	}
	if(subMesh->indexData != NULL)
	{
		index = gcnew IndexData(subMesh->indexData);
	}
}

SubMesh::~SubMesh(void)
{
	subMesh = 0;
	if(vertex != nullptr)
	{
		delete vertex;
		vertex = nullptr;
	}
	if(index != nullptr)
	{
		delete index;
		index = nullptr;
	}
}

bool SubMesh::UseSharedVertices::get() 
{
	return subMesh->useSharedVertices;
}

VertexData^ SubMesh::vertexData::get() 
{
	return vertex;
}

IndexData^ SubMesh::indexData::get() 
{
	return index;
}

}

}