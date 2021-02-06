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
