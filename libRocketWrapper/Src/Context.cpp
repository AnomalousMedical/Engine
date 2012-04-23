#include "StdAfx.h"

String Context_GetName()
{

}

void Context_SetDimensions(const Vector2i& dimensions)
{

}

const Vector2i& Context_GetDimensions()
{

}

bool Context_Update()
{

}

bool Context_Render()
{

}

Rocket::Core::ElementDocument* Context_CreateDocument()
{

}

Rocket::Core::ElementDocument* Context_CreateDocument_Tag(String tag)
{

}

Rocket::Core::ElementDocument* Context_LoadDocument(String document_path)
{

}

//Rocket::Core::ElementDocument* Context_LoadDocument(Stream* document_stream)
//{
//
//}

Rocket::Core::ElementDocument* Context_LoadDocumentFromMemory(const String& string)
{

}

void Context_UnloadDocument(Rocket::Core::ElementDocument* document)
{

}

void Context_UnloadAllDocuments()
{

}

void Context_AddMouseCursor(Rocket::Core::ElementDocument* cursor_document)
{

}

Rocket::Core::ElementDocument* Context_LoadMouseCursor(const String& cursor_document_path)
{

}

void Context_UnloadMouseCursor(const String& cursor_name)
{

}

void Context_UnloadAllMouseCursors()
{

}

bool Context_SetMouseCursor(const String& cursor_name)
{

}

void Context_ShowMouseCursor(bool show)
{

}

Rocket::Core::ElementDocument* Context_GetDocument(const String& id)
{

}

Rocket::Core::ElementDocument* Context_GetDocument(int index)
{

}

int Context_GetNumDocuments()
{

}

Rocket::Core::Element* Context_GetHoverElement()
{

}

Rocket::Core::Element* Context_GetFocusElement()
{

}

Rocket::Core::Element* Context_GetRootElement()
{

}

void Context_PullDocumentToFront(Rocket::Core::ElementDocument* document)
{

}

void Context_PushDocumentToBack(Rocket::Core::ElementDocument* document)
{

}

void Context_AddEventListener(const String& event, Rocket::Core::EventListener* listener, bool in_capture_phase = false)
{

}

void Context_RemoveEventListener(const String& event, Rocket::Core::EventListener* listener, bool in_capture_phase = false)
{

}

bool Context_ProcessKeyDown(Rocket::Core::Input::KeyIdentifier key_identifier, int key_modifier_state)
{

}

bool Context_ProcessKeyUp(Rocket::Core::Input::KeyIdentifier key_identifier, int key_modifier_state)
{

}

bool Context_ProcessTextInput(Rocket::Core::word character)
{

}

bool Context_ProcessTextInput(const String& string)
{

}

void Context_ProcessMouseMove(int x, int y, int key_modifier_state)
{

}

void Context_ProcessMouseButtonDown(int button_index, int key_modifier_state)
{

}

void Context_ProcessMouseButtonUp(int button_index, int key_modifier_state)
{

}

bool Context_ProcessMouseWheel(int wheel_delta, int key_modifier_state)
{

}

Rocket::Core::RenderInterface* Context_GetRenderInterface()
{

}