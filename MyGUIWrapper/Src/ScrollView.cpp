#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

//ScrollView
extern "C" _AnomalousExport void ScrollView_setVisibleVScroll(MyGUI::ScrollView* scrollView, bool value)
{
	scrollView->setVisibleVScroll(value);
}

extern "C" _AnomalousExport bool ScrollView_isVisibleVScroll(MyGUI::ScrollView* scrollView)
{
	return scrollView->isVisibleVScroll();
}

extern "C" _AnomalousExport void ScrollView_setVisibleHScroll(MyGUI::ScrollView* scrollView, bool value)
{
	scrollView->setVisibleHScroll(value);
}

extern "C" _AnomalousExport bool ScrollView_isVisibleHScroll(MyGUI::ScrollView* scrollView)
{
	return scrollView->isVisibleHScroll();
}

extern "C" _AnomalousExport void ScrollView_setCanvasAlign(MyGUI::ScrollView* scrollView, MyGUI::Align::Enum value)
{
	scrollView->setCanvasAlign(value);
}

extern "C" _AnomalousExport MyGUI::Align::Enum ScrollView_getCanvasAlign(MyGUI::ScrollView* scrollView)
{
	return getAlignEnumVal(scrollView->getCanvasAlign());
}

extern "C" _AnomalousExport void ScrollView_setCanvasSize(MyGUI::ScrollView* scrollView, int width, int height)
{
	scrollView->setCanvasSize(width, height);
}

extern "C" _AnomalousExport ThreeIntHack ScrollView_getCanvasSize(MyGUI::ScrollView* scrollView)
{
	return scrollView->getCanvasSize();
}

extern "C" _AnomalousExport ThreeIntHack ScrollView_getCanvasPosition(MyGUI::ScrollView* scrollView)
{
	return scrollView->getCanvasPosition();
}

extern "C" _AnomalousExport void ScrollView_setCanvasPosition(MyGUI::ScrollView* scrollView, int x, int y)
{
	scrollView->setCanvasPosition(MyGUI::IntPoint(x, y));
}

extern "C" _AnomalousExport void ScrollView_setFavorVertical(MyGUI::ScrollView* scrollView, bool value)
{
	scrollView->setFavorVertical(value);
}

extern "C" _AnomalousExport bool ScrollView_getFavorVertical(MyGUI::ScrollView* scrollView)
{
	return scrollView->getFavorVertical();
}

extern "C" _AnomalousExport void ScrollView_setAllowMouseScroll(MyGUI::ScrollView* scrollView, bool value)
{
	scrollView->setAllowMouseScroll(value);
}

extern "C" _AnomalousExport bool ScrollView_getAllowMouseScroll(MyGUI::ScrollView* scrollView)
{
	return scrollView->getAllowMouseScroll();
}

extern "C" _AnomalousExport IntCoord ScrollView_getViewCoord(MyGUI::ScrollView* scrollView)
{
	return scrollView->getViewCoord();
}

#pragma warning(pop)