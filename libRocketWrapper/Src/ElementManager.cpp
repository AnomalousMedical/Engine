#include "StdAfx.h"

enum ElementType
{
    Element = 0,
    ElementDocument = 1,
	ElementFormControl = 2,
};

class ElementManager : public Rocket::Core::Plugin
{
public:
	typedef void (*ElementDestructorCallback)(Rocket::Core::Element* element);
	typedef void (*ContextDestructorCallback)(Rocket::Core::Context* context);

	ElementManager(ElementDestructorCallback elementDestroyed, ContextDestructorCallback contextDestroyed)
		:elementDestroyed(elementDestroyed),
		contextDestroyed(contextDestroyed)
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

	virtual void OnContextDestroy (Rocket::Core::Context *context)
	{
		contextDestroyed(context);
	}

private:
	ElementDestructorCallback elementDestroyed;
	ContextDestructorCallback contextDestroyed;
};

extern "C" _AnomalousExport ElementManager* ElementManager_create(ElementManager::ElementDestructorCallback elementDestroyed, ElementManager::ContextDestructorCallback contextDestroyed)
{
	ElementManager* elementManager = new ElementManager(elementDestroyed, contextDestroyed);
	Rocket::Core::RegisterPlugin(elementManager);
	return elementManager;
}

extern "C" _AnomalousExport ElementType ElementManager_getType(Rocket::Core::Element* element)
{	
	if(typeid(*element) == typeid(Rocket::Core::ElementDocument))
	{
		return ElementDocument;
	}
	else if(typeid(*element) == typeid(Rocket::Controls::ElementFormControlSelect))
	{
		return ElementFormControl;
	}
	return Element;
}