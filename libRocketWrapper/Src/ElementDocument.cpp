#include "StdAfx.h"

//void ProcessHeader(const DocumentHeader* header);

extern "C" _AnomalousExport void ElementDocument_SetTitle(Rocket::Core::ElementDocument* elementDocument, String title)
{
	Rocket::Core::String rktTitle = Rocket::Core::String(title);
	elementDocument->SetTitle(rktTitle);
}
	
extern "C" _AnomalousExport String ElementDocument_GetTitle(Rocket::Core::ElementDocument* elementDocument)
{
	return elementDocument->GetTitle().CString();
}

extern "C" _AnomalousExport String ElementDocument_GetSourceURL(Rocket::Core::ElementDocument* elementDocument)
{
	return elementDocument->GetSourceURL().CString();
}

//void SetStyleSheet(StyleSheet* style_sheet);

extern "C" _AnomalousExport void ElementDocument_PullToFront(Rocket::Core::ElementDocument* elementDocument)
{
	elementDocument->PullToFront();
}
	
extern "C" _AnomalousExport void ElementDocument_PushToBack(Rocket::Core::ElementDocument* elementDocument)
{
	elementDocument->PushToBack();
}

extern "C" _AnomalousExport void ElementDocument_Show(Rocket::Core::ElementDocument* elementDocument, Rocket::Core::ElementDocument::FocusFlags focusFlags)
{
	elementDocument->Show(focusFlags);
}

extern "C" _AnomalousExport void ElementDocument_Hide(Rocket::Core::ElementDocument* elementDocument)
{
	elementDocument->Hide();
}

extern "C" _AnomalousExport void ElementDocument_Close(Rocket::Core::ElementDocument* elementDocument)
{
	elementDocument->Close();
}

extern "C" _AnomalousExport Rocket::Core::Element* ElementDocument_CreateElement(Rocket::Core::ElementDocument* elementDocument, String name)
{
	return elementDocument->CreateElement(name);
}
	
extern "C" _AnomalousExport Rocket::Core::ElementText* ElementDocument_CreateTextNode(Rocket::Core::ElementDocument* elementDocument, String text)
{
	return elementDocument->CreateTextNode(text);
}

extern "C" _AnomalousExport bool ElementDocument_IsModal(Rocket::Core::ElementDocument* elementDocument)
{
	return elementDocument->IsModal();
}

extern "C" _AnomalousExport void ElementDocument_UpdatePosition(Rocket::Core::ElementDocument* elementDocument)
{
	elementDocument->UpdatePosition();
}

extern "C" _AnomalousExport void ElementDocument_UpdateLayout(Rocket::Core::ElementDocument* elementDocument)
{
	elementDocument->UpdateLayout();
}