#pragma once

#include "App.h"

class AndroidApp : public App
{
public:
	AndroidApp();
    
	virtual ~AndroidApp();
    
    virtual void run();
    
    virtual void exit();
};