#include "StdAfx.h"

//extern "C" _AnomalousExport String Context_GetName(Rocket::Core::Context* context)
//{
//	return context->GetName().CString();
//}

extern "C" _AnomalousExport void Context_SetDimensions(Rocket::Core::Context* context, Vector2i dimensions)
{
	context->SetDimensions(dimensions.toVector2i());
}

extern "C" _AnomalousExport ThreeIntHack Context_GetDimensions(Rocket::Core::Context* context)
{
	return context->GetDimensions();
}

extern "C" _AnomalousExport bool Context_Update(Rocket::Core::Context* context)
{
	return context->Update();
}

extern "C" _AnomalousExport bool Context_Render(Rocket::Core::Context* context)
{
	return context->Render();
}

extern "C" _AnomalousExport Rocket::Core::ElementDocument* Context_CreateDocument(Rocket::Core::Context* context)
{
	return context->CreateDocument();
}

extern "C" _AnomalousExport Rocket::Core::ElementDocument* Context_CreateDocument_Tag(Rocket::Core::Context* context, String tag)
{
	return context->CreateDocument(tag);
}

extern "C" _AnomalousExport Rocket::Core::ElementDocument* Context_LoadDocument(Rocket::Core::Context* context, String document_path)
{
	return context->LoadDocument(document_path);
}

//Rocket::Core::ElementDocument* Context_LoadDocument(Stream* document_stream)
//{
//
//}

extern "C" _AnomalousExport Rocket::Core::ElementDocument* Context_LoadDocumentFromMemory(Rocket::Core::Context* context, String string)
{
	return context->LoadDocumentFromMemory(string);
}

extern "C" _AnomalousExport void Context_UnloadDocument(Rocket::Core::Context* context, Rocket::Core::ElementDocument* document)
{
	return context->UnloadDocument(document);
}

extern "C" _AnomalousExport void Context_UnloadAllDocuments(Rocket::Core::Context* context)
{
	return context->UnloadAllDocuments();
}

extern "C" _AnomalousExport void Context_AddMouseCursor(Rocket::Core::Context* context, Rocket::Core::ElementDocument* cursor_document)
{
	return context->AddMouseCursor(cursor_document);
}

extern "C" _AnomalousExport Rocket::Core::ElementDocument* Context_LoadMouseCursor(Rocket::Core::Context* context, String cursor_document_path)
{
	return context->LoadMouseCursor(cursor_document_path);
}

extern "C" _AnomalousExport void Context_UnloadMouseCursor(Rocket::Core::Context* context, String cursor_name)
{
	return context->UnloadMouseCursor(cursor_name);
}

extern "C" _AnomalousExport void Context_UnloadAllMouseCursors(Rocket::Core::Context* context)
{
	return context->UnloadAllMouseCursors();
}

extern "C" _AnomalousExport bool Context_SetMouseCursor(Rocket::Core::Context* context, String cursor_name)
{
	return context->SetMouseCursor(cursor_name);
}

extern "C" _AnomalousExport void Context_ShowMouseCursor(Rocket::Core::Context* context, bool show)
{
	return context->ShowMouseCursor(show);
}

extern "C" _AnomalousExport Rocket::Core::ElementDocument* Context_GetDocument(Rocket::Core::Context* context, String id)
{
	return context->GetDocument(id);
}

extern "C" _AnomalousExport Rocket::Core::ElementDocument* Context_GetDocument_Index(Rocket::Core::Context* context, int index)
{
	return context->GetDocument(index);
}

extern "C" _AnomalousExport int Context_GetNumDocuments(Rocket::Core::Context* context)
{
	return context->GetNumDocuments();
}

extern "C" _AnomalousExport Rocket::Core::Element* Context_GetHoverElement(Rocket::Core::Context* context)
{
	return context->GetHoverElement();
}

extern "C" _AnomalousExport Rocket::Core::Element* Context_GetFocusElement(Rocket::Core::Context* context)
{
	return context->GetFocusElement();
}

extern "C" _AnomalousExport Rocket::Core::Element* Context_GetRootElement(Rocket::Core::Context* context)
{
	return context->GetRootElement();
}

extern "C" _AnomalousExport void Context_PullDocumentToFront(Rocket::Core::Context* context, Rocket::Core::ElementDocument* document)
{
	return context->PullDocumentToFront(document);
}

extern "C" _AnomalousExport void Context_PushDocumentToBack(Rocket::Core::Context* context, Rocket::Core::ElementDocument* document)
{
	return context->PushDocumentToBack(document);
}

extern "C" _AnomalousExport void Context_AddEventListener(Rocket::Core::Context* context, String event, Rocket::Core::EventListener* listener, bool in_capture_phase/* = false*/)
{
	return context->AddEventListener(event, listener, in_capture_phase);
}

extern "C" _AnomalousExport void Context_RemoveEventListener(Rocket::Core::Context* context, String event, Rocket::Core::EventListener* listener, bool in_capture_phase/* = false*/)
{
	return context->RemoveEventListener(event, listener, in_capture_phase);
}

extern "C" _AnomalousExport bool Context_ProcessKeyDown(Rocket::Core::Context* context, Rocket::Core::Input::KeyIdentifier key_identifier, int key_modifier_state)
{
	return context->ProcessKeyDown(key_identifier, key_modifier_state);
}

extern "C" _AnomalousExport bool Context_ProcessKeyUp(Rocket::Core::Context* context, Rocket::Core::Input::KeyIdentifier key_identifier, int key_modifier_state)
{
	return context->ProcessKeyUp(key_identifier, key_modifier_state);
}

extern "C" _AnomalousExport bool Context_ProcessTextInput_Word(Rocket::Core::Context* context, Rocket::Core::word character)
{
	return context->ProcessTextInput(character);
}

extern "C" _AnomalousExport bool Context_ProcessTextInput(Rocket::Core::Context* context, String string)
{
	return context->ProcessTextInput(string);
}

extern "C" _AnomalousExport void Context_ProcessMouseMove(Rocket::Core::Context* context, int x, int y, int key_modifier_state)
{
	return context->ProcessMouseMove(x, y, key_modifier_state);
}

extern "C" _AnomalousExport void Context_ProcessMouseButtonDown(Rocket::Core::Context* context, int button_index, int key_modifier_state)
{
	return context->ProcessMouseButtonDown(button_index, key_modifier_state);
}

extern "C" _AnomalousExport void Context_ProcessMouseButtonUp(Rocket::Core::Context* context, int button_index, int key_modifier_state)
{
	return context->ProcessMouseButtonUp(button_index, key_modifier_state);
}

extern "C" _AnomalousExport bool Context_ProcessMouseWheel(Rocket::Core::Context* context, int wheel_delta, int key_modifier_state)
{
	return context->ProcessMouseWheel(wheel_delta, key_modifier_state);
}

extern "C" _AnomalousExport Rocket::Core::RenderInterface* Context_GetRenderInterface(Rocket::Core::Context* context)
{
	return context->GetRenderInterface();
}