#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/Texture.h"
using namespace Diligent;

extern "C" _AnomalousExport Uint32 ITexture_GetDesc_MipLevels(
	ITexture * objPtr
)
{
	return objPtr->GetDesc().MipLevels;
}

extern "C" _AnomalousExport Uint32 ITexture_GetDesc_Width(
	ITexture * objPtr
)
{
	return objPtr->GetDesc().Width;
}

extern "C" _AnomalousExport Uint32 ITexture_GetDesc_Height(
	ITexture * objPtr
)
{
	return objPtr->GetDesc().Height;
}