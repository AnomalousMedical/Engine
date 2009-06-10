using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using PhysXWrapper;
using Engine.Saving;

namespace PhysXPlugin
{
    class PhysActorGroupPairDefinition : EditableProperty, Saveable
    {
        private const int GROUP_0_COL = 0;
        private const int GROUP_1_COL = 1;
        private const int FLAG_COL = 2;
        private const int NUM_COLS = 3;

        private ushort group0;
        private ushort group1;
        private ContactPairFlag flags;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhysActorGroupPairDefinition()
        {

        }

        /// <summary>
        /// Constructor takes a name and a subScene.
        /// </summary>
        public PhysActorGroupPairDefinition(ushort group0, ushort group1, ContactPairFlag flags)
        {
            this.group0 = group0;
            this.group1 = group1;
            this.flags = flags;
        }

        public ushort Group0
        {
            get
            {
                return group0;
            }
            set
            {
                group0 = value;
            }
        }

        public ushort Group1
        {
            get
            {
                return group1;
            }
            set
            {
                group1 = value;
            }
        }

        public ContactPairFlag Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        #region EditableProperty Members

        /// <summary>
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        public String getValue(int column)
        {
            switch (column)
            {
                case GROUP_0_COL:
                    return Group0.ToString();
                case GROUP_1_COL:
                    return Group1.ToString();
                case FLAG_COL:
                    return Flags.ToString();
            }
            return null;
        }

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void setValueStr(int column, string value)
        {
            switch (column)
            {
                case GROUP_0_COL:
                    ushort.TryParse(value, out group0);
                    break;
                case GROUP_1_COL:
                    ushort.TryParse(value, out group1);
                    break;
                case FLAG_COL:
                    flags = (ContactPairFlag)Enum.Parse(typeof(ContactPairFlag), value);
                    break;
            }
        }

        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value">The value to try to parse.</param>
        /// <param name="errorMessage">An error message if the function returns false.</param>
        /// <returns>True if the string can be parsed.</returns>
        public bool canParseString(int column, string value, out string errorMessage)
        {
            ushort test;
            switch (column)
            {
                case GROUP_0_COL:
                    if (!ushort.TryParse(value, out test))
                    {
                        errorMessage = "Cannot parse a ushort from this value.";
                        return false;
                    }
                    break;
                case GROUP_1_COL:
                    if (!ushort.TryParse(value, out test))
                    {
                        errorMessage = "Cannot parse a ushort from this value.";
                        return false;
                    }
                    break;
                case FLAG_COL:
                    try 
                    {
                        Enum.Parse(typeof(ContactPairFlag), value);
                    }
                    catch
                    {
                        errorMessage = "Cannot parse a ContactPairFlag from this value.";
                        return false;
                    }
                    break;
            }
            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <param name="column"></param>
        /// <returns>The Type of the object this property will set.</returns>
        public Type getPropertyType(int column)
        {
            switch (column)
            {
                case GROUP_0_COL:
                    return typeof(ushort);
                case GROUP_1_COL:
                    return typeof(ushort);
                case FLAG_COL:
                    return typeof(ContactPairFlag);
            }
            return null;
        }

        #endregion

        #region Saveable Members

        private const String GROUP_0 = "Group0";
        private const String GROUP_1 = "Group1";
        private const String FLAGS = "Flags";

        private PhysActorGroupPairDefinition(LoadInfo info)
        {
            group0 = info.GetUInt16(GROUP_0);
            group1 = info.GetUInt16(GROUP_1);
            flags = info.GetValue<ContactPairFlag>(FLAGS);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(GROUP_0, group0);
            info.AddValue(GROUP_1, group1);
            info.AddValue(FLAGS, flags);
        }

        #endregion
    }
}
