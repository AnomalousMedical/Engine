#include "StdAfx.h"
#include "../DiligentTools/TextureLoader/interface/TextureLoader.h"
using namespace Diligent;
extern "C" _AnomalousExport ITexture* KtxLoader_CreateTextureFromKTX(
	void* pKTXData
	, size_t DataSize	
	, Char* TexLoadInfo_Name
	, USAGE TexLoadInfo_Usage
	, BIND_FLAGS TexLoadInfo_BindFlags
	, Uint32 TexLoadInfo_MipLevels
	, CPU_ACCESS_FLAGS TexLoadInfo_CPUAccessFlags
	, Bool TexLoadInfo_IsSRGB
	, Bool TexLoadInfo_GenerateMips
	, TEXTURE_FORMAT TexLoadInfo_Format
	, IRenderDevice* pDevice)
{
	TextureLoadInfo TexLoadInfo;
	TexLoadInfo.Name = TexLoadInfo_Name;
	TexLoadInfo.Usage = TexLoadInfo_Usage;
	TexLoadInfo.BindFlags = TexLoadInfo_BindFlags;
	TexLoadInfo.MipLevels = TexLoadInfo_MipLevels;
	TexLoadInfo.CPUAccessFlags = TexLoadInfo_CPUAccessFlags;
	TexLoadInfo.IsSRGB = TexLoadInfo_IsSRGB;
	TexLoadInfo.GenerateMips = TexLoadInfo_GenerateMips;
	TexLoadInfo.Format = TexLoadInfo_Format;
	ITexture* theReturnValue = nullptr;
	CreateTextureFromKTX(
		pKTXData
		, DataSize
		, TexLoadInfo
		, pDevice
		, &theReturnValue
	);
	return theReturnValue;
}
