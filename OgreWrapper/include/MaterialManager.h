#pragma once

#include "Enums.h"
#include "MaterialPtr.h"

namespace Ogre
{
	class MaterialManager;
	class Material;
}

namespace OgreWrapper{

ref class Material;

/// <summary>
/// Handles the management of material resources.
/// <para>
/// This class deals with the runtime management of material data; like other
/// resource managers it handles the creation of resources (in this case material
/// data), working within a fixed memory budget. 
/// </para>
/// <para>
/// The wrapper also includes a centralized method to identify ogre materials
/// so that the same Material class is returned on each request for an ogre material
/// see getMaterialObject.
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class MaterialManager
{
private:
	RenderMaterialPtrCollection renderMaterials;

	Ogre::MaterialManager* materialManager;
	static MaterialManager^ instance = gcnew MaterialManager();

	MaterialManager();

internal:
	MaterialPtr^ getObject(const Ogre::MaterialPtr& materialPtr);

public:
	virtual ~MaterialManager();

	/// <summary>
	/// Get the instance of this MaterialManager.
	/// </summary>
	/// <returns>The MaterialManager instance.</returns>
	static MaterialManager^ getInstance();

	MaterialPtr^ getByName(System::String^ name);

	bool resourceExists(System::String^ name);

	//Material^ createManual(System::String^ name, System::String^ groupName, bool isManual);
};

}