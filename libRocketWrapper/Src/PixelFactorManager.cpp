#include "StdAfx.h"
#include <Rocket/Core/PixelFactorManager.h>

extern "C" _AnomalousExport float PixelFactorManager_GetPixelFactor()
{
	return Rocket::Core::PixelFactorManager::GetPixelFactor();
}

extern "C" _AnomalousExport void PixelFactorManager_SetPixelFactor(float value)
{
	return Rocket::Core::PixelFactorManager::SetPixelFactor(value);
}