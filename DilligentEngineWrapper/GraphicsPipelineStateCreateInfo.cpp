#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/PipelineState.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"

#include "Common/interface/RefCntAutoPtr.hpp"

using namespace Diligent;

extern "C" _AnomalousExport GraphicsPipelineStateCreateInfo * GraphicsPipelineStateCreateInfo_Create()
{

    GraphicsPipelineStateCreateInfo* PSOCreateInfo = new GraphicsPipelineStateCreateInfo;

    return PSOCreateInfo;
}

extern "C" _AnomalousExport void GraphicsPipelineStateCreateInfo_Delete(GraphicsPipelineStateCreateInfo * obj)
{
	delete obj;
}

extern "C" _AnomalousExport void GraphicsPipelineStateCreateInfo_Set_pVS(GraphicsPipelineStateCreateInfo * obj, IShader* value)
{
    obj->pVS = value;
}

//extern "C" _AnomalousExport IShader* GraphicsPipelineStateCreateInfo_Get_pVS(GraphicsPipelineStateCreateInfo * obj)
//{
//    return obj->pVS;
//}

extern "C" _AnomalousExport void GraphicsPipelineStateCreateInfo_Set_pPS(GraphicsPipelineStateCreateInfo * obj, IShader * value)
{
    obj->pPS = value;
}

//extern "C" _AnomalousExport IShader * GraphicsPipelineStateCreateInfo_Get_pPS(GraphicsPipelineStateCreateInfo * obj)
//{
//    return obj->pPS;
//}