#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport ShaderCreateInfo * ShaderCreateInfo_Create()
{
    return new ShaderCreateInfo;
}

extern "C" _AnomalousExport void ShaderCreateInfo_Delete(ShaderCreateInfo* obj)
{
    delete obj;
}