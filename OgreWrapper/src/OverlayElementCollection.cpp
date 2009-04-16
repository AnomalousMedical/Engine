//Source
#include "stdafx.h"
#include "OverlayElementCollection.h"
#include "OverlayElement.h"
#include "MarshalUtils.h"
#include "TextAreaOverlayElement.h"
#include "PanelOverlayElement.h"
#include "BorderPanelOverlayElement.h"

namespace Rendering{

OverlayElement^ OverlayElementCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	Ogre::OverlayElement* ogreElement = static_cast<Ogre::OverlayElement*>(nativeObject);
	System::String^ type = MarshalUtils::convertString(ogreElement->getTypeName());
	if(type->Equals(TextAreaOverlayElement::TypeName))
	{
		return gcnew TextAreaOverlayElement(static_cast<Ogre::TextAreaOverlayElement*>(nativeObject));
	}
	else if(type->Equals(PanelOverlayElement::TypeName))
	{
		return gcnew PanelOverlayElement(static_cast<Ogre::PanelOverlayElement*>(nativeObject));
	}
	else if(type->Equals(BorderPanelOverlayElement::TypeName))
	{
		return gcnew BorderPanelOverlayElement(static_cast<Ogre::BorderPanelOverlayElement*>(nativeObject));
	}
	throw gcnew System::NotImplementedException();
}

OverlayElement^ OverlayElementCollection::getObject(Ogre::OverlayElement* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void OverlayElementCollection::destroyObject(Ogre::OverlayElement* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}