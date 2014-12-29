﻿using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Medical
{
    public static class MacOSXFunctions
    {
        private static object sslTrustLock = new object();

        public static unsafe bool TrustSSLCertificate(X509Certificate certificate, string hostName)
        {
            unsafe
            {
                //Apple says that the functions used on the native side to check validity are potentially not thread safe, so lock here when checking.
                lock (sslTrustLock)
                {
                    byte[] certBytes = certificate.Export(X509ContentType.Cert);
                    fixed (byte* certBytesPtr = &certBytes[0])
                    {
                        return CertificateValidator_ValidateSSLCertificate(certBytesPtr, (uint)certBytes.Length, hostName);
                    }
                }
            }
        }

        public static String LocalUserDocumentsFolder
        {
            get
            {
                StringRetriever sr = new StringRetriever();
                MacPlatformConfig_getLocalUserDocumentsFolder(sr.StringCallback);
                return sr.retrieveString();
            }
        }

        public static String LocalDataFolder
        {
            get
            {
                StringRetriever sr = new StringRetriever();
                MacPlatformConfig_getLocalDataFolder(sr.StringCallback);
                return sr.retrieveString();
            }
        }

        public static String LocalPrivateDataFolder
        {
            get
            {
                StringRetriever sr = new StringRetriever();
                MacPlatformConfig_getLocalPrivateDataFolder(sr.StringCallback);
                return sr.retrieveString();
            }
        }

        #region PInvoke

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static unsafe extern bool CertificateValidator_ValidateSSLCertificate(byte* certBytes, uint certBytesLength, [MarshalAs(UnmanagedType.LPWStr)] String url);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern void MacPlatformConfig_getLocalUserDocumentsFolder(StringRetriever.Callback retrieve);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern void MacPlatformConfig_getLocalDataFolder(StringRetriever.Callback retrieve);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern void MacPlatformConfig_getLocalPrivateDataFolder(StringRetriever.Callback retrieve);

        #endregion
    }
}
