/// <file>Light.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "MovableObject.h"
#include "Enums.h"

namespace Ogre
{
	class Light;
}

namespace OgreWrapper
{

/// <summary>
/// Provides a wrapper for a native light.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class Light : MovableObject
{
public:
[Engine::Attributes::SingleEnum]
enum class LightTypes : unsigned int
{
	LT_POINT,
	LT_DIRECTIONAL,
	LT_SPOTLIGHT
};

private:
	Ogre::Light* light;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="light">The native light to wrap.</param>
	/// <param name="name">The name of the light.</param>
	Light(Ogre::Light* light);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~Light();

	/// <summary>
	/// Gets the native light wrapped by this class.
	/// </summary>
	/// <returns>The native light wrapped by this class.</returns>
	Ogre::Light* getLight();

	/// <summary>
	/// Gets the name of this light.
	/// </summary>
	/// <returns>Returns the name of the light.</returns>
	virtual System::String^ getName() override;

	/// <summary>
	/// Sets the type of light - see LightTypes for more info.
	/// </summary>
	/// <param name="type">The type of light.</param>
	void setType(Light::LightTypes type);

	/// <summary>
	/// Returns the light type.
	/// </summary>
	/// <returns>The LightTypes of the light.</returns>
	Light::LightTypes getType();

	/// <summary>
	/// Sets the colour of the diffuse light given off by this source.
	/// </summary>
	/// <param name="red">Red color component.</param>
	/// <param name="green">Green color component.</param>
	/// <param name="blue">Blue color component.</param>
	void setDiffuseColor(float red, float green, float blue);

	/// <summary>
	/// Sets the colour of the diffuse light given off by this source.
	/// </summary>
	/// <param name="color">A vector3 with the color info.  x=red, y=green, z=blue</param>
	void setDiffuseColor(Engine::Color color);

	/// <summary>
	/// Get the diffuse color of the light.
	/// </summary>
	Engine::Color getDiffuseColor();

	/// <summary>
	/// Sets the colour of the specular light given off by this source.
	/// </summary>
	/// <param name="red">Red color component.</param>
	/// <param name="green">Green color component.</param>
	/// <param name="blue">Blue color component.</param>
	void setSpecularColor(float red, float green, float blue);

	/// <summary>
	/// Sets the colour of the specular light given off by this source.
	/// </summary>
	/// <param name="color">A vector3 with the color info.  x=red, y=green, z=blue</param>
	void setSpecularColor(Engine::Color color);

	/// <summary>
	/// Get the specular color of the light.
	/// </summary>
	Engine::Color getSpecularColor();

	/// <summary>
	/// Sets the attenuation parameters of the light source ie how it diminishes with distance.
	///
	/// Remarks:
    /// Lights normally get fainter the further they are away. Also, each light is given a 
	/// maximum range beyond which it cannot affect any objects. 
	///
    /// Light attentuation is not applicable to directional lights since they have an 
	/// infinite range and constant intensity.
	/// </summary>
	/// <param name="range">The absolute upper range of the light in world units.</param>
	/// <param name="constant">The constant factor in the attenuation formula: 1.0 means never attenuate, 0.0 is complete attenuation.</param>
	/// <param name="linear">The linear factor in the attenuation formula: 1 means attenuate evenly over the distance.</param>
	/// <param name="quadratic">The quadratic factor in the attenuation formula: adds a curvature to the attenuation formula.</param>
	void setAttenuation(float range, float constant, float linear, float quadratic);

	/// <summary>
	/// Returns the absolute upper range of the light.
	/// </summary>
	/// <returns>Returns the absolute upper range of the light.</returns>
	float getAttenuationRange();

	/// <summary>
	/// Returns the constant factor in the attenuation formula.
	/// </summary>
	/// <returns>Returns the constant factor in the attenuation formula.</returns>
	float getAttenuationConstant();

	/// <summary>
	/// Returns the linear factor in the attenuation formula.
	/// </summary>
	/// <returns>Returns the linear factor in the attenuation formula.</returns>
	float getAttenuationLinear();
	
	/// <summary>
	/// Returns the quadric factor in the attenuation formula.
	/// </summary>
	/// <returns>Returns the quadric factor in the attenuation formula.</returns>
	float getAttenuationQuadric();

	/// <summary>
	/// Sets the position of the light. Applicable to point lights and
    /// spotlights only. This will be overridden if the light is attached to a
    /// SceneNode. 
	/// </summary>
	/// <param name="pos">The position to set.</param>
	void setPosition(Engine::Vector3 pos);

	/// <summary>
	/// Sets the position of the light. Applicable to point lights and
    /// spotlights only. This will be overridden if the light is attached to a
    /// SceneNode. 
	/// </summary>
	/// <param name="x">The x translation.</param>
	/// <param name="y">The y translation.</param>
	/// <param name="z">The z translation.</param>
	void setPosition(float x, float y, float z);

	/// <summary>
	/// Returns the position of the light. Applicable to point lights and
    /// spotlights only.
	/// </summary>
	/// <returns></returns>
	Engine::Vector3 getPosition();

	/// <summary>
	/// Sets the direction in which a light points.
	/// </summary>
	/// <param name="x">X Direction.</param>
	/// <param name="y">Y Direction.</param>
	/// <param name="z">Z Direction.</param>
	void setDirection(float x, float y, float z);

	/// <summary>
	/// Sets the direction in which a light points.
	/// </summary>
	/// <param name="dir">A vector containing the direction.</param>
	void setDirection(Engine::Vector3 dir);

	/// <summary>
	/// Get the direction the light is facing.
	/// </summary>
	Engine::Vector3 getDirection();

	/// <summary>
	/// Sets the range of a spotlight, i.e.
	/// 
	/// the angle of the inner and outer cones and the rate of falloff between them
	/// </summary>
	/// <param name="innerAngleRad">Angle covered by the bright inner cone The inner cone applicable only to Direct3D, it'll always treat as zero in OpenGL.</param>
	/// <param name="outerAngleRad">Angle covered by the outer cone.</param>
	/// <param name="falloff">The rate of falloff between the inner and outer cones. 1.0 means a linear falloff, less means slower falloff, higher means faster falloff.</param>
	void setSpotlightRange(float innerAngleRad, float outerAngleRad, float falloff);

	/// <summary>
	/// Returns the angle covered by the spotlights inner cone.
	/// </summary>
	/// <returns>Returns the angle covered by the spotlights inner cone.</returns>
	float getSpotlightInnerAngle();

	/// <summary>
	/// Returns the angle covered by the spotlights outer cone.
	/// </summary>
	/// <returns>Returns the angle covered by the spotlights outer cone.</returns>
	float getSpotlightOuterAngle();

	/// <summary>
	/// Returns the falloff between the inner and outer cones of the spotlight.
	/// </summary>
	/// <returns>Returns the falloff between the inner and outer cones of the spotlight.</returns>
	float getSpotlightFalloff();

	/// <summary>
	/// Sets the angle covered by the spotlights inner cone.
	/// </summary>
	/// <param name="innerAngleRad">Angle covered by the bright inner cone The inner cone applicable only to Direct3D, it'll always treat as zero in OpenGL.</param>
	void setSpotlightInnerAngle(float innerAngleRad);

	/// <summary>
	/// Sets the angle covered by the spotlights outer cone.
	/// </summary>
	/// <param name="outerAngleRad">Angle covered by the outer cone.</param>
	void setSpotlightOuterAngle(float outerAngleRad);

	/// <summary>
	/// Sets the falloff between the inner and outer cones of the spotlight.
	/// </summary>
	/// <param name="value">The rate of falloff between the inner and outer cones. 1.0 means a linear falloff, less means slower falloff, higher means faster falloff.</param>
	void setSpotlightFalloff(float value);

	/// <summary>
	/// Set a scaling factor to indicate the relative power of a light.
	/// This factor is only useful in High Dynamic Range (HDR) rendering. You can bind it 
	/// to a shader variable to take it into account.
	/// </summary>
	/// <param name="power">The power rating of this light, default is 1.0.</param>
	void setPowerScale(float power);

	/// <summary>
	/// Get the scaling factor which indicates the relative power of a light.
	/// </summary>
	/// <returns>Get the scaling factor which indicates the relative power of a light.</returns>
	float getPowerScale();
};

}