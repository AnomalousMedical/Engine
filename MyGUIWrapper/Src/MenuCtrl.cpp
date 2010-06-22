#include "Stdafx.h"

extern "C" _AnomalousExport size_t MenuCtrl_getItemCount(MyGUI::MenuCtrl* menuCtrl)
{
	return menuCtrl->getItemCount();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItemAt(MyGUI::MenuCtrl* menuCtrl, size_t index, UString name)
{
	return menuCtrl->insertItemAt(index, name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItemAt2(MyGUI::MenuCtrl* menuCtrl, size_t index, UString name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->insertItemAt(index, name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItemAt3(MyGUI::MenuCtrl* menuCtrl, size_t index, UString name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->insertItemAt(index, name, type, id);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItem(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* to, UString name)
{
	return menuCtrl->insertItem(to, name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItem2(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* to, UString name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->insertItem(to, name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItem3(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* to, UString name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->insertItem(to, name, type, id);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_addItem(MyGUI::MenuCtrl* menuCtrl, UString name)
{
	return menuCtrl->addItem(name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_addItem2(MyGUI::MenuCtrl* menuCtrl, UString name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->addItem(name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_addItem3(MyGUI::MenuCtrl* menuCtrl, UString name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->addItem(name, type, id);
}

extern "C" _AnomalousExport void MenuCtrl_removeItemAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	menuCtrl->removeItemAt(index);
}

extern "C" _AnomalousExport void MenuCtrl_removeItem(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	menuCtrl->removeItem(item);
}

extern "C" _AnomalousExport void MenuCtrl_removeAllItems(MyGUI::MenuCtrl* menuCtrl)
{
	menuCtrl->removeAllItems();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_getItemAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	return menuCtrl->getItemAt(index);
}

extern "C" _AnomalousExport size_t MenuCtrl_getItemIndex(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemIndex(item);
}

extern "C" _AnomalousExport size_t MenuCtrl_findItemIndex(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->findItemIndex(item);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_findItemWith(MyGUI::MenuCtrl* menuCtrl, UString name)
{
	return menuCtrl->findItemWith(name);
}

extern "C" _AnomalousExport void MenuCtrl_setItemIdAt(MyGUI::MenuCtrl* menuCtrl, size_t index, String id)
{
	menuCtrl->setItemIdAt(index, id);
}

extern "C" _AnomalousExport void MenuCtrl_setItemId(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item, String id)
{
	menuCtrl->setItemId(item, id);
}

extern "C" _AnomalousExport String MenuCtrl_getItemIdAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	return menuCtrl->getItemIdAt(index).c_str();
}

extern "C" _AnomalousExport String MenuCtrl_getItemId(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemId(item).c_str();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_getItemById(MyGUI::MenuCtrl* menuCtrl, String id)
{
	return menuCtrl->getItemById(id);
}

extern "C" _AnomalousExport size_t MenuCtrl_getItemIndexById(MyGUI::MenuCtrl* menuCtrl, String id)
{
	return menuCtrl->getItemIndexById(id);
}

extern "C" _AnomalousExport void MenuCtrl_setItemNameAt(MyGUI::MenuCtrl* menuCtrl, size_t index, UString name)
{
	menuCtrl->setItemNameAt(index, name);
}

extern "C" _AnomalousExport void MenuCtrl_setItemName(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item, UString name)
{
	menuCtrl->setItemName(item, name);
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* MenuCtrl_getItemNameAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	return menuCtrl->getItemNameAt(index).c_str();
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* MenuCtrl_getItemName(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemName(item).c_str();
}

extern "C" _AnomalousExport size_t MenuCtrl_findItemIndexWith(MyGUI::MenuCtrl* menuCtrl, UString name)
{
	return menuCtrl->findItemIndexWith(name);
}

extern "C" _AnomalousExport void MenuCtrl_setItemChildVisibleAt(MyGUI::MenuCtrl* menuCtrl, size_t index, bool visible)
{
	menuCtrl->setItemChildVisibleAt(index, visible);
}

extern "C" _AnomalousExport void MenuCtrl_setItemChildVisible(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item, bool visible)
{
	menuCtrl->setItemChildVisible(item, visible);
}

extern "C" _AnomalousExport MyGUI::MenuCtrl* MenuCtrl_getItemChildAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	return menuCtrl->getItemChildAt(index);
}

extern "C" _AnomalousExport MyGUI::MenuCtrl* MenuCtrl_getItemChild(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemChild(item);
}

extern "C" _AnomalousExport MyGUI::MenuCtrl* MenuCtrl_createItemChildAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	return menuCtrl->createItemChildAt(index);
}

extern "C" _AnomalousExport MyGUI::MenuCtrl* MenuCtrl_createItemChild(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->createItemChild(item);
}

extern "C" _AnomalousExport void MenuCtrl_removeItemChildAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	menuCtrl->removeItemChildAt(index);
}

extern "C" _AnomalousExport void MenuCtrl_removeItemChild(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	menuCtrl->removeItemChild(item);
}

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuCtrl_getItemTypeAt(MyGUI::MenuCtrl* menuCtrl, size_t index)
{
	MyGUI::MenuItemType type = menuCtrl->getItemTypeAt(index);
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

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuCtrl_getItemType(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item)
{
	MyGUI::MenuItemType type = menuCtrl->getItemType(item);
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

extern "C" _AnomalousExport void MenuCtrl_setItemTypeAt(MyGUI::MenuCtrl* menuCtrl, size_t index, MyGUI::MenuItemType::Enum type)
{
	menuCtrl->setItemTypeAt(index, type);
}

extern "C" _AnomalousExport void MenuCtrl_setItemType(MyGUI::MenuCtrl* menuCtrl, MyGUI::MenuItem* item, MyGUI::MenuItemType::Enum type)
{
	menuCtrl->setItemType(item, type);
}

extern "C" _AnomalousExport void MenuCtrl_setPopupAccept(MyGUI::MenuCtrl* menuCtrl, bool value)
{
	menuCtrl->setPopupAccept(value);
}

extern "C" _AnomalousExport bool MenuCtrl_getPopupAccept(MyGUI::MenuCtrl* menuCtrl)
{
	return menuCtrl->getPopupAccept();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_getMenuItemParent(MyGUI::MenuCtrl* menuCtrl)
{
	return menuCtrl->getMenuItemParent();
}