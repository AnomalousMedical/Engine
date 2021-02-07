#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/Texture.h"
using namespace Diligent;
extern "C" _AnomalousExport ITextureView* ITexture_CreateView(
	ITexture* objPtr
	, TEXTURE_VIEW_TYPE ViewDesc_ViewType
	, RESOURCE_DIMENSION ViewDesc_TextureDim
	, TEXTURE_FORMAT ViewDesc_Format
	, Uint32 ViewDesc_MostDetailedMip
	, Uint32 ViewDesc_NumMipLevels
	, Uint32 ViewDesc_FirstArraySlice
	, Uint32 ViewDesc_NumArraySlices
	, UAV_ACCESS_FLAG ViewDesc_AccessFlags
	, TEXTURE_VIEW_FLAGS ViewDesc_Flags
	, Char* ViewDesc_Name
)
{
	TextureViewDesc ViewDesc;
	ViewDesc.ViewType = ViewDesc_ViewType;
	ViewDesc.TextureDim = ViewDesc_TextureDim;
	ViewDesc.Format = ViewDesc_Format;
	ViewDesc.MostDetailedMip = ViewDesc_MostDetailedMip;
	ViewDesc.NumMipLevels = ViewDesc_NumMipLevels;
	ViewDesc.FirstArraySlice = ViewDesc_FirstArraySlice;
	ViewDesc.NumArraySlices = ViewDesc_NumArraySlices;
	ViewDesc.AccessFlags = ViewDesc_AccessFlags;
	ViewDesc.Flags = ViewDesc_Flags;
	ViewDesc.Name = ViewDesc_Name;
	ITextureView* theReturnValue = nullptr;
	objPtr->CreateView(
		ViewDesc
		, &theReturnValue
	);
	return theReturnValue;
}
extern "C" _AnomalousExport ITextureView* ITexture_GetDefaultView(
	ITexture* objPtr
, TEXTURE_VIEW_TYPE ViewType)
{
	return objPtr->GetDefaultView(
		ViewType
	);
}
