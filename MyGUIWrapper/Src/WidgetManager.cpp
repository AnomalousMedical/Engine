#include "Stdafx.h"

enum WidgetType
{
    Widget,
        Canvas,
        DDContainer,
            ItemBox,
            ListCtrl,
                ListBox,
        List,
        MenuCtrl,
            MenuBar,
            PopupMenu,
        MultiList,
        Progress,
        ScrollView,
        StaticImage,
        StaticText,
            Button,
                MenuItem,
            Edit,
                ComboBox,
        Tab,
        TabItem,
        VScroll,
            HScroll,
        Window,
            Message,
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

			return Edit;
		}

		return StaticText;
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

		if(widget->isType<MyGUI::ListCtrl>())
		{
			if(widget->isType<MyGUI::ListBox>())
			{
				return ListBox;
			}

			return ListCtrl;
		}

		return DDContainer;
	}

	if(widget->isType<MyGUI::ListBox>())
	{
		return List;
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

		return MenuCtrl;
	}

	if(widget->isType<MyGUI::MultiListBox>())
	{
		return MultiList;
	}

	if(widget->isType<MyGUI::ProgressBar>())
	{
		return Progress;
	}

	if(widget->isType<MyGUI::ScrollView>())
	{
		return ScrollView;
	}

	if(widget->isType<MyGUI::ImageBox>())
	{
		return StaticImage;
	}

	if(widget->isType<MyGUI::TabControl>())
	{
		return Tab;
	}

	if(widget->isType<MyGUI::TabItem>())
	{
		return TabItem;
	}

	if(widget->isType<MyGUI::ScrollBar>())
	{
		if(widget->isType<MyGUI::HScroll>())
		{
			return HScroll;
		}

		return VScroll;
	}

	if(widget->isType<MyGUI::Window>())
	{
		if(widget->isType<MyGUI::Message>())
		{
			return Message;
		}

		return Window;
	}

	return Widget;
}