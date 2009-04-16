#pragma once
/// <file>ManualObjectSection.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "Enums.h"

namespace Ogre
{
	class ManualObject;
	class ManualObject::ManualObjectSection;
}

namespace Rendering
{

/// <summary>
/// This class wraps a native ManualObjectSection.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
[Engine::Attributes::DoNotSaveAttribute]
public ref class ManualObjectSection
{
private:
	Ogre::ManualObject::ManualObjectSection* section;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="section">The Ogre ManualObjectSection to wrap.</param>
	ManualObjectSection(Ogre::ManualObject::ManualObjectSection* section);

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~ManualObjectSection(void);

public:
	//getRenderOperation

	/// <summary>
	/// Retrieve the material name in use. 
	/// </summary>
	/// <returns>The material name.</returns>
	System::String^ getMaterialName();

	/// <summary>
	/// Update the material name in use.
	/// </summary>
	/// <param name="name">The new material to use.</param>
	void setMaterialName(System::String^ name);

	/// <summary>
	/// Set whether we need 32-bit indices. 
	/// </summary>
	/// <param name="n32">True to use 32 bit indices.  False to not use them.</param>
	void set32BitIndices(bool n32);

	/// <summary>
	/// Get whether we need 32-bit indices.
	/// </summary>
	/// <returns>True if 32 bit indicies are in use.</returns>
	bool get32BitIndices();
};

}