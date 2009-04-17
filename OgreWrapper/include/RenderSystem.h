#pragma once

namespace Ogre
{
	class RenderSystem;
}

namespace OgreWrapper{

/// <summary>
/// 
/// </summary>
public ref class RenderSystem
{
private:
	Ogre::RenderSystem* renderSystem;

internal:
	/// <summary>
	/// Returns the native RenderSystem
	/// </summary>
	Ogre::RenderSystem* getRenderSystem();

	/// <summary>
	/// Constructor
	/// </summary>
	RenderSystem(Ogre::RenderSystem* renderSystem);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~RenderSystem();

	/// <summary>
	/// Validates the options set for the rendering system, returning a message
    /// if there are problems. 
	/// </summary>
	/// <returns>An error message or an empty string if there are no problems.</returns>
	System::String^ validateConfigOptions();

	void _initRenderTargets();
};

}