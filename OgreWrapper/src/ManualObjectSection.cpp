/// <file>ManualObjectSection.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\ManualObjectSection.h"

#include "Ogre.h"
#include "MarshalUtils.h"

namespace OgreWrapper
{

ManualObjectSection::ManualObjectSection(Ogre::ManualObject::ManualObjectSection* section)
:section(section)
{

}

ManualObjectSection::~ManualObjectSection(void)
{

}

System::String^ ManualObjectSection::getMaterialName()
{
	return gcnew System::String(section->getMaterialName().c_str());
}

void ManualObjectSection::setMaterialName(System::String^ name)
{
	section->setMaterialName(MarshalUtils::convertString(name));
}

void ManualObjectSection::set32BitIndices(bool n32)
{
	section->set32BitIndices(n32);
}

bool ManualObjectSection::get32BitIndices()
{
	return section->get32BitIndices();
}

}