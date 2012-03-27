#include "Stdafx.h"

extern "C" _AnomalousExport void Edit_setTextIntervalColor(MyGUI::EditBox* edit, size_t start, size_t count, Color colour)
{
	edit->setTextIntervalColour(start, count, colour.toMyGUI());
}

extern "C" _AnomalousExport void Edit_setTextSelection(MyGUI::EditBox* edit, size_t start, size_t end)
{
	edit->setTextSelection(start, end);
}

extern "C" _AnomalousExport void Edit_deleteTextSelection(MyGUI::EditBox* edit)
{
	edit->deleteTextSelection();
}

extern "C" _AnomalousExport void Edit_setTextSelectionColor(MyGUI::EditBox* edit, Color value)
{
	edit->setTextSelectionColour(value.toMyGUI());
}

extern "C" _AnomalousExport void Edit_insertText1(MyGUI::EditBox* edit, UStringIn text)
{
	edit->insertText(text);
}

extern "C" _AnomalousExport void Edit_insertText2(MyGUI::EditBox* edit, UStringIn text, size_t index)
{
	edit->insertText(text, index);
}

extern "C" _AnomalousExport void Edit_addText(MyGUI::EditBox* edit, UStringIn text)
{
	edit->addText(text);
}

extern "C" _AnomalousExport void Edit_eraseText1(MyGUI::EditBox* edit, size_t start)
{
	edit->eraseText(start);
}

extern "C" _AnomalousExport void Edit_eraseText2(MyGUI::EditBox* edit, size_t start, size_t count)
{
	edit->eraseText(start, count);
}

extern "C" _AnomalousExport size_t Edit_getTextSelectionStart(MyGUI::EditBox* edit)
{
	return edit->getTextSelectionStart();
}

extern "C" _AnomalousExport size_t Edit_getTextSelectionEnd(MyGUI::EditBox* edit)
{
	return edit->getTextSelectionEnd();
}

extern "C" _AnomalousExport size_t Edit_getTextSelectionLength(MyGUI::EditBox* edit)
{
	return edit->getTextSelectionLength();
}

extern "C" _AnomalousExport bool Edit_isTextSelection(MyGUI::EditBox* edit)
{
	return edit->isTextSelection();
}

extern "C" _AnomalousExport void Edit_setTextCursor(MyGUI::EditBox* edit, size_t index)
{
	edit->setTextCursor(index);
}

extern "C" _AnomalousExport size_t Edit_getTextCursor(MyGUI::EditBox* edit)
{
	return edit->getTextCursor();
}

extern "C" _AnomalousExport size_t Edit_getTextLength(MyGUI::EditBox* edit)
{
	return edit->getTextLength();
}

extern "C" _AnomalousExport void Edit_setOverflowToTheLeft(MyGUI::EditBox* edit, bool value)
{
	edit->setOverflowToTheLeft(value);
}

extern "C" _AnomalousExport bool Edit_getOverflowToTheLeft(MyGUI::EditBox* edit)
{
	return edit->getOverflowToTheLeft();
}

extern "C" _AnomalousExport void Edit_setMaxTextLength(MyGUI::EditBox* edit, size_t value)
{
	edit->setMaxTextLength(value);
}

extern "C" _AnomalousExport size_t Edit_getMaxTextLength(MyGUI::EditBox* edit)
{
	return edit->getMaxTextLength();
}

extern "C" _AnomalousExport void Edit_setEditReadOnly(MyGUI::EditBox* edit, bool value)
{
	edit->setEditReadOnly(value);
}

extern "C" _AnomalousExport bool Edit_getEditReadOnly(MyGUI::EditBox* edit)
{
	return edit->getEditReadOnly();
}

extern "C" _AnomalousExport void Edit_setEditPassword(MyGUI::EditBox* edit, bool value)
{
	edit->setEditPassword(value);
}

extern "C" _AnomalousExport bool Edit_getEditPassword(MyGUI::EditBox* edit)
{
	return edit->getEditPassword();
}

extern "C" _AnomalousExport void Edit_setEditMultiLine(MyGUI::EditBox* edit, bool value)
{
	edit->setEditMultiLine(value);
}

extern "C" _AnomalousExport bool Edit_getEditMultiLine(MyGUI::EditBox* edit)
{
	return edit->getEditMultiLine();
}

extern "C" _AnomalousExport void Edit_setEditStatic(MyGUI::EditBox* edit, bool value)
{
	edit->setEditStatic(value);
}

extern "C" _AnomalousExport bool Edit_getEditStatic(MyGUI::EditBox* edit)
{
	return edit->getEditStatic();
}

extern "C" _AnomalousExport void Edit_setPasswordChar(MyGUI::EditBox* edit, char value)
{
	edit->setPasswordChar(value);
}

extern "C" _AnomalousExport char Edit_getPasswordChar(MyGUI::EditBox* edit)
{
	return edit->getPasswordChar();
}

extern "C" _AnomalousExport void Edit_setEditWordWrap(MyGUI::EditBox* edit, bool value)
{
	edit->setEditWordWrap(value);
}

extern "C" _AnomalousExport bool Edit_getEditWordWrap(MyGUI::EditBox* edit)
{
	return edit->getEditWordWrap();
}

extern "C" _AnomalousExport void Edit_setTabPrinting(MyGUI::EditBox* edit, bool value)
{
	edit->setTabPrinting(value);
}

extern "C" _AnomalousExport bool Edit_getTabPrinting(MyGUI::EditBox* edit)
{
	return edit->getTabPrinting();
}

extern "C" _AnomalousExport bool Edit_getInvertSelected(MyGUI::EditBox* edit)
{
	return edit->getInvertSelected();
}

extern "C" _AnomalousExport void Edit_setInvertSelected(MyGUI::EditBox* edit, bool value)
{
	edit->setInvertSelected(value);
}

extern "C" _AnomalousExport void Edit_setVisibleVScroll(MyGUI::EditBox* edit, bool value)
{
	edit->setVisibleVScroll(value);
}

extern "C" _AnomalousExport bool Edit_isVisibleVScroll(MyGUI::EditBox* edit)
{
	return edit->isVisibleVScroll();
}

extern "C" _AnomalousExport size_t Edit_getVScrollRange(MyGUI::EditBox* edit)
{
	return edit->getVScrollRange();
}

extern "C" _AnomalousExport size_t Edit_getVScrollPosition(MyGUI::EditBox* edit)
{
	return edit->getVScrollPosition();
}

extern "C" _AnomalousExport void Edit_setVScrollPosition(MyGUI::EditBox* edit, size_t index)
{
	edit->setVScrollPosition(index);
}

extern "C" _AnomalousExport void Edit_setVisibleHScroll(MyGUI::EditBox* edit, bool value)
{
	edit->setVisibleHScroll(value);
}

extern "C" _AnomalousExport bool Edit_isVisibleHScroll(MyGUI::EditBox* edit)
{
	return edit->isVisibleHScroll();
}

extern "C" _AnomalousExport size_t Edit_getHScrollRange(MyGUI::EditBox* edit)
{
	return edit->getHScrollRange();
}

extern "C" _AnomalousExport size_t Edit_getHScrollPosition(MyGUI::EditBox* edit)
{
	return edit->getHScrollPosition();
}

extern "C" _AnomalousExport void Edit_setHScrollPosition(MyGUI::EditBox* edit, size_t index)
{
	edit->setHScrollPosition(index);
}

extern "C" _AnomalousExport void Edit_setAllowMouseScroll(MyGUI::EditBox* edit, bool value)
{
	edit->setAllowMouseScroll(value);
}

extern "C" _AnomalousExport bool Edit_getAllowMouseScroll(MyGUI::EditBox* edit)
{
	return edit->getAllowMouseScroll();
}

extern "C" _AnomalousExport void Edit_setOnlyText(MyGUI::EditBox* edit, UStringIn value)
{
	edit->setOnlyText(value);
}

extern "C" _AnomalousExport void Edit_getOnlyText(MyGUI::EditBox* edit, TempStringCallback onlyTextDelegate)
{
	onlyTextDelegate(edit->getOnlyText().c_str());
}