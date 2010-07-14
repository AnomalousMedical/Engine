#include "stdafx.h"

extern "C" _AnomalousExport size_t MultiList_getColumnCount(MyGUI::MultiList* multiList)
{
	return multiList->getColumnCount();
}

extern "C" _AnomalousExport void MultiList_insertColumnAt(MyGUI::MultiList* multiList, size_t column, UStringIn name, int width)
{
	multiList->insertColumnAt(column, name, width);
}

extern "C" _AnomalousExport void MultiList_addColumn(MyGUI::MultiList* multiList, UStringIn name, int width)
{
	multiList->addColumn(name, width);
}

extern "C" _AnomalousExport void MultiList_removeColumnAt(MyGUI::MultiList* multiList, size_t column)
{
	multiList->removeColumnAt(column);
}

extern "C" _AnomalousExport void MultiList_removeAllColumns(MyGUI::MultiList* multiList)
{
	multiList->removeAllColumns();
}

extern "C" _AnomalousExport void MultiList_setColumnNameAt(MyGUI::MultiList* multiList, size_t column, UStringIn name)
{
	multiList->setColumnNameAt(column, name);
}

extern "C" _AnomalousExport void MultiList_setColumnWidthAt(MyGUI::MultiList* multiList, size_t column, int width)
{
	multiList->setColumnWidthAt(column, width);
}

extern "C" _AnomalousExport UStringOut MultiList_getColumnNameAt(MyGUI::MultiList* multiList, size_t column)
{
	return multiList->getColumnNameAt(column).c_str();
}

extern "C" _AnomalousExport int MultiList_getColumnWidthAt(MyGUI::MultiList* multiList, size_t column)
{
	return multiList->getColumnWidthAt(column);
}

extern "C" _AnomalousExport void MultiList_sortByColumn(MyGUI::MultiList* multiList, size_t column)
{
	multiList->sortByColumn(column);
}

extern "C" _AnomalousExport void MultiList_sortByColumn2(MyGUI::MultiList* multiList, size_t column, bool backward)
{
	multiList->sortByColumn(column, backward);
}

extern "C" _AnomalousExport size_t MultiList_getItemCount(MyGUI::MultiList* multiList)
{
	return multiList->getItemCount();
}

extern "C" _AnomalousExport void MultiList_insertItemAt(MyGUI::MultiList* multiList, size_t index, UStringIn name)
{
	multiList->insertItemAt(index, name);
}

extern "C" _AnomalousExport void MultiList_addItem(MyGUI::MultiList* multiList, UStringIn name)
{
	multiList->addItem(name);
}

extern "C" _AnomalousExport void MultiList_removeItemAt(MyGUI::MultiList* multiList, size_t index)
{
	multiList->removeItemAt(index);
}

extern "C" _AnomalousExport void MultiList_removeAllItems(MyGUI::MultiList* multiList)
{
	multiList->removeAllItems();
}

extern "C" _AnomalousExport void MultiList_swapItemsAt(MyGUI::MultiList* multiList, size_t index1, size_t index2)
{
	multiList->swapItemsAt(index1, index2);
}

extern "C" _AnomalousExport void MultiList_setItemNameAt(MyGUI::MultiList* multiList, size_t index, UStringIn name)
{
	multiList->setItemNameAt(index, name);
}

extern "C" _AnomalousExport UStringOut MultiList_getItemNameAt(MyGUI::MultiList* multiList, size_t index)
{
	return multiList->getItemNameAt(index).c_str();
}

extern "C" _AnomalousExport size_t MultiList_getIndexSelected(MyGUI::MultiList* multiList)
{
	return multiList->getIndexSelected();
}

extern "C" _AnomalousExport void MultiList_setIndexSelected(MyGUI::MultiList* multiList, size_t index)
{
	multiList->setIndexSelected(index);
}

extern "C" _AnomalousExport void MultiList_clearIndexSelected(MyGUI::MultiList* multiList)
{
	multiList->clearIndexSelected();
}

extern "C" _AnomalousExport void MultiList_setSubItemNameAt(MyGUI::MultiList* multiList, size_t column, size_t index, UStringIn name)
{
	multiList->setSubItemNameAt(column, index, name);
}

extern "C" _AnomalousExport UStringOut MultiList_getSubItemNameAt(MyGUI::MultiList* multiList, size_t column, size_t index)
{
	return multiList->getSubItemNameAt(column, index).c_str();
}

extern "C" _AnomalousExport size_t MultiList_findSubItemWith(MyGUI::MultiList* multiList, size_t column, UStringIn name)
{
	return multiList->findSubItemWith(column, name);
}