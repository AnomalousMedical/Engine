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
extern "C" _AnomalousExport const Char* ShaderCreateInfo_Get_FilePath(ShaderCreateInfo* objPtr)
{
    return objPtr->FilePath;
}

extern "C" _AnomalousExport void ShaderCreateInfo_Set_FilePath(ShaderCreateInfo* objPtr, const Char* value)
{
    objPtr->FilePath = value;
}

extern "C" _AnomalousExport const Char* ShaderCreateInfo_Get_Source(ShaderCreateInfo* objPtr)
{
    return objPtr->Source;
}

extern "C" _AnomalousExport void ShaderCreateInfo_Set_Source(ShaderCreateInfo* objPtr, const Char* value)
{
    objPtr->Source = value;
}

extern "C" _AnomalousExport const Char* ShaderCreateInfo_Get_EntryPoint(ShaderCreateInfo* objPtr)
{
    return objPtr->EntryPoint;
}

extern "C" _AnomalousExport void ShaderCreateInfo_Set_EntryPoint(ShaderCreateInfo* objPtr, const Char* value)
{
    objPtr->EntryPoint = value;
}

