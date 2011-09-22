using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public class ContactInfo
    {
        IntPtr contactInfo;
        ManifoldPoint manifoldPoint = new ManifoldPoint();

        public ContactInfo()
        {

        }

        internal void setInfo(IntPtr contactInfo)
        {
            this.contactInfo = contactInfo;
        }

        public int getNumContacts()
        {
            return ContactInfo_getNumContacts(contactInfo);
        }

        public void startPointIterator()
        {
            ContactInfo_startPointIterator(contactInfo);
        }

        public bool hasNextPoint()
        {
            return ContactInfo_hasNextPoint(contactInfo);
        }

        public ManifoldPoint nextPoint()
        {
            manifoldPoint.setPoint(ContactInfo_nextPoint(contactInfo));
            return manifoldPoint;
        }

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int ContactInfo_getNumContacts(IntPtr contactInfo);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ContactInfo_startPointIterator(IntPtr contactInfo);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ContactInfo_hasNextPoint(IntPtr contactInfo);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ContactInfo_nextPoint(IntPtr contactInfo);
    }
}
