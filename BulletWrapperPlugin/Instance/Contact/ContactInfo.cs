using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletPlugin
{
    public class ContactInfo
    {
        public RigidBody RigidBodyA { get; set; }

        public RigidBody RigidBodyB { get; set; }

        public int getNumContacts()
        {
            throw new NotImplementedException();
        }

        public void getContactPoint(int index, ManifoldPoint point)
        {
            throw new NotImplementedException();
        }
    }
}
