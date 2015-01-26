#include "StdAfx.h"

class ManagedEventListenerInstancer : public Rocket::Core::EventListenerInstancer
{
public:
	typedef Rocket::Core::EventListener* (*InstanceEventListenerCb)(String name, Rocket::Core::Element* element HANDLE_ARG);
	typedef void(*ReleaseCb)(HANDLE_FIRST_ARG);

	ManagedEventListenerInstancer(InstanceEventListenerCb instanceEventListener, ReleaseCb release HANDLE_ARG)
		:instanceEventListener(instanceEventListener),
		release(release)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedEventListenerInstancer() {}

	/// Instance and event listener object
	/// @param value Value of the event	
	virtual Rocket::Core::EventListener* InstanceEventListener(const Rocket::Core::String& value, Rocket::Core::Element* element)
	{
		return instanceEventListener(value.CString(), element PASS_HANDLE_ARG);
	}

	/// Releases this event listener instancer
	virtual void Release()
	{
		release(PASS_HANDLE);
	}

private:
	InstanceEventListenerCb instanceEventListener;
	ReleaseCb release;
	HANDLE_INSTANCE
};

extern "C" _AnomalousExport ManagedEventListenerInstancer* ManagedEventListenerInstancer_Create(ManagedEventListenerInstancer::InstanceEventListenerCb instanceEventListener, ManagedEventListenerInstancer::ReleaseCb release HANDLE_ARG)
{
	return new ManagedEventListenerInstancer(instanceEventListener, release PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedEventListenerInstancer_Delete(ManagedEventListenerInstancer* eventListenerInstancer)
{
	delete eventListenerInstancer;
}