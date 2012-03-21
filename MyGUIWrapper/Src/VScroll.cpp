#include "Stdafx.h"

extern "C" _AnomalousExport void VScroll_setScrollRange(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollRange(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollRange(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollRange();
}

extern "C" _AnomalousExport void VScroll_setScrollPosition(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollPosition(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollPosition(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollPosition();
}

extern "C" _AnomalousExport void VScroll_setScrollPage(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollPage(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollPage(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollPage();
}

extern "C" _AnomalousExport void VScroll_setScrollViewPage(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollViewPage(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollViewPage(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollViewPage();
}

extern "C" _AnomalousExport void VScroll_setScrollIncrement(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollIncrement(value);
}

extern "C" _AnomalousExport size_t VScroll_getScrollIncrement(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollIncrement();
}

extern "C" _AnomalousExport int VScroll_getLineSize(MyGUI::ScrollBar* scroll)
{
	return scroll->getLineSize();
}

extern "C" _AnomalousExport void VScroll_setTrackSize(MyGUI::ScrollBar* scroll, int value)
{
	scroll->setTrackSize(value);
}

extern "C" _AnomalousExport int VScroll_getTrackSize(MyGUI::ScrollBar* scroll)
{
	return scroll->getTrackSize();
}

extern "C" _AnomalousExport void VScroll_setMinTrackSize(MyGUI::ScrollBar* scroll, int value)
{
	scroll->setMinTrackSize(value);
}

extern "C" _AnomalousExport int VScroll_getMinTrackSize(MyGUI::ScrollBar* scroll)
{
	return scroll->getMinTrackSize();
}

extern "C" _AnomalousExport void VScroll_setMoveToClick(MyGUI::ScrollBar* scroll, bool value)
{
	scroll->setMoveToClick(value);
}

extern "C" _AnomalousExport bool VScroll_getMoveToClick(MyGUI::ScrollBar* scroll)
{
	return scroll->getMoveToClick();
}