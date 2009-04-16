#pragma once

namespace Rendering
{

[Engine::Attributes::SingleEnum]
public enum class OperationType 
{
	OT_POINT_LIST = 1,
	OT_LINE_LIST = 2,
	OT_LINE_STRIP = 3,
	OT_TRIANGLE_LIST = 4,
	OT_TRIANGLE_STRIP = 5,
	OT_TRIANGLE_FAN = 6
};

[Engine::Attributes::SingleEnum]
public enum class PolygonMode : unsigned int
{
	PM_POINTS = 1,
	PM_WIREFRAME = 2,
	PM_SOLID = 3
};

[Engine::Attributes::SingleEnum]
public enum class SkeletonChunkID : unsigned int
{
	SKELETON_HEADER            = 0x1000,
	SKELETON_BONE              = 0x2000,
	SKELETON_BONE_PARENT       = 0x3000,
	SKELETON_ANIMATION         = 0x4000,
	SKELETON_ANIMATION_TRACK = 0x4100,
	SKELETON_ANIMATION_TRACK_KEYFRAME = 0x4110,
	SKELETON_ANIMATION_LINK         = 0x5000
};

[Engine::Attributes::SingleEnum]
public enum class ProjectionType : unsigned int
{
    PT_ORTHOGRAPHIC,
    PT_PERSPECTIVE
};

[Engine::Attributes::SingleEnum]
public enum class ResourceTypes : unsigned int
{
	Compositor,
	Font,
	GpuProgram,
	HighLevelGpuProgram,
	Material,
	Mesh,
	Skeleton,
	Texture,
	BspLevel,
};

/// <summary>
/// Comparison functions used for the depth/stencil buffer operations and others. 
/// </summary>
[Engine::Attributes::SingleEnum]
public enum class CompareFunction : unsigned int
{
    CMPF_ALWAYS_FAIL,
    CMPF_ALWAYS_PASS,
    CMPF_LESS,
    CMPF_LESS_EQUAL,
    CMPF_EQUAL,
    CMPF_NOT_EQUAL,
    CMPF_GREATER_EQUAL,
    CMPF_GREATER
};

/// <summary>
/// Hardware culling modes based on vertex winding.
/// This setting applies to how the hardware API culls triangles it is sent.
/// </summary>
public enum class CullingMode : unsigned int
{
	/// <summary>
	/// Hardware never culls triangles and renders everything it receives.
	/// </summary>
	CULL_NONE = 1,

	/// <summary>
	/// Hardware culls triangles whose vertices are listed clockwise in the view (default).
	/// </summary>
	CULL_CLOCKWISE = 2,

	/// <summary>
	/// Hardware culls triangles whose vertices are listed anticlockwise in the view.
	/// </summary>
    CULL_ANTICLOCKWISE = 3
};

/// <summary>
/// Manual culling modes based on vertex normals. This setting applies to how
/// the software culls triangles before sending them to the hardware API. This
/// culling mode is used by scene managers which choose to implement it
/// -normally those which deal with large amounts of fixed world geometry which
/// is often planar (software culling movable variable geometry is expensive).
/// </summary>
public enum class ManualCullingMode
{
	/// <summary>
	/// No culling so everything is sent to the hardware.
	/// </summary>
	MANUAL_CULL_NONE = 1,
	/// <summary>
	/// Cull triangles whose normal is pointing away from the camera (default).
	/// </summary>
	MANUAL_CULL_BACK = 2,
	/// <summary>
	/// Cull triangles whose normal is pointing towards the camera.
	/// </summary>
    MANUAL_CULL_FRONT = 3
};


/// <summary>
/// Lighting shading options.
/// </summary>
public enum class ShadeOptions
{
    SO_FLAT,
    SO_GOURAUD,
    SO_PHONG
};

/// <summary>
/// Fog modes. 
/// </summary>
public enum class FogMode
{
	/// <summary>
	/// No fog.
	/// </summary>
	FOG_NONE,
	/// <summary>
	/// Fog density increases  exponentially from the camera (fog = 1/e^(distance * density))
	/// </summary>
	FOG_EXP,
	/// <summary>
	/// Fog density increases at the square of FOG_EXP, i.e. even quicker (fog = 1/e^(distance * density)^2)
	/// </summary>
	FOG_EXP2,
	/// <summary>
	/// Fog density increases linearly between the start and end distances
	/// </summary>
    FOG_LINEAR
};

/// <summary>
/// High-level filtering options providing shortcuts to settings the
/// minification, magnification and mip filters.
/// </summary>
public enum class TextureFilterOptions
{
	/// <summary>
	/// Equal to: min=FO_POINT, mag=FO_POINT, mip=FO_NONE
	/// </summary>
	TFO_NONE,
	/// <summary>
	/// Equal to: min=FO_LINEAR, mag=FO_LINEAR, mip=FO_POINT
	/// </summary>
	TFO_BILINEAR,
	/// <summary>
	/// Equal to: min=FO_LINEAR, mag=FO_LINEAR, mip=FO_LINEAR
	/// </summary>
	TFO_TRILINEAR,
	/// <summary>
	/// Equal to: min=FO_ANISOTROPIC, max=FO_ANISOTROPIC, mip=FO_LINEAR
	/// </summary>
	TFO_ANISOTROPIC
};


/// <summary>
/// Types of blending that you can specify between an object and the existing
/// contents of the scene.
/// <para>
/// As opposed to the LayerBlendType, which classifies blends between texture
/// layers, these blending types blend between the output of the texture units
/// and the pixels already in the viewport, allowing for object transparency,
/// glows, etc. 
/// </para>
/// <para>
/// These types are provided to give quick and easy access to common effects.
/// You can also use the more manual method of supplying source and destination
/// blending factors. See Material::setSceneBlending for more details. 
/// </para>
/// </summary>
public enum class SceneBlendType
{
	/// <summary>
	/// Make the object transparent based on the final alpha values in the texture
	/// </summary>
	SBT_TRANSPARENT_ALPHA,
	/// <summary>
	/// Make the object transparent based on the colour values in the texture (brighter = more opaque)
	/// </summary>
	SBT_TRANSPARENT_COLOUR,
	/// <summary>
	/// Add the texture values to the existing scene content
	/// </summary>
	SBT_ADD,
	/// <summary>
	/// Multiply the 2 colours together
	/// </summary>
	SBT_MODULATE,
	/// <summary>
	/// The default blend mode where source replaces destination
	/// </summary>
    SBT_REPLACE
};

/// <summary>
/// Blending factors for manually blending objects with the scene. If there
/// isn't a predefined SceneBlendType that you like, then you can specify the
/// blending factors directly to affect the combination of object and the
/// existing scene. See Material::setSceneBlending for more details.
/// </summary>
public enum class SceneBlendFactor
{
    SBF_ONE,
    SBF_ZERO,
    SBF_DEST_COLOUR,
    SBF_SOURCE_COLOUR,
    SBF_ONE_MINUS_DEST_COLOUR,
    SBF_ONE_MINUS_SOURCE_COLOUR,
    SBF_DEST_ALPHA,
    SBF_SOURCE_ALPHA,
    SBF_ONE_MINUS_DEST_ALPHA,
    SBF_ONE_MINUS_SOURCE_ALPHA
};

}