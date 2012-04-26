#include "StdAfx.h"

enum ElementType
{
    Element = 0,
    ElementDocument = 1,
};

class ElementManager : public Rocket::Core::Plugin
{
public:
	typedef void (*ElementDestructorCallback)(Rocket::Core::Element* element);

	ElementManager(ElementDestructorCallback elementDestroyed)
		:elementDestroyed(elementDestroyed)
	{

	}

	virtual ~ElementManager()
	{

	}

	virtual void OnShutdown()
	{
		delete this;
	}

	virtual void OnElementDestroy(Rocket::Core::Element* element)
	{
		elementDestroyed(element);
	}

private:
	ElementDestructorCallback elementDestroyed;
};

extern "C" _AnomalousExport ElementManager* ElementManager_create(ElementManager::ElementDestructorCallback elementDestroyed)
{
	ElementManager* elementManager = new ElementManager(elementDestroyed);
	Rocket::Core::RegisterPlugin(elementManager);
	return elementManager;
}

extern "C" _AnomalousExport ElementType ElementManager_getType(Rocket::Core::Element* element)
{
	//typeid static variables
	static const type_info &ELEMENT_DOCUMENT = typeid(Rocket::Core::ElementDocument);

	//Test
	const type_info &typeInfo = typeid(*element);
	if(typeInfo == ELEMENT_DOCUMENT)
	{
		return ElementDocument;
	}
	return Element;
}