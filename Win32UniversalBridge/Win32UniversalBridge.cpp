// Win32UniversalBridge.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"


#include <Windows.UI.ViewManagement.h>
#include <UIViewSettingsInterop.h>
#include <windows.ui.viewmanagement.h>
#include <wrl.h>
#include <wrl\wrappers\corewrappers.h>
#include <wrl\client.h>

using namespace ABI::Windows::UI::ViewManagement;
using namespace Microsoft::WRL;
using namespace Microsoft::WRL::Wrappers;
using namespace Windows::Foundation;

#include "Win32UniversalBridge.h"

extern "C" _declspec(dllexport) HRESULT GetUserInteractionMode(int & iMode, HWND hWnd)
{
	iMode = UserInteractionMode_Mouse; //Just in case default to mouse mode

	//Make sure we are a "tablet pc"
	bool isTabletPC = (GetSystemMetrics(SM_TABLETPC) != 0);
	int state = GetSystemMetrics(SM_CONVERTIBLESLATEMODE);
	if ((state == 0) && isTabletPC)
	{
		ComPtr<IUIViewSettingsInterop> uiViewSettingsInterop;
		HRESULT hr = GetActivationFactory(
			HStringReference(RuntimeClass_Windows_UI_ViewManagement_UIViewSettings).Get(), &uiViewSettingsInterop);
		if (SUCCEEDED(hr))
		{
			ComPtr<IUIViewSettings> uiViewSettings;
			hr = uiViewSettingsInterop->GetForWindow(hWnd, IID_PPV_ARGS(&uiViewSettings));
			if (SUCCEEDED(hr))
			{
				UserInteractionMode mode;
				hr = uiViewSettings->get_UserInteractionMode(&mode);
				if (SUCCEEDED(hr))
				{
					switch (mode)
					{
					case UserInteractionMode_Mouse:
						iMode = UserInteractionMode_Mouse;
						break;
					case UserInteractionMode_Touch:
						iMode = UserInteractionMode_Touch;
						break;
					default:
						break;
					}
				}
			}
		}
	}
	return S_OK;
}