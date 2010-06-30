#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

MyGUIEventTranslator::MyGUIEventTranslator(void)
{
}

MyGUIEventTranslator::~MyGUIEventTranslator(void)
{
}

extern "C" _AnomalousExport void MyGUIEventTranslator_delete(MyGUIEventTranslator* nativeEventTranslator)
{
	delete nativeEventTranslator;
}

extern "C" _AnomalousExport void MyGUIEventTranslator_bindEvent(MyGUIEventTranslator* nativeEventTranslator)
{
	nativeEventTranslator->bindEvent();
}

extern "C" _AnomalousExport void MyGUIEventTranslator_unbindEvent(MyGUIEventTranslator* nativeEventTranslator)
{
	nativeEventTranslator->unbindEvent();
}