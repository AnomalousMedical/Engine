using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// This class can copy IDictionary classes.
    /// </summary>
    /// <typeparam name="Key">The key type.</typeparam>
    /// <typeparam name="Value">The value type.</typeparam>
    class IDictionaryMemberCopier<Key, Value> : MemberCopier
    {
        /// <summary>
        /// Copy source into destination.
        /// </summary>
        /// <param name="source">The source dictionary.</param>
        /// <param name="destination">The destination dictionary.</param>
        /// <param name="info">The member wrapper to set data.</param>
        internal override void copyValue(object source, object destination, MemberWrapper info, CopyFilter filter)
        {
            IDictionary<Key, Value> sourceDictionary = (IDictionary<Key, Value>)info.getValue(source, null);
            if (sourceDictionary != null)
            {
                Type dictionaryType = sourceDictionary.GetType();
                IDictionary<Key, Value> destDictionary = (IDictionary<Key, Value>)System.Activator.CreateInstance(dictionaryType);
                Type[] keyValueTypes = dictionaryType.GetGenericArguments();
                MemberCopier keyCopier = MemberCopier.getCopyClass(keyValueTypes[0]);
                MemberCopier valueCopier = MemberCopier.getCopyClass(keyValueTypes[1]);
                foreach (Key key in sourceDictionary.Keys)
                {
                    Key copiedKey = (Key)keyCopier.createCopy(key, filter);
                    Value copiedValue = (Value)valueCopier.createCopy(sourceDictionary[key], filter);
                    destDictionary.Add(copiedKey, copiedValue);
                }
                info.setValue(destination, destDictionary, null);
            }
        }

        /// <summary>
        /// Create a new dictionary that is an exact copy of source.
        /// </summary>
        /// <param name="source">The dictionary to clone.</param>
        /// <returns>A new dictionary that is an exact copy of source.</returns>
        internal override object createCopy(object source, CopyFilter filter)
        {
            IDictionary<Key, Value> sourceDictionary = (IDictionary<Key, Value>)source;
            Type dictionaryType = sourceDictionary.GetType();
            IDictionary<Key, Value> destDictionary = (IDictionary<Key, Value>)System.Activator.CreateInstance(dictionaryType);
            Type[] keyValueTypes = dictionaryType.GetGenericArguments();
            MemberCopier keyCopier = MemberCopier.getCopyClass(keyValueTypes[0]);
            MemberCopier valueCopier = MemberCopier.getCopyClass(keyValueTypes[1]);
            foreach (Key key in sourceDictionary.Keys)
            {
                Key copiedKey = (Key)keyCopier.createCopy(key, filter);
                Value copiedValue = (Value)valueCopier.createCopy(sourceDictionary[key], filter);
                destDictionary.Add(copiedKey, copiedValue);
            }
            return destDictionary;
        }
    }
}
