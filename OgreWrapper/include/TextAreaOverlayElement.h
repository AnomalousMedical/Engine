#pragma once

#include "OverlayElement.h"

namespace Ogre
{
	class TextAreaOverlayElement;
}

namespace OgreWrapper{

value class Color;

/// <summary>
/// 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class TextAreaOverlayElement : public OverlayElement
{
public:
	enum class Alignment : unsigned int
	{
		Left,
		Right,
		Center
	};

private:
	Ogre::TextAreaOverlayElement* textArea;

internal:
	/// <summary>
	/// Returns the native TextAreaOverlayElement
	/// </summary>
	Ogre::TextAreaOverlayElement* getTextAreaOverlayElement();

	/// <summary>
	/// Constructor
	/// </summary>
	TextAreaOverlayElement(Ogre::TextAreaOverlayElement* textArea);

public:
	static property System::String^ TypeName
	{
		System::String^ get()
		{
			return "TextArea";
		}
	}

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~TextAreaOverlayElement();

	void setCharHeight(float height);

	float getCharHeight();

	void setSpaceWidth(float width);

	float getSpaceWidth();

	void setFontName(System::String^ font);

	System::String^ getFontName();

	void setColor(Color color);

	void setColor(Color% color);

	Color getColor();

	void setColorTop(Color color);

	void setColorTop(Color% color);

	Color getColorTop();

	void setColorBottom(Color color);

	void setColorBottom(Color% color);

	Color getColorBottom();

	void setAlignment(Alignment a);

	Alignment getAlignment();
};

}