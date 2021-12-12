#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/ShaderBindingTable.h"
using namespace Diligent;
extern "C" _AnomalousExport void IShaderBindingTable_BindRayGenShader(
	IShaderBindingTable* objPtr
, char* pShaderGroupName, void* pData)
{
	objPtr->BindRayGenShader(
		pShaderGroupName
		, pData
	);
}
extern "C" _AnomalousExport void IShaderBindingTable_BindMissShader(
	IShaderBindingTable* objPtr
, char* pShaderGroupName, Uint32 MissIndex, void* pData)
{
	objPtr->BindMissShader(
		pShaderGroupName
		, MissIndex
		, pData
	);
}
extern "C" _AnomalousExport void IShaderBindingTable_BindHitGroupForInstance(
	IShaderBindingTable* objPtr
, ITopLevelAS* pTLAS, char* pInstanceName, Uint32 RayOffsetInHitGroupIndex, char* pShaderGroupName, void* pData)
{
	objPtr->BindHitGroupForInstance(
		pTLAS
		, pInstanceName
		, RayOffsetInHitGroupIndex
		, pShaderGroupName
		, pData
	);
}
extern "C" _AnomalousExport void IShaderBindingTable_BindHitGroupForTLAS(
	IShaderBindingTable* objPtr
, ITopLevelAS* pTLAS, Uint32 RayOffsetInHitGroupIndex, char* pShaderGroupName, void* pData)
{
	objPtr->BindHitGroupForTLAS(
		pTLAS
		, RayOffsetInHitGroupIndex
		, pShaderGroupName
		, pData
	);
}
