#include "StdAfx.h"
#include "..\include\TetraMesh.h"

namespace PhysXWrapper
{

TetraMesh::TetraMesh(void)
:mesh(new NxTetraMesh())
{

}

NxTetraMesh* TetraMesh::getNxTetraMesh()
{
	return mesh.Get();
}

bool TetraMesh::IsTetra::get() 
{
	return mesh->mIsTetra;
}

void TetraMesh::IsTetra::set(bool value) 
{
	mesh->mIsTetra = value;
}

unsigned int TetraMesh::VertexCount::get() 
{
	return mesh->mVcount;
}

void TetraMesh::VertexCount::set(unsigned int value) 
{
	mesh->mVcount = value;
}

float* TetraMesh::Vertices::get() 
{
	return mesh->mVertices;
}

void TetraMesh::Vertices::set(float* value) 
{
	mesh->mVertices = value;
}

unsigned int TetraMesh::TriangleCount::get() 
{
	return mesh->mTcount;
}

void TetraMesh::TriangleCount::set(unsigned int value) 
{
	mesh->mTcount = value;
}

unsigned int* TetraMesh::Indices::get() 
{
	return mesh->mIndices;
}

void TetraMesh::Indices::set(unsigned int* value) 
{
	mesh->mIndices = value;
}

}