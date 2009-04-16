#pragma once

#include "OgreOverlayManager.h"
#include "AutoPtr.h"
#include "OverlayElementCollection.h"
#include "OverlayCollection.h"

namespace OgreWrapper{

ref class Overlay;
ref class OverlayElement;

[Engine::Attributes::DoNotSaveAttribute]
public ref class OverlayManager
{
private:
	OverlayCollection overlays;
	OverlayElementCollection overlayElements;
	Ogre::OverlayManager* overlayManager;

	static OverlayManager^ instance = gcnew OverlayManager();

internal:
	OverlayManager();

	OverlayElement^ getObject(const Ogre::OverlayElement* overlayElement);

public:
	static OverlayManager^ getInstance()
	{
		return instance;
	}

	Overlay^ create(System::String^ name);

	Overlay^ getByName(System::String^ name);

	void destroy(System::String^ name);

	void destroy(Overlay^ overlay);

	void destroyAll();

	bool hasViewportChanged();

	int getViewportHeight();

	int getViewportWidth();

	float getViewportAspectRatio();

	OverlayElement^ createOverlayElement(System::String^ typeName, System::String^ instanceName);

	OverlayElement^ createOverlayElement(System::String^ typeName, System::String^ instanceName, bool isTemplate);

	OverlayElement^ getOverlayElement(System::String^ name);

	OverlayElement^ getOverlayElement(System::String^ name, bool isTemplate);

	void destroyOverlayElement(System::String^ name);

	void destroyOverlayElement(System::String^ name, bool isTemplate);

	void destroyOverlayElement(OverlayElement^ element);

	void destroyOverlayElement(OverlayElement^ element, bool isTemplate);

	void destroyAllOverlayElements();

	void destroyAllOverlayElements(bool isTemplate);

	OverlayElement^ createOverlayElementFromTemplate(System::String^ templateName, System::String^ typeName, System::String^ instanceName);

	OverlayElement^ createOverlayElementFromTemplate(System::String^ templateName, System::String^ typeName, System::String^ instanceName, bool isTemplate);

	OverlayElement^ cloneOverlayElementFromTemplate(System::String^ templateName, System::String^ instanceName);
};

}