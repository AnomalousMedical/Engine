#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "StateTransitionDesc.PassStruct.h"

using namespace Diligent;

extern "C" _AnomalousExport void IDeviceContext_SetRenderTarget(IDeviceContext * objPtr,
	ITextureView * pRenderTarget,
	ITextureView * pDepthStencil,
	RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->SetRenderTargets(1, &pRenderTarget, pDepthStencil, StateTransitionMode);
}

extern "C" _AnomalousExport void IDeviceContext_SetRenderTargets(
	IDeviceContext * objPtr
	, Uint32 NumRenderTargets, ITextureView * ppRenderTargets[], ITextureView * pDepthStencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->SetRenderTargets(
		NumRenderTargets
		, ppRenderTargets
		, pDepthStencil
		, StateTransitionMode
	);
}

extern "C" _AnomalousExport void IDeviceContext_TransitionResourceStates(
	IDeviceContext * objPtr
	, Uint32 BarrierCount
	, StateTransitionDescPassStruct * pResourceBarriers)
{
	StateTransitionDesc* nativeArray = new StateTransitionDesc[BarrierCount];
	for (Uint32 i = 0; i < BarrierCount; ++i) 
	{
		nativeArray[i].ArraySliceCount = pResourceBarriers[i].ArraySliceCount;
		nativeArray[i].FirstArraySlice = pResourceBarriers[i].FirstArraySlice;
		nativeArray[i].FirstMipLevel = pResourceBarriers[i].FirstMipLevel;
		nativeArray[i].MipLevelsCount = pResourceBarriers[i].MipLevelsCount;
		nativeArray[i].NewState = pResourceBarriers[i].NewState;
		nativeArray[i].OldState = pResourceBarriers[i].OldState;
		nativeArray[i].pResource = pResourceBarriers[i].pResource;
		nativeArray[i].TransitionType = pResourceBarriers[i].TransitionType;
		nativeArray[i].UpdateResourceState = pResourceBarriers[i].UpdateResourceState;
	}
	objPtr->TransitionResourceStates(
		BarrierCount
		, nativeArray
	);
	delete[] nativeArray;
}