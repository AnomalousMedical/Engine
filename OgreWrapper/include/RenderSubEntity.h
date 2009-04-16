#pragma once

namespace Ogre
{
	class SubEntity;
}

namespace Rendering{

ref class RenderMaterialPtr;

[Engine::Attributes::DoNotSaveAttribute]
public ref class RenderSubEntity
{
private:
	Ogre::SubEntity* subEntity;

internal:
	RenderSubEntity(Ogre::SubEntity* subEntity);

public:
	virtual ~RenderSubEntity(void);

	System::String^ getMaterialName();

	void setMaterialName(System::String^ name);

	void setVisible(bool visible);

	bool isVisible();
	
	RenderMaterialPtr^ getMaterial();
};

}