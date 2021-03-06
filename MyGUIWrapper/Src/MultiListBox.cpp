#include "stdafx.h"

extern "C" _AnomalousExport size_t MultiListBox_getColumnCount(MyGUI::MultiListBox* multiList)
{
	return multiList->getColumnCount();
}

extern "C" _AnomalousExport void MultiListBox_insertColumnAt(MyGUI::MultiListBox* multiList, size_t column, UStringIn name, int width)
{
	multiList->insertColumnAt(column, name, width);
}

extern "C" _AnomalousExport void MultiListBox_addColumn(MyGUI::MultiListBox* multiList, UStringIn name, int width)
{
	multiList->addColumn(name, width);
}

extern "C" _AnomalousExport void MultiListBox_removeColumnAt(MyGUI::MultiListBox* multiList, size_t column)
{
	multiList->removeColumnAt(column);
}

extern "C" _AnomalousExport void MultiListBox_removeAllColumns(MyGUI::MultiListBox* multiList)
{
	multiList->removeAllColumns();
}

extern "C" _AnomalousExport void MultiListBox_setColumnNameAt(MyGUI::MultiListBox* multiList, size_t column, UStringIn name)
{
	multiList->setColumnNameAt(column, name);
}

extern "C" _AnomalousExport void MultiListBox_setColumnWidthAt(MyGUI::MultiListBox* multiList, size_t column, int width)
{
	multiList->setColumnWidthAt(column, width);
}

extern "C" _AnomalousExport UStringOut MultiListBox_getColumnNameAt(MyGUI::MultiListBox* multiList, size_t column)
{
	return multiList->getColumnNameAt(column).c_str();
}

extern "C" _AnomalousExport int MultiListBox_getColumnWidthAt(MyGUI::MultiListBox* multiList, size_t column)
{
	return multiList->getColumnWidthAt(column);
}

extern "C" _AnomalousExport void MultiListBox_sortByColumn(MyGUI::MultiListBox* multiList, size_t column)
{
	multiList->sortByColumn(column);
}

extern "C" _AnomalousExport void MultiListBox_sortByColumn2(MyGUI::MultiListBox* multiList, size_t column, bool backward)
{
	multiList->sortByColumn(column, backward);
}

extern "C" _AnomalousExport size_t MultiListBox_getItemCount(MyGUI::MultiListBox* multiList)
{
	return multiList->getItemCount();
}

extern "C" _AnomalousExport void MultiListBox_insertItemAt(MyGUI::MultiListBox* multiList, size_t index, UStringIn name)
{
	multiList->insertItemAt(index, name);
}

extern "C" _AnomalousExport void MultiListBox_addItem(MyGUI::MultiListBox* multiList, UStringIn name)
{
	multiList->addItem(name);
}

extern "C" _AnomalousExport void MultiListBox_removeItemAt(MyGUI::MultiListBox* multiList, size_t index)
{
	multiList->removeItemAt(index);
}

extern "C" _AnomalousExport void MultiListBox_removeAllItems(MyGUI::MultiListBox* multiList)
{
	multiList->removeAllItems();
}

extern "C" _AnomalousExport void MultiListBox_swapItemsAt(MyGUI::MultiListBox* multiList, size_t index1, size_t index2)
{
	multiList->swapItemsAt(index1, index2);
}

extern "C" _AnomalousExport void MultiListBox_setItemNameAt(MyGUI::MultiListBox* multiList, size_t index, UStringIn name)
{
	multiList->setItemNameAt(index, name);
}

extern "C" _AnomalousExport UStringOut MultiListBox_getItemNameAt(MyGUI::MultiListBox* multiList, size_t index)
{
	return multiList->getItemNameAt(index).c_str();
}

extern "C" _AnomalousExport size_t MultiListBox_getIndexSelected(MyGUI::MultiListBox* multiList)
{
	return multiList->getIndexSelected();
}

extern "C" _AnomalousExport void MultiListBox_setIndexSelected(MyGUI::MultiListBox* multiList, size_t index)
{
	multiList->setIndexSelected(index);
}

extern "C" _AnomalousExport void MultiListBox_clearIndexSelected(MyGUI::MultiListBox* multiList)
{
	multiList->clearIndexSelected();
}

extern "C" _AnomalousExport void MultiListBox_setSubItemNameAt(MyGUI::MultiListBox* multiList, size_t column, size_t index, UStringIn name)
{
	multiList->setSubItemNameAt(column, index, name);
}

extern "C" _AnomalousExport UStringOut MultiListBox_getSubItemNameAt(MyGUI::MultiListBox* multiList, size_t column, size_t index)
{
	return multiList->getSubItemNameAt(column, index).c_str();
}

extern "C" _AnomalousExport size_t MultiListBox_findSubItemWith(MyGUI::MultiListBox* multiList, size_t column, UStringIn name)
{
	return multiList->findSubItemWith(column, name);
}

extern "C" _AnomalousExport void MultiListBox_setSortOnChanges(MyGUI::MultiListBox* multiList, bool value)
{
	multiList->setSortOnChanges(value);
}

extern "C" _AnomalousExport bool MultiListBox_getSortOnChanges(MyGUI::MultiListBox* multiList)
{
	return multiList->getSortOnChanges();
}

extern "C" _AnomalousExport void MultiListBox_setColumnResizingPolicyAt(MyGUI::MultiListBox* multiList, size_t _index, MyGUI::ResizingPolicy::Enum _value)
{
	multiList->setColumnResizingPolicyAt(_index, _value);
}