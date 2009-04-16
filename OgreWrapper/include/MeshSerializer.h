#pragma once

#include "AutoPtr.h"

namespace Ogre
{
	class MeshSerializer;
}

namespace Engine{

namespace Rendering{

ref class Mesh;

/// <summary>
/// 
/// </summary>
public ref class MeshSerializer
{
public:
	enum class Endian : unsigned int
		{
			/// Use the platform native endian
			ENDIAN_NATIVE,
			/// Use big endian (0x1000 is serialised as 0x10 0x00)
			ENDIAN_BIG,
			/// Use little endian (0x1000 is serialised as 0x00 0x10)
			ENDIAN_LITTLE
		};

private:
	AutoPtr<Ogre::MeshSerializer> meshSerializer;

internal:
	/// <summary>
	/// Returns the native MeshSerializer
	/// </summary>
	Ogre::MeshSerializer* getMeshSerializer();

public:
	/// <summary>
	/// Constructor
	/// </summary>
	MeshSerializer();

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~MeshSerializer();

	void exportMesh(Mesh^ mesh, System::String^ filename);

	void exportMesh(Mesh^ mesh, System::String^ filename, Endian endianMode);
};

}

}
