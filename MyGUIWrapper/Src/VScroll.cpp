#include "Stdafx.h"

extern "C" _AnomalousExport void VScroll_setScrollRange(MyGUI::VScroll* scroll, size_t value)
{
	scroll->setScrollRange(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollRange(MyGUI::VScroll* scroll)
{
	return scroll->getScrollRange();
}

extern "C" _AnomalousExport void VScroll_setScrollPosition(MyGUI::VScroll* scroll, size_t value)
{
	scroll->setScrollPosition(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollPosition(MyGUI::VScroll* scroll)
{
	return scroll->getScrollPosition();
}

extern "C" _AnomalousExport void VScroll_setScrollPage(MyGUI::VScroll* scroll, size_t value)
{
	scroll->setScrollPage(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollPage(MyGUI::VScroll* scroll)
{
	return scroll->getScrollPage();
}

extern "C" _AnomalousExport void VScroll_setScrollViewPage(MyGUI::VScroll* scroll, size_t value)
{
	scroll->setScrollViewPage(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollViewPage(MyGUI::VScroll* scroll)
{
	return scroll->getScrollViewPage();
}

extern "C" _AnomalousExport int VScroll_getLineSize(MyGUI::VScroll* scroll)
{
	return scroll->getLineSize();
}

extern "C" _AnomalousExport void VScroll_setTrackSize(MyGUI::VScroll* scroll, int value)
{
	scroll->setTrackSize(value);
}

extern "C" _AnomalousExport int VScroll_getTrackSize(MyGUI::VScroll* scroll)
{
	return scroll->getTrackSize();
}

extern "C" _AnomalousExport void VScroll_setMinTrackSize(MyGUI::VScroll* scroll, int value)
{
	scroll->setMinTrackSize(value);
}

extern "C" _AnomalousExport int VScroll_getMinTrackSize(MyGUI::VScroll* scroll)
{
	return scroll->getMinTrackSize();
}

extern "C" _AnomalousExport void VScroll_setMoveToClick(MyGUI::VScroll* scroll, bool value)
{
	scroll->setMoveToClick(value);
}

extern "C" _AnomalousExport bool VScroll_getMoveToClick(MyGUI::VScroll* scroll)
{
	return scroll->getMoveToClick();
}