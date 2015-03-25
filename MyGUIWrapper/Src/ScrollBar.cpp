#include "Stdafx.h"

extern "C" _AnomalousExport void ScrollBar_setScrollRange(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollRange(value);
}

extern "C" _AnomalousExport size_t ScrollBar_getScrollRange(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollRange();
}

extern "C" _AnomalousExport void ScrollBar_setScrollPosition(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollPosition(value);
}

extern "C" _AnomalousExport size_t ScrollBar_getScrollPosition(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollPosition();
}

extern "C" _AnomalousExport void ScrollBar_setScrollPage(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollPage(value);
}

extern "C" _AnomalousExport size_t ScrollBar_getScrollPage(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollPage();
}

extern "C" _AnomalousExport void ScrollBar_setScrollViewPage(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollViewPage(value);
}

extern "C" _AnomalousExport size_t ScrollBar_getScrollViewPage(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollViewPage();
}

extern "C" _AnomalousExport void ScrollBar_setScrollWheelPage(MyGUI::ScrollBar* scroll, size_t value)
{
	scroll->setScrollWheelPage(value);
}

extern "C" _AnomalousExport size_t ScrollBar_getScrollWheelPage(MyGUI::ScrollBar* scroll)
{
	return scroll->getScrollWheelPage();
}

extern "C" _AnomalousExport int ScrollBar_getLineSize(MyGUI::ScrollBar* scroll)
{
	return scroll->getLineSize();
}

extern "C" _AnomalousExport void ScrollBar_setTrackSize(MyGUI::ScrollBar* scroll, int value)
{
	scroll->setTrackSize(value);
}

extern "C" _AnomalousExport int ScrollBar_getTrackSize(MyGUI::ScrollBar* scroll)
{
	return scroll->getTrackSize();
}

extern "C" _AnomalousExport void ScrollBar_setMinTrackSize(MyGUI::ScrollBar* scroll, int value)
{
	scroll->setMinTrackSize(value);
}

extern "C" _AnomalousExport int ScrollBar_getMinTrackSize(MyGUI::ScrollBar* scroll)
{
	return scroll->getMinTrackSize();
}

extern "C" _AnomalousExport void ScrollBar_setMoveToClick(MyGUI::ScrollBar* scroll, bool value)
{
	scroll->setMoveToClick(value);
}

extern "C" _AnomalousExport bool ScrollBar_getMoveToClick(MyGUI::ScrollBar* scroll)
{
	return scroll->getMoveToClick();
}