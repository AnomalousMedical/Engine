#pragma once

#include "OgreHardwarePixelBuffer.h"
#include "AutoPtr.h"

namespace Ogre
{
	class HardwarePixelBuffer;
}

namespace OgreWrapper{

ref class RenderTexture;

/// <summary>
/// 
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class HardwarePixelBuffer
{
private:
	AutoPtr<Ogre::HardwarePixelBufferSharedPtr> pixelBufferAutoPtr;
	Ogre::HardwarePixelBuffer* pixelBuffer;
	RenderTexture^ renderTexture;

internal:
	/// <summary>
	/// Returns the native HardwarePixelBuffer
	/// </summary>
	Ogre::HardwarePixelBuffer* getHardwarePixelBuffer();

	/// <summary>
	/// Constructor
	/// </summary>
	HardwarePixelBuffer(const Ogre::HardwarePixelBufferSharedPtr& pixelBuffer);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~HardwarePixelBuffer();

	RenderTexture^ getRenderTarget();
};

}