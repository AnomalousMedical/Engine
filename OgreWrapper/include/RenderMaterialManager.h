#pragma once

#include "Enums.h"
#include "RenderMaterialPtr.h"

namespace Ogre
{
	class MaterialManager;
	class Material;
}

namespace OgreWrapper{

ref class RenderMaterial;

/// <summary>
/// Handles the management of material resources.
/// <para>
/// This class deals with the runtime management of material data; like other
/// resource managers it handles the creation of resources (in this case material
/// data), working within a fixed memory budget. 
/// </para>
/// <para>
/// The wrapper also includes a centralized method to identify ogre materials
/// so that the same RenderMaterial class is returned on each request for an ogre material
/// see getMaterialObject.
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class RenderMaterialManager
{
private:
	RenderMaterialPtrCollection renderMaterials;

	Ogre::MaterialManager* materialManager;
	static RenderMaterialManager^ instance = gcnew RenderMaterialManager();

	RenderMaterialManager();

internal:
	RenderMaterialPtr^ getObject(const Ogre::MaterialPtr& materialPtr);

public:
	virtual ~RenderMaterialManager();

	/// <summary>
	/// Get the instance of this RenderMaterialManager.
	/// </summary>
	/// <returns>The RenderMaterialManager instance.</returns>
	static RenderMaterialManager^ getInstance();

	RenderMaterialPtr^ getByName(System::String^ name);

	bool resourceExists(System::String^ name);

	//RenderMaterial^ createManual(System::String^ name, System::String^ groupName, bool isManual);
};

}