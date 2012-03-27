#include "Stdafx.h"

extern "C" _AnomalousExport void ProgressBar_setProgressRange(MyGUI::ProgressBar* progress, size_t value)
{
	progress->setProgressRange(value);
}

extern "C" _AnomalousExport size_t ProgressBar_getProgressRange(MyGUI::ProgressBar* progress)
{
	return progress->getProgressRange();
}

extern "C" _AnomalousExport void ProgressBar_setProgressPosition(MyGUI::ProgressBar* progress, size_t value)
{
	progress->setProgressPosition(value);
}

extern "C" _AnomalousExport size_t ProgressBar_getProgressPosition(MyGUI::ProgressBar* progress)
{
	return progress->getProgressPosition();
}

extern "C" _AnomalousExport void ProgressBar_setProgressAutoTrack(MyGUI::ProgressBar* progress, bool value)
{
	progress->setProgressAutoTrack(value);
}

extern "C" _AnomalousExport bool ProgressBar_getProgressAutoTrack(MyGUI::ProgressBar* progress)
{
	return progress->getProgressAutoTrack();
}

extern "C" _AnomalousExport void ProgressBar_setFlowDirection(MyGUI::ProgressBar* progress, MyGUI::FlowDirection::Enum value)
{
	progress->setFlowDirection(value);
}

extern "C" _AnomalousExport MyGUI::FlowDirection::Enum ProgressBar_getFlowDirection(MyGUI::ProgressBar* progress)
{
	return getFlowDirectionEnumValue(progress->getFlowDirection());
}