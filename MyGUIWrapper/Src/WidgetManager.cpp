#include "Stdafx.h"

enum WidgetType
{
    Widget = 0,
        Canvas = 1,
        DDContainer = 2,
            ItemBox = 3,
        ListBox = 4,
        MenuControl = 5,
            MenuBar = 6,
            PopupMenu = 7,
        MultiListBox = 8,
        ProgressBar = 9,
        ScrollView = 10,
        ImageBox = 11,
        TextBox = 12,
            Button = 13,
                MenuItem = 14,
            EditBox = 15,
                ComboBox = 16,
			Window = 17,
			TabItem = 18,
        TabControl = 19,
        ScrollBar = 20,
};

extern "C" _AnomalousExport WidgetType WidgetManager_getType(MyGUI::Widget* widget)
{
	//Check for buttons first since they are likely to be the most common control
	if(widget->isType<MyGUI::TextBox>())
	{
		if(widget->isType<MyGUI::Button>())
		{
			if(widget->isType<MyGUI::MenuItem>())
			{
				return MenuItem;
			}

			return Button;
		}

		if(widget->isType<MyGUI::EditBox>())
		{
			if(widget->isType<MyGUI::ComboBox>())
			{
				return ComboBox;
			}

			return EditBox;
		}

		if(widget->isType<MyGUI::Window>())
		{
			return Window;
		}

		if(widget->isType<MyGUI::TabItem>())
		{
			return TabItem;
		}

		return TextBox;
	}

	if(widget->isType<MyGUI::Canvas>())
	{
		return Canvas;
	}

	if(widget->isType<MyGUI::DDContainer>())
	{
		if(widget->isType<MyGUI::ItemBox>())
		{
			return ItemBox;
		}

		return DDContainer;
	}

	if(widget->isType<MyGUI::ListBox>())
	{
		return ListBox;
	}

	if(widget->isType<MyGUI::MenuControl>())
	{
		if(widget->isType<MyGUI::MenuBar>())
		{
			return MenuBar;
		}

		if(widget->isType<MyGUI::PopupMenu>())
		{
			return PopupMenu;
		}

		return MenuControl;
	}

	if(widget->isType<MyGUI::MultiListBox>())
	{
		return MultiListBox;
	}

	if(widget->isType<MyGUI::ProgressBar>())
	{
		return ProgressBar;
	}

	if(widget->isType<MyGUI::ScrollView>())
	{
		return ScrollView;
	}

	if(widget->isType<MyGUI::ImageBox>())
	{
		return ImageBox;
	}

	if(widget->isType<MyGUI::TabControl>())
	{
		return TabControl;
	}

	if(widget->isType<MyGUI::ScrollBar>())
	{
		return ScrollBar;
	}

	return Widget;
}