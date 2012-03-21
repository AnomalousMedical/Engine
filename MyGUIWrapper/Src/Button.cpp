#include "Stdafx.h"

extern "C" _AnomalousExport void Button_setStateCheck(MyGUI::Button* button, bool value)
{
	button->setStateCheck(value);
}

extern "C" _AnomalousExport bool Button_getStateCheck(MyGUI::Button* button)
{
	return button->getStateCheck();
}

extern "C" _AnomalousExport void Button_setImageIndex(MyGUI::Button* button, size_t value)
{
	button->setImageIndex(value);
}

extern "C" _AnomalousExport size_t Button_getImageIndex(MyGUI::Button* button)
{
	return button->getImageIndex();
}

extern "C" _AnomalousExport void Button_setModeImage(MyGUI::Button* button, bool value)
{
	button->setModeImage(value);
}

extern "C" _AnomalousExport bool Button_getModeImage(MyGUI::Button* button)
{
	return button->getModeImage();
}

extern "C" _AnomalousExport MyGUI::ImageBox* Button_getStaticImage(MyGUI::Button* button)
{
	return button->getStaticImage();
}