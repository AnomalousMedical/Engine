using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Engine.Reflection
{
    public class FilteredMemberScanner : MemberScanner
    {
        /// <summary>
        /// Default constructor. Will process both fields and properties, public
        /// and non public up to object.
        /// </summary>
        public FilteredMemberScanner()
            :this(null)
        {
            
        }

        /// <summary>
        /// Constructor. Will process both fields and properties, public and non
        /// public up to object.
        /// </summary>
        /// <param name="filter">The filter to use on scanned objects.</param>
        public FilteredMemberScanner(MemberScannerFilter filter)
        {
            ProcessFields = true;
            ProcessProperties = true;
            ProcessNonPublicFields = true;
            ProcessPublicFields = true;
            ProcessNonPublicProperties = true;
            ProcessPublicProperties = true;
            this.Filter = filter;
        }

        /// <summary>
        /// Scan the type for all appropriate members. What members are returned
        /// is determined by the implementing class. This can also return null
        /// if this MemberScanner does not process members.
        /// </summary>
        /// <param name="type">The type to scan for members.</param>
        /// <returns>A list of all members found.</returns>
        public IEnumerable<MemberWrapper> getMatchingMembers(Type type)
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
                Type searchType = type;
                while ((Filter == null || Filter.allowType(searchType)) && searchType.BaseType() != null)
                {
                    foreach (FieldInfo levelField in searchType.GetFields(searchFlags))
                    {
                        MemberWrapper fieldWrapper = new FieldMemberWrapper(levelField);
                        if (Filter == null || Filter.allowMember(fieldWrapper))
                        {
                            yield return fieldWrapper;
                        }
                    }
                    searchType = searchType.BaseType();
                }
            }
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
                Type searchType = type;
                while ((Filter == null || Filter.allowType(searchType)) && searchType.BaseType() != null)
                {
                    foreach (PropertyInfo levelProp in searchType.GetProperties(searchFlags))
                    {
                        MemberWrapper propWrapper = new PropertyMemberWrapper(levelProp);
                        if (Filter == null || Filter.allowMember(propWrapper))
                        {
                            yield return propWrapper;
                        }
                    }
                    searchType = searchType.BaseType();
                }
            }
        }

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

        /// <summary>
        /// This should be set to a MemberScannerFilter to further filter the
        /// results or null to skip this extra check.
        /// </summary>
        public MemberScannerFilter Filter { get; set; }
    }
}
