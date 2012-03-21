#include "Stdafx.h"

extern "C" _AnomalousExport void Button_setStateSelected(MyGUI::Button* button, bool value)
{
	button->setStateSelected(value);
}

extern "C" _AnomalousExport bool Button_getStateSelected(MyGUI::Button* button)
{
	return button->getStateSelected();
}

extern "C" _AnomalousExport void Button_setModeImage(MyGUI::Button* button, bool value)
{
	button->setModeImage(value);
}

extern "C" _AnomalousExport bool Button_getModeImage(MyGUI::Button* button)
{
	return button->getModeImage();
}

extern "C" _AnomalousExport MyGUI::ImageBox* Button__getImageBox(MyGUI::Button* button)
{
	return button->_getImageBox();
}