#pragma once

#include "OgreTexture.h"
#include "AutoPtr.h"
#include "Resource.h"
#include "HardwareBuffer.h"

namespace OgreWrapper{

ref class HardwarePixelBufferSharedPtr;

/** Enum identifying the texture usage
*/
public enum class TextureUsage
{
	/// @copydoc HardwareBuffer::Usage
	TU_STATIC = HardwareBuffer::Usage::HBU_STATIC,
	TU_DYNAMIC = HardwareBuffer::Usage::HBU_DYNAMIC,
	TU_WRITE_ONLY = HardwareBuffer::Usage::HBU_WRITE_ONLY,
	TU_STATIC_WRITE_ONLY = HardwareBuffer::Usage::HBU_STATIC_WRITE_ONLY, 
	TU_DYNAMIC_WRITE_ONLY = HardwareBuffer::Usage::HBU_DYNAMIC_WRITE_ONLY,
	TU_DYNAMIC_WRITE_ONLY_DISCARDABLE = HardwareBuffer::Usage::HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE,
	/// mipmaps will be automatically generated for this texture
	TU_AUTOMIPMAP = 0x100,
	/// this texture will be a render target, i.e. used as a target for render to texture
	/// setting this flag will ignore all other texture usages except TU_AUTOMIPMAP
	TU_RENDERTARGET = 0x200,
	/// default to automatic mipmap generation static textures
	TU_DEFAULT = TU_AUTOMIPMAP | TU_STATIC_WRITE_ONLY
    
};

/** Enum identifying the texture type
*/
public enum class TextureType
{
    /// 1D texture, used in combination with 1D texture coordinates
    TEX_TYPE_1D = 1,
    /// 2D texture, used in combination with 2D texture coordinates (default)
    TEX_TYPE_2D = 2,
    /// 3D volume texture, used in combination with 3D texture coordinates
    TEX_TYPE_3D = 3,
    /// 3D cube map, used in combination with 3D texture coordinates
    TEX_TYPE_CUBE_MAP = 4
};

/// <summary>
/// 
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class Texture : public Resource
{
private:
	AutoPtr<Ogre::TexturePtr> textureAutoPtr;
	Ogre::Texture* texture;

internal:
	/// <summary>
	/// Returns the native Texture
	/// </summary>
	Ogre::Texture* getTexture();

	/// <summary>
	/// Constructor
	/// </summary>
	Texture(const Ogre::TexturePtr& texture);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~Texture();

	HardwarePixelBufferSharedPtr^ getBuffer();
};

}
