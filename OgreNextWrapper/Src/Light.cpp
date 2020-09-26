#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport void Light_setType(Ogre::Light* light, Ogre::Light::LightTypes type)
{
	light->setType(type);
}

extern "C" _AnomalousExport Ogre::Light::LightTypes Light_getType(Ogre::Light* light)
{
	return light->getType();
}

extern "C" _AnomalousExport void Light_setDiffuseColorRaw(Ogre::Light* light, float red, float green, float blue)
{
	light->setDiffuseColour(red, green, blue);
}

extern "C" _AnomalousExport void Light_setDiffuseColor(Ogre::Light* light, Color color)
{
	light->setDiffuseColour(color.toOgre());
}

extern "C" _AnomalousExport Color Light_getDiffuseColor(Ogre::Light* light)
{
	return light->getDiffuseColour();
}

extern "C" _AnomalousExport void Light_setSpecularColorRaw(Ogre::Light* light, float red, float green, float blue)
{
	light->setSpecularColour(red, green, blue);
}

extern "C" _AnomalousExport void Light_setSpecularColor(Ogre::Light* light, Color color)
{
	light->setSpecularColour(color.toOgre());
}

extern "C" _AnomalousExport Color Light_getSpecularColor(Ogre::Light* light)
{
	return light->getSpecularColour();
}

extern "C" _AnomalousExport void Light_setAttenuation(Ogre::Light* light, float range, float constant, float linear, float quadratic)
{
	light->setAttenuation(range, constant, linear, quadratic);
}

extern "C" _AnomalousExport float Light_getAttenuationRange(Ogre::Light* light)
{
	return light->getAttenuationRange();
}

extern "C" _AnomalousExport float Light_getAttenuationConstant(Ogre::Light* light)
{
	return light->getAttenuationConstant();
}

extern "C" _AnomalousExport float Light_getAttenuationLinear(Ogre::Light* light)
{
	return light->getAttenuationLinear();
}

extern "C" _AnomalousExport float Light_getAttenuationQuadric(Ogre::Light* light)
{
	return light->getAttenuationQuadric();
}

extern "C" _AnomalousExport void Light_setDirection(Ogre::Light* light, Vector3 dir)
{
	light->setDirection(dir.toOgre());
}

extern "C" _AnomalousExport Vector3 Light_getDirection(Ogre::Light* light)
{
	return light->getDirection();
}

extern "C" _AnomalousExport void Light_setSpotlightRange(Ogre::Light* light, float innerAngleRad, float outerAngleRad, float falloff)
{
	light->setSpotlightRange(Ogre::Radian(innerAngleRad), Ogre::Radian(outerAngleRad), falloff);
}

extern "C" _AnomalousExport float Light_getSpotlightInnerAngle(Ogre::Light* light)
{
	return light->getSpotlightInnerAngle().valueRadians();
}

extern "C" _AnomalousExport float Light_getSpotlightOuterAngle(Ogre::Light* light)
{
	return light->getSpotlightOuterAngle().valueRadians();
}

extern "C" _AnomalousExport float Light_getSpotlightFalloff(Ogre::Light* light)
{
	return light->getSpotlightFalloff();
}

extern "C" _AnomalousExport void Light_setSpotlightInnerAngle(Ogre::Light* light, float innerAngleRad)
{
	light->setSpotlightInnerAngle(Ogre::Radian(innerAngleRad));
}

extern "C" _AnomalousExport void Light_setSpotlightOuterAngle(Ogre::Light* light, float outerAngleRad)
{
	light->setSpotlightOuterAngle(Ogre::Radian(outerAngleRad));
}

extern "C" _AnomalousExport void Light_setSpotlightFalloff(Ogre::Light* light, float value)
{
	light->setSpotlightFalloff(value);
}

extern "C" _AnomalousExport void Light_setPowerScale(Ogre::Light* light, float power)
{
	light->setPowerScale(power);
}

extern "C" _AnomalousExport float Light_getPowerScale(Ogre::Light* light)
{
	return light->getPowerScale();
}

extern "C" _AnomalousExport void Light_setCastShadows(Ogre::Light* light, bool cast)
{
	light->setCastShadows(cast);
}

extern "C" _AnomalousExport bool Light_getCastShadows(Ogre::Light* light)
{
	return light->getCastShadows();
}

extern "C" _AnomalousExport void Light_setAttenuationBasedOnRadius(Ogre::Light * light, float radius, float lumThreshold)
{
	light->setAttenuationBasedOnRadius(radius, lumThreshold);
}

#pragma warning(pop)