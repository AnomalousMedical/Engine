#include "Stdafx.h"
#include "OgreTextAreaOverlayElement.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" __declspec(dllexport) void TextAreaOverlayElement_setCharHeight(Ogre::TextAreaOverlayElement* textArea, float height)
{
	textArea->setCharHeight(height);
}

extern "C" __declspec(dllexport) float TextAreaOverlayElement_getCharHeight(Ogre::TextAreaOverlayElement* textArea)
{
	return textArea->getCharHeight();
}

extern "C" __declspec(dllexport) void TextAreaOverlayElement_setSpaceWidth(Ogre::TextAreaOverlayElement* textArea, float width)
{
	textArea->setSpaceWidth(width);
}

extern "C" __declspec(dllexport) float TextAreaOverlayElement_getSpaceWidth(Ogre::TextAreaOverlayElement* textArea)
{
	return textArea->getSpaceWidth();
}

extern "C" __declspec(dllexport) void TextAreaOverlayElement_setFontName(Ogre::TextAreaOverlayElement* textArea, String font)
{
	textArea->setFontName(font);
}

extern "C" __declspec(dllexport) String TextAreaOverlayElement_getFontName(Ogre::TextAreaOverlayElement* textArea)
{
	return textArea->getFontName().c_str();
}

extern "C" __declspec(dllexport) void TextAreaOverlayElement_setColor(Ogre::TextAreaOverlayElement* textArea, Color color)
{
	textArea->setColour(color.toOgre());
}

extern "C" __declspec(dllexport) Color TextAreaOverlayElement_getColor(Ogre::TextAreaOverlayElement* textArea)
{
	return textArea->getColour();
}

extern "C" __declspec(dllexport) void TextAreaOverlayElement_setColorTop(Ogre::TextAreaOverlayElement* textArea, Color color)
{
	textArea->setColourTop(color.toOgre());
}

extern "C" __declspec(dllexport) Color TextAreaOverlayElement_getColorTop(Ogre::TextAreaOverlayElement* textArea)
{
	return textArea->getColourTop();
}

extern "C" __declspec(dllexport) void TextAreaOverlayElement_setColorBottom(Ogre::TextAreaOverlayElement* textArea, Color color)
{
	textArea->setColourBottom(color.toOgre());
}

extern "C" __declspec(dllexport) Color TextAreaOverlayElement_getColorBottom(Ogre::TextAreaOverlayElement* textArea)
{
	return textArea->getColourBottom();
}

extern "C" __declspec(dllexport) void TextAreaOverlayElement_setAlignment(Ogre::TextAreaOverlayElement* textArea, Ogre::TextAreaOverlayElement::Alignment a)
{
	textArea->setAlignment(a);
}

extern "C" __declspec(dllexport) Ogre::TextAreaOverlayElement::Alignment TextAreaOverlayElement_getAlignment(Ogre::TextAreaOverlayElement* textArea)
{
	return textArea->getAlignment();
}

#pragma warning(pop)