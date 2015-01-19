#include "stdafx.h"

namespace AnomalousOSHelper
{
	enum RuntimeOperatingSystem
	{
		Windows,
		Mac,
		WinRT,
        iOS,
	};
}

extern "C" _AnomalousExport AnomalousOSHelper::RuntimeOperatingSystem PlatformConfig_getPlatform()
{
#if WINDOWS
	return AnomalousOSHelper::Windows;
#elif WINRT
	return AnomalousOSHelper::WinRT;
#elif MAC_OSX
	return AnomalousOSHelper::Mac;
#elif APPLE_IOS
    return AnomalousOSHelper::iOS;
#endif
}