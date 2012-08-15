#include "StdAfx.h"
#include "ElementListIter.h"

ElementListIter::ElementListIter()
{

}

ElementListIter::~ElementListIter()
{

}

void ElementListIter::startIterator()
{
	elementListIter = elementList.begin();
	elementListIterEnd = elementList.end();
}

Rocket::Core::Element* ElementListIter::getNextElement()
{
	Rocket::Core::Element* element = NULL;
	if(elementListIter != elementListIterEnd)
	{
		element = *elementListIter;
		++elementListIter;
	}
	return element;
}

extern "C" _AnomalousExport ElementListIter* ElementListIter_Create()
{
	return new ElementListIter();
}

extern "C" _AnomalousExport void ElementListIter_Delete(ElementListIter* elementListIter)
{
	delete elementListIter;
}

extern "C" _AnomalousExport void ElementListIter_startIterator(ElementListIter* elementListIter)
{
	elementListIter->startIterator();
}

extern "C" _AnomalousExport Rocket::Core::Element* ElementListIter_getNextElement(ElementListIter* elementListIter)
{
	return elementListIter->getNextElement();
}