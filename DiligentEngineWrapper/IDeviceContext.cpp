#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "Color.h"

using namespace Diligent;

extern "C" _AnomalousExport void IDeviceContext_Flush(IDeviceContext * objPtr)
{
	objPtr->Flush();
}

//extern "C" _AnomalousExport  void SetRenderTargets(IDeviceContext * objPtr,
//    Uint32 NumRenderTargets,
//    ITextureView * ppRenderTargets[],
//    ITextureView * pDepthStencil,
//    RESOURCE_STATE_TRANSITION_MODE StateTransitionMode) 
//{
//
//}

extern "C" _AnomalousExport void IDeviceContext_SetRenderTarget(IDeviceContext * objPtr,
	ITextureView * pRenderTarget,
	ITextureView * pDepthStencil,
	RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->SetRenderTargets(1, &pRenderTarget, pDepthStencil, StateTransitionMode);
}

extern "C" _AnomalousExport void IDeviceContext_ClearRenderTarget(IDeviceContext * objPtr,
	ITextureView * pView,
	Color RGBA,
	RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->ClearRenderTarget(pView, (float*)&RGBA, StateTransitionMode);
}

extern "C" _AnomalousExport void IDeviceContext_ClearDepthStencil(IDeviceContext * objPtr,
	ITextureView * pView,
	CLEAR_DEPTH_STENCIL_FLAGS      ClearFlags,
	float                          fDepth,
	Uint8                          Stencil,
	RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->ClearDepthStencil(pView,
		ClearFlags,
		fDepth,
		Stencil,
		StateTransitionMode);
}

extern "C" _AnomalousExport void IDeviceContext_SetPipelineState(IDeviceContext * objPtr, IPipelineState * pPipelineState)
{
	objPtr->SetPipelineState(pPipelineState);
}

extern "C" _AnomalousExport void IDeviceContext_CheaterDraw(IDeviceContext * m_pImmediateContext)
{
	DrawAttribs drawAttrs;
	drawAttrs.NumVertices = 3; // Render 3 vertices
	m_pImmediateContext->Draw(drawAttrs);
}