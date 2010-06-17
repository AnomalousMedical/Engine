#include "Stdafx.h"

enum WidgetType
{
	Widget,
	Button,
	Canvas,
	ComboBox,
	DDContainer,
	Edit,
	HScroll,
	ItemBox,
	List,
	MenuCtrl,
	Message,
	MultiList,
	PopupMenu,
	Progress,
	RenderBox,
	ScrollView,
	StaticImage,
	StaticText,
	Tab,
	VScroll,
	Window,
};

extern "C" _AnomalousExport WidgetType WidgetManager_getType(MyGUI::Widget* widget)
{
	if(widget->isType<MyGUI::Button>())
	{
		return Button;
	}

	if(widget->isType<MyGUI::Canvas>())
	{
		return Canvas;
	}

	if(widget->isType<MyGUI::ComboBox>())
	{
		return ComboBox;
	}

	if(widget->isType<MyGUI::DDContainer>())
	{
		return DDContainer;
	}

	if(widget->isType<MyGUI::Edit>())
	{
		return Edit;
	}

	if(widget->isType<MyGUI::HScroll>())
	{
		return HScroll;
	}

	if(widget->isType<MyGUI::ItemBox>())
	{
		return ItemBox;
	}

	if(widget->isType<MyGUI::List>())
	{
		return List;
	}

	if(widget->isType<MyGUI::MenuCtrl>())
	{
		return MenuCtrl;
	}

	if(widget->isType<MyGUI::Message>())
	{
		return Message;
	}

	if(widget->isType<MyGUI::MultiList>())
	{
		return MultiList;
	}

	if(widget->isType<MyGUI::PopupMenu>())
	{
		return PopupMenu;
	}

	if(widget->isType<MyGUI::Progress>())
	{
		return Progress;
	}

	if(widget->isType<MyGUI::RenderBox>())
	{
		return RenderBox;
	}

	if(widget->isType<MyGUI::ScrollView>())
	{
		return ScrollView;
	}

	if(widget->isType<MyGUI::StaticImage>())
	{
		return StaticImage;
	}

	if(widget->isType<MyGUI::StaticText>())
	{
		return StaticText;
	}

	if(widget->isType<MyGUI::Tab>())
	{
		return Tab;
	}

	if(widget->isType<MyGUI::VScroll>())
	{
		return VScroll;
	}

	if(widget->isType<MyGUI::Window>())
	{
		return Window;
	}
	
	return Widget;
}