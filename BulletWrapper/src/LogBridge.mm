//
//  LogBridge.m
//  BulletWrapper
//
//  Created by Andrew Piper on 5/6/15.
//  Copyright (c) 2015 Andrew Piper. All rights reserved.
//

#include "StdAfx.h"
#import <Foundation/Foundation.h>

void LogMessage(const std::string& message)
{
    NSString *string = [[NSString alloc] initWithUTF8String:message.c_str()];
    NSLog(@"%@\n", string);
}

void LogTransform(const std::string& info, const btTransform& transform)
{
    std::stringstream ss;
    ss << info << " Transform basis is\n"
    << transform.getBasis()[0][0] << ", " << transform.getBasis()[0][1] << ", " << transform.getBasis()[0][2] << "\n"
    << transform.getBasis()[1][0] << ", " << transform.getBasis()[1][1] << ", " << transform.getBasis()[1][2] << "\n"
    << transform.getBasis()[2][0] << ", " << transform.getBasis()[2][1] << ", " << transform.getBasis()[2][2] << "\n"
    << "origin is\n"
    << transform.getOrigin()[0] << ", " << transform.getOrigin()[1] << ", " << transform.getOrigin()[2] << ", ";
    LogMessage(ss.str());
}