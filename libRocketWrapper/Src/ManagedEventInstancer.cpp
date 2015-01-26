#include "StdAfx.h"

class ManagedEventInstancer : public Rocket::Core::EventInstancer
{
public:
	typedef Rocket::Core::Event* (*InstanceEventCb)(Rocket::Core::Element* target, String name, const Rocket::Core::Dictionary* parameters, bool interuptable HANDLE_ARG);
	typedef void(*ReleaseEventCb)(Rocket::Core::Event* event  HANDLE_ARG);
	typedef void (*ReleaseCb)(HANDLE_FIRST_ARG);

	ManagedEventInstancer(InstanceEventCb instanceEvent, ReleaseEventCb releaseEvent, ReleaseCb release HANDLE_ARG)
		:instanceEvent(instanceEvent),
		releaseEvent(releaseEvent),
		release(release)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedEventInstancer()
	{

	}

	/// Instance an event
	virtual Rocket::Core::Event* InstanceEvent(Rocket::Core::Element* target, const Rocket::Core::String& name, const Rocket::Core::Dictionary& parameters, bool interuptable)
	{
		return instanceEvent(target, name.CString(), &parameters, interuptable PASS_HANDLE_ARG);
	}

	/// Releases an event instanced by this instancer.
	/// @param[in] event The event to release.
	virtual void ReleaseEvent(Rocket::Core::Event* event)
	{
		releaseEvent(event PASS_HANDLE_ARG);
	}

	/// Release this instancer
	virtual void Release()
	{
		release(PASS_HANDLE);
	}

private:
	InstanceEventCb instanceEvent;
	ReleaseEventCb releaseEvent;
	ReleaseCb release;
	HANDLE_INSTANCE
};

extern "C" _AnomalousExport ManagedEventInstancer* ManagedEventInstancer_Create(ManagedEventInstancer::InstanceEventCb instanceEvent, ManagedEventInstancer::ReleaseEventCb releaseEvent, ManagedEventInstancer::ReleaseCb release HANDLE_ARG)
{
	return new ManagedEventInstancer(instanceEvent, releaseEvent, release PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedEventInstancer_Delete(ManagedEventInstancer* eventInstancer)
{
	delete eventInstancer;
}