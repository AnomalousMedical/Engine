#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport IBuffer * IRenderDevice_CreateBuffer_Null_Data(
	IRenderDevice * objPtr
	, Uint32 BuffDesc_uiSizeInBytes
	, BIND_FLAGS BuffDesc_BindFlags
	, USAGE BuffDesc_Usage
	, CPU_ACCESS_FLAGS BuffDesc_CPUAccessFlags
	, BUFFER_MODE BuffDesc_Mode
	, Uint32 BuffDesc_ElementByteStride
	, Uint64 BuffDesc_CommandQueueMask
	, Char * BuffDesc_Name
)
{
	BufferDesc BuffDesc;
	BuffDesc.uiSizeInBytes = BuffDesc_uiSizeInBytes;
	BuffDesc.BindFlags = BuffDesc_BindFlags;
	BuffDesc.Usage = BuffDesc_Usage;
	BuffDesc.CPUAccessFlags = BuffDesc_CPUAccessFlags;
	BuffDesc.Mode = BuffDesc_Mode;
	BuffDesc.ElementByteStride = BuffDesc_ElementByteStride;
	BuffDesc.CommandQueueMask = BuffDesc_CommandQueueMask;
	BuffDesc.Name = BuffDesc_Name;
	IBuffer* ppBuffer = nullptr;
	objPtr->CreateBuffer(
		BuffDesc
		, nullptr
		, &ppBuffer
	);
	return ppBuffer;
}