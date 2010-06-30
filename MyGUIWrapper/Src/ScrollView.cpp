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

extern "C" _AnomalousExport MyGUI::IntCoord ScrollView_getClientCoord(MyGUI::ScrollView* scrollView)
{
	return scrollView->getClientCoord();
}

#pragma warning(pop)