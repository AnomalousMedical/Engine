#pragma once

namespace Ogre
{
	class SubEntity;
}

namespace OgreWrapper{

ref class RenderMaterialPtr;

[Engine::Attributes::DoNotSaveAttribute]
public ref class SubEntity
{
private:
	Ogre::SubEntity* subEntity;

internal:
	SubEntity(Ogre::SubEntity* subEntity);

public:
	virtual ~SubEntity(void);

	System::String^ getMaterialName();

	void setMaterialName(System::String^ name);

	void setVisible(bool visible);

	bool isVisible();
	
	RenderMaterialPtr^ getMaterial();
};

}