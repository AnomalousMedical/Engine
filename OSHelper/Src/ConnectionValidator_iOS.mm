#include "StdAfx.h"
#import <Foundation/Foundation.h>

extern "C" _AnomalousExport bool CertificateValidator_ValidateSSLCertificate(unsigned char* certBytes, unsigned int certBytesLength, String hostName)
{
    return true;
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalUserDocumentsFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSString* path = @"Documents/";
    retrieve([path UTF8String], handle);
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSString* path = @"Library/Application Support";
    retrieve([path UTF8String], handle);
}

extern "C" _AnomalousExport void MacPlatformConfig_getLocalPrivateDataFolder(StringRetrieverCallback retrieve, void* handle)
{
    NSString* path = @"Library/Application Support";
    retrieve([path UTF8String], handle);
}