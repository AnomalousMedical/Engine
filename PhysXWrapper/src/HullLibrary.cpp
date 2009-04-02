#include "StdAfx.h"
#include "..\include\HullLibrary.h"

#include <string.h>  //for memcpy
#include "hull.h"
#include "HullDesc.h"
#include "HullResult.h"

namespace PhysXWrapper
{

namespace StanHull
{

HullLibrary::HullLibrary(void)
:library(new ::HullLibrary())
{

}

HullError HullLibrary::createConvexHull(HullDesc^ desc, HullResult^ result)
{
	return (HullError)library->CreateConvexHull(*desc->desc.Get(), *result->result.Get());
}

HullError HullLibrary::releaseResult(HullResult^ result)
{
	return (HullError)library->ReleaseResult(*result->result.Get());
}

}

}