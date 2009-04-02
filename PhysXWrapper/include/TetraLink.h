#pragma once

using namespace System::Runtime::InteropServices;

namespace PhysXWrapper
{

ref class TetraMesh;

[StructLayout(LayoutKind::Explicit, Size=16, CharSet=CharSet::Ansi)]
public value class TetraLink
{
public:
	[FieldOffset(0)]
	EngineMath::Vector3 barycentricCoord;
	[FieldOffset(12)]
	unsigned int tetraIndex;

	static void buildLinks(TetraMesh^ graphicsMesh, TetraMesh^ tetraMesh, TetraLink* tetraLinks);
};

}