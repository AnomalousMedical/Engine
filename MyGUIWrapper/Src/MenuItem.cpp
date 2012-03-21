#include "Stdafx.h"

extern "C" _AnomalousExport void MenuItem_setItemName(MyGUI::MenuItem* menuItem, UStringIn value)
{
	menuItem->setItemName(value);
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* MenuItem_getItemName(MyGUI::MenuItem* menuItem)
{
	return menuItem->getItemName().c_str();
}

extern "C" _AnomalousExport void MenuItem_removeItem(MyGUI::MenuItem* menuItem)
{
	menuItem->removeItem();
}

extern "C" _AnomalousExport void MenuItem_setItemId(MyGUI::MenuItem* menuItem, String value)
{
	menuItem->setItemId(value);
}

extern "C" _AnomalousExport String MenuItem_getItemId(MyGUI::MenuItem* menuItem)
{
	return menuItem->getItemId().c_str();
}

extern "C" _AnomalousExport size_t MenuItem_getItemIndex(MyGUI::MenuItem* menuItem)
{
	return menuItem->getItemIndex();
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuItem_createItemChild(MyGUI::MenuItem* menuItem)
{
	return menuItem->createItemChild();
}

extern "C" _AnomalousExport void MenuItem_setItemType(MyGUI::MenuItem* menuItem, MyGUI::MenuItemType::Enum value)
{
	menuItem->setItemType(value);
}

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuItem_getItemType(MyGUI::MenuItem* menuItem)
{
	return getMenuItemTypeEnumVal(menuItem->getItemType());
}

extern "C" _AnomalousExport void MenuItem_setItemChildVisible(MyGUI::MenuItem* menuItem, bool value)
{
	menuItem->setItemChildVisible(value);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuItem_getMenuCtrlParent(MyGUI::MenuItem* menuItem)
{
	return menuItem->getMenuCtrlParent();
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuItem_getItemChild(MyGUI::MenuItem* menuItem)
{
	return menuItem->getItemChild();
}