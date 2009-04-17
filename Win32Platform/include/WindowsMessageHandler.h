#pragma once

namespace Engine
{

namespace Platform
{

ref class WindowsMessageHandler : public UpdateListener
{
public:
	WindowsMessageHandler(void);

	virtual void sendUpdate( Clock^ clock );

	virtual void loopStarting();

	virtual void exceededMaxDelta();
};

}

}