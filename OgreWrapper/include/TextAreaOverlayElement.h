#pragma once

#include "OverlayElement.h"

namespace Ogre
{
	class TextAreaOverlayElement;
}

namespace OgreWrapper{

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

	void setColor(EngineMath::Color color);

	void setColor(EngineMath::Color% color);

	EngineMath::Color getColor();

	void setColorTop(EngineMath::Color color);

	void setColorTop(EngineMath::Color% color);

	EngineMath::Color getColorTop();

	void setColorBottom(EngineMath::Color color);

	void setColorBottom(EngineMath::Color% color);

	EngineMath::Color getColorBottom();

	void setAlignment(Alignment a);

	Alignment getAlignment();
};

}