#pragma once

#include "RenderTarget.h"

namespace Ogre
{
	class RenderTexture;
}

namespace OgreWrapper{

/// <summary>
/// 
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class RenderTexture : public RenderTarget
{
private:
	Ogre::RenderTexture* renderTexture;

internal:
	/// <summary>
	/// Returns the native RenderTexture
	/// </summary>
	Ogre::RenderTexture* getRenderTexture();

	/// <summary>
	/// Constructor
	/// </summary>
	RenderTexture(Ogre::RenderTexture* renderTexture);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~RenderTexture();
};

}
