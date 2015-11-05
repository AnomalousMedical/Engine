#include "StdAfx.h"
#import <Foundation/Foundation.h>

extern "C" _AnomalousExport void iOSPlatformConfig_getLocalUserDocumentsFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSURL* url = [[[NSFileManager defaultManager] URLsForDirectory:NSDocumentDirectory inDomains:NSUserDomainMask] lastObject];
    NSString* path = [url path];
    retrieve([path UTF8String], handle);
}

extern "C" _AnomalousExport void iOSPlatformConfig_getLocalDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSURL* url = [[[NSFileManager defaultManager] URLsForDirectory:NSApplicationSupportDirectory inDomains:NSUserDomainMask] lastObject];
    NSString* path = [url path];
    retrieve([path UTF8String], handle);
}

extern "C" _AnomalousExport void iOSPlatformConfig_getLocalPrivateDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSURL* url = [[[NSFileManager defaultManager] URLsForDirectory:NSApplicationSupportDirectory inDomains:NSUserDomainMask] lastObject];
    NSString* path = [url path];
    retrieve([path UTF8String], handle);
}