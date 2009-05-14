#pragma once

namespace PhysXWrapper
{

using namespace System::Runtime::InteropServices;

[StructLayout(LayoutKind::Sequential)]
public value class PhysDebugPoint
{
public:
	Engine::Vector3 p;

	NxU32 color;
};

}