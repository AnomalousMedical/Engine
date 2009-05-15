//Header
#pragma once

namespace OgreWrapper
{

ref class TextureUnitState;
ref class TextureUnitStateCollection : public WrapperCollection<TextureUnitState^>
{
protected:
	virtual TextureUnitState^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~TextureUnitStateCollection() {}

	TextureUnitState^ getObject(Ogre::TextureUnitState* nativeObject);

	void destroyObject(Ogre::TextureUnitState* nativeObject);
};

}