﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// Used by the TypeFinder to load custom ObjectIdentifiers if needed. This currently applies only to the XmlSaver because
    /// it loads files that may contain old classes. Any other savers that could deal with legacy classes should use this factory
    /// to create ObjectIdentifiers when loading.
    /// </summary>
    public static class ObjectIdentifierFactory
    {
        public delegate ObjectIdentifier CreateObjectIdentifierDelegate(long id, String assemblyQualifiedName, TypeFinder typeFinder);

        private static Dictionary<String, CreateObjectIdentifierDelegate> creationMethods = new Dictionary<String, CreateObjectIdentifierDelegate>();

        public static ObjectIdentifier CreateObjectIdentifier(long id, String assemblyQualifiedName, TypeFinder typeFinder)
        {
            CreateObjectIdentifierDelegate create;
            if (creationMethods.TryGetValue(assemblyQualifiedName, out create))
            {
                return create(id, assemblyQualifiedName, typeFinder);
            }
            Type type = typeFinder.findType(assemblyQualifiedName);
            return new ObjectIdentifier(id, null, type);
        }

        public static void AddCreationMethod(String assemblyQualifiedTypeName, CreateObjectIdentifierDelegate create)
        {
            creationMethods.Add(assemblyQualifiedTypeName, create);
        }

        public static void AddCreationMethod(String assemblyQualifiedTypeName)
        {
            creationMethods.Remove(assemblyQualifiedTypeName);
        }
    }
}
