/*
 *  libRocketWrapper.cp
 *  libRocketWrapper
 *
 *  Created by Andrew Piper on 4/26/12.
 *  Copyright 2012 Anomalous Medical. All rights reserved.
 *
 */

#include <iostream>
#include "libRocketWrapper.h"
#include "libRocketWrapperPriv.h"

void libRocketWrapper::HelloWorld(const char * s)
{
	 libRocketWrapperPriv *theObj = new libRocketWrapperPriv;
	 theObj->HelloWorldPriv(s);
	 delete theObj;
};

void libRocketWrapperPriv::HelloWorldPriv(const char * s) 
{
	std::cout << s << std::endl;
};

