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
    public static class TypeShim
    {
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static Assembly Assembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        public static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static bool IsSubclassOf(this Type type, Type parent)
        {
            return type.GetTypeInfo().IsSubclassOf(parent);
        }

        public static bool IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            if (interfaceType.IsGenericType())
            {
                interfaceType = interfaceType.GetGenericTypeDefinition();
            }
            return type.GetTypeInfo().ImplementedInterfaces.Any(i => 
            {
                Type checkType = i;
                if (i.IsGenericType())
                {
                    checkType = i.GetGenericTypeDefinition();
                }
                return interfaceType == checkType;
            });
        }

        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, Type attrType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attrType, inherit);
        }

        public static FieldInfo[] GetFields(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetFields(flags);
        }

        public static Type GetInterface(this Type type, String name)
        {
            return type.GetTypeInfo().GetInterface(name);
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments();
        }

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetProperties(flags);
        }

#if ENABLE_LEGACY_SHIMS
        public static ConstructorInfo GetConstructor(this Type type, BindingFlags bindingAttr, UnusedParameter binder, Type[] types, UnusedParameter modifiers)
        {
            throw new NotImplementedException();
            //foreach(var constructor in type.GetTypeInfo().DeclaredConstructors)
            //{
            //    if (matches(constructor.IsPublic, bindingAttr, BindingFlags.Public)
            //        && matches(constructor.IsPrivate, bindingAttr, BindingFlags.NonPublic) 
            //        && matches(constructor.IsStatic, bindingAttr, BindingFlags.Static)
            //        && matchesParameters(constructor.GetParameters(), types))
            //    {
            //        return constructor;
            //    }
            //}
            //return null;
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

        public static IEnumerable<FieldInfo> GetFields(this Type type, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();

            //foreach(var field in type.GetTypeInfo().DeclaredFields)
            //{
            //    if (matches(field.IsPublic, bindingAttr, BindingFlags.Public)
            //        && matches(field.IsPrivate, bindingAttr, BindingFlags.NonPublic)
            //        && matches(field., bindingAttr, BindingFlags.Instance)
            //        && matches(field.IsStatic, bindingAttr, BindingFlags.Static))
            //    {
            //        yield return field;
            //    }
            //}
        }

        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit);
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type, BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetGenericArguments();
        }

        public static bool IsAssignableFrom(this Type type, Type from)
        {
            return type.GetTypeInfo().IsAssignableFrom(from.GetTypeInfo());
        }

        private static bool inverseMatch(bool sourceCondition, BindingFlags searchFlags, BindingFlags matchFlag)
        {
            if((searchFlags & matchFlag) != 0)
            {
                return sourceCondition;
            }
            else
            {
                return !sourceCondition;
            }
        }

        private static bool forwardMatch(bool sourceCondition, BindingFlags searchFlags, BindingFlags matchFlag)
        {
            if ((searchFlags & matchFlag) != 0)
            {
                return sourceCondition;
            }
            return false;
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