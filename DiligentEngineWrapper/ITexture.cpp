#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/Texture.h"
using namespace Diligent;
extern "C" _AnomalousExport ITextureView* ITexture_GetDefaultView(
	ITexture* objPtr
, TEXTURE_VIEW_TYPE ViewType)
{
	return objPtr->GetDefaultView(
		ViewType
	);
}
