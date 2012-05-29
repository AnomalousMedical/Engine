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
//float GetAbsoluteLeft();
//
//float GetAbsoluteTop();
//
//float GetClientLeft();
//
//float GetClientTop();
//
//float GetClientWidth();
//
//float GetClientHeight();
//
//Element* GetOffsetParent();
//
//float GetOffsetLeft();
//
//float GetOffsetTop();
//
//float GetOffsetWidth();
//
//float GetOffsetHeight();
//
//float GetScrollLeft();
//
//void SetScrollLeft(float scroll_left);
//
//float GetScrollTop();
//
//void SetScrollTop(float scroll_top);
//
//float GetScrollWidth();
//
//float GetScrollHeight();
//
//ElementStyle* GetStyle();
//
//virtual ElementDocument* GetOwnerDocument();
//
//Element* GetParentNode() const;
//
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