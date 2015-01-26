#include "StdAfx.h"

class ManagedEventListener : Rocket::Core::EventListener
{
	public:
		typedef void (*ProcessEventCb)(Rocket::Core::Event* evt HANDLE_ARG);
		typedef void (*AttachDetatchCb)(Rocket::Core::Element* element HANDLE_ARG);

		ManagedEventListener(ProcessEventCb processEvent, AttachDetatchCb onAttach, AttachDetatchCb onDetatch HANDLE_ARG)
			:processEvent(processEvent),
			onAttach(onAttach),
			onDetatch(onDetatch)
			ASSIGN_HANDLE_INITIALIZER
		{

		}

		virtual ~ManagedEventListener() {}

		/// Process the incoming Event
		virtual void ProcessEvent(Rocket::Core::Event& event)
		{
			processEvent(&event PASS_HANDLE_ARG);
		}

		/// Called when the listener has been attached to a new Element
		virtual void OnAttach(Rocket::Core::Element* element)
		{
			onAttach(element PASS_HANDLE_ARG);
		}

		/// Called when the listener has been detached from a Element
		virtual void OnDetach(Rocket::Core::Element* element)
		{
			onDetatch(element PASS_HANDLE_ARG);
		}

private:
	ProcessEventCb processEvent;
	AttachDetatchCb onAttach;
	AttachDetatchCb onDetatch;
	HANDLE_INSTANCE
};

extern "C" _AnomalousExport ManagedEventListener* ManagedEventListener_Create(ManagedEventListener::ProcessEventCb processEvent, ManagedEventListener::AttachDetatchCb onAttach, ManagedEventListener::AttachDetatchCb onDetatch HANDLE_ARG)
{
	return new ManagedEventListener(processEvent, onAttach, onDetatch PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedEventListener_Delete(ManagedEventListener* managedEventListener)
{
	delete managedEventListener;
}