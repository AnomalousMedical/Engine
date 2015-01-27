#pragma once

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
#define HANDLE_ARG_OBJC andHandle:(void*) _instanceHandle 
#define PASS_HANDLE_ARG_OBJC andHandle:instanceHandle 
#define ASSIGN_HANDLE_OBJC instanceHandle = _instanceHandle;
#define CLEAR_HANDLE instanceHandle = NULL;
#else
#define HANDLE_INSTANCE 
#define HANDLE_FIRST_ARG
#define HANDLE_ARG 
#define ASSIGN_HANDLE 
#define ASSIGN_HANDLE_INITIALIZER 
#define PASS_HANDLE 
#define PASS_HANDLE_ARG 
#define HANDLE_ARG_OBJC 
#define PASS_HANDLE_ARG_OBJC 
#define ASSIGN_HANDLE_OBJC 
#define CLEAR_HANDLE 
#endif

//Typedef for common NativeAction
typedef void(*NativeAction)(HANDLE_FIRST_ARG);

//Typedef for StringRetriever callback function, these always have handles
typedef void(*StringRetrieverCallback)(const char* value, void* handle);

typedef void(*UnicodeStringRetrieverCallback)(const unsigned short* value, void* handle);