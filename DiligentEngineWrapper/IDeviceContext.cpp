#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport void IDeviceContext_SetPipelineState(
	IDeviceContext* objPtr
, IPipelineState* pPipelineState)
{
	objPtr->SetPipelineState(
		pPipelineState
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
