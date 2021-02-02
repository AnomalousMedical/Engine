#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/PipelineState.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"

#include "Common/interface/RefCntAutoPtr.hpp"

using namespace Diligent;

extern "C" _AnomalousExport GraphicsPipelineStateCreateInfo * GraphicsPipelineStateCreateInfo_Create()
{
	return new GraphicsPipelineStateCreateInfo;
}

extern "C" _AnomalousExport void GraphicsPipelineStateCreateInfo_Delete(GraphicsPipelineStateCreateInfo * obj)
{
	delete obj;
}