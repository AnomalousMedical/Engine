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

extern "C" _AnomalousExport MyGUI::MenuCtrl* MenuItem_createItemChild(MyGUI::MenuItem* menuItem)
{
	return menuItem->createItemChild();
}

extern "C" _AnomalousExport void MenuItem_setItemType(MyGUI::MenuItem* menuItem, MyGUI::MenuItemType::Enum value)
{
	menuItem->setItemType(value);
}

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuItem_getItemType(MyGUI::MenuItem* menuItem)
{
	MyGUI::MenuItemType type = menuItem->getItemType();
	if(type == MyGUI::MenuItemType::Normal)
	{
		return MyGUI::MenuItemType::Normal;
	}
	else if(type == MyGUI::MenuItemType::Popup)
	{
		return MyGUI::MenuItemType::Popup;
	}
	else if(type == MyGUI::MenuItemType::Separator)
	{
		return MyGUI::MenuItemType::Separator;
	}
	return MyGUI::MenuItemType::MAX;
}

extern "C" _AnomalousExport void MenuItem_setItemChildVisible(MyGUI::MenuItem* menuItem, bool value)
{
	menuItem->setItemChildVisible(value);
}

extern "C" _AnomalousExport MyGUI::MenuCtrl* MenuItem_getMenuCtrlParent(MyGUI::MenuItem* menuItem)
{
	return menuItem->getMenuCtrlParent();
}

extern "C" _AnomalousExport MyGUI::MenuCtrl* MenuItem_getItemChild(MyGUI::MenuItem* menuItem)
{
	return menuItem->getItemChild();
}