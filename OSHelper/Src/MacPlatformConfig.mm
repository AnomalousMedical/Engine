#include "StdAfx.h"
#import <Cocoa/Cocoa.h>

extern "C" _AnomalousExport void MacPlatformConfig_getLocalUserDocumentsFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    NSString* path = [NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES) lastObject];
    retrieve([path UTF8String], handle);
    [pool drain];
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    NSString* path = [NSSearchPathForDirectoriesInDomains(NSApplicationSupportDirectory, NSUserDomainMask, YES) lastObject];
    retrieve([path UTF8String], handle);
    [pool drain];
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalPrivateDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    NSString* path = [NSSearchPathForDirectoriesInDomains(NSApplicationSupportDirectory, NSUserDomainMask, YES) lastObject];
    retrieve([path UTF8String], handle);
    [pool drain];
}