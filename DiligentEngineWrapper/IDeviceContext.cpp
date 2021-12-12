#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "Color.h"
#include "BLASBuildBoundingBoxData.PassStruct.h"
#include "BLASBuildTriangleData.PassStruct.h"
#include "TLASBuildInstanceData.PassStruct.h"
using namespace Diligent;
extern "C" _AnomalousExport void IDeviceContext_SetPipelineState(
	IDeviceContext* objPtr
, IPipelineState* pPipelineState)
{
	objPtr->SetPipelineState(
		pPipelineState
	);
}
extern "C" _AnomalousExport void IDeviceContext_CommitShaderResources(
	IDeviceContext* objPtr
, IShaderResourceBinding* pShaderResourceBinding, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->CommitShaderResources(
		pShaderResourceBinding
		, StateTransitionMode
	);
}
extern "C" _AnomalousExport void IDeviceContext_SetVertexBuffers(
	IDeviceContext* objPtr
, Uint32 StartSlot, Uint32 NumBuffersSet, IBuffer** ppBuffers, Uint32* pOffsets, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode, SET_VERTEX_BUFFERS_FLAGS Flags)
{
	objPtr->SetVertexBuffers(
		StartSlot
		, NumBuffersSet
		, ppBuffers
		, pOffsets
		, StateTransitionMode
		, Flags
	);
}
extern "C" _AnomalousExport void IDeviceContext_SetIndexBuffer(
	IDeviceContext* objPtr
, IBuffer* pIndexBuffer, Uint32 ByteOffset, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->SetIndexBuffer(
		pIndexBuffer
		, ByteOffset
		, StateTransitionMode
	);
}
extern "C" _AnomalousExport void IDeviceContext_Draw(
	IDeviceContext* objPtr
	, Uint32 Attribs_NumVertices
	, DRAW_FLAGS Attribs_Flags
	, Uint32 Attribs_NumInstances
	, Uint32 Attribs_StartVertexLocation
	, Uint32 Attribs_FirstInstanceLocation
)
{
	DrawAttribs Attribs;
	Attribs.NumVertices = Attribs_NumVertices;
	Attribs.Flags = Attribs_Flags;
	Attribs.NumInstances = Attribs_NumInstances;
	Attribs.StartVertexLocation = Attribs_StartVertexLocation;
	Attribs.FirstInstanceLocation = Attribs_FirstInstanceLocation;
	objPtr->Draw(
		Attribs
	);
}
extern "C" _AnomalousExport void IDeviceContext_DrawIndexed(
	IDeviceContext* objPtr
	, Uint32 Attribs_NumIndices
	, VALUE_TYPE Attribs_IndexType
	, DRAW_FLAGS Attribs_Flags
	, Uint32 Attribs_NumInstances
	, Uint32 Attribs_FirstIndexLocation
	, Uint32 Attribs_BaseVertex
	, Uint32 Attribs_FirstInstanceLocation
)
{
	DrawIndexedAttribs Attribs;
	Attribs.NumIndices = Attribs_NumIndices;
	Attribs.IndexType = Attribs_IndexType;
	Attribs.Flags = Attribs_Flags;
	Attribs.NumInstances = Attribs_NumInstances;
	Attribs.FirstIndexLocation = Attribs_FirstIndexLocation;
	Attribs.BaseVertex = Attribs_BaseVertex;
	Attribs.FirstInstanceLocation = Attribs_FirstInstanceLocation;
	objPtr->DrawIndexed(
		Attribs
	);
}
extern "C" _AnomalousExport void IDeviceContext_ClearDepthStencil(
	IDeviceContext* objPtr
, ITextureView* pView, CLEAR_DEPTH_STENCIL_FLAGS ClearFlags, float fDepth, Uint8 Stencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->ClearDepthStencil(
		pView
		, ClearFlags
		, fDepth
		, Stencil
		, StateTransitionMode
	);
}
extern "C" _AnomalousExport void IDeviceContext_ClearRenderTarget(
	IDeviceContext* objPtr
, ITextureView* pView, Color RGBA, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->ClearRenderTarget(
		pView
		, (float*)&RGBA
		, StateTransitionMode
	);
}
extern "C" _AnomalousExport void IDeviceContext_Flush(
	IDeviceContext* objPtr
)
{
	objPtr->Flush(
	);
}
extern "C" _AnomalousExport void IDeviceContext_UpdateBuffer(
	IDeviceContext* objPtr
, IBuffer* pBuffer, Uint32 Offset, Uint32 Size, void* pData, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->UpdateBuffer(
		pBuffer
		, Offset
		, Size
		, pData
		, StateTransitionMode
	);
}
extern "C" _AnomalousExport PVoid IDeviceContext_MapBuffer(
	IDeviceContext* objPtr
, IBuffer* pBuffer, MAP_TYPE MapType, MAP_FLAGS MapFlags)
{
	PVoid theReturnValue;
	objPtr->MapBuffer(
		pBuffer
		, MapType
		, MapFlags
		, theReturnValue
	);
	return theReturnValue;
}
extern "C" _AnomalousExport void IDeviceContext_UnmapBuffer(
	IDeviceContext* objPtr
, IBuffer* pBuffer, MAP_TYPE MapType)
{
	objPtr->UnmapBuffer(
		pBuffer
		, MapType
	);
}
extern "C" _AnomalousExport void IDeviceContext_BuildBLAS(
	IDeviceContext* objPtr
	, IBottomLevelAS* Attribs_pBLAS
	, RESOURCE_STATE_TRANSITION_MODE Attribs_BLASTransitionMode
	, RESOURCE_STATE_TRANSITION_MODE Attribs_GeometryTransitionMode
	, BLASBuildTriangleDataPassStruct* Attribs_pTriangleData
	, Uint32 Attribs_TriangleDataCount
	, BLASBuildBoundingBoxDataPassStruct* Attribs_pBoxData
	, Uint32 Attribs_BoxDataCount
	, IBuffer* Attribs_pScratchBuffer
	, Uint32 Attribs_ScratchBufferOffset
	, RESOURCE_STATE_TRANSITION_MODE Attribs_ScratchBufferTransitionMode
	, Bool Attribs_Update
)
{
	BuildBLASAttribs Attribs;
	Attribs.pBLAS = Attribs_pBLAS;
	Attribs.BLASTransitionMode = Attribs_BLASTransitionMode;
	Attribs.GeometryTransitionMode = Attribs_GeometryTransitionMode;
	BLASBuildTriangleData* Attribs_pTriangleData_Native_Array = new BLASBuildTriangleData[Attribs_TriangleDataCount];
	if(Attribs_TriangleDataCount > 0)
	{
		for (Uint32 i = 0; i < Attribs_TriangleDataCount; ++i)
		{
	    Attribs_pTriangleData_Native_Array[i].GeometryName = Attribs_pTriangleData[i].GeometryName;
	    Attribs_pTriangleData_Native_Array[i].pVertexBuffer = Attribs_pTriangleData[i].pVertexBuffer;
	    Attribs_pTriangleData_Native_Array[i].VertexOffset = Attribs_pTriangleData[i].VertexOffset;
	    Attribs_pTriangleData_Native_Array[i].VertexStride = Attribs_pTriangleData[i].VertexStride;
	    Attribs_pTriangleData_Native_Array[i].VertexCount = Attribs_pTriangleData[i].VertexCount;
	    Attribs_pTriangleData_Native_Array[i].VertexValueType = Attribs_pTriangleData[i].VertexValueType;
	    Attribs_pTriangleData_Native_Array[i].VertexComponentCount = Attribs_pTriangleData[i].VertexComponentCount;
	    Attribs_pTriangleData_Native_Array[i].PrimitiveCount = Attribs_pTriangleData[i].PrimitiveCount;
	    Attribs_pTriangleData_Native_Array[i].pIndexBuffer = Attribs_pTriangleData[i].pIndexBuffer;
	    Attribs_pTriangleData_Native_Array[i].IndexOffset = Attribs_pTriangleData[i].IndexOffset;
	    Attribs_pTriangleData_Native_Array[i].IndexType = Attribs_pTriangleData[i].IndexType;
	    Attribs_pTriangleData_Native_Array[i].pTransformBuffer = Attribs_pTriangleData[i].pTransformBuffer;
	    Attribs_pTriangleData_Native_Array[i].TransformBufferOffset = Attribs_pTriangleData[i].TransformBufferOffset;
	    Attribs_pTriangleData_Native_Array[i].Flags = Attribs_pTriangleData[i].Flags;
		}
		Attribs.pTriangleData = Attribs_pTriangleData_Native_Array;  
	}
	Attribs.TriangleDataCount = Attribs_TriangleDataCount;
	BLASBuildBoundingBoxData* Attribs_pBoxData_Native_Array = new BLASBuildBoundingBoxData[Attribs_BoxDataCount];
	if(Attribs_BoxDataCount > 0)
	{
		for (Uint32 i = 0; i < Attribs_BoxDataCount; ++i)
		{
	    Attribs_pBoxData_Native_Array[i].GeometryName = Attribs_pBoxData[i].GeometryName;
	    Attribs_pBoxData_Native_Array[i].pBoxBuffer = Attribs_pBoxData[i].pBoxBuffer;
	    Attribs_pBoxData_Native_Array[i].BoxOffset = Attribs_pBoxData[i].BoxOffset;
	    Attribs_pBoxData_Native_Array[i].BoxStride = Attribs_pBoxData[i].BoxStride;
	    Attribs_pBoxData_Native_Array[i].BoxCount = Attribs_pBoxData[i].BoxCount;
	    Attribs_pBoxData_Native_Array[i].Flags = Attribs_pBoxData[i].Flags;
		}
		Attribs.pBoxData = Attribs_pBoxData_Native_Array;  
	}
	Attribs.BoxDataCount = Attribs_BoxDataCount;
	Attribs.pScratchBuffer = Attribs_pScratchBuffer;
	Attribs.ScratchBufferOffset = Attribs_ScratchBufferOffset;
	Attribs.ScratchBufferTransitionMode = Attribs_ScratchBufferTransitionMode;
	Attribs.Update = Attribs_Update;
	objPtr->BuildBLAS(
		Attribs
	);
    delete[] Attribs_pTriangleData_Native_Array;
    delete[] Attribs_pBoxData_Native_Array;
}
extern "C" _AnomalousExport void IDeviceContext_BuildTLAS(
	IDeviceContext* objPtr
	, ITopLevelAS* Attribs_pTLAS
	, RESOURCE_STATE_TRANSITION_MODE Attribs_TLASTransitionMode
	, RESOURCE_STATE_TRANSITION_MODE Attribs_BLASTransitionMode
	, TLASBuildInstanceDataPassStruct* Attribs_pInstances
	, Uint32 Attribs_InstanceCount
	, IBuffer* Attribs_pInstanceBuffer
	, Uint32 Attribs_InstanceBufferOffset
	, RESOURCE_STATE_TRANSITION_MODE Attribs_InstanceBufferTransitionMode
	, Uint32 Attribs_HitGroupStride
	, Uint32 Attribs_BaseContributionToHitGroupIndex
	, HIT_GROUP_BINDING_MODE Attribs_BindingMode
	, IBuffer* Attribs_pScratchBuffer
	, Uint32 Attribs_ScratchBufferOffset
	, RESOURCE_STATE_TRANSITION_MODE Attribs_ScratchBufferTransitionMode
	, Bool Attribs_Update
)
{
	BuildTLASAttribs Attribs;
	Attribs.pTLAS = Attribs_pTLAS;
	Attribs.TLASTransitionMode = Attribs_TLASTransitionMode;
	Attribs.BLASTransitionMode = Attribs_BLASTransitionMode;
	TLASBuildInstanceData* Attribs_pInstances_Native_Array = new TLASBuildInstanceData[Attribs_InstanceCount];
	if(Attribs_InstanceCount > 0)
	{
		for (Uint32 i = 0; i < Attribs_InstanceCount; ++i)
		{
	    Attribs_pInstances_Native_Array[i].InstanceName = Attribs_pInstances[i].InstanceName;
	    Attribs_pInstances_Native_Array[i].pBLAS = Attribs_pInstances[i].pBLAS;
	    Attribs_pInstances_Native_Array[i].Transform = Attribs_pInstances[i].Transform;
	    Attribs_pInstances_Native_Array[i].CustomId = Attribs_pInstances[i].CustomId;
	    Attribs_pInstances_Native_Array[i].Flags = Attribs_pInstances[i].Flags;
	    Attribs_pInstances_Native_Array[i].Mask = Attribs_pInstances[i].Mask;
	    Attribs_pInstances_Native_Array[i].ContributionToHitGroupIndex = Attribs_pInstances[i].ContributionToHitGroupIndex;
		}
		Attribs.pInstances = Attribs_pInstances_Native_Array;  
	}
	Attribs.InstanceCount = Attribs_InstanceCount;
	Attribs.pInstanceBuffer = Attribs_pInstanceBuffer;
	Attribs.InstanceBufferOffset = Attribs_InstanceBufferOffset;
	Attribs.InstanceBufferTransitionMode = Attribs_InstanceBufferTransitionMode;
	Attribs.HitGroupStride = Attribs_HitGroupStride;
	Attribs.BaseContributionToHitGroupIndex = Attribs_BaseContributionToHitGroupIndex;
	Attribs.BindingMode = Attribs_BindingMode;
	Attribs.pScratchBuffer = Attribs_pScratchBuffer;
	Attribs.ScratchBufferOffset = Attribs_ScratchBufferOffset;
	Attribs.ScratchBufferTransitionMode = Attribs_ScratchBufferTransitionMode;
	Attribs.Update = Attribs_Update;
	objPtr->BuildTLAS(
		Attribs
	);
    delete[] Attribs_pInstances_Native_Array;
}
extern "C" _AnomalousExport void IDeviceContext_TraceRays(
	IDeviceContext* objPtr
	, IShaderBindingTable* Attribs_pSBT
	, Uint32 Attribs_DimensionX
	, Uint32 Attribs_DimensionY
	, Uint32 Attribs_DimensionZ
)
{
	TraceRaysAttribs Attribs;
	Attribs.pSBT = Attribs_pSBT;
	Attribs.DimensionX = Attribs_DimensionX;
	Attribs.DimensionY = Attribs_DimensionY;
	Attribs.DimensionZ = Attribs_DimensionZ;
	objPtr->TraceRays(
		Attribs
	);
}
