#include "StdAfx.h"
#include "..\include\HullResult.h"

#include <string.h>  //for memcpy
#include "hull.h"

namespace Engine
{

namespace Physics
{

namespace StanHull
{

HullResult::HullResult(void)
:result(new ::HullResult())
{

}

bool HullResult::IsPolygons::get() 
{
	return result->mPolygons;
}

unsigned int HullResult::NumOutputVertices::get() 
{
	return result->mNumOutputVertices;
}

double* HullResult::OutputVertices::get() 
{
	return result->mOutputVertices;
}

unsigned int HullResult::NumFaces::get() 
{
	return result->mNumFaces;
}

unsigned int HullResult::NumIndices::get() 
{
	return result->mNumIndices;
}

unsigned int* HullResult::Indices::get() 
{
	return result->mIndices;
}

}

}

}