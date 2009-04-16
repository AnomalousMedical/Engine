#pragma once

#include "Enums.h"
#include "AutoPtr.h"
#include "RenderResource.h"
#include "TechniqueCollection.h"

namespace Ogre
{
	class Ogre;
}

namespace Rendering
{

ref class Technique;
value class Color;
ref class RenderMaterialPtr;

[Engine::Attributes::DoNotSaveAttribute]
public ref class RenderMaterial : public RenderResource
{
private:
	AutoPtr<Ogre::MaterialPtr> autoMaterialPtr;
	Ogre::Material* ogreMaterial;

	TechniqueCollection techniques;

internal:
	RenderMaterial(const Ogre::MaterialPtr& ogreMaterial);

	Ogre::Material* getOgreMaterial()
	{
		return ogreMaterial;
	}

public:
	virtual ~RenderMaterial(void);

	bool isTransparent();

	void setReceiveShadows(bool enabled);

	bool getReceiveShadows();

	void setTransparencyCastsShadows(bool enabled);

	bool getTransparencyCastsShadows();

	Technique^ createTechnique();

	Technique^ getTechnique(unsigned short index);

	Technique^ getTechnique(System::String^ name);

	unsigned short getNumTechniques();

	void removeTechnique(unsigned short index);

	void removeAllTechniques();

	Technique^ getSupportedTechnique(unsigned short index);

	unsigned short getNumSupportedTechniques();

	System::String^ getUnsupportedTechniquesExplanation();

	unsigned short getNumLodLevels(unsigned short schemeIndex);

	unsigned short getNumLodLevels(System::String^ schemeName);

	Technique^ getBestTechnique();

	Technique^ getBestTechnique(unsigned short lodIndex);

	RenderMaterialPtr^ clone(System::String^ newName);

	RenderMaterialPtr^ clone(System::String^ newName, bool changeGroup, System::String^ newGroup);

	void copyDetailsTo(RenderMaterial^ material);

	void compile();

	void compile(bool autoManageTextureUnits);

	void setPointSize(float ps);

	void setAmbient(float red, float green, float blue);

	void setAmbient(Color color);

	void setAmbient(Color% color);

	void setDiffuse(float red, float green, float blue, float alpha);

	void setDiffuse(Color color);

	void setDiffuse(Color% color);

	void setSpecular(float red, float green, float blue, float alpha);

	void setSpecular(Color color);

	void setSpecular(Color% color);

	void setShininess(float value);

	void setSelfIllumination(float red, float green, float blue);

	void setSelfIllumination(Color color);

	void setSelfIllumination(Color% color);

	void setDepthCheckEnabled(bool enabled);

	void setDepthWriteEnabled(bool enabled);

	void setDepthFunction(CompareFunction func);

	void setColorWriteEnabled(bool enabled);

	void setCullingMode(CullingMode mode);

	void setManualCullingMode(ManualCullingMode mode);

	void setLightingEnabled(bool enabled);

	void setShadingMode(ShadeOptions mode);

	void setFog(bool overrideScene, FogMode mode, Color% color);

	void setFog(bool overrideScene, FogMode mode, Color% color, float expDensity, float linearStart, float linearEnd);

	void setDepthBias(float constantBias, float slopeScaleBias);

	void setTextureFiltering(TextureFilterOptions filterType);

	void setTextureAnisotropy(int maxAniso);

	void setSceneBlending(SceneBlendType sbt);

	void setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta);

	void setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor);

	void setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha);
};

}