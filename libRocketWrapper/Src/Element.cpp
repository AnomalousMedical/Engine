#include "StdAfx.h"

//Element* Clone() const;
//
//void SetClass(const String& class_name, bool activate);
//
//bool IsClassSet(const String& class_name) const;
//
//void SetClassNames(const String& class_names);
//
//String GetClassNames() const;
//
//virtual StyleSheet* GetStyleSheet() const;
//
//const ElementDefinition* GetDefinition();
//
//String GetAddress(bool include_pseudo_classes = false) const;
//
//void SetOffset(const Vector2f& offset, Element* offset_parent, bool offset_fixed = false);
//
//Vector2f GetRelativeOffset(Box::Area area = Box::CONTENT);
//
//Vector2f GetAbsoluteOffset(Box::Area area = Box::CONTENT);
//
//void SetClientArea(Box::Area client_area);
//
//Box::Area GetClientArea() const;
//
//void SetContentBox(const Vector2f& content_offset, const Vector2f& content_box);
//
//void SetBox(const Box& box);
//
//void AddBox(const Box& box);
//
//const Box& GetBox(int index = 0);
//
//int GetNumBoxes();
//
//virtual float GetBaseline() const;
//
//virtual bool GetIntrinsicDimensions(Vector2f& dimensions);
//
//virtual bool IsPointWithinElement(const Vector2f& point);
//
//bool IsVisible() const;
//
//float GetZIndex() const;
//
//FontFaceHandle* GetFontFaceHandle() const;
//
//bool SetProperty(const String& name, const String& value);
//
//bool SetProperty(const String& name, const Property& property);
//
//void RemoveProperty(const String& name);
//
//const Property* GetProperty(const String& name);		
//
//template < typename T >
//T GetProperty(const String& name);
//
//const Property* GetLocalProperty(const String& name);		
//
//float ResolveProperty(const String& name, float base_value);
//
//bool IterateProperties(int& index, PseudoClassList& pseudo_classes, String& name, const Property*& property) const;
//
//void SetPseudoClass(const String& pseudo_class, bool activate);
//
//bool IsPseudoClassSet(const String& pseudo_class) const;
//
//bool ArePseudoClassesSet(const PseudoClassList& pseudo_classes) const;
//
//const PseudoClassList& GetActivePseudoClasses() const;
//
//template< typename T >
//void SetAttribute(const String& name, const T& value);
//
//Variant* GetAttribute(const String& name) const;
//
//template< typename T >
//T GetAttribute(const String& name, const T& default_value) const;
//
//bool HasAttribute(const String& name);
//
//void RemoveAttribute(const String& name);
//
//void SetAttributes(const ElementAttributes* attributes);
//
//template< typename T >
//bool IterateAttributes(int& index, String& name, T& value) const;
//
//int GetNumAttributes() const;
//
//bool IterateDecorators(int& index, PseudoClassList& pseudo_classes, String& name, Decorator*& decorator, DecoratorDataHandle& decorator_data);
//
//Element* GetFocusLeafNode();
//
//Context* GetContext();
//
//const String& GetTagName() const;
//
//const String& GetId() const;
//
//void SetId(const String& id);
//
extern "C" _AnomalousExport float Element_GetAbsoluteLeft(Rocket::Core::Element* element)
{
	return element->GetAbsoluteLeft();
}

extern "C" _AnomalousExport float Element_GetAbsoluteTop(Rocket::Core::Element* element)
{
	return element->GetAbsoluteTop();
}

extern "C" _AnomalousExport float Element_GetClientLeft(Rocket::Core::Element* element)
{
	return element->GetClientLeft();
}

extern "C" _AnomalousExport float Element_GetClientTop(Rocket::Core::Element* element)
{
	return element->GetClientTop();
}

extern "C" _AnomalousExport float Element_GetClientWidth(Rocket::Core::Element* element)
{
	return element->GetClientWidth();
}

extern "C" _AnomalousExport float Element_GetClientHeight(Rocket::Core::Element* element)
{
	return element->GetClientHeight();
}
//
//Element* GetOffsetParent();
//
extern "C" _AnomalousExport float Element_GetOffsetLeft(Rocket::Core::Element* element)
{
	return element->GetOffsetLeft();
}

extern "C" _AnomalousExport float Element_GetOffsetTop(Rocket::Core::Element* element)
{
	return element->GetOffsetTop();
}

extern "C" _AnomalousExport float Element_GetOffsetWidth(Rocket::Core::Element* element)
{
	return element->GetOffsetWidth();
}

extern "C" _AnomalousExport float Element_GetOffsetHeight(Rocket::Core::Element* element)
{
	return element->GetOffsetHeight();
}

extern "C" _AnomalousExport float Element_GetScrollLeft(Rocket::Core::Element* element)
{
	return element->GetScrollLeft();
}

extern "C" _AnomalousExport void Element_SetScrollLeft(Rocket::Core::Element* element, float scroll_left)
{
	element->SetScrollLeft(scroll_left);
}

extern "C" _AnomalousExport float Element_GetScrollTop(Rocket::Core::Element* element)
{
	return element->GetScrollTop();
}

extern "C" _AnomalousExport void Element_SetScrollTop(Rocket::Core::Element* element, float scroll_top)
{
	element->SetScrollTop(scroll_top);
}

extern "C" _AnomalousExport float Element_GetScrollWidth(Rocket::Core::Element* element)
{
	return element->GetScrollWidth();
}

extern "C" _AnomalousExport float Element_GetScrollHeight(Rocket::Core::Element* element)
{
	return element->GetScrollHeight();
}
//
//ElementStyle* GetStyle();
//
//virtual ElementDocument* GetOwnerDocument();
//
extern "C" _AnomalousExport Rocket::Core::Element* Element_GetParentNode(Rocket::Core::Element* element)
{
	return element->GetParentNode();
}

//Element* GetNextSibling() const;
//
//Element* GetPreviousSibling() const;
//
//Element* GetFirstChild() const;
//
//Element* GetLastChild() const;
//
//Element* GetChild(int index) const;
//
//int GetNumChildren(bool include_non_dom_elements = false) const;
//
//virtual void GetInnerRML(String& content) const;
//
//void SetInnerRML(const String& rml);
//

extern "C" _AnomalousExport bool Element_Focus(Rocket::Core::Element* element)
{
	return element->Focus();
}

extern "C" _AnomalousExport void Element_Blur(Rocket::Core::Element* element)
{
	element->Blur();
}

//void Click();
//
//void AddEventListener(const String& event, EventListener* listener, bool in_capture_phase = false);
//
//void RemoveEventListener(const String& event, EventListener* listener, bool in_capture_phase = false);
//
//bool DispatchEvent(const String& event, const Dictionary& parameters, bool interruptible = false);
//
//void ScrollIntoView(bool align_with_top = true);
//
//void AppendChild(Element* element, bool dom_element = true);
//
//void InsertBefore(Element* element, Element* adjacent_element);
//
//bool ReplaceChild(Element* inserted_element, Element* replaced_element);
//
//bool RemoveChild(Element* element);
//
//bool HasChildNodes() const;
//
//Element* GetElementById(const String& id);
//
//void GetElementsByTagName(ElementList& elements, const String& tag);