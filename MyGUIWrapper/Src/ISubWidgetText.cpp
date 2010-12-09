#include "StdAfx.h"

extern "C" _AnomalousExport void ISubWidgetText_setWordWrap(MyGUI::ISubWidgetText* subWidgetText, bool value)
{
	subWidgetText->setWordWrap(value);
}