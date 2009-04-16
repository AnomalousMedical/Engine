//Source
#include "stdafx.h"
#include "RenderTargetCollection.h"
#include "RenderTarget.h"
#include "RenderWindow.h"

namespace Rendering{

RenderTarget^ RenderTargetCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	RenderTargetType type = static_cast<RenderTargetType>(args[0]);
	switch(type)
	{
	case RenderTargetType::RenderWindow:
		return gcnew RenderWindow(static_cast<Ogre::RenderWindow*>(nativeObject));
		break;
	case RenderTargetType::RenderTexture:
		throw gcnew System::NotImplementedException();
		break;
	case RenderTargetType::MultiRenderTarget:
		throw gcnew System::NotImplementedException();
		break;
	default:
		throw gcnew System::NotImplementedException();
		break;
	}
}

RenderWindow^ RenderTargetCollection::getObject(Ogre::RenderWindow* nativeObject)
{
	return static_cast<RenderWindow^>(getObjectVoid(nativeObject, RenderTargetType::RenderWindow));
}

RenderTarget^ RenderTargetCollection::getExistingObject(Ogre::RenderTarget* nativeObject)
{
	RenderTarget^ ret = nullptr;
	getObjectVoidNoCreate(nativeObject, ret);
	return ret;
}

void RenderTargetCollection::destroyObject(Ogre::RenderTarget* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}