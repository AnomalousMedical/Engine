using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#if ENABLE_LEGACY_SHIMS
using Engine.MissingUtil;
#endif

namespace System
{
    public static class TypeExtensions
    {
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static Assembly Assembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

#if ENABLE_LEGACY_SHIMS
        public static ConstructorInfo GetConstructor(this Type type, BindingFlags bindingAttr, UnusedParameter binder, Type[] types, UnusedParameter modifiers)
        {
            foreach(var constructor in type.GetTypeInfo().DeclaredConstructors)
            {
                if(matchesVisibility(constructor.IsPublic, bindingAttr) 
                    && matchesStatic(constructor.IsStatic, bindingAttr) 
                    && matchesParameters(constructor.GetParameters(), types))
                {
                    return constructor;
                }
            }
            return null;
        }

        public static Type GetInterface(this Type type, String name)
        {
            foreach(var face in type.GetTypeInfo().ImplementedInterfaces)
            {
                if(face.Name == name || face.FullName == name)
                {
                    return face;
                }
            }
            return null;
        }

        private static bool matchesVisibility(bool isPublic, BindingFlags bindingAttr)
        {
            return (isPublic && (bindingAttr & BindingFlags.Public) != 0)
            || (!isPublic && (bindingAttr & BindingFlags.NonPublic) != 0);
        }

        private static bool matchesStatic(bool isStatic, BindingFlags bindingAttr)
        {
            return (isStatic && (bindingAttr & BindingFlags.Static) != 0)
            || (!isStatic && (bindingAttr & BindingFlags.Instance) != 0);
        }

        private static bool matchesParameters(ParameterInfo[] parameters, Type[] searchTypes)
        {
            if (searchTypes == null && parameters.Length == 0)
            {
                return true;
            }

            bool matches = parameters.Length == searchTypes.Length;
            for (int i = parameters.Length - 1; i >= 0 && matches; --i)
            {
                if (parameters[i].ParameterType != searchTypes[i])
                {
                    matches = false;
                }
            }
            return matches;
        }
#endif
    }
}

#if ENABLE_LEGACY_SHIMS
namespace System.Reflection
{
    // Summary:
    //     Specifies flags that control binding and the way in which the search for
    //     members and types is conducted by reflection.
    [Flags]
    public enum BindingFlags
    {
        // Summary:
        //     Specifies no binding flag.
        Default = 0,
        //
        // Summary:
        //     Specifies that the case of the member name should not be considered when
        //     binding.
        IgnoreCase = 1,
        //
        // Summary:
        //     Specifies that only members declared at the level of the supplied type's
        //     hierarchy should be considered. Inherited members are not considered.
        DeclaredOnly = 2,
        //
        // Summary:
        //     Specifies that instance members are to be included in the search.
        Instance = 4,
        //
        // Summary:
        //     Specifies that static members are to be included in the search.
        Static = 8,
        //
        // Summary:
        //     Specifies that public members are to be included in the search.
        Public = 16,
        //
        // Summary:
        //     Specifies that non-public members are to be included in the search.
        NonPublic = 32,
    }
}
#endif