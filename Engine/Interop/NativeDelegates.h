﻿#pragma once

#if defined(APPLE_IOS)
#define FULL_AOT_COMPILE 1
#endif

#if FULL_AOT_COMPILE
#define HANDLE_INSTANCE void* instanceHandle;
#define HANDLE_FIRST_ARG void* instanceHandle
#define HANDLE_ARG , void* instanceHandle
#define ASSIGN_HANDLE this->instanceHandle = instanceHandle;
#define ASSIGN_HANDLE_INITIALIZER , instanceHandle(instanceHandle)
#define PASS_HANDLE instanceHandle
#define PASS_HANDLE_ARG , instanceHandle
#else
#define HANDLE_INSTANCE 
#define HANDLE_FIRST_ARG
#define HANDLE_ARG 
#define ASSIGN_HANDLE 
#define ASSIGN_HANDLE_INITIALIZER 
#define PASS_HANDLE 
#define PASS_HANDLE_ARG 
#endif

//NativeAction - functions that take values and return void

typedef void(*NativeAction)(HANDLE_FIRST_ARG);

typedef void(*NativeAction_Bool)(bool arg0 HANDLE_ARG);

//NativeAction no handles - these do not have instanceHandles passed no matter what mode.

typedef void(*NativeAction_String_NoHandle)(String fullMessage);