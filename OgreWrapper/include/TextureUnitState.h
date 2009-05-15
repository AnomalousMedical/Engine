#pragma once

namespace Ogre
{
	class TextureUnitState;
}

namespace OgreWrapper{

/// <summary>
/// 
/// </summary>
public ref class TextureUnitState
{
private:
	Ogre::TextureUnitState* textureUnitState;

internal:
	/// <summary>
	/// Returns the native TextureUnitState
	/// </summary>
	Ogre::TextureUnitState* getTextureUnitState();

	/// <summary>
	/// Constructor
	/// </summary>
	TextureUnitState(Ogre::TextureUnitState* textureUnitState);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~TextureUnitState();

	System::String^ getTextureName();

	void setTextureName(System::String^ name);
};

}
