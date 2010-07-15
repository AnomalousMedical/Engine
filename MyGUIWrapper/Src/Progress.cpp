#include "Stdafx.h"

extern "C" _AnomalousExport void Progress_setProgressRange(MyGUI::Progress* progress, size_t value)
{
	progress->setProgressRange(value);
}

extern "C" _AnomalousExport size_t Progress_getProgressRange(MyGUI::Progress* progress)
{
	return progress->getProgressRange();
}

extern "C" _AnomalousExport void Progress_setProgressPosition(MyGUI::Progress* progress, size_t value)
{
	progress->setProgressPosition(value);
}

extern "C" _AnomalousExport size_t Progress_getProgressPosition(MyGUI::Progress* progress)
{
	return progress->getProgressPosition();
}

extern "C" _AnomalousExport void Progress_setProgressAutoTrack(MyGUI::Progress* progress, bool value)
{
	progress->setProgressAutoTrack(value);
}

extern "C" _AnomalousExport bool Progress_getProgressAutoTrack(MyGUI::Progress* progress)
{
	return progress->getProgressAutoTrack();
}

extern "C" _AnomalousExport void Progress_setProgressStartPoint(MyGUI::Progress* progress, MyGUI::Align::Enum value)
{
	progress->setProgressStartPoint(value);
}

extern "C" _AnomalousExport MyGUI::Align::Enum Progress_getProgressStartPoint(MyGUI::Progress* progress)
{
	return getAlignEnumVal(progress->getProgressStartPoint());
}