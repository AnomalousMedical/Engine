#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/TextureView.h"
using namespace Diligent;
extern "C" _AnomalousExport void ITextureView_SetSampler(
	ITextureView* objPtr
, ISampler* pSampler)
{
	objPtr->SetSampler(
		pSampler
	);
}
extern "C" _AnomalousExport  ITexture* ITextureView_GetTexture(
	ITextureView* objPtr
)
{
	return objPtr->GetTexture(
	);
}
