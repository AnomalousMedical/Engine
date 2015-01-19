#include "StdAfx.h"

extern "C" _AnomalousExport uint SystemInfo_getDisplayCount()
{
    return 1;
}

extern "C" _AnomalousExport void SystemInfo_getDisplayLocation(int displayIndex, int& x, int& y)
{
    x = 0;
    y = 0;
}