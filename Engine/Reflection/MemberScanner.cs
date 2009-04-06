using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Engine.Reflection
{
    /// <summary>
    /// This interface will scan a given type and return the list of fields and
    /// properties as configured.
    /// </summary>
    public class MemberScanner
    {
        #region Constructors

        /// <summary>
        /// Default constructor. Will process both fields and properties, public
        /// and non public up to object.
        /// </summary>
        public MemberScanner()
        {
            ProcessFields = true;
            ProcessProperties = true;
            TerminatingType = typeof(Object);
            ProcessNonPublicFields = true;
            ProcessPublicFields = true;
            ProcessNonPublicProperties = true;
            ProcessPublicProperties = true;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Scan the type for all appropriate fields. What fields are returned
        /// is determined by the implementing class. This can also return null
        /// if this MemberScanner does not process fields.
        /// </summary>
        /// <param name="type">The type to scan for fields.</param>
        /// <returns>A list of all fields found or null if this scanner does not process fields.</returns>
        public List<FieldInfo> getFields(Type type)
        {
            if (ProcessFields)
            {
                BindingFlags searchFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly;
                if (ProcessNonPublicFields)
                {
                    searchFlags |= BindingFlags.NonPublic;
                }
                if (ProcessPublicFields)
                {
                    searchFlags |= BindingFlags.Public;
                }
                List<FieldInfo> fields = new List<FieldInfo>();
                while (type != TerminatingType)
                {
                    FieldInfo[] levelFields = type.GetFields(searchFlags);
                    fields.AddRange(levelFields);
                    type = type.BaseType;
                }
                return fields;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Scan the type for all appropriate properties. What properties are
        /// returned is determined by the implementing class. This can also
        /// return null if this MemberScanner does not process properties.
        /// </summary>
        /// <param name="type">The type to scan for properties.</param>
        /// <returns>A list of all properties found or null if this scanner does not process properties.</returns>
        public List<PropertyInfo> getProperties(Type type)
        {
            if (ProcessProperties)
            {
                BindingFlags searchFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly;
                if (ProcessNonPublicProperties)
                {
                    searchFlags |= BindingFlags.NonPublic;
                }
                if (ProcessPublicProperties)
                {
                    searchFlags |= BindingFlags.Public;
                }
                List<PropertyInfo> properties = new List<PropertyInfo>();
                while (type != TerminatingType)
                {
                    PropertyInfo[] levelProperties = type.GetProperties(searchFlags);
                    properties.AddRange(levelProperties);
                    type = type.BaseType;
                }
                return properties;
            }
            else
            {
                return null;
            }
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// This is the type that will terminate the up hierarchy scan of the
        /// given type. The default is Object, which will cause all types up the
        /// inheretance chain to be scanned. Note that this type is not included
        /// in the scan because the scan is stopped when this type is
        /// encountered.
        /// </summary>
        public Type TerminatingType { get; set; }

        /// <summary>
        /// This should be true if this MemberScanner will process fields.
        /// </summary>
        public bool ProcessFields { get; set; }

        /// <summary>
        /// This should be true if this MemberScanner will process properties.
        /// </summary>
        public bool ProcessProperties { get; set; }

        /// <summary>
        /// This should be true if the MemberScanner will process non public fields.
        /// </summary>
        public bool ProcessNonPublicFields { get; set; }

        /// <summary>
        /// This should be true if the MemberScanner will process public fields.
        /// </summary>
        public bool ProcessPublicFields { get; set; }

        /// <summary>
        /// This should be true if the MemberScanner will process non public properties.
        /// </summary>
        public bool ProcessNonPublicProperties { get; set; }

        /// <summary>
        /// This should be true if the MemberScanner will process public properties.
        /// </summary>
        public bool ProcessPublicProperties { get; set; }

        #endregion Properties
    }
}
