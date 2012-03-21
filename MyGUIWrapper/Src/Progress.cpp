#include "Stdafx.h"

extern "C" _AnomalousExport void Progress_setProgressRange(MyGUI::ProgressBar* progress, size_t value)
{
	progress->setProgressRange(value);
}

extern "C" _AnomalousExport size_t Progress_getProgressRange(MyGUI::ProgressBar* progress)
{
	return progress->getProgressRange();
}

extern "C" _AnomalousExport void Progress_setProgressPosition(MyGUI::ProgressBar* progress, size_t value)
{
	progress->setProgressPosition(value);
}

extern "C" _AnomalousExport size_t Progress_getProgressPosition(MyGUI::ProgressBar* progress)
{
	return progress->getProgressPosition();
}

extern "C" _AnomalousExport void Progress_setProgressAutoTrack(MyGUI::ProgressBar* progress, bool value)
{
	progress->setProgressAutoTrack(value);
}

extern "C" _AnomalousExport bool Progress_getProgressAutoTrack(MyGUI::ProgressBar* progress)
{
	return progress->getProgressAutoTrack();
}

extern "C" _AnomalousExport void Progress_setProgressStartPoint(MyGUI::ProgressBar* progress, MyGUI::Align::Enum value)
{
	progress->setProgressStartPoint(value);
}

extern "C" _AnomalousExport MyGUI::Align::Enum Progress_getProgressStartPoint(MyGUI::ProgressBar* progress)
{
	return getAlignEnumVal(progress->getProgressStartPoint());
}