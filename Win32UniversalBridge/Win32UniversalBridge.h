#pragma once

enum UniversalAppInteractionMode
{
	UniversalAppInteractionMode_Mouse = 0,
	UniversalAppInteractionMode_Touch = 1
};

typedef HRESULT(*GetUserInteractionModeFunc)(int& mode, HWND hWnd);