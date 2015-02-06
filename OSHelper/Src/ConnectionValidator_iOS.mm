#include "StdAfx.h"
#import <Foundation/Foundation.h>

OSStatus EvaluateCert (SecCertificateRef cert, CFTypeRef policyRef, SecTrustResultType *result, SecTrustRef *pTrustRef)
{
    OSStatus status1;
    OSStatus status2;
    
    const void* evalCertArray[] = { cert };
    CFArrayRef cfCertRef = CFArrayCreate(kCFAllocatorDefault, evalCertArray, 1, NULL);
    
//    if (!cfCertRef)
//        return memFullErr;
    
    status1 = SecTrustCreateWithCertificates(cfCertRef, policyRef, pTrustRef);
    if (status1)
    {
        return status1;
    }
    
    status2 = SecTrustEvaluate (*pTrustRef, result);
    
    // Release the objects we allocated
    if (cfCertRef)
    {
        CFRelease(cfCertRef);
    }
    //if (cfDate)
    //    CFRelease(cfDate);
    
    return (status2);
}

extern "C" _AnomalousExport bool CertificateValidator_ValidateSSLCertificate(unsigned char* certBytes, unsigned int certBytesLength, String hostName)
{
    //NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    
    SecTrustRef trustRef = nil;
    SecTrustResultType result = kSecTrustResultInvalid;
    bool success = false;
    
    CFStringRef cfHostName = NULL;
    if(hostName != NULL)
    {
        cfHostName = CFStringCreateWithFormat(kCFAllocatorDefault, NULL, CFSTR("%S"), hostName);
        if(cfHostName == NULL)
        {
            return false; //Fail if a host is provided, but cannot be translated. This prevents us trusting something we shouldn't if there is an error here.
        }
    }
    
    NSData *certData = [NSData dataWithBytes:certBytes length:certBytesLength];
    SecCertificateRef cert = SecCertificateCreateWithData(NULL, (__bridge CFDataRef)certData);
    if(cert != nil)
    {
        SecPolicyRef policyRef = SecPolicyCreateSSL(true, cfHostName);
        EvaluateCert(cert, policyRef, &result, &trustRef);
        switch(result)
        {
            case kSecTrustResultProceed:
            case kSecTrustResultUnspecified:
                success = true;
        }
        
        if(trustRef != nil)
        {
            CFRelease(trustRef);
        }
        
        CFRelease(policyRef);
    }
    
    if(cfHostName != NULL)
    {
        CFRelease(cfHostName);
    }
    
    CFRelease(cert);
    
    //[pool drain];
    
    return success;
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