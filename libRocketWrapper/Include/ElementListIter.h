#pragma once

class ElementListIter
{
public:
	ElementListIter();

	virtual ~ElementListIter();

	void startIterator();

	Rocket::Core::Element* getNextElement();

	Rocket::Core::ElementList elementList;

private:
	Rocket::Core::ElementList::iterator elementListIter;
	Rocket::Core::ElementList::iterator elementListIterEnd;
};