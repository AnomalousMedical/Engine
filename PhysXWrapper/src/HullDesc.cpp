#include "StdAfx.h"
#include "..\include\HullDesc.h"

#include <string.h>  //for memcpy
#include "hull.h"

namespace Engine
{

namespace Physics
{

namespace StanHull
{

HullDesc::HullDesc(void)
:desc(new ::HullDesc())
{

}

bool HullDesc::hasHullFlag(HullFlag flag)
{
	return desc->HasHullFlag((::HullFlag)flag);
}

void HullDesc::setHullFlag(HullFlag flag)
{
	desc->SetHullFlag((::HullFlag)flag);
}

void HullDesc::clearHullFlag(HullFlag flag)
{
	desc->ClearHullFlag((::HullFlag)flag);
}

HullFlag HullDesc::Flags::get() 
{
	return (HullFlag)desc->mFlags;
}

void HullDesc::Flags::set(HullFlag value) 
{
	desc->mFlags = (unsigned int)value;
}

unsigned int HullDesc::Vcount::get() 
{
	return desc->mVcount;
}

void HullDesc::Vcount::set(unsigned int value) 
{
	desc->mVcount = value;
}

double* HullDesc::Vertices::get() 
{
	return desc->mVertices;
}

void HullDesc::Vertices::set(double* value) 
{
	desc->mVertices = value;
}

unsigned int HullDesc::VertexStride::get() 
{
	return desc->mVertexStride;
}

void HullDesc::VertexStride::set(unsigned int value) 
{
	desc->mVertexStride = value;
}

double HullDesc::NormalEpsilon::get() 
{
	return desc->mNormalEpsilon;
}

void HullDesc::NormalEpsilon::set(double value) 
{
	desc->mNormalEpsilon = value;
}

double HullDesc::SkinWidth::get() 
{
	return desc->mSkinWidth;
}

void HullDesc::SkinWidth::set(double value) 
{
	desc->mSkinWidth = value;
}

unsigned int HullDesc::MaxVertices::get() 
{
	return desc->mMaxVertices;
}

void HullDesc::MaxVertices::set(unsigned int value) 
{
	desc->mMaxVertices = value;
}

}

}

}