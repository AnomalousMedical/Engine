using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This class will provide an EditInterface to a collection of objects.
    /// These objects can be placed in any type of collection as this class does
    /// not direcly handle the adding/removing of objects, but instead defers
    /// these operations to CreateEditablePropertyCommands and
    /// DestroyEditablePropertyCommands.
    /// </summary>
    /// <typeparam name="T">The type in the collection this interface wraps. Not the type of the collection itself.</typeparam>
    class ReflectedCollectionEditInterface<T>
    {
        private static MemberScanner sharedScanner = new MemberScanner(new EditableAttributeFilter());

        #region Delegates

        /// <summary>
        /// This delegate can be used to implement a custom validate function.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>True if the data is valid, false if it is invalid.</returns>
        public delegate bool Validate(out String errorMessage);

        #endregion Delegates

        #region Fields

        private Dictionary<T, EditableProperty> items = new Dictionary<T, EditableProperty>();
        private MemberScanner memberScanner;
        private String name;
        private EditablePropertyInfo propertyInfo = new EditablePropertyInfo();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">A name for this interface.</param>
        /// <param name="collection">The collection that has the elements this interface will hold.</param>
        public ReflectedCollectionEditInterface(String name, IEnumerable<T> collection)
            : this(name, collection, sharedScanner)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">A name for this interface.</param>
        /// <param name="collection">The collection that has the elements this interface will hold.</param>
        /// <param name="scanner">A MemberScanner to discover the properties of T with.</param>
        public ReflectedCollectionEditInterface(String name, IEnumerable<T> collection, MemberScanner scanner)
        {
            this.memberScanner = scanner;
            this.name = name;
            LinkedList<MemberWrapper> matches = memberScanner.getMatchingMembers(typeof(T));
            foreach (MemberWrapper wrapper in matches)
            {
                if (ReflectedVariable.canCreateVariable(wrapper.getWrappedType()))
                {
                    propertyInfo.addColumn(new EditablePropertyColumn(wrapper.getWrappedName(), false));
                }
            }
            foreach (T item in collection)
            {
                addItem(item);
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add an item to the collection. This will scan the item with a
        /// ReflectedObjectEditableProperty and return that property. It does
        /// not actually add the given item to the wrapped collection that must
        /// be done by the caller.
        /// </summary>
        /// <param name="item">The item to add an property for.</param>
        /// <returns>A new ReflectedObjectEditableProperty for item.</returns>
        public ReflectedObjectEditableProperty addItem(T item)
        {
            ReflectedObjectEditableProperty prop = new ReflectedObjectEditableProperty(item, memberScanner);
            items.Add(item, prop);
            return prop;
        }

        /// <summary>
        /// Removes an item's property from the collection. Note this does not
        /// actually remove the item from the wrapped collection that is up to
        /// the caller.
        /// </summary>
        /// <param name="item">The item to remove the property for.</param>
        public void removeItem(T item)
        {
            items.Remove(item);
        }

        /// <summary>
        /// This function will validate the data in the EditInterface and return
        /// true if it is valid. It will also fill out errorMessage with any
        /// errors that may occur.
        /// </summary>
        /// <param name="errorMessage">A string that will get an error message for the interface.</param>
        /// <returns>True if the settings are valid, false if they are not.</returns>
        public bool validate(out String errorMessage)
        {
            if (ValidateFunction != null)
            {
                return ValidateFunction.Invoke(out errorMessage);
            }
            errorMessage = null;
            return true;
        }

        #endregion

        #region Properties

        public Validate ValidateFunction { get; set; }

        #endregion Properties
    }
}
