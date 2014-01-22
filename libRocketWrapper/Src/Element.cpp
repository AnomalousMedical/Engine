#include "StdAfx.h"
#include "ElementListIter.h"
#include <queue>

//Element* Clone() const;
//
extern "C" _AnomalousExport void Element_SetClass(Rocket::Core::Element* element, String class_name, bool activate)
{
	element->SetClass(class_name, activate);
}

extern "C" _AnomalousExport bool Element_IsClassSet(Rocket::Core::Element* element, String class_name)
{
	return element->IsClassSet(class_name);
}

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
extern "C" _AnomalousExport bool Element_SetProperty(Rocket::Core::Element* element, String name, String value)
{
	return element->SetProperty(name, value);
}

//bool SetProperty(const String& name, const Property& property);
//
extern "C" _AnomalousExport void Element_RemoveProperty(Rocket::Core::Element* element, String name)
{
	element->RemoveProperty(name);
}

extern "C" _AnomalousExport void Element_GetPropertyString(Rocket::Core::Element* element, String name, StringRetrieverCallback strRetriever)
{
	const Rocket::Core::Property* prop = element->GetProperty(name);
	if(prop != NULL)
	{
		Rocket::Core::String propString = prop->ToString();
		strRetriever(propString.CString());
	}
	else
	{
		strRetriever(NULL);
	}
}

extern "C" _AnomalousExport const Rocket::Core::Variant* Element_GetPropertyVariant(Rocket::Core::Element* element, String name)
{
	const Rocket::Core::Property* prop = element->GetProperty(name);
	if(prop != NULL)
	{
		return &prop->value;
	}
	else
	{
		return NULL;
	}
}

extern "C" _AnomalousExport void Element_GetLocalPropertyString(Rocket::Core::Element* element, String name, StringRetrieverCallback strRetriever)
{
	const Rocket::Core::Property* prop = element->GetLocalProperty(name);
	if(prop != NULL)
	{
		Rocket::Core::String propString = prop->ToString();
		strRetriever(propString.CString());
	}
	else
	{
		strRetriever(NULL);
	}
}

extern "C" _AnomalousExport const Rocket::Core::Variant* Element_GetLocalPropertyVariant(Rocket::Core::Element* element, String name)
{
	const Rocket::Core::Property* prop = element->GetLocalProperty(name);
	if(prop != NULL)
	{
		return &prop->value;
	}
	else
	{
		return NULL;
	}
}

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

extern "C" _AnomalousExport void Element_SetAttribute(Rocket::Core::Element* element, String name, String value)
{
	element->SetAttribute(name, value);
}

extern "C" _AnomalousExport Rocket::Core::Variant* Element_GetAttribute(Rocket::Core::Element* element, String name)
{
	return element->GetAttribute(name);
}

//template< typename T >
//T GetAttribute(const String& name, const T& default_value) const;

extern "C" _AnomalousExport bool Element_HasAttribute(Rocket::Core::Element* element, String name)
{
	return element->HasAttribute(name);
}

extern "C" _AnomalousExport void Element_RemoveAttribute(Rocket::Core::Element* element, String name)
{
	element->RemoveAttribute(name);
}

//void SetAttributes(const ElementAttributes* attributes);

extern "C" _AnomalousExport bool Element_IterateAttributes(Rocket::Core::Element* element, int& index, StringRetrieverCallback keyRetrieve, StringRetrieverCallback valueRetrieve)
{
	Rocket::Core::String rktKey;
	Rocket::Core::String rktValue;
	bool retVal = element->IterateAttributes(index, rktKey, rktValue);
	keyRetrieve(rktKey.CString());
	valueRetrieve(rktValue.CString());
	return retVal;
}

extern "C" _AnomalousExport int Element_GetNumAttributes(Rocket::Core::Element* element)
{
	return element->GetNumAttributes();
}
//
//bool IterateDecorators(int& index, PseudoClassList& pseudo_classes, String& name, Decorator*& decorator, DecoratorDataHandle& decorator_data);
//
//Element* GetFocusLeafNode();
//
extern "C" _AnomalousExport Rocket::Core::Context* Element_GetContext(Rocket::Core::Element* element)
{
	return element->GetContext();
}

extern "C" _AnomalousExport String Element_GetTagName(Rocket::Core::Element* element)
{
	return element->GetTagName().CString();
}

extern "C" _AnomalousExport String Element_GetId(Rocket::Core::Element* element)
{
	return element->GetId().CString();
}

extern "C" _AnomalousExport void Element_SetId(Rocket::Core::Element* element, String id)
{
	element->SetId(id);
}

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

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetOffsetParent(Rocket::Core::Element* element)
{
	return element->GetOffsetParent();
}

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
extern "C" _AnomalousExport Rocket::Core::ElementDocument* Element_GetOwnerDocument(Rocket::Core::Element* element)
{
	return element->GetOwnerDocument();
}

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetParentNode(Rocket::Core::Element* element)
{
	return element->GetParentNode();
}

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetNextSibling(Rocket::Core::Element* element)
{
	return element->GetNextSibling();
}

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetPreviousSibling(Rocket::Core::Element* element)
{
	return element->GetPreviousSibling();
}

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetFirstChild(Rocket::Core::Element* element)
{
	return element->GetFirstChild();
}

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetLastChild(Rocket::Core::Element* element)
{
	return element->GetLastChild();
}

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetChild(Rocket::Core::Element* element, int index)
{
	return element->GetChild(index);
}

extern "C" _AnomalousExport int Element_GetNumChildren(Rocket::Core::Element* element, bool include_non_dom_elements/* = false*/)
{
	return element->GetNumChildren(include_non_dom_elements);
}

extern "C" _AnomalousExport void Element_GetInnerRML(Rocket::Core::Element* element, StringRetrieverCallback retrieve)
{
	Rocket::Core::String str;
	element->GetInnerRML(str);
	retrieve(str.CString());
}

extern "C" _AnomalousExport void Element_SetInnerRML(Rocket::Core::Element* element, String rml)
{
	element->SetInnerRML(rml);
}


extern "C" _AnomalousExport bool Element_Focus(Rocket::Core::Element* element)
{
	return element->Focus();
}

extern "C" _AnomalousExport void Element_Blur(Rocket::Core::Element* element)
{
	element->Blur();
}

extern "C" _AnomalousExport void Element_Click(Rocket::Core::Element* element)
{
	element->Click();
}

//void AddEventListener(const String& event, EventListener* listener, bool in_capture_phase = false);
//
//void RemoveEventListener(const String& event, EventListener* listener, bool in_capture_phase = false);
//
//bool DispatchEvent(const String& event, const Dictionary& parameters, bool interruptible = false);
//
extern "C" _AnomalousExport void Element_ScrollIntoView(Rocket::Core::Element* element, bool align_with_top/* = true*/)
{
	return element->ScrollIntoView();
}

extern "C" _AnomalousExport void Element_AppendChild(Rocket::Core::Element* element, Rocket::Core::Element* append, bool dom_element/* = true*/)
{
	return element->AppendChild(append, dom_element);
}

extern "C" _AnomalousExport void Element_InsertBefore(Rocket::Core::Element* element, Rocket::Core::Element* insert, Rocket::Core::Element* adjacent_element)
{
	return element->InsertBefore(insert, adjacent_element);
}

extern "C" _AnomalousExport bool Element_ReplaceChild(Rocket::Core::Element* element, Rocket::Core::Element* inserted_element, Rocket::Core::Element* replaced_element)
{
	return element->ReplaceChild(inserted_element, replaced_element);
}

extern "C" _AnomalousExport bool Element_RemoveChild(Rocket::Core::Element* element, Rocket::Core::Element* remove)
{
	return element->RemoveChild(remove);
}

extern "C" _AnomalousExport bool Element_HasChildNodes(Rocket::Core::Element* element)
{
	return element->HasChildNodes();
}

extern "C" _AnomalousExport Rocket::Core::Element* Element_GetElementById(Rocket::Core::Element* element, String id)
{
	return element->GetElementById(id);
}

extern "C" _AnomalousExport void Element_GetElementsByTagName(Rocket::Core::Element* element, ElementListIter* elements, String tag)
{
	element->GetElementsByTagName(elements->elementList, tag);
}

extern "C" _AnomalousExport void Element_GetElementRML(Rocket::Core::Element* element, StringRetrieverCallback retrieve)
{
	Rocket::Core::String str;
	element->GetElementRML(str);
	retrieve(str.CString());
}

extern "C" _AnomalousExport void Element_GetElementsWithAttribute(Rocket::Core::Element* root_element, ElementListIter* elementListIter, String attribute)
{
	Rocket::Core::ElementList& elements = elementListIter->elementList;
	// Breadth first search on elements for the corresponding id
	typedef std::queue< Rocket::Core::Element* > SearchQueue;
	SearchQueue search_queue;
	for (int i = 0; i < root_element->GetNumChildren(); ++i)
		search_queue.push(root_element->GetChild(i));

	while (!search_queue.empty())
	{
		Rocket::Core::Element* element = search_queue.front();
		search_queue.pop();

		if (element->HasAttribute(attribute))
			elements.push_back(element);

		// Add all children to search.
		for (int i = 0; i < element->GetNumChildren(); i++)
			search_queue.push(element->GetChild(i));
	}
}