#include "StdAfx.h"
#import <Foundation/Foundation.h>

extern "C" _AnomalousExport bool CertificateValidator_ValidateSSLCertificate(unsigned char* certBytes, unsigned int certBytesLength, String hostName)
{
    return true; //IOS_FIXLATER Need to actually implement, but need this to work for now.
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalUserDocumentsFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSURL* url = [[[NSFileManager defaultManager] URLsForDirectory:NSDocumentDirectory inDomains:NSUserDomainMask] lastObject];
    NSString* path = [url path];
    retrieve([path UTF8String], handle);
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSURL* url = [[[NSFileManager defaultManager] URLsForDirectory:NSApplicationSupportDirectory inDomains:NSUserDomainMask] lastObject];
    NSString* path = [url path];
    retrieve([path UTF8String], handle);
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalPrivateDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSURL* url = [[[NSFileManager defaultManager] URLsForDirectory:NSApplicationSupportDirectory inDomains:NSUserDomainMask] lastObject];
    NSString* path = [url path];
    retrieve([path UTF8String], handle);
}