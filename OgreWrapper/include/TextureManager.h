#pragma once

#include "TexturePtr.h"
#include "Texture.h"
#include "PixelBox.h"

namespace Ogre
{
	class TextureManager;
}

namespace OgreWrapper{

ref class TexturePtr;

/// <summary>
/// 
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class TextureManager
{
private:
	static TextureManager^ instance = nullptr;

	TexturePtrCollection texturePtrCollection;

	Ogre::TextureManager* textureManager;

	/// <summary>
	/// Constructor
	/// </summary>
	TextureManager();

internal:
	/// <summary>
	/// Returns the native TextureManager
	/// </summary>
	Ogre::TextureManager* getTextureManager();

	TexturePtr^ getObject(const Ogre::TexturePtr& ogrePtr);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~TextureManager();

	/// <summary>
	/// Get the instance of this TextureManager.
	/// </summary>
	/// <returns>The MeshManager instance.</returns>
	static TextureManager^ getInstance();

	TexturePtr^ createManual(System::String^ name, System::String^ group, TextureType texType, unsigned int width, unsigned int height, unsigned int depth, int num_mips, PixelFormat format, TextureUsage usage, bool hwGammaCorrection, unsigned int fsaa);
};

}
