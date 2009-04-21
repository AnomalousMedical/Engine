//Header
#pragma once

namespace OgreWrapper
{

ref class Viewport;
ref class ViewportCollection : public WrapperCollection<Viewport^>
{
protected:
	virtual Viewport^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~ViewportCollection() {}

	Viewport^ getObject(Ogre::Viewport* nativeObject);

	void destroyObject(Ogre::Viewport* nativeObject);
};

}