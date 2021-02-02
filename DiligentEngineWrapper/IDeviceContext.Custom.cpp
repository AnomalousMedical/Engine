#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/DeviceContext.h"

using namespace Diligent;

extern "C" _AnomalousExport void IDeviceContext_SetRenderTarget(IDeviceContext * objPtr,
	ITextureView * pRenderTarget,
	ITextureView * pDepthStencil,
	RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
{
	objPtr->SetRenderTargets(1, &pRenderTarget, pDepthStencil, StateTransitionMode);
}