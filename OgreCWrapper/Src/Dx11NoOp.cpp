#include "Stdafx.h"
#include "D3D11.h"

/*
This file is needed if you use the Dx11 renderer through a dll. If you don't include this (and link to d3d11.dll) when Ogre
unloads the dx11 renderer it will also unload dx11.dll, which is needed later in the shutdown process. When this happens
you will get crashes on close.

By including this file we make OgreCWrapper also dependent on d3d11.dll and it will therefore stay loaded after Ogre shuts
down. Nothing ever actually calls this function but since we export it the linker will still think we need d3d11.dll.
*/

//Do not EVER call this, it is not correct.
extern "C" _AnomalousExport void Dx11_NoOp()
{	
	//THIS IS NOT A VALID WAY TO BUILD A D3D11 device, we just need it to compile in.
	DWORD createDeviceFlags = 0;
	D3D_FEATURE_LEVEL lvl[] = { D3D_FEATURE_LEVEL_9_1 };
	ID3D11Device* pDevice = nullptr;
	D3D_DRIVER_TYPE driverType = D3D_DRIVER_TYPE_HARDWARE;
	HRESULT hr = D3D11CreateDevice(nullptr, driverType, nullptr, createDeviceFlags, lvl, _countof(lvl), D3D11_SDK_VERSION, &pDevice, nullptr, nullptr);
}