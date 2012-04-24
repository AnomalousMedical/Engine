#include "StdAfx.h"

class ManagedEventListener : Rocket::Core::EventListener
{
	public:
		typedef void (*ProcessEventCb)(Rocket::Core::Event* evt);
		typedef void (*AttachDetatchCb)(Rocket::Core::Element* element);

		ManagedEventListener(ProcessEventCb processEvent, AttachDetatchCb onAttach, AttachDetatchCb onDetatch)
			:processEvent(processEvent),
			onAttach(onAttach),
			onDetatch(onDetatch)
		{

		}

		virtual ~ManagedEventListener() {}

		/// Process the incoming Event
		virtual void ProcessEvent(Rocket::Core::Event& event)
		{
			processEvent(&event);
		}

		/// Called when the listener has been attached to a new Element
		virtual void OnAttach(Rocket::Core::Element* element)
		{
			onAttach(element);
		}

		/// Called when the listener has been detached from a Element
		virtual void OnDetach(Rocket::Core::Element* element)
		{
			onDetatch(element);
		}

private:
	ProcessEventCb processEvent;
	AttachDetatchCb onAttach;
	AttachDetatchCb onDetatch;
};

extern "C" _AnomalousExport ManagedEventListener* ManagedEventListener_Create(ManagedEventListener::ProcessEventCb processEvent, ManagedEventListener::AttachDetatchCb onAttach, ManagedEventListener::AttachDetatchCb onDetatch)
{
	return new ManagedEventListener(processEvent, onAttach, onDetatch);
}

extern "C" _AnomalousExport void ManagedEventListener_Delete(ManagedEventListener* managedEventListener)
{
	delete managedEventListener;
}