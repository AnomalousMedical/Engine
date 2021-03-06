// stdafx.cpp : source file that includes just the standard includes
// MyGUIWrapper.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

MyGUI::MenuItemType::Enum getMenuItemTypeEnumVal(const MyGUI::MenuItemType& type)
{
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

MyGUI::Align::Enum getAlignEnumVal(const MyGUI::Align& align)
{
	if(align == MyGUI::Align::HCenter)
	{
		return MyGUI::Align::HCenter;	
	}

	if(align == MyGUI::Align::VCenter)
	{
		return MyGUI::Align::VCenter;	
	}

	if(align == MyGUI::Align::Center)
	{
		return MyGUI::Align::Center;	
	}

	if(align == MyGUI::Align::Left)
	{
		return MyGUI::Align::Left;	
	}

	if(align == MyGUI::Align::Right)
	{
		return MyGUI::Align::Right;	
	}

	if(align == MyGUI::Align::HStretch)
	{
		return MyGUI::Align::HStretch;	
	}

	if(align == MyGUI::Align::Top)
	{
		return MyGUI::Align::Top;	
	}

	if(align == MyGUI::Align::Bottom)
	{
		return MyGUI::Align::Bottom;	
	}

	if(align == MyGUI::Align::VStretch)
	{
		return MyGUI::Align::VStretch;	
	}

	if(align == MyGUI::Align::Stretch)
	{
		return MyGUI::Align::Stretch;	
	}

	if(align == MyGUI::Align::Default)
	{
		return MyGUI::Align::Default;	
	}

	if(align == MyGUI::Align::HRelative)
	{
		return MyGUI::Align::HRelative;	
	}

	if(align == MyGUI::Align::VRelative)
	{
		return MyGUI::Align::VRelative;	
	}

	if(align == MyGUI::Align::Relative)
	{
		return MyGUI::Align::Relative;	
	}

	return MyGUI::Align::Default;
}

MyGUI::FlowDirection::Enum getFlowDirectionEnumValue(const MyGUI::FlowDirection& flowDirection)
{
	if(flowDirection == MyGUI::FlowDirection::LeftToRight)
	{
		return MyGUI::FlowDirection::LeftToRight;
	}

	if(flowDirection == MyGUI::FlowDirection::RightToLeft)
	{
		return MyGUI::FlowDirection::RightToLeft;
	}

	if(flowDirection == MyGUI::FlowDirection::TopToBottom)
	{
		return MyGUI::FlowDirection::TopToBottom;
	}

	if(flowDirection == MyGUI::FlowDirection::BottomToTop)
	{
		return MyGUI::FlowDirection::BottomToTop;
	}

	return MyGUI::FlowDirection::MAX;
}

MyGUI::ResizingPolicy::Enum getResizingPolicyEnumValue(const MyGUI::ResizingPolicy& resizingPolicy)
{
	if(resizingPolicy == MyGUI::ResizingPolicy::Auto)
	{
		return MyGUI::ResizingPolicy::Auto;
	}

	if(resizingPolicy == MyGUI::ResizingPolicy::Fixed)
	{
		return MyGUI::ResizingPolicy::Fixed;
	}

	if(resizingPolicy == MyGUI::ResizingPolicy::Fill)
	{
		return MyGUI::ResizingPolicy::Fill;
	}
	
	return MyGUI::ResizingPolicy::MAX;
}