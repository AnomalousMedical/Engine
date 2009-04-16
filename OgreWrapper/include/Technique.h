#pragma once

#include "Enums.h"
#include "PassCollection.h"

namespace Ogre
{
	class Technique;
}

namespace OgreWrapper{

ref class Pass;
ref class Material;
value class Color;
ref class Pass;

[Engine::Attributes::DoNotSaveAttribute]
public ref class Technique
{
private:
	Ogre::Technique* ogreTechnique;
	Material^ parent;

	PassCollection passes;

internal:
	Technique(Ogre::Technique* ogreTechnique, Material^ parent);

public:
	virtual ~Technique(void);

	Pass^ createPass();

	Pass^ getPass(unsigned short index);

	Pass^ getPass(System::String^ name);

	unsigned short getNumPasses();

	void removePass(unsigned short index);

	void removeAllPasses();

	bool movePass(unsigned short sourceIndex, unsigned short destinationIndex);

	Material^ getParent();

	System::String^ getResourceGroup();

	bool isTransparent();

	bool isTransparentSortingEnabled();

	Material^ getShadowCasterMaterial();

	void setShadowCasterMaterial(Material^ material);

	void setShadowCasterMaterial(System::String^ name);

	Material^ getShadowReceiverMaterial();

	void setShadowReceiverMaterial(Material^ material);

	void ShadowReceiverMaterial(System::String^ name);

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

	void setLodIndex(unsigned short index);

	unsigned short getLodIndex();

	void setSchemeName(System::String^ schemeName);

	System::String^ getSchemeName();

	bool isDepthWriteEnabled();

	bool isDepthCheckEnabled();

	bool hasColorWriteDisabled();

	void setName(System::String^ name);

	System::String^ getName();
};

}