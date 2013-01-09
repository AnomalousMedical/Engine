#include "StdAfx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport void TabControl_removeItemAt(MyGUI::TabControl* tabControl, size_t index)
{
	return tabControl->removeItemAt(index);
}

extern "C" _AnomalousExport void TabControl_setIndexSelected(MyGUI::TabControl* tabControl, size_t index)
{
	return tabControl->setIndexSelected(index);
}

extern "C" _AnomalousExport size_t TabControl_getIndexSelected(MyGUI::TabControl* tabControl)
{
	return tabControl->getIndexSelected();
}

extern "C" _AnomalousExport MyGUI::TabItem* TabControl_insertItemAt(MyGUI::TabControl* tabControl, size_t _index, UStringIn _name)
{
	return tabControl->insertItemAt(_index, _name);
}

extern "C" _AnomalousExport MyGUI::TabItem* TabControl_addItem(MyGUI::TabControl* tabControl, UStringIn _name)
{
	return tabControl->addItem(_name);
}