#include "StdAfx.h"
#include "..\include\TextAreaOverlayElement.h"
#include "MarshalUtils.h"
#include "MathUtils.h"
#include "Color.h"

#pragma warning(push)
#pragma warning(disable : 4251) //Disable dll interface warning
#pragma warning(disable : 4635) //Disable xml comment warning
#include "OgreTextAreaOverlayElement.h"
#pragma warning(pop)

namespace Engine
{

namespace Rendering
{

TextAreaOverlayElement::TextAreaOverlayElement(Ogre::TextAreaOverlayElement* textArea)
:OverlayElement(textArea),
textArea( textArea )
{

}

TextAreaOverlayElement::~TextAreaOverlayElement()
{
	textArea = 0;
}

Ogre::TextAreaOverlayElement* TextAreaOverlayElement::getTextAreaOverlayElement()
{
	return textArea;
}

void TextAreaOverlayElement::setCharHeight(float height)
{
	return textArea->setCharHeight(height);
}

float TextAreaOverlayElement::getCharHeight()
{
	return textArea->getCharHeight();
}

void TextAreaOverlayElement::setSpaceWidth(float width)
{
	return textArea->setSpaceWidth(width);
}

float TextAreaOverlayElement::getSpaceWidth()
{
	return textArea->getSpaceWidth();
}

void TextAreaOverlayElement::setFontName(System::String^ font)
{
	return textArea->setFontName(MarshalUtils::convertString(font));
}

System::String^ TextAreaOverlayElement::getFontName()
{
	return MarshalUtils::convertString(textArea->getFontName());
}

void TextAreaOverlayElement::setColor(Color color)
{
	return textArea->setColour(MathUtils::copyColor(color));
}

void TextAreaOverlayElement::setColor(Color% color)
{
	return textArea->setColour(MathUtils::copyColor(color));
}

Color TextAreaOverlayElement::getColor()
{
	return MathUtils::copyColor(textArea->getColour());
}

void TextAreaOverlayElement::setColorTop(Color color)
{
	return textArea->setColourTop(MathUtils::copyColor(color));
}

void TextAreaOverlayElement::setColorTop(Color% color)
{
	return textArea->setColourTop(MathUtils::copyColor(color));
}

Color TextAreaOverlayElement::getColorTop()
{
	return MathUtils::copyColor(textArea->getColourTop());
}

void TextAreaOverlayElement::setColorBottom(Color color)
{
	return textArea->setColourBottom(MathUtils::copyColor(color));
}

void TextAreaOverlayElement::setColorBottom(Color% color)
{
	return textArea->setColourBottom(MathUtils::copyColor(color));
}

Color TextAreaOverlayElement::getColorBottom()
{
	return MathUtils::copyColor(textArea->getColourBottom());
}

void TextAreaOverlayElement::setAlignment(Alignment a)
{
	return textArea->setAlignment(static_cast<Ogre::TextAreaOverlayElement::Alignment>(a));
}

TextAreaOverlayElement::Alignment TextAreaOverlayElement::getAlignment()
{
	return static_cast<Alignment>(textArea->getAlignment());
}

}

}