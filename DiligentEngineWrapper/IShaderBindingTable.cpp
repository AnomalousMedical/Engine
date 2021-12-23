#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/ShaderBindingTable.h"
using namespace Diligent;
extern "C" _AnomalousExport void IShaderBindingTable_BindRayGenShader(
	IShaderBindingTable* objPtr
, char* pShaderGroupName, void* pData, Uint32 DataSize)
{
	objPtr->BindRayGenShader(
		pShaderGroupName
		, pData
		, DataSize
	);
}
extern "C" _AnomalousExport void IShaderBindingTable_BindMissShader(
	IShaderBindingTable* objPtr
, char* pShaderGroupName, Uint32 MissIndex, void* pData, Uint32 DataSize)
{
	objPtr->BindMissShader(
		pShaderGroupName
		, MissIndex
		, pData
		, DataSize
	);
}
extern "C" _AnomalousExport void IShaderBindingTable_BindHitGroupForInstance(
	IShaderBindingTable* objPtr
, ITopLevelAS* pTLAS, char* pInstanceName, Uint32 RayOffsetInHitGroupIndex, char* pShaderGroupName, void* pData, Uint32 DataSize)
{
	objPtr->BindHitGroupForInstance(
		pTLAS
		, pInstanceName
		, RayOffsetInHitGroupIndex
		, pShaderGroupName
		, pData
		, DataSize
	);
}
extern "C" _AnomalousExport void IShaderBindingTable_BindHitGroupForTLAS(
	IShaderBindingTable* objPtr
, ITopLevelAS* pTLAS, Uint32 RayOffsetInHitGroupIndex, char* pShaderGroupName, void* pData, Uint32 DataSize)
{
	objPtr->BindHitGroupForTLAS(
		pTLAS
		, RayOffsetInHitGroupIndex
		, pShaderGroupName
		, pData
		, DataSize
	);
}
