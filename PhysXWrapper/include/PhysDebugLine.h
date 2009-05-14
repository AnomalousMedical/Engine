#pragma once

namespace PhysXWrapper
{

using namespace System::Runtime::InteropServices;

[StructLayout(LayoutKind::Sequential)]
public value class PhysDebugLine
{
public:
	Engine::Vector3 p0;

	Engine::Vector3 p1;

	NxU32 color;
};

}