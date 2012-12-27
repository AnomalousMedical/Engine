#include "StdAfx.h"
#include "Rocket/Core/StreamMemory.h"

/// Clears the style sheet cache. This will force style sheets to be reloaded.
extern "C" _AnomalousExport void Factory_ClearStyleSheetCache()
{
	Rocket::Core::Factory::ClearStyleSheetCache();
}

/// Registers an instancer for all events.
/// @param[in] instancer The instancer to be called.
/// @return The registered instanced on success, NULL on failure.
extern "C" _AnomalousExport Rocket::Core::EventInstancer* Factory_RegisterEventInstancer(Rocket::Core::EventInstancer* instancer)
{
	return Rocket::Core::Factory::RegisterEventInstancer(instancer);
}

/// Register the instancer to be used for all event listeners.
/// @return The registered instancer on success, NULL on failure.
extern "C" _AnomalousExport Rocket::Core::EventListenerInstancer* Factory_RegisterEventListenerInstancer(Rocket::Core::EventListenerInstancer* instancer)	
{
	return Rocket::Core::Factory::RegisterEventListenerInstancer(instancer);
}

extern "C" _AnomalousExport bool Factory_InstanceElementString(Rocket::Core::Element* parent, String rml)
{
	Rocket::Core::String string = rml;
	// Open the stream based on the string contents.
	Rocket::Core::StreamMemory* stream = new Rocket::Core::StreamMemory((byte*)string.CString(), string.Length());
	stream->SetSourceURL("[document from memory]");

	// Load the document from the stream.
	bool loaded = Rocket::Core::Factory::InstanceElementStream(parent, stream);

	stream->RemoveReference();

	return loaded;
}